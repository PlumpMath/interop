using Common.Services.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using Common.Services.Utils;

namespace Common.Services
{
	public class DynamicHost
	{
		private readonly string _contractAssemblyPath;
		private readonly ServiceEndpointConfiguration _endpointConfig;
		// private const string SvcImplNamespace = "Aamva.Avattar.DynamicServices";
		private ServiceHost _serviceHost;
		public string MetaDataUrl { get; private set; }
		private string _tempFolder;
		private readonly Dictionary<Type, string> _contractWsdlUrls = new Dictionary<Type, string>();

		public DynamicHost(string contractAssemblyPath, ServiceEndpointConfiguration endpointConfig)
		{
			_contractAssemblyPath = contractAssemblyPath;
			_endpointConfig = endpointConfig;
		}

		public string CreateDummyHost(Type contractType, string defaultNamespace, string tempFolder)
		{
			if (!contractType.IsInterface)
				throw new ArgumentException("The generic type passed in must be an interface");
			contractType.Log("Proxy Contract");
			// 1. generate svc impl code 
			this._tempFolder = TempPathUtil.GetTempFolder(tempFolder);
			string serviceImplTypeFullName;
			string svcCodeFilePath = CreateServiceImplCodeFile(contractType, defaultNamespace, out serviceImplTypeFullName);
			// 2. generate svc assembly 
			string contractTypeName = contractType.Name;
			Type svcType = GenerateDynamicHostAssembly(
				this._contractAssemblyPath,
 				serviceImplTypeFullName,
				contractTypeName,
				svcCodeFilePath);
			// 3. host service 
			string metaDataUrl = CreateServiceHost(svcType, this._endpointConfig);
			return metaDataUrl;
		}

		public void GetWsdlFile(string outputFolder)
		{
			if(_serviceHost==null)
				throw new Exception("Service Host is not created");
			if(_serviceHost.State!= CommunicationState.Opened)
				throw new Exception("Service host is not opened");
			if (string.IsNullOrEmpty(this.MetaDataUrl))
				throw new Exception("Mex url is not specified");

			SvcUtil.DownloadWsdlFile(this.MetaDataUrl, outputFolder);
		}

		#region private methods
		private string CreateServiceImplCodeFile(Type contractType, string implNamespace, out string serviceImplTypeFullName)
		{
			// string tmpFolderPath = TempPathUtil.GetTempFolder("svc");
			string serviceImplClassName = contractType.Name + "_Impl";
			serviceImplTypeFullName = string.Format("{0}.{1}", implNamespace, serviceImplClassName);
			string serviceImplCodeFilePath = Path.Combine(this._tempFolder, serviceImplClassName + ".cs");
			ServiceImplSkeletonTemplate template = new ServiceImplSkeletonTemplate(
				implNamespace, contractType, serviceImplClassName);
			string serviceImplCode = template.TransformText();
			if (File.Exists(serviceImplCodeFilePath)) File.Delete(serviceImplCodeFilePath);
			File.WriteAllText(serviceImplCodeFilePath, serviceImplCode);
			return serviceImplCodeFilePath;
		}

		private Type GenerateDynamicHostAssembly(
			string contractAssemblyPath,
			string serviceTypeFullName,
			string contractTypeName,
			string serviceImplCodeFilePath)
		{
			List<string> referencedAssembliePaths1 = new List<string>();
			Assembly originalContractAssembly = Assembly.LoadFrom(contractAssemblyPath);
			ReflectionUtil.DiscoverDependency(originalContractAssembly, referencedAssembliePaths1);

			List<string> referencedAssembliePaths2 = new List<string>();
			foreach (var srcDllPath in referencedAssembliePaths1)
			{
				string tgtAssemblyFilePath = Path.Combine(this._tempFolder, Path.GetFileName(srcDllPath));
				referencedAssembliePaths2.Add(tgtAssemblyFilePath);
				File.Copy(srcDllPath, tgtAssemblyFilePath, true);
			}
			
			string dllFilePath = Path.ChangeExtension(serviceImplCodeFilePath, ".dll");
			string dllFileName = Path.GetFileName(dllFilePath);
			string svcCode = File.ReadAllText(serviceImplCodeFilePath);
			List<string> errors = new List<string>();
			
			// add Common.Services.dll
			string commonServiceAssemblyFilePath = typeof (NotImplementedError).Assembly.Location;
			if (!string.IsNullOrEmpty(commonServiceAssemblyFilePath))
			{
				referencedAssembliePaths2.Add(commonServiceAssemblyFilePath);
			}
			// add serilog
			string serilogAssemblyFilePath = typeof(Serilog.ILogger).Assembly.Location;
			if(!string.IsNullOrEmpty(serilogAssemblyFilePath))
			{
				referencedAssembliePaths2.Add(serilogAssemblyFilePath);
			}

			bool isCompiled = CodeCompilerHelper.CompileCodeToDisk(
				referencedAssembliePaths2,
				new List<string>(),
				this._tempFolder, dllFileName, errors, true, svcCode);
			if (isCompiled)
			{
				Assembly svcAssembly = Assembly.LoadFrom(dllFilePath);
				Type svcImplType = svcAssembly.GetTypes().FirstOrDefault(t =>
					t.FullName == serviceTypeFullName && t.GetInterface(contractTypeName) != null);
				if(svcImplType!=null)
				{
					Type contractType = svcImplType.GetInterface(contractTypeName);
					LogFactory.GetLog().Information("Service type:: {0}, contract type: {1}", svcImplType.FullName, contractType.FullName);
					contractType.Log("Service Impl Contract");
				}
				
				return svcImplType;
			}
			else
				throw new Exception("Unable to compile service assembly code: " + errors.First());
		}

		private string CreateServiceHost(
			Type serviceType, 
			ServiceEndpointConfiguration endPointConfig)
		{
			_serviceHost = new ServiceHost(serviceType);
			Binding binding = endPointConfig.InitBinding();
			if (binding == null)
			{
				throw new NotSupportedException("Binding is not supported");
			}
			var contractTypes =
				serviceType.GetInterfaces().Where(t => t.GetCustomAttribute<ServiceContractAttribute>() != null).ToList();
			foreach (Type contractType in contractTypes)
			{
				var svcEndpoint = _serviceHost.AddServiceEndpoint(contractType, binding, endPointConfig.ServiceAddress);
				//svcEndpoint.EndpointBehaviors.Add(new WsdlExtensions(new WsdlExtensionsConfig()
				//{
				//	Location = new Uri(endPointConfig.ServiceAddress),
				//	SingleFile = true
				//}));
				_contractWsdlUrls.Add(contractType, endPointConfig.ServiceAddress);
			}
			// this.ServiceUrl = endPointConfig.ServiceAddress;
			this.MetaDataUrl = endPointConfig.ServiceAddress + "/mex";
			this._serviceHost.ApplyMexBehavior(this.MetaDataUrl);
			this._serviceHost.ApplyDebugBehavior();
			this._serviceHost.ApplyInstanceModeBehavior(InstanceContextMode.Single);
			this._serviceHost.ApplyMessageLogBehavior();

			ServiceDebugBehavior debugBehavior =
				_serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();
			if (debugBehavior == null)
			{
				debugBehavior = new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true };
				_serviceHost.Description.Behaviors.Add(debugBehavior);
			}
			else
				debugBehavior.IncludeExceptionDetailInFaults = true;

			_serviceHost.Open();

			return MetaDataUrl;
		}

		private void ApplySecurity(
			Binding binding, ServiceSecurityModes securityMode,
			ClientCredentialTypes credentialType)
		{
			#region BasicHttpBinding
			BasicHttpBinding basicHttpBinding = binding as BasicHttpBinding;
			if (basicHttpBinding != null)
			{
				switch (securityMode)
				{
					case ServiceSecurityModes.None:
						basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;
						break;
					case ServiceSecurityModes.Transport:
						basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Transport;
						break;
					case ServiceSecurityModes.Message:
						basicHttpBinding.Security.Mode = BasicHttpSecurityMode.Message;
						break;
					case ServiceSecurityModes.TransportWithMessaegCredential:
						basicHttpBinding.Security.Mode = BasicHttpSecurityMode.TransportWithMessageCredential;
						break;
					case ServiceSecurityModes.TransportCredentialOnly:
						basicHttpBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
						break;
					default:
						basicHttpBinding.Security.Mode = BasicHttpSecurityMode.None;
						break;
				}

				switch (credentialType)
				{
					case ClientCredentialTypes.UserName:
						basicHttpBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
						break;
					case ClientCredentialTypes.Certificate:
						basicHttpBinding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.Certificate;
						break;
				}
				return;
			}
			#endregion

			#region NetTcpBinding
			NetTcpBinding netTcpBinding = binding as NetTcpBinding;
			if (netTcpBinding != null)
			{
				switch (securityMode)
				{
					case ServiceSecurityModes.None:
						netTcpBinding.Security.Mode = SecurityMode.None;
						break;
					case ServiceSecurityModes.Transport:
						netTcpBinding.Security.Mode = SecurityMode.Transport;
						break;
					case ServiceSecurityModes.Message:
						netTcpBinding.Security.Mode = SecurityMode.Message;
						break;
					case ServiceSecurityModes.TransportWithMessaegCredential:
						netTcpBinding.Security.Mode = SecurityMode.TransportWithMessageCredential;
						break;
					default:
						netTcpBinding.Security.Mode = SecurityMode.None;
						break;
				}


				switch (credentialType)
				{
					case ClientCredentialTypes.UserName:
						netTcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
						break;
					case ClientCredentialTypes.Certificate:
						netTcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
						break;
					case ClientCredentialTypes.Windows:
						netTcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
						break;
					case ClientCredentialTypes.IssuedToken:
						netTcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.IssuedToken;
						break;
					default:
						netTcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.None;
						break;
				}
				return;
			}
			#endregion

			#region WSHttpBinding
			WSHttpBinding wsHttpBinding = binding as WSHttpBinding;
			if (wsHttpBinding != null)
			{
				switch (securityMode)
				{
					case ServiceSecurityModes.None:
						wsHttpBinding.Security.Mode = SecurityMode.None;
						break;
					case ServiceSecurityModes.Transport:
						wsHttpBinding.Security.Mode = SecurityMode.Transport;
						break;
					case ServiceSecurityModes.Message:
						wsHttpBinding.Security.Mode = SecurityMode.Message;
						break;
					case ServiceSecurityModes.TransportWithMessaegCredential:
						wsHttpBinding.Security.Mode = SecurityMode.TransportWithMessageCredential;
						break;
					default:
						wsHttpBinding.Security.Mode = SecurityMode.None;
						break;
				}

				switch (credentialType)
				{
					case ClientCredentialTypes.UserName:
						wsHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
						break;
					case ClientCredentialTypes.Certificate:
						wsHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Certificate;
						break;
					case ClientCredentialTypes.Windows:
						wsHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
						break;
					case ClientCredentialTypes.IssuedToken:
						wsHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.IssuedToken;
						break;
					default:
						wsHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.None;
						break;
				}
				return;
			}
			#endregion
		} 
		#endregion


	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using Common.Services.Templates;
using Common.Services.Tests.Models;
using Common.Services.Utils;
using Common.Services.Wrappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace Common.Services.Tests.Steps
{
	[Binding]
	public class HostWsdlServiceSteps
	{
		// private const string dynamicHostNamespace = "Aamva.Avattar.DynamicHost";
		[Given(@"set serialization method to '(.*)'")]
		public void GivenSetSerializationMethodTo(string serializationMethod)
		{
			bool useXml = serializationMethod == "Xml";
			ScenarioContext.Current.Set(useXml, "UseXml");
		}

		[Given(@"set rehost namespace to '(.*)'")]
		public void GivenSetRehostNamespaceTo(string rehostNamespace)
		{
			ScenarioContext.Current.Set(rehostNamespace, "RehostNamespace");
		}

		[When(@"I generate wsdl assembly and save them to folder '(.*)' with name '(.*)' using SvcUtil")]
		public void WhenIGenerateWsdlAssemblyAndSaveThemToFolderWithName(string targetFolder, string wsdlAssemblyFileName)
		{
			string metaDataUrl = ScenarioContext.Current.Get<string>("MetaDataUrl");
			Assert.IsFalse(string.IsNullOrEmpty(metaDataUrl),"Metadata Url is not set");
			bool useXml = ScenarioContext.Current.Get<bool>("UseXml");
			ScenarioContext.Current.Set(false, "UseMetadataSet");
			string rehostNamespace = ScenarioContext.Current.Get<string>("RehostNamespace");

			targetFolder = TempPathUtil.GetTempFolder(targetFolder);
			string wsdlAssemblyFilePath = Path.Combine(targetFolder, wsdlAssemblyFileName);
			if (File.Exists(wsdlAssemblyFilePath))
				File.Delete(wsdlAssemblyFilePath);
			string assemblyFilePath = Wsdl2Assembly.GetWsdlAssemblyUsingSvcUtil(metaDataUrl, rehostNamespace, useXml);
			File.Copy(assemblyFilePath, wsdlAssemblyFilePath);
			ScenarioContext.Current.Set(wsdlAssemblyFilePath, "WsdlAssemblyFilePath");
		}

		[When(@"I generate wsdl assembly using method MetadataSet")]
		public void WhenIGenerateWsdlAssemblyUsingMethodMetadataSet()
		{
			string metaDataUrl = ScenarioContext.Current.Get<string>("MetaDataUrl");
			Assert.IsFalse(string.IsNullOrEmpty(metaDataUrl), "Metadata Url is not set");
			ScenarioContext.Current.Set(true, "UseMetadataSet");

			Assembly wsdlAssembly = Wsdl2Assembly.GetWsdlAssemblyUsingMetadataSet(metaDataUrl);
			Assert.IsNotNull(wsdlAssembly, "Unable to generate wsdl assembly using metadataset");
			ScenarioContext.Current.Set(wsdlAssembly, "WsdlAssembly");
		}

		[When(@"I rehost services using generated wsdl assembly")]
		public void WhenIHostServicesThatImplementsAllContractsWithinWsdlFileOnWithBinding(Table table)
		{
			ServiceEndpointConfiguration config = table.GetServiceEndPointConfiguration();
			ScenarioContext.Current.Set(config,"RehostConfig");

			#region get wsdl assembly
			bool useMetadataSet = ScenarioContext.Current.Get<bool>("UseMetadataSet");
			Assembly wsdlAssembly = null;
			if (useMetadataSet)
			{
				wsdlAssembly = ScenarioContext.Current.Get<Assembly>("WsdlAssembly");
			}
			else
			{
				string wsdlAssemblyFilePath = ScenarioContext.Current.Get<string>("WsdlAssemblyFilePath");
				Assert.IsFalse(string.IsNullOrEmpty(wsdlAssemblyFilePath));
				Assert.IsTrue(File.Exists(wsdlAssemblyFilePath));
				wsdlAssembly = Assembly.LoadFile(wsdlAssemblyFilePath);
			} 
			Assert.IsNotNull(wsdlAssembly, "Unable to get wsdl assembly");
			#endregion

			#region check contract type
			Type contractType = ScenarioContext.Current.Get<Type>("ContractType");
			Assert.IsNotNull(contractType, "Contract type is not set");
			Type contractType2 = wsdlAssembly.GetTypes().FirstOrDefault(t => t.Name == contractType.Name);
			Assert.IsNotNull(contractType2, "Unable to get contract type from wsdl assembly");
			
			ServiceContractAttribute svcContractAttr = contractType.GetCustomAttributes<ServiceContractAttribute>().FirstOrDefault();
			Assert.IsNotNull(svcContractAttr, "Contract type must have service contract attribute");
			string contractTypeFullName =
				svcContractAttr.Namespace + "." +
				(string.IsNullOrEmpty(svcContractAttr.Name) ? contractType.Name : svcContractAttr.Name);
			if (contractTypeFullName != contractType2.FullName)
			{
				Trace.TraceWarning("Expected: {0}, Actual: {1}. contract namespace do not match", contractTypeFullName, contractType2.FullName);
			} 
			#endregion

			if (useMetadataSet)
			{
				try
				{
					var dynamicServiceHost = new DynamicServiceHost(contractType2);
					LogFactory.GetLog().Information("Contract type: {0}", contractType2.FullName);
					GenericServiceObject genericServiceObj = new GenericServiceObject(contractType2);
					var serviceObj = new DynamicWrapper().CreateWrapper(contractType2, genericServiceObj,
						genericServiceObj.MethodMappings);
					dynamicServiceHost.CreateAndStartService(serviceObj, config);
					ScenarioContext.Current.Set(dynamicServiceHost);
				}
				catch (Exception ex)
				{
					LogFactory.GetLog().Error(ex, "Unable to create service host");
					LogFactory.GetLog().Information(ex.Message);
					Assert.Fail(ex.Message);
				}
			}
			else
			{
				string wsdlAssemblyFilePath = ScenarioContext.Current.Get<string>("WsdlAssemblyFilePath");
				string defaultNamespace = ScenarioContext.Current.Get<string>("DefaultNamespace");
				DynamicHost dynamicHost = new DynamicHost(wsdlAssemblyFilePath, config);
				try
				{
					dynamicHost.CreateDummyHost(contractType2, defaultNamespace, "svc-gen");
					string folder = TempPathUtil.GetTempFolder(string.Format(@"wsdl\rehost\{0}", contractType.Name));
					dynamicHost.GetWsdlFile(folder);
					var wsdlFilesGenerated = Directory.GetFiles(folder, "*.wsdl").ToList();
					Assert.IsNotNull(wsdlFilesGenerated);
					Assert.IsTrue(wsdlFilesGenerated.Count > 0, "Unable to download wsdl file");

					ScenarioContext.Current.Set(config.ServiceAddress, "RehostServiceAddress");
					ScenarioContext.Current.Set(dynamicHost);
				}
				catch (Exception ex)
				{
					if (ex is ReflectionTypeLoadException)
					{
						var typeLoadException = ex as ReflectionTypeLoadException;
						var loaderExceptions = typeLoadException.LoaderExceptions;
						foreach (var loadEx in loaderExceptions)
						{
							LogFactory.GetLog().Error(loadEx, "Loader exception");
						}
					}

					LogFactory.GetLog().Error(ex, "Unable to create service host");
					Assert.Fail(ex.Message);
				}
			}
		}

		[When(@"download metadata from rehost metadata url to folder '(.*)'")]
		public void WhenDownloadMetadataFromRehostMetadataUrlToFolder(string targetMetadataFolder)
		{
			bool useMetadataSet = ScenarioContext.Current.Get<bool>("UseMetadataSet");
			string metaDataUrl = null;
			if(useMetadataSet)
			{
				DynamicServiceHost svcHost = ScenarioContext.Current.Get<DynamicServiceHost>();
				Assert.IsNotNull(svcHost);
				metaDataUrl = svcHost.MetaDataUrl;
			}
			else
			{
				DynamicHost dynamicHost = ScenarioContext.Current.Get<DynamicHost>();
				Assert.IsNotNull(dynamicHost);
				metaDataUrl = dynamicHost.MetaDataUrl;
			}
			targetMetadataFolder = TempPathUtil.GetTempFolder(targetMetadataFolder);
			SvcUtil.DownloadWsdlFile(metaDataUrl, targetMetadataFolder);
			var schemaFiles = Directory.GetFiles(targetMetadataFolder, "*.wsdl");
			Assert.IsTrue(schemaFiles.Length > 0);
		}


		[When(@"call service using channel factory and the same contract interface")]
		public void WhenCallServiceUsingChannelFactoryAndTheSameContractInterface()
		{
			Type clientContractType = ScenarioContext.Current.Get<Type>("ContractType");
			Assert.IsNotNull(clientContractType,"Contract type is not specified");
			ServiceEndpointConfiguration config = ScenarioContext.Current.Get<ServiceEndpointConfiguration>("RehostConfig");
			

			var proxy = DynamicProxy.Create(clientContractType, config);
			var method = clientContractType.GetMethods().First();
			List<object> methodArgs = new List<object>();
			var methodParams = method.GetParameters();
			foreach (var methodParam in methodParams)
			{
				methodArgs.Add(null);
			}
			try
			{
				object returnObj = method.Invoke(proxy, methodArgs.ToArray());
				if (method.ReturnType != typeof (void))
				{
					ScenarioContext.Current.Set(returnObj.GetType().Name, "MethodInvokeResult");
				}
				else
				{
					ScenarioContext.Current.Set("Empty", "MethodInvokeResult");
				}
			}
			catch (FaultException<NotImplementedError> error)
			{
				// this is considered pass
				ScenarioContext.Current.Set("Fault", "MethodInvokeResult");
			}
			catch (FaultException faultEx)
			{
				if (faultEx.Message.ToLower().Contains("not implemented"))
				{
					// this is considered pass
					ScenarioContext.Current.Set("Fault", "MethodInvokeResult");
				}
				else
				{
					Assert.Fail(faultEx.Message);
				}
			}
			catch (Exception ex)
			{
				if (ex.InnerException is FaultException)
				{
					if(ex.InnerException.Message.ToLower().Contains("not implemented"))
					{
						// this is considered pass
						ScenarioContext.Current.Set("Fault", "MethodInvokeResult");
					}
					else
					{
						Assert.Fail(ex.Message);
					}
				}
				else
				{
					Assert.Fail(ex.Message);
				}
			}
		}

		[Then(@"I should be able to hit service impl class and return dummy object")]
		public void ThenIShouldBeAbleToHitServiceImplClassAndReturnDummyObject()
		{
			var returnObjTypeName = ScenarioContext.Current.Get<string>("MethodInvokeResult");
			Assert.IsFalse(string.IsNullOrEmpty(returnObjTypeName));
		}
	}
}

using Common.Services;
using Common.Services.Utils;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;

namespace ServiceHosts
{
	public class DynamicHostFromWsdlUrl
	{
		#region props
		public string WsdlUrl { get; private set; }
		public bool UseXml { get; private set; }
		public string DefaultNamespace { get; private set; }
		public ServiceEndpointConfiguration EndpointConfig { get; private set; }
		#endregion

		#region ctor
		public DynamicHostFromWsdlUrl(string wsdlUrl, bool useXml, string defaultNamespace, ServiceEndpointConfiguration endpointConfig)
		{
			EndpointConfig = endpointConfig;
			DefaultNamespace = defaultNamespace;
			UseXml = useXml;
			WsdlUrl = wsdlUrl;
		} 
		#endregion
		
		public void CreateWsdlAssemblyAndHost()
		{
			string assemblyFilePath = Wsdl2Assembly.GetWsdlAssemblyUsingSvcUtil(this.WsdlUrl, this.DefaultNamespace, this.UseXml);
			string targetFolder = TempPathUtil.GetTempFolder(this.GetType().Name);
			string tgtAssemblyFilePath = Path.Combine(targetFolder, Path.GetFileName(assemblyFilePath));
			File.Copy(assemblyFilePath, tgtAssemblyFilePath, true);
			Assembly proxyAssembly = Assembly.LoadFrom(tgtAssemblyFilePath);
			Type contractType = proxyAssembly.GetTypes().First(t => t.IsInterface && t.GetCustomAttribute<ServiceContractAttribute>() != null);

			DynamicHost dynamicHost = new DynamicHost(tgtAssemblyFilePath, this.EndpointConfig);
			dynamicHost.CreateDummyHost(contractType, this.DefaultNamespace, targetFolder);
		}

	}
}

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using Common.Services.Tests.Models;
using Common.Services.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace Common.Services.Tests.Steps
{
	[Binding]
	public class WsdlGeneratorSteps
	{
		[Given(@"a referenced assembly in bin folder called '(.*)'")]
		public void GivenAReferencedAssemblyInBinFolderCalled(string assemblyFileName)
		{
			string binFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string assemblyFilePath = Path.Combine(binFolder, assemblyFileName);
			Assert.IsTrue(File.Exists(assemblyFilePath), "Unable to find assembly from path " + assemblyFilePath);
			Assembly contractAssembly = Assembly.LoadFile(assemblyFilePath);
			ScenarioContext.Current.Set(contractAssembly, "ContractAssembly");
			ScenarioContext.Current.Set(assemblyFilePath, "ContractAssemblyFilePath");
		}

		[Given(@"Pick an interface with name '(.*)'")]
		public void GivenPickAnInterfaceWithName(string interfaceName)
		{
			var contractAssembly = ScenarioContext.Current.Get<Assembly>("ContractAssembly");
			Assert.IsNotNull(contractAssembly, "Unable to get contract assembly");

			Type interfaceType = contractAssembly.GetTypes().FirstOrDefault(t => t.Name == interfaceName);
			Assert.IsNotNull(interfaceType, "Unable to get interface type from assembly");
			Assert.IsTrue(interfaceType.IsInterface, "The type " + interfaceName + " is not interface type");
			Assert.IsTrue(interfaceType.GetCustomAttribute<ServiceContractAttribute>()!=null, "The interface do not have service contract decoration");
			ScenarioContext.Current.Set(interfaceType, "ContractType");
		}

		[Given(@"set generated assembly namespace to ""(.*)""")]
		public void GivenSetGeneratedAssemblyNamespaceTo(string defaultNamespace)
		{
			ScenarioContext.Current.Set(defaultNamespace, "DefaultNamespace");
		}


		[When(@"I generate and host service with the following setting")]
		public void WhenIGenerateAndHostServiceHostOnWithBinding(Table table)
		{
			ServiceEndpointConfiguration config = table.GetServiceEndPointConfiguration();
			var contractAssembly = ScenarioContext.Current.Get<Assembly>("ContractAssembly");
			Assert.IsNotNull(contractAssembly, "Unable to get contract assembly");
			Type contractType = ScenarioContext.Current.Get<Type>("ContractType");
			Assert.IsNotNull(contractType, "Unable to get contract type");
			string contractAssemblyPath = ScenarioContext.Current.Get<string>("ContractAssemblyFilePath");
			string defaultNamespace = ScenarioContext.Current.Get<string>("DefaultNamespace");
			
			DynamicHost dynamicHost = new DynamicHost(contractAssemblyPath, config);
			try
			{
				string metaDataUrl = dynamicHost.CreateDummyHost(contractType, defaultNamespace, "svc");
				Assert.IsNotNull(metaDataUrl);
				ScenarioContext.Current.Set(metaDataUrl, "MetaDataUrl");
				string folder = TempPathUtil.GetTempFolder(string.Format(@"wsdl\{0}", contractType.Name));
				dynamicHost.GetWsdlFile(folder);
				var wsdlFilesGenerated = Directory.GetFiles(folder, "*.wsdl").Where(f=>!f.ToLower().Contains("tempuri")).ToList();
				Assert.IsNotNull(wsdlFilesGenerated);
				Assert.IsTrue(wsdlFilesGenerated.Count==1, "Unable to download wsdl file");
				ScenarioContext.Current.Set(wsdlFilesGenerated[0], "WSDL");
			}
			catch (Exception ex)
			{
				LogFactory.GetLog().Error(ex, "Unable to create service host");
				Assert.Fail(ex.Message);
			}
		}

		[Then(@"I should be able to download wsdl files to folder '(.*)'")]
		public void ThenIShouldBeAbleToDownloadWsdlFileTo(string tgtFolder)
		{
			if (tgtFolder.StartsWith(@"~\"))
				tgtFolder = tgtFolder.Substring(2);
			tgtFolder = TempPathUtil.GetTempFolder(tgtFolder);

			string currentWsdlFile = ScenarioContext.Current.Get<string>("WSDL");
			Assert.IsTrue(File.Exists(currentWsdlFile));
			string srcFolder = Path.GetDirectoryName(currentWsdlFile);
			var allSchemaFiles = Directory.GetFiles(srcFolder);
			Assert.IsTrue(allSchemaFiles.Length>0);
			
			foreach (string srcFile in allSchemaFiles)
			{
				string tgtPath = Path.Combine(tgtFolder, Path.GetFileName(srcFile));
				File.Copy(srcFile, tgtPath,true);
			}
		}
	}

	
}

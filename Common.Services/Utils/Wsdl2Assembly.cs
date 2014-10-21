using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceModel.Description;

namespace Common.Services.Utils
{
	public class Wsdl2Assembly
	{
		public static Assembly GetWsdlAssemblyUsingMetadataSet(string metadataUrl)
		{
			MetadataExchangeClient metaClient =
				new MetadataExchangeClient(new Uri(metadataUrl), MetadataExchangeClientMode.HttpGet)
				{
					MaximumResolvedReferences = 100,
					ResolveMetadataReferences = true
				};
			MetadataSet metadataSet = metaClient.GetMetadata();
			var importer = new WsdlImporter(metadataSet);
			var contracts = importer.ImportAllContracts();
			//var bindings = importer.ImportAllBindings();
			//var endpoints = importer.ImportAllEndpoints();
			var generator = new ServiceContractGenerator();
			foreach (ContractDescription contract in contracts)
			{
				generator.GenerateServiceContractType(contract);
			}
			var options = new CodeGeneratorOptions();
			options.BracingStyle = "C";
			CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("C#");

			// Compile the code file to an in-memory assembly (add all WCF-related assemblies as references)
			var compilerParameters = new CompilerParameters(
				new[]
                    {
                        "System.dll", "System.ServiceModel.dll",
                        "System.Runtime.Serialization.dll"
                    });
			compilerParameters.GenerateInMemory = true;
			CompilerResults results = codeDomProvider.CompileAssemblyFromDom(compilerParameters, generator.TargetCompileUnit);
			if (results.Errors.Count > 0)
			{
				throw new Exception("There were errors during generated code compilation");
			}
			return results.CompiledAssembly;
		}

		public static string GetWsdlAssemblyUsingSvcUtil(string metadataUrl, string wsdlNamespace, bool useXmlSerializer)
		{
			List<string> errors = new List<string>();
			string outputFolderPath = TempPathUtil.GetTempFolder("proxy");
			string contractCodeFilePath = Path.Combine(outputFolderPath, "Contracts.cs");
			string assemblyFilePath = Path.Combine(outputFolderPath, "proxy.dll");
			bool isGenerated = SvcUtil.GenerateAssemblyFromWsdl(
				metadataUrl, wsdlNamespace, 
				outputFolderPath, contractCodeFilePath, assemblyFilePath, 
				useXmlSerializer, true, out errors);
			if (isGenerated)
			{
				return assemblyFilePath;
			}
			foreach (string error in errors)
			{
				Trace.WriteLine(error);
			}
			throw new Exception("Unable to generate assembly from metadata url");
		}
	}
}

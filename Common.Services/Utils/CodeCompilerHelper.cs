using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;

namespace Common.Services.Utils
{
	public class CodeCompilerHelper
	{
		public static Assembly CompileCode(List<Assembly> referencedAssemblies, params string[] csharpCodes)
		{
			CSharpCodeProvider provider = new CSharpCodeProvider();
			CompilerParameters compilerparams =
				new CompilerParameters(new[]
				{
					"System.dll", 
					"System.ServiceModel.dll",
					"System.Runtime.Serialization.dll",
					"System.Xml.dll",
					"System.Xml.Serialization.dll"
				});
			compilerparams.GenerateExecutable = false;
			compilerparams.GenerateInMemory = true;
			Dictionary<string, string> assemblyNameLocations = new Dictionary<string, string>();
			foreach (Assembly assembly in referencedAssemblies)
			{
				try
				{
					string location = assembly.Location;
					AssemblyName assemblyName = assembly.GetName();
					if (!String.IsNullOrEmpty(location) && assemblyName.Name != null)
					{
						if (!assemblyNameLocations.ContainsKey(assemblyName.Name))
						{
							assemblyNameLocations.Add(assemblyName.Name, location);
							compilerparams.ReferencedAssemblies.Add(location);
						}
						else
						{
							Trace.WriteLine(string.Format("Duplicated assembly in app domain: {0}", assemblyName.Name));
						}
					}
				}
				catch (NotSupportedException)
				{
					// this happens for dynamic assemblies, so just ignore it. 
				}
			}
			CompilerResults results =
			   provider.CompileAssemblyFromSource(compilerparams, csharpCodes);
			if (results.Errors.HasErrors)
			{
				StringBuilder errors = new StringBuilder("Compiler Errors :\r\n");
				foreach (CompilerError error in results.Errors)
				{
					errors.AppendFormat("Line {0},{1}\t: {2}\n",
						   error.Line, error.Column, error.ErrorText);
				}
				throw new Exception(errors.ToString());
			}

			AppDomain.CurrentDomain.Load(results.CompiledAssembly.GetName());
			return results.CompiledAssembly;
		}

		public static bool CompileCodeToDisk(
			List<string> referencedAssemblyPaths,
			List<string> resourceFiles,
			string outputFolder,
			string assemblyFileName,
			List<string> errorList,
			bool isDebug,
			params string[] csharpCodes)
		{
			string[] referencedAssemblies = new[]
			{
				"System.dll", 
				"System.ServiceModel.dll",
				"System.Runtime.Serialization.dll",
				"System.Xml.dll",
				"System.Xml.Serialization.dll"
			};

			bool isCompiled = CompileHelper(
				csharpCodes, false, outputFolder,
				assemblyFileName, "CS", errorList,
				referencedAssemblies,
				resourceFiles,
				referencedAssemblyPaths,
				isDebug);
			if (errorList.Count > 0)
			{
				return false;
			}
			return isCompiled;
		}

		private static Boolean CompileHelper(
			string[] sources, Boolean isFromFile,
			string outputFolder,
			string outputFileName, string languageType,
			List<string> errorList,
			string[] referencedAssemblies,
			List<string> embeddedResourcePaths,
			IEnumerable<string> assemblyReferencePaths,
			Boolean isDebug)
		{
			CodeDomProvider provider;

			switch (languageType.Trim().ToUpper())
			{
				case "CS":
					provider = CodeDomProvider.CreateProvider("CSharp");
					break;
				case "VB":
					provider = CodeDomProvider.CreateProvider("VisualBasic");
					break;
				default:
					const string errorMsg = "Not supported language.";
					errorList.Add(errorMsg);
					return false;
			}

			if (provider != null)
			{
				String dllPath = Path.Combine(outputFolder, outputFileName);
				CompilerParameters cp = new CompilerParameters();
				if (referencedAssemblies != null)
					cp = new CompilerParameters(referencedAssemblies);

				cp.GenerateExecutable = false;

				cp.OutputAssembly = dllPath;

				cp.GenerateInMemory = false;

				cp.TreatWarningsAsErrors = false;

				cp.TempFiles.KeepFiles = true;

				cp.IncludeDebugInformation = isDebug;

				// Load embedded resources
				if (embeddedResourcePaths != null && embeddedResourcePaths.Count > 0)
				{
					if (provider.Supports(GeneratorSupport.Resources))
					{
						foreach (String emPath in embeddedResourcePaths)
						{
							AddEmbeddedResource(cp, emPath, errorList);
						}
					}
					else
					{
						const string errorMsg = "Provider does not support embedded resources.";
						errorList.Add(errorMsg);
					}
				}

				// Load referenced assemblies
				string binFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				foreach (String referencePath in assemblyReferencePaths)
				{
					if (File.Exists(referencePath))
					{
						string assemblyFileName = Path.GetFileName(referencePath);
						string copyRefPath = Path.Combine(binFolder, assemblyFileName);
						if (!File.Exists(copyRefPath))
							File.Copy(referencePath, copyRefPath);
					}

					AddReferenceAssembly(cp, referencePath, errorList);
				}

				// Compile sources with option defined above.
				CompilerResults cr;
				if (isFromFile)
				{
					cr = provider.CompileAssemblyFromFile(cp, sources);
				}
				else
				{
					cr = provider.CompileAssemblyFromSource(cp, sources);
				}

				if (cr.Errors.Count > 0)
				{
					foreach (CompilerError ce in cr.Errors)
					{
						String errorStr = String.Format("{0}", ce);
						errorList.Add(errorStr);
					}
				}
			}
			else
			{
				const string errorMsg = "Cannot create CodeDom provider.";
				errorList.Add(errorMsg);
			}

			return errorList.Count == 0;
		}

		private static void AddEmbeddedResource(
			CompilerParameters cp, String path, List<string> errorList)
		{
			if (File.Exists(path))
			{
				cp.EmbeddedResources.Add(path);
			}
			else
			{
				String errorStr = String.Format("File not found at: {0}", path);
				errorList.Add(errorStr);
			}
		}

		private static void AddReferenceAssembly(
			CompilerParameters cp, String dllPath, List<string> errorList)
		{
			if (File.Exists(dllPath))
			{
				cp.ReferencedAssemblies.Add(dllPath);
			}
			else
			{
				String errorMsg = String.Format("File not found at: {0}", dllPath);
				errorList.Add(errorMsg);
			}
		}
	}
}

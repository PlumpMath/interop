using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace Common.Services.Utils
{
	public class SvcUtil
	{
		public static bool GenerateAssemblyFromWsdl(
			string wsdlFilePath, string nameSpace, string outputFolder,
			string codeFilePath, string assemblyFilePath, bool useXmlSerializer,
			bool isDebug, out List<string> errors)
		{
			errors = new List<string>();
			Process svcUtilProcess = new Process();
			if (File.Exists(codeFilePath))
				File.Delete(codeFilePath);
			string exeFilePath = ConfigurationManager.AppSettings.Get("SvcUtilPath");
			string arguments =
				string.Format("\"{0}\" /t:code /serializer:{1} /n:*,{2} /out:\"{3}\" /syncOnly",
				wsdlFilePath,
				(useXmlSerializer ? "XmlSerializer /UseSerializerForFaults" : "DataContractSerializer"),
				nameSpace,
				codeFilePath);

			svcUtilProcess.StartInfo =
				new ProcessStartInfo(exeFilePath, arguments)
				{
					WorkingDirectory = outputFolder
				};

			svcUtilProcess.Start();
			svcUtilProcess.WaitForExit();

			if (File.Exists(codeFilePath))
			{
				string contractCode = File.ReadAllText(codeFilePath);
				bool isCompiled = CodeCompilerHelper.CompileCodeToDisk(
					new List<string>(),
					new List<string>(),
					outputFolder,
					assemblyFilePath,
					errors,
					isDebug,
					contractCode);
				// File.Delete(codeFilePath);
				return isCompiled;
			}
			return false;
		}

		public static void DownloadWsdlFile(string wsdlUrl, string outputFolder)
		{
			string exeFilePath = ConfigurationManager.AppSettings.Get("SvcUtilPath");
			string arguments = string.Format("/t:metadata \"{0}\"", wsdlUrl);
			Process svcUtilProcess = new Process();
			svcUtilProcess.StartInfo =
				new ProcessStartInfo(exeFilePath, arguments)
				{
					WorkingDirectory = outputFolder
				};
			svcUtilProcess.Start();
			svcUtilProcess.WaitForExit();
		}
	}
}

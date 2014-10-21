using System.IO;
using System.Reflection;

namespace Common.Services.Utils
{
	public static class TempPathUtil
	{
		public static string GetTempFolder(string subFolder = null)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			string folder= Path.GetDirectoryName(assembly.Location);
			string tmpFolder=Path.Combine(folder, "tmp");
			if(!Directory.Exists(tmpFolder))
				Directory.CreateDirectory(tmpFolder);
			if(!string.IsNullOrEmpty(subFolder))
			{
				tmpFolder=Path.Combine(tmpFolder, subFolder);
				if (!Directory.Exists(tmpFolder))
					Directory.CreateDirectory(tmpFolder);
			}
			return tmpFolder;
		}
	}
}

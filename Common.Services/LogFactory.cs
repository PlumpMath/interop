using Serilog;

namespace Common.Services
{
	public class LogFactory
	{
		const string customTemplate = "Will be logged {Timestamp:yyyy-MMM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}";

		static LogFactory()
		{
			Log.Logger = new LoggerConfiguration()
				.WriteTo.ColoredConsole()
				.WriteTo.File("logs/log.txt", outputTemplate: customTemplate, fileSizeLimitBytes: null)
				// .WriteTo.MongoDB("mongodb://localhost/logs")
				.CreateLogger();
		}

		public static ILogger GetLog()
		{
			return Log.Logger;
		}
	}
}

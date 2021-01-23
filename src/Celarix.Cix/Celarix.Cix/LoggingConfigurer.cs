using System;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Celarix.Cix.Compiler
{
	public static class LoggingConfigurer
	{
        public static void ConfigureLogging(string minimumLevel)
        {
			// https://blog.elmah.io/nlog-tutorial-the-essential-guide-for-logging-from-csharp/#configuration-in-c-
            var config = new LoggingConfiguration();
            var coloredConsoleTarget = new ColoredConsoleTarget
            {
                Name = "coloredConsole",
                Layout = "${time}|${level:uppercase=true}|${logger}|${message}"
            };
            config.AddRule(LogLevel.FromString(minimumLevel), LogLevel.Fatal, coloredConsoleTarget);
            LogManager.Configuration = config;

            LogManager.GetCurrentClassLogger().Info($"Configured logging with minimum level {minimumLevel}");
        }
	}
}

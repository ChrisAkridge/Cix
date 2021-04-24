using System;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Celarix.Cix.Compiler
{
	public static class LoggingConfigurer
	{
        // Log level usages:
        //
        // Fatal: Used for compiler errors and internal compiler errors
        // Error: Not used as all errors are fatal
        // Warning: Reserved for compiler warnings
        // Info: Standard log level, prints phase information
        // Debug: Used to mark the starts/ends of internal phases
        // Trace: Deep logging for every part of the compilation
        
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

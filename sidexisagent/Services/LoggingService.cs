using System;
using NLog;

namespace AutomatedAgent.Services
{
    public class LoggingService
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // Log an info message
        public void LogInfo(string message)
        {
            Logger.Info(message);
        }

        // Log a warning message
        public void LogWarning(string message)
        {
            Logger.Warn(message);
        }

        // Log an error message
        public void LogError(string message)
        {
            Logger.Error(message);
        }

        // Log an exception
        public void LogException(Exception ex)
        {
            Logger.Error(ex);
        }
    }
}
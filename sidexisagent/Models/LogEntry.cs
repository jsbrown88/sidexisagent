using System;

namespace src.Models
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }

        public LogEntry(DateTime timestamp, string level, string message, string exception = null)
        {
            Timestamp = timestamp;
            Level = level;
            Message = message;
            Exception = exception;
        }
    }
}
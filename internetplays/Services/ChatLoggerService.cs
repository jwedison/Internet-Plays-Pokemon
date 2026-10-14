using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace internetplays.Services
{
    public class ChatLoggerService : IChatLogger
    {
        private string logFilePath { get; set; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "InternetPlays",
            "chatlog-" + DateTime.Now.ToString("yyyyMMdd") + ".txt");

        private bool loggingEnabled = true;

        public ChatLoggerService()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));
            
        }

        public void LogMessage(string message)
        {
            if (!loggingEnabled) return;

            using StreamWriter writer = new StreamWriter(logFilePath, append: true);
            {
                writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + message);
            }
        }

        public void Dispose()
        {
            logFilePath = null;
            loggingEnabled = false;
        }
    }
}

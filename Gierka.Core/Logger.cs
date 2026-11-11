using System;
using System.IO;

namespace Gierka.Core
{
    public static class Logger
    {
        private static readonly string LogFilePath = Path.Combine(AppContext.BaseDirectory, "application.log");

        public static void LogInfo(string message)
        {
            WriteLog("INFO", message);
        }

        public static void LogWarning(string message)
        {
            WriteLog("WARNING", message);
        }

        public static void LogError(string message, Exception ex = null)
        {
            string details = ex != null ? $"\nException: {ex.Message}\nStack Trace: {ex.StackTrace}" : "";
            WriteLog("ERROR", message + details);
        }

        private static void WriteLog(string level, string message)
        {
            try
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
                
                // Append text to file (creating if doesn't exist)
                File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
                
                // W prawdziwym świecie tutaj wysłalibyśmy też alert (np. e-mail lub Sentry)
                if (level == "ERROR") 
                {
                    // Symulacja alertu
                    Console.WriteLine($"!!! ALERT !!! Critical Error: {message}"); 
                }
            }
            catch
            {
                // Fallback: If logging fails (e.g. disk full), don't crash the app
            }
        }
    }
}

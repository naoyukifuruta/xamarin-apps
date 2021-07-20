using System;

namespace SimpleTimecard.Common
{
    public class Logger
    {
        private enum LogLevel
        {
            DEBUG,
            TRAECE,
            INFO,
            ERROR,
        }

        public static void Debug(string message)
        {
#if DEBUG
            Show(LogLevel.DEBUG, message);
#endif
        }

        public static void Trace(string message)
        {
            Show(LogLevel.TRAECE, message);
        }

        public static void Info(string message)
        {
            Show(LogLevel.INFO, message);
        }

        public static void Error(string message)
        {
            Show(LogLevel.ERROR, message);
        }

        private static void Show(LogLevel logLevel, string message)
        {
            System.Diagnostics.Debug.WriteLine("{0} [{1}] {2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), logLevel.ToString(), message);
        }
    }
}

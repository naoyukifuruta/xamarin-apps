using System;
using System.Runtime.CompilerServices;

namespace SimpleTimecard.Common
{
    public class Logger
    {
        private enum LogLevel
        {
            Debug,
            Info,
            Warning,
            Error,
        }

        public static void Trace(
            [CallerMemberName] string method = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            var s = new System.Diagnostics.StackFrame(1, false).GetMethod().DeclaringType.FullName.Split('.');
            var callerClassName = s.Length > 0 ? s[s.Length - 1] : string.Empty;

            OutputTrace(LogLevel.Info, callerClassName, method, "Trace");
        }

        public static void Debug(
            string message,
            [CallerMemberName] string method = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Output(LogLevel.Debug, method, filePath, lineNumber, message);
        }

        public static void Info(
            string message,
            [CallerMemberName] string method = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Output(LogLevel.Info, method, filePath, lineNumber, message);
        }

        public static void Warning(
            string message,
            [CallerMemberName] string method = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Output(LogLevel.Warning, method, filePath, lineNumber, message);
        }

        public static void Error(
            string message,
            [CallerMemberName] string method = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Output(LogLevel.Error, method, filePath, lineNumber, message);
        }

        private static void OutputTrace(LogLevel logLevel, string className, string method, string message)
        {
            System.Diagnostics.Debug.WriteLine(
                $"[{logLevel}] {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff")} {className}.{method} {message}"
            );
        }

        private static void Output(LogLevel logLevel, string method, string filePath, int lineNumber, string message)
        {
#if !DEBUG
            if (logLevel == LogLevel.Debug)
            {
                return;
            }
#endif

            System.Diagnostics.Debug.WriteLine(
                $"[{logLevel} {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff")} {method} {filePath} {lineNumber} {message}"
            );
        }
    }
}

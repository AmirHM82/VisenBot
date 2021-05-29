using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrimedBot
{
    public static class StringExtensions
    {
        static Task LogWriter;
        static ConcurrentBag<Log> Logs;
        static StringExtensions()
        {
            Logs = new ConcurrentBag<Log>();
            LogWriter = WriteLogs();
        }
        private static object logLock = new object();
        public const string InfoText = "info";
        public const string WarnText = "warn";
        public const string FailText = "fail";
        public static void LogInfo(this string log)
        {
            Logs.Add(new Log(ConsoleColor.DarkGreen, InfoText, log));
        }
        public static void LogWarning(this string log)
        {
            Logs.Add(new Log(ConsoleColor.DarkCyan, WarnText, log));
        }
        public static void LogError(this string log)
        {
            Logs.Add(new Log(ConsoleColor.DarkRed, FailText, log));
        }
        static async Task WriteLogs()
        {
            while (true)
            {
                if (Logs.TryTake(out Log log))
                {
                    WriteLog(log);
                }
                else
                {
                    await Task.Delay(10);
                }
            }
        }
        private static void WriteLog(Log log)
        {
            Console.ForegroundColor = log.Color;
            Console.Write($"{log.Title}: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(log.Text);
        }

        public struct Log
        {
            public ConsoleColor Color { get; set; }
            public string Title { get; set; }
            public string Text { get; set; }
            public Log(ConsoleColor color, string title, string text)
            {
                this.Color = color;
                Title = title;
                Text = text;
            }
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CockatriceCardImageLoader
{
    public class Logger
    {
        private static int logLength = 0;

        private static string logFilepath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), $"operation{DateTime.Now.ToString("yyyyMMdd")}.log");


        public static void Log(string entry, bool newLine = true)
        {
            LogToFile(entry);

            logLength += entry.Length;

            if (logLength > 25000)
            {
                Console.Clear();
                logLength = 0;
            }

            if (newLine)
                Console.WriteLine(entry);
            else
                Console.Write(entry);
        }

        public static void Log(string entry, Exception ex)
        {
            Log(entry);

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                Log(ex.Message);
            }
        }

        public static void LogToFile(string entry)
        {
            System.IO.File.AppendAllText(logFilepath, DateTime.Now.ToString() + "\t" + entry + "\r\n");
        }
    }
}

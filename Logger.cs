using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Text;

namespace IOTDeviceManagementSystem
{
    public class Logger
    {
        private static List<string> logs = new List<string>();

        public static void Logs(string message)
        {
            logs.Add(message);
            Console.WriteLine(message);
        }
        public static List<string> DisplayLogs()
        {
            if (logs.Count > 0)
            {
                string filePath = ConfigurationManager.AppSettings["LogFilePath"].ToString();
                logs.Add("\n");
                Console.WriteLine("\nLogs");
                foreach (string log in logs)
                {
                    Console.WriteLine(log);
                }
                File.AppendAllLines(filePath, logs);
                return logs;
            }
            return [];
        }
    }
}


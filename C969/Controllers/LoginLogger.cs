using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C969.Controllers
{
    /// <summary>
    /// Class to log the login history of users.
    /// </summary>
    public static class LoginLogger
    {
        //set the directory of Logs in Project/Bin/Debug and puts a file called Login_History.txt in it
        private static readonly string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly string _logFilePath = Path.Combine(logDirectory, "Login_History.txt");

        public static void LogLogger(string username)
        {
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            DateTime utcNow = DateTime.UtcNow;
            DateTime userTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, TimeZoneInfo.Local);
            string logMessage = $"{userTime}: {username} logged in.";
            File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
        }
    }
}

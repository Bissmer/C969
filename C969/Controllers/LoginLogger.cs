using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C969.Controllers
{
    public static class LoginLogger
    {
        private static readonly string _logFilePath = "Login_History.txt";

        public static void LogLogger(string username)
        {
            string logMessage = $"{DateTime.Now}: {username} logged in.";
            File.AppendAllText(_logFilePath, logMessage + Environment.NewLine);
        }
    }
}

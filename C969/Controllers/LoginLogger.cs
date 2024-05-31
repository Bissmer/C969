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
        private const string logFilePath = "Login_History.txt";

        public static void LogLogger(string username)
        {
            string logMessage = $"{DateTime.UtcNow}: {username} logged in.";
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }
    }
}

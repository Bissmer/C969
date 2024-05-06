using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Bcpg;

namespace C969.Models
{
    /// <summary>
    /// Class to manage the current user session
    /// </summary>
    public static class UserSession
    {
        public static int UserId { get; private set; }
        public static string CurrentUser { get; private set; }

        public static void Login(int userId, string userName)
        {
            UserId = userId;
            CurrentUser = userName;
        }

        /// <summary>
        /// Clear the current user session
        /// </summary>
        public static void Logout()
        {
            UserId = 0;
            CurrentUser = string.Empty;
        }
    }
}

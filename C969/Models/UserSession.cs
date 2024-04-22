using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C969.Models
{
    /// <summary>
    /// Class to manage the current user session
    /// </summary>
    public static class UserSession
    {
        public static string CurrentUser { get; private set; }

        public static void SetCurrentUser(string userName)
        {
            CurrentUser = userName;
        }

        /// <summary>
        /// Clear the current user session
        /// </summary>
        public static void ClearCurrentUser()
        {
            CurrentUser = null;
        }
    }
}

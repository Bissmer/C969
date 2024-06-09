using C969.Forms;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C969.Models;
using C969.Controllers;

namespace C969
{
    public partial class Login : Form
    {
        private readonly string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        public Login()
        {
            
            InitializeComponent();
            SetLanguage(CultureInfo.CurrentCulture);
            InitializeTimeZoneLabel();

        }

        CultureInfo currentCulture = CultureInfo.CurrentUICulture;
        ResourceManager _rm;
        
        /// <summary>
        /// Method to set the language of the form, using resource files
        /// </summary>
        /// <param name="culture"></param>
        private void SetLanguage(CultureInfo culture)
        {

            //check culture and set the appropriate resource file
            _rm = culture.TwoLetterISOLanguageName == "ru" ? new ResourceManager("C969.Resources.LoginFormPrompts_ru", typeof(Login).Assembly) : 
                new ResourceManager("C969.Resources.LoginFormPrompts_en", typeof(Login).Assembly);
         
            loginHeaderLabel.Text = _rm.GetString("loginHeaderLabel", culture);
            loginHeaderLabel.Text = _rm.GetString("loginHeaderLabel", culture);
            loginLoginButton.Text = _rm.GetString("loginLoginButton", culture);
            loginPasswordLabel.Text = _rm.GetString("loginPasswordLabel", culture);
            loginUsernameLabel.Text = _rm.GetString("loginUsernameLabel", culture);
            loginQuitButton.Text = _rm.GetString("loginQuitButton", culture);
            this.Text = _rm.GetString("Login", culture);

        }

        /// <summary>
        /// Method to authenticate the user by checking the username and password against the database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private int AuthenticateUser(string username, string password)
        {
            using (var conn = new MySqlConnection(_connString))
            {
                conn.Open();
                var query = "SELECT UserId FROM user WHERE Username = @username AND Password = @password";  
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);  
                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }

        }

        /// <summary>
        /// Method to handle the Login button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginLoginButton_Click(object sender, EventArgs e)
        {
            string username = loginUserNametxt.Text;
            string password = loginPasswordtxt.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(_rm.GetString("UsernamePwdEmptyMessage", currentCulture));
                return;
            }
            int userId = AuthenticateUser(username, password);
            if (userId > 0)  // Check if a valid user ID was returned
            {
                UserSession.Login(userId, username);  // Update the session manager
                CustomerManagementForm customerManagementForm = new CustomerManagementForm();
                customerManagementForm.Show();
                LoginLogger.LogLogger(username);
                this.Hide();
            }
            else
            {
                MessageBox.Show(_rm.GetString("LoginFailedMessage", currentCulture));
            }

        }

        /// <summary>
        /// Method to handle the Quit button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginQuitButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(_rm.GetString("QuitMessage", currentCulture), _rm.GetString("QuitTitle", currentCulture), MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        /// <summary>
        /// Method to initialize the time zone label
        /// </summary>
        private void InitializeTimeZoneLabel()
        {
            loginTimeZoneLabel.Text = _rm.GetString("loginTimeZoneLabel", currentCulture);
            // Getting the current time zone of the system
            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;

            // Setting the label to show the display name of the time zone
            loginTimeZoneText.Text = $"{localTimeZone.DisplayName}";

        }

    }
}

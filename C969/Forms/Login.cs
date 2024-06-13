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
using System.Text.RegularExpressions;

namespace C969
{
    public partial class Login : Form
    {
        private readonly string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        ResourceManager _rm;
        CultureInfo currentCulture = CultureInfo.CurrentUICulture;
        public Login()
        {
            
            InitializeComponent();
            SetLanguage(CultureInfo.CurrentCulture);
            InitializeTimeZoneLabel();

        }


        /// <summary>
        /// Method to set the language of the form based on the culture passed
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

            if (culture.TwoLetterISOLanguageName == "ru")
            {

                loginHeaderLabel.Location = new Point(loginHeaderLabel.Location.X - 80, loginHeaderLabel.Location.Y);
            }
            else
            {
                loginHeaderLabel.Location = new Point(137, loginHeaderLabel.Location.Y);
            }

        }

        /// <summary>
        /// Method to authenticate the user by checking the username and password against the database
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private int AuthenticateUser(string username, string password)
        {
            try
            {
                using (var conn = new MySqlConnection(_connString))
                {
                    conn.Open(); // Check the connection here
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
            catch (Exception ex)
            {
                throw new DatabaseConnectionException(_rm.GetString("ConnectionFailedMessage", currentCulture) + "\n" + ex.Message);
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

            try
            {
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
                    string message = _rm.GetString("LoginFailedMessage", currentCulture);
                    string caption = _rm.GetString("LoginFailedCaption", currentCulture);
                    MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (DatabaseConnectionException dbEx)
            {
                MessageBox.Show(dbEx.Message, _rm.GetString("ConnectionFailedCaption", currentCulture), MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

        }

        /// <summary>
        /// Method to handle the Quit button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginQuitButton_Click(object sender, EventArgs e)
        {
            string message = _rm.GetString("QuitMessage", currentCulture);
            string caption = _rm.GetString("QuitTitle", currentCulture);
            DialogResult dialogResult = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

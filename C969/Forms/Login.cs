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
        private string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        public Login()
        {
            
            InitializeComponent();
            SetLanguage(CultureInfo.CurrentCulture);

        }

        CultureInfo currentCulture = CultureInfo.CurrentUICulture;
        ResourceManager _rm;
        
        //Localization using ResourceManager and CultureInfo
        private void SetLanguage(CultureInfo culture)
        {

            //check culture and set the appropriate resource file
            if (culture.TwoLetterISOLanguageName == "en")
            {
                _rm = new ResourceManager("C969.Resources.LoginFormPrompts_en", typeof(Login).Assembly);
            }
            else
            {
                _rm = new ResourceManager("C969.Resources.LoginFormPrompts_ru", typeof(Login).Assembly);
            }
         
            loginHeaderLabel.Text = _rm.GetString("loginHeaderLabel", culture);
            loginHeaderLabel.Text = _rm.GetString("loginHeaderLabel", culture);
            loginLoginButton.Text = _rm.GetString("loginLoginButton", culture);
            loginPasswordLabel.Text = _rm.GetString("loginPasswordLabel", culture);
            loginUsernameLabel.Text = _rm.GetString("loginUsernameLabel", culture);
            loginQuitButton.Text = _rm.GetString("loginQuitButton", culture);
            this.Text = _rm.GetString("Login", culture);

        }

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
                this.Hide();
            }
            else
            {
                MessageBox.Show(_rm.GetString("LoginFailedMessage", currentCulture));
            }

        }

        private void loginQuitButton_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show(_rm.GetString("QuitMessage", currentCulture), _rm.GetString("QuitTitle", currentCulture), MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}

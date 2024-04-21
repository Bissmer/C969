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

namespace C969
{
    public partial class Login : Form
    {
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

        private bool AuthenticateUser(string username, string password)
        {
            bool isAuthenticated = false;

            string connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connString))
            {
                string query = "SELECT userId FROM user WHERE userName = @username AND password = @password";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                try
                {
                    connection.Open();
                    object result = cmd.ExecuteScalar();
                    isAuthenticated = result != null && Convert.ToInt32(result) == 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_rm.GetString("DBConnectionFailure", currentCulture));
                }
            }
            return isAuthenticated;

        }

        private void loginLoginButton_Click(object sender, EventArgs e)
        {
            string username = loginUserName.Text;
            string password = loginPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(_rm.GetString("UsernamePwdEmptyMessage", currentCulture));
                return;
            }
            if (AuthenticateUser(username, password))
            {
                CustomerManagementForm customerManagementForm = new CustomerManagementForm();
                customerManagementForm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show(_rm.GetString("LoginFailedMessage", currentCulture));
            }
        }
    }
}

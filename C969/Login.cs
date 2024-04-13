using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

        //Localization using ResourceManager and CultureInfo
        private void SetLanguage(CultureInfo culture)
        {
            ResourceManager rm = new ResourceManager("C969.Resources.LoginFormPrompts_en", typeof(Login).Assembly);
            loginHeaderLabel.Text = rm.GetString("loginHeaderLabel", culture);
            loginHeaderLabel.Text = rm.GetString("loginHeaderLabel", culture);
            loginLoginButton.Text = rm.GetString("loginLoginButton", culture);
            loginPasswordLabel.Text = rm.GetString("loginPasswordLabel", culture);
            loginUsernameLabel.Text = rm.GetString("loginUsernameLabel", culture);
            loginQuitButton.Text = rm.GetString("loginQuitButton", culture);
            this.Text = rm.GetString("Login", culture);

            //check culture and set the appropriate resource file
            if(culture.TwoLetterISOLanguageName == "ru")
            {
                rm = new ResourceManager("C969.Resources.LoginFormPrompts_ru", typeof(Login).Assembly);
            }
            else
            {
                rm = new ResourceManager("C969.Resources.LoginFormPrompts_en", typeof(Login).Assembly);
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            bool isAuthenticated = false;

            string connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connString))
            {
                string query = "SELECT userId FROM user WHERE userName = @username AND password = @password";
                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);

                try
                {
                    connection.Open();
                    isAuthenticated = (int)cmd.ExecuteScalar() > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return isAuthenticated;

        }

        private void loginLoginButton_Click(object sender, EventArgs e)
        {
            string username = loginUserName.Text;
            string password = loginPassword.Text;

            if (AuthenticateUser(username, password))
            {
                MessageBox.Show("Login Successful");
            }
            else
            {
                MessageBox.Show("Login Failed");
            }
        }
    }
}

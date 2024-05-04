using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C969.Controllers;
using C969.Models;
using MySql.Data.MySqlClient;

namespace C969.Forms
{
    public partial class AddCustomerForm : Form
    {
        private CustomerDataHandler _customerDataHandler;
        private string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        private string _currentUser = UserSession.CurrentUser;

        public AddCustomerForm()
        {
            InitializeComponent();
            _customerDataHandler = new CustomerDataHandler(_connString);
            LoadCountries();
        }

        private void LoadCountries()
        {
            using (var connection = new MySqlConnection(_connString))
            {
                connection.Open();
                var query = "SELECT country FROM country";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            addCustomerCountryCombo.Items.Add(reader["country"].ToString());
                        }
                    }
                }
            }
        }

        private void addCustomerSaveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = addCustomerNameText.Text;
                string address = addCustomerAddresText.Text;
                string address2 = addCustomerAddress2Text.Text;
                string phone = addCustomerPhoneText.Text;
                string city = addCustomerCityText.Text;
                string postalCode = addCustomerZipText.Text;
                string country = addCustomerCountryCombo.SelectedItem.ToString() ?? "";
                bool isActive = addCustomerActiveCheck.Checked;


                if (string.IsNullOrEmpty(country))
                {
                    MessageBox.Show("Please select a country from the list.");
                    return;
                }

                bool result = _customerDataHandler.AddCustomerWithDetails(customerName, address, address2, phone, city,
                    postalCode, country, isActive);
                if (result)
                {
                    MessageBox.Show("Customer added successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to add customer. Check the data and try again.");
                }

            }
            catch (Exception ex)
            {
                
                MessageBox.Show($"An error occurred while saving the customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Close();
            }


        }

        private void addCustomerCancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

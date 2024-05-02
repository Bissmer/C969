using C969.Controllers;
using C969.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace C969.Forms
{
    public partial class EditCustomerForm : Form
    {
        private int _customerId;
        private CustomerDataHandler _customerDataHandler;
        private string _currentUser = UserSession.CurrentUser;
        private string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        public EditCustomerForm(int customerId, CustomerDataHandler customerDataHandler)
        {
            InitializeComponent();
            _customerId = customerId;
            _customerDataHandler = customerDataHandler;
            LoadCustomerData();


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
                            editCustomerCountryCombo.Items.Add(reader["country"].ToString());
                        }
                    }
                }
            }
        }

        private void LoadCustomerData()
        {
            var customerDetails = _customerDataHandler.GetCustomerDetails(_customerId);
            if (customerDetails != null)
            {
                editCustomerNameText.Text = customerDetails.CustomerName;
                editCustomerAddresText.Text = customerDetails.Address;
                editCustomerAddress2Text.Text = customerDetails.Address2;
                editCustomerPhoneText.Text = customerDetails.Phone;
                editCustomerCityText.Text = customerDetails.City;
                editCustomerZipText.Text = customerDetails.PostalCode;
                editCustomerActiveCheck.Checked = customerDetails.IsActive;

                editCustomerCountryCombo.DataSource = _customerDataHandler.GetCountries();
                editCustomerCountryCombo.SelectedItem = customerDetails.Country;
            }
            else
            {
                MessageBox.Show("Customer not found.");
                this.Close();
            }

           
        }

        private void editCustomerSaveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (editCustomerCountryCombo.SelectedItem == null)
                {
                    MessageBox.Show("Please select a country.");
                    return; // Stop further execution
                }

                CustomerDetails customer = new CustomerDetails()
                {
                    CustomerID = _customerId,
                    CustomerName = editCustomerNameText.Text,
                    Address = editCustomerAddresText.Text,
                    Address2 = editCustomerAddress2Text.Text,
                    Phone = editCustomerPhoneText.Text,
                    City = editCustomerCityText.Text,
                    PostalCode = editCustomerZipText.Text,
                    Country = editCustomerCountryCombo.SelectedItem.ToString(),
                    IsActive = editCustomerActiveCheck.Checked
                };
                if (_customerDataHandler.UpdateCustomerDetails(customer))
                {
                    MessageBox.Show("Customer updated successfully.");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to update customer.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        private void editCustomerCancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

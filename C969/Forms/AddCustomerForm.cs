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
        private readonly CustomerDataHandler _customerDataHandler;
        private readonly string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        public AddCustomerForm()
        {
            InitializeComponent();
            _customerDataHandler = new CustomerDataHandler(_connString);
            LoadCountries();
            addCustomerPhoneText.KeyPress += new KeyPressEventHandler(PhoneTextBox_KeyPress);
        }

        /// <summary>
        /// Method that loads the countries into the country combo box
        /// </summary>
        private void LoadCountries()
        {
            try
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
                if (addCustomerCountryCombo.Items.Count > 0)
                {
                    addCustomerCountryCombo.SelectedIndex = 0; // Set the first item as the default
                }
            } catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading countries: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Method that saves the customer details to the database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addCustomerSaveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = addCustomerNameText.Text.Trim();
                string address = addCustomerAddresText.Text.Trim();
                string address2 = addCustomerAddress2Text.Text.Trim();
                string phone = addCustomerPhoneText.Text.Trim();
                string city = addCustomerCityText.Text.Trim();
                string postalCode = addCustomerZipText.Text.Trim();
                string country = addCustomerCountryCombo.SelectedItem.ToString() ?? "";
                bool isActive = addCustomerActiveCheck.Checked;


                //fields validation block

                var fields = new Dictionary<string, Control>
                {
                    { "Customer Name", addCustomerNameText },
                    { "Address", addCustomerAddresText },
                    { "Phone", addCustomerPhoneText },
                    { "City", addCustomerCityText },
                    { "Postal Code", addCustomerZipText },
                    { "Country", addCustomerCountryCombo }
                };

                if (!ValidateFields(fields))
                {
                    return;
                }

                bool result = _customerDataHandler.AddCustomerWithDetails(customerName, address, address2, phone, city,
                    postalCode, country, isActive);

                if (result)
                {
                    MessageBox.Show("Customer added successfully.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to add customer. Check the entered data and try again.");
                }

            }
            catch (Exception ex)
            {
                
                MessageBox.Show($"An error occurred while saving the customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Method that cancels the adding of a customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addCustomerCancelBtn_Click(object sender, EventArgs e)
        {
            
            DialogResult result = MessageBox.Show("Are you sure you want to cancel adding the customer?", "Cancel Add",
                                              MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Method that validates the form fields before saving the customer
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        private bool ValidateFields(Dictionary<string, Control> fields)
        {

            foreach (var field in fields)
            {
                if (field.Value is TextBox textBox)
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        MessageBox.Show($"{field.Key} cannot be empty.");
                        return false;
                    }
                    if (field.Value is ComboBox comboBox)
                    {
                        if (comboBox.SelectedItem == null || string.IsNullOrWhiteSpace(comboBox.SelectedItem.ToString()))
                        {
                            MessageBox.Show($"{field.Key} cannot be empty.");
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Method that enables the phone number field to only allow numbers and hyphens,
        /// other characters are ignored
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhoneTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
            {
                e.Handled = true;
            }
        }
    }
}

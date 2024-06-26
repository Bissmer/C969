﻿using C969.Controllers;
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
        private readonly int _customerId;
        private readonly CustomerAppointmentsDataHandler _customerAppointmentsDataHandler;

        private readonly string _connString =
            ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        public EditCustomerForm(int customerId, CustomerAppointmentsDataHandler customerAppointmentsDataHandler)
        {
            InitializeComponent();
            _customerId = customerId;
            _customerAppointmentsDataHandler = customerAppointmentsDataHandler;
            LoadCountries();
            LoadCustomerData();
            editCustomerPhoneText.KeyPress += new KeyPressEventHandler(PhoneTextBox_KeyPress);
        }

        /// <summary>
        /// Method to load countries into the country combobox
        /// </summary>
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
        /// <summary>
        /// Method to load customer data when an EditCustomer is opened 
        /// </summary>
        private void LoadCustomerData()
        {
            var customerDetails = _customerAppointmentsDataHandler.GetCustomerDetails(_customerId);
            if (customerDetails != null)
            {
                editCustomerNameText.Text = customerDetails.CustomerName;
                editCustomerAddresText.Text = customerDetails.Address;
                editCustomerAddress2Text.Text = customerDetails.Address2;
                editCustomerPhoneText.Text = customerDetails.Phone;
                editCustomerCityText.Text = customerDetails.City;
                editCustomerZipText.Text = customerDetails.PostalCode;
                editCustomerActiveCheck.Checked = customerDetails.IsActive;

                editCustomerCountryCombo.DataSource = _customerAppointmentsDataHandler.GetCountries();
                editCustomerCountryCombo.SelectedItem = customerDetails.Country;
            }
            else
            {
                MessageBox.Show("Customer not found.");
                this.Close();
            }

        }

        /// <summary>
        /// Method to save the edited customer details on Save button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editCustomerSaveBtn_Click(object sender, EventArgs e)
        {
            try
            {

                var fields = new Dictionary<string, Control>
                {
                    { "Customer Name", editCustomerNameText },
                    { "Address", editCustomerAddresText },
                    { "Phone", editCustomerPhoneText },
                    { "City", editCustomerCityText },
                    { "Postal Code", editCustomerZipText },
                    { "Country", editCustomerCountryCombo }
                };

                if (!ValidateFields(fields))
                {
                    return;
                }

                CustomerDetails customer = new CustomerDetails()
                {
                    CustomerID = _customerId,
                    CustomerName = editCustomerNameText.Text.Trim(),
                    Address = editCustomerAddresText.Text.Trim(),
                    Address2 = editCustomerAddress2Text.Text.Trim(),
                    Phone = editCustomerPhoneText.Text.Trim(),
                    City = editCustomerCityText.Text.Trim(),
                    PostalCode = editCustomerZipText.Text.Trim(),
                    Country = editCustomerCountryCombo.SelectedItem.ToString(),
                    IsActive = editCustomerActiveCheck.Checked
                };

                if (_customerAppointmentsDataHandler.UpdateCustomerDetails(customer))
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

        /// <summary>
        /// Method to cancel editing customer details on Cancel button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editCustomerCancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to cancel editing the customer and exit without save?", "Cancel Edit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Method to validate phone number input
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

        /// <summary>
        /// Method to validate form fields
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
                        MessageBox.Show($"{field.Key} cannot be empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (field.Value is ComboBox comboBox)
                    {
                        if (comboBox.SelectedItem == null || string.IsNullOrWhiteSpace(comboBox.SelectedItem.ToString()))
                        {
                            MessageBox.Show($"{field.Key} cannot be empty.", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}

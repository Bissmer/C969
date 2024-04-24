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

namespace C969.Forms
{
    public partial class EditCustomerForm : Form
    {
        private CustomerController _customerController;
        private Customer _currentCustomer;
        public EditCustomerForm(Customer customer)
        {
            InitializeComponent();
            _customerController = new CustomerController();
            _currentCustomer = customer;
            PopulateCustomerDetails();
        }

        private void PopulateCustomerDetails()
        {
            editCustomerCustomerNametxt.Text = _currentCustomer.CustomerName;
            editCustomerAddressIDtxt.Text = _currentCustomer.AddressID.ToString();
            editCustomerActiveCheck.Checked = _currentCustomer.Active == 1;
        }

        private void editCustomerSaveBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(editCustomerCustomerNametxt.Text) ||
                string.IsNullOrEmpty(editCustomerAddressIDtxt.Text))
            {
                MessageBox.Show("Please enter a customer name and address ID.");
                return;
            }

            if (!int.TryParse(editCustomerAddressIDtxt.Text, out var addressID))
            {
                MessageBox.Show("Address ID must be a number.");
                return;
            }

            // Check if the AddressID exists in the database
            if (!_customerController.AddressExists(addressID))
            {
                MessageBox.Show("No address found with the given Address ID. Please enter a valid Address ID.");
                return;
            }

            // Update the customer object and save it to the database
            _currentCustomer.CustomerName = editCustomerCustomerNametxt.Text.Trim();
            _currentCustomer.AddressID = addressID;
            _currentCustomer.Active = editCustomerActiveCheck.Checked ? 1 : 0;
            _currentCustomer.LastUpdate = DateTime.Now;
            _currentCustomer.LastUpdateBy = UserSession.CurrentUser;

            try
            {
                _customerController.UpdateCustomer(_currentCustomer);
                MessageBox.Show("Customer updated successfully.");
                DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while updating the customer: {ex.Message}");
            }
        }

        private void editCustomerCanceBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

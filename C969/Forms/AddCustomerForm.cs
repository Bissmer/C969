using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C969.Controllers;
using C969.Models;

namespace C969.Forms
{
    public partial class AddCustomerForm : Form
    {
        private CustomerController _customerController;
        public AddCustomerForm()
        {
            InitializeComponent();
            _customerController = new CustomerController();
        }

        private void addCustomerSaveBtn_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrEmpty(addCustomerCustomerNametxt.Text) ||
                string.IsNullOrEmpty(addCustomerAddressIDtxt.Text))
            {
                    MessageBox.Show("Please enter a customer name and address ID.");
                    return;
            }
            if (!int.TryParse(addCustomerAddressIDtxt.Text, out var addressID))
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

            // Create a new customer object and save it to the database

            string currentUser = UserSession.CurrentUser;
            var customer = new Customer
            (
                addCustomerCustomerNametxt.Text.Trim(),
                addressID,
                addCustomerActiveCheck.Checked ? 1 : 0,
                currentUser,
                currentUser
            );

            try
            {
                var customerController = new CustomerController();
                customerController.AddCustomer(customer);
                MessageBox.Show("Customer added successfully.");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding the customer: {ex.Message}");
            }
        }

        private void addCustomerCanceBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

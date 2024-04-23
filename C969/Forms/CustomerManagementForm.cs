using C969.Controllers;
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
    public partial class CustomerManagementForm : Form
    {
        private CustomerController _customerController;
        public CustomerManagementForm()
        {
            InitializeComponent();
            _customerController = new CustomerController();
            LoadCurrentUser();
            LoadCustomers();
            
        }

        //debug label to show current user
        private void LoadCurrentUser()
        {
            if (!string.IsNullOrEmpty(Models.UserSession.CurrentUser))
            {
                cusMgmCurrentUserlbl.Text = $"Logged user: {Models.UserSession.CurrentUser}";
            }
            else
            {
                cusMgmCurrentUserlbl.Text = "Unknown";
            }
        }
        private void cusMgmtAddCustomerButton_Click(object sender, EventArgs e)
        {
            var addForm = new AddCustomerForm();
            addForm.ShowDialog();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            var customers = _customerController.GetAllCustomers();
            cusMgmtDgvCustomers.DataSource = customers;
            cusMgmtDgvCustomers.Refresh();
        }

        private void cusMgmtEditCustomerButton_Click(object sender, EventArgs e)
        {
            if (cusMgmtDgvCustomers.CurrentRow != null)
            {
                //int customerId = Convert.ToInt32(cusMgmtDgvCustomers.CurrentRow.Cells["customerID"].Value);
                //var editForm = new EditCustomerForm(customerId);
                //editForm.ShowDialog();
                //LoadCustomers();
                MessageBox.Show("Edit Customer functionality is not implemented yet.");
            }
        }

        private void cusMgmtDeleteCustomerButton_Click(object sender, EventArgs e)
        {
            if (cusMgmtDgvCustomers.CurrentRow != null)
            {
                int customerId = Convert.ToInt32(cusMgmtDgvCustomers.CurrentRow.Cells["customerID"].Value);
                _customerController.DeleteCustomer(customerId);
                LoadCustomers();
            }
        }
    }
}

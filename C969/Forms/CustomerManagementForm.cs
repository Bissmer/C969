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
            LoadCustomers();
            
        }

        private void cusMgmtAddCustomerButton_Click(object sender, EventArgs e)
        {

        }

        private void LoadCustomers()
        {
            var customers = _customerController.GetAllCustomers();
            cusMgmtDgvCustomers.DataSource = customers;
            cusMgmtDgvCustomers.Refresh();
        }
    }
}

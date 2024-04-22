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
    }
}

using C969.Controllers;
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

namespace C969.Forms
{
    public partial class CustomerManagementForm : Form
    {
        private CustomerDataHandler _customerDataHandler;
        private string _currentUser = Models.UserSession.CurrentUser;
        private string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        public CustomerManagementForm()
        {
            InitializeComponent();
            _customerDataHandler = new CustomerDataHandler(_connString);
            cusMgmtSearchAppByCustomer.TextChanged += cusMgmtSearchAppByCustomer_TextChanged;
            cusMgmtDeleteAppointmentButton.Click += cusMgmtDeleteAppointmentButton_Click;
            LoadCurrentUser();
            LoadCustomers();
            LoadAppointments();


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
            var customers = _customerDataHandler.GetAllCustomers();
            cusMgmtDgvCustomers.DataSource = customers;
            cusMgmtDgvCustomers.Refresh();
        }

        private void LoadAppointments()
        {
            var appointments = _customerDataHandler.GetAllAppointments();
            cusMgmtDgvAppontments.DataSource = appointments;
            cusMgmtDgvAppontments.Refresh();
        }

        private void ExitApplication()
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to end the session and exit?", "End Session", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void cusMgmtEditCustomerButton_Click(object sender, EventArgs e)
        {
            if (cusMgmtDgvCustomers.CurrentRow != null)
            {
                int customerId = Convert.ToInt32(cusMgmtDgvCustomers.CurrentRow.Cells["CustomerID"].Value);
                var editForm = new EditCustomerForm(customerId, _customerDataHandler);
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    LoadCustomers();
                }
            }
        }

        private void cusMgmtDeleteCustomerButton_Click(object sender, EventArgs e)
        {
            if (cusMgmtDgvCustomers.CurrentRow != null)
            {
                int customerId = Convert.ToInt32(cusMgmtDgvCustomers.CurrentRow.Cells["customerID"].Value);
                _customerDataHandler.DeleteCustomer(customerId);
                LoadCustomers();
            }
        }

        private void cusMgmtEndSessionButton_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }

        private void cusMgmtAddAppointmentButton_Click(object sender, EventArgs e)
        {
            var addForm = new AddAppointmentForm();
            addForm.ShowDialog();
            LoadAppointments();
        }



        private void cusMgmtSearchAppByCustomer_TextChanged(object sender, EventArgs e)
        {
            FilterAppointmentsByCustomerName(cusMgmtSearchAppByCustomer.Text);
        }

        private void FilterAppointmentsByCustomerName(string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
            {
                LoadAppointments(); // Reload all if the search box is cleared
                return;
            }

            var filteredAppointments = _customerDataHandler.GetAppointmentsByCustomerName(customerName);
            cusMgmtDgvAppontments.DataSource = filteredAppointments;
        }

        private void cusMgmtEditAppointmentButton_Click(object sender, EventArgs e)
        {
            if (cusMgmtDgvAppontments.CurrentRow != null)
            {
                int appointmentId = Convert.ToInt32(cusMgmtDgvAppontments.CurrentRow.Cells["AppointmentID"].Value);
                EditAppointmentForm editForm = new EditAppointmentForm(appointmentId);
                editForm.ShowDialog();
                LoadAppointments();
            }
        }

        /// <summary>
        /// Function to delete the selected appointment in the row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cusMgmtDeleteAppointmentButton_Click(object sender, EventArgs e)
        {
            if (cusMgmtDgvAppontments.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show("Are you sure you want to delete the selected appointment?", "Delete Appointment",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    int appointmentId = Convert.ToInt32(cusMgmtDgvAppontments.SelectedRows[0].Cells["AppointmentId"].Value);
                    if (_customerDataHandler.DeleteApppointment(appointmentId))
                    {
                        MessageBox.Show("Appointment deleted successfully.");
                        LoadAppointments();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete the appointment. Please try again.");
                    }
                    
                }
            }
        }
        }
}


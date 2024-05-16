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
        private bool isProgrammaticallyAdjusting = false; // Flag to prevent infinite loop when adjusting the calendar date
        private CustomerDataHandler _customerDataHandler;
        private string _currentUser = Models.UserSession.CurrentUser;
        private string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        public CustomerManagementForm()
        {
            InitializeComponent();
            _customerDataHandler = new CustomerDataHandler(_connString);
            cusMgmtSearchAppByCustomer.TextChanged += cusMgmtSearchAppByCustomer_TextChanged;
            cusMgmtDeleteAppointmentButton.Click += cusMgmtDeleteAppointmentButton_Click;
            cusMgmtAppointmentsCalendar.DateChanged += cusMgmtAppointmentsCalendar_DateChanged;
            cusMgmtSearchAppByCustomer.Enter += cusMgmtSearchAppByCustomer_Enter;
            cusMgmtSearchAppByCustomer.Leave += cusMgmtSearchAppByCustomer_Leave;
            LoadAppointmentsForSelectedDate(cusMgmtAppointmentsCalendar.SelectionStart); // Load appointments for the selected date
            LoadCurrentUser();
            LoadCustomers();
            LoadAppointments();
            UpdateCalendarHighlights();


        }


        private void cusMgmtAddCustomerButton_Click(object sender, EventArgs e)
        {
            var addForm = new AddCustomerForm();
            addForm.ShowDialog();
            LoadCustomers();
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
            UpdateCalendarHighlights();

        }

        /// <summary>
        /// Debug label to show current user
        /// </summary>
        private void LoadCurrentUser()
        {
            if (!string.IsNullOrEmpty(Models.UserSession.CurrentUser))
            {
                cusMgmCurrentUserlbl.Text = $"Welcome back, {Models.UserSession.CurrentUser}.";
            }
            else
            {
                cusMgmCurrentUserlbl.Text = "Unknown user";
            }
        }


        /// <summary>
        /// Function to load all customers and refresh the Customers data grid view
        /// </summary>
        private void LoadCustomers()
        {
            var customers = _customerDataHandler.GetAllCustomers();
            cusMgmtDgvCustomers.DataSource = customers;
            cusMgmtDgvCustomers.Refresh();
        }

        /// <summary>
        /// Fuction that loads all appointments and refreshes the Appointments data grid view
        /// </summary>
        private void LoadAppointments()
        {
            var appointments = _customerDataHandler.GetAllAppointments();
            cusMgmtDgvAppontments.DataSource = appointments;
            cusMgmtDgvAppontments.Refresh();
        }

        /// <summary>
        /// Function to exit the application
        /// </summary>
        private void ExitApplication()
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to end the session and exit?", "End Session", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
        }


        private void cusMgmtSearchAppByCustomer_TextChanged(object sender, EventArgs e)
        {
            FilterAppointmentsByCustomerName(cusMgmtSearchAppByCustomer.Text);
        }

        /// <summary>
        /// Function to filter appointments by customer name
        /// </summary>
        /// <param name="customerName"></param>
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
                UpdateCalendarHighlights();
            }
        }

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
                        UpdateCalendarHighlights();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete the appointment. Please try again.");
                    }
                    
                }
            }
        }

        /// <summary>
        /// Function to load appointments for the selected date
        /// </summary>
        /// <param name="selectedDate"></param>
        private void LoadAppointmentsForSelectedDate(DateTime selectedDate)
        {
            // Assuming _dataHandler is your database access object
            var appointments = _customerDataHandler.GetAppointmentsByDate(selectedDate);
            cusMgmtDgvAppontments.DataSource = appointments;
        }

        /// <summary>
        /// Function to handle the date change event on the calendar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cusMgmtAppointmentsCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            {
                // Prevent infinite loop when adjusting the calendar date
                if (isProgrammaticallyAdjusting)
                    return;
                // If the selected date is a weekend, adjust it to the next weekday
                if (e.Start.DayOfWeek == DayOfWeek.Saturday || e.Start.DayOfWeek == DayOfWeek.Sunday)
                {
                    isProgrammaticallyAdjusting = true;
                    DateTime nextWeekday = FindNextWeekday(e.Start);
                    cusMgmtAppointmentsCalendar.SelectionStart = nextWeekday;
                    cusMgmtAppointmentsCalendar.SelectionEnd = nextWeekday;
                    isProgrammaticallyAdjusting = false;
                }

                // Load appointments for the selected date
                LoadAppointmentsForSelectedDate(cusMgmtAppointmentsCalendar.SelectionStart);
            }
        }

        /// <summary>
        /// Function to find the next weekday
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns></returns>
        private DateTime FindNextWeekday(DateTime startDate)
        {
            DateTime nextDate = startDate;
            while (nextDate.DayOfWeek == DayOfWeek.Saturday || nextDate.DayOfWeek == DayOfWeek.Sunday)
            {
                nextDate = nextDate.AddDays(1);
            }
            return nextDate;
        }

        /// <summary>
        /// Function to bold the dates on the calendar that have appointments
        /// </summary>
        private void BoldAppointmentDates()
        {
            List<DateTime> datesWithAppointments = _customerDataHandler.GetDatesWithAppointments(); // Fetch or calculate these dates
            cusMgmtAppointmentsCalendar.BoldedDates = datesWithAppointments.ToArray();
        }

        /// <summary>
        /// Function to update the calendar highlights
        /// </summary>
        private void UpdateCalendarHighlights()
        {
            BoldAppointmentDates();
            cusMgmtAppointmentsCalendar.UpdateBoldedDates();  // Refreshes the calendar display
        }

        private void cusMgmtSearchAppByCustomer_Enter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Search by Customer Name")
            {
                textBox.Text = "";
                textBox.ForeColor = Color.Black;
            }
        }

        private void cusMgmtSearchAppByCustomer_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Search by Customer Name";
                textBox.ForeColor = Color.Gray;
            }
        }
    }
}


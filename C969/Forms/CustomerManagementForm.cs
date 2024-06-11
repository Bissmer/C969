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
using C969.Models;
using MySql.Data.MySqlClient;

namespace C969.Forms
{
    public partial class CustomerManagementForm : Form
    {
        private bool _isProgrammaticallyAdjusting = false; // Flag to prevent infinite loop when adjusting the calendar date
        private readonly CustomerDataHandler _customerDataHandler;
        private  readonly string _currentUser = UserSession.CurrentUser;
        private readonly string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        public CustomerManagementForm()
        {
            InitializeComponent();
            this.Load += CustomerManagementForm_Load;
            _customerDataHandler = new CustomerDataHandler(_connString);
            LoadAppointmentsForSelectedDate(cusMgmtAppointmentsCalendar.SelectionStart); // Load appointments for the selected date
            LoadCurrentUser();
            LoadCustomers();
            LoadAppointments();
            UpdateCalendarHighlights();
            this.Shown += CustomerManagementForm_Shown;


        }

        /// <summary>
        /// Load event handler for the Customer Management form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerManagementForm_Load(object sender, EventArgs e)
        {

            cusMgmtEditCustomerButton.Enabled = false;
            cusMgmtDeleteCustomerButton.Enabled = false;
            cusMgmtEditAppointmentButton.Enabled = false;
            cusMgmtDeleteAppointmentButton.Enabled = false;
            cusMgmtDgvCustomers.SelectionChanged += cusMgmtDgvCustomers_SelectionChanged;
            cusMgmtDgvAppontments.SelectionChanged += cusMgmtDgvAppontments_SelectionChanged;
            cusMgmtSearchAppByCustomer.TextChanged += cusMgmtSearchAppByCustomer_TextChanged;
            cusMgmtAppointmentsCalendar.DateChanged += cusMgmtAppointmentsCalendar_DateChanged;
            cusMgmtSearchAppByCustomer.Enter += cusMgmtSearchAppByCustomer_Enter;
            cusMgmtSearchAppByCustomer.Leave += cusMgmtSearchAppByCustomer_Leave;

        }

        #region Forms open

        /// <summary>
        /// Triggers the Add Customer form open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cusMgmtAddCustomerButton_Click(object sender, EventArgs e)
        {
            var addForm = new AddCustomerForm();
            addForm.ShowDialog();
            LoadCustomers();
        }

        /// <summary>
        /// Handler to trigger an Add Appointment form open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cusMgmtAddAppointmentButton_Click(object sender, EventArgs e)
        {
            var addForm = new AddAppointmentForm();
            addForm.ShowDialog();
            LoadAppointments();
            UpdateCalendarHighlights();

        }

        /// <summary>
        /// Triggers the Edit Customer form open for the selected customer on the Customer data grid view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Triggers the edit appointment form output for the selected appointment on the Appointments data grid view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Enables or disables the edit and delete buttons based on the selection of a row in the Customers data grid view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cusMgmtDgvCustomers_SelectionChanged(object sender, EventArgs e)
        {
            bool isRowSelected = cusMgmtDgvCustomers.SelectedRows.Count > 0;
            cusMgmtEditCustomerButton.Enabled = isRowSelected;
            cusMgmtDeleteCustomerButton.Enabled = isRowSelected;
        }

        /// <summary>
        /// Enables or disables the edit and delete buttons based on the selection of a row in the Appointments data grid view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cusMgmtDgvAppontments_SelectionChanged(object sender, EventArgs e)
        {
            bool isRowSelected = cusMgmtDgvAppontments.SelectedRows.Count > 0;
            cusMgmtEditAppointmentButton.Enabled = isRowSelected;
            cusMgmtDeleteAppointmentButton.Enabled = isRowSelected;
        }

        /// <summary>
        /// Triggers the reports form open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cusMgmtReportsButton_Click(object sender, EventArgs e)
        {
            var reportsForm = new ReportsForm();
            reportsForm.ShowDialog();
        }

        #endregion

        #region Upcoming appointments alerts handling

        /// <summary>
        /// Handle the event of the form to show an alert for upcoming appointments
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerManagementForm_Shown(object sender, EventArgs e)
        {
            ShowUpcomingAppointmentsAlert();
        }

        /// <summary>
        ///Method to show an alert for upcoming appointments
        /// </summary>
        private void ShowUpcomingAppointmentsAlert()
        {
            int userId = UserSession.UserId;
            List<AppointmentDetails> upcomingAppointments = _customerDataHandler.GetUpcomingAppointments(userId);

            if (upcomingAppointments.Count > 0)
            {
                StringBuilder appointmentAlert = new StringBuilder("You have the following appointments within the next 15 minutes:\n\n");
                foreach (var appointment in upcomingAppointments)
                {
                    appointmentAlert.AppendLine(
                        $"{appointment.Title} is about to start at {appointment.Start.ToShortTimeString()}");
                }
                MessageBox.Show(appointmentAlert.ToString(), "Upcoming Appointments", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("There are no upcoming appointments within the next 15 minutes.", "No Upcoming Appointments", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region Delete actions handling

        /// <summary>
        /// Triggers a delete operation on the selected customer on the Customer data grid view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cusMgmtDeleteCustomerButton_Click(object sender, EventArgs e)
        {
            if (cusMgmtDgvCustomers.SelectedRows.Count > 0)
            {
                int customerId = Convert.ToInt32(cusMgmtDgvCustomers.SelectedRows[0].Cells["customerId"].Value);

                var result = MessageBox.Show("Are you sure you want to delete the selected customer?", "Delete Customer",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        var deleteResult = _customerDataHandler.DeleteCustomer(customerId);
                        if (deleteResult)
                        {
                            MessageBox.Show("Customer deleted successfully.", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            LoadCustomers();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete customer.");
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show($"Database error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("No Customers to Delete", "No Customers", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Triggers the delete operation on the selected appointment on the Appointments data grid view
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

                    try
                    {
                        if (_customerDataHandler.DeleteAppointment(appointmentId))
                        {
                            MessageBox.Show("Appointment deleted successfully.", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            LoadAppointments();
                            UpdateCalendarHighlights();
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete the appointment. Please try again.");
                        }
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show($"Database error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("No Appointments Exist", "No Appoitnments", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion

        #region Calendar handling

        /// <summary>
        /// Function to handle the date change event on the calendar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cusMgmtAppointmentsCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            {
                // Prevent infinite loop when adjusting the calendar date
                if (_isProgrammaticallyAdjusting)
                    return;
                // If the selected date is a weekend, adjust it to the next weekday
                if (e.Start.DayOfWeek == DayOfWeek.Saturday || e.Start.DayOfWeek == DayOfWeek.Sunday)
                {
                    _isProgrammaticallyAdjusting = true;
                    DateTime nextWeekday = FindNextWeekday(e.Start);
                    cusMgmtAppointmentsCalendar.SelectionStart = nextWeekday;
                    cusMgmtAppointmentsCalendar.SelectionEnd = nextWeekday;
                    _isProgrammaticallyAdjusting = false;
                }

                // Load appointments for the selected date
                LoadAppointmentsForSelectedDate(cusMgmtAppointmentsCalendar.SelectionStart);
            }
        }

        /// <summary>
        /// Function to load appointments for the selected date from a calendar
        /// </summary>
        /// <param name="selectedDate"></param>
        private void LoadAppointmentsForSelectedDate(DateTime selectedDate)
        {
            var appointments = _customerDataHandler.GetAppointmentsByDate(selectedDate);
            cusMgmtDgvAppontments.DataSource = appointments;
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

        #endregion

        #region Search by customer name on the Appointments data grid view
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

        /// <summary>
        /// Triggers filtering appointments by customer name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cusMgmtSearchAppByCustomer_TextChanged(object sender, EventArgs e)
        {
            FilterAppointmentsByCustomerName(cusMgmtSearchAppByCustomer.Text);
        }


        /// <summary>
        /// Removes the default text in the search box when it is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cusMgmtSearchAppByCustomer_Enter(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (textBox.Text == "Search by Customer Name")
            {
                textBox.Text = "";
                textBox.ForeColor = Color.Black;
            }
        }

        /// <summary>
        /// Adds the default text in the search box when it loses focus and is empty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cusMgmtSearchAppByCustomer_Leave(object sender, EventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Search by Customer Name";
                textBox.ForeColor = Color.Gray;
            }
        }

        #endregion

        #region Datagrid view data loading

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
        /// Function that loads all appointments and refreshes the Appointments data grid view
        /// </summary>
        private void LoadAppointments()
        {
            var appointments = _customerDataHandler.GetAllAppointments();
            cusMgmtDgvAppontments.DataSource = appointments;
            cusMgmtDgvAppontments.Refresh();
        }

        #endregion

        #region End session and exit application

        /// <summary>
        /// Triggers the end session and closes the application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cusMgmtEndSessionButton_Click(object sender, EventArgs e)
        {
            ExitApplication();
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
        #endregion

        #region Debugging and misc

        /// <summary>
        /// Click event handler for the Show All Appointments button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cusMgmtShowAllAppts_Click(object sender, EventArgs e)
        {
            try
            {
                LoadAppointments();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured during the retrieval of appointments: {ex.Message}");
            }
        }

        /// <summary>
        /// Debug label to show current user
        /// </summary>
        private void LoadCurrentUser()
        {
            cusMgmCurrentUserlbl.Text = !string.IsNullOrEmpty(_currentUser) ? $"Welcome back, {_currentUser}." : "Unknown user";
        }


        #endregion

    }
}


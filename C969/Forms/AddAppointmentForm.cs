using C969.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C969.Models;
using MySql.Data.MySqlClient;

namespace C969.Forms
{
    public partial class AddAppointmentForm : Form
    {
        private readonly CustomerAppointmentsDataHandler _customerAppointmentsDataHandler;
        private readonly ReportsDataHandler _reportDataHandler;
        private readonly string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        private bool _ignoreEvent = false; //Flag to prevent infinite loop in DateTimePicker event handler
        private readonly TimeZoneInfo estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");


        public AddAppointmentForm()
        {
            InitializeComponent();
            _customerAppointmentsDataHandler = new CustomerAppointmentsDataHandler(_connString);
            _reportDataHandler = new ReportsDataHandler(_connString);
            this.addAppointmentStartDatePicker.ValueChanged += AddAppointmentStartDatePicker_ValueChanged;
            this.addAppointmentEndDatePicker.ValueChanged += AddAppointmentEndDatePicker_ValueChanged;
            this.addAppointmentStartTimeCombo.SelectedIndexChanged += AddAppointmentStartTimeCombo_SelectedIndexChanged;
            SetupTimeComboBoxes();
            LoadCustomerNames();
            DisplayCurrentUser();
            AdjustDefaultDates();
        }


        #region Date and Time Event Handlers

        /// <summary>
        /// Prevent user from selecting weekends for appointment start date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAppointmentStartDatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (_ignoreEvent) return;

            DateTimePicker picker = (DateTimePicker)sender;
            if (picker.Value.DayOfWeek == DayOfWeek.Saturday)
            {
                _ignoreEvent = true;
                MessageBox.Show("Appointments cannot be scheduled on weekends. Please select a weekday.");
                picker.Value = picker.Value.AddDays(2); // Adjust to next Monday
                _ignoreEvent = false;
            }
            else if (picker.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                _ignoreEvent = true;
                MessageBox.Show("Appointments cannot be scheduled on weekends. Please select a weekday.");
                picker.Value = picker.Value.AddDays(1); // Adjust to next Monday
                _ignoreEvent = false;
            }

            //Make sure the end date is the same as the start date (not before the start date)
            if (addAppointmentEndDatePicker.Value.Date != picker.Value.Date)
            {
                addAppointmentEndDatePicker.Value = picker.Value;
            }
        }

        /// <summary>
        /// Prevent user from selecting weekends for appointment end date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAppointmentEndDatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (_ignoreEvent) return;

            DateTimePicker picker = (DateTimePicker)sender;
            if (picker.Value.DayOfWeek == DayOfWeek.Saturday)
            {
                _ignoreEvent = true;
                MessageBox.Show("Appointments cannot be scheduled on weekends. Please select a weekday.");
                picker.Value = picker.Value.AddDays(2); // Adjust to next Monday
                _ignoreEvent = false;
            }
            else if (picker.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                _ignoreEvent = true;
                MessageBox.Show("Appointments cannot be scheduled on weekends. Please select a weekday.");
                picker.Value = picker.Value.AddDays(1); // Adjust to next Monday
                _ignoreEvent = false;
            }

            if (picker.Value.Date > addAppointmentStartDatePicker.Value.Date)
            {
                _ignoreEvent = true;
                MessageBox.Show("Appointment can't last more than the whole day. Consider splitting into several days.");
                picker.Value = addAppointmentStartDatePicker.Value;
                _ignoreEvent = false;
            }
            else if (picker.Value.Date < addAppointmentStartDatePicker.Value.Date)
            {
                _ignoreEvent = true;
                MessageBox.Show("Appointment can't end before it starts.");
                picker.Value = addAppointmentStartDatePicker.Value;
                _ignoreEvent = false;
            }

   
        }

        private void AddAppointmentStartTimeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEndTimeComboBox();
        }
        #endregion

        #region Time ComboBoxes logic
        /// <summary>
        /// Setup the time combo boxes for the appointment start and end times
        /// </summary>
        private void SetupTimeComboBoxes()
        {
            var userTimeZone = UserSession.CurrentTimeZone;

            // Define working hours in EST
            var estStartWork = new DateTime(1, 1, 1, 9, 0, 0); // 9 AM
            var estEndWork = new DateTime(1, 1, 1, 17, 0, 0); // 5 PM

            // Convert to user's time zone
            var userStartWork = TimeZoneHandler.ConvertToUserTimeZone(estStartWork, userTimeZone);
            var userEndWork = TimeZoneHandler.ConvertToUserTimeZone(estEndWork, userTimeZone);

            // Generate time slots based on the converted hours
            var timeSlots = GenerateTimeSlots(userStartWork, userEndWork);

            addAppointmentStartTimeCombo.Items.Clear();
            addAppointmentEndTimeCombo.Items.Clear();
            addAppointmentStartTimeCombo.Items.AddRange(timeSlots.ToArray());
            addAppointmentEndTimeCombo.Items.AddRange(timeSlots.ToArray());
            addAppointmentStartTimeCombo.SelectedIndex = 0;
            UpdateEndTimeComboBox();


        }

        /// <summary>
        /// Generate time slots for the appointment start and end times
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        private List<string> GenerateTimeSlots(DateTime startTime, DateTime endTime)
        {
            List<string> timeSlots = new List<string>();

            while (startTime < endTime)
            {
                timeSlots.Add(startTime.ToString("hh:mm tt"));
                startTime = startTime.AddMinutes(15); // gaps are 15 minutes
            }

            return timeSlots;
        }

        /// <summary>
        /// Update the end time combo box based on the selected start time
        /// </summary>
        private void UpdateEndTimeComboBox()
        {
            if (addAppointmentStartTimeCombo.SelectedIndex == -1) return;

            string selectedStartTime = addAppointmentStartTimeCombo.SelectedItem.ToString();
            DateTime startTime = DateTime.Parse(selectedStartTime, new CultureInfo("en-US"));

            // Clear current items and add only times that are later than the selected start time
            addAppointmentEndTimeCombo.Items.Clear();
            List<string> slots = GenerateTimeSlots(startTime, startTime.AddHours(8)); // 8 hours working period

            foreach (string slot in slots)
            {
                DateTime slotTime = DateTime.Parse(slot, new CultureInfo("en-US"));
                if (slotTime > startTime)
                {
                    addAppointmentEndTimeCombo.Items.Add(slot);
                }
            }

            addAppointmentEndTimeCombo.SelectedIndex = 0;


        }
        #endregion

        #region Save appointment logic
        public void SaveAppointment()
        {
            try
            {
                // Parse the selected date and time in the local time zone
                DateTime startDateTimeLocal = DateTime.Parse(
                    $"{addAppointmentStartDatePicker.Value.ToShortDateString()} {addAppointmentStartTimeCombo.SelectedItem}");
                DateTime endDateTimeLocal = DateTime.Parse(
                    $"{addAppointmentEndDatePicker.Value.ToShortDateString()} {addAppointmentEndTimeCombo.SelectedItem}");


                if (addAppointmentEndDatePicker.Value.Date != addAppointmentStartDatePicker.Value.Date)
                {
                    MessageBox.Show("End date cannot be different from start date. Please adjust the dates.");
                    return;
                }


                AppointmentDetails appointment = new AppointmentDetails
                {
                    Title = addAppointmentTitleText.Text,
                    Type = addAppointmentTypeText.Text,
                    Description = addAppointmentDescriptionText.Text,
                    Location = addAppointmentLocationText.Text,
                    Contact = addAppointmentContactText.Text,
                    Start = startDateTimeLocal,
                    End = endDateTimeLocal,
                    CustomerId = (int)addAppointmentCustomerNameCombo.SelectedValue,
                    UserId = UserSession.UserId,
                    CreatedBy = addAppointmentCurrentUserText.Text,
                    LastUpdateBy = addAppointmentCurrentUserText.Text,
                    Url = addAppointmentUrlText.Text,
                    CreateDate = DateTime.Now,

                };

                var overlappingAppointment = GetOverlappingAppointment(appointment);

                if (overlappingAppointment != null)
                {
                    MessageBox.Show($"The appointment '{appointment.Title}' overlaps with an existing appointment: \n" +
                                    $"Title: {overlappingAppointment.Title}\n" +
                                    $"Start: {overlappingAppointment.Start}\n" +
                                    $"End: {overlappingAppointment.End}\n" +
                                    "Please adjust the time.",
                        "Overlapping Appointment",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                if (_customerAppointmentsDataHandler.AddAppointment(appointment))
                {
                    MessageBox.Show("Appointment added successfully.");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to add appointment. Check the data and try again.");
                }
            }
            catch (MySqlException mse)
            {
                MessageBox.Show($"A database error occurred: {mse.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        /// <summary>
        /// Method to handle the Add Appointment button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addAppointmentSaveBtn_Click(object sender, EventArgs e)
        {
            SaveAppointment();
        }

        /// <summary>
        /// Button click event to cancel adding an appointment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addAppointmentCancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel adding the appointment?", "Cancel Appointment",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        /// <summary>
        /// Retrieve an overlapping appointment for the Customer if it exists
        /// </summary>
        /// <param name="newAppointment"></param>
        /// <returns></returns>
        private AppointmentDetails GetOverlappingAppointment(AppointmentDetails newAppointment)
        {
            var existingAppointments = _customerAppointmentsDataHandler.GetAppointmentsByCustomerName(addAppointmentCustomerNameCombo.Text);
            // Check if the new appointment overlaps with any existing appointments
            foreach (var appointment in existingAppointments)
            {
                if (newAppointment.CustomerId == appointment.CustomerId &&
                    newAppointment.Start <= appointment.End && newAppointment.End >= appointment.Start)
                {
                    return appointment;
                }
            }

            return null;
        }
        #endregion

        #region Misc (Default dates adjustment, Combos setup)
        /// <summary>
        /// Load the customer names into the addAppointmentCustomerNameCombo combo box
        /// </summary>
        private void LoadCustomerNames()
        {
            var customerData = _customerAppointmentsDataHandler.GetCustomerNameAndId();
            addAppointmentCustomerNameCombo.DataSource = new BindingSource(customerData, null);
            addAppointmentCustomerNameCombo.DisplayMember = "Value"; //this will show the customer name
            addAppointmentCustomerNameCombo.ValueMember = "Key"; //this will store the customer id

        }

        /// <summary>
        /// Display the current user in the form
        /// </summary>
        private void DisplayCurrentUser()
        {
            addAppointmentCurrentUserText.Text = UserSession.CurrentUser;
            addAppointmentCurrentUserText.ReadOnly = true;
        }


        /// <summary>
        /// Adjust the default dates for the appointment to the next available weekday, if the selection was on a weekend
        /// </summary>
        private void AdjustDefaultDates()
        {
            DateTime initialDate = DateTime.UtcNow;
            if (initialDate.DayOfWeek == DayOfWeek.Saturday)
            {
                initialDate = initialDate.AddDays(2);
            }
            else if (initialDate.DayOfWeek == DayOfWeek.Sunday)
            {
                initialDate = initialDate.AddDays(1);
            }

            addAppointmentStartDatePicker.Value = initialDate;
            addAppointmentEndDatePicker.Value = initialDate;
        }
    }
    #endregion


}

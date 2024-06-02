using C969.Controllers;
using C969.Models;
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
using Org.BouncyCastle.Asn1.Cmp;
using System.Globalization;

namespace C969.Forms
{
    public partial class EditAppointmentForm : Form
    {
        private readonly int _appointmentId;
        private readonly CustomerDataHandler _customerDataHandler;
        private readonly ReportsDataHandler _reportDataHandler;
        private readonly string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        private bool ignoreEvent = false; //Flag to prevent infinite loop in DateTimePicker event handler
        
        public EditAppointmentForm(int appointmentId)
        {
            InitializeComponent();
            _appointmentId = appointmentId;
            _customerDataHandler = new CustomerDataHandler(_connString);
            _reportDataHandler = new ReportsDataHandler(_connString);
            LoadAppointmentDetails(appointmentId);
            LoadCustomerNames();
            DisplayCurrentUser();
            editAppointmentStartDatePicker.ValueChanged += editAppointmentStartDatePicker_ValueChanged;
            editAppointmentEndDatePicker.ValueChanged += editAppointmentEndDatePicker_ValueChanged;
            editAppointmentStartTimeCombo.SelectedIndexChanged += EditAppointmentStartTimeCombo_SelectedIndexChanged;
        }


        private void SetupTimeComboBoxes(DateTime selectedStartTime, DateTime selectedEndTime)
        {
            var timeSlots = GenerateTimeSlots();
            editAppointmentStartTimeCombo.Items.Clear();
            editAppointmentStartTimeCombo.Items.AddRange(timeSlots.ToArray());
            editAppointmentEndTimeCombo.Items.Clear();
            editAppointmentEndTimeCombo.Items.AddRange(timeSlots.ToArray());
            editAppointmentStartTimeCombo.SelectedItem = selectedStartTime.ToString("hh:mm tt");
            editAppointmentEndTimeCombo.SelectedItem = selectedEndTime.ToString("hh:mm tt");

        }

        private List<string> GenerateTimeSlots()
        {
            List<string> timeSlots = new List<string>();
            DateTime startTime = DateTime.Today.AddHours(9); //start at 9am
            DateTime endTime = DateTime.Today.AddHours(17); //end at 5pm

            while (startTime < endTime)
            {
                timeSlots.Add(startTime.ToString("hh:mm tt"));
                startTime = startTime.AddMinutes(15); //gaps are 15 minutes
            }

            return timeSlots;
        }

        private void LoadCustomerNames()
        {
            var customerData = _customerDataHandler.GetCustomerNameAndId();
            editAppointmentCustomerNameCombo.DataSource = new BindingSource(customerData, null);
            editAppointmentCustomerNameCombo.DisplayMember = "Value";
            editAppointmentCustomerNameCombo.ValueMember = "Key";

        }

        private void DisplayCurrentUser()
        {
            editAppointmentCurrentUserText.Text = UserSession.CurrentUser;
            editAppointmentCurrentUserText.ReadOnly = true;
        }

        /// <summary>
        /// Function to load the appointment details into the form
        /// </summary>
        /// <param name="appointmentId"></param>
        private void LoadAppointmentDetails(int appointmentId)
        {
            var appointment = _customerDataHandler.GetAppointmentById(appointmentId);
            if (appointment != null)
            {
                editAppointmentTitleText.Text = appointment.Title;
                editAppointmentTypeText.Text = appointment.Type;
                editAppointmentDescriptionText.Text = appointment.Description;
                editAppointmentLocationText.Text = appointment.Location;
                editAppointmentContactText.Text = appointment.Contact;
                SetupTimeComboBoxes(appointment.Start, appointment.End);
                editAppointmentStartDatePicker.Value = appointment.Start;
                editAppointmentEndDatePicker.Value = appointment.End;
                editAppointmentCustomerNameCombo.SelectedValue = appointment.CustomerId;
                editAppointmentUrlText.Text = appointment.Url;
                editAppointmentCurrentUserText.Text = appointment.CreatedBy;
                editAppointmentCurrentUserText.ReadOnly = true;
                editAppointmentContactText.Text = appointment.Contact;
            }
        }

        private void editAppointmentSaveBtn_Click(object sender, EventArgs e)
        {
            if (ValidateAppointment())
            {
                try
                {
                    // Parse the selected date and time in the local time zone
                    DateTime startDateTimeLocal = DateTime.Parse(
                        $"{editAppointmentStartDatePicker.Value.ToShortDateString()} {editAppointmentStartTimeCombo.SelectedItem}");
                    DateTime endDateTimeLocal = DateTime.Parse(
                        $"{editAppointmentEndDatePicker.Value.ToShortDateString()} {editAppointmentEndTimeCombo.SelectedItem}");

                    // Convert local time to EST
                    TimeZoneInfo estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    DateTime estStart = TimeZoneInfo.ConvertTime(startDateTimeLocal, estTimeZone);
                    DateTime estEnd = TimeZoneInfo.ConvertTime(endDateTimeLocal, estTimeZone);

                    AppointmentDetails appointment = new AppointmentDetails
                    {
                        AppointmentId = _appointmentId,
                        Title = editAppointmentTitleText.Text,
                        Type = editAppointmentTypeText.Text,
                        Description = editAppointmentDescriptionText.Text,
                        Location = editAppointmentLocationText.Text,
                        Contact = editAppointmentContactText.Text,
                        Start = estStart,
                        End = estEnd,
                        CustomerId = (int)editAppointmentCustomerNameCombo.SelectedValue,
                        UserId = UserSession.UserId,
                        CreatedBy = editAppointmentCurrentUserText.Text,
                        LastUpdate = DateTime.UtcNow,
                        LastUpdateBy = editAppointmentCurrentUserText.Text,
                        Url = editAppointmentUrlText.Text

                    };

                    if (IsOverlappingAppointment(appointment))
                    {
                        MessageBox.Show(
                            $"Appointment overlaps with an existing appointment: {appointment.Title}.\\n Please adjust the time.");
                        return;
                    }

                    if (_customerDataHandler.UpdateAppointment(appointment))
                    {
                        MessageBox.Show("Appointment updated successfully.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update appointment. Check the data and try again.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while updating the appointment: {ex.Message}");
                }

            }
        }

        /// <summary>
        /// Function to validate the appointment data before saving
        /// </summary>
        /// <returns></returns>
        private bool ValidateAppointment()
        {
            {

                DateTime startDateTime = DateTime.Parse($"{editAppointmentStartDatePicker.Value.ToShortDateString()} {editAppointmentStartTimeCombo.SelectedItem}");
                DateTime endDateTime = DateTime.Parse($"{editAppointmentEndDatePicker.Value.ToShortDateString()} {editAppointmentEndTimeCombo.SelectedItem}");

                if (endDateTime <= startDateTime)
                {
                    MessageBox.Show("End time must be after start time.");
                    return false;
                }

                if (editAppointmentEndDatePicker.Value.Date != editAppointmentStartDatePicker.Value.Date)
                {
                    MessageBox.Show("End date cannot be different from start date. Please adjust the dates.");
                    return false;
                }

                return true;
            }   
        }

        private void editAppointmentStartDatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (ignoreEvent) return;

            DateTimePicker picker = (DateTimePicker)sender;
            if (picker.Value.DayOfWeek == DayOfWeek.Saturday)
            {
                ignoreEvent = true;
                MessageBox.Show("Appointments cannot be scheduled on weekends. Please select a weekday.");
                picker.Value = picker.Value.AddDays(2); // Adjust to next Monday
                ignoreEvent = false;
            }
            else if (picker.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                ignoreEvent = true;
                MessageBox.Show("Appointments cannot be scheduled on weekends. Please select a weekday.");
                picker.Value = picker.Value.AddDays(1); // Adjust to next Monday
                ignoreEvent = false;
            }

            if (editAppointmentEndDatePicker.Value.Date != picker.Value.Date)
            {
                ignoreEvent = true;
                MessageBox.Show("End date cannot be different from start date. Adjusting to match the start date.");
                editAppointmentEndDatePicker.Value = picker.Value;
                ignoreEvent = false;
            }
        }
        private void editAppointmentEndDatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (ignoreEvent) return;

            DateTimePicker picker = (DateTimePicker)sender;
            if (picker.Value.DayOfWeek == DayOfWeek.Saturday)
            {
                ignoreEvent = true;
                MessageBox.Show("Appointments cannot be scheduled on weekends. Please select a weekday.");
                picker.Value = picker.Value.AddDays(2); // Adjust to next Monday
                ignoreEvent = false;
            }
            else if (picker.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                ignoreEvent = true;
                MessageBox.Show("Appointments cannot be scheduled on weekends. Please select a weekday.");
                picker.Value = picker.Value.AddDays(1); // Adjust to next Monday
                ignoreEvent = false;
            }

            if (picker.Value.Date != editAppointmentStartDatePicker.Value.Date)
            {
                ignoreEvent = true;
                MessageBox.Show("End date cannot be different from start date. Adjusting to match the start date.");
                picker.Value = editAppointmentStartDatePicker.Value;
                ignoreEvent = false;
            }
        }

        private void EditAppointmentStartTimeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEndTimeComboBox();
        }

        private void UpdateEndTimeComboBox()
        {
            if(editAppointmentStartTimeCombo.SelectedIndex == -1) return;
            string selectedStartTime = editAppointmentStartTimeCombo.SelectedItem.ToString();
            DateTime startTime = DateTime.Parse(selectedStartTime, new CultureInfo("en-US"));

            // Clear current items and add only times that are later than the selected start time
            editAppointmentEndTimeCombo.Items.Clear();
            List<string> slots = GenerateTimeSlots();

            foreach (string slot in slots)
            {
                DateTime slotTime = DateTime.Parse(slot, new CultureInfo("en-US"));
                if (slotTime > startTime)
                {
                    editAppointmentEndTimeCombo.Items.Add(slot);
                }
            }

            editAppointmentEndTimeCombo.SelectedIndex = 0;
        }

        private void editAppointmentCancelBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to cancel editing the appointment?", "Cancel Edit",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private bool IsOverlappingAppointment(AppointmentDetails newAppointment)
        {
            var existingAppointments = _reportDataHandler.GetAppointmentsByUserId(UserSession.UserId);
            return existingAppointments.Any(existingAppointment =>
                newAppointment.AppointmentId != existingAppointment.AppointmentId && // Exclude the current appointment being edited
                newAppointment.Start < existingAppointment.End &&
                newAppointment.End > existingAppointment.Start);
        }

    }
}

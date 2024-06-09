﻿using C969.Controllers;
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
using MySql.Data.MySqlClient;

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
            LoadAppointmentData();
            LoadCustomerNames();
            DisplayCurrentUser();
            editAppointmentStartDatePicker.ValueChanged += editAppointmentStartDatePicker_ValueChanged;
            editAppointmentEndDatePicker.ValueChanged += editAppointmentEndDatePicker_ValueChanged;
            editAppointmentStartTimeCombo.SelectedIndexChanged += EditAppointmentStartTimeCombo_SelectedIndexChanged;
        }

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

            editAppointmentStartTimeCombo.Items.Clear();
            editAppointmentEndTimeCombo.Items.Clear();
            editAppointmentStartTimeCombo.Items.AddRange(timeSlots.ToArray());
            editAppointmentEndTimeCombo.Items.AddRange(timeSlots.ToArray());
            editAppointmentStartTimeCombo.SelectedIndex = 0;
            UpdateEndTimeComboBox();

        }

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

        private void LoadAppointmentData()
        {
            var appointmentDetails = _customerDataHandler.GetAppointmentById(_appointmentId);
            if (appointmentDetails != null)
            {
                editAppointmentTitleText.Text = appointmentDetails.Title;
                editAppointmentTypeText.Text = appointmentDetails.Type;
                editAppointmentDescriptionText.Text = appointmentDetails.Description;
                editAppointmentLocationText.Text = appointmentDetails.Location;
                editAppointmentContactText.Text = appointmentDetails.Contact;
                editAppointmentUrlText.Text = appointmentDetails.Url;


                // Set date and time values
                editAppointmentStartDatePicker.Value = appointmentDetails.Start.Date;
                editAppointmentEndDatePicker.Value = appointmentDetails.End.Date;
                SetupTimeComboBoxes();
                editAppointmentStartTimeCombo.SelectedItem = appointmentDetails.Start.ToString("hh:mm tt");
                editAppointmentEndTimeCombo.SelectedItem = appointmentDetails.End.ToString("hh:mm tt");


                // Set customer
                editAppointmentCustomerNameCombo.DataSource = new BindingSource(_customerDataHandler.GetCustomerNameAndId(), null);
                editAppointmentCustomerNameCombo.DisplayMember = "Value";
                editAppointmentCustomerNameCombo.ValueMember = "Key";
                editAppointmentCustomerNameCombo.SelectedValue = appointmentDetails.CustomerId;

                // Set current user (readonly)
                editAppointmentCurrentUserText.Text = UserSession.CurrentUser;
                editAppointmentCurrentUserText.ReadOnly = true;
            }
            else
            {
                MessageBox.Show("Appointment not found.");
                this.Close();
            }
        }

        private void editAppointmentSaveBtn_Click(object sender, EventArgs e)
        {
           SaveAppointmentEdit();
        }

        public void SaveAppointmentEdit()
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

                    AppointmentDetails appointment = new AppointmentDetails
                    {
                        AppointmentId = _appointmentId,
                        Title = editAppointmentTitleText.Text,
                        Type = editAppointmentTypeText.Text,
                        Description = editAppointmentDescriptionText.Text,
                        Location = editAppointmentLocationText.Text,
                        Contact = editAppointmentContactText.Text,
                        Start = startDateTimeLocal,
                        End = endDateTimeLocal,
                        CustomerId = (int)editAppointmentCustomerNameCombo.SelectedValue,
                        UserId = UserSession.UserId,
                        CreatedBy = editAppointmentCurrentUserText.Text,
                        LastUpdate = DateTime.UtcNow,
                        LastUpdateBy = editAppointmentCurrentUserText.Text,
                        Url = editAppointmentUrlText.Text
                    };

                    var overlappingAppointment = GetOverlappingAppointment(appointment, _appointmentId);

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
                catch (FormatException fe)
                {
                    MessageBox.Show($"Date/Time format is incorrect: {fe.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (InvalidCastException ice)
                {
                    MessageBox.Show($"Invalid data type: {ice.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (NullReferenceException nre)
                {
                    MessageBox.Show($"A required field is missing: {nre.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        }

        private AppointmentDetails GetOverlappingAppointment(AppointmentDetails newAppointment, int excludeAppointmentId = 0)
        {
            var existingAppointments = _customerDataHandler.GetAppointmentsByCustomerName(editAppointmentCustomerNameCombo.Text);

            foreach (var appointment in existingAppointments)
            {
                if (newAppointment.CustomerId == appointment.CustomerId &&
                    newAppointment.Start < appointment.End && newAppointment.End > appointment.Start &&
                    appointment.AppointmentId != excludeAppointmentId)
                {
                    return appointment;
                }
            }

            return null;
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
            if (editAppointmentStartTimeCombo.SelectedIndex == -1) return;

            string selectedStartTime = editAppointmentStartTimeCombo.SelectedItem.ToString();
            DateTime startTime = DateTime.Parse(selectedStartTime, new CultureInfo("en-US"));

            // Clear current items and add only times that are later than the selected start time
            editAppointmentEndTimeCombo.Items.Clear();
            List<string> slots = GenerateTimeSlots(startTime, startTime.AddHours(8)); // 8 hours working period

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


    }
}

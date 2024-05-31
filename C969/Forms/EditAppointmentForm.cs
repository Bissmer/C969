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

namespace C969.Forms
{
    public partial class EditAppointmentForm : Form
    {
        private readonly int _appointmentId;
        private readonly CustomerDataHandler _customerDataHandler;
        private readonly string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        private bool ignoreEvent = false; //Flag to prevent infinite loop in DateTimePicker event handler
        
        public EditAppointmentForm(int appointmentId)
        {
            InitializeComponent();
            _appointmentId = appointmentId;
            _customerDataHandler = new CustomerDataHandler(_connString);
            LoadAppointmentDetails(appointmentId);
            LoadCustomerNames();
            DisplayCurrentUser();
            editAppointmentStartDatePicker.ValueChanged += editAppointmentStartDatePicker_ValueChanged;
            editAppointmentEndDatePicker.ValueChanged += editAppointmentEndDatePicker_ValueChanged;
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
                AppointmentDetails appointment = new AppointmentDetails
                {
                    AppointmentId = _appointmentId,
                    Title = editAppointmentTitleText.Text,
                    Type = editAppointmentTypeText.Text,
                    Description = editAppointmentDescriptionText.Text,
                    Location = editAppointmentLocationText.Text,
                    Contact = editAppointmentContactText.Text,
                    Start = DateTime.Parse(
                        $"{editAppointmentStartDatePicker.Value.ToShortDateString()} {editAppointmentStartTimeCombo.SelectedItem}"),
                    End = DateTime.Parse(
                        $"{editAppointmentEndDatePicker.Value.ToShortDateString()} {editAppointmentEndTimeCombo.SelectedItem}"),
                    CustomerId = (int)editAppointmentCustomerNameCombo.SelectedValue,
                    UserId = UserSession.UserId,
                    CreatedBy = editAppointmentCurrentUserText.Text,
                    LastUpdate = DateTime.UtcNow,
                    LastUpdateBy = editAppointmentCurrentUserText.Text,
                    Url = editAppointmentUrlText.Text

                };
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
        }

        /// <summary>
        /// Function to validate the appointment data before saving
        /// </summary>
        /// <returns></returns>
        private bool ValidateAppointment()
        {
            { 
                if (string.IsNullOrWhiteSpace(editAppointmentTitleText.Text))
                {
                    MessageBox.Show("Title is required.");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(editAppointmentTypeText.Text))
                {
                    MessageBox.Show("Type is required.");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(editAppointmentDescriptionText.Text))
                {
                    MessageBox.Show("Description is required.");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(editAppointmentLocationText.Text))
                {
                    MessageBox.Show("Location is required.");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(editAppointmentContactText.Text))
                {
                    MessageBox.Show("Contact is required.");
                    return false;
                }

                if (editAppointmentCustomerNameCombo.SelectedIndex == -1)
                {
                    MessageBox.Show("Customer is required.");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(editAppointmentUrlText.Text))
                {
                    MessageBox.Show("URL is required.");
                    return false;
                }

                DateTime startDateTime = DateTime.Parse($"{editAppointmentStartDatePicker.Value.ToShortDateString()} {editAppointmentStartTimeCombo.SelectedItem}");
                DateTime endDateTime = DateTime.Parse($"{editAppointmentEndDatePicker.Value.ToShortDateString()} {editAppointmentEndTimeCombo.SelectedItem}");
                if (endDateTime <= startDateTime)
                {
                    MessageBox.Show("End time must be after start time.");
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
        }
    }
}

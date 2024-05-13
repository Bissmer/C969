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

namespace C969.Forms
{
    public partial class AddAppointmentForm : Form
    {
        private CustomerDataHandler _customerDataHandler;
        private string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        private bool ignoreEvent = false; //Flag to prevent infinite loop in DateTimePicker event handler

        public AddAppointmentForm()
        {
            InitializeComponent();
            _customerDataHandler = new CustomerDataHandler(_connString);
            this.addAppointmentStartDatePicker.ValueChanged += AddAppointmentStartDatePicker_ValueChanged;
            this.addAppointmentEndDatePicker.ValueChanged += AddAppointmentEndDatePicker_ValueChanged;
            this.addAppointmentStartTimeCombo.SelectedIndexChanged += AddAppointmentStartTimeCombo_SelectedIndexChanged;
            SetupTimeComboBoxes();
            LoadCustomerNames();
            DisplayCurrentUser();
        }

        /// <summary>
        /// Prevent user from selecting weekends for appointment start date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAppointmentStartDatePicker_ValueChanged(object sender, EventArgs e)
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

        /// <summary>
        /// Prevent user from selecting weekends for appointment end date
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAppointmentEndDatePicker_ValueChanged(object sender, EventArgs e)
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

        /// <summary>
        /// Setup the time combo boxes for the appointment start and end times
        /// </summary>
        private void SetupTimeComboBoxes()
        {
            var timeSlots = GenerateTimeSlots();
            addAppointmentStartTimeCombo.Items.AddRange(timeSlots.ToArray());
            addAppointmentEndTimeCombo.SelectedItem = timeSlots.First();
            UpdateEndTimeComboBox();


        }

        /// <summary>
        /// Generate time slots for the appointment time combo boxes
        /// </summary>
        /// <returns></returns>
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

        private void AddAppointmentStartTimeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEndTimeComboBox();
        }

        /// <summary>
        /// Update the end time combo box based on the selected start time
        /// </summary>
        private void UpdateEndTimeComboBox()
        {
            if (addAppointmentStartTimeCombo.SelectedIndex == -1) return;

            string selectedStartTime = addAppointmentStartTimeCombo.SelectedItem.ToString();
            DateTime startTime = DateTime.Parse(selectedStartTime);

            // Clear current items and add only times that are later than the selected start time
            addAppointmentEndTimeCombo.Items.Clear();
            List<string> slots = GenerateTimeSlots();

            foreach (string slot in slots)
            {
                DateTime slotTime = DateTime.Parse(slot);
                if (slotTime > startTime)
                {
                    addAppointmentEndTimeCombo.Items.Add(slot);
                }
            }

            addAppointmentEndTimeCombo.SelectedIndex = 0;


        }

        private void LoadCustomerNames()
        {
            var customerData = _customerDataHandler.GetCustomerNameAndId();
            addAppointmentCustomerNameCombo.DataSource = new BindingSource(customerData, null);
            addAppointmentCustomerNameCombo.DisplayMember = "Value"; //this will show the customer name
            addAppointmentCustomerNameCombo.ValueMember = "Key"; //this will store the customer id

        }

        private void DisplayCurrentUser()
        {
            addAppointmentCurrentUserText.Text = UserSession.CurrentUser;
            addAppointmentCurrentUserText.ReadOnly = true;
        }

        public void SaveAppointment()
        {
            AppointmentDetails appointment = new AppointmentDetails
            {
                Title = addAppointmentTitleText.Text,
                Type = addAppointmentTypeText.Text,
                Description = addAppointmentDescriptionText.Text,
                Location = addAppointmentLocationText.Text,
                Contact = addAppointmentContactText.Text,
                Start = DateTime.Parse(
                    $"{addAppointmentStartDatePicker.Value.ToShortDateString()} {addAppointmentStartTimeCombo.SelectedItem}"),
                End = DateTime.Parse(
                    $"{addAppointmentEndDatePicker.Value.ToShortDateString()} {addAppointmentEndTimeCombo.SelectedItem}"),
                CustomerId = (int)addAppointmentCustomerNameCombo.SelectedValue,
                UserId = UserSession.UserId,
                CreatedBy = addAppointmentCurrentUserText.Text,
                LastUpdateBy = addAppointmentCurrentUserText.Text,
                Url = addAppointmentUrlText.Text

            };

            if (_customerDataHandler.AddAppointment(appointment))
            {
                MessageBox.Show("Appointment added successfully.");
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed to add appointment. Check the data and try again.");
            }
        }

        private void addAppointmentSaveBtn_Click(object sender, EventArgs e)
        {
            SaveAppointment();
        }

        private void addAppointmentCancelBtn_Click(object sender, EventArgs e)
        {
           DialogResult result = MessageBox.Show("Are you sure you want to cancel adding the appointment?", "Cancel Appointment",
                              MessageBoxButtons.YesNo, MessageBoxIcon.Question);
           if (result == DialogResult.Yes)
           {
                this.Close();
           }
        }
    }
}

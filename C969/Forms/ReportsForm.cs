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
    
    public partial class ReportsForm : Form
    {

        private CustomerDataHandler _customerDataHandler;
        private ReportsDataHandler _reportsDataHandler;
        private string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;
        
        public ReportsForm()
        {
            InitializeComponent();
            _reportsDataHandler = new ReportsDataHandler(_connString);
            _customerDataHandler = new CustomerDataHandler(_connString);
            reportsFormUsersCombo.SelectedIndexChanged += reportsFormUsersCombo_SelectedIndexChanged;
            this.Load += ReportsForm_Load;
        }

        private void ReportsForm_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            var users = _reportsDataHandler.GetAllUsers();
            reportsFormUsersCombo.DataSource = users;
            reportsFormUsersCombo.DisplayMember = "userName";
            reportsFormUsersCombo.ValueMember = "userId";
        }

        private void reportsFormUsersCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reportsFormUsersCombo.SelectedValue != null)
            {
                int selectedUserId = (int)reportsFormUsersCombo.SelectedValue;
                LoadAppointmentsByUser(selectedUserId);
            }
        }

        private void LoadAppointmentsByUser(int userId)
        {
            var appointments = _reportsDataHandler.GetAppointmentsByUserId(userId);
            reportsFormDgvAppointmentsByUser.DataSource = appointments;
        }

        private void ConfigureAppointmentsDataGridView()
        {
            reportsFormDgvAppointmentsByUser.AutoGenerateColumns = false;

            reportsFormDgvAppointmentsByUser.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "AppointmentId",
                HeaderText = "ID",
                ReadOnly = true
            });

            reportsFormDgvAppointmentsByUser.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Title",
                HeaderText = "Title"
            });

            reportsFormDgvAppointmentsByUser.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Description",
                HeaderText = "Description"
            });

            reportsFormDgvAppointmentsByUser.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Location",
                HeaderText = "Location"
            });

            reportsFormDgvAppointmentsByUser.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Contact",
                HeaderText = "Contact"
            });

            reportsFormDgvAppointmentsByUser.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Start",
                HeaderText = "Start Time"
            });

            reportsFormDgvAppointmentsByUser.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "End",
                HeaderText = "End Time"
            });

        }
    }
}

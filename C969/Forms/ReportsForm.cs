using C969.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C969.Forms
{
    
    public partial class ReportsForm : Form
    {

        private readonly ReportsDataHandler _reportsDataHandler;
        private readonly string _connString = ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;


        
        public ReportsForm()
        {
            InitializeComponent();
            _reportsDataHandler = new ReportsDataHandler(_connString);
            this.Load += ReportsForm_Load;
            this.reportsFormDownloadSchedulesByUser.Click += reportsFormDownloadSchedulesByUser_Click;
            reportsFormUsersCombo.SelectedIndexChanged += reportsFormUsersCombo_SelectedIndexChanged;
            reportsFormDownloadAppointmentsByMonth.Click += reportsFormDownloadAppointmentsByMonth_Click;
            reportsFormCountriesCombo.SelectedIndexChanged += reportsFormCountriesCombo_SelectedIndexChanged;
            
        }

        private void ReportsForm_Load(object sender, EventArgs e)
        {
            LoadUsers();
            LoadCountries();
            LoadCustomerCountByCountry();
            ConfigureAppointmentsDataGridView();
            ConfigureMonthlyReportDataGridView();
            ConfigureCustomerCountByCountryCityDataGridView();
            LoadDefaultAppointments();
            LoadMonthlyReport();
        }


        #region Report DataGridViews Columns Configuration

        /// <summary>
        /// Method to configure tables in Schedules By User DGV
        /// </summary>
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

        /// <summary>
        /// Method to configure tables in Appointments By Months DGV
        /// </summary>
        private void ConfigureMonthlyReportDataGridView()
        {
            reportsFormDgvAppointmentsByMonth.AutoGenerateColumns = false;
            reportsFormDgvAppointmentsByMonth.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            reportsFormDgvAppointmentsByMonth.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MonthName",
                HeaderText = "Month"
            });

            reportsFormDgvAppointmentsByMonth.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Type",
                HeaderText = "Type"
            });

            reportsFormDgvAppointmentsByMonth.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Count",
                HeaderText = "Count"
            });
        }

        /// <summary>
        /// Method to configure tables in Customer Appointments Breakdown DGV
        /// </summary>
        private void ConfigureCustomerCountByCountryCityDataGridView()
        {
            reportsFormDgvCustomerCountByCountry.AutoGenerateColumns = false;
            reportsFormDgvCustomerCountByCountry.Columns.Clear();
            reportsFormDgvCustomerCountByCountry.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            reportsFormDgvCustomerCountByCountry.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Name",
                HeaderText = "Name",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            reportsFormDgvCustomerCountByCountry.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CusCount",
                HeaderText = "Customer Count",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
        }

        #endregion

        #region Reports Export Methods
        /// <summary>
        /// Load the appointments for the selected user and export them to a CSV file
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="fileName"></param>
        private void ExportReportsToCsv(DataGridView dgv, string fileName)
        {
            try
            {
                var sb = new StringBuilder();
                var headers = dgv.Columns.Cast<DataGridViewColumn>();

                sb.AppendLine(string.Join(",", headers.Select(column => "\"" + column.HeaderText + "\"").ToArray())); // Write the headers to the file

                // Write the data to the file
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    var cells = row.Cells.Cast<DataGridViewCell>();
                    sb.AppendLine(string.Join(",", cells.Select(cell => "\"" + cell.Value?.ToString() + "\"").ToArray()));
                }

                File.WriteAllText(fileName, sb.ToString());
                MessageBox.Show("File saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while exporting data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Schedules by User report controls and logic

        /// <summary>
        /// Method to load users data into the user combo box
        /// </summary>
        private void LoadUsers()
        {
            var users = _reportsDataHandler.GetAllUsers();
            reportsFormUsersCombo.DataSource = users;
            reportsFormUsersCombo.DisplayMember = "userName";
            reportsFormUsersCombo.ValueMember = "userId";
        }

        /// <summary>
        /// Handler to load Schedules by User when a user is selected from the combo box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reportsFormUsersCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (reportsFormUsersCombo.SelectedValue != null && int.TryParse(reportsFormUsersCombo.SelectedValue.ToString(), out int selectedUserId))
                {
                    LoadAppointmentsByUser(selectedUserId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Method to load appointments for a chosen user
        /// </summary>
        /// <param name="userId"></param>
        private void LoadAppointmentsByUser(int userId)
        {
            var appointments = _reportsDataHandler.GetAppointmentsByUserId(userId);
            reportsFormDgvAppointmentsByUser.DataSource = appointments;
            reportsFormDgvAppointmentsByUser.Refresh();
        }

        /// <summary>
        /// Load the default appointments for the first user in the combo box
        /// </summary>
        private void LoadDefaultAppointments()
        {
            if (reportsFormUsersCombo.Items.Count > 0)
            {
                reportsFormUsersCombo.SelectedIndex = 0; // Select the first item by default
                int selectedUserId = (int)reportsFormUsersCombo.SelectedValue;
                LoadAppointmentsByUser(selectedUserId);
            }
        }

        /// <summary>
        /// Save the Schedules by User report to a CSV file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reportsFormDownloadSchedulesByUser_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Save schedules by user"
            };


            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportReportsToCsv(reportsFormDgvAppointmentsByUser, saveFileDialog.FileName);
            }
        }
        #endregion

        #region Appointments by Months report controls and logic

        /// <summary>
        /// Method to load the appointments by month report when the Report form is loaded
        /// </summary>
        private void LoadMonthlyReport()
        {
            var monthlyReport = _reportsDataHandler.GetAppointmentTypesByMonth();
            reportsFormDgvAppointmentsByMonth.DataSource = monthlyReport;
            reportsFormDgvAppointmentsByMonth.Refresh();
        }

        /// <summary>
        /// Handler to save the Appointments by Month report to a CSV file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reportsFormDownloadAppointmentsByMonth_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Save appointments by month"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportReportsToCsv(reportsFormDgvAppointmentsByMonth, saveFileDialog.FileName);
            }
        }

        #endregion

        #region Customer Appointments Breakdown report controls and logic

        /// <summary>
        /// Method to load the customer list into Customer Appointments Breakdown countries combo box
        /// </summary>
        private void LoadCountries()
        {
            var countries = _reportsDataHandler.GetAllCountries();
            reportsFormCountriesCombo.DataSource = countries;
            reportsFormCountriesCombo.DisplayMember = "CountryName";
            reportsFormCountriesCombo.ValueMember = "CountryId";
        }

        /// <summary>
        /// Method to load the customer count by country report when the Report form is loaded
        /// </summary>
        private void LoadCustomerCountByCountry()
        {
            var customerCounts = _reportsDataHandler.GetCustomerCountByCountry();
            reportsFormDgvCustomerCountByCountry.DataSource = customerCounts;
            reportsFormDgvCustomerCountByCountry.Refresh();

        }

        /// <summary>
        /// Handler that loads the report depending on the selected country
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reportsFormCountriesCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reportsFormCountriesCombo.SelectedValue != null && int.TryParse(reportsFormCountriesCombo.SelectedValue.ToString(), out int selectedCountryId))
            {
                LoadCustomerCountByCity(selectedCountryId);
            }
        }

        /// <summary>
        /// Method to load the customer count by city
        /// </summary>
        /// <param name="countryId"></param>
        private void LoadCustomerCountByCity(int countryId)
        {
            var customerCounts = _reportsDataHandler.GetCustomerCountByCity(countryId);
            reportsFormDgvCustomerCountByCountry.DataSource = customerCounts;
        }

        #endregion






















    }
}

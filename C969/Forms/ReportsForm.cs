using C969.Controllers;
using C969.Models;
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
        private readonly CustomerAppointmentsDataHandler _customerAppointmentsDataHandler;

        private readonly string _connString =
            ConfigurationManager.ConnectionStrings["DbConnectionString"].ConnectionString;

        private TimeZoneInfo _userTimeZone = UserSession.CurrentTimeZone;
        private List<AppointmentCountByCustomer> _appointmentCountsByCustomer;
        private string _timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");



        public ReportsForm()
        {
            _customerAppointmentsDataHandler = new CustomerAppointmentsDataHandler(_connString);
            InitializeComponent();
            _reportsDataHandler = new ReportsDataHandler(_connString);
            this.Load += ReportsForm_Load;

        }

        private void ReportsForm_Load(object sender, EventArgs e)
        {
            ConfigureAppointmentsDataGridView();
            ConfigureMonthlyReportDataGridView();
            ConfigureAppointmentsByCountryDataGridView();
            LoadSchedulesByUser();
            LoadMonthlyReport();
            LoadAppointmentCountsByCustomer();
        }


        #region Report DataGridViews Custom Columns Configuration

        /// <summary>
        /// Method to configure tables in Schedules By User DGV
        /// </summary>
        private void ConfigureAppointmentsDataGridView()
        {
            reportsFormDgvAppointmentsByUser.DataSource = null;
            reportsFormDgvAppointmentsByUser.AutoGenerateColumns = false;
            reportsFormDgvAppointmentsByUser.Columns.Clear();
            reportsFormDgvAppointmentsByUser.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            reportsFormDgvAppointmentsByUser.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "UserName",
                HeaderText = "User Name",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            reportsFormDgvAppointmentsByUser.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Title",
                HeaderText = "Appointment Title",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            reportsFormDgvAppointmentsByUser.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Start",
                HeaderText = "Start Time",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            reportsFormDgvAppointmentsByUser.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "End",
                HeaderText = "End Time",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
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
        private void ConfigureAppointmentsByCountryDataGridView()
        {
            reportsFormDgvAppointmentsByCustomer.AutoGenerateColumns = false;
            reportsFormDgvAppointmentsByCustomer.Columns.Clear();
            reportsFormDgvAppointmentsByCustomer.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            reportsFormDgvAppointmentsByCustomer.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CustomerName",
                HeaderText = "Customer Name",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            reportsFormDgvAppointmentsByCustomer.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "AppointmentsCount",
                HeaderText = "Appointments Count",
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

                sb.AppendLine(string.Join(",",
                    headers.Select(column => "\"" + column.HeaderText + "\"")
                        .ToArray())); // Write the headers to the file

                // Write the data to the file
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    var cells = row.Cells.Cast<DataGridViewCell>();
                    sb.AppendLine(
                        string.Join(",", cells.Select(cell => "\"" + cell.Value?.ToString() + "\"").ToArray()));
                }

                File.WriteAllText(fileName, sb.ToString());
                MessageBox.Show("File saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while exporting data: {ex.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Schedules by User report controls and logic

        private void LoadSchedulesByUser()
        {
            var schedulesByUser = _reportsDataHandler.GetSchedulesByUser();
            reportsFormDgvAppointmentsByUser.DataSource = schedulesByUser;
            reportsFormDgvAppointmentsByUser.Refresh();
        }

        /// <summary>
        /// Save the Schedules by User report to a CSV file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reportsFormDownloadSchedulesByUser_Click(object sender, EventArgs e)
        {
            string defaultFileName = $"SchedulesByUserReport_{_timestamp}.csv";
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Save schedules by user",
                FileName = defaultFileName
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
            string defaultFileName = $"AppointmentsByMonthReport_{_timestamp}.csv";
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Save appointments by month",
                FileName = defaultFileName
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportReportsToCsv(reportsFormDgvAppointmentsByMonth, saveFileDialog.FileName);
            } 
        }

        #endregion

        #region Customer Appointments Breakdown report controls and logic


        /// <summary>
        /// Method to load the appointments count by customer report when the Report form is loaded
        /// </summary>
        private void LoadAppointmentCountsByCustomer()
        {
            _appointmentCountsByCustomer = _reportsDataHandler.GetAppointmentsCountByCustomer();
            reportsFormDgvAppointmentsByCustomer.DataSource =
                new BindingList<AppointmentCountByCustomer>(_appointmentCountsByCustomer);
        }

        private void reportsFormDownloadCustomerCountByCountryCity_Click(object sender, EventArgs e)
        {


            string defaultFileName = $"AppointmentsByCustomerReport_{_timestamp}.csv";

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Save appointments by month",
                FileName = defaultFileName
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportReportsToCsv(reportsFormDgvAppointmentsByCustomer, saveFileDialog.FileName);
            }
        }

        #endregion

        #region Reports Form Quit Button
        private void reportsFormQuitBtn_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close the Reports Form?", "Close the Reports Form", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
        #endregion

    }
}

namespace C969.Forms
{
    partial class ReportsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.reportsFormTitleLabel = new System.Windows.Forms.Label();
            this.reportsFormUsersCombo = new System.Windows.Forms.ComboBox();
            this.reportsFormUsersComboLabel = new System.Windows.Forms.Label();
            this.reportsFormDgvAppointmentsByUser = new System.Windows.Forms.DataGridView();
            this.reportsFormSchedulesByUserLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.reportsFormDownloadSchedulesByUser = new System.Windows.Forms.Button();
            this.reportsFormAppointmentsByMonthLabel = new System.Windows.Forms.Label();
            this.reportsFormDgvAppointmentsByMonth = new System.Windows.Forms.DataGridView();
            this.reportsFormDownloadAppointmentsByMonth = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.reportsFormDgvAppointmentsByUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportsFormDgvAppointmentsByMonth)).BeginInit();
            this.SuspendLayout();
            // 
            // reportsFormTitleLabel
            // 
            this.reportsFormTitleLabel.AutoSize = true;
            this.reportsFormTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reportsFormTitleLabel.Location = new System.Drawing.Point(12, 31);
            this.reportsFormTitleLabel.Name = "reportsFormTitleLabel";
            this.reportsFormTitleLabel.Size = new System.Drawing.Size(75, 24);
            this.reportsFormTitleLabel.TabIndex = 0;
            this.reportsFormTitleLabel.Text = "Reports";
            // 
            // reportsFormUsersCombo
            // 
            this.reportsFormUsersCombo.FormattingEnabled = true;
            this.reportsFormUsersCombo.Location = new System.Drawing.Point(863, 124);
            this.reportsFormUsersCombo.Name = "reportsFormUsersCombo";
            this.reportsFormUsersCombo.Size = new System.Drawing.Size(121, 21);
            this.reportsFormUsersCombo.TabIndex = 1;
            // 
            // reportsFormUsersComboLabel
            // 
            this.reportsFormUsersComboLabel.AutoSize = true;
            this.reportsFormUsersComboLabel.Location = new System.Drawing.Point(739, 127);
            this.reportsFormUsersComboLabel.Name = "reportsFormUsersComboLabel";
            this.reportsFormUsersComboLabel.Size = new System.Drawing.Size(102, 13);
            this.reportsFormUsersComboLabel.TabIndex = 2;
            this.reportsFormUsersComboLabel.Text = "Please select a user";
            // 
            // reportsFormDgvAppointmentsByUser
            // 
            this.reportsFormDgvAppointmentsByUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.reportsFormDgvAppointmentsByUser.Location = new System.Drawing.Point(17, 124);
            this.reportsFormDgvAppointmentsByUser.Name = "reportsFormDgvAppointmentsByUser";
            this.reportsFormDgvAppointmentsByUser.Size = new System.Drawing.Size(698, 150);
            this.reportsFormDgvAppointmentsByUser.TabIndex = 3;
            // 
            // reportsFormSchedulesByUserLabel
            // 
            this.reportsFormSchedulesByUserLabel.AutoSize = true;
            this.reportsFormSchedulesByUserLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reportsFormSchedulesByUserLabel.Location = new System.Drawing.Point(14, 93);
            this.reportsFormSchedulesByUserLabel.Name = "reportsFormSchedulesByUserLabel";
            this.reportsFormSchedulesByUserLabel.Size = new System.Drawing.Size(144, 20);
            this.reportsFormSchedulesByUserLabel.TabIndex = 4;
            this.reportsFormSchedulesByUserLabel.Text = "Schedules By User";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label2.Location = new System.Drawing.Point(16, 301);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(985, 10);
            this.label2.TabIndex = 5;
            // 
            // reportsFormDownloadSchedulesByUser
            // 
            this.reportsFormDownloadSchedulesByUser.Location = new System.Drawing.Point(742, 251);
            this.reportsFormDownloadSchedulesByUser.Name = "reportsFormDownloadSchedulesByUser";
            this.reportsFormDownloadSchedulesByUser.Size = new System.Drawing.Size(123, 23);
            this.reportsFormDownloadSchedulesByUser.TabIndex = 6;
            this.reportsFormDownloadSchedulesByUser.Text = "Save Data to File";
            this.reportsFormDownloadSchedulesByUser.UseVisualStyleBackColor = true;
            this.reportsFormDownloadSchedulesByUser.Click += new System.EventHandler(this.reportsFormDownloadSchedulesByUser_Click);
            // 
            // reportsFormAppointmentsByMonthLabel
            // 
            this.reportsFormAppointmentsByMonthLabel.AutoSize = true;
            this.reportsFormAppointmentsByMonthLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reportsFormAppointmentsByMonthLabel.Location = new System.Drawing.Point(15, 343);
            this.reportsFormAppointmentsByMonthLabel.Name = "reportsFormAppointmentsByMonthLabel";
            this.reportsFormAppointmentsByMonthLabel.Size = new System.Drawing.Size(187, 20);
            this.reportsFormAppointmentsByMonthLabel.TabIndex = 7;
            this.reportsFormAppointmentsByMonthLabel.Text = "Appointments By Months";
            // 
            // reportsFormDgvAppointmentsByMonth
            // 
            this.reportsFormDgvAppointmentsByMonth.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.reportsFormDgvAppointmentsByMonth.Location = new System.Drawing.Point(19, 378);
            this.reportsFormDgvAppointmentsByMonth.Name = "reportsFormDgvAppointmentsByMonth";
            this.reportsFormDgvAppointmentsByMonth.Size = new System.Drawing.Size(698, 150);
            this.reportsFormDgvAppointmentsByMonth.TabIndex = 8;
            // 
            // reportsFormDownloadAppointmentsByMonth
            // 
            this.reportsFormDownloadAppointmentsByMonth.Location = new System.Drawing.Point(742, 505);
            this.reportsFormDownloadAppointmentsByMonth.Name = "reportsFormDownloadAppointmentsByMonth";
            this.reportsFormDownloadAppointmentsByMonth.Size = new System.Drawing.Size(123, 23);
            this.reportsFormDownloadAppointmentsByMonth.TabIndex = 9;
            this.reportsFormDownloadAppointmentsByMonth.Text = "Save Data to File";
            this.reportsFormDownloadAppointmentsByMonth.UseVisualStyleBackColor = true;
            this.reportsFormDownloadAppointmentsByMonth.Click += new System.EventHandler(this.reportsFormDownloadAppointmentsByMonth_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label1.Location = new System.Drawing.Point(16, 559);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(985, 10);
            this.label1.TabIndex = 10;
            // 
            // ReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 893);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.reportsFormDownloadAppointmentsByMonth);
            this.Controls.Add(this.reportsFormDgvAppointmentsByMonth);
            this.Controls.Add(this.reportsFormAppointmentsByMonthLabel);
            this.Controls.Add(this.reportsFormDownloadSchedulesByUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.reportsFormSchedulesByUserLabel);
            this.Controls.Add(this.reportsFormDgvAppointmentsByUser);
            this.Controls.Add(this.reportsFormUsersComboLabel);
            this.Controls.Add(this.reportsFormUsersCombo);
            this.Controls.Add(this.reportsFormTitleLabel);
            this.Name = "ReportsForm";
            this.Text = "ReportsForm";
            ((System.ComponentModel.ISupportInitialize)(this.reportsFormDgvAppointmentsByUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reportsFormDgvAppointmentsByMonth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label reportsFormTitleLabel;
        private System.Windows.Forms.ComboBox reportsFormUsersCombo;
        private System.Windows.Forms.Label reportsFormUsersComboLabel;
        private System.Windows.Forms.DataGridView reportsFormDgvAppointmentsByUser;
        private System.Windows.Forms.Label reportsFormSchedulesByUserLabel;
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button reportsFormDownloadSchedulesByUser;
        private System.Windows.Forms.Label reportsFormAppointmentsByMonthLabel;
        private System.Windows.Forms.DataGridView reportsFormDgvAppointmentsByMonth;
        private System.Windows.Forms.Button reportsFormDownloadAppointmentsByMonth;
        public System.Windows.Forms.Label label1;
    }
}
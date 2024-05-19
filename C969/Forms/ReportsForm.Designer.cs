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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.reportsFormDownloadSchedulesByUser = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.reportsFormDgvAppointmentsByUser)).BeginInit();
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
            this.reportsFormUsersCombo.Location = new System.Drawing.Point(864, 124);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Schedules By User";
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
            this.reportsFormDownloadSchedulesByUser.Text = "Save Results to File";
            this.reportsFormDownloadSchedulesByUser.UseVisualStyleBackColor = true;
            this.reportsFormDownloadSchedulesByUser.Click += new System.EventHandler(this.reportsFormDownloadSchedulesByUser_Click);
            // 
            // ReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 636);
            this.Controls.Add(this.reportsFormDownloadSchedulesByUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.reportsFormDgvAppointmentsByUser);
            this.Controls.Add(this.reportsFormUsersComboLabel);
            this.Controls.Add(this.reportsFormUsersCombo);
            this.Controls.Add(this.reportsFormTitleLabel);
            this.Name = "ReportsForm";
            this.Text = "ReportsForm";
            ((System.ComponentModel.ISupportInitialize)(this.reportsFormDgvAppointmentsByUser)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label reportsFormTitleLabel;
        private System.Windows.Forms.ComboBox reportsFormUsersCombo;
        private System.Windows.Forms.Label reportsFormUsersComboLabel;
        private System.Windows.Forms.DataGridView reportsFormDgvAppointmentsByUser;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button reportsFormDownloadSchedulesByUser;
    }
}
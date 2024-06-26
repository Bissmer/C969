﻿using System.Drawing;
using System.Windows.Forms;

namespace C969.Forms
{
    partial class CustomerManagementForm
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
            this.cusMgmtAddCustomerButton = new System.Windows.Forms.Button();
            this.cusMgmtEditCustomerButton = new System.Windows.Forms.Button();
            this.cusMgmtDeleteCustomerButton = new System.Windows.Forms.Button();
            this.cusMgmtDgvCustomers = new System.Windows.Forms.DataGridView();
            this.cusMgmCurrentUserlbl = new System.Windows.Forms.Label();
            this.cusMgmtCustomersLabel = new System.Windows.Forms.Label();
            this.cusMgmtAppintmentsLabel = new System.Windows.Forms.Label();
            this.cusMgmtDgvAppontments = new System.Windows.Forms.DataGridView();
            this.cusMgmtAddAppointmentButton = new System.Windows.Forms.Button();
            this.cusMgmtEditAppointmentButton = new System.Windows.Forms.Button();
            this.cusMgmtDeleteAppointmentButton = new System.Windows.Forms.Button();
            this.cusMgmtEndSessionButton = new System.Windows.Forms.Button();
            this.cusMgmtSearchAppByCustomer = new System.Windows.Forms.TextBox();
            this.cusMgmtAppointmentsCalendar = new System.Windows.Forms.MonthCalendar();
            this.label1 = new System.Windows.Forms.Label();
            this.cusMgmtShowAllAppts = new System.Windows.Forms.Button();
            this.cusMgmtReportsButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cusMgmtDgvCustomers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cusMgmtDgvAppontments)).BeginInit();
            this.SuspendLayout();
            // 
            // cusMgmtAddCustomerButton
            // 
            this.cusMgmtAddCustomerButton.Location = new System.Drawing.Point(12, 65);
            this.cusMgmtAddCustomerButton.Name = "cusMgmtAddCustomerButton";
            this.cusMgmtAddCustomerButton.Size = new System.Drawing.Size(136, 23);
            this.cusMgmtAddCustomerButton.TabIndex = 0;
            this.cusMgmtAddCustomerButton.Text = "Add Customer";
            this.cusMgmtAddCustomerButton.UseVisualStyleBackColor = true;
            this.cusMgmtAddCustomerButton.Click += new System.EventHandler(this.cusMgmtAddCustomerButton_Click);
            // 
            // cusMgmtEditCustomerButton
            // 
            this.cusMgmtEditCustomerButton.Location = new System.Drawing.Point(154, 65);
            this.cusMgmtEditCustomerButton.Name = "cusMgmtEditCustomerButton";
            this.cusMgmtEditCustomerButton.Size = new System.Drawing.Size(133, 23);
            this.cusMgmtEditCustomerButton.TabIndex = 1;
            this.cusMgmtEditCustomerButton.Text = "Edit Customer";
            this.cusMgmtEditCustomerButton.UseVisualStyleBackColor = true;
            this.cusMgmtEditCustomerButton.Click += new System.EventHandler(this.cusMgmtEditCustomerButton_Click);
            // 
            // cusMgmtDeleteCustomerButton
            // 
            this.cusMgmtDeleteCustomerButton.Location = new System.Drawing.Point(293, 65);
            this.cusMgmtDeleteCustomerButton.Name = "cusMgmtDeleteCustomerButton";
            this.cusMgmtDeleteCustomerButton.Size = new System.Drawing.Size(148, 23);
            this.cusMgmtDeleteCustomerButton.TabIndex = 2;
            this.cusMgmtDeleteCustomerButton.Text = "Delete Customer";
            this.cusMgmtDeleteCustomerButton.UseVisualStyleBackColor = true;
            this.cusMgmtDeleteCustomerButton.Click += new System.EventHandler(this.cusMgmtDeleteCustomerButton_Click);
            // 
            // cusMgmtDgvCustomers
            // 
            this.cusMgmtDgvCustomers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cusMgmtDgvCustomers.Location = new System.Drawing.Point(12, 94);
            this.cusMgmtDgvCustomers.Name = "cusMgmtDgvCustomers";
            this.cusMgmtDgvCustomers.ReadOnly = true;
            this.cusMgmtDgvCustomers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.cusMgmtDgvCustomers.Size = new System.Drawing.Size(923, 226);
            this.cusMgmtDgvCustomers.TabIndex = 3;
            // 
            // cusMgmCurrentUserlbl
            // 
            this.cusMgmCurrentUserlbl.AutoSize = true;
            this.cusMgmCurrentUserlbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cusMgmCurrentUserlbl.Location = new System.Drawing.Point(996, 34);
            this.cusMgmCurrentUserlbl.Name = "cusMgmCurrentUserlbl";
            this.cusMgmCurrentUserlbl.Size = new System.Drawing.Size(229, 20);
            this.cusMgmCurrentUserlbl.TabIndex = 4;
            this.cusMgmCurrentUserlbl.Text = "Currently Logged In User Label";
            this.cusMgmCurrentUserlbl.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // cusMgmtCustomersLabel
            // 
            this.cusMgmtCustomersLabel.AutoSize = true;
            this.cusMgmtCustomersLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cusMgmtCustomersLabel.Location = new System.Drawing.Point(12, 25);
            this.cusMgmtCustomersLabel.Name = "cusMgmtCustomersLabel";
            this.cusMgmtCustomersLabel.Size = new System.Drawing.Size(115, 25);
            this.cusMgmtCustomersLabel.TabIndex = 5;
            this.cusMgmtCustomersLabel.Text = "Customers";
            // 
            // cusMgmtAppintmentsLabel
            // 
            this.cusMgmtAppintmentsLabel.AutoSize = true;
            this.cusMgmtAppintmentsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cusMgmtAppintmentsLabel.Location = new System.Drawing.Point(13, 375);
            this.cusMgmtAppintmentsLabel.Name = "cusMgmtAppintmentsLabel";
            this.cusMgmtAppintmentsLabel.Size = new System.Drawing.Size(131, 25);
            this.cusMgmtAppintmentsLabel.TabIndex = 6;
            this.cusMgmtAppintmentsLabel.Text = "Appointemts";
            // 
            // cusMgmtDgvAppontments
            // 
            this.cusMgmtDgvAppontments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cusMgmtDgvAppontments.Location = new System.Drawing.Point(12, 456);
            this.cusMgmtDgvAppontments.Name = "cusMgmtDgvAppontments";
            this.cusMgmtDgvAppontments.ReadOnly = true;
            this.cusMgmtDgvAppontments.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.cusMgmtDgvAppontments.Size = new System.Drawing.Size(923, 226);
            this.cusMgmtDgvAppontments.TabIndex = 7;
            // 
            // cusMgmtAddAppointmentButton
            // 
            this.cusMgmtAddAppointmentButton.Location = new System.Drawing.Point(18, 427);
            this.cusMgmtAddAppointmentButton.Name = "cusMgmtAddAppointmentButton";
            this.cusMgmtAddAppointmentButton.Size = new System.Drawing.Size(130, 23);
            this.cusMgmtAddAppointmentButton.TabIndex = 8;
            this.cusMgmtAddAppointmentButton.Text = "Add Appointment";
            this.cusMgmtAddAppointmentButton.UseVisualStyleBackColor = true;
            this.cusMgmtAddAppointmentButton.Click += new System.EventHandler(this.cusMgmtAddAppointmentButton_Click);
            // 
            // cusMgmtEditAppointmentButton
            // 
            this.cusMgmtEditAppointmentButton.Location = new System.Drawing.Point(155, 427);
            this.cusMgmtEditAppointmentButton.Name = "cusMgmtEditAppointmentButton";
            this.cusMgmtEditAppointmentButton.Size = new System.Drawing.Size(132, 23);
            this.cusMgmtEditAppointmentButton.TabIndex = 9;
            this.cusMgmtEditAppointmentButton.Text = "Edit Appointment";
            this.cusMgmtEditAppointmentButton.UseVisualStyleBackColor = true;
            this.cusMgmtEditAppointmentButton.Click += new System.EventHandler(this.cusMgmtEditAppointmentButton_Click);
            // 
            // cusMgmtDeleteAppointmentButton
            // 
            this.cusMgmtDeleteAppointmentButton.Location = new System.Drawing.Point(294, 427);
            this.cusMgmtDeleteAppointmentButton.Name = "cusMgmtDeleteAppointmentButton";
            this.cusMgmtDeleteAppointmentButton.Size = new System.Drawing.Size(147, 23);
            this.cusMgmtDeleteAppointmentButton.TabIndex = 10;
            this.cusMgmtDeleteAppointmentButton.Text = "Delete Appointment";
            this.cusMgmtDeleteAppointmentButton.UseVisualStyleBackColor = true;
            this.cusMgmtDeleteAppointmentButton.Click += new System.EventHandler(this.cusMgmtDeleteAppointmentButton_Click);
            // 
            // cusMgmtEndSessionButton
            // 
            this.cusMgmtEndSessionButton.Location = new System.Drawing.Point(999, 182);
            this.cusMgmtEndSessionButton.Name = "cusMgmtEndSessionButton";
            this.cusMgmtEndSessionButton.Size = new System.Drawing.Size(234, 60);
            this.cusMgmtEndSessionButton.TabIndex = 11;
            this.cusMgmtEndSessionButton.Text = "End Session";
            this.cusMgmtEndSessionButton.UseVisualStyleBackColor = true;
            this.cusMgmtEndSessionButton.Click += new System.EventHandler(this.cusMgmtEndSessionButton_Click);
            // 
            // cusMgmtSearchAppByCustomer
            // 
            this.cusMgmtSearchAppByCustomer.ForeColor = System.Drawing.Color.Gray;
            this.cusMgmtSearchAppByCustomer.Location = new System.Drawing.Point(697, 430);
            this.cusMgmtSearchAppByCustomer.Name = "cusMgmtSearchAppByCustomer";
            this.cusMgmtSearchAppByCustomer.Size = new System.Drawing.Size(238, 20);
            this.cusMgmtSearchAppByCustomer.TabIndex = 13;
            this.cusMgmtSearchAppByCustomer.Text = "Search by Customer Name";
            // 
            // cusMgmtAppointmentsCalendar
            // 
            this.cusMgmtAppointmentsCalendar.Location = new System.Drawing.Point(1006, 459);
            this.cusMgmtAppointmentsCalendar.MaxSelectionCount = 1;
            this.cusMgmtAppointmentsCalendar.MinDate = new System.DateTime(2018, 12, 1, 0, 0, 0, 0);
            this.cusMgmtAppointmentsCalendar.Name = "cusMgmtAppointmentsCalendar";
            this.cusMgmtAppointmentsCalendar.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoEllipsis = true;
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(1002, 430);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 20);
            this.label1.TabIndex = 15;
            this.label1.Text = "Appointments calendar lookup";
            // 
            // cusMgmtShowAllAppts
            // 
            this.cusMgmtShowAllAppts.Location = new System.Drawing.Point(1045, 659);
            this.cusMgmtShowAllAppts.Name = "cusMgmtShowAllAppts";
            this.cusMgmtShowAllAppts.Size = new System.Drawing.Size(146, 23);
            this.cusMgmtShowAllAppts.TabIndex = 16;
            this.cusMgmtShowAllAppts.Text = "Show All Appointments";
            this.cusMgmtShowAllAppts.UseVisualStyleBackColor = true;
            this.cusMgmtShowAllAppts.Click += new System.EventHandler(this.cusMgmtShowAllAppts_Click);
            // 
            // cusMgmtReportsButton
            // 
            this.cusMgmtReportsButton.Location = new System.Drawing.Point(999, 94);
            this.cusMgmtReportsButton.Name = "cusMgmtReportsButton";
            this.cusMgmtReportsButton.Size = new System.Drawing.Size(234, 57);
            this.cusMgmtReportsButton.TabIndex = 17;
            this.cusMgmtReportsButton.Text = "Reports";
            this.cusMgmtReportsButton.UseVisualStyleBackColor = true;
            this.cusMgmtReportsButton.Click += new System.EventHandler(this.cusMgmtReportsButton_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label2.Location = new System.Drawing.Point(12, 351);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(1217, 1);
            this.label2.TabIndex = 18;
            // 
            // CustomerManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1290, 728);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cusMgmtReportsButton);
            this.Controls.Add(this.cusMgmtShowAllAppts);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cusMgmtAppointmentsCalendar);
            this.Controls.Add(this.cusMgmtSearchAppByCustomer);
            this.Controls.Add(this.cusMgmtEndSessionButton);
            this.Controls.Add(this.cusMgmtDeleteAppointmentButton);
            this.Controls.Add(this.cusMgmtEditAppointmentButton);
            this.Controls.Add(this.cusMgmtAddAppointmentButton);
            this.Controls.Add(this.cusMgmtDgvAppontments);
            this.Controls.Add(this.cusMgmtAppintmentsLabel);
            this.Controls.Add(this.cusMgmtCustomersLabel);
            this.Controls.Add(this.cusMgmCurrentUserlbl);
            this.Controls.Add(this.cusMgmtDgvCustomers);
            this.Controls.Add(this.cusMgmtDeleteCustomerButton);
            this.Controls.Add(this.cusMgmtEditCustomerButton);
            this.Controls.Add(this.cusMgmtAddCustomerButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "CustomerManagementForm";
            this.Text = "CustomerManagementForm";
            ((System.ComponentModel.ISupportInitialize)(this.cusMgmtDgvCustomers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cusMgmtDgvAppontments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cusMgmtAddCustomerButton;
        private System.Windows.Forms.Button cusMgmtEditCustomerButton;
        private System.Windows.Forms.Button cusMgmtDeleteCustomerButton;
        private System.Windows.Forms.DataGridView cusMgmtDgvCustomers;
        private System.Windows.Forms.Label cusMgmCurrentUserlbl;
        private System.Windows.Forms.Label cusMgmtCustomersLabel;
        private System.Windows.Forms.Label cusMgmtAppintmentsLabel;
        private System.Windows.Forms.DataGridView cusMgmtDgvAppontments;
        private System.Windows.Forms.Button cusMgmtAddAppointmentButton;
        private System.Windows.Forms.Button cusMgmtEditAppointmentButton;
        private System.Windows.Forms.Button cusMgmtDeleteAppointmentButton;
        private System.Windows.Forms.Button cusMgmtEndSessionButton;
        private System.Windows.Forms.TextBox cusMgmtSearchAppByCustomer;
        private System.Windows.Forms.MonthCalendar cusMgmtAppointmentsCalendar;
        private System.Windows.Forms.Label label1;
        private Button cusMgmtShowAllAppts;
        private Button cusMgmtReportsButton;
        public Label label2;
    }
}
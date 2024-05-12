using System;
using System.Windows.Forms;
using Org.BouncyCastle.Tls;

namespace C969.Forms
{
    partial class AddAppointmentForm
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
            this.addAppointmentFormLabel = new System.Windows.Forms.Label();
            this.addAppointmentStartDateLabel = new System.Windows.Forms.Label();
            this.addAppointmentLocationLabel = new System.Windows.Forms.Label();
            this.addAppointmentDescriptionLabel = new System.Windows.Forms.Label();
            this.addAppointmentTypeLabel = new System.Windows.Forms.Label();
            this.addAppointmentTitleLabel = new System.Windows.Forms.Label();
            this.addAppointmentLocationText = new System.Windows.Forms.TextBox();
            this.addAppointmentDescriptionText = new System.Windows.Forms.TextBox();
            this.addAppointmentTypeText = new System.Windows.Forms.TextBox();
            this.addAppointmentTitleText = new System.Windows.Forms.TextBox();
            this.addAppointmentCancelBtn = new System.Windows.Forms.Button();
            this.addAppointmentSaveBtn = new System.Windows.Forms.Button();
            this.mySqlDataAdapter1 = new MySql.Data.MySqlClient.MySqlDataAdapter();
            this.addAppointmentStartDatePicker = new System.Windows.Forms.DateTimePicker();
            this.addAppointmentStartTimeLabel = new System.Windows.Forms.Label();
            this.addAppointmentEndDateLabel = new System.Windows.Forms.Label();
            this.addAppointmentEndDatePicker = new System.Windows.Forms.DateTimePicker();
            this.addAppointmentEndTimeLabel = new System.Windows.Forms.Label();
            this.addAppointmentStartTimeCombo = new System.Windows.Forms.ComboBox();
            this.addAppointmentEndTimeCombo = new System.Windows.Forms.ComboBox();
            this.addAppointmentCustomerNameLabel = new System.Windows.Forms.Label();
            this.addAppointmentCustomerNameCombo = new System.Windows.Forms.ComboBox();
            this.addAppointmentCreatedByLabel = new System.Windows.Forms.Label();
            this.addAppointmentCurrentUserText = new System.Windows.Forms.TextBox();
            this.addAppointmentContactLabel = new System.Windows.Forms.Label();
            this.addAppointmentContactText = new System.Windows.Forms.TextBox();
            this.addAppointmentUrlLabel = new System.Windows.Forms.Label();
            this.addAppointmentUrlText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // addAppointmentFormLabel
            // 
            this.addAppointmentFormLabel.AutoSize = true;
            this.addAppointmentFormLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addAppointmentFormLabel.Location = new System.Drawing.Point(149, 73);
            this.addAppointmentFormLabel.Name = "addAppointmentFormLabel";
            this.addAppointmentFormLabel.Size = new System.Drawing.Size(206, 25);
            this.addAppointmentFormLabel.TabIndex = 38;
            this.addAppointmentFormLabel.Text = "Add an Appointment";
            // 
            // addAppointmentStartDateLabel
            // 
            this.addAppointmentStartDateLabel.AutoSize = true;
            this.addAppointmentStartDateLabel.Location = new System.Drawing.Point(84, 295);
            this.addAppointmentStartDateLabel.Name = "addAppointmentStartDateLabel";
            this.addAppointmentStartDateLabel.Size = new System.Drawing.Size(55, 13);
            this.addAppointmentStartDateLabel.TabIndex = 32;
            this.addAppointmentStartDateLabel.Text = "Start Date";
            // 
            // addAppointmentLocationLabel
            // 
            this.addAppointmentLocationLabel.AutoSize = true;
            this.addAppointmentLocationLabel.Location = new System.Drawing.Point(84, 259);
            this.addAppointmentLocationLabel.Name = "addAppointmentLocationLabel";
            this.addAppointmentLocationLabel.Size = new System.Drawing.Size(48, 13);
            this.addAppointmentLocationLabel.TabIndex = 31;
            this.addAppointmentLocationLabel.Text = "Location";
            // 
            // addAppointmentDescriptionLabel
            // 
            this.addAppointmentDescriptionLabel.AutoSize = true;
            this.addAppointmentDescriptionLabel.Location = new System.Drawing.Point(84, 218);
            this.addAppointmentDescriptionLabel.Name = "addAppointmentDescriptionLabel";
            this.addAppointmentDescriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.addAppointmentDescriptionLabel.TabIndex = 30;
            this.addAppointmentDescriptionLabel.Text = "Description";
            // 
            // addAppointmentTypeLabel
            // 
            this.addAppointmentTypeLabel.AutoSize = true;
            this.addAppointmentTypeLabel.Location = new System.Drawing.Point(84, 175);
            this.addAppointmentTypeLabel.Name = "addAppointmentTypeLabel";
            this.addAppointmentTypeLabel.Size = new System.Drawing.Size(31, 13);
            this.addAppointmentTypeLabel.TabIndex = 29;
            this.addAppointmentTypeLabel.Text = "Type";
            // 
            // addAppointmentTitleLabel
            // 
            this.addAppointmentTitleLabel.AutoSize = true;
            this.addAppointmentTitleLabel.Location = new System.Drawing.Point(84, 137);
            this.addAppointmentTitleLabel.Name = "addAppointmentTitleLabel";
            this.addAppointmentTitleLabel.Size = new System.Drawing.Size(27, 13);
            this.addAppointmentTitleLabel.TabIndex = 28;
            this.addAppointmentTitleLabel.Text = "Title";
            // 
            // addAppointmentLocationText
            // 
            this.addAppointmentLocationText.Location = new System.Drawing.Point(184, 252);
            this.addAppointmentLocationText.Name = "addAppointmentLocationText";
            this.addAppointmentLocationText.Size = new System.Drawing.Size(171, 20);
            this.addAppointmentLocationText.TabIndex = 26;
            // 
            // addAppointmentDescriptionText
            // 
            this.addAppointmentDescriptionText.Location = new System.Drawing.Point(184, 211);
            this.addAppointmentDescriptionText.Name = "addAppointmentDescriptionText";
            this.addAppointmentDescriptionText.Size = new System.Drawing.Size(171, 20);
            this.addAppointmentDescriptionText.TabIndex = 25;
            // 
            // addAppointmentTypeText
            // 
            this.addAppointmentTypeText.Location = new System.Drawing.Point(184, 172);
            this.addAppointmentTypeText.Name = "addAppointmentTypeText";
            this.addAppointmentTypeText.Size = new System.Drawing.Size(171, 20);
            this.addAppointmentTypeText.TabIndex = 24;
            // 
            // addAppointmentTitleText
            // 
            this.addAppointmentTitleText.Location = new System.Drawing.Point(184, 134);
            this.addAppointmentTitleText.Name = "addAppointmentTitleText";
            this.addAppointmentTitleText.Size = new System.Drawing.Size(171, 20);
            this.addAppointmentTitleText.TabIndex = 23;
            // 
            // addAppointmentCancelBtn
            // 
            this.addAppointmentCancelBtn.Location = new System.Drawing.Point(184, 638);
            this.addAppointmentCancelBtn.Name = "addAppointmentCancelBtn";
            this.addAppointmentCancelBtn.Size = new System.Drawing.Size(75, 23);
            this.addAppointmentCancelBtn.TabIndex = 22;
            this.addAppointmentCancelBtn.Text = "Cancel";
            this.addAppointmentCancelBtn.UseVisualStyleBackColor = true;
            // 
            // addAppointmentSaveBtn
            // 
            this.addAppointmentSaveBtn.Location = new System.Drawing.Point(184, 594);
            this.addAppointmentSaveBtn.Name = "addAppointmentSaveBtn";
            this.addAppointmentSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.addAppointmentSaveBtn.TabIndex = 21;
            this.addAppointmentSaveBtn.Text = "Save";
            this.addAppointmentSaveBtn.UseVisualStyleBackColor = true;
            this.addAppointmentSaveBtn.Click += new System.EventHandler(this.addAppointmentSaveBtn_Click);
            // 
            // mySqlDataAdapter1
            // 
            this.mySqlDataAdapter1.DeleteCommand = null;
            this.mySqlDataAdapter1.InsertCommand = null;
            this.mySqlDataAdapter1.SelectCommand = null;
            this.mySqlDataAdapter1.UpdateCommand = null;
            // 
            // addAppointmentStartDatePicker
            // 
            this.addAppointmentStartDatePicker.CustomFormat = "MM/dd/yyyy";
            this.addAppointmentStartDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.addAppointmentStartDatePicker.Location = new System.Drawing.Point(184, 289);
            this.addAppointmentStartDatePicker.MinDate = DateTime.Now;
            this.addAppointmentStartDatePicker.MaxDate = DateTime.Now.AddYears(1);
            this.addAppointmentStartDatePicker.Name = "addAppointmentStartDatePicker";
            this.addAppointmentStartDatePicker.Size = new System.Drawing.Size(171, 20);
            this.addAppointmentStartDatePicker.TabIndex = 39;
            
            // 
            // addAppointmentStartTimeLabel
            // 
            this.addAppointmentStartTimeLabel.AutoSize = true;
            this.addAppointmentStartTimeLabel.Location = new System.Drawing.Point(84, 327);
            this.addAppointmentStartTimeLabel.Name = "addAppointmentStartTimeLabel";
            this.addAppointmentStartTimeLabel.Size = new System.Drawing.Size(55, 13);
            this.addAppointmentStartTimeLabel.TabIndex = 40;
            this.addAppointmentStartTimeLabel.Text = "Start Time";
            // 
            // addAppointmentEndDateLabel
            // 
            this.addAppointmentEndDateLabel.AutoSize = true;
            this.addAppointmentEndDateLabel.Location = new System.Drawing.Point(84, 356);
            this.addAppointmentEndDateLabel.Name = "addAppointmentEndDateLabel";
            this.addAppointmentEndDateLabel.Size = new System.Drawing.Size(52, 13);
            this.addAppointmentEndDateLabel.TabIndex = 42;
            this.addAppointmentEndDateLabel.Text = "End Date";
            // 
            // addAppointmentEndDatePicker
            // 
            this.addAppointmentEndDatePicker.CustomFormat = "MM/dd/yyyy";
            this.addAppointmentEndDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.addAppointmentEndDatePicker.Location = new System.Drawing.Point(184, 356);
            this.addAppointmentEndDatePicker.Name = "addAppointmentEndDatePicker";
            this.addAppointmentEndDatePicker.Size = new System.Drawing.Size(171, 20);
            this.addAppointmentEndDatePicker.TabIndex = 43;
            // 
            // addAppointmentEndTimeLabel
            // 
            this.addAppointmentEndTimeLabel.AutoSize = true;
            this.addAppointmentEndTimeLabel.Location = new System.Drawing.Point(84, 385);
            this.addAppointmentEndTimeLabel.Name = "addAppointmentEndTimeLabel";
            this.addAppointmentEndTimeLabel.Size = new System.Drawing.Size(52, 13);
            this.addAppointmentEndTimeLabel.TabIndex = 44;
            this.addAppointmentEndTimeLabel.Text = "End Time";
            // 
            // addAppointmentStartTimeCombo
            // 
            this.addAppointmentStartTimeCombo.FormattingEnabled = true;
            this.addAppointmentStartTimeCombo.Location = new System.Drawing.Point(184, 324);
            this.addAppointmentStartTimeCombo.Name = "addAppointmentStartTimeCombo";
            this.addAppointmentStartTimeCombo.Size = new System.Drawing.Size(171, 21);
            this.addAppointmentStartTimeCombo.TabIndex = 45;
            // 
            // addAppointmentEndTimeCombo
            // 
            this.addAppointmentEndTimeCombo.FormattingEnabled = true;
            this.addAppointmentEndTimeCombo.Location = new System.Drawing.Point(184, 385);
            this.addAppointmentEndTimeCombo.Name = "addAppointmentEndTimeCombo";
            this.addAppointmentEndTimeCombo.Size = new System.Drawing.Size(171, 21);
            this.addAppointmentEndTimeCombo.TabIndex = 46;
            // 
            // addAppointmentCustomerNameLabel
            // 
            this.addAppointmentCustomerNameLabel.AutoSize = true;
            this.addAppointmentCustomerNameLabel.Location = new System.Drawing.Point(84, 423);
            this.addAppointmentCustomerNameLabel.Name = "addAppointmentCustomerNameLabel";
            this.addAppointmentCustomerNameLabel.Size = new System.Drawing.Size(82, 13);
            this.addAppointmentCustomerNameLabel.TabIndex = 47;
            this.addAppointmentCustomerNameLabel.Text = "Customer Name";
            // 
            // addAppointmentCustomerNameCombo
            // 
            this.addAppointmentCustomerNameCombo.FormattingEnabled = true;
            this.addAppointmentCustomerNameCombo.Location = new System.Drawing.Point(184, 420);
            this.addAppointmentCustomerNameCombo.Name = "addAppointmentCustomerNameCombo";
            this.addAppointmentCustomerNameCombo.Size = new System.Drawing.Size(171, 21);
            this.addAppointmentCustomerNameCombo.TabIndex = 48;
            // 
            // addAppointmentCreatedByLabel
            // 
            this.addAppointmentCreatedByLabel.AutoSize = true;
            this.addAppointmentCreatedByLabel.Location = new System.Drawing.Point(84, 456);
            this.addAppointmentCreatedByLabel.Name = "addAppointmentCreatedByLabel";
            this.addAppointmentCreatedByLabel.Size = new System.Drawing.Size(58, 13);
            this.addAppointmentCreatedByLabel.TabIndex = 49;
            this.addAppointmentCreatedByLabel.Text = "Created by";
            // 
            // addAppointmentCurrentUserText
            // 
            this.addAppointmentCurrentUserText.Location = new System.Drawing.Point(184, 453);
            this.addAppointmentCurrentUserText.Name = "addAppointmentCurrentUserText";
            this.addAppointmentCurrentUserText.Size = new System.Drawing.Size(171, 20);
            this.addAppointmentCurrentUserText.TabIndex = 50;
            // 
            // addAppointmentContactLabel
            // 
            this.addAppointmentContactLabel.AutoSize = true;
            this.addAppointmentContactLabel.Location = new System.Drawing.Point(84, 492);
            this.addAppointmentContactLabel.Name = "addAppointmentContactLabel";
            this.addAppointmentContactLabel.Size = new System.Drawing.Size(44, 13);
            this.addAppointmentContactLabel.TabIndex = 51;
            this.addAppointmentContactLabel.Text = "Contact";
            // 
            // addAppointmentContactText
            // 
            this.addAppointmentContactText.Location = new System.Drawing.Point(184, 485);
            this.addAppointmentContactText.Name = "addAppointmentContactText";
            this.addAppointmentContactText.Size = new System.Drawing.Size(171, 20);
            this.addAppointmentContactText.TabIndex = 52;
            // 
            // addAppointmentUrlLabel
            // 
            this.addAppointmentUrlLabel.AutoSize = true;
            this.addAppointmentUrlLabel.Location = new System.Drawing.Point(88, 523);
            this.addAppointmentUrlLabel.Name = "addAppointmentUrlLabel";
            this.addAppointmentUrlLabel.Size = new System.Drawing.Size(20, 13);
            this.addAppointmentUrlLabel.TabIndex = 53;
            this.addAppointmentUrlLabel.Text = "Url";
            // 
            // addAppointmentUrlText
            // 
            this.addAppointmentUrlText.Location = new System.Drawing.Point(184, 520);
            this.addAppointmentUrlText.Name = "addAppointmentUrlText";
            this.addAppointmentUrlText.Size = new System.Drawing.Size(171, 20);
            this.addAppointmentUrlText.TabIndex = 54;
            // 
            // AddAppointmentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(431, 695);
            this.Controls.Add(this.addAppointmentUrlText);
            this.Controls.Add(this.addAppointmentUrlLabel);
            this.Controls.Add(this.addAppointmentContactText);
            this.Controls.Add(this.addAppointmentContactLabel);
            this.Controls.Add(this.addAppointmentCurrentUserText);
            this.Controls.Add(this.addAppointmentCreatedByLabel);
            this.Controls.Add(this.addAppointmentCustomerNameCombo);
            this.Controls.Add(this.addAppointmentCustomerNameLabel);
            this.Controls.Add(this.addAppointmentEndTimeCombo);
            this.Controls.Add(this.addAppointmentStartTimeCombo);
            this.Controls.Add(this.addAppointmentEndTimeLabel);
            this.Controls.Add(this.addAppointmentEndDatePicker);
            this.Controls.Add(this.addAppointmentEndDateLabel);
            this.Controls.Add(this.addAppointmentStartTimeLabel);
            this.Controls.Add(this.addAppointmentStartDatePicker);
            this.Controls.Add(this.addAppointmentFormLabel);
            this.Controls.Add(this.addAppointmentStartDateLabel);
            this.Controls.Add(this.addAppointmentLocationLabel);
            this.Controls.Add(this.addAppointmentDescriptionLabel);
            this.Controls.Add(this.addAppointmentTypeLabel);
            this.Controls.Add(this.addAppointmentTitleLabel);
            this.Controls.Add(this.addAppointmentLocationText);
            this.Controls.Add(this.addAppointmentDescriptionText);
            this.Controls.Add(this.addAppointmentTypeText);
            this.Controls.Add(this.addAppointmentTitleText);
            this.Controls.Add(this.addAppointmentCancelBtn);
            this.Controls.Add(this.addAppointmentSaveBtn);
            this.Name = "AddAppointmentForm";
            this.Text = "Add an Appointment";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label addAppointmentFormLabel;
        private System.Windows.Forms.Label addAppointmentStartDateLabel;
        private System.Windows.Forms.Label addAppointmentLocationLabel;
        private System.Windows.Forms.Label addAppointmentDescriptionLabel;
        private System.Windows.Forms.Label addAppointmentTypeLabel;
        private System.Windows.Forms.Label addAppointmentTitleLabel;
        private System.Windows.Forms.TextBox addAppointmentLocationText;
        private System.Windows.Forms.TextBox addAppointmentDescriptionText;
        private System.Windows.Forms.TextBox addAppointmentTypeText;
        private System.Windows.Forms.TextBox addAppointmentTitleText;
        private System.Windows.Forms.Button addAppointmentCancelBtn;
        private System.Windows.Forms.Button addAppointmentSaveBtn;
        private MySql.Data.MySqlClient.MySqlDataAdapter mySqlDataAdapter1;
        private System.Windows.Forms.DateTimePicker addAppointmentStartDatePicker;
        private Label addAppointmentStartTimeLabel;
        private Label addAppointmentEndDateLabel;
        private DateTimePicker addAppointmentEndDatePicker;
        private Label addAppointmentEndTimeLabel;
        private ComboBox addAppointmentStartTimeCombo;
        private ComboBox addAppointmentEndTimeCombo;
        private Label addAppointmentCustomerNameLabel;
        private ComboBox addAppointmentCustomerNameCombo;
        private Label addAppointmentCreatedByLabel;
        private TextBox addAppointmentCurrentUserText;
        private Label addAppointmentContactLabel;
        private TextBox addAppointmentContactText;
        private Label addAppointmentUrlLabel;
        private TextBox addAppointmentUrlText;
    }
}
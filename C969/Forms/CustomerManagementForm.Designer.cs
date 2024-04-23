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
            ((System.ComponentModel.ISupportInitialize)(this.cusMgmtDgvCustomers)).BeginInit();
            this.SuspendLayout();
            // 
            // cusMgmtAddCustomerButton
            // 
            this.cusMgmtAddCustomerButton.Location = new System.Drawing.Point(12, 25);
            this.cusMgmtAddCustomerButton.Name = "cusMgmtAddCustomerButton";
            this.cusMgmtAddCustomerButton.Size = new System.Drawing.Size(136, 23);
            this.cusMgmtAddCustomerButton.TabIndex = 0;
            this.cusMgmtAddCustomerButton.Text = "Add Customer";
            this.cusMgmtAddCustomerButton.UseVisualStyleBackColor = true;
            this.cusMgmtAddCustomerButton.Click += new System.EventHandler(this.cusMgmtAddCustomerButton_Click);
            // 
            // cusMgmtEditCustomerButton
            // 
            this.cusMgmtEditCustomerButton.Location = new System.Drawing.Point(171, 25);
            this.cusMgmtEditCustomerButton.Name = "cusMgmtEditCustomerButton";
            this.cusMgmtEditCustomerButton.Size = new System.Drawing.Size(133, 23);
            this.cusMgmtEditCustomerButton.TabIndex = 1;
            this.cusMgmtEditCustomerButton.Text = "Edit Customer";
            this.cusMgmtEditCustomerButton.UseVisualStyleBackColor = true;
            this.cusMgmtEditCustomerButton.Click += new System.EventHandler(this.cusMgmtEditCustomerButton_Click);
            // 
            // cusMgmtDeleteCustomerButton
            // 
            this.cusMgmtDeleteCustomerButton.Location = new System.Drawing.Point(335, 25);
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
            this.cusMgmtDgvCustomers.Location = new System.Drawing.Point(13, 102);
            this.cusMgmtDgvCustomers.Name = "cusMgmtDgvCustomers";
            this.cusMgmtDgvCustomers.Size = new System.Drawing.Size(923, 226);
            this.cusMgmtDgvCustomers.TabIndex = 3;
            // 
            // cusMgmCurrentUserlbl
            // 
            this.cusMgmCurrentUserlbl.AutoSize = true;
            this.cusMgmCurrentUserlbl.Location = new System.Drawing.Point(933, 25);
            this.cusMgmCurrentUserlbl.Name = "cusMgmCurrentUserlbl";
            this.cusMgmCurrentUserlbl.Size = new System.Drawing.Size(35, 13);
            this.cusMgmCurrentUserlbl.TabIndex = 4;
            this.cusMgmCurrentUserlbl.Text = "label1";
            // 
            // CustomerManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1056, 553);
            this.Controls.Add(this.cusMgmCurrentUserlbl);
            this.Controls.Add(this.cusMgmtDgvCustomers);
            this.Controls.Add(this.cusMgmtDeleteCustomerButton);
            this.Controls.Add(this.cusMgmtEditCustomerButton);
            this.Controls.Add(this.cusMgmtAddCustomerButton);
            this.Name = "CustomerManagementForm";
            this.Text = "CustomerManagementForm";
            ((System.ComponentModel.ISupportInitialize)(this.cusMgmtDgvCustomers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cusMgmtAddCustomerButton;
        private System.Windows.Forms.Button cusMgmtEditCustomerButton;
        private System.Windows.Forms.Button cusMgmtDeleteCustomerButton;
        private System.Windows.Forms.DataGridView cusMgmtDgvCustomers;
        private System.Windows.Forms.Label cusMgmCurrentUserlbl;
    }
}
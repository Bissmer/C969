namespace C969.Forms
{
    partial class AddCustomerForm
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
            this.components = new System.ComponentModel.Container();
            this.addCustomerCustomerNametxt = new System.Windows.Forms.TextBox();
            this.addCustomerAddressIDtxt = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addCustomerCustomerNameLabel = new System.Windows.Forms.Label();
            this.addCustomerAddressIdLabel = new System.Windows.Forms.Label();
            this.addCustomerSaveBtn = new System.Windows.Forms.Button();
            this.addCustomerCanceBtn = new System.Windows.Forms.Button();
            this.addCustomerActiveCheck = new System.Windows.Forms.CheckBox();
            this.addCustomerActiveLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // addCustomerCustomerNametxt
            // 
            this.addCustomerCustomerNametxt.Location = new System.Drawing.Point(243, 163);
            this.addCustomerCustomerNametxt.Name = "addCustomerCustomerNametxt";
            this.addCustomerCustomerNametxt.Size = new System.Drawing.Size(100, 20);
            this.addCustomerCustomerNametxt.TabIndex = 0;
            // 
            // addCustomerAddressIDtxt
            // 
            this.addCustomerAddressIDtxt.Location = new System.Drawing.Point(243, 224);
            this.addCustomerAddressIDtxt.Name = "addCustomerAddressIDtxt";
            this.addCustomerAddressIDtxt.Size = new System.Drawing.Size(100, 20);
            this.addCustomerAddressIDtxt.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // addCustomerCustomerNameLabel
            // 
            this.addCustomerCustomerNameLabel.AutoSize = true;
            this.addCustomerCustomerNameLabel.Location = new System.Drawing.Point(142, 166);
            this.addCustomerCustomerNameLabel.Name = "addCustomerCustomerNameLabel";
            this.addCustomerCustomerNameLabel.Size = new System.Drawing.Size(82, 13);
            this.addCustomerCustomerNameLabel.TabIndex = 4;
            this.addCustomerCustomerNameLabel.Text = "Customer Name";
            // 
            // addCustomerAddressIdLabel
            // 
            this.addCustomerAddressIdLabel.AutoSize = true;
            this.addCustomerAddressIdLabel.Location = new System.Drawing.Point(142, 227);
            this.addCustomerAddressIdLabel.Name = "addCustomerAddressIdLabel";
            this.addCustomerAddressIdLabel.Size = new System.Drawing.Size(59, 13);
            this.addCustomerAddressIdLabel.TabIndex = 5;
            this.addCustomerAddressIdLabel.Text = "Address ID";
            // 
            // addCustomerSaveBtn
            // 
            this.addCustomerSaveBtn.Location = new System.Drawing.Point(205, 353);
            this.addCustomerSaveBtn.Name = "addCustomerSaveBtn";
            this.addCustomerSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.addCustomerSaveBtn.TabIndex = 7;
            this.addCustomerSaveBtn.Text = "Save";
            this.addCustomerSaveBtn.UseVisualStyleBackColor = true;
            this.addCustomerSaveBtn.Click += new System.EventHandler(this.addCustomerSaveBtn_Click);
            // 
            // addCustomerCanceBtn
            // 
            this.addCustomerCanceBtn.Location = new System.Drawing.Point(205, 406);
            this.addCustomerCanceBtn.Name = "addCustomerCanceBtn";
            this.addCustomerCanceBtn.Size = new System.Drawing.Size(75, 23);
            this.addCustomerCanceBtn.TabIndex = 8;
            this.addCustomerCanceBtn.Text = "Cancel";
            this.addCustomerCanceBtn.UseVisualStyleBackColor = true;
            this.addCustomerCanceBtn.Click += new System.EventHandler(this.addCustomerCanceBtn_Click);
            // 
            // addCustomerActiveCheck
            // 
            this.addCustomerActiveCheck.AutoSize = true;
            this.addCustomerActiveCheck.Location = new System.Drawing.Point(243, 266);
            this.addCustomerActiveCheck.Name = "addCustomerActiveCheck";
            this.addCustomerActiveCheck.Size = new System.Drawing.Size(15, 14);
            this.addCustomerActiveCheck.TabIndex = 9;
            this.addCustomerActiveCheck.UseVisualStyleBackColor = true;
            // 
            // addCustomerActiveLabel
            // 
            this.addCustomerActiveLabel.AutoSize = true;
            this.addCustomerActiveLabel.Location = new System.Drawing.Point(142, 266);
            this.addCustomerActiveLabel.Name = "addCustomerActiveLabel";
            this.addCustomerActiveLabel.Size = new System.Drawing.Size(37, 13);
            this.addCustomerActiveLabel.TabIndex = 10;
            this.addCustomerActiveLabel.Text = "Active";
            // 
            // AddCustomerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 533);
            this.Controls.Add(this.addCustomerActiveLabel);
            this.Controls.Add(this.addCustomerActiveCheck);
            this.Controls.Add(this.addCustomerCanceBtn);
            this.Controls.Add(this.addCustomerSaveBtn);
            this.Controls.Add(this.addCustomerAddressIdLabel);
            this.Controls.Add(this.addCustomerCustomerNameLabel);
            this.Controls.Add(this.addCustomerAddressIDtxt);
            this.Controls.Add(this.addCustomerCustomerNametxt);
            this.Name = "AddCustomerForm";
            this.Text = "Add a Customer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox addCustomerCustomerNametxt;
        private System.Windows.Forms.TextBox addCustomerAddressIDtxt;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label addCustomerCustomerNameLabel;
        private System.Windows.Forms.Label addCustomerAddressIdLabel;
        private System.Windows.Forms.Button addCustomerSaveBtn;
        private System.Windows.Forms.Button addCustomerCanceBtn;
        private System.Windows.Forms.CheckBox addCustomerActiveCheck;
        private System.Windows.Forms.Label addCustomerActiveLabel;
    }
}
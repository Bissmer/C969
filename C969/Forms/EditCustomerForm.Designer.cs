namespace C969.Forms
{
    partial class EditCustomerForm
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.editCustomerActiveLabel = new System.Windows.Forms.Label();
            this.editCustomerActiveCheck = new System.Windows.Forms.CheckBox();
            this.editCustomerCanceBtn = new System.Windows.Forms.Button();
            this.editCustomerSaveBtn = new System.Windows.Forms.Button();
            this.editCustomerAddressIdLabel = new System.Windows.Forms.Label();
            this.editCustomerCustomerNameLabel = new System.Windows.Forms.Label();
            this.editCustomerAddressIDtxt = new System.Windows.Forms.TextBox();
            this.editCustomerCustomerNametxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // editCustomerActiveLabel
            // 
            this.editCustomerActiveLabel.AutoSize = true;
            this.editCustomerActiveLabel.Location = new System.Drawing.Point(125, 225);
            this.editCustomerActiveLabel.Name = "editCustomerActiveLabel";
            this.editCustomerActiveLabel.Size = new System.Drawing.Size(37, 13);
            this.editCustomerActiveLabel.TabIndex = 18;
            this.editCustomerActiveLabel.Text = "Active";
            // 
            // editCustomerActiveCheck
            // 
            this.editCustomerActiveCheck.AutoSize = true;
            this.editCustomerActiveCheck.Location = new System.Drawing.Point(226, 225);
            this.editCustomerActiveCheck.Name = "editCustomerActiveCheck";
            this.editCustomerActiveCheck.Size = new System.Drawing.Size(15, 14);
            this.editCustomerActiveCheck.TabIndex = 17;
            this.editCustomerActiveCheck.UseVisualStyleBackColor = true;
            // 
            // editCustomerCanceBtn
            // 
            this.editCustomerCanceBtn.Location = new System.Drawing.Point(188, 365);
            this.editCustomerCanceBtn.Name = "editCustomerCanceBtn";
            this.editCustomerCanceBtn.Size = new System.Drawing.Size(75, 23);
            this.editCustomerCanceBtn.TabIndex = 16;
            this.editCustomerCanceBtn.Text = "Cancel";
            this.editCustomerCanceBtn.UseVisualStyleBackColor = true;
            this.editCustomerCanceBtn.Click += new System.EventHandler(this.editCustomerCanceBtn_Click);
            // 
            // editCustomerSaveBtn
            // 
            this.editCustomerSaveBtn.Location = new System.Drawing.Point(188, 312);
            this.editCustomerSaveBtn.Name = "editCustomerSaveBtn";
            this.editCustomerSaveBtn.Size = new System.Drawing.Size(75, 23);
            this.editCustomerSaveBtn.TabIndex = 15;
            this.editCustomerSaveBtn.Text = "Save";
            this.editCustomerSaveBtn.UseVisualStyleBackColor = true;
            this.editCustomerSaveBtn.Click += new System.EventHandler(this.editCustomerSaveBtn_Click);
            // 
            // editCustomerAddressIdLabel
            // 
            this.editCustomerAddressIdLabel.AutoSize = true;
            this.editCustomerAddressIdLabel.Location = new System.Drawing.Point(125, 186);
            this.editCustomerAddressIdLabel.Name = "editCustomerAddressIdLabel";
            this.editCustomerAddressIdLabel.Size = new System.Drawing.Size(59, 13);
            this.editCustomerAddressIdLabel.TabIndex = 14;
            this.editCustomerAddressIdLabel.Text = "Address ID";
            // 
            // editCustomerCustomerNameLabel
            // 
            this.editCustomerCustomerNameLabel.AutoSize = true;
            this.editCustomerCustomerNameLabel.Location = new System.Drawing.Point(125, 125);
            this.editCustomerCustomerNameLabel.Name = "editCustomerCustomerNameLabel";
            this.editCustomerCustomerNameLabel.Size = new System.Drawing.Size(82, 13);
            this.editCustomerCustomerNameLabel.TabIndex = 13;
            this.editCustomerCustomerNameLabel.Text = "Customer Name";
            // 
            // editCustomerAddressIDtxt
            // 
            this.editCustomerAddressIDtxt.Location = new System.Drawing.Point(226, 183);
            this.editCustomerAddressIDtxt.Name = "editCustomerAddressIDtxt";
            this.editCustomerAddressIDtxt.Size = new System.Drawing.Size(100, 20);
            this.editCustomerAddressIDtxt.TabIndex = 12;
            // 
            // editCustomerCustomerNametxt
            // 
            this.editCustomerCustomerNametxt.Location = new System.Drawing.Point(226, 122);
            this.editCustomerCustomerNametxt.Name = "editCustomerCustomerNametxt";
            this.editCustomerCustomerNametxt.Size = new System.Drawing.Size(100, 20);
            this.editCustomerCustomerNametxt.TabIndex = 11;
            // 
            // EditCustomerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 511);
            this.Controls.Add(this.editCustomerActiveLabel);
            this.Controls.Add(this.editCustomerActiveCheck);
            this.Controls.Add(this.editCustomerCanceBtn);
            this.Controls.Add(this.editCustomerSaveBtn);
            this.Controls.Add(this.editCustomerAddressIdLabel);
            this.Controls.Add(this.editCustomerCustomerNameLabel);
            this.Controls.Add(this.editCustomerAddressIDtxt);
            this.Controls.Add(this.editCustomerCustomerNametxt);
            this.Name = "EditCustomerForm";
            this.Text = "Edit a Customer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.Label editCustomerActiveLabel;
        private System.Windows.Forms.CheckBox editCustomerActiveCheck;
        private System.Windows.Forms.Button editCustomerCanceBtn;
        private System.Windows.Forms.Button editCustomerSaveBtn;
        private System.Windows.Forms.Label editCustomerAddressIdLabel;
        private System.Windows.Forms.Label editCustomerCustomerNameLabel;
        private System.Windows.Forms.TextBox editCustomerAddressIDtxt;
        private System.Windows.Forms.TextBox editCustomerCustomerNametxt;
    }
}
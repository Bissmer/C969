namespace C969
{
    partial class Login
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
            this.loginHeaderLabel = new System.Windows.Forms.Label();
            this.loginUserNametxt = new System.Windows.Forms.TextBox();
            this.loginPasswordtxt = new System.Windows.Forms.TextBox();
            this.loginLoginButton = new System.Windows.Forms.Button();
            this.loginQuitButton = new System.Windows.Forms.Button();
            this.loginUsernameLabel = new System.Windows.Forms.Label();
            this.loginPasswordLabel = new System.Windows.Forms.Label();
            this.loginTimeZoneLabel = new System.Windows.Forms.Label();
            this.loginTimeZoneText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // loginHeaderLabel
            // 
            this.loginHeaderLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.loginHeaderLabel.AutoSize = true;
            this.loginHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loginHeaderLabel.Location = new System.Drawing.Point(137, 98);
            this.loginHeaderLabel.Name = "loginHeaderLabel";
            this.loginHeaderLabel.Size = new System.Drawing.Size(92, 18);
            this.loginHeaderLabel.TabIndex = 0;
            this.loginHeaderLabel.Text = "Please log in";
            // 
            // loginUserNametxt
            // 
            this.loginUserNametxt.Location = new System.Drawing.Point(140, 142);
            this.loginUserNametxt.Name = "loginUserNametxt";
            this.loginUserNametxt.Size = new System.Drawing.Size(100, 20);
            this.loginUserNametxt.TabIndex = 1;
            // 
            // loginPasswordtxt
            // 
            this.loginPasswordtxt.Location = new System.Drawing.Point(140, 181);
            this.loginPasswordtxt.Name = "loginPasswordtxt";
            this.loginPasswordtxt.Size = new System.Drawing.Size(100, 20);
            this.loginPasswordtxt.TabIndex = 2;
            // 
            // loginLoginButton
            // 
            this.loginLoginButton.Location = new System.Drawing.Point(140, 233);
            this.loginLoginButton.Name = "loginLoginButton";
            this.loginLoginButton.Size = new System.Drawing.Size(75, 23);
            this.loginLoginButton.TabIndex = 3;
            this.loginLoginButton.Text = "Login";
            this.loginLoginButton.UseVisualStyleBackColor = true;
            this.loginLoginButton.Click += new System.EventHandler(this.loginLoginButton_Click);
            // 
            // loginQuitButton
            // 
            this.loginQuitButton.Location = new System.Drawing.Point(140, 273);
            this.loginQuitButton.Name = "loginQuitButton";
            this.loginQuitButton.Size = new System.Drawing.Size(75, 23);
            this.loginQuitButton.TabIndex = 4;
            this.loginQuitButton.Text = "Quit";
            this.loginQuitButton.UseVisualStyleBackColor = true;
            this.loginQuitButton.Click += new System.EventHandler(this.loginQuitButton_Click);
            // 
            // loginUsernameLabel
            // 
            this.loginUsernameLabel.AutoSize = true;
            this.loginUsernameLabel.Location = new System.Drawing.Point(58, 145);
            this.loginUsernameLabel.Name = "loginUsernameLabel";
            this.loginUsernameLabel.Size = new System.Drawing.Size(55, 13);
            this.loginUsernameLabel.TabIndex = 5;
            this.loginUsernameLabel.Text = "Username";
            // 
            // loginPasswordLabel
            // 
            this.loginPasswordLabel.AutoSize = true;
            this.loginPasswordLabel.Location = new System.Drawing.Point(60, 181);
            this.loginPasswordLabel.Name = "loginPasswordLabel";
            this.loginPasswordLabel.Size = new System.Drawing.Size(53, 13);
            this.loginPasswordLabel.TabIndex = 6;
            this.loginPasswordLabel.Text = "Password";
            // 
            // loginTimeZoneLabel
            // 
            this.loginTimeZoneLabel.AutoSize = true;
            this.loginTimeZoneLabel.Location = new System.Drawing.Point(12, 9);
            this.loginTimeZoneLabel.Name = "loginTimeZoneLabel";
            this.loginTimeZoneLabel.Size = new System.Drawing.Size(87, 13);
            this.loginTimeZoneLabel.TabIndex = 7;
            this.loginTimeZoneLabel.Text = "Local Time Zone";
            // 
            // loginTimeZoneText
            // 
            this.loginTimeZoneText.AutoSize = true;
            this.loginTimeZoneText.Location = new System.Drawing.Point(12, 31);
            this.loginTimeZoneText.Name = "loginTimeZoneText";
            this.loginTimeZoneText.Size = new System.Drawing.Size(169, 13);
            this.loginTimeZoneText.TabIndex = 8;
            this.loginTimeZoneText.Text = "here comes the timezone of a user";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 403);
            this.Controls.Add(this.loginTimeZoneText);
            this.Controls.Add(this.loginTimeZoneLabel);
            this.Controls.Add(this.loginPasswordLabel);
            this.Controls.Add(this.loginUsernameLabel);
            this.Controls.Add(this.loginQuitButton);
            this.Controls.Add(this.loginLoginButton);
            this.Controls.Add(this.loginPasswordtxt);
            this.Controls.Add(this.loginUserNametxt);
            this.Controls.Add(this.loginHeaderLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Login";
            this.Text = "Sceduling App Login Window";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label loginHeaderLabel;
        private System.Windows.Forms.TextBox loginUserNametxt;
        private System.Windows.Forms.TextBox loginPasswordtxt;
        private System.Windows.Forms.Button loginLoginButton;
        private System.Windows.Forms.Button loginQuitButton;
        private System.Windows.Forms.Label loginUsernameLabel;
        private System.Windows.Forms.Label loginPasswordLabel;
        private System.Windows.Forms.Label loginTimeZoneLabel;
        private System.Windows.Forms.Label loginTimeZoneText;
    }
}


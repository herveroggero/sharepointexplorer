namespace ui
{
    partial class connect
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelInfo = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxAuthToken = new System.Windows.Forms.TextBox();
            this.textBoxConfigName = new System.Windows.Forms.TextBox();
            this.textBoxEnzoURI = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonTestConnection = new System.Windows.Forms.Button();
            this.linkLabelHelp = new System.Windows.Forms.LinkLabel();
            this.linkLabelSignUp = new System.Windows.Forms.LinkLabel();
            this.checkBoxSave = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(606, 194);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(101, 37);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(499, 194);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(101, 37);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(12, 9);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(147, 13);
            this.labelInfo.TabIndex = 2;
            this.labelInfo.Text = "Connect to an Enzo instance:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Enzo URL:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Auth Token:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Configuration Name:";
            // 
            // textBoxAuthToken
            // 
            this.textBoxAuthToken.Location = new System.Drawing.Point(133, 72);
            this.textBoxAuthToken.Name = "textBoxAuthToken";
            this.textBoxAuthToken.PasswordChar = '*';
            this.textBoxAuthToken.Size = new System.Drawing.Size(397, 20);
            this.textBoxAuthToken.TabIndex = 7;
            this.toolTip1.SetToolTip(this.textBoxAuthToken, "Your Enzo authToken");
            // 
            // textBoxConfigName
            // 
            this.textBoxConfigName.Location = new System.Drawing.Point(133, 98);
            this.textBoxConfigName.Name = "textBoxConfigName";
            this.textBoxConfigName.Size = new System.Drawing.Size(226, 20);
            this.textBoxConfigName.TabIndex = 8;
            this.toolTip1.SetToolTip(this.textBoxConfigName, "Your configuration name (leave blank to use the default)");
            // 
            // textBoxEnzoURI
            // 
            this.textBoxEnzoURI.Location = new System.Drawing.Point(132, 46);
            this.textBoxEnzoURI.Name = "textBoxEnzoURI";
            this.textBoxEnzoURI.Size = new System.Drawing.Size(397, 20);
            this.textBoxEnzoURI.TabIndex = 10;
            this.toolTip1.SetToolTip(this.textBoxEnzoURI, "The Enzo URI or IP address to use");
            // 
            // buttonTestConnection
            // 
            this.buttonTestConnection.Location = new System.Drawing.Point(17, 194);
            this.buttonTestConnection.Name = "buttonTestConnection";
            this.buttonTestConnection.Size = new System.Drawing.Size(112, 37);
            this.buttonTestConnection.TabIndex = 11;
            this.buttonTestConnection.Text = "Test Connection...";
            this.buttonTestConnection.UseVisualStyleBackColor = true;
            this.buttonTestConnection.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // linkLabelHelp
            // 
            this.linkLabelHelp.AutoSize = true;
            this.linkLabelHelp.Location = new System.Drawing.Point(573, 49);
            this.linkLabelHelp.Name = "linkLabelHelp";
            this.linkLabelHelp.Size = new System.Drawing.Size(100, 13);
            this.linkLabelHelp.TabIndex = 12;
            this.linkLabelHelp.TabStop = true;
            this.linkLabelHelp.Text = "Open Help Window";
            this.linkLabelHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelHelp_LinkClicked);
            // 
            // linkLabelSignUp
            // 
            this.linkLabelSignUp.AutoSize = true;
            this.linkLabelSignUp.Location = new System.Drawing.Point(573, 74);
            this.linkLabelSignUp.Name = "linkLabelSignUp";
            this.linkLabelSignUp.Size = new System.Drawing.Size(134, 13);
            this.linkLabelSignUp.TabIndex = 13;
            this.linkLabelSignUp.TabStop = true;
            this.linkLabelSignUp.Text = "Sign up with Enzo Online...";
            this.linkLabelSignUp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSignUp_LinkClicked);
            // 
            // checkBoxSave
            // 
            this.checkBoxSave.AutoSize = true;
            this.checkBoxSave.Location = new System.Drawing.Point(133, 143);
            this.checkBoxSave.Name = "checkBoxSave";
            this.checkBoxSave.Size = new System.Drawing.Size(150, 17);
            this.checkBoxSave.TabIndex = 14;
            this.checkBoxSave.Text = "Save settings for next time";
            this.checkBoxSave.UseVisualStyleBackColor = true;
            // 
            // connect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(717, 242);
            this.Controls.Add(this.checkBoxSave);
            this.Controls.Add(this.linkLabelSignUp);
            this.Controls.Add(this.linkLabelHelp);
            this.Controls.Add(this.buttonTestConnection);
            this.Controls.Add(this.textBoxEnzoURI);
            this.Controls.Add(this.textBoxConfigName);
            this.Controls.Add(this.textBoxAuthToken);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "connect";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connect to Enzo";
            this.Load += new System.EventHandler(this.connect_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxAuthToken;
        private System.Windows.Forms.TextBox textBoxConfigName;
        private System.Windows.Forms.TextBox textBoxEnzoURI;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button buttonTestConnection;
        private System.Windows.Forms.LinkLabel linkLabelHelp;
        private System.Windows.Forms.LinkLabel linkLabelSignUp;
        private System.Windows.Forms.CheckBox checkBoxSave;
    }
}
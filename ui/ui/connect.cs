using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ui
{
    public partial class connect : Form
    {
        public connect()
        {
            InitializeComponent();
        }
        
        private void connect_Load(object sender, EventArgs e)
        {
            string connStr = Properties.Settings.Default.EnzoConnection;
            if (!string.IsNullOrEmpty(connStr))
            {
                try
                {
                    byte[] conn = System.Convert.FromBase64String(connStr);
                    EnzoHttpSP http = EnzoHttpSP.Deserialize(conn);
                    textBoxAuthToken.Text = http.AuthToken;
                    textBoxConfigName.Text = http.ConfigName;
                    textBoxEnzoURI.Text = http.URI;

                    checkBoxSave.Checked = true;
                }
                catch { }
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        internal byte[] ConnectionInfo { get; private set; }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                EnzoHttpSP http = new EnzoHttpSP(textBoxEnzoURI.Text, textBoxAuthToken.Text, textBoxConfigName.Text);
                http.TryConnect(true);
                Cursor.Current = Cursors.Default;
                byte[] conn = http.Serialize();
                ConnectionInfo = conn;

                if (checkBoxSave.Checked)
                {
                    Properties.Settings.Default.EnzoConnection = System.Convert.ToBase64String(conn);
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.EnzoConnection = "";
                    Properties.Settings.Default.Save();
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                EnzoHttpSP http = new EnzoHttpSP(textBoxEnzoURI.Text, textBoxAuthToken.Text, textBoxConfigName.Text);
                http.TryConnect(true);

                MessageBox.Show(this, "Connected successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        private void linkLabelHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            help help = new help();
            help.Show();
        }

        private void linkLabelSignUp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://portal.enzounified.com/Account/Register");
            }
            catch { }
        }
    }
}

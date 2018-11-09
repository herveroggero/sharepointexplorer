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
    public partial class addGroupUser : Form
    {
        public addGroupUser()
        {
            InitializeComponent();
        }

        public addGroupUser(EnzoHttpSP enzoSP, string groupName)
        {
            InitializeComponent();

            Cursor.Current = Cursors.WaitCursor;
            labelGroupName.Text = groupName;

            try
            {
                var data = enzoSP.Execute("getusers");
                listViewUsers.Fill(data, "id");
            }
            catch { }
            Cursor.Current = Cursors.Default;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (listViewUsers.SelectedItems.Count > 0)
            {
                if (MessageBox.Show(this, "You are about to add the selected user. Proceed?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string[] loginParts = listViewUsers.SelectedItems[0].SubItems[3].Text.Split('|');
                    LoginId = loginParts[loginParts.Count() - 1];
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            else
            {
                MessageBox.Show(this, "Please select a user to add", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        public string LoginId { get; set; }

    }
}

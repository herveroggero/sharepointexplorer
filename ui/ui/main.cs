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
    public partial class main : Form
    {
        #region Private properties
        /// <summary>
        /// Current Enzo connection settings
        /// </summary>
        private byte[] _connectionBytes = null;
        #endregion

        public main()
        {
            InitializeComponent();
            splitContainerRoot.Visible = false;

        }

        private void main_Load(object sender, EventArgs e)
        {

            panelLists.BringToFront();
            panelLists.Dock = DockStyle.Fill;
            panelLists.Visible = true;
        }


        #region Menu events

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var form = new connect();
                var res = form.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    _connectionBytes = form.ConnectionInfo;

                    RefreshUI();

                    splitContainerRoot.Visible = true;
                }
            }
            catch { }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                RefreshUI();
            }
            catch { }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshToolStripMenuItem.Enabled = _connectionBytes != null;
        }
        #endregion

        #region Global Form Properties

        /// <summary>
        /// Current Enzo Connection Object
        /// </summary>
        public EnzoHttpSP EnzoSP
        {
            get
            {
                if (_connectionBytes == null)
                    return null;

                return EnzoHttpSP.Deserialize(_connectionBytes);

            }
        }

        public EnzoSharePointOperations EnzoSharePointOp { get { return new EnzoSharePointOperations(EnzoSP); } }

        /// <summary>
        /// The currently selected list name, if any
        /// </summary>
        public string SelectedListName { get { return (listViewLists.SelectedItems.Count == 0) ? "" : listViewLists.SelectedItems[0].SubItems[1].Text; } }
        /// <summary>
        /// The currently selected group name, if any
        /// </summary>
        public string SelectedGroupName { get { return (listViewGroups.SelectedItems.Count == 0) ? "" : listViewGroups.SelectedItems[0].SubItems[1].Text; } }
        /// <summary>
        /// The currently selected list type, if any
        /// </summary>
        public string SelectedListType { get { return (listViewLists.SelectedItems.Count == 0) ? "" : listViewLists.SelectedItems[0].SubItems[4].Text; } }
        /// <summary>
        /// True when the currently selected list is a document library
        /// </summary>
        public bool IsDocumentLibrarySelected { get { return SelectedListType == "DocumentLibrary"; } }
        #endregion

        #region Global Methods
        public void RefreshUI()
        {
            try
            {
                ClearUI();
                Cursor.Current = Cursors.WaitCursor;
                if (_connectionBytes != null)
                {
                    // Refresh lists...
                    var data1 = EnzoSharePointOp.GetLists();
                    listViewLists.Fill(data1, "Id");

                    // Refresh Groups...
                    var data2 = EnzoSharePointOp.GetGroups();
                    listViewGroups.Fill(data2, "groupId");
                }

            }
            catch { }
            Cursor.Current = Cursors.Default;
        }

        public void ClearUI()
        {
            listViewGroups.Items.Clear();
            listViewGroupUsers.Items.Clear();
            listViewLists.Items.Clear();
            listViewItems.Items.Clear();
            listViewDocuments.Items.Clear();
            listViewPermissions.Items.Clear();
            listViewFolderPermissions.Items.Clear();
            listViewFields.Items.Clear();
            listViewFolderRoles.Items.Clear();
        }

        #endregion

        #region UI Events

        private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                switch(tabControlMain.SelectedIndex)
                {
                    case 0: // lists
                        panelLists.BringToFront();
                        panelLists.Dock = DockStyle.Fill;
                        panelLists.Visible = true;
                        break;

                    case 1: // groups
                        panelGroups.BringToFront();
                        panelGroups.Dock = DockStyle.Fill;
                        panelGroups.Visible = true;
                        break;
                }
            }
            catch { }
        }

        private void listViewGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                var data = EnzoSharePointOp.GetGroupUsers(SelectedGroupName);
                listViewGroupUsers.Fill(data, "groupId");
            }
            catch (Exception ex) { }
            Cursor.Current = Cursors.Default;
        }
        
        private void listViewLists_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                var data = EnzoSharePointOp.GetFolderUserPermissions(SelectedListName);
                listViewPermissions.Fill(data, "title", "title,loginName,role");

                if (IsDocumentLibrarySelected)
                {
                    listViewDocuments.Items.Clear();
                    listViewFolderRoles.Items.Clear();
                    listViewFolderPermissions.Items.Clear();
                    tabControlDocuments.Dock = DockStyle.Fill;
                    tabControlDocuments.Visible = true;
                    listViewItems.Visible = false;
                    LoadDocuments(SelectedListName);
                }
                else
                {
                    listViewItems.Items.Clear();
                    listViewItems.Dock = DockStyle.Fill;
                    listViewItems.Visible = true;
                    tabControlDocuments.Visible = false;
                    LoadListItems(SelectedListName);
                }

                listViewFields.Items.Clear();

            }
            catch (Exception ex) { }
            Cursor.Current = Cursors.Default;
        }
        
        private void linkLabelItemsRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            if (IsDocumentLibrarySelected)
                LoadDocuments(SelectedListName);
            else
                LoadListItems(SelectedListName);
            Cursor.Current = Cursors.Default;
        }

        private void linkLabelFieldsRefresh_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var data = EnzoSharePointOp.GetListFields(SelectedListName);
            listViewFields.Fill(data, "Id");
            Cursor.Current = Cursors.Default;
        }
        
        private void listViewFolderRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewFolderRoles.SelectedItems.Count == 0) return;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (IsDocumentLibrarySelected)
                {
                    string folderName = listViewFolderRoles.SelectedItems[0].SubItems[3].Text;
                    if (folderName.Contains(SelectedListName))
                        folderName = folderName.Substring(folderName.IndexOf(SelectedListName) + SelectedListName.Length + 1);

                    var data = EnzoSharePointOp.GetFolderUserPermissions(SelectedListName, folderName);
                    listViewFolderPermissions.Fill(data, "title");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            Cursor.Current = Cursors.Default;
        }
        
        private void toolStripButtonAddUser_Click(object sender, EventArgs e)
        {
            try
            {
                string group = SelectedGroupName;
                addGroupUser form = new addGroupUser(EnzoSP, group);
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    string loginId = form.LoginId;

                    Cursor.Current = Cursors.WaitCursor;
                    EnzoSharePointOp.AddUserToGroup(group, loginId);

                    var data = EnzoSharePointOp.GetGroupUsers(group);
                    listViewGroupUsers.Fill(data, "groupId");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }

        private void toolStripButtonRemoveUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (listViewGroupUsers.SelectedItems.Count > 0)
                {
                    string group = SelectedGroupName;
                    if (!string.IsNullOrEmpty(group))
                    {
                        if (MessageBox.Show(this, "You are about to remove the selected user from this group. Proceed?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            string[] userParts = listViewGroupUsers.SelectedItems[0].SubItems[2].Text.Split('|');
                            string userId = userParts[userParts.Count() - 1];

                            Cursor.Current = Cursors.WaitCursor;
                            EnzoSharePointOp.RemoveUserFromGroup(group, userId);

                            var data = EnzoSharePointOp.GetGroupUsers(group);
                            listViewGroupUsers.Fill(data, "groupId");
                        }
                    }
                }
                else
                {
                    MessageBox.Show(this, "Please select a user to remove", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
        }
        
        
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about form = new about();
            form.ShowDialog(this);
        }
        #endregion

        #region List items & documents refresh 
        
        /// <summary>
        /// Loads a document library
        /// </summary>
        private void LoadDocuments(string listName)
        {
            listViewDocuments.Items.Clear();
            listViewFolderRoles.Items.Clear();
            listViewFolderPermissions.Items.Clear();

            #region Check limit setting
            int limit = 0;

            if (!int.TryParse(textBoxItemsLimit.Text, out limit))
            {
                MessageBox.Show(this, "Cannot refresh list items: the limit value is not an integer.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            #endregion

            var data = EnzoSharePointOp.GetListDocuments(listName, limit);

            // TODO: Order data based on folders, if DocumentLibrary
            listViewDocuments.Fill(data, "id");

            var data2 = EnzoSharePointOp.GetFoldersWithRoles(listName);
            listViewFolderRoles.Fill(data2, "id");

        }

        /// <summary>
        /// Loads a SharePoint list
        /// </summary>
        private void LoadListItems(string listName)
        {
            listViewItems.Items.Clear();

            #region Check limit setting
            int limit = 0;

            if (!int.TryParse(textBoxItemsLimit.Text, out limit))
            {
                MessageBox.Show(this, "Cannot refresh list items: the limit value is not an integer.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            #endregion

            var data = EnzoSharePointOp.GetListItems(listName, limit);

            listViewItems.Fill(data, "ID");
        }

        #endregion

    }
}

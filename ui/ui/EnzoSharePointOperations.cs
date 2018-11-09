using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ui
{
    public class EnzoSharePointOperations
    {
        private EnzoHttpSP _enzoSP = null;

        public EnzoSharePointOperations(EnzoHttpSP enzoSP) { _enzoSP = enzoSP; }
        
        public List<dynamic> GetFolderUserPermissions(string listName, string folderName = null)
        {
            string param = "title:" + listName;
            if (folderName != null)
                folderName = "folder:" + folderName;
            return _enzoSP.ExecuteAsDynamic("getuserpermissions", param, folderName);
        }

        public List<dynamic> GetFoldersWithRoles(string listName)
        {
            string param = "title:" + listName;
            return _enzoSP.ExecuteAsDynamic("getfolderswithroles", param);
        }

        public List<dynamic> GetGroupUsers(string groupName)
        {
            string param = "name:" + groupName;
            return _enzoSP.ExecuteAsDynamic("getgroupusers", param);
        }

        public List<dynamic> GetGroups()
        {
            return _enzoSP.ExecuteAsDynamic("getgroups");
        }

        public List<dynamic> GetLists()
        {
            return _enzoSP.ExecuteAsDynamic("lists");
        }

        public List<dynamic> GetListDocuments(string listName, int limit)
        {
            string param = "title:" + listName;
            string top = "top:" + limit.ToString();
            string where = "recursive:1";
            
            return _enzoSP.ExecuteAsDynamic("ListDocuments", param, top, where);
        }

        public List<dynamic> GetListItems(string listName, int limit)
        {
            string param = "viewname:" + listName;
            string top = "top:" + limit.ToString();

            return _enzoSP.ExecuteAsDynamic("getlistitemsex", param, top);
        }

        public List<dynamic> GetListFields(string listName)
        {
            string param = "title:" + listName;
            return _enzoSP.ExecuteAsDynamic("getfields", param);
        }

        public void RemoveUserFromGroup(string groupName, string userId)
        {
            string name = "name:" + groupName;
            string userInfo = "userInfo:" + userId;
            _enzoSP.Execute("RemoveGroupUser", name, userInfo);
        }

        public void AddUserToGroup(string groupName, string userId)
        {
            string name = "name:" + groupName;
            string userInfo = "userInfo:" + userId;
            _enzoSP.Execute("addgroupuser", name, userInfo);
        }

    }
}

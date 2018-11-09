using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ui
{
    /// <summary>
    /// HTTP Enzo class that provides access to a SharePoint site
    /// </summary>
    public class EnzoHttpSP
    {
        public string URI { get; set; }
        public string URIBSC { get { return URI + @"bsc/sharepoint/"; } }
        public string URISPLists { get { return URIBSC + @"lists"; } }

        public string AuthToken { get; set; }
        public string ConfigName { get; set; }

        public EnzoHttpSP() { }

        public EnzoHttpSP(string uri, string authToken, string configName)
        {
            URI = uri;
            AuthToken = authToken;
            ConfigName = configName;
            if (!URI.EndsWith(@"/")) URI = URI + @"/";
        }

        public string LastErrorMessage { get; set; }

        public int LastErrorCode { get; set; }

        /// <summary>
        /// Makes the actual call to Enzo and returns a JSON string
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        private dynamic FetchInternal(string cmd, out string tableName, params string[] headers)
        {
            dynamic res = null;
            string uri = URIBSC + cmd;
            tableName = "";

            WebRequest wr = WebRequest.CreateHttp(uri);
            wr.Headers.Add("authToken", AuthToken);
            wr.Headers.Add("_config", ConfigName);
            wr.Method = "GET";

            if (headers != null)
            {
                foreach (string hdr in headers)
                {
                    if (!string.IsNullOrEmpty(hdr))
                    {
                        string hdrName = hdr.Split(':')[0];
                        string hdrVal = hdr.Split(':')[1];
                        wr.Headers.Add(hdrName, hdrVal);
                    }
                }
            }

            var resp = wr.GetResponse();    // Execute the command

            using (var stream = resp.GetResponseStream())
            {
                StreamReader r = new StreamReader(stream);
                string json = r.ReadToEnd();
                dynamic payload = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

                // Error being returned by Enzo?
                LastErrorCode = payload.error.errorCode;
                LastErrorMessage = payload.error.errorMsg;
                if (LastErrorCode > 0)
                    throw new Exception(LastErrorMessage);

                res = payload.results;
                if (res != null)
                    tableName = ((IDictionary<string, object>)res.ToObject<Dictionary<string, object>>()).First().Key;
            }
            return res;
        }
        

        /// <summary>
        /// Makes a call to a specific method on Enzo and returns a list of string/value pairs representing the result
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public List<IDictionary<string, object>> Execute(string cmd, params string[] headers)
        {
            List<IDictionary<string, object>> res = new List<IDictionary<string, object>>();
            string tableName = "";
            dynamic results = FetchInternal(cmd, out tableName, headers);
            
            if (results != null)
                res = (List<IDictionary<string, object>>)results[tableName].ToObject<List<IDictionary<string, object>>>();
            
            return res;
        }

        /// <summary>
        /// Makes a call to a specific method on Enzo and returns a list of dynamic objects representing the result
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public List<dynamic> ExecuteAsDynamic(string cmd, params string[] headers)
        {
            List<dynamic> res = new List<dynamic>();
            string tableName = "";
            dynamic results = FetchInternal(cmd, out tableName, headers);

            if (results != null)
            {
                foreach (dynamic item in results[tableName])
                    res.Add(item);
            }
              
            return res;
        }

        /// <summary>
        /// Attempt to connect to the Enzo service by making a call to Enzo
        /// </summary>
        /// <param name="throwOnError"></param>
        /// <returns></returns>
        public bool TryConnect(bool throwOnError)
        {
            bool res = false;

            try
            {
                // Establish connection
                WebRequest wr = WebRequest.CreateHttp(URISPLists);
                wr.Headers.Add("authToken", AuthToken);
                wr.Headers.Add("_config", ConfigName);

                wr.Method = "GET";

                var resp = wr.GetResponse();    // Execute the command

                using (var stream = resp.GetResponseStream())
                {
                    

                }
                res = true;
            }
            catch
            {
                res = false;
                if (throwOnError)
                    throw;
            }
            
            return res;
        }
        
        public byte[] Serialize()
        {
            System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof(EnzoHttpSP));
            MemoryStream stream = new MemoryStream();
            s.Serialize(stream, this);
            byte[] data = stream.ToArray();
            return data;
        }

        public static EnzoHttpSP Deserialize(byte[] data)
        {
            System.Xml.Serialization.XmlSerializer s = new System.Xml.Serialization.XmlSerializer(typeof(EnzoHttpSP));
            MemoryStream stream = new MemoryStream(data);
            EnzoHttpSP obj = (EnzoHttpSP)s.Deserialize(stream);
            return obj;

        }

    }

    /// <summary>
    /// Extension methods that abstracts specific operations against Enzo result sets
    /// </summary>
    public static class EnzoHelper
    {
        /// <summary>
        /// Attempts to retrieve a value from a field, if found, and returns a default value if not found 
        /// </summary>
        /// <param name="row">IDictionary object</param>
        /// <param name="field">The name of the field to retrieve</param>
        /// <param name="defaultValue">Default value to return if the field is not found</param>
        /// <returns></returns>
        public static string TryGetField(this IDictionary<string, object> row, string field, string defaultValue)
        {
            string res = (row.ContainsKey(field)) ? (row[field] ?? "").ToString() : defaultValue;
            return res;
        }

        /// <summary>
        /// Extracts all available fields from a returned data set
        /// </summary>
        /// <param name="data">IDictionary</param>
        /// <returns></returns>
        public static string ExtractFields(List<IDictionary<string, object>> data)
        {
            string fields = "";
            if (data != null && data.Count() > 0)
            {
                foreach (string field in data.First().Keys)
                {
                    fields += ((fields.Length > 0) ? "," : "") + field;
                }
            }
            return fields;
        }
        
        /// <summary>
        /// Dynamically creates columns in a ListView object based on the returned data set if items are available.
        /// </summary>
        /// <param name="list">A ListView object</param>
        /// <param name="data">IDictionary object containing the result set</param>
        /// <param name="addIDIfMissing">Automatically add the ID field if missing from the result set</param>
        /// <returns></returns>
        public static string BuildColumns(this ListView list, List<IDictionary<string, object>> data, string fields = null, bool addIDIfMissing = false)
        {
            list.Columns.Clear();

            if (data == null || data.Count() == 0) return null;

            fields = fields ?? ExtractFields(data);

            bool hasID = false;
            foreach(string field in fields.Split(','))
            {
                int width = 100;
                if (field == "ID") hasID = true;
                if (field == "Title") width = 200;
                list.Columns.Add(field.Trim(), field.Trim(), width);
            }

            if (addIDIfMissing && !hasID)
            {
                fields = "ID," + fields;
                list.Columns.Insert(0, "ID", 100);
                hasID = true;
            }
            else if (hasID)
            {
                // If the ID field is found in the list columns, move as first column
                int idx = list.Columns["ID"].Index;
                list.Columns.RemoveAt(idx);
                list.Columns.Insert(0, "ID", 100);
            }

            return fields;
        }

        /// <summary>
        /// Fill a ListView based on a list of columns provided; if null, the column list is dynamically determine based on the result
        /// </summary>
        /// <param name="list">A ListView object</param>
        /// <param name="data">An IDictionary object containing the returned result set</param>
        /// <param name="fields">Fields to display (leave null to display all available fields)</param>
        /// <param name="keyField">The field name used as the key (displayed as first column)</param>
        /// <returns></returns>
        public static int Fill(this ListView list, List<IDictionary<string, object>> data, string keyField = "ID", string fields = null)
        {
            int count = 0;
            list.Items.Clear();

            fields = list.BuildColumns(data, fields);

            if (data != null || data.Count() > 0)
            {
                foreach (IDictionary<string, object> row in data)
                {
                    ListViewItem item = new ListViewItem(row[keyField].ToString());
                    foreach (string field in fields.Split(','))
                    {
                        if (field.Trim() == keyField) continue; // already added - warning: field names are case-sensitive
                        item.SubItems.Add(row.TryGetField(field.Trim(), ""));
                    }
                    list.Items.Add(item);
                }
            }
            return count;
        }

        /// <summary>
        /// Overload of Fill operation using a dynamic object
        /// </summary>
        /// <param name="list"></param>
        /// <param name="data"></param>
        /// <param name="fields"></param>
        /// <param name="keyField"></param>
        /// <returns></returns>
        public static int Fill(this ListView list, dynamic data, string keyField = "ID", string fields = null)
        {
            list.Items.Clear();
            if (data == null) return 0;
            List<IDictionary<string, object>> dataAsDict = new List<IDictionary<string, object>>();
            JArray asJArray = (JArray)JToken.FromObject(data);
            
            foreach(JObject obj in asJArray)
            {
                dataAsDict.Add(obj.ToObject<IDictionary<string, object>>());
            }
            return Fill(list, dataAsDict, keyField, fields);
        }

    }


}

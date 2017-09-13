using Microsoft.Data.Sqlite;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MockupWeb.Shared {
    public class BmprReader {
        public Bmpr ReadFromFile(string filepath) {
            if (!File.Exists(filepath)) { throw new FileNotFoundException($"Mockup not found at {filepath}"); }

            var resxFound = new List<MockupResource>();
            using (var connection = new SqliteConnection($"DataSource={filepath}")) {
                connection.Open();

                using (var command = connection.CreateCommand()) {
                    command.CommandText = @"SELECT ID,BRANCHID,ATTRIBUTES,DATA  from RESOURCES;";
                    var cmdresult = command.ExecuteReader();

                    while (cmdresult.Read()) {
                        //var attrs = JObject.Parse(Attributes)
                        string id = cmdresult.GetString(0);
                        string branchid = cmdresult.GetString(1);
                        string attributes = cmdresult.GetString(2);
                        string data = cmdresult.GetString(3);

                        var attrObj = JObject.Parse(attributes);
                        var kind = attrObj["kind"].Value<string>();
                        if (string.Equals("mockup", kind, StringComparison.OrdinalIgnoreCase)) {
                            var resx = new MockupResource(id, branchid, attributes, data);
                            if (!resx.Trashed) {
                                resxFound.Add(resx);
                            }
                        }
                    }
                }
            }

            return new Bmpr(resxFound);
        }
    }
}

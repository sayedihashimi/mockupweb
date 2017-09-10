using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MockupWeb.Shared
{
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
                        var resx = new MockupResource(cmdresult.GetInt32(0), cmdresult.GetInt32(1), cmdresult.GetString(2), cmdresult.GetString(3));

                        resxFound.Add(resx);
                    }
                }
            }

            return new Bmpr(resxFound);
        }
        /*
            string mockupfilepath = @"C:\temp\balsamiq\basic.bmpr";
            string constring = $"DataSource={mockupfilepath}";
            try {
                using (var connection = new SqliteConnection(constring)) {
                    connection.Open();

                    using (var command = connection.CreateCommand()) {
                        command.CommandText = @"SELECT ID,BRANCHID,ATTRIBUTES,DATA  from RESOURCES;";
                        var cmdresult = command.ExecuteReader();

                        var resxFound = new List<MockupResource>();                        
                        while (cmdresult.Read()) {
                            var resx = new MockupResource();
                            resx.ID = cmdresult.GetInt32(0);
                            resx.BranchId = cmdresult.GetInt32(1);
                            resx.Attributes = cmdresult.GetString(2);
                            resx.Data = cmdresult.GetString(3);

                            resxFound.Add(resx);
                        }
                    }

                    System.Console.WriteLine("Foobar");
                }
         */
    }
}

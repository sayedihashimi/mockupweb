using Newtonsoft.Json.Linq;
using Microsoft.Data.Sqlite;
using System;
using MockupWeb.Shared;
using System.Collections.Generic;

namespace MockupWeb.Cli
{
    class Program
    {
        static void Main(string[] args) {
            string mockupfilepath = @"C:\temp\balsamiq\basic.bmpr";
            var bmprfile = new BmprReader().ReadFromFile(mockupfilepath);

            var ctrlsWithLinks = bmprfile.GetControlsWithLinks();

            System.Console.WriteLine("foo");


        }
        static void MainOld(string[] args)
        {
            string mockupfilepath = @"C:\temp\balsamiq\basic.bmpr";
            string constring = $"DataSource={mockupfilepath}";
            System.Console.WriteLine("Foobar");
            try {
                using (var connection = new SqliteConnection(constring)) {
                    connection.Open();

                    using (var command = connection.CreateCommand()) {
                        command.CommandText = @"SELECT ID,BRANCHID,ATTRIBUTES,DATA  from RESOURCES;";
                        var cmdresult = command.ExecuteReader();

                        var resxFound = new List<MockupResource>();                        
                        while (cmdresult.Read()) {
                            var resx = new MockupResource(cmdresult.GetInt32(0), cmdresult.GetInt32(1), cmdresult.GetString(2), cmdresult.GetString(3));

                            resxFound.Add(resx);
                        }
                    }

                    System.Console.WriteLine("Foobar");
                }
            }
            catch {

            }

            /*
"mockup": {
        "controls": {
            "control": [
                {
                    "ID": "0",
                    "measuredH": "43",
                    "measuredW": "278",
                    "properties": {
                        "href": {
             */

            Console.WriteLine("Hello World!");
        }
    }
}

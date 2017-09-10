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
    }
}

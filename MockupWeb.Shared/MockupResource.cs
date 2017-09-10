using System;
using System.Collections.Generic;
using System.Text;

namespace MockupWeb.Shared
{
    public class MockupResource
    {
        // command.CommandText = @"SELECT ID,BranchId,ATTRIBUTES,DATA  from RESOURCES;";
        public int ID { get; set; }
        public int BranchId { get; set; }
        public string Attributes { get; set; }
        public string Data { get; set; }

        public new string ToString => $"ID:{ID} BranchId:{BranchId}";
    }
}

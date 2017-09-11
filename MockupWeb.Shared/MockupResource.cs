using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MockupWeb.Shared
{
    public class MockupResource
    {
        public MockupResource(string id,string branchiId, string attributes, string data) {
            ID = id;
            BranchId = branchiId;
            Attributes = attributes;
            Data = data;

            InitFromAttributes();
            InitFromData();
        }
        public string ID { get; private set; }
        public string BranchId { get; private set; }
        public string Attributes { get; private set; }
        public string Data { get; private set; }

        public string Name { get; private set; }
        //public double Order { get; private set; }
        public Mockup Mockup { get; set; }

        public new string ToString => $"ID:{ID} BranchId:{BranchId}";

        private void InitFromData() {
            if (string.IsNullOrWhiteSpace(Data)) { return; }
            var dataObject = JObject.Parse(Data);
            var mc = new Mockup();
            mc.MeasuredHeight = dataObject["mockup"]["measuredH"].Value<int>();
            mc.MeasuredWidth = dataObject["mockup"]["measuredW"].Value<int>();
            mc.MockupHeight = dataObject["mockup"]["mockupH"].Value<int>();
            mc.MockupWidth = dataObject["mockup"]["mockupW"].Value<int>();
            
            var controlsJObject = dataObject["mockup"]["controls"]["control"];

            foreach(var ctrl in controlsJObject) {
                var control = new Control();
                control.ID = ctrl["ID"].Value<int>();
                control.CtrlType = ctrl["typeID"] != null ? ctrl["typeID"].Value<string>() : null;
                control.Height = ctrl["h"] != null ? ctrl["h"].Value<int>() : -1;
                control.Width = ctrl["w"] != null ? ctrl["w"].Value<int>() : -1;
                control.MeasuredHeight = ctrl["measuredH"] != null ? ctrl["measuredH"].Value<int>() : -1;
                control.MeasuredWidth = ctrl["measuredW"] != null ? ctrl["measuredW"].Value<int>() : -1;
                control.LocationX = ctrl["x"] != null ? ctrl["x"].Value<int>() : -1;
                control.LocationY = ctrl["y"] != null ? ctrl["y"].Value<int>() : -1;
                control.Zorder = ctrl["zOrder"] != null ? ctrl["zOrder"].Value<int>() : -1;

                // see if there it has a link
                if(ctrl["properties"] != null
                    && ctrl["properties"]["href"] != null
                    && ctrl["properties"]["href"]["ID"] != null
                    ) {
                    control.LinkId = ctrl["properties"]["href"]["ID"].Value<string>();
                }

                mc.Controls.Add(control);
            }

            Mockup = mc;
        }

        private void InitFromAttributes() {
            if (string.IsNullOrWhiteSpace(Attributes)) { return; }

            var attJObject = JObject.Parse(Attributes);
            Name = attJObject["name"].Value<string>();
            // Order = attJObject["order"] != null ? attJObject["order"].Value<double>() : double.MinValue;
        }
    }
}

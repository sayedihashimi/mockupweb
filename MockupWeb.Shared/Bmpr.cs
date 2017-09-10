using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MockupWeb.Shared
{
    public class Bmpr
    {
        public Bmpr() {

        }
        public Bmpr(List<MockupResource> resources) {
            Resources = resources;
        }
        public List<MockupResource> Resources { get; set; } = new List<MockupResource>();

        public List<Control> GetControlsWithLinks() {
            var result = new List<Control>();

            if(Resources != null && Resources.Count > 0) {
                // loop through resources and examine the Data field
                foreach(var resx in Resources) {
                    var resxdata = JObject.Parse(resx.Data);

                    var controls = resxdata["mockup"]["controls"]["control"];

                }
            }

            return result;
        }
    }

    public class Mockup {
        public int MeasuredHeight { get; set; } = -1;
        public int MeasuredWidth { get; set; } = -1;
        public int MockupHeight { get; set; } = -1;
        public int MockupWidth { get; set; } = -1;
        public List<Control> Controls{get;set;} = new List<Control>();
    }
   
    public class Control {
        public int ID { get; set; }
        public int MeasuredHeight { get; set; }
        public int MeasuredWidth { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string LinkId { get; set; }
        public int LocationX { get; set; }
        public int LocationY { get; set; }
        public int Zorder { get; set; }
    }
}

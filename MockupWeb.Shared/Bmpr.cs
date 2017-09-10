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

            }

            return result;
        }

        
    }
    /*
"ID": "1",
                    "measuredH": "24",
                    "measuredW": "58",
                    "properties": {
                        "href": {
                            "ID": "B3E99078-DBDA-BC80-FF32-698C443AD20B"
                        }
                    },
                    "typeID": "Button",
                    "x": "442",
                    "y": "47",
                    "zOrder": "1"
     */
    public class Control {
        public int ID { get; set; }
        public int MeasuredHeight { get; set; }
        public int MeasuredWidth { get; set; }
        public string LinkValue { get; set; }
        public int LocationX { get; set; }
        public int LocationY { get; set; }
    }
}

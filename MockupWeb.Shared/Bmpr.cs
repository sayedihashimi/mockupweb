using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;

namespace MockupWeb.Shared
{
    public class Bmpr
    {
        public Bmpr() { }
        public Bmpr(List<MockupResource> resources) {
            Resources = resources;
        }
        public List<MockupResource> Resources { get; set; } = new List<MockupResource>();

        public List<Control> GetAllControls() {
            var result = new List<Control>();
            foreach(var resx in Resources) {
                result.AddRange(resx.Mockup.Controls);
            }

            return result;
        }

        public string GetMockupNameFromId(string id) {
            if (string.IsNullOrWhiteSpace(id)) {
                return null;
            }

            var foundMockup = (from resx in Resources
                       where string.Compare(id, resx.ID, true) == 0
                       select resx).FirstOrDefault();
            
            if(foundMockup != null) {
                return foundMockup.Name;
            }
            else {
                return null;
            }
        }

        public List<(string id, string name)> GetMockupPages() {
            var result = new List<(string id, string name)>();
            foreach(var resx in Resources) {
                result.Add((id: resx.ID, name: resx.Name));
            }
            return result;
        }

        public string GetJson() {
            return JsonConvert.SerializeObject(this);
        }
        public static Bmpr GetFromJson(string json) {
            return JsonConvert.DeserializeObject<Bmpr>(json);
        }
    }

    public class Mockup {
        public float MeasuredHeight { get; set; } = -1;
        public float MeasuredWidth { get; set; } = -1;
        public float MockupHeight { get; set; } = -1;
        public float MockupWidth { get; set; } = -1;
        public List<Control> Controls{get;set;} = new List<Control>();

        public List<Control> GetControlsWithLinks() {
            return (from ctrl in Controls
                    where !string.IsNullOrWhiteSpace(ctrl.LinkId)
                    select ctrl).ToList();
        }
    }
   
    public class Control {
        public int ID { get; set; }
        public string CtrlType { get; set; }
        public float MeasuredHeight { get; set; }
        public float MeasuredWidth { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
        public string LinkId { get; set; }
        public float LocationX { get; set; }
        public float LocationY { get; set; }
        public float Zorder { get; set; }
    }
}

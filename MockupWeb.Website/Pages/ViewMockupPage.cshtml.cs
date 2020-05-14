using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using MockupWeb.Shared;
using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace MockupWeb.Website.Pages {
    public class ViewMockupPageModel : MockupBaseModel {
        public ViewMockupPageModel(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment) {

        }
        [BindProperty]
        public string MockupPath { get; set; }
        public string MockupImagePath { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
        public List<string> ScriptsToInclude { get; set; } = new List<string>();
        public List<string> CssToInclude { get; set; } = new List<string>();
        public string ImageUrlsString {
            get {
                string res = string.Empty;
                if (ImageUrls != null) {
                    res = string.Join(";", ImageUrls);
                }
                return res;
            }
        }
        public List<(string id, string name)> MockupPages { get; set; } = new List<(string id, string name)>();
        public string LinkedControlsJson { get; set; }
        public List<LinkedControl> LinkedControls { get; set; } = new List<LinkedControl>();

        private List<string> GetScriptsToInclude(string mockupImagePath) {
            List<string> scripts = new List<string>();
            // see if there is a .js file with same name next to the image
            string localpath = this.GetLocalMockupFilepath(mockupImagePath);

            string jsrelpath = $"{mockupImagePath}.js";
            if (System.IO.File.Exists(GetLocalMockupFilepath(jsrelpath))) {
                scripts.Add($"/mockups/{jsrelpath.Replace("\\", "/")}");
            }

            return scripts;
        }
        private List<string> GetCssToInclude(string mockupImagePath) {
            List<string> scripts = new List<string>();
            // see if there is a .js file with same name next to the image
            string localpath = this.GetLocalMockupFilepath(mockupImagePath);

            string jsrelpath = $"{mockupImagePath}.css";
            if (System.IO.File.Exists(GetLocalMockupFilepath(jsrelpath))) {
                scripts.Add($"/mockups/{jsrelpath.Replace("\\", "/")}");
            }

            return scripts;
        }

        public void OnGet(string mockupPath, string mockupName) {
            if (string.IsNullOrEmpty(mockupPath)) { throw new ArgumentNullException(nameof(mockupPath)); }

            if (!mockupPath.EndsWith(".json", StringComparison.OrdinalIgnoreCase)) {
                mockupPath += ".json";
            }

            MockupPath = mockupPath;
            string filepath = GetLocalMockupFilepath(mockupPath);
            string json = System.IO.File.ReadAllText(filepath);
            Bmpr bmprfile = Bmpr.GetFromJson(json);

            MockupPages = bmprfile.GetMockupPages();

            // find the resource to get the name of the file
            var resx = (from res in bmprfile.Resources
                        where string.Equals(mockupName, res.Name, StringComparison.OrdinalIgnoreCase)
                        select res).FirstOrDefault();

            string relfolderpath = mockupPath.Substring(0, mockupPath.Length - (Path.GetFileName(mockupPath).Length)).TrimEnd('/').TrimEnd('\\');

            ImageUrls = (from mp in MockupPages
                         select string.Format("/mockups/{0}/{1}.png", relfolderpath, mp.name)).ToList();

            MockupImagePath = $"/mockups/{relfolderpath}/{mockupName}.png";
            ScriptsToInclude = GetScriptsToInclude($@"{relfolderpath}\{mockupName}.png");
            CssToInclude = GetCssToInclude($@"{relfolderpath}\{mockupName}.png");

            if (resx != null) {
                var linkedControls = resx.Mockup.GetControlsWithLinks();

                var foundlc = from ctrl in linkedControls
                              select new LinkedControl {
                                  LinkId = ctrl.LinkId,
                                  MockupUrl = $"/ViewMockupPage?MockupPath={UriEscape(mockupPath,"brokenpath")}&mockupName={UriEscape(bmprfile.GetMockupNameFromId(ctrl.LinkId),"brokenname")}",
                                  LocationX = ctrl.LocationX,
                                  LocationY = ctrl.LocationY,
                                  MeasuredHeight = ctrl.MeasuredHeight,
                                  MeasuredWidth = ctrl.MeasuredWidth,
                                  Height = ctrl.Height,
                                  Width = ctrl.Width,
                                  MaxLocationX = GetMaxX(ctrl.LocationX, ctrl.Width, ctrl.MeasuredWidth),
                                  MaxLocationY = GetMaxY(ctrl.LocationY, ctrl.Height, ctrl.MeasuredHeight)
                              };
                if (foundlc != null && foundlc.Count() > 0) {
                    LinkedControlsJson = JsonConvert.SerializeObject(foundlc);
                    LinkedControls = foundlc.ToList();
                }
                // /ViewMockupPage?MockupPath=@mockupPathUrlEncoded&mockupName=@System.Net.WebUtility.UrlEncode(mpage.name)
            }
        }
        public string UriEscape(string str, string defaultValue) {
            if (!string.IsNullOrWhiteSpace(str)) {
                return System.Uri.EscapeUriString(str);
            }
            else {
                return defaultValue;
            }
        }
        private float GetMaxX(float locationX, float width, float measuredWidth) {
            float widthToUse = width;
            if (widthToUse <= 0) {
                widthToUse = measuredWidth;
            }
            if (widthToUse < 0) {
                widthToUse = 0;
            }

            return locationX + widthToUse;
        }
        private float GetMaxY(float locationY, float height, float measuredHeight) {
            float heightToUse = height;
            if (heightToUse <= 0) {
                heightToUse = measuredHeight;
            }
            if (heightToUse < 0) {
                heightToUse = 0;
            }

            return locationY + heightToUse;
        }
    }
    public class LinkedControl {
        public string LinkId { get; set; }
        public string MockupUrl { get; set; }
        public float LocationX { get; set; }
        public float LocationY { get; set; }
        public float MaxLocationX { get; set; }
        public float MaxLocationY { get; set; }
        public float MeasuredHeight { get; set; }
        public float MeasuredWidth { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }
    }
}
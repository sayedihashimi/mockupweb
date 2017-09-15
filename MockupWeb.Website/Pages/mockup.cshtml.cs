using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MockupWeb.Shared;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.IO;

namespace MockupWeb.Website.Pages {
    public class MockupModel : MockupBaseModel {
        public MockupModel(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment) {

        }

        [BindProperty]
        public string MockupPath { get; set; }
        public List<(string id, string name)> MockupPages { get; set; } = new List<(string id, string name)>();
        public List<string> ImageUrls { get; set; } = new List<string>();
        public string ImageUrlsString {
            get {
                string res = string.Empty;
                if(ImageUrls != null) {
                    res = string.Join(";", ImageUrls);
                }
                return res;
            }
        }

        public void OnGetOld(string mockupPath) {
            MockupPath = mockupPath;

            string filepath = GetLocalMockupFilepath(mockupPath);
            var bmprfile = new BmprReader().ReadFromFile(filepath);
            MockupPages = bmprfile.GetMockupPages();

            string relfolderpath = mockupPath.Substring(0, mockupPath.Length - (Path.GetFileName(mockupPath).Length)).TrimEnd('/').TrimEnd('\\');

            ImageUrls = (from mp in MockupPages
                       select string.Format("/mockups/{0}/{1}.png", relfolderpath, mp.name)).ToList();
        }
        
        public void OnGet(string mockupPath) {
            if (string.IsNullOrEmpty(mockupPath)) { throw new ArgumentNullException(nameof(mockupPath)); }

            if (!mockupPath.EndsWith(".json", StringComparison.OrdinalIgnoreCase)) {
                mockupPath += ".json";
            }

            MockupPath = mockupPath;
            string filepath = GetLocalMockupFilepath(mockupPath);
            string json = System.IO.File.ReadAllText(filepath);
            Bmpr bmprfile = Bmpr.GetFromJson(json);

            MockupPages = bmprfile.GetMockupPages();

            string relfolderpath = mockupPath.Substring(0, mockupPath.Length - (Path.GetFileName(mockupPath).Length)).TrimEnd('/').TrimEnd('\\');

            ImageUrls = (from mp in MockupPages
                         select string.Format("/mockups/{0}/{1}.png", relfolderpath, mp.name)).ToList();
        }
    }
}
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
        public List<(int id, string name)> MockupPages { get; set; } = new List<(int id, string name)>();
        public string MockupPagesJson { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
        public void OnGet(string mockupPath) {
            MockupPath = mockupPath;

            string filepath = GetLocalMockupFilepath(mockupPath);
            var bmprfile = new BmprReader().ReadFromFile(filepath);
            MockupPages = bmprfile.GetMockupPages();
            MockupPagesJson = JsonConvert.SerializeObject(MockupPages);

            string relfolderpath = mockupPath.Substring(0, mockupPath.Length - (Path.GetFileName(mockupPath).Length)).TrimEnd('/').TrimEnd('\\');

            ImageUrls = (from mp in MockupPages
                       select string.Format("/mockups/{0}/{1}.png", relfolderpath, mp.name)).ToList();
        }
    }
}
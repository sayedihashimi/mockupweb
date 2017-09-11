using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using MockupWeb.Shared;
using System.IO;

namespace MockupWeb.Website.Pages
{
    public class ViewMockupPageModel : MockupBaseModel {
        public ViewMockupPageModel(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment) {

        }
        [BindProperty]
        public string MockupPath { get; set; }
        public string MockupImagePath { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
        public string ImageUrlsString {
            get {
                string res = string.Empty;
                if (ImageUrls != null) {
                    res = string.Join(";", ImageUrls);
                }
                return res;
            }
        }
        public List<(int id, string name)> MockupPages { get; set; } = new List<(int id, string name)>();

        public void OnGet(string mockupPath, string mockupName) {
            MockupPath = mockupPath;

            string filepath = GetLocalMockupFilepath(mockupPath);
            var bmprfile = new BmprReader().ReadFromFile(filepath);
            MockupPages = bmprfile.GetMockupPages();

            // find the resource to get the name of the file
            var resx = (from res in bmprfile.Resources
                        where string.Equals(mockupName, res.Name, StringComparison.OrdinalIgnoreCase)
                        select res).FirstOrDefault();

            string relfolderpath = mockupPath.Substring(0, mockupPath.Length - (Path.GetFileName(mockupPath).Length)).TrimEnd('/').TrimEnd('\\');

            ImageUrls = (from mp in MockupPages
                         select string.Format("/mockups/{0}/{1}.png", relfolderpath, mp.name)).ToList();

            MockupImagePath = $"/mockups/{relfolderpath}/{mockupName}.png";
        }
    }
}
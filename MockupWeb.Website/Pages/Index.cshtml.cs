using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text.Encodings.Web;
using System.Net;

namespace MockupWeb.Website.Pages
{
    public class IndexModel : MockupBaseModel
    {
        public IndexModel(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment) {

        }
        public List<string> MockupNames { get; set; } = new List<string>();

        public List<string> MockupFiles { get; set; } = new List<string>();

        // key = relfilepath not URL Encoded
        // value = relfilepath URL Encoded
        public List<(string relpath, string relpathurl)> Mockups { get; set; } = new List<(string, string)>();

        public void OnGet()
        {
            MockupFiles.Clear();
            var bmprfiles = Directory.GetFiles(MockupRoot, "*.bmpr.json", SearchOption.AllDirectories);
            foreach(var file in bmprfiles) {
                string relpath = GetRelativePath(file, MockupRoot);
                Mockups.Add((relpath: relpath, relpathurl: System.Uri.EscapeUriString(relpath)));
            }
            
        }

        private string GetRelativePath(string filespec, string folder) {
            Uri pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString())) {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        private void remove() {
            var foo = new List<(string, string)>();
        }

    }
}

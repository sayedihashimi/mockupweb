using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using MockupWeb.Shared;

namespace MockupWeb.Website.Pages {
    public class UploadMockupModel : MockupBaseModel {
        public UploadMockupModel(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment) {

        }
        public void OnGet() {
            var foo = DateTime.Now;
        }

        private bool IsValidFilename(string filename) {
            return !(filename.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || filename.IndexOfAny(new char[] { '/', '\\' }) >= 0);
        }

        public async Task<IActionResult> OnPost(List<IFormFile> files, string mockupName) {
            string targetDir = Path.Combine(MockupRoot, mockupName);
            if (files == null || files.Count < 0 || string.IsNullOrWhiteSpace(mockupName)) {
                return Page();
            }

            if (!IsValidFilename(mockupName)) {
                throw new ArgumentException("mockupName has invalid characters. only characters for a filename are allowed here.", nameof(mockupName));
            }

            if (!Directory.Exists(targetDir)) {
                Directory.CreateDirectory(targetDir);
            }
            var addedFiles = new List<string>();
            foreach(var formFile in files) {
                if (formFile.Length > 0) {
                    string filename = Path.GetFileName(formFile.FileName);
                    if (!IsValidFilename(filename)) {
                        throw new ArgumentException("File to upload contains invalid characters for a filename.", formFile.FileName);
                    }
                    var targetFilepath = Path.Combine(targetDir, filename);
                    using (var stream = new FileStream(targetFilepath, FileMode.Create)) {
                        await formFile.CopyToAsync(stream);
                    }
                    addedFiles.Add(targetFilepath);
                }
            }

            // for each .bmpr file convert to .json and put into targetDir
            var reader = new BmprReader();
            foreach (var file in addedFiles) {
                var fi = new FileInfo(file);
                if (fi.Extension.Equals(".bmpr", StringComparison.OrdinalIgnoreCase)) {
                    var filename = $"{fi.Name}.json";
                    System.IO.File.WriteAllText(
                        Path.Combine(targetDir, filename),
                        reader.ReadFromFile(file).GetJson());
                }
            }




            return Redirect(@"/");
            // return Redirect(@"/ViewMockupPage?MockupPath=@mockupPathUrlEncoded&mockupName=@System.Net.WebUtility.UrlEncode(mpage.name)");
            

            var foo = "bar";
        }
    }
}
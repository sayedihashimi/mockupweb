using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MockupWeb.Website.Pages
{
    public class MockupBaseModel : PageModel {
        public MockupBaseModel(IHostingEnvironment hostingEnvironment) : base() {
            _hostingEnvironment = hostingEnvironment;
            _webRoot = _hostingEnvironment.WebRootPath;
            SetMockupRoot("mockups");
        }
        private IHostingEnvironment _hostingEnvironment;
        public string _webRoot { get; set; }
        public string MockupRoot { get; set; }
        public void SetMockupRoot(string reldirpath) {
            MockupRoot = Path.Combine(_webRoot, reldirpath);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace MockupWeb.Website.api {
    public class BaseApiController : Controller {
        public BaseApiController(IHostingEnvironment hostingEnvironment) : base() {
            _hostingEnvironment = hostingEnvironment;
            _webRoot = _hostingEnvironment.WebRootPath;
            SetMockupRoot("mockups");
        }
        private IHostingEnvironment _hostingEnvironment;
        public string _webRoot { get; set; }
        public string MockupRoot { get; set; }
        public void SetMockupRoot(string reldirpath) {
            if (!string.IsNullOrWhiteSpace(reldirpath)) {
                MockupRoot = Path.Combine(_webRoot, reldirpath);
            }
        }
    }
}
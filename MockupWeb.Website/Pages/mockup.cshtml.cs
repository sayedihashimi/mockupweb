using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MockupWeb.Website.Pages {
    public class mockupModel : PageModel {
        [BindProperty]
        public string MockupPath { get; set; }
        public void OnGet(string MockupPath) {
            MockupPath = MockupPath;



        }
    }
}
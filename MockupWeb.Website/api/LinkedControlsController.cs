using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using MockupWeb.Shared;
using Newtonsoft.Json;
using MockupWeb.Website.Pages;

namespace MockupWeb.Website.api {
    [Produces("application/json")]
    [Route("api/LinkedControls")]
    public class LinkedControlsController : BaseApiController {
        public LinkedControlsController(IHostingEnvironment hostingEnvironment) : base(hostingEnvironment) { }

        // [HttpGet("{mockupUrl?}")]
        [HttpGet]
        public string GetLinkedControlsForUrl(string mockupUrl) {
            // get path to mockup from the URL
            // http://localhost:63108/ViewMockupPage?MockupPath=mockupname%5Cpublish.2017.09.12-pub-spec.bmpr.json&mockupName=publish-tab-publishing
            string mockupPathPattern = @"MockupPath=(.*)&mockupName";
            Regex mockupPathRegex = new Regex(mockupPathPattern);
            var matches = mockupPathRegex.Matches(mockupUrl);
            // matches[0].Groups[1].Value
            string mockupPath = null;
            string mockupFilePath = null;
            string mockupName = null;
            if(matches != null || matches[0] != null && matches[0].Groups != null && matches[0].Groups.Count >= 2) {
                var res = matches[0].Groups[1].Value;
                mockupPath = WebUtility.UrlDecode(res);
                mockupFilePath = Path.Combine(MockupRoot, mockupPath);
            }

            var mockupNameRegex = new Regex("mockupName=(.*)");
            var mockupNameMatches = mockupNameRegex.Matches(mockupUrl);
            if (mockupNameMatches != null || mockupNameMatches[0] != null && mockupNameMatches[0].Groups != null && mockupNameMatches[0].Groups.Count >= 2) {
                var res = mockupNameMatches[0].Groups[1].Value;
                mockupName = WebUtility.UrlDecode(res);
            }

            if (!string.IsNullOrWhiteSpace(mockupFilePath)) {
                if (System.IO.File.Exists(mockupFilePath) && !string.IsNullOrWhiteSpace(mockupName)) {
                    string json = System.IO.File.ReadAllText(mockupFilePath);
                    Bmpr bmprfile = Bmpr.GetFromJson(json);

                    var resx = (from res in bmprfile.Resources
                                where string.Equals(mockupName, res.Name, StringComparison.OrdinalIgnoreCase)
                                select res).FirstOrDefault();

                    var linkedControls = resx.Mockup.GetControlsWithLinks();

                    var foundlc = from ctrl in linkedControls
                                  select new LinkedControl {
                                      LinkId = ctrl.LinkId,
                                      MockupUrl = $"/ViewMockupPage?MockupPath={UriEscape(mockupPath,"brokenlink")}&mockupName={UriEscape(bmprfile.GetMockupNameFromId(ctrl.LinkId),"brokenlink")}",
                                      LocationX = ctrl.LocationX,
                                      LocationY = ctrl.LocationY,
                                      MeasuredHeight = ctrl.MeasuredHeight,
                                      MeasuredWidth = ctrl.MeasuredWidth,
                                      Height = ctrl.Height,
                                      Width = ctrl.Width,
                                      MaxLocationX = GetMaxX(ctrl.LocationX, ctrl.Width, ctrl.MeasuredWidth),
                                      MaxLocationY = GetMaxY(ctrl.LocationY, ctrl.Height, ctrl.MeasuredHeight)
                                  };

                    var linkedControlsJson = JsonConvert.SerializeObject(foundlc);
                    return linkedControlsJson;
                }
                else {
                    throw new ArgumentException(string.Format("mockup file not found at: [{0}] or name is null [{1}]", mockupPath, mockupName));
                }
            }

            return null;
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
}
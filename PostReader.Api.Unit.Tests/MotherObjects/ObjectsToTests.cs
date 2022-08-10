using PostReader.Api.Common.CommonModels.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostReader.Api.Unit.Tests.MotherObjects
{
    internal static class ObjectsToTests
    {
        internal static EuropePmcSettings GetSettings()
        {
            return new()
            {
                BaseUrl = "http://europepmc.org/",
                UrlBasePart = "search?query=",
                UrlAjaxFirst = "api/get/articleApi?query=",
                UrlAjaxMiddle = "&cursorMark=*&format=json&pageSize=",
                UrlAjaxEnd = "&sort=Relevance&synonym=FALSE",
                DefaultListSize = 25,
                RegexQuntity = "(hitCount.:)(?<values>.[^,]*)"
            };
        }
    }
}

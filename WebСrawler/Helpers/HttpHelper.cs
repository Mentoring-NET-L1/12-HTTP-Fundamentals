using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebСrawler.Helpers
{
    internal static class HttpHelper
    {
        public static string GetShortContentType(string contentType)
        {
            var startIndex = contentType.IndexOf('/') + 1;
            return contentType.Substring(startIndex);
        }
    }
}

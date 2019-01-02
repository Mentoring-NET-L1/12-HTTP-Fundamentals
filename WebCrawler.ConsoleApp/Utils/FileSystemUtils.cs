using System.IO;
using System.Text.RegularExpressions;

namespace WebCrawler.ConsoleApp.Utils
{
    internal class FileSystemUtils
    {
        public static string EscapeInvalidFilePathSymbols(string filePath)
        {
            string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
            var regex = new Regex($"[{Regex.Escape(regexSearch)}]");
            return regex.Replace(filePath, "");
        }
    }
}

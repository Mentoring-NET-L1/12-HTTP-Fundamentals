using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.ConsoleApp.Utils;
using WebСrawler;

namespace WebCrawler.ConsoleApp
{
    internal class Program
    {
        private const string _rootDirectory = "Content";
        private const string _fileDirectory = _rootDirectory + "\\Files";

        private static void Main(string[] args)
        {
            Directory.CreateDirectory(_rootDirectory);
            Directory.CreateDirectory(_fileDirectory);

            var crawler = new Crawler();
            crawler.ResourceLoaded += OnResourceLoaded;
            crawler.Browse(new Uri("https://ru.wikipedia.org/wiki/Wget")).Wait();

            Console.ReadLine();
        }

        private static void OnResourceLoaded(object sender, ResourceLoadedEventArgs args)
        {
            Console.WriteLine(args.Uri);

            var directory = args.ContentType == "html" ? _rootDirectory : _fileDirectory;

            var escapedUri = FileSystemUtils.EscapeInvalidFilePathSymbols(args.Uri.ToString());
            escapedUri = escapedUri.Substring(0, Math.Min(escapedUri.Length, 100));
            var filePath = Path.Combine(directory, escapedUri) + "." + args.ContentType;
            using (var fileStream = File.Create(filePath))
            {
                args.Content.CopyTo(fileStream);
            }
        }
    }
}

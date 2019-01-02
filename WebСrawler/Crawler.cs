using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CsQuery;
using WebСrawler.Helpers;

namespace WebСrawler
{
    public class Crawler
    {
        private readonly HttpClient _httpClient;
        private CrawlerSettings _settings;

        public Crawler()
        {
            _httpClient = new HttpClient();
        }

        public CrawlerSettings Settings { get; set; }

        public event EventHandler<ResourceLoadedEventArgs> ResourceLoaded;

        public async Task Browse(Uri startUri)
        {
            if (startUri == null)
                throw new ArgumentNullException(nameof(startUri));

            _settings = Settings ?? CrawlerSettings.GetDefault();
            await BrowseAsync(startUri, 0);
            _settings = null;
        }

        protected virtual void OnResourceLoaded(ResourceLoadedEventArgs args)
        {
            ResourceLoaded?.Invoke(this, args);
        }

        private async Task BrowseAsync(Uri startUri, int depth)
        {
            if (depth > _settings.Depth)
                return;

            var response = await _httpClient.GetAsync(startUri);
            var contentType = HttpHelper.GetShortContentType(response.Content.Headers.ContentType.MediaType);

            if (SkipType(contentType))
                return;

            OnResourceLoaded(new ResourceLoadedEventArgs(
                startUri, 
                await response.Content.ReadAsStreamAsync(),
                contentType));

            if (contentType == "html")
            {
                var tasks = new List<Task>();

                var dom = (CQ)(await response.Content.ReadAsStringAsync());
                var links = dom["a"].Select(a => a.Attributes["href"])
                    .Union(dom["img"].Select(img => img.Attributes["src"]));
                foreach (var link in links.Where(l => !string.IsNullOrEmpty(l)))
                {
                    var uri = ToAbsoluteUri(startUri, link);
                    if ((uri != null) && !(uri.Host != startUri.Host && _settings.DomainRestriction == DomainRestriction.InsideDomain))
                        tasks.Add(BrowseAsync(uri, depth + 1));
                }

                Task.WaitAll(tasks.ToArray());
            }
        }

        private static Uri ToAbsoluteUri(Uri baseUri, string link)
        {
            if (link == null || link.Length <= 1 || link[0] == '#')
                return null;

            string absoluteLink;
            if (link[0] == 'h')
                absoluteLink = link;
            else if (link[0] == '/' && link[1] == '/')
                absoluteLink = $"{baseUri.Scheme}:{link}";
            else
                absoluteLink = $"{baseUri.Scheme}://{baseUri.Host}{link}";

            try
            {
                return new Uri(absoluteLink);
            }
            catch
            {
                return null;
            }
        }

        private bool SkipType(string type)
        {
            return !_settings.ResourcesRestriction.Contains(type);
        }
    }
}

using System;
using System.IO;

namespace WebСrawler
{
    public class ResourceLoadedEventArgs : EventArgs
    {
        public ResourceLoadedEventArgs(Uri uri, Stream content, string contentType)
        {
            Uri = uri;
            Content = content;
            ContentType = contentType;
        }

        public Uri Uri { get; }

        public Stream Content { get; }

        public string ContentType { get; }
    }
}

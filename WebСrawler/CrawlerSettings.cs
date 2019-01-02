namespace WebСrawler
{
    public class CrawlerSettings
    {
        public int Depth { get; set; }

        public DomainRestriction DomainRestriction { get; set; }

        public string ResourcesRestriction { get; set; }

        public static CrawlerSettings GetDefault()
        {
            return new CrawlerSettings() {
                Depth = 1,
                DomainRestriction = DomainRestriction.InsideDomain,
                ResourcesRestriction = "html,jpg,png",
            };
        }
    }
}

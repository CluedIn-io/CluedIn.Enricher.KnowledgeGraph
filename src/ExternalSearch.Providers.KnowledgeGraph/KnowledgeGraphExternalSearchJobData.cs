using System.Collections.Generic;
using CluedIn.Core.Crawling;

namespace CluedIn.ExternalSearch.Providers.KnowledgeGraph
{
    public class KnowledgeGraphExternalSearchJobData : CrawlJobData
    {
        public KnowledgeGraphExternalSearchJobData(IDictionary<string, object> configuration)
        {
            AcceptedEntityType = GetValue<string>(configuration, Constants.KeyName.AcceptedEntityType);
            OrganizationNameKey = GetValue<string>(configuration, Constants.KeyName.OrganizationNameKey);
            WebsiteKey = GetValue<string>(configuration, Constants.KeyName.WebsiteKey);
        }

        public IDictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object> {
                { Constants.KeyName.AcceptedEntityType, AcceptedEntityType },
                { Constants.KeyName.OrganizationNameKey, OrganizationNameKey },
                { Constants.KeyName.WebsiteKey, WebsiteKey }
            };
        }
        public string AcceptedEntityType { get; set; }
        public string OrganizationNameKey { get; set; }
        public string WebsiteKey { get; set; }
    }
}

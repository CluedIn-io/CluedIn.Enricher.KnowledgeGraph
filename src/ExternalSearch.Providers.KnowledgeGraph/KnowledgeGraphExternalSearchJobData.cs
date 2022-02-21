using System.Collections.Generic;
using CluedIn.Core.Crawling;

namespace CluedIn.ExternalSearch.Providers.KnowledgeGraph
{
    public class KnowledgeGraphExternalSearchJobData : CrawlJobData
    {
        public KnowledgeGraphExternalSearchJobData(IDictionary<string, object> configuration)
        {
        }

        public IDictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object> {
                
            };
        }
    }
}

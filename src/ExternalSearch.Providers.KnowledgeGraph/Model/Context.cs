using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.KnowledgeGraph.Model
{
    public class Context
    {
        [JsonProperty("@@vocab")]
        public string vocab { get; set; }
        public string goog { get; set; }
        public string EntitySearchResult { get; set; }
        public string detailedDescription { get; set; }
        public string resultScore { get; set; }
        public string kg { get; set; }
    }
}

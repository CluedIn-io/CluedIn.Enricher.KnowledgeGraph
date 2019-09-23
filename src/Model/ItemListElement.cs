using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.KnowledgeGraph.Model
{
    public class ItemListElement
    {
        [JsonProperty("@type")]
        public string type { get; set; }
        public Result result { get; set; }
        public double resultScore { get; set; }
    }
}
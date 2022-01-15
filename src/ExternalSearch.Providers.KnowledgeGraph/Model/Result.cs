using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.KnowledgeGraph.Model
{
    public class Result
    {
        [JsonProperty("@id")]
        public string id { get; set; }
        public string name { get; set; }
        [JsonProperty("@type")]
        public List<string> type { get; set; }
        public string description { get; set; }
        public Image image { get; set; }
        public DetailedDescription detailedDescription { get; set; }
        public string url { get; set; }
    }
}
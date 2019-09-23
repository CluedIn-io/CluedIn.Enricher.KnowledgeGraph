using System.Collections.Generic;
using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.KnowledgeGraph.Model
{
    public class KnowledgeResponse
    {
        [JsonProperty("@context")]
        public Context context { get; set; }
        [JsonProperty("@type")]
        public string type { get; set; }
        public List<ItemListElement> itemListElement { get; set; }
    }
}
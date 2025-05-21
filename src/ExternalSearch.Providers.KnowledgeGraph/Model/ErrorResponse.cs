using Newtonsoft.Json;

namespace CluedIn.ExternalSearch.Providers.KnowledgeGraph.Model
{
    public class ErrorDetails
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class ErrorResponse
    {
        [JsonProperty("error")]
        public ErrorDetails Error { get; set; }
    }
}

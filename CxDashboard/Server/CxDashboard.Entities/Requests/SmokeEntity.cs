using Newtonsoft.Json;

namespace CxDashboard.Entities.Requests
{
    public class SmokeEntityRequest
    {
        [JsonProperty("pipeline.keyword")]
        public string Pipeline { get; set; }

        [JsonProperty("state.keyword")]
        public string State { get; set; }

        [JsonProperty("stage.keyword")]
        public string Stage { get; set; }

        [JsonProperty("status.keyword")]
        public string Status { get; set; }

        [JsonProperty("@timestamp")]
        public string Timestamp { get; set; }
    }
}

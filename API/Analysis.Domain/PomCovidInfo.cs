using Newtonsoft.Json;

namespace Analysis.Domain
{
    public class PomCovidInfo
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("confirmed")]
        public int Confirmed { get; set; }

        [JsonProperty("deaths")]
        public int Deaths { get; set; }

        [JsonProperty("recovered")]
        public int Recovered { get; set; }
    }
}

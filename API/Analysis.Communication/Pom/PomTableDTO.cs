using Newtonsoft.Json;

namespace Analysis.Communication.Pom
{
    public class PomTableDTO
    {
        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("totalCases")]
        public int TotalCases { get; set; }

        [JsonProperty("newCases")]
        public int NewCases { get; set; }

        [JsonProperty("totalDeaths")]
        public int TotalDeaths { get; set; }

        [JsonProperty("newDeaths")]
        public int NewDeaths { get; set; }

        [JsonProperty("totalRecovered")]
        public int TotalRecovered { get; set; }

        [JsonProperty("newRecovered")]
        public int NewRecovered { get; set; }

        [JsonProperty("totalCasesPer1M")]
        public int TotalCasesPer1M { get; set; }

        [JsonProperty("totalDeathsPer1M")]
        public int TotalDeathsPer1M { get; set; }
    }
}

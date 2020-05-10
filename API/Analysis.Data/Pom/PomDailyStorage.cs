using Analysis.Common;
using Analysis.Domain;
using System.Collections.Generic;

namespace Analysis.Data.Pom
{
    public class PomDailyStorage : PomBaseStorage
    {
        public PomDailyStorage(PomRawStorage pomStorage, AppSettings appSettings) : 
            base(pomStorage, appSettings)
        {
        }

        public override PomCovidInfo GetSummaryByCountry(string country)
        {
            var countryData = GetByCountry(country);
            return (countryData.Count > 0) ? countryData[countryData.Count - 1] : new PomCovidInfo();
        }

        protected override Dictionary<string, List<PomCovidInfo>> Transform(Dictionary<string, List<PomCovidInfo>> data)
        {
            return data;
        }
    }
}

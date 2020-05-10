using Analysis.Common;
using Analysis.Domain;
using System.Collections.Generic;

namespace Analysis.Data.Pom
{
    public class PomSeriesStorage : PomBaseStorage
    {
        public PomSeriesStorage(PomRawStorage pomStorage, AppSettings appSettings) : base(pomStorage, appSettings)
        {
        }

        public override PomCovidInfo GetSummaryByCountry(string country)
        {
            var res = new PomCovidInfo();
            var countryData = GetByCountry(country);
            if (countryData.Count == 0)
                return res;
            res.Date = countryData[countryData.Count - 1].Date;
            for (int i = 0; i < countryData.Count; i++)
            {
                AddDayValues(res, countryData[i]);
            }
            return res;
        }

        protected override Dictionary<string, List<PomCovidInfo>> Transform(Dictionary<string, List<PomCovidInfo>> data)
        {
            var res = new Dictionary<string, List<PomCovidInfo>>();
            foreach (var country in data)
            {
                var countryRes = new List<PomCovidInfo>();
                var series = country.Value;
                for (int i = 1; i < series.Count; i++)
                {
                    countryRes.Add(Diff(series[i-1], series[i]));
                }
                res[country.Key] = countryRes;
            }
            return res;
        }

        private PomCovidInfo Diff(PomCovidInfo pci1, PomCovidInfo pci2)
        {
            return new PomCovidInfo
            {
                Date = pci2.Date,
                Confirmed = pci2.Confirmed - pci1.Confirmed,
                Deaths = pci2.Deaths - pci1.Deaths,
                Recovered = pci2.Recovered - pci1.Recovered
            };
        }
    }
}

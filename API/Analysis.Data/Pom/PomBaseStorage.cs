using Analysis.Common;
using Analysis.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Analysis.Data.Pom
{
    public abstract class PomBaseStorage
    {
        protected readonly PomRawStorage pomRawStorage;
        protected Dictionary<string, List<PomCovidInfo>> all;
        protected readonly string summaryKey;

        public Dictionary<string, List<PomCovidInfo>> All
        {
            get
            {
                this.all = this.all ?? Parse();
                return this.all;
            }
        }

        public PomBaseStorage(PomRawStorage pomRawStorage, AppSettings appSettings)
        {
            this.pomRawStorage = pomRawStorage;
            this.summaryKey = appSettings.PomCovid.SummaryKey;
        }

        public Dictionary<string, List<PomCovidInfo>> GetAll()
        {
            return All;
        }

        public List<PomCovidInfo> GetByCountry(string country)
        {
            return All.ContainsKey(country) ? All[country] : new List<PomCovidInfo>();
        }

        public abstract PomCovidInfo GetSummaryByCountry(string country);

        private Dictionary<string, List<PomCovidInfo>> Parse()
        {
            var json = this.pomRawStorage.Json;
            var outcomeData = JsonConvert.DeserializeObject<Dictionary<string, List<PomCovidInfo>>>(json);
            outcomeData = Transform(outcomeData);
            if (!outcomeData.ContainsKey(this.summaryKey))
            {
                outcomeData[this.summaryKey] = CalculateTotalField(outcomeData);
            }
            return new Dictionary<string, List<PomCovidInfo>>(outcomeData, StringComparer.InvariantCultureIgnoreCase);
        }

        protected abstract Dictionary<string, List<PomCovidInfo>> Transform(Dictionary<string, List<PomCovidInfo>> data);

        protected List<PomCovidInfo> CalculateTotalField(Dictionary<string, List<PomCovidInfo>> data)
        {
            var anyCountry = data.FirstOrDefault().Value;
            var total = new List<PomCovidInfo>();
            for (int i = 0; i < anyCountry.Count; i++)
            {
                total.Add(new PomCovidInfo 
                {
                    Date = anyCountry[i].Date
                });
            }
            foreach (var country in data)
            {
                var series = country.Value;
                for (int i = 0; i < series.Count; i++)
                {
                    AddDayValues(total[i], series[i]);
                }
            }
            return total;
        }

        protected static void AddDayValues(PomCovidInfo oneDayFromTotal, PomCovidInfo oneDayFromSeries)
        {
            oneDayFromTotal.Confirmed += oneDayFromSeries.Confirmed;
            oneDayFromTotal.Deaths += oneDayFromSeries.Deaths;
            oneDayFromTotal.Recovered += oneDayFromSeries.Recovered;
        }
    }
}

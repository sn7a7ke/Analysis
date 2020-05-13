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
        protected List<string> countries;
        protected readonly string summaryKey;

        public PomBaseStorage(PomRawStorage pomRawStorage, AppSettings appSettings)
        {
            this.pomRawStorage = pomRawStorage;
            this.summaryKey = appSettings.PomCovid.SummaryKey;
        }

        public Dictionary<string, List<PomCovidInfo>> GetAll()
        {
            this.all = this.all ?? Parse();
            return this.all;
        }

        public List<PomCovidInfo> GetByCountry(string country)
        {
            return GetAll().ContainsKey(country) ? GetAll()[country] : new List<PomCovidInfo>();
        }

        public PomCovidInfo GetSummary()
        {
            return GetSummaryByCountry(this.summaryKey);
        }

        public abstract PomCovidInfo GetSummaryByCountry(string country);

        public List<string> GetAllCountries()
        {
                this.countries = this.countries ?? GetAll().Keys.ToList();
                return this.countries;
        }

        private Dictionary<string, List<PomCovidInfo>> Parse()
        {
            var json = this.pomRawStorage.Json;
            if (string.IsNullOrEmpty(json))
                return new Dictionary<string, List<PomCovidInfo>>();
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

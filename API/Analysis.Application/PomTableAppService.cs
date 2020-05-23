using Analysis.Communication.Pom;
using Analysis.Data.Pom;
using Analysis.Domain;
using System.Collections.Generic;

namespace Analysis.Application
{
    public class PomTableAppService
    {
        private readonly PomTotalStorage pomTotalStorage;
        private readonly PomDailyStorage pomDailyStorage;

        public PomTableAppService(PomTotalStorage pomTotalStorage, PomDailyStorage pomDailyStorage)
        {
            this.pomTotalStorage = pomTotalStorage;
            this.pomDailyStorage = pomDailyStorage;
        }

        public List<PomTableDTO> GetAll()
        {
            var countries = this.pomTotalStorage.GetAllCountries();
            var res = new List<PomTableDTO>(countries.Count);
            PomTableDTO item;
            PomCovidInfo summary;
            PomCovidInfo lastDay;
            for (int i = 0; i < countries.Count; i++)
            {
                summary = this.pomTotalStorage.GetSummaryByCountry(countries[i]);
                lastDay = this.pomDailyStorage.GetLastDayByCountry(countries[i]);
                item = new PomTableDTO
                {
                    Country = countries[i],
                    TotalCases = summary.Confirmed,
                    NewCases = lastDay.Confirmed,
                    TotalDeaths = summary.Deaths,
                    NewDeaths = lastDay.Deaths,
                    TotalRecovered = summary.Recovered,
                    NewRecovered = lastDay.Recovered
                };
                res.Add(item);
            }
            return res;
        }
    }
}

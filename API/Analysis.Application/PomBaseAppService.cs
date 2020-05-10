using Analysis.Data.Pom;
using Analysis.Domain;
using System.Collections.Generic;

namespace Analysis.Application
{
    public class PomBaseAppService
    {
        private readonly PomBaseStorage pomStorage;

        public PomBaseAppService(PomBaseStorage pomStorage)
        {
            this.pomStorage = pomStorage;
        }

        public Dictionary<string, List<PomCovidInfo>> GetAll()
        {
            return this.pomStorage.GetAll();
        }

        public List<PomCovidInfo> GetByCountry(string country)
        {
            return this.pomStorage.GetByCountry(country);
        }

        public PomCovidInfo GetSummary()
        {
            return this.pomStorage.GetSummary();
        }

        public PomCovidInfo GetSummaryByCountry(string country)
        {
            return this.pomStorage.GetSummaryByCountry(country);
        }

        public List<string> GetAllCountries()
        {
            return this.pomStorage.GetAllCountries();
        }
    }
}

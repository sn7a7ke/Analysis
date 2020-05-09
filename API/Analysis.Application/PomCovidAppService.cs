using Analysis.Application.Interface;
using Analysis.Data.Pom;
using Analysis.Infrastructure.Interface;
using System.Threading.Tasks;

namespace Analysis.Application
{
    public class PomCovidAppService : ICovidAppService
    {
        private readonly IExternalStorage externalStorage;
        private readonly PomStorage pomStorage;

        public string Json
        { 
            get 
            {
                if (string.IsNullOrEmpty(this.pomStorage.Json))
                    RefreshDataAsync().Wait();
                return this.pomStorage.Json; 
            }
        }

        public PomCovidAppService(IExternalStorage externalStorage, PomStorage pomStorage)
        {
            this.externalStorage = externalStorage;
            this.pomStorage = pomStorage;
        }

        public async Task RefreshDataAsync()
        {
            var jsonCovid = await this.externalStorage.GetAsync();
            this.pomStorage.Save(jsonCovid);
        }
    }
}

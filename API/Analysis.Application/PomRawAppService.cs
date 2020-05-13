using Analysis.Application.Interface;
using Analysis.Data.Pom;
using Analysis.Infrastructure.Interface;
using System.Threading.Tasks;

namespace Analysis.Application
{
    public class PomRawAppService : IRawAppService
    {
        private readonly IExternalStorage externalStorage;
        private readonly PomRawStorage pomStorage;

        public string Json
        { 
            get 
            {
                if (string.IsNullOrEmpty(this.pomStorage.Json))
                    RefreshDataAsync().Wait();
                return this.pomStorage.Json; 
            }
        }

        public PomRawAppService(IExternalStorage externalStorage, PomRawStorage pomStorage)
        {
            this.externalStorage = externalStorage;
            this.pomStorage = pomStorage;
        }

        public async Task<bool> RefreshDataAsync()
        {
            var jsonCovid = await this.externalStorage.GetAsync();
            if (string.IsNullOrEmpty(jsonCovid))
                return false;
            this.pomStorage.Save(jsonCovid);
            return true;
        }
    }
}

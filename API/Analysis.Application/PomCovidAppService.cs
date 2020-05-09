using Analysis.Application.Interface;
using Analysis.Data.Interface;
using Analysis.Infrastructure.Interface;
using System.Threading.Tasks;

namespace Analysis.Application
{
    public class PomCovidAppService : ICovidAppService
    {
        private readonly IExternalStorage externalStorage;
        private readonly IStorage storage;
        private string jsonCovid;

        public string JsonCovid
        { 
            get 
            {
                if (string.IsNullOrEmpty(this.jsonCovid))
                    this.jsonCovid = this.storage.Load();
                if (string.IsNullOrEmpty(this.jsonCovid))
                    RefreshDataAsync().Wait();
                return jsonCovid; 
            }
        }

        public PomCovidAppService(IExternalStorage externalStorage, IStorage storage)
        {
            this.externalStorage = externalStorage;
            this.storage = storage;
        }

        public async Task RefreshDataAsync()
        {
            this.jsonCovid = await this.externalStorage.GetAsync();
            this.storage.Save(this.jsonCovid);
        }
    }
}

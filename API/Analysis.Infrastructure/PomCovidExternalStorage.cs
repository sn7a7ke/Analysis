using Analysis.Common;
using Analysis.Infrastructure.Interface;
using System.Net.Http;
using System.Threading.Tasks;

namespace Analysis.Infrastructure
{
    public class PomCovidExternalStorage : IExternalStorage
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly PomCovidSettings pomCovidSettings;

        public PomCovidExternalStorage(AppSettings appSettings)
        {
            this.pomCovidSettings = appSettings.PomCovid;
        }

        public async Task<string> GetAsync()
        {
            return await client.GetStringAsync(pomCovidSettings.Url);
        }
    }
}

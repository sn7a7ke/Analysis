using Analysis.Infrastructure.Interface;
using System.Net.Http;
using System.Threading.Tasks;

namespace Analysis.Infrastructure
{
    public class PomCovidExternalStorage : IExternalStorage
    {
        private static readonly HttpClient client = new HttpClient();
        public const string POM_URL = "https://pomber.github.io/covid19/timeseries.json";

        public async Task<string> GetAsync()
        {
            return await client.GetStringAsync(POM_URL);
        }
    }
}

using System.Threading.Tasks;

namespace Analysis.Application.Interface
{
    public interface ICovidAppService
    {
        string JsonCovid { get; }
        Task RefreshDataAsync();
    }
}

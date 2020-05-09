using System.Threading.Tasks;

namespace Analysis.Application.Interface
{
    public interface ICovidAppService
    {
        string GetJson();
        Task RefreshDataAsync();
    }
}

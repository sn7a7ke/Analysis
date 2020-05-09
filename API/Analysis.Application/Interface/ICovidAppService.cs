using System.Threading.Tasks;

namespace Analysis.Application.Interface
{
    public interface ICovidAppService
    {
        string Json { get; }
        Task RefreshDataAsync();
    }
}

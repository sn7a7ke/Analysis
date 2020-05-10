using System.Threading.Tasks;

namespace Analysis.Application.Interface
{
    public interface IRawAppService
    {
        string Json { get; }
        Task RefreshDataAsync();
    }
}

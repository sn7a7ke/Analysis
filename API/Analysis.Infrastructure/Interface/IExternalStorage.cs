using System.Threading.Tasks;

namespace Analysis.Infrastructure.Interface
{
    public interface IExternalStorage
    {
        Task<string> GetAsync();
    }
}

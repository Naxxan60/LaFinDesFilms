using System.Threading.Tasks;

namespace Services
{
    public interface IOmdbApiService
    {
        Task<OmdbApiResult> GetOmdbInfoAsync(string id);
    }
}

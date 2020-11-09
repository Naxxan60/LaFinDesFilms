using DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ICacheManagerService
    {
        public Task<List<TopRatedMovie>> GetTopRatedMoviesFromCacheAsync();
        public bool TryGetTopRatedMoviesFromCache(out List<TopRatedMovie> list);
        public void RemoveTopRatedMoviesCache();
    }
}

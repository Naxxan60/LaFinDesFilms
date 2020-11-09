using DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class TopRatedMoviesService : ITopRatedMoviesService
    {
        private readonly ITopRatedMoviesRepo _topRatedMoviesRepo;
        private readonly ICacheManagerService _cacheManagerService;

        public TopRatedMoviesService(ICacheManagerService cacheManagerService, ITopRatedMoviesRepo topRatedMoviesRepo)
        {
            _cacheManagerService = cacheManagerService;
            _topRatedMoviesRepo = topRatedMoviesRepo;
        }

        public async Task<List<TopRatedMovie>> GetTopRatedMoviesAsync(int numberOfEntriesToGet = 0)
        {
            var list = await _cacheManagerService.GetTopRatedMoviesFromCacheAsync();
            if (numberOfEntriesToGet == 0)
            {
                return list;
            }
            else
            {
                return list.Take(numberOfEntriesToGet).ToList();
            }
        }

        public TopRatedMovie GetTopRatedMovieById(string id)
        {
            return _cacheManagerService.TryGetTopRatedMoviesFromCache(out var cacheEntry)
                ? cacheEntry.Find(m => m.Id == id)
                : _topRatedMoviesRepo.GetTopRatedMovieById(id);
        }
    }
}

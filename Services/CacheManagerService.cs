using DTO;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class CacheManagerService : ICacheManagerService
    {
        private readonly ITopRatedMoviesRepo _topRatedMoviesRepo;
        private readonly IMemoryCache _cache;

        public CacheManagerService(ITopRatedMoviesRepo topRatedMoviesRepo, IMemoryCache memoryCache)
        {
            _topRatedMoviesRepo = topRatedMoviesRepo;
            _cache = memoryCache;
        }

        public async Task<List<TopRatedMovie>> GetTopRatedMoviesFromCacheAsync()
        {
            var cacheEntry = await _cache.GetOrCreateAsync(ConstantHelper.TOP_RATED_MOVIES_CACHE_KEY, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromMinutes(5);
                return Task.FromResult(_topRatedMoviesRepo.GetTopRatedMoviesOrderDesc().ToList());
            });
            return cacheEntry;
        }

        public bool TryGetTopRatedMoviesFromCache(out List<TopRatedMovie> list)
        {
            if (_cache.TryGetValue(ConstantHelper.TOP_RATED_MOVIES_CACHE_KEY, out list))
            {
                return true;
            }
            return false;
        }

        public void RemoveTopRatedMoviesCache()
        {
            _cache.Remove(ConstantHelper.TOP_RATED_MOVIES_CACHE_KEY);
        }
    }
}

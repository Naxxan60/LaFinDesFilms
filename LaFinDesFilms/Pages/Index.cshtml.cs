using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Services;

namespace LaFinDesFilms.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICacheManagerService _cacheManagerService;

        public IndexModel(ILogger<IndexModel> logger, ICacheManagerService cacheManagerService)
        {
            _logger = logger;
            _cacheManagerService = cacheManagerService;
        }

        public void OnGet()
        {
            _cacheManagerService.GetTopRatedMoviesFromCacheAsync();
        }
    }
}

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Services;

namespace LaFinDesFilms.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly ICacheManagerService _cacheManagerService;

        public PrivacyModel(ILogger<PrivacyModel> logger, ICacheManagerService cacheManagerService)
        {
            _logger = logger;
            _cacheManagerService = cacheManagerService;
        }

        public void OnGet()
        {
            _logger.LogInformation("test");
        }

        public void OnGetRemoveCache()
        {
            _cacheManagerService.RemoveTopRatedMoviesCache();
        }
    }
}

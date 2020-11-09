using System.Threading.Tasks;
using DTO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace LaFinDesFilms.Pages
{
    public class MovieModel : PageModel
    {
        private readonly ITopRatedMoviesService _topRatedMoviesService;
        private readonly IOmdbApiService _omdbApiService;

        public string Id { get; set; }

        public TopRatedMovie TopRatedMovie { get; set; }
        public OmdbApiResult OmdbMovieInfo { get; private set; }

        public MovieModel(ITopRatedMoviesService topRatedMoviesService, IOmdbApiService omdbApiService)
        {
            _topRatedMoviesService = topRatedMoviesService;
            _omdbApiService = omdbApiService;
        }

        public async Task OnGetAsync(string id)
        {
            Id = id;
            TopRatedMovie = _topRatedMoviesService.GetTopRatedMovieById(id);
            OmdbMovieInfo = await _omdbApiService.GetOmdbInfoAsync(id);
        }
    }
}

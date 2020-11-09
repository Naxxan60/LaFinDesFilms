using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace LaFinDesFilms.Pages
{
    public class TopVotedMoviesModel : PageModel
    {
        public IEnumerable<TopRatedMovie> TopRatedMovies { get; set; }
        public ITopRatedMoviesService topRatedMoviesService { get; set; }

        public TopVotedMoviesModel(ITopRatedMoviesService _topRatedMoviesService)
        {
            topRatedMoviesService = _topRatedMoviesService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            TopRatedMovies = await topRatedMoviesService.GetTopRatedMoviesAsync(10);
            return Page();
        }
    }
}

using DTO;
using System.Collections.Generic;

namespace Services
{
    public interface ITopRatedMoviesService
    {
        public IEnumerable<TopRatedMovie> GetTopRatedMovies();
        TopRatedMovie GetTopRatedMovie(string id);
    }
}

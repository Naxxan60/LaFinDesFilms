using DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface ITopRatedMoviesService
    {
        public Task<List<TopRatedMovie>> GetTopRatedMoviesAsync(int numberOfEntriesToGet = 0);
        TopRatedMovie GetTopRatedMovieById(string id);
    }
}

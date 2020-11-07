using DataAccess;
using DTO;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class TopRatedMoviesService : ITopRatedMoviesService
    {
        private  FilmContext context { get; set; }

        public TopRatedMoviesService(FilmContext _context)
        {
            this.context = _context;
        }

        public IEnumerable<TopRatedMovie> GetTopRatedMovies()
        {
            return context.TopRatedMovies.OrderByDescending(m => m.NbVote).Take(10);
        }

        public TopRatedMovie GetTopRatedMovie(string id)
        {
            return context.TopRatedMovies.Find(id);
        }
    }
}

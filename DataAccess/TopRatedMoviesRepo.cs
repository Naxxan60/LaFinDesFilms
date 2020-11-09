using DTO;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class TopRatedMoviesRepo : ITopRatedMoviesRepo
    {
        private FilmContext _context { get; set; }
        public TopRatedMoviesRepo(FilmContext context)
        {
            _context = context;
        }

        public IEnumerable<TopRatedMovie> GetTopRatedMoviesOrderDesc()
        {
            return _context.TopRatedMovies.OrderByDescending(m => m.NbVote);
        }

        public TopRatedMovie GetTopRatedMovieById(string id)
        {
            return _context.TopRatedMovies.Find(id);
        }
    }
}

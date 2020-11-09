using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public interface ITopRatedMoviesRepo
    {
        public IEnumerable<TopRatedMovie> GetTopRatedMoviesOrderDesc();
        public TopRatedMovie GetTopRatedMovieById(string id);
    }
}

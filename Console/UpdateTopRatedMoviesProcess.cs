using DataAccess;
using DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleFinDesFilms
{
    internal static class UpdateTopRatedMoviesProcess
    {
        private const int NUMBER_OF_LINES_TO_CHECK = 6000000;
        private const int NUMBER_OF_ENTRY_TO_UPDATE_IN_ONE_TIME = 200;
        private const int NUMBER_OF_ENTRY_TO_ADD_IN_ONE_TIME = 2000;

        internal static void RunProcess()
        {
            Console.WriteLine("NUMBER_OF_LINES_TO_CHECK:" + NUMBER_OF_LINES_TO_CHECK);
            Console.WriteLine("NUMBER_OF_ENTRY_TO_UPDATE_IN_ONE_TIME:" + NUMBER_OF_ENTRY_TO_UPDATE_IN_ONE_TIME);
            Console.WriteLine("NUMBER_OF_ENTRY_TO_ADD_IN_ONE_TIME:" + NUMBER_OF_ENTRY_TO_ADD_IN_ONE_TIME);
            Console.WriteLine("Started at : " + DateTime.Now.ToString("t"));
            var listIdCompleteExistingMovies = GetCompleteExistingMoviesInDb();
        }

        private static List<string> GetCompleteExistingMoviesInDb()
        {
            using FilmContext context = new FilmContext();
            Console.WriteLine($"Finding complete movies in db...");
            List<string> listIdFullExistingMovies = context.Films.Where(m => !string.IsNullOrWhiteSpace(m.Name)).Select(x => x.Id).ToList();
            Console.WriteLine($"Found {listIdFullExistingMovies.Count} complete movies in db");
            return listIdFullExistingMovies;
        }
    }
}

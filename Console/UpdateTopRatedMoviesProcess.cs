using DataAccess;
using DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleFinDesFilms
{
    public class UpdateTopRatedMoviesProcess : IUpdateTopRatedMoviesProcess
    {
        private const int NUMBER_OF_LINES_TO_CHECK = 2000000;
        private const int NUMBER_OF_ENTRY_TO_UPDATE_IN_ONE_TIME = 200;
        private const int NUMBER_OF_ENTRY_TO_ADD_IN_ONE_TIME = 2000;
        private const int NUMBER_OF_ENTRY_TO_ADD = 30000;
        private const int NUMBER_OF_VOTE_MINI = 2000;
        private readonly FilmContext Context;

        public UpdateTopRatedMoviesProcess(FilmContext context)
        {
            Context = context;
        }

        public void RunProcess()
        {
            Console.WriteLine("NUMBER_OF_LINES_TO_CHECK:" + NUMBER_OF_LINES_TO_CHECK);
            Console.WriteLine("NUMBER_OF_ENTRY_TO_UPDATE_IN_ONE_TIME:" + NUMBER_OF_ENTRY_TO_UPDATE_IN_ONE_TIME);
            Console.WriteLine("NUMBER_OF_ENTRY_TO_ADD_IN_ONE_TIME:" + NUMBER_OF_ENTRY_TO_ADD_IN_ONE_TIME);
            Console.WriteLine("Started at : " + DateTime.Now.ToString("t"));
            Console.WriteLine("BE CAREFUL : this program is reseting the table TopRatedMovies after the first entries added...");
            string pathToRatingFile = @"C:\Users\morga\Downloads\titleratingstsv\data-rating.tsv";
            var listIdCompleteExistingMovies = GetCompleteExistingMoviesInDb();
            var topRatedEnrtiesInFile = GetTopRatedEnrtiesInFile(pathToRatingFile, listIdCompleteExistingMovies);
            DeleteAndInsertTopRatedMoviesInContext(topRatedEnrtiesInFile);
            Console.WriteLine("Finished at : " + DateTime.Now.ToString("t"));
        }

        private List<Film> GetCompleteExistingMoviesInDb()
        {
            Console.WriteLine($"Finding complete movies in db...");
            List<Film> listIdFullExistingMovies = Context.Films.Take(NUMBER_OF_LINES_TO_CHECK).ToList();
            Console.WriteLine($"Found {listIdFullExistingMovies.Count} complete movies in db");
            return listIdFullExistingMovies;
        }

        private List<TopRatedMovie> GetTopRatedEnrtiesInFile(string pathToRatingFile, List<Film> listIdCompleteExistingMovies)
        {
            try
            {
                Console.WriteLine("Starting check in rating file...");
                var topRatedEnrtiesInFile = new List<TopRatedMovie>();
                using (StreamReader sr = File.OpenText(pathToRatingFile))
                {
                    string s = string.Empty;
                    int countAdded = 0;
                    int countChecked = 0;
                    sr.ReadLine();
                    while ((s = sr.ReadLine()) != null && countAdded < NUMBER_OF_LINES_TO_CHECK)
                    {
                        countChecked++;
                        if (countChecked % 100000 == 0)
                        {
                            Console.WriteLine($"{countChecked} lines checked, at :" + DateTime.Now.ToString("t"));
                        }
                        string[] ligne = s.Split("\t");
                        if (ligne.Length != 3)
                        {
                            throw new InvalidDataException("ligne.Length != 3, value : " + s);
                        }
                        var id = ligne[0];
                        var nbvote = int.Parse(ligne[2]);
                        if (nbvote < NUMBER_OF_VOTE_MINI)
                        {
                            continue;
                        }
                        var film = listIdCompleteExistingMovies.Find(m => m.Id == id);
                        if (film == null)
                        {
                            continue;
                        }
                        var ligneFilm = new TopRatedMovie()
                        {
                            Id = id,
                            Name = film.Name,
                            NbVote = nbvote
                        };
                        topRatedEnrtiesInFile.Add(ligneFilm);
                        countAdded++;
                        if (countAdded >= NUMBER_OF_ENTRY_TO_ADD)
                        {
                            break;
                        }
                    }
                    Console.WriteLine("Lines checked in basic file : " + countChecked);
                }
                return topRatedEnrtiesInFile;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception GetType : {ex.GetType()}");
                Console.WriteLine($"Exception Message : {ex.Message}");
                Console.WriteLine($"Exception StackTrace : {ex.StackTrace}");
                throw;
            }
        }

        private void DeleteAndInsertTopRatedMoviesInContext(List<TopRatedMovie> listFilms)
        {
            Console.WriteLine("Saving in database at : " + DateTime.Now.ToString("t"));
            int totalToAdd = 0;
            Context.Database.ExecuteSqlRaw($"TRUNCATE TABLE [TopRatedMovies]");
            foreach (TopRatedMovie filmDTO in listFilms)
            {
                Context.TopRatedMovies.Add(filmDTO);
                totalToAdd++;
                if (totalToAdd % NUMBER_OF_ENTRY_TO_ADD_IN_ONE_TIME == 0)
                {
                    Console.WriteLine($"Adding entries...");
                    Context.SaveChanges();
                    Console.WriteLine($"Added a total of : {totalToAdd}");
                }
            }
            Console.WriteLine($"Adding entries...");
            Context.SaveChanges();
            Console.WriteLine($"Added a total of : {totalToAdd}");
        }
    }
}
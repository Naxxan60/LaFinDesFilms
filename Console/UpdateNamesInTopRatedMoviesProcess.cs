using DataAccess;
using DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleFinDesFilms
{
    public class UpdateNamesInTopRatedMoviesProcess : IUpdateNamesInTopRatedMoviesProcess
    {
        private const int NUMBER_OF_LINES_TO_CHECK = 25000000;
        private const int START_AT_LINE = 21600000;
        private const int NUMBER_OF_ENTRY_TO_UPDATE_IN_ONE_TIME = 400;
        private readonly FilmContext Context;
        private List<Task> savingTasks = new List<Task>();
        private int NumberOfTitlesChecked = 0;
        private int NumberOfLinesChecked = 0;

        public UpdateNamesInTopRatedMoviesProcess(FilmContext context)
        {
            Context = context;
        }

        public void RunProcess()
        {
            Console.WriteLine("NUMBER_OF_LINES_TO_CHECK:" + NUMBER_OF_LINES_TO_CHECK);
            Console.WriteLine("START_AT_LINE:" + START_AT_LINE);
            Console.WriteLine("NUMBER_OF_ENTRY_TO_UPDATE_IN_ONE_TIME:" + NUMBER_OF_ENTRY_TO_UPDATE_IN_ONE_TIME);
            Console.WriteLine("Started at : " + DateTime.Now.ToString("t"));
            var listExistingMovies = GetExistingTopRatedMoviesInDb();
            FillTitlesInImdbTitleFile("C:/Users/morga/Downloads/titleakastsv/data-title.tsv", ref listExistingMovies);
        }

        private List<TopRatedMovie> GetExistingTopRatedMoviesInDb()
        {
            Console.WriteLine($"Finding movies in db...");
            //var Context = _serviceProvider.GetRequiredService<FilmContext>();
            List<TopRatedMovie> listIdFullExistingMovies = Context.TopRatedMovies.ToList();
            Console.WriteLine($"Found {listIdFullExistingMovies.Count} top rated movies");
            return listIdFullExistingMovies;
        }

        private void FillTitlesInImdbTitleFile(string pathTitleAkaImdbFile, ref List<TopRatedMovie> listExistingMovies)
        {
            Console.WriteLine("Starting check in title file at : " + DateTime.Now.ToString("t"));
            if (listExistingMovies.Count == 0)
            {
                Console.WriteLine("listExistingMovies.Count == 0 at : " + DateTime.Now.ToString("t"));
                return;
            }
            var listTitleOfAMovie = new List<LigneTitleAkaImdb>();
            List<TopRatedMovie> listMoviesToUpdate = new List<TopRatedMovie>();
            string previousId = string.Empty;
            string currentId = string.Empty;
            string line = string.Empty;
            string chosenTitle = string.Empty;
            int totalToUpdate = 0;
            using (StreamReader sr = File.OpenText(pathTitleAkaImdbFile))
            {
                for (var i = 1; i < START_AT_LINE; i++)
                {
                    sr.ReadLine();
                }
                Console.WriteLine($"{START_AT_LINE} skipped lines, at :" + DateTime.Now.ToString("t"));
                while ((line = sr.ReadLine()) != null)
                {
                    NumberOfLinesChecked++;
                    if (NumberOfLinesChecked % 100000 == 0)
                    {
                        Console.WriteLine($"{NumberOfLinesChecked} lines checked, at :" + DateTime.Now.ToString("t"));
                    }
                    string[] ligne = line.Split("\t");
                    if (ligne.Length != 8)
                    {
                        throw new InvalidDataException("ligne.Length != 8, value : " + line);
                    }
                    if (!listExistingMovies.Any(m => m.Id == ligne[0]))
                    {
                        continue;
                    }
                    currentId = ligne[0];
                    if (!string.IsNullOrEmpty(previousId) && currentId != previousId)
                    {
                        chosenTitle = FindTheFrenchTitle(listTitleOfAMovie);
                        var film = listExistingMovies.Find(m => m.Id == previousId);
                        film.Name = chosenTitle;
                        listMoviesToUpdate.Add(film);
                        if (CheckAndSaveInDatabaseEachXMovies(listMoviesToUpdate, ref totalToUpdate))
                        {
                            listMoviesToUpdate = new List<TopRatedMovie>();
                        }
                        listTitleOfAMovie.Clear();
                        NumberOfTitlesChecked++;
                    }
                    previousId = currentId;
                    var ligneFilm = new LigneTitleAkaImdb()
                    {
                        ImdbId = ligne[0],
                        Ordering = ligne[1],
                        Title = ligne[2],
                        Region = ligne[3],
                        Language = ligne[4],
                        Type = ligne[5]
                    };
                    listTitleOfAMovie.Add(ligneFilm);
                }
                if (listTitleOfAMovie.Count > 0)
                {
                    // One last time
                    chosenTitle = FindTheFrenchTitle(listTitleOfAMovie);
                    var film = listExistingMovies.Find(m => m.Id == previousId);
                    film.Name = chosenTitle;
                    listMoviesToUpdate.Add(film);
                    savingTasks.Add(SaveInDatabaseMoviesSync(listMoviesToUpdate, totalToUpdate));
                    Task.WaitAll(savingTasks.ToArray());
                    NumberOfTitlesChecked++;
                }
            }
            Console.WriteLine("Checked titles : " + NumberOfTitlesChecked);
            Console.WriteLine("Checked lines : " + NumberOfLinesChecked);
        }

        private bool CheckAndSaveInDatabaseEachXMovies(List<TopRatedMovie> listMoviesToUpdate, ref int totalToUpdate)
        {
            if (listMoviesToUpdate.Count % NUMBER_OF_ENTRY_TO_UPDATE_IN_ONE_TIME == 0)
            {
                if (savingTasks.Count > 0)
                {
                    Task.WaitAll(savingTasks.ToArray());
                    savingTasks.Clear(); 
                }
                totalToUpdate += listMoviesToUpdate.Count;
                savingTasks.Add(SaveInDatabaseMoviesSync(listMoviesToUpdate, totalToUpdate));
                return true;
            }
            return false;
        }

        private async Task SaveInDatabaseMoviesSync(List<TopRatedMovie> listMoviesToUpdate, int totalToUpdate)
        {
            Console.WriteLine("Saving in database... at : " + DateTime.Now.ToString("t"));
            foreach (TopRatedMovie trFilmDTO in listMoviesToUpdate)
            {
                Context.TopRatedMovies.Update(trFilmDTO);
                await Context.SaveChangesAsync();
            }
            Console.WriteLine($"Updated a total of : {totalToUpdate}");
            Console.WriteLine("Checked titles : " + NumberOfTitlesChecked);
            Console.WriteLine("Checked lines : " + NumberOfLinesChecked);
        }

        private string FindTheFrenchTitle(List<LigneTitleAkaImdb> listTitleOfAMovie)
        {
            if (listTitleOfAMovie.Count == 0)
            {
                throw new InvalidDataException("Aucun titre trouvé function : FindTheTitle");
            }
            listTitleOfAMovie = listTitleOfAMovie.OrderBy(x => x.Ordering).ToList();
            var chosenTitle = listTitleOfAMovie.Find(x => x.Region == "FR" && !x.Type.StartsWith("alternative"));
            if (chosenTitle == null)
            {
                chosenTitle = listTitleOfAMovie.Find(x => x.Region == "FR");
                if (chosenTitle == null)
                {
                    chosenTitle = listTitleOfAMovie.Find(x => x.Language == "fr");
                    if (chosenTitle == null)
                    {
                        chosenTitle = listTitleOfAMovie.Find(x => x.Type == "original");
                        if (chosenTitle == null)
                        {
                            chosenTitle = listTitleOfAMovie.First();
                        }
                    }
                }
            }
            return chosenTitle.Title;
        }
    }
}

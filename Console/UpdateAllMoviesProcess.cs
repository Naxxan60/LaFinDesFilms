using DataAccess;
using DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleFinDesFilms
{
    public class UpdateAllMoviesProcess : IUpdateAllMoviesProcess
    {
        private const int NUMBER_OF_LINES_TO_CHECK = 6000000;
        private const int NUMBER_OF_ENTRY_TO_UPDATE_IN_ONE_TIME = 200;
        private const int NUMBER_OF_ENTRY_TO_ADD_IN_ONE_TIME = 2000;
        private readonly FilmContext Context;

        public UpdateAllMoviesProcess(FilmContext context)
        {
            Context = context;
        }

        public void RunProcess()
        {
            Console.WriteLine("NUMBER_OF_LINES_TO_CHECK:" + NUMBER_OF_LINES_TO_CHECK);
            Console.WriteLine("NUMBER_OF_ENTRY_TO_UPDATE_IN_ONE_TIME:" + NUMBER_OF_ENTRY_TO_UPDATE_IN_ONE_TIME);
            Console.WriteLine("NUMBER_OF_ENTRY_TO_ADD_IN_ONE_TIME:" + NUMBER_OF_ENTRY_TO_ADD_IN_ONE_TIME);
            Console.WriteLine("Started at : " + DateTime.Now.ToString("t"));
            var listIdCompleteExistingMovies = GetCompleteExistingMoviesInDb();
            var listIdUncompleteExistingMovies = GetUncompleteExistingMoviesInDb();
            var listOfMoviesUnexistingOrIncompleteInDb = FindUnexistingMoviesInBasicFile("C:/Users/morga/Downloads/titlebasicstsv/data-basic.tsv", listIdCompleteExistingMovies);
            FillTitlesInImdbTitleFile("C:/Users/morga/Downloads/titleakastsv/data-title.tsv", ref listOfMoviesUnexistingOrIncompleteInDb);
            CheckAndAddOrUpdateMoviesInContext(listOfMoviesUnexistingOrIncompleteInDb, listIdUncompleteExistingMovies);
        }

        private List<string> GetCompleteExistingMoviesInDb()
        {
            Console.WriteLine($"Finding complete movies in db...");
            List<string> listIdFullExistingMovies = Context.Films.Where(m => !string.IsNullOrWhiteSpace(m.Name)).Select(x => x.Id).ToList();
            Console.WriteLine($"Found {listIdFullExistingMovies.Count} complete movies in db");
            return listIdFullExistingMovies;
        }

        private List<string> GetUncompleteExistingMoviesInDb()
        {
            Console.WriteLine($"Finding uncomplete movies in db...");
            var listIdsExistingInDb = Context.Films.Where(m => string.IsNullOrWhiteSpace(m.Name)).Select(m => m.Id).ToList();
            Console.WriteLine($"Found {listIdsExistingInDb.Count} uncomplete movies in db");
            return listIdsExistingInDb;
        }

        private List<Film> FindUnexistingMoviesInBasicFile(string pathListFilms, List<string> listIdFullExistingMovies)
        {
            Console.WriteLine("Starting check in basic file at : " + DateTime.Now.ToString("t"));
            var listOfMoviesUnexistingInDb = new List<Film>();
            using (StreamReader sr = File.OpenText(pathListFilms))
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
                    if (ligne.Length != 9)
                    {
                        throw new InvalidDataException("ligne.Length != 9, value : " + s);
                    }
                    if (ligne[1] != "movie" || listIdFullExistingMovies.Any(m => m == ligne[0]))
                    {
                        continue;
                    }
                    var ligneFilm = new Film()
                    {
                        Id = ligne[0],
                        Name = ligne[3], // some movies are not in the aka title file
                    };
                    listOfMoviesUnexistingInDb.Add(ligneFilm);
                    countAdded++;
                }
                Console.WriteLine("Lines checked in basic file : " + countChecked);
            }
            return listOfMoviesUnexistingInDb;
        }

        private static void FillTitlesInImdbTitleFile(string pathTitleAkaImdbFile, ref List<Film> listOfMoviesUnexistingOrIncompleteInDb)
        {
            Console.WriteLine($"Number of unexisting or uncomplete movies : {listOfMoviesUnexistingOrIncompleteInDb.Count}");
            Console.WriteLine("Starting check in title file at : " + DateTime.Now.ToString("t"));
            if (listOfMoviesUnexistingOrIncompleteInDb.Count == 0)
            {
                return;
            }
            var listTitleOfAMovie = new List<LigneTitleAkaImdb>();
            string previousId = string.Empty;
            string currentId = string.Empty;
            string line = string.Empty;
            string chosenTitle = string.Empty;
            int numberOfTitlesChecked = 0;
            int numberOfLinesChecked = 0;
            using (StreamReader sr = File.OpenText(pathTitleAkaImdbFile))
            {
                sr.ReadLine(); // column names
                while ((line = sr.ReadLine()) != null)
                {
                    numberOfLinesChecked++;
                    if (numberOfLinesChecked % 100000 == 0)
                    {
                        Console.WriteLine($"{numberOfLinesChecked} lines checked, at :" + DateTime.Now.ToString("t"));
                    }
                    string[] ligne = line.Split("\t");
                    if (ligne.Length != 8)
                    {
                        throw new InvalidDataException("ligne.Length != 8, value : " + line);
                    }
                    if (!listOfMoviesUnexistingOrIncompleteInDb.Any(m => m.Id == ligne[0]))
                    {
                        continue;
                    }
                    currentId = ligne[0];
                    if (!string.IsNullOrEmpty(previousId) && currentId != previousId)
                    {
                        chosenTitle = FindTheFrenchTitle(listTitleOfAMovie);
                        listOfMoviesUnexistingOrIncompleteInDb.Find(m => m.Id == previousId).Name = chosenTitle;
                        listTitleOfAMovie.Clear();
                        numberOfTitlesChecked++;
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
                    listOfMoviesUnexistingOrIncompleteInDb.Find(m => m.Id == previousId).Name = chosenTitle;
                }
            }
            Console.WriteLine("Checked titles : " + numberOfTitlesChecked);
            Console.WriteLine("Checked lines : " + numberOfLinesChecked);
        }

        private static string FindTheFrenchTitle(List<LigneTitleAkaImdb> listTitleOfAMovie)
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

        private void CheckAndAddOrUpdateMoviesInContext(List<Film> listFilms, List<string> listIdUncompleteExistingMovies)
        {
            Console.WriteLine("Saving in database at : " + DateTime.Now.ToString("t"));
            int totalToAdd = 0;
            int totalToUpdate = 0;
            foreach (Film filmDTO in listFilms)
            {
                if (!listIdUncompleteExistingMovies.Any(id => id == filmDTO.Id))
                {
                    Context.Films.Add(filmDTO);
                    totalToAdd++;
                    if (totalToAdd % NUMBER_OF_ENTRY_TO_ADD_IN_ONE_TIME == 0)
                    {
                        Console.WriteLine($"Adding entries...");
                        Context.SaveChanges();
                        Console.WriteLine($"Added a total of : {totalToAdd}");
                    }
                }
                else
                {
                    Context.Films.Update(filmDTO);
                    totalToUpdate++;
                    if (totalToUpdate % NUMBER_OF_ENTRY_TO_UPDATE_IN_ONE_TIME == 0)
                    {
                        Console.WriteLine($"Updating entries...");
                        Context.SaveChanges();
                        Console.WriteLine($"Updated a total of : {totalToUpdate}");
                    }
                }
            }
            Console.WriteLine($"Adding/Updating entries...");
            Context.SaveChanges();
            Console.WriteLine($"Added a total of : {totalToAdd}");
            Console.WriteLine($"Updated a total of : {totalToUpdate}");
        }
    }
}

using ConsoLovers.ConsoleToolkit.Contracts;
using ConsoLovers.ConsoleToolkit.Menu;
using System;

namespace ConsoleFinDesFilms
{
    class ConsoleApplication
    {
        private readonly IUpdateTopRatedMoviesProcess _updateTopRatedMoviesProcess;
        private readonly IUpdateNamesInTopRatedMoviesProcess _updateNamesInTopRatedMoviesProcess;
        private readonly IUpdateAllMoviesProcess _updateAllMoviesProcess;
        public ConsoleApplication(IUpdateTopRatedMoviesProcess topRservice, IUpdateNamesInTopRatedMoviesProcess topNameRservice, IUpdateAllMoviesProcess allMovieService)
        {
            _updateTopRatedMoviesProcess = topRservice;
            _updateNamesInTopRatedMoviesProcess = topNameRservice;
            _updateAllMoviesProcess = allMovieService;
        }

        // Application starting point
        public void Run()
        {
            Console.Title = "La Fin Des Films - Console";
            Console.CursorSize = 4;
            Console.WindowHeight = 40;
            Console.WindowWidth = 120;
            string header = @"
▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄▄
██ ████ ▄▄▀█████ ▄▄▄█▄██ ▄▄▀█████ ▄▄▀█ ▄▄█ ▄▄█████ ▄▄▄█▄██ ██ ▄▀▄ █ ▄▄██████ ▄▄▀█▀▄▄▀█ ▄▄▀█ ▄▄█▀▄▄▀█ ██ ▄▄██
██ ████ ▀▀ █████ ▄▄██ ▄█ ██ █████ ██ █ ▄▄█▄▄▀█████ ▄▄██ ▄█ ██ █▄█ █▄▄▀██▄▄██ ████ ██ █ ██ █▄▄▀█ ██ █ ██ ▄▄██
██ ▀▀ █▄██▄█████ ███▄▄▄█▄██▄█████ ▀▀ █▄▄▄█▄▄▄█████ ███▄▄▄█▄▄█▄███▄█▄▄▄██████ ▀▀▄██▄▄██▄██▄█▄▄▄██▄▄██▄▄█▄▄▄██
▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀";
            string footer = Environment.NewLine + "";

            ColoredConsoleMenu menu = new ColoredConsoleMenu { Header = header, Footer = footer, CircularSelection = false, Selector = "» " };
            menu.SelectionStrech = SelectionStrech.UnifiedLength;
            menu.IndexMenuItems = false;
            menu.Add(new ConsoleMenuItem("Update all movies", UpdateAllMoviesRunProcess));
            menu.Add(new ConsoleMenuItem("Update top rated movies", UpdateTopRatedMoviesRunProcess));
            menu.Add(new ConsoleMenuItem("Update names in top rated movies", UpdateNamesInTopRatedMoviesRunProcess));
            menu.Add(new ConsoleMenuItem("Close menu", x => menu.Close()));
            menu.Show();
        }

        private void UpdateAllMoviesRunProcess(ConsoleMenuItem sender)
        {
            _updateAllMoviesProcess.RunProcess();
            Console.WriteLine("Done. Press any key to return to the menu...");
            Console.ReadLine();
        }

        private void UpdateTopRatedMoviesRunProcess(ConsoleMenuItem sender)
        {
            _updateTopRatedMoviesProcess.RunProcess();
            Console.WriteLine("Done. Press any key to return to the menu...");
            Console.ReadLine();
        }

        private void UpdateNamesInTopRatedMoviesRunProcess(ConsoleMenuItem sender)
        {
            _updateNamesInTopRatedMoviesProcess.RunProcess();
            Console.WriteLine("Done. Press any key to return to the menu...");
            Console.ReadLine();
        }
    }
}

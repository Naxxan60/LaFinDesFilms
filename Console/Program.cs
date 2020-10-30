using System;
using ConsoLovers.ConsoleToolkit.Contracts;
using ConsoLovers.ConsoleToolkit.Menu;

namespace ConsoleFinDesFilms
{
    class Program
    {
        static void Main()
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
            menu.Add(new ConsoleMenuItem("Close menu", x => menu.Close()));
            menu.Show();
        }

        private static void UpdateAllMoviesRunProcess(ConsoleMenuItem sender)
        {
            UpdateAllMoviesProcess.RunProcess();
            Console.WriteLine("Done. Press any key to return to the menu...");
            Console.ReadLine();
        }

        private static void UpdateTopRatedMoviesRunProcess(ConsoleMenuItem sender)
        {
            UpdateTopRatedMoviesProcess.RunProcess();
            Console.WriteLine("Done. Press any key to return to the menu...");
            Console.ReadLine();
        }
    }
}

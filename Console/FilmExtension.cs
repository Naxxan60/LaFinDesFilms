using DTO;

namespace ConsoleFinDesFilms
{
    internal static class FilmExtension
    {
        public static Film ToFilmDTO(this LigneTitleBasicImdb ligne)
        {
            return new Film()
            {
                Id = ligne.ImdbId
            };
        }
    }
}

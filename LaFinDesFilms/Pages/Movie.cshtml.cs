using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DTO;
using LaFinDesFilms.omdbapi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace LaFinDesFilms.Pages
{
    public class MovieModel : PageModel
    {
        private readonly ITopRatedMoviesService _topRatedMoviesService;
        private readonly IHttpClientFactory _clientFactory;
        public string Id { get; set; }

        public TopRatedMovie TopRatedMovie { get; set; }
        public OmdbApiResult OmdbMovieInfo { get; private set; }

        public MovieModel(IHttpClientFactory clientFactory, ITopRatedMoviesService topRatedMoviesService)
        {
            _topRatedMoviesService = topRatedMoviesService;
            _clientFactory = clientFactory;
        }

        public async Task OnGetAsync(string id)
        {
            Id = id;
            TopRatedMovie = _topRatedMoviesService.GetTopRatedMovieById(id);

            string requestedUrl = $"http://www.omdbapi.com/?i={Id}&apikey=67111ede";
            var request = new HttpRequestMessage(HttpMethod.Get, requestedUrl);
            request.Headers.Add("Accept", "application/json");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                OmdbMovieInfo = await JsonSerializer.DeserializeAsync<OmdbApiResult>(responseStream);
            }
            else
            {
                throw new Exception("Impossible de récupérer les info de l'API omdbapi");
            }
        }
    }
}

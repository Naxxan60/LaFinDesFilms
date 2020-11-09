using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class OmdbApiService : IOmdbApiService
    {
        private readonly IHttpClientFactory _clientFactory;
        public OmdbApiService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<OmdbApiResult> GetOmdbInfoAsync(string Id)
        {
            string requestedUrl = $"http://www.omdbapi.com/?i={Id}&apikey=67111ede";
            var request = new HttpRequestMessage(HttpMethod.Get, requestedUrl);
            request.Headers.Add("Accept", "application/json");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<OmdbApiResult>(responseStream);
            }
            else
            {
                throw new Exception("Impossible de récupérer les info de l'API omdbapi");
            }
        }
    }
}

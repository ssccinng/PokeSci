namespace PokemonIsshoni.Net.Client.Services
{
    public class PCLServices
    {
        private readonly HttpClient _httpClient;
        public PCLServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}

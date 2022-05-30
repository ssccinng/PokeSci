using PokemonIsshoni.Net.Shared.Info;
using System.Net.Http.Json;

namespace PokemonIsshoni.Net.Client.Services
{
    public class UserInfoServices
    {
        private readonly HttpClient _httpClient;
        public UserInfoServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserInfo> GetUserIdByEmail(string email)
        {
            return await _httpClient.GetFromJsonAsync<UserInfo>($"/api/userinfo/GetUserByEmail/{email}");
        }

        public async Task<UserInfo> GetUserIdByNameAsync(string name)
        {
            return await _httpClient.GetFromJsonAsync<UserInfo>($"/api/userinfo/GetUserByName/{name}");
        }
    }
}

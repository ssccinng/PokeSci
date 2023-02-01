using System.Net.Http.Json;
using PokemonIsshoni.Net.Shared.Info;

namespace PokemonIsshoni.Net.Client.Services
{
    public class UserInfoServices
    {
        private readonly HttpClient _httpClient;
        public UserInfoServices(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<UserInfo>> GetAllUserAsync()
        {

            return await _httpClient.GetFromJsonAsync<List<UserInfo>>($"/api/userinfo/GetAllUser");
        }
        public async Task<UserInfo> GetUserByEmailAsync(string email)
        {
            return await _httpClient.GetFromJsonAsync<UserInfo>($"/api/userinfo/GetUserByEmail/{email}");
        }

        public async Task<UserInfo> GetUserByNameAsync(string name)
        {
            return await _httpClient.GetFromJsonAsync<UserInfo>($"/api/userinfo/GetUserByName/{name}");
        }
        public async Task<UserInfo> GetUserByIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<UserInfo>($"/api/userinfo/GetUserById/{id}");
        }

        public async Task AddGustAsync(int cnt)
        {
            await _httpClient.GetAsync($"/api/userinfo/AddGuset/{cnt}");
        }
    }
}

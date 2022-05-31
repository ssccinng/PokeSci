using PokemonIsshoni.Net.Shared.Models;
using System.Net.Http.Json;

namespace PokemonIsshoni.Net.Client.Services
{
    public class PCLServices
    {
        private HttpClient _httpClient => IsAnonymous ? _httpClientAnonymous : _httpClientAu;
        private readonly HttpClient _httpClientAu;
        private readonly IHttpClientFactory _factory;
        private readonly HttpClient _httpClientAnonymous;

        //public bool IsAnonymous = false;  
        public static bool IsAnonymous = false;  
        public PCLServices(HttpClient httpClient, IHttpClientFactory factory)
        {
            _httpClientAu = httpClient;
            _factory = factory;
            _httpClientAnonymous = _factory.CreateClient("PokemonIsshoni.Net.ServerAPI.Anonymous");

        }
        /// <summary>
        /// 创建比赛 返回id
        /// </summary>
        /// <param name="pCLMatch"></param>
        /// <returns></returns>
        public async Task<PCLMatch> CreateMatch(PCLMatch pCLMatch)
        {
            var res = await _httpClient.PostAsJsonAsync("api/PCLMatches", pCLMatch);
            if (!res.IsSuccessStatusCode)
            {
                return null;
            }
            //var jsonn = res.Content.ToString();
            //var jsonn1 = await res.Content.ReadAsStringAsync();

            //var json2 = await res..ReadFromJsonAsync<PCLMatch>();
            //var json1 = res.ToString();
            return await res.Content.ReadFromJsonAsync<PCLMatch>();
        }
        public async Task<PCLMatch> GetMatchByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<PCLMatch>($"api/PCLMatches/{id}");
        }

        public async Task<IEnumerable<PCLMatch>> GetMatchesAsync()
        {
            // 限制一下数目(? 有必要的话
            return await _httpClient.GetFromJsonAsync<IEnumerable<PCLMatch>>("api/PCLMatches");
        }
        /// <summary>
        /// 报名
        /// </summary>
        /// <param name="pCLMatchPlayer"></param>
        /// <returns></returns>
        public async Task<PCLMatchPlayer> RegisterPCLMatch(PCLMatchPlayer pCLMatchPlayer, string pwd = "123")
        {
            // 限制一下数目(? 有必要的话
            var res = await _httpClient.PostAsJsonAsync($"api/PCLMatches/RegisterUser/{pwd}", pCLMatchPlayer);
            if (!res.IsSuccessStatusCode) return null;
            return await res.Content.ReadFromJsonAsync<PCLMatchPlayer>();
            //return null;
        }

        public async Task<bool> UpdatePCLMatchAsync(PCLMatch pCLMatch)
        {
            // 限制一下数目(? 有必要的话
            var res = await _httpClient.PutAsJsonAsync($"api/PCLMatches/{pCLMatch.Id}", pCLMatch);

            return res.IsSuccessStatusCode;
            //return null;
        }

    }
}

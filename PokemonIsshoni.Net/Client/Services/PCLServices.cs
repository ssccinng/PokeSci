using PokeCommon.Models;
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
        public async Task<PCLMatchPlayer> AddPCLMatch(PCLMatchPlayer pCLMatchPlayer)
        {
            // 限制一下数目(? 有必要的话
            var res = await _httpClient.PostAsJsonAsync($"api/PCLMatches/AddUser", pCLMatchPlayer);
            if (!res.IsSuccessStatusCode) return null;
            return await res.Content.ReadFromJsonAsync<PCLMatchPlayer>();
            //return null;
        }
        /// <summary>
        /// 取消报名
        /// </summary>
        /// <param name="pCLMatchPlayer"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public async Task<bool> DeRegisterPCLMatch(PCLMatchPlayer pCLMatchPlayer)
        {
            // 限制一下数目(? 有必要的话
            // 直接删？还是只是取消 可以再想想
            var res = await _httpClient.DeleteAsync($"api/PCLMatchPlayers/{pCLMatchPlayer.Id}");
            return res.IsSuccessStatusCode;
            //return null;
        }
        public async Task<PCLRoundPlayer> AddPCLRound(PCLRoundPlayer pCLRoundPlayer)
        {
            // 限制一下数目(? 有必要的话
            var res = await _httpClient.PostAsJsonAsync($"api/PCLRoundPlayers", pCLRoundPlayer);
            if (!res.IsSuccessStatusCode) return null;
            return await res.Content.ReadFromJsonAsync<PCLRoundPlayer>();
            //return null;
        }
        public async Task<bool> DeRegisterPCLRound(PCLRoundPlayer pCLRoundPlayer)
        {
            // 限制一下数目(? 有必要的话
            // 直接删？还是只是取消 可以再想想
            var res = await _httpClient.DeleteAsync($"api/PCLRoundPlayers/{pCLRoundPlayer.Id}");
            return res.IsSuccessStatusCode;
            //return null;
        }
        public async Task<bool> UpdatePCLMatchAsync(PCLMatch pCLMatch)
        {
            // 限制一下数目(? 有必要的话
            var res = await _httpClient.PutAsJsonAsync($"api/PCLMatches/{pCLMatch.Id}", pCLMatch);

            return res.IsSuccessStatusCode;
            //return null;
        }
        #region 裁判
        public async Task<PCLReferee> RegisterRefereePCLMatchAsync(PCLReferee referee)
        {
            // 限制一下数目(? 有必要的话
            var res = await _httpClient.PostAsJsonAsync($"api/Referees", referee);
            if (!res.IsSuccessStatusCode)
            {
                return null;
            }
            else
            {
                return await res.Content.ReadFromJsonAsync<PCLReferee>();
            }

            //return null;
        }

        public async Task<bool> DeRegisterRefereePCLMatchAsync(PCLReferee referee)
        {
            // 限制一下数目(? 有必要的话
            var res = await _httpClient.DeleteAsync($"api/Referees/{referee.Id}");
            return res.IsSuccessStatusCode;

            //return null;
        }
        #endregion
        #region PokemonHome
        public async Task<List<PokemonHomeTrainerRankData>> GetTrainerRankDataAsync(PokemonHomeSession pokemonHomeSession = null)
        {
            // 限制一下数目(? 有必要的话

            if (pokemonHomeSession == null)
            {
                var res = await _httpClientAnonymous.GetAsync($"api/GetTrainerRankDataLast");
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadFromJsonAsync<List<PokemonHomeTrainerRankData>>();
                }
            }
            else
            {
                var res = await _httpClientAnonymous.PostAsJsonAsync($"api/GetTrainerRankData", pokemonHomeSession);
                if (res.IsSuccessStatusCode)
                {
                    return await res.Content.ReadFromJsonAsync<List<PokemonHomeTrainerRankData>>();
                }
            }
            return null;
            //return null;
        }
        public async Task<List<PokemonHomeSession>> GetPokemonHomeSessionsAsync()
        {
            // 限制一下数目(? 有必要的话

            var res = await _httpClientAnonymous.GetAsync($"api/GetPokemonHomeSessions");
            if (res.IsSuccessStatusCode)
            {
                return await res.Content.ReadFromJsonAsync<List<PokemonHomeSession>>();
            }
            return null;
            //return null;
        }
        #endregion

        #region 比赛流程控制
        public async Task<bool> PCLMatcStartAsync(PCLMatch pCLMatch)
        {
            // 限制一下数目(? 有必要的话
            return await PCLMatcStartAsync(pCLMatch.Id);
            //return null;
        }

        public async Task<bool> PCLMatcStartAsync(int id)
        {
            // 限制一下数目(? 有必要的话
            var res = await _httpClient.PostAsync($"api/PCLMatches/MatchStart/{id}", null );

            return res.IsSuccessStatusCode;
            //return null;
        }

        public async Task<bool> PCLRoundStartAsync(int matchId, int roundId)
        {
            var res = await _httpClient.PostAsync($"api/PCLMatches/RoundStart/{matchId}/{roundId}", null);

            return res.IsSuccessStatusCode;
        }
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using System.Text.Json;
using PokeCommon.Models;
using System.IO.Compression;
using PokeCommon.BattleEngine;

namespace PokeCommon.PokemonHome
{
    public class PokemonHomeTools
    {
        // 最新数据
        public List<PokemonHomeSession> PokemonHomeSessions = new();
        public List<PokemonHomeTrainerRankData> PokemonHomeTrainerRankDatas = new();
        public List<PokemonHomeTrainerRankData> PokemonHomeLastTrainerRankDatas = new();

        private Timer _timer;

        private string _bundleUrl = "https://resource.pokemon-home.com/battledata/js/bundle.js";
        private string _rankmatchApiUrl = "https://api.battle.pokemon-home.com/cbd/competition/rankmatch/list";
        private string _pDataUrl = "https://resource.pokemon-home.com/battledata/ranking/{0}/{1}/{2}/pdetail-{3}";
        private string _trainerUrl = "https://resource.pokemon-home.com/battledata/ranking/{0}/{1}/{2}/traner-{3}";

        private string _header = @"accept: application/json, text/javascript, */*; q=0.01
countrycode: 304
authorization: Bearer
langcode: 1
user-agent: Mozilla/5.0 (Linux; Android 8.0; Pixel 2 Build/OPD3.170816.012) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Mobile Safari/537.36
Accept-Encoding: gzip";
//Accept-Encoding: gzip";
//Accept-Encoding: gzip,deflate";

        private HttpClient _httpClient = new();
        public PokemonHomeTools(bool autoUpdate = false)
        {
            foreach (var item in _header.Split("\r\n"))
            {
                var data = item.Split(": ");
                _httpClient.DefaultRequestHeaders.Add(data[0], data[1]);
            }
            if (autoUpdate)
            {
                UpdateLastRankMatchAsync().Wait();
                _timer = new Timer(new TimerCallback(async _ =>
                {
                    await UpdateRankMatchAsync();
                }), null, 5000, 10 * 60);
            }
            
        }

        public void UpdateBundle()
        {
            // 访问bundle并更新译名数据
        }

        public async Task<List<PokemonHomeSession>> GetRankMatchAsync()
        {
            var response = await _httpClient.PostAsJsonAsync(_rankmatchApiUrl, new { soft = "Sw" });
            List<PokemonHomeSession> resp = new();
            var res = await response.Content.ReadAsStringAsync();
            var data = JsonDocument.Parse(res).RootElement.GetProperty("list");
            foreach (var item in data.EnumerateObject())
            {
                //int idx = 0;
                foreach (var item1 in item.Value.EnumerateObject())
                {
                    var a = JsonSerializer.Deserialize<PokemonHomeSession>(item1.Value);
                    a.SeasonId = int.Parse(item1.Name);
                    //if (idx == 0)
                    //{
                    //    a.Type = BattleEngine.BattleType.Double;
                    //}
                    //else
                    //{
                    //    a.Type = BattleEngine.BattleType.Single;

                    //}
                    //idx++;
                    resp.Add(a);
                }
            }
            return resp;
        }
        // 双打
        public async Task UpdateLastRankMatchAsync(BattleType battleType = BattleType.Double)
        {
            PokemonHomeSessions = await GetRankMatchAsync();
            PokemonHomeLastTrainerRankDatas = await GetTrainerDataAsync(PokemonHomeSessions.Where(s => s.Type == battleType).ElementAt(1), -1);
        }
        public async Task UpdateRankMatchAsync(BattleType battleType = BattleType.Double, bool all = false)
        {
            try
            {
                PokemonHomeSessions = await GetRankMatchAsync();
                PokemonHomeTrainerRankDatas = await GetTrainerDataAsync(PokemonHomeSessions.First(s => s.Type == battleType), all == true ? -1 : 1);

            }
            catch (Exception e)
            {

                Console.WriteLine("dani");
                Console.WriteLine(e.Message);
            }

        }

        public async Task<List<PokemonHomeTrainerRankData>> GetTrainerDataAsync(int sessionId, int rst, int ts1, int page = 1)
        {
            List<PokemonHomeTrainerRankData> res = new();
            //System.Console.WriteLine(string.Format(url3, stId, rts, ts2, i));


            if (page == -1)
            {
                page = 1;
                while (true)
                {
                    var response = await _httpClient.GetAsync(string.Format(_trainerUrl, sessionId, rst, ts1, page));
                    if (!response.IsSuccessStatusCode) break;
                    MemoryStream output = new MemoryStream();
                    using var decompressor = new GZipStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress);
                    decompressor.CopyTo(output);
                    var gfata = Encoding.UTF8.GetString(output.ToArray());
                    //System.Console.WriteLine(gfata);
                    //var gd = JsonDocument.Parse(output).RootElement;
                    var gd = JsonDocument.Parse(gfata).RootElement;
                    for (int j = 0; j < gd.GetArrayLength(); j++)
                    {
                        res.Add(JsonSerializer.Deserialize<PokemonHomeTrainerRankData>(gd[j]));
                        //if (gd[j].GetProperty("lng").GetString() == "7")
                        //{
                        //    System.Console.WriteLine(gd[j]);
                        //}
                    }
                    page++;
                }
            }
            else
            {

            
                var response = await _httpClient.GetAsync(string.Format(_trainerUrl, sessionId, rst, ts1, page));
                MemoryStream output = new MemoryStream();
                using var decompressor = new GZipStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress);
                decompressor.CopyTo(output);
                var gfata = Encoding.UTF8.GetString(output.ToArray());
                //System.Console.WriteLine(gfata);
                //var gd = JsonDocument.Parse(output).RootElement;
                var gd = JsonDocument.Parse(gfata).RootElement;
                for (int j = 0; j < gd.GetArrayLength(); j++)
                {
                    res.Add(JsonSerializer.Deserialize<PokemonHomeTrainerRankData>(gd[j]));
                    //if (gd[j].GetProperty("lng").GetString() == "7")
                    //{
                    //    System.Console.WriteLine(gd[j]);
                    //}
                }
            }
            return res;
        }
        public async Task<List<PokemonHomeTrainerRankData>> GetTrainerDataAsync(PokemonHomeSession pokemonHomeSession, int page = 1)
        {
            return await GetTrainerDataAsync(pokemonHomeSession.SeasonId, pokemonHomeSession.RST, pokemonHomeSession.TS1, page);
        }

        public async Task<byte[]> GetIconAsync(string name)
        {
            // 如果失败 返回初代赤 或者光

            return await (await _httpClient.GetAsync($"https://resource.pokemon-home.com/battledata/img/icons/trainer/{name}")).Content.ReadAsByteArrayAsync();
        }

    }
}

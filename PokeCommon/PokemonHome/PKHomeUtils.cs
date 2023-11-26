using PokeCommon.Models;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokeCommon.PokemonHome
{
    public class PKHomeUtils
    {
        public readonly string BundleUrl = "https://resource.pokemon-home.com/battledata/js/bundle.js";
        public readonly string RankmatchApiUrl = "https://api.battle.pokemon-home.com/cbd/competition/rankmatch/list";
        public readonly string RankmatchApiSVUrl = "https://api.battle.pokemon-home.com/tt/cbd/competition/rankmatch/list";
        public readonly string PDataUrl = "https://resource.pokemon-home.com/battledata/ranking/{0}/{1}/{2}/pdetail-{3}";
        //private string _trainerUrl = "https://resource.pokemon-home.com/battledata/ranking/{0}/{1}/{2}/traner-{3}";
        public readonly string TrainerUrl = "https://resource.pokemon-home.com/battledata/ranking/scvi/{0}/{1}/{2}/traner-{3}";

        private readonly string _header = @"accept: application/json, text/javascript, */*; q=0.01
countrycode: 304
authorization: Bearer
langcode: 1
user-agent: Mozilla/5.0 (Linux; Android 8.0; Pixel 2 Build/OPD3.170816.012) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Mobile Safari/537.36
Accept-Encoding: gzip";
        //Accept-Encoding: gzip";
        //Accept-Encoding: gzip,deflate";

        private HttpClient _httpClient = new();

        public PKHomeUtils()
        {
            foreach (var item in _header.Split("\r\n"))
            {
                var data = item.Split(": ");
                _httpClient.DefaultRequestHeaders.Add(data[0], data[1]);
            }
        }

        public async Task<List<SVPokemonHomeSession>> GetSVPokemonHomeSessionsAsync()
        {
            var response = await _httpClient.PostAsJsonAsync(RankmatchApiSVUrl, new { soft = "Sw" });
            List<SVPokemonHomeSession> resp = new();
            var res = await response.Content.ReadAsStringAsync();
            var data = JsonDocument.Parse(res).RootElement.GetProperty("list");
            foreach (var item in data.EnumerateObject())
            {
                //int idx = 0;
                foreach (var item1 in item.Value.EnumerateObject())
                {
                    var a = JsonSerializer.Deserialize<SVPokemonHomeSession>(item1.Value);
                    a.SeasonId = item1.Name;
                    resp.Add(a);
                }
            }
            return resp;
        }

        public async Task<List<SVPokemonHomeTrainerRankData>> GetSVTrainerDataAsync(string sessionId, int rst, int ts1, int page = 1)
        {
            List<SVPokemonHomeTrainerRankData> res = new();


            if (page == -1)
            {
                page = 1;
                while (page < 50)
                {
                    var response = await _httpClient.GetAsync(string.Format(TrainerUrl, sessionId, rst, ts1, page));
                    if (!response.IsSuccessStatusCode) break;
                    MemoryStream output = new MemoryStream();
                    using var decompressor = new GZipStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress);
                    decompressor.CopyTo(output);
                    var gfata = Encoding.UTF8.GetString(output.ToArray());
                    var gd = JsonDocument.Parse(gfata).RootElement;
                    for (int j = 0; j < gd.GetArrayLength(); j++)
                    {
                        res.Add(JsonSerializer.Deserialize<SVPokemonHomeTrainerRankData>(gd[j]));
                    }
                    page++;
                }
            }
            else
            {


                var response = await _httpClient.GetAsync(string.Format(TrainerUrl, sessionId, rst, ts1, page));
                MemoryStream output = new MemoryStream();
                using var decompressor = new GZipStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress);
                decompressor.CopyTo(output);
                var gfata = Encoding.UTF8.GetString(output.ToArray());
                var gd = JsonDocument.Parse(gfata).RootElement;
                for (int j = 0; j < gd.GetArrayLength(); j++)
                {
                    res.Add(JsonSerializer.Deserialize<SVPokemonHomeTrainerRankData>(gd[j]));

                }
            }
            return res;
        }


        public async Task<List<SVPokemonHomeTrainerRankData>> GetSVTrainerDataAsync(SVPokemonHomeSession pokemonHomeSession, int page = 1)
        {
            return await GetSVTrainerDataAsync(pokemonHomeSession.SeasonId, pokemonHomeSession.RST, pokemonHomeSession.TS1, page);
        }
    }
}

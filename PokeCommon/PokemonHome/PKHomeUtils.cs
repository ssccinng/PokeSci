using Newtonsoft.Json;
using PokeCommon.Models;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

//using static System.Text.Json.JsonSerializer;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PokeCommon.PokemonHome
{
    public class PKHomeUtils
    {

        private readonly static string[] lang_suffix = new string[] { "ja", "us", "fr", "it", "de", "es", "ko", "sc", "tc" };

        static readonly string formjson = "https://resource.pokemon-home.com/battledata/json/zkn_form_{0}.json";
        static readonly string tokuseijson = "https://resource.pokemon-home.com/battledata/json/tokuseiinfo_{0}.json";
        static readonly string itemjson = "https://resource.pokemon-home.com/battledata/json/iteminfo_{0}.json";
        static readonly string itemnamejson = "https://resource.pokemon-home.com/battledata/json/itemname_{0}.json";
        static readonly string wazainfojson = "https://resource.pokemon-home.com/battledata/json/wazainfo_{0}.json";

        public static readonly string BundleUrl = "https://resource.pokemon-home.com/battledata/js/bundle.js";
        public static readonly string RankmatchApiUrl = "https://api.battle.pokemon-home.com/cbd/competition/rankmatch/list";
        public static readonly string RankmatchApiSVUrl = "https://api.battle.pokemon-home.com/tt/cbd/competition/rankmatch/list";
        //public readonly static string PDataUrl = "https://resource.pokemon-home.com/battledata/ranking/{0}/{1}/{2}/pdetail-{3}";
        public readonly static string PJsonDataUrl = "https://resource.pokemon-home.com/battledata/ranking/scvi/{0}/{1}/{2}/pokemon";
        public readonly static string PDataUrl = "https://resource.pokemon-home.com/battledata/ranking/scvi/{0}/{1}/{2}/pdetail-{3}";
        //private string _trainerUrl = "https://resource.pokemon-home.com/battledata/ranking/{0}/{1}/{2}/traner-{3}";
        public readonly static string TrainerUrl = "https://resource.pokemon-home.com/battledata/ranking/scvi/{0}/{1}/{2}/traner-{3}";

//        private readonly static string _header = @"accept: application/json, text/javascript, */*; q=0.01
//countrycode: 304
//authorization: Bearer
//langcode: 1
//user-agent: Mozilla/5.0 (Linux; Android 8.0; Pixel 2 Build/OPD3.170816.012) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Mobile Safari/537.36
//Accept-Encoding: gzip";
        private readonly static string _header = @"accept: application/json, text/javascript, */*; q=0.01
countrycode: 304
langcode: 1
user-agent: Mozilla/5.0 (Linux; Android 8.0; Pixel 2 Build/OPD3.170816.012) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.61 Mobile Safari/537.36
Accept-Encoding: gzip";
        //Accept-Encoding: gzip";
        //Accept-Encoding: gzip,deflate";

        //private static HttpClient  _httpClient = new();
        private static HttpClient  _httpClient = InitHttpClient();

        private static Regex _getDex = new Regex(@"this.dex=(((?'Open'\{)[^\{\}]*)+((?'Close-Open'\})[^\{\}]*)+)*(?(Open)(?!))");
        private static Regex _getPokeType = new Regex(@"pokeType:(((?'Open'\{)[^\{\}]*)+((?'Close-Open'\})[^\{\}]*)+)*(?(Open)(?!))");
        private static Regex _getPokeType1 = new Regex(@"pokeType:(\{                          #普通字符“(”

                            (                       #分组构造，用来限定量词“*”修饰范围

                                (?<Open>\{)         #命名捕获组，遇到开括弧’Open’计数加1

                            |                       #分支结构

                                (?<-Open>\})        #狭义平衡组，遇到闭括弧’Open’计数减1

                            |                       #分支结构

                                [^{}]+              #非括弧的其它任意字符

                            )*                      #以上子串出现0次或任意多次

                            (?(Open)(?!))           #判断是否还有’Open’，有则说明不配对，什么都不匹配

                        \})                        #普通闭括弧

                     ", RegexOptions.IgnorePatternWhitespace);

        static HttpClient InitHttpClient()
        {
            HttpClient httpClient = new();
            foreach (var item in _header.Split("\r\n"))
            {
                var data = item.Split(": ");
                httpClient.DefaultRequestHeaders.Add(data[0], data[1]);
            }
            return httpClient;
        }

        public PKHomeUtils()
        {
            foreach (var item in _header.Split("\r\n"))
            {
                var data = item.Split(": ");
                _httpClient.DefaultRequestHeaders.Add(data[0], data[1]);
            }
        }

        public static async Task<byte[]> GetBundleAsync()
        {
            return await _httpClient.GetByteArrayAsync(BundleUrl);
        }

        public static async Task<List<SVPokemonHomeSession>> GetSVPokemonHomeSessionsAsync()
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
        static string[] GetPoke(string str)
        {
            var regex = new Regex(@"poke:(\[(.*?)\])");
            var match = regex.Match(str);
            var result = match.Groups[1].Value.Replace("[", "").Replace("]", "").Replace("\"", "").Split(",");
            return result;
        }

        static string[] GetPokeType(string str)
        {
            var regex = new Regex(@"pokeType:(\[(.*?)\])");
            var match = regex.Match(str);
            var result = match.Groups[1].Value.Replace("[", "").Replace("]", "").Split(",");
            return result;
        }

        static string[] GetWaza(string str)
        {
            var regex = new Regex(@"waza:(\{([\s\S\n]*?)\})");
            var regex1 = new Regex(@"\d:""(.+?)""");
            var match = regex.Match(str);
            // System.Console.WriteLine(match.Groups[1].Value);
            //var result = match.Groups[1].Value.Replace("{", "").Replace("}", "").Trim().Split(",").Select(s => { return s.Split(":")[1]; }).ToArray();
            var result = regex1.Matches(match.Groups[1].Value).Select(s => { return s.Groups[1].Value; }).ToArray();
            return result;
        }

        static string[] GetTokusei(string str)
        {
            var regex = new Regex(@"tokusei:(\{([\s\S\n]*?)\})");
            var regex1 = new Regex(@"\d:""(.+?)""");

            var match = regex.Match(str);
            // System.Console.WriteLine(match.Groups[1].Value);
            //var result = match.Groups[1].Value.Replace("{", "").Replace("}", "").Trim().Split(",").Select(s => { return s.Split(":")[1]; }).ToArray();
            var result = regex1.Matches(match.Groups[1].Value).Select(s => { return s.Groups[1].Value; }).ToArray();

            return result;
        }

        static string[] GetSeikaku(string str)
        {
            var regex = new Regex(@"seikaku:(\{([\s\S\n]*?)\})");
            var match = regex.Match(str);
            // System.Console.WriteLine(match.Groups[1].Value);
            var result = match.Groups[1].Value.Replace("{", "").Replace("}", "").Trim().Split(",").Select(s => { return s.Split(":")[1]; }).ToArray();
            return result;
        }


        public static List<PokeModel> PokeModels;
        public static List<PokeModel> WazaModels ;
        public static List<PokeModel> ItemModels ;
        public static List<PokeModel> AbilityModels;
        public static Dictionary<int, Dictionary<string, PokeModel>> FormsModels;
        public static Dictionary<int, Dictionary<int, List<int>>> PokeTypes;
        // 使用前先调用这个
        public static async Task LoadAll()
        {

            PokeModels = JsonSerializer.Deserialize<List<PokeModel>>(File.ReadAllBytes("homedata/pokenameall.json"));
            WazaModels = JsonSerializer.Deserialize<List<PokeModel>>(File.ReadAllBytes("homedata/wazanameall.json"));
            ItemModels = JsonSerializer.Deserialize<List<PokeModel>>(File.ReadAllBytes("homedata/itemnameall.json"));
            AbilityModels = JsonSerializer.Deserialize<List<PokeModel>>(File.ReadAllBytes("homedata/tokuseinameall.json"));

            FormsModels = JsonSerializer.Deserialize<Dictionary<int, Dictionary<string, PokeModel>>>(File.ReadAllBytes("homedata/formnameall.json"));

            PokeTypes = JsonSerializer.Deserialize<Dictionary<int, Dictionary<int, List<int>>>>(File.ReadAllBytes("homedata/poketype.json"));



        }

        public static async Task UpdatePokeType()
        {
            // 后续优化
            string bundles = await _httpClient.GetStringAsync(BundleUrl);

            var dex = _getPokeType1.Matches(bundles);

            if (!Directory.Exists("homedata"))
            {
                Directory.CreateDirectory("homedata");
            }
            Dictionary<string, Dictionary<string, List<int>>> dictionaryData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<int>>>>(dex.Last().Groups[1].Value.Replace("1e3", "1000"));
            File.WriteAllText("homedata/poketype.json", JsonConvert.SerializeObject(dictionaryData));
            //File.WriteAllText("homedata/poketype.json", dex.Last().Groups[1].Value);
            //var options = new JsonSerializerOptions
            //{
            //    NumberHandling = JsonNumberHandling.AllowReadingFromString
            //};

            List<PokeModel> pokeModels = new();
        }

        public static async Task UpdateAll()
        {
            // 访问bundle并更新译名数据

            string bundles = await _httpClient.GetStringAsync(BundleUrl);

            // 此为获取不同语言的数据
            var dex = _getDex.Matches(bundles);
            List<PokeModel> pokeModels = new();
            List<PokeModel> itemModels = new();
            List<PokeModel> wazaModels = new();
            List<PokeModel> tokuseiModels = new();
            Dictionary<int, Dictionary<string, PokeModel>> formModels = new();
            for (int i = 0; i <= 3000; i++)
            {

                pokeModels.Add(new PokeModel { Id = i });
                itemModels.Add(new PokeModel { Id = i });
                wazaModels.Add(new PokeModel { Id = i });
                tokuseiModels.Add(new PokeModel { Id = i });
                //formModels.Add(new ());

            }

            if (!Directory.Exists("homedata"))
            {
                Directory.CreateDirectory("homedata");
            }
            File.WriteAllText("homedata/bundle.js", bundles);

            if (!Directory.Exists("homedata/wazainfo"))
            {
                Directory.CreateDirectory("homedata/wazainfo");
            }
            if (!Directory.Exists("homedata/iteminfo"))
            {
                Directory.CreateDirectory("homedata/iteminfo");
            }
            if (!Directory.Exists("homedata/itemname"))
            {
                Directory.CreateDirectory("homedata/itemname");
            }
            if (!Directory.Exists("homedata/tokuseiinfo"))
            {
                Directory.CreateDirectory("homedata/tokuseiinfo");
            }
            if (!Directory.Exists("homedata/zkn_form"))
            {
                Directory.CreateDirectory("homedata/zkn_form");
            }


            for (int i = 0; i < 9; i++)
            {
                string zkn_forms = await _httpClient.GetStringAsync(string.Format(formjson, lang_suffix[i]));
                File.WriteAllText($"homedata/zkn_form/{Path.GetFileName(string.Format(formjson, lang_suffix[i]))}", zkn_forms);


                File.WriteAllText($"homedata/tokuseiinfo/{Path.GetFileName(string.Format(tokuseijson, lang_suffix[i]))}", await _httpClient.GetStringAsync(string.Format(tokuseijson, lang_suffix[i])));
                File.WriteAllText($"homedata/iteminfo/{Path.GetFileName(string.Format(itemjson, lang_suffix[i]))}", await _httpClient.GetStringAsync(string.Format(itemjson, lang_suffix[i])));


                string itemnames = await _httpClient.GetStringAsync(string.Format(itemnamejson, lang_suffix[i]));
                File.WriteAllText($"homedata/itemname/{Path.GetFileName(string.Format(itemnamejson, lang_suffix[i]))}", itemnames);


                File.WriteAllText($"homedata/wazainfo/{Path.GetFileName(string.Format(wazainfojson, lang_suffix[i]))}", await _httpClient.GetStringAsync(string.Format(wazainfojson, lang_suffix[i])));

                var itemnameArray = Regex.Matches(itemnames, @"""\d+?"": ""(.+?)""");

                for (int j = 0; j < itemnameArray.Count; j++)
                {
                    itemModels[j + 1][i] = itemnameArray[j].Groups[1].Value;
                }

                var zkn_formArray = Regex.Matches(zkn_forms, @"""(\d+?)_([0-9_]+?)"": ""(.+?)""");

                for (int j = 0; j < zkn_formArray.Count; j++)
                {
                    var id = int.Parse(zkn_formArray[j].Groups[1].Value);
                    //var form = int.Parse(zkn_formArray[j].Groups[2].Value);
                    if (!formModels.ContainsKey(id))
                    {
                        formModels[id] = new();
                    }
                    formModels[id].TryAdd(zkn_formArray[j].Groups[2].Value, new PokeModel { Id = id });
                    formModels[id][zkn_formArray[j].Groups[2].Value][i] = zkn_formArray[j].Groups[3].Value;
                }




            }

               
            for (int i = 0; i < dex.Count - 1; i++)
            {
                Console.WriteLine(i);
                var ss = dex[i].Value.ToString();
                var GetPokess = GetPoke(ss);
                var GetTokuseis = GetTokusei(ss);
                var GetWazas = GetWaza(ss);

                for (int j = 0; j < GetPokess.Length; j++)
                {
                    pokeModels[j + 1][i] = GetPokess[j];
                }

                for (int j = 0; j < GetTokuseis.Length; j++)
                {
                    tokuseiModels[j + 1][i] = GetTokuseis[j];
                }

                for (int j = 0; j < GetWazas.Length; j++)
                {
                    wazaModels[j + 1][i] = GetWazas[j];
                }
            }

                JsonSerializerOptions options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            File.WriteAllText($"homedata/pokenameall.json", JsonSerializer.Serialize(pokeModels.Skip(1).Where(s => s.Name_Eng != null).ToList(), options));
            File.WriteAllText($"homedata/itemnameall.json", JsonSerializer.Serialize(itemModels.Skip(1).Where(s => s.Name_Eng != null).ToList(), options));
            File.WriteAllText($"homedata/wazanameall.json", JsonSerializer.Serialize(wazaModels.Skip(1).Where(s => s.Name_Eng != null).ToList(), options));
            File.WriteAllText($"homedata/tokuseinameall.json", JsonSerializer.Serialize(tokuseiModels.Skip(1).Where(s => s.Name_Eng != null).ToList(), options));

            File.WriteAllText($"homedata/formnameall.json", JsonSerializer.Serialize(formModels, options));


        }


        public static async Task<List<SVPokemonHomeTrainerRankData>> GetSVTrainerDataAsync(string sessionId, int rst, int ts1, int page = 1)
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


        public static async Task<List<SVPokemonHomeTrainerRankData>> GetSVTrainerDataAsync(SVPokemonHomeSession pokemonHomeSession, int page = 1)
        {
            return await GetSVTrainerDataAsync(pokemonHomeSession.SeasonId, pokemonHomeSession.RST, pokemonHomeSession.TS1, page);
        }


        public static async Task GetSVPokemonRankdataAsync(SVPokemonHomeSession pokemonHomeSession)
        {
            await GetSVPokemonRankdataAsync(pokemonHomeSession.SeasonId, pokemonHomeSession.RST, pokemonHomeSession.TS2);
        }

        public static async Task GetSVPokemonRankdataAsync(string sessionId, int rst, int ts1)
        {
            var pokedata = await _httpClient.GetStringAsync(string.Format(PJsonDataUrl, sessionId, rst, ts1));

            for (int j = 1; j <= 5; j++)
            {
                var data = await _httpClient.GetStringAsync(string.Format(PDataUrl, sessionId, rst, ts1, j));
            }
        }
    }
}

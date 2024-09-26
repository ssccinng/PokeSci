using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSStatClawer
{

    public enum StatisticScore
    {
        _0 = 0,
        _1500 = 1500,
        _1630 = 1630,
        _1760 = 1760
    }

    public static partial class Utils
    {
        public static readonly string StatisticUrl = "https://www.smogon.com/stats/";

        public static readonly HttpClient Client = new HttpClient { BaseAddress = new(StatisticUrl) };

        [GeneratedRegex(@"href=""(.+?)/""", RegexOptions.IgnoreCase)]
        public  static partial Regex GetHref();

        // moveset, chaos
        // 0 1500 1630 1760
        public async static Task<string[]> GetDates()
        {
            var data = await Client.GetStringAsync("");

            var match = GetHref().Matches(data);

            string[] dates = new string[match.Count - 2];

            for (int i = 2; i < match.Count; i++)
            {
                dates[i-2] = match[i-2].Groups[1].Value;
            }
            return dates;

        }

        public async static Task<Dictionary<string, ItemProbability>> GetChaos(string date, string rule, StatisticScore statisticScore)
        {
            var data = await Client.GetByteArrayAsync($"{date}/chaos/{rule}-{(int)statisticScore}.json.gz");

            using GZipStream stream = new GZipStream(new System.IO.MemoryStream(data), CompressionMode.Decompress);

            using var reader = new System.IO.StreamReader(stream);

            return JsonSerializer.Deserialize< Dictionary<string, ItemProbability>>(reader.ReadToEnd());

        }
    }
}

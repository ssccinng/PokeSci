using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Linq;
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

        [GeneratedRegex(@"href=""([^/]+?)""", RegexOptions.IgnoreCase)]
        public static partial Regex GetRuleHref();

        // moveset, chaos
        // 0 1500 1630 1760
        public async static Task<string[]> GetDates()
        {
            var data = await Client.GetStringAsync("");

            var match = GetHref().Matches(data);

            string[] dates = new string[match.Count];

            for (int i = 0; i < match.Count; i++)
            {
                dates[i] = match[i].Groups[1].Value;
            }
            return dates;

        }

        public async static Task<string[]> GetRules(string date)
        {
            var data = await Client.GetStringAsync($"{date}/");
            var match = GetRuleHref().Matches(data);
            string[] rules = new string[match.Count - 0];
            for (int i = 0; i < match.Count; i++)
            {
                rules[i - 0] = match[i - 0].Groups[1].Value;
            }

            rules = rules.Select(s => s.Split('.')[0]).Distinct().ToArray();

            return rules;

        }

        public async static Task<Dictionary<string, ItemProbability>> GetChaos(string date, string rule)
        {
            var data = await Client.GetByteArrayAsync($"{date}/chaos/{rule}.json.gz");

            using GZipStream stream = new GZipStream(new System.IO.MemoryStream(data), CompressionMode.Decompress);

            using var reader = new System.IO.StreamReader(stream);

            var datastr = reader.ReadToEnd();

            var jsonData = JsonSerializer.Deserialize<JsonElement>(datastr).GetProperty("data");

            return JsonSerializer.Deserialize<Dictionary<string, ItemProbability>>(jsonData);
        }
        public async static Task<Dictionary<string, ItemProbability>> GetChaos(string date, string rule, StatisticScore statisticScore)
        {

            return await GetChaos(date, $"{rule}-{statisticScore}");


        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSStatClawer
{
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

        public async static Task<string> GetChaos(string date, string rule)
        {
            var data = await Client.GetStringAsync(date);
            return data;
        }
    }
}

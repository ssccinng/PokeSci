using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace RK9Tool
{
    public static partial class RK9Client
    {
        [GeneratedRegex(@"<table id=""dtUpcomingEvents"" .+?>([\s\S]+?)</table>", RegexOptions.IgnoreCase)]
        public static partial Regex GetUpcomingTable();


        [GeneratedRegex(@"<table id=""dtPastEvents"" .+?>([\s\S]+?)</table>", RegexOptions.IgnoreCase)]
        public static partial Regex GetPastTable(); 
        
        [GeneratedRegex(@"<table id=""dtLiveRoster"" .+?>([\s\S]+?)</table>", RegexOptions.IgnoreCase)]
        public static partial Regex GetLiveRosterTable();

        // todo: 整一点通用的正则表达式
        [GeneratedRegex(@"<tr.*?>([\s\S]+?)</tr>", RegexOptions.IgnoreCase)]
        public static partial Regex GetTr();

        [GeneratedRegex(@"<td.*?>([\s\S]+?)</t[d|h]>", RegexOptions.IgnoreCase)]
        public static partial Regex GetTd();
        [GeneratedRegex(@"<th.*?>([\s\S]+?)</th>", RegexOptions.IgnoreCase)]
        public static partial Regex GetTh();

        [GeneratedRegex(@"<a href=""(.+?)"".*?>([\s\S]+?)</a>", RegexOptions.IgnoreCase)]
        public static partial Regex GetHref();

        [GeneratedRegex(@"<img src=""(.+?)"".*?>", RegexOptions.IgnoreCase)]
        public static partial Regex GetImg();

        //[GeneratedRegex(@"<div\b[^>]*>([\s\S]*?((?:(?!</?div\b).)*?)*?)</div>", RegexOptions.IgnoreCase)]
        //[GeneratedRegex(@"<div\b[^>]*>([\s\S]*?((?:(?!</?div\b).)*?)*?)</div>", RegexOptions.IgnoreCase)]
        [GeneratedRegex(@"<div[^\)]*>[^\(\)]*(((?'Open'<div[^\)]*>)[^\(\)]*)+((?'-Open'</div>)[^\(\)]*)+)*(?(Open)(?!))</div>", RegexOptions.IgnoreCase)]
        //[GeneratedRegex(@"(((?'Open'<div>)[^(<div>|</div>)]*)+((?'Close-Open'\</div>)[^(<div>|</div>)]*)+)*(?(Open)(?!))", RegexOptions.IgnoreCase)]
        public static partial Regex GetDiv();


        private static HttpClient _client = GetClient();

        static string _eventListUrl = "/events/pokemon";
        static string _tournamentUrl = "/tournament";
        static string _rosterUrl = "/roster";
        static string _pairingsUrl = "/pairings";
        //static string _eventListUrl = "https://rk9.gg/events/pokemon";

        public static async Task<List<PokemonEvent>> GetEvents()
        {
            List<PokemonEvent> events = new();
            HttpResponseMessage response = await _client.GetAsync(_eventListUrl);
            if (response.IsSuccessStatusCode)
            {
                string html = await response.Content.ReadAsStringAsync();
                var match = GetUpcomingTable().Match(html);

                if (match.Success)
                {
                    var trs = GetTr().Matches(match.Groups[1].Value);
                    foreach (Match tr in trs)
                    {
                        PokemonEvent? pokemonEvent = GetEvent(tr);
                        if (pokemonEvent == null) continue;
                        events.Add(pokemonEvent);


                    }
                }

                match = GetPastTable().Match(html);

                if (match.Success)
                {
                    var trs = GetTr().Matches(match.Groups[1].Value);
                    foreach (Match tr in trs)
                    {
                        PokemonEvent? pokemonEvent = GetEvent(tr);
                        if (pokemonEvent == null) continue;
                        pokemonEvent.Past = true;
                        events.Add(pokemonEvent);


                    }
                }


            }



            return events;
        }

        private static PokemonEvent GetEvent(Match tr)
        {
            var tds = GetTd().Matches(tr.Groups[1].Value);
            if (tds.Count < 5)
            {
                return null;
            }
            PokemonEvent pokemonEvent = new();
            pokemonEvent.Date = tds[0].Groups[1].Value;
            var nameHref = GetHref().Match(tds[2].Groups[1].Value);
            var name = nameHref.Groups[2].Value;
            pokemonEvent.Name = name;
            pokemonEvent.EventUrl = nameHref.Groups[1].Value;

            pokemonEvent.Location = tds[3].Groups[1].Value;

            var matchHrefs = GetHref().Matches(tds[4].Groups[1].Value);



            foreach (Match matchHref in matchHrefs)
            {
                var href = matchHref.Groups[1].Value.Split('/').Last();
                var text = matchHref.Groups[2].Value.Trim();

                text = GetImg().Replace(text, "").Replace("&nbsp;", "").Trim();

                //Console.WriteLine(text);
                pokemonEvent.FormatUrl.TryAdd(text, href);

            }

            return pokemonEvent;
        }

        public static async Task<List<MatchPlayer>> GetMatchPlayers(string id)
        {
            List<MatchPlayer> matchPlayers = new();
            HttpResponseMessage response = await _client.GetAsync($"{_rosterUrl}/{id}");

            if (response.IsSuccessStatusCode)
            {
                string html = await response.Content.ReadAsStringAsync();
                var match = GetLiveRosterTable().Match(html);

                if (match.Success)
                {
                    var trs = GetTr().Matches(match.Groups[1].Value);
                    var ths = GetTh().Matches(trs[0].Groups[1].Value);
                    var thsText = ths.Select(th => th.Groups[1].Value).ToList();
                    foreach (Match tr in trs.Skip(1))
                    {
                        var tds = GetTd().Matches(tr.Groups[1].Value);
                        if (tds.Count < thsText.Count)
                        {
                            continue;
                        }
                        JsonNode matchPlayer = JsonNode.Parse("{}")!;

                        for (int i = 0; i < thsText.Count; i++)
                        {
                            if (thsText[i] == "Team List")
                            {
                                matchPlayer["TeamListUrl"] = GetHref().Matches(tds[i].Groups[1].Value)[0].Groups[1].Value;
                                continue;
                            }

                            if (thsText[i] == "Standing")
                            {
                                matchPlayer["Standing"] = int.Parse(tds[i].Groups[1].Value);
                                continue;
                            }

                            matchPlayer[thsText[i].Replace(" ", "")] = tds[i].Groups[1].Value.Replace("&nbsp;", "").Trim();
                        }

                        matchPlayers.Add(matchPlayer.Deserialize<MatchPlayer>(new JsonSerializerOptions { PropertyNameCaseInsensitive = true}));
                    }
                }
            }

            return matchPlayers;

        }

        public static async Task<List<MatchPairing>> GetMatchPairings(string id)
        {
            List<MatchPairing> matchPairings = new();

            for (int pod = 0; pod < 3; ++pod)
            {
                int rnd = 1;
                while (true)
                {
                    HttpResponseMessage response = await _client.GetAsync($"{_pairingsUrl}/{id}?pod={pod}&rnd={rnd}");

                    if (response.IsSuccessStatusCode)
                    {
                        string html = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(html))
                        {
                            break;
                        }
                        var match = GetDiv().Matches(html);

                        for (int i = 0; i < match.Count; i++)
                        {
                            var div = match[i].Groups[0].Value;
                            var matchPairing = new MatchPairing();
                            var trs = GetTr().Matches(div);
                            if (trs.Count < 2)
                            {
                                continue;
                            }
                            var tds = GetTd().Matches(trs[0].Groups[1].Value);
                            if (tds.Count < 3)
                            {
                                continue;
                            }
                            matchPairing.Table = int.Parse(tds[0].Groups[1].Value);
                            matchPairing.Player1Name = tds[1].Groups[1].Value;
                            matchPairing.Player2Name = tds[2].Groups[1].Value;

                            tds = GetTd().Matches(trs[1].Groups[1].Value);
                            if (tds.Count < 3)
                            {
                                continue;
                            }
                            matchPairing.Player1Score = tds[1].Groups[1].Value;
                            matchPairing.Player2Score = tds[2].Groups[1].Value;

                            matchPairings.Add(matchPairing);
                        }

                        
                        
                    }
                    else
                    {
                        break;
                    }
                    rnd++;
                }

            }
            return matchPairings;

        }

        public static HttpClient GetClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://rk9.gg");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

    }
}

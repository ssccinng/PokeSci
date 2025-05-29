using PokeCommon.Models;
using PokeCommon.Utils;
using PokemonDataAccess.Models;
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

        [GeneratedRegex(@">([^<]+?)<", RegexOptions.IgnoreCase)]
        public static partial Regex GetTexts();
        [GeneratedRegex(@"id=""P(\d)-tab""", RegexOptions.IgnoreCase)]
        public static partial Regex GetPTabs();

        //[GeneratedRegex(@"<div\b[^>]*>([\s\S]*?((?:(?!</?div\b).)*?)*?)</div>", RegexOptions.IgnoreCase)]
        //[GeneratedRegex(@"<div\b[^>]*>([\s\S]*?((?:(?!</?div\b).)*?)*?)</div>", RegexOptions.IgnoreCase)]
        //[GeneratedRegex(@"<div[^\)]*>[^\(\)]*(((?'Open'<div[^\)]*>)[^\(\)]*)+((?'-Open'</div>)[^\(\)]*)+)*(?(Open)(?!))</div>", RegexOptions.IgnoreCase)]
        [GeneratedRegex(@"(?isx)                      #匹配模式，忽略大小写，“.”匹配任意字符

                      <div[^>]*>                      #开始标记“<div...>”

                          ((?>                         #分组构造，用来限定量词“*”修饰范围

                              <div[^>]*>  (?<Open>)   #命名捕获组，遇到开始标记，入栈，Open计数加1

                          |                           #分支结构

                              </div>  (?<-Open>)      #狭义平衡组，遇到结束标记，出栈，Open计数减1

                          |                           #分支结构

                              (?:(?!</?div\b).)*      #右侧不为开始或结束标记的任意字符

                          )*                          #以上子串出现0次或任意多次

                          (?(Open)(?!)))               #判断是否还有'OPEN'，有则说明不配对，什么都不匹配

                      </div>                          #结束标记“</div>”", RegexOptions.IgnoreCase)]
        //[GeneratedRegex(@"(?isx)                      #匹配模式，忽略大小写，“.”匹配任意字符

        //              <div[^>]*>                      #开始标记“<div...>”

        //                  (?>                         #分组构造，用来限定量词“*”修饰范围

        //                      <div[^>]*>  (?<Open>)   #命名捕获组，遇到开始标记，入栈，Open计数加1

        //                  |                           #分支结构

        //                      </div>  (?<-Open>)      #狭义平衡组，遇到结束标记，出栈，Open计数减1

        //                  |                           #分支结构

        //                      (?:(?!</?div\b).)*      #右侧不为开始或结束标记的任意字符

        //                  )*                          #以上子串出现0次或任意多次

        //                  (?(Open)(?!))               #判断是否还有'OPEN'，有则说明不配对，什么都不匹配

        //              </div>                          #结束标记“</div>”", RegexOptions.IgnoreCase)]
        //[GeneratedRegex(@"(((?'Open'<div>)[^(<div>|</div>)]*)+((?'Close-Open'\</div>)[^(<div>|</div>)]*)+)*(?(Open)(?!))", RegexOptions.IgnoreCase)]
        public static partial Regex GetDiv();
        [GeneratedRegex(@"(?isx)                      #匹配模式，忽略大小写，“.”匹配任意字符

                      <div[^>]*id=""lang-EN"">                      #开始标记“<div...>”

                          ((?>                         #分组构造，用来限定量词“*”修饰范围

                              <div[^>]*>  (?<Open>)   #命名捕获组，遇到开始标记，入栈，Open计数加1

                          |                           #分支结构

                              </div>  (?<-Open>)      #狭义平衡组，遇到结束标记，出栈，Open计数减1

                          |                           #分支结构

                              (?:(?!</?div\b).)*      #右侧不为开始或结束标记的任意字符

                          )*                          #以上子串出现0次或任意多次

                          (?(Open)(?!)))               #判断是否还有'OPEN'，有则说明不配对，什么都不匹配

                      </div>                          #结束标记“</div>”", RegexOptions.IgnoreCase)]
        private static partial Regex GetTeamDiv();

        [GeneratedRegex(@"(?isx)                      #匹配模式，忽略大小写，“.”匹配任意字符

                      <div class=""pokemon [^>]*>                      #开始标记“<div...>”

                          ((?>                         #分组构造，用来限定量词“*”修饰范围

                              <div[^>]*>  (?<Open>)   #命名捕获组，遇到开始标记，入栈，Open计数加1

                          |                           #分支结构

                              </div>  (?<-Open>)      #狭义平衡组，遇到结束标记，出栈，Open计数减1

                          |                           #分支结构

                              (?:(?!</?div\b).)*      #右侧不为开始或结束标记的任意字符

                          )*                          #以上子串出现0次或任意多次

                          (?(Open)(?!)))               #判断是否还有'OPEN'，有则说明不配对，什么都不匹配

                      </div>                          #结束标记“</div>”", RegexOptions.IgnoreCase)]
        private static partial Regex GetPokeDiv ();


        private static HttpClient _client = GetClient();

        static string _eventListUrl = "/events/pokemon";
        static string _tournamentUrl = "/tournament";
        static string _rosterUrl = "/roster";
        static string _pairingsUrl = "/pairings";
        //static string _eventListUrl = "https://rk9.gg/events/pokemon";

        


        public static async Task<List<PokemonEvent>> GetEventsAsync()
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
            if (!nameHref.Success)
            {
                var name = tds[2].Groups[1].Value.Split('<')[0].Trim();
                pokemonEvent.Name = name;

            }
            else
            {
                var name = nameHref.Groups[2].Value;
                pokemonEvent.Name = name;
                pokemonEvent.EventUrl = nameHref.Groups[1].Value;
            }
           

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
                                var matchHref = GetHref().Match(tds[i].Groups[1].Value);
                                if (!matchHref.Success)
                                {
                                    continue;
                                }
                                matchPlayer["TeamListUrl"] = matchHref.Groups[1].Value;
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
            HttpResponseMessage response1 = await _client.GetAsync($"{_pairingsUrl}/{id}");
            if (!response1.IsSuccessStatusCode)
            {
                return matchPairings;
            }

            var data = await response1.Content.ReadAsStringAsync();

            var matchTabs = GetPTabs().Matches(data);


            for (int pod = 0; pod < matchTabs.Count; ++pod)
            {
                var matchPairing = new MatchPairing() { Division = (Division)(2 - pod) };
                var oo = matchTabs[pod].Groups[1].Value;
                int rnd = 1;
                while (true)
                {
                    HttpResponseMessage response = await _client.GetAsync($"{_pairingsUrl}/{id}?pod={oo}&rnd={rnd}");
                    
                    if (response.IsSuccessStatusCode)
                    {
                        string html = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrWhiteSpace(html))
                        {
                            break;
                        }
                        var match = GetDiv().Matches(html);

                        PairingRound pairingRound = new PairingRound() { RoundId = rnd };

                        for (int i = 0; i < match.Count; i++)
                        {
                            PairingTable pairingTable = new() { };

                            var div = match[i].Groups[1].Value;

                            var matchDivs = GetDiv().Matches(div);

                            if (matchDivs.Count < 3)
                            {
                                continue;
                            }
                            var row = matchDivs[0].Groups[1].Value;
                            var texts = GetTexts().Matches(row);
                            
                            if (texts.Count == 3)
                            {
                                var name1 = texts[0].Groups[1].Value;
                                var name2andCountry = texts[1].Groups[1].Value;
                                var name = $"{name1}{name2andCountry.Split('[')[0]}";
                                //Console.WriteLine(name2andCountry);
                                var country = name2andCountry.Contains("[") ? name2andCountry.Split('[')[1].Split(']')[0] : string.Empty;
                                pairingTable.Player1Name = name.Trim();
                                pairingTable.Player1Country = country.Trim();
                                pairingTable.Player1Score = texts[2].Groups[1].Value.Trim();
                                if (matchDivs[0].Groups[0].Value.Contains("winner"))
                                {
                                    pairingTable.Player1Win = true;
                                }
                            }
                           

                            row = matchDivs[2].Groups[1].Value;
                            texts = GetTexts().Matches(row);
                            if (texts.Count == 3)
                            {
                                var name1 = texts[0].Groups[1].Value;
                                var name2andCountry = texts[1].Groups[1].Value;
                                var name = $"{name1}{name2andCountry.Split('[')[0]}";
                                var country = name2andCountry.Contains("[") ? name2andCountry.Split('[')[1].Split(']')[0] : string.Empty;

                                pairingTable.Player2Name = name.Trim();
                                pairingTable.Player2Country = country.Trim();
                                pairingTable.Player2Score = texts[2].Groups[1].Value.Trim();
                                if (matchDivs[2].Groups[0].Value.Contains("winner"))
                                {
                                    pairingTable.Player2Win = true;
                                }
                            }

                            row = matchDivs[1].Groups[1].Value;
                            texts = GetTexts().Matches(row);
                            if (texts.Count == 1)
                            {
                               int.TryParse(texts[0].Groups[1].Value, out var t);
                                pairingTable.Table = t;
                                 
                            }

                            pairingRound.Pairings.Add(pairingTable);

                        }

                        matchPairing.PairingRounds.Add(pairingRound);
                        
                        
                    }
                    else
                    {
                        break;
                    }
                    rnd++;
                }
                matchPairings.Add(matchPairing);


            }
            return matchPairings;

        }
        static Pokemon[] pokemons = PokemonTools.PokemonContext.Pokemons.ToArray();
        public static async Task<GamePokemonTeam> GetPokemonTeamAsync(string url)
        {
            HttpResponseMessage response = await _client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string html = await response.Content.ReadAsStringAsync();
                var match = GetTeamDiv().Match(html);
                if (match.Success)
                {
                    GamePokemonTeam gamePokemonTeam = new();
                    var pokes = GetDiv().Matches(match.Groups[1].Value);

                    if (pokes.Count == 1)
                    {
                        pokes = GetDiv().Matches(pokes[0].Groups[1].Value);
                    }

                    for (int i = 0; i < pokes.Count; i++)
                    {
                        var poke = pokes[i].Groups[1].Value;
                        var img = GetImg().Match(poke).Groups[1].Value;
                        var pokeimgName = img.Split('/').Last().Split('.')[0];

                        var pokeid = int.Parse(pokeimgName.Split('_')[0]);
                        var pokeFormid = int.Parse(pokeimgName.Split('_')[1]);

                        if (pokeid == 1017)
                        {
                            if (pokeFormid == 1)
                            {
                                pokeFormid = 2;
                            }
                            else if (pokeFormid == 2)
                            {
                                
                                    pokeFormid = 1;
                                
                            }
                        }
                        GamePokemon gamePokemon = new(pokemons.FirstOrDefault(s => s.DexId == pokeid && s.PokeFormId == pokeFormid));

                        var matchPokeText = GetTexts().Matches(poke);


                        var texts = matchPokeText.Select(s => s.Groups[1].Value.Replace("&nbsp;", "").Trim()).Where(s => !string.IsNullOrEmpty(s)).ToList();

                        if (texts.Count < 8)
                        {
                            continue;
                        }
                        int idx = texts.FindIndex(0, s => s == "Tera Type:") + 1;
                        gamePokemon.TreaType = await PokemonTools.GetTypeAsync(texts[idx].Replace('’', '\'').Replace("&#39;", "'"));
                        idx = texts.FindIndex(0, s => s == "Ability:") + 1;
                        gamePokemon.Ability = await PokemonTools.GetAbilityAsync(texts[idx].Replace('’', '\'').Replace("&#39;", "'"));
                        if (gamePokemon.Ability == null)
                        {
                            Console.WriteLine();
                        }
                        idx = texts.FindIndex(0, s => s == "Held Item:") + 1;
                        gamePokemon.Item = await PokemonTools.GetItemAsync(texts[idx].Replace('’', '\'').Replace("&#39;", "'"));
                        if (gamePokemon.Item == null)
                        {
                            Console.WriteLine();
                        }
                        idx++;

                        for (int m = idx; m < texts.Count; m++)
                        {
                            if (string.IsNullOrEmpty(texts[m]))
                            {
                                continue;
                            }
                            gamePokemon.Moves.Add(new GameMove(await PokemonTools.GetMoveAsync(texts[m])));
                            
                        }


                        gamePokemonTeam.GamePokemons.Add(gamePokemon);

                    }

                    return gamePokemonTeam;

                }
            }
            return null;
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

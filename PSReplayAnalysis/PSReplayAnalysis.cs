using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PSReplayAnalysis
{
    public class PSReplayAnalysis
    {
        public static ImmutableDictionary<string, PSPokemon> Pokemons =
            JsonSerializer.Deserialize<List<PSPokemon>>(File.ReadAllText("PSPokemons.json"))!.ToImmutableDictionary(s => s.PSName, s => s);
        public static ImmutableDictionary<string, PSData> PsPokes =
            JsonSerializer.Deserialize<List<PSData>>(File.ReadAllText("Pokedata.zqd"))!.ToImmutableDictionary(s => s.name, s => s);

        public static string ConvFile(string path)
        {
            var txt = File.ReadAllText(path);
            var sidx = txt.IndexOf(">battle-gen");
            var eidx = txt.IndexOf("</script>", sidx);

            var tt = txt.Substring(sidx, eidx - sidx);
            //tt.Length
            var cc = tt.Trim();
            return cc;

        }
        /// <summary>
        /// 分析具体文件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static BattleData Thonk(string data)
        {
            BattleData battle = new BattleData();
            battle.BattleTurns.Add(new BattleTurn
            {
                TurnId = 0,
            });
            var datalines = data.Split('\n');
            foreach (var line in datalines)
            {
                string[] d = line.Split('|');
                if (d.Length < 2) continue;
                switch (d[1])
                {
                    case "poke":
                        var pspoke = PokeToId(d[3].Split(',')[0]);

                        if (d[2] == "p1")
                        {
                            // 赋予种族
                            battle.Player1Team.Pokemons.Add(new Pokemon
                            {
                                Id = pspoke.num
                            });

                        }
                        else
                        {
                            battle.Player2Team.Pokemons.Add(new Pokemon
                            {
                                Id = pspoke.num
                            }) ;
                        }
                        break;
                    case "turn":
                        // next turn
                        battle.BattleTurns.Add(battle.BattleTurns.Last().NextTurn());
                        break;
                    case "switch":
                        // 切换
                        var switchName = d[3].Split(',')[0];
                        var swpoke = PokeToId(switchName);

                        if (d[2].StartsWith("p1a"))
                        {
                            //battle.p
                        }
                        else if (d[2].StartsWith("p1b"))
                        {

                        } 
                        else if (d[2].StartsWith("p2a"))
                        {

                        } 
                        else if (d[2].StartsWith("p2b"))
                        {

                        }
                        break;
                    case "detailschange":
                        // 状态改变
                        break;
                    case "-ability":
                        // 发动特性
                        break;

                    case "-singleturn":
                        break;
                    case "-damage":
                        //受伤
                        break;
                    case "faint":
                        // 被击倒
                        break;  
                    case "move":
                        // 使用技能
                        break;
                    case "-heal":
                        break;
                    case "-fail":
                        // 失败
                        break;
                    case "-sidestart":
                        // 单边状态开始
                        break;
                    case "-sideend":
                        break;
                    case "-enditem":
                        // 物品用完
                        break;
                    case "-supereffective":
                        // 效果绝佳
                        break; 
                    case "-resisted":
                        // 抵抗
                        break;
                    case "activate":
                        // 大概是什么起效果
                        break;
                    case "unboost":
                        // 能力减弱
                        break;

                    case "boost":
                        // 能力提升
                        break;
                    case "-start":
                        // 已知寄生种子
                        break;
                    case "-end":
                        break;
                    case "crit":
                        // ct
                        break;
                    default:
                        // 未处理 输出检查
                        break;

                }
            }

            return battle;
        }

        public static PSData PokeToId(string name)
        {
            string orr = name;
            //var dad = JsonSerializer.Deserialize<List<PSPokemon>>(File.ReadAllText("PSPokemons.json"))!.ToImmutableDictionary(s => s.PSName, s => s);
            name = name.Replace("-*", "");
            PSData p = new ();
            while (!PsPokes.TryGetValue(name, out p))
            {
                name = name.Substring(0, name.LastIndexOf('-'));
            }
            return p;
            throw new Exception();
            //return Pokemons[name];
            //var res = Pokemons.Where(s => name.Contains(s.PSName)).OrderByDescending(s => s.PSName.Length);

            //// 数据库里去搜
            //return res.FirstOrDefault();
        }
    }


    public class PSData
    {
        public int num { get; set; }
        public string name { get; set; }
        public string baseSpecies { get; set; }
        public string forme { get; set; }
        public string[] types { get; set; }
        public Genderratio genderRatio { get; set; }
        public Basestats baseStats { get; set; }
        public Abilities abilities { get; set; }
        public float heightm { get; set; }
        public float weightkg { get; set; }
        public string color { get; set; }
        public string[] eggGroups { get; set; }
        public string requiredItem { get; set; }
    }

    public class Genderratio
    {
        public float M { get; set; }
        public float F { get; set; }
    }

    public class Basestats
    {
        public int hp { get; set; }
        public int atk { get; set; }
        public int def { get; set; }
        public int spa { get; set; }
        public int spd { get; set; }
        public int spe { get; set; }
    }

    public class Abilities
    {
        public string _0 { get; set; }
    }

    public class PSPokemon
    {
        public string Id { get; set; }
        public string PSName { get; set; }
        public string AllValue { get; set; }
        public string PokemonId { get; set; }
    }

}



using PokemonDataAccess.Models;

namespace PokeCommon.PokemonShowdownTools
{
    // 换为ps工具
    public static class PSReplayThonk
    {

        public static void Thonk(string[] Replay, ref PsBattle pSBattle)
        {
            //PSDisplayContext dbContext = new();
            List<PSPokemon> team1 = new(), team2 = new();
            for (int i = 0; i < Replay.Length; ++i)
            {
                string[] data = Replay[i].Split('|');



                //int 
                if (data.Length >= 2)
                {
                    switch (data[1])
                    {
                        // 宝可梦
                        case "poke":
                            //if (data[2] == "p1")
                            //{
                            //    team1.Add(PokeToId(data[3].Split(',')[0], dbContext));

                            //}
                            //else
                            //{
                            //    team2.Add(PokeToId(data[3].Split(',')[0], dbContext));
                            //}
                            break;
                        // 玩家
                        case "player":
                            if (data.Length > 5)
                            {
                                int.TryParse(data[5], out int res);
                                if (data[2] == "p1")
                                {
                                    pSBattle.Rating1 = res;
                                    pSBattle.Player1 = data[3];
                                }
                                else
                                {
                                    pSBattle.Rating2 = res;
                                    pSBattle.Player2 = data[3];
                                }
                            }

                            break;
                        case "win":
                            if (data[2] == pSBattle.Player1)
                            {
                                pSBattle.Whowin = 0;
                            }
                            else
                            {
                                pSBattle.Whowin = 1;
                            }
                            // 找到胜者
                            break;
                        default:
                            break;
                    }
                }

            }

            team1.Sort((x, y) => y.AllValue * 10000 - x.AllValue * 10000 + y.Id - x.Id);
            team2.Sort((x, y) => y.AllValue * 10000 - x.AllValue * 10000 + y.Id - x.Id);
            for (int ii = 0; ii < team1.Count; ++ii)
            {
                if (ii == 0)
                {
                    pSBattle.Player1poke1 = team1[ii].PSName;
                }
                else if (ii == 1)
                {
                    pSBattle.Player1poke2 = team1[ii].PSName;
                }
                else if (ii == 2)
                {
                    pSBattle.Player1poke3 = team1[ii].PSName;
                }
                else if (ii == 3)
                {
                    pSBattle.Player1poke4 = team1[ii].PSName;
                }
                else if (ii == 4)
                {
                    pSBattle.Player1poke5 = team1[ii].PSName;
                }
                else if (ii == 5)
                {
                    pSBattle.Player1poke6 = team1[ii].PSName;
                }
            }
            for (int ii = 0; ii < team2.Count; ++ii)
            {
                if (ii == 0)
                {
                    pSBattle.Player2poke1 = team2[ii].PSName;
                }
                else if (ii == 1)
                {
                    pSBattle.Player2poke2 = team2[ii].PSName;
                }
                else if (ii == 2)
                {
                    pSBattle.Player2poke3 = team2[ii].PSName;
                }
                else if (ii == 3)
                {
                    pSBattle.Player2poke4 = team2[ii].PSName;
                }
                else if (ii == 4)
                {
                    pSBattle.Player2poke5 = team2[ii].PSName;
                }
                else if (ii == 5)
                {
                    pSBattle.Player2poke6 = team2[ii].PSName;
                }
            }
        }
        //public static PSPokemon PokeToId(string name, PSDisplayContext dbContext)
        //{

        //    name = name.Replace("-*", "");
        //    //var vv = dbContext.Pspokemons.ToList();
        //    var res = dbContext.Pspokemons.Where(s => name.Contains(s.Psname)).OrderByDescending(s => s.Psname.Length);

        //    // 数据库里去搜
        //    return res.FirstOrDefault();
        //}
    }
}

using PokeCommon.Utils;
using PokePSCore;

namespace PSAITest
{
    public class AI
    {
        //public string MakeTeamOrder(TeamOrderPolicy teamOrderPolicy, int[] pSBattlePokemons)
        //{
        //    return "";
        //}
        public static async Task<string> MakeTeamOrderAsync(TeamOrderPolicy teamOrderPolicy, PSBattlePokemon[] pSBattlePokemons, PSBattlePokemon[] mypSBattlePokemons)
        {
            int idx = 1;
            return await MakeTeamOrderAsync(teamOrderPolicy, pSBattlePokemons, mypSBattlePokemons.ToDictionary(s => s.PSName, s => idx++));
        }
        public static async Task<string> MakeTeamOrderAsync
            (TeamOrderPolicy teamOrderPolicy, PSBattlePokemon[] pSBattlePokemons, Dictionary<string, int> dzb)
        {
            string teamRes = "";
            var cnt = teamOrderPolicy.ChoosePoke.Sum(s => s.Ratio);
            if (cnt < 1)
            {
                return "123456";
            }
            var choose = Random.Shared.Next(cnt);
            int idx = 0;
            while (choose > 0)
            {
                choose -= teamOrderPolicy.ChoosePoke[idx].Ratio;
                idx++;
            }

            List<int> qs = new();
            teamRes = string.Join("", teamOrderPolicy.ChoosePoke[idx].Res.Select(s => dzb[s]));
            // 先对choose

            // teamorderpolicy 需要先转化为图鉴id
            // 如果对手里有符合条件的 则进入下一轮 否则返回
            List<PSBattlePokemon> ids = new();
            List<PSBattlePokemon> Chooseids = new();
            for (int i = 0; i < teamOrderPolicy.OppPoke.Count; i++)
            {
                ids.Add(new PSBattlePokemon(await PokemonTools
                    .GetPokemonFromPsNameAsync(teamOrderPolicy.OppPoke[i]), teamOrderPolicy.OppPoke[i]));
            }


            bool flag = true;
            if (teamOrderPolicy.Has)
            {
                foreach (var id in ids)
                {
                    if (!pSBattlePokemons.Any(s => s.MetaPokemon.DexId == id.MetaPokemon.DexId))
                    {
                        flag = false;
                        break;
                    }
                }
            }
            else
            {
                foreach (var id in ids)
                {
                    if (pSBattlePokemons.Any(s => s.MetaPokemon.DexId == id.MetaPokemon.DexId))
                    {
                        flag = false;
                        break;
                    }
                }
            }
            if (flag)
            {
                // 符合此次决策，尝试继续深入
                foreach (var item in teamOrderPolicy.TeamOrderPolices)
                {
                    // 如果能得到结果 就直接返回
                    var res = await MakeTeamOrderAsync(item, pSBattlePokemons, dzb);
                    if (res != "") return res;

                }
                // 否则 返回这次决策的结果
                return teamRes;
            }
            else
            {
                // 不符合此次决策
                return "";
                // 随机选择结果返回
            }
            return "123456";
        }
    }
}

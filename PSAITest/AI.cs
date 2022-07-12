using PokeCommon.Utils;
using PokePSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSAITest
{
    public class AI
    {
        //public string MakeTeamOrder(TeamOrderPolicy teamOrderPolicy, int[] pSBattlePokemons)
        //{
        //    return "";
        //}
        public async Task<string> MakeTeamOrderAsync(TeamOrderPolicy teamOrderPolicy, PSBattlePokemon[] pSBattlePokemons, PSBattlePokemon[] mypSBattlePokemons)
        {
            return null;
        }
        public async Task<string> MakeTeamOrderAsync(TeamOrderPolicy teamOrderPolicy, PSBattlePokemon[] pSBattlePokemons, Dictionary<string, int> dzb)
        {
            string teamRes = "";
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


            if (teamOrderPolicy.Has)
            {
                bool flag = true;
                foreach (var id in ids)
                {
                    if (!pSBattlePokemons.Any(s => s.MetaPokemon.DexId == id.MetaPokemon.DexId))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    foreach (var item in teamOrderPolicy.TeamOrderPolices)
                    {
                        var res = await MakeTeamOrderAsync(item, pSBattlePokemons, dzb);
                        if (res != "") return res;

                    }
                    return "";
                }
                else
                {
                    // 随机选择结果返回
                }
            }
            else
            {
                // 返回结果
                return teamOrderPolicy.ChoosePoke[0].Res;
            }
            return "123456";
        }
    }
}

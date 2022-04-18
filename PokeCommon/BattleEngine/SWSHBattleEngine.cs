using PokeCommon.Interface;
using PokeCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.BattleEngine
{
    public class SWSHBattleEngine : BattleEngine
    {
        public SWSHBattleEngine(BattleVersion battleVersion) : base(battleVersion)
        {
        }

        //public SWSHBattle CreateBattle(List<GamePokemon> gamePokemons)
        //{
        //    return new SWSHBattle(this);
        //}


        /// <summary>
        /// 为null则创建失败
        /// </summary>
        /// <param name="gamePokemonTeams"></param>
        /// <returns></returns>
        public override SWSHBattle? CreateBattle(List<GamePokemonTeam> gamePokemonTeams, BattleType battleType)
        {
            //battle.BattlePokemons
            foreach (var team in gamePokemonTeams)
            {
                if (!BattleRule.IsTeamOk(team)) return null;
                //if (battleru)
            }
            var battle = new SWSHBattle(this, battleType);
            return battle;
        }
    }
}

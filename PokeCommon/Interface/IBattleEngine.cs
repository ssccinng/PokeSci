using PokeCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.Interface
{
    public interface IBattleEngine
    {
        /// <summary>
        /// 伤害计算器
        /// </summary>
        public IDamageCalc DamageCalc { get; }
        /// <summary>
        /// 状态计算器
        /// </summary>
        public IStatusCalc StatusCalc { get; }
        /// <summary>
        /// 创建一个比赛
        /// </summary>
        /// <returns></returns>
        public IPokeBattle CreateBattle(List<GamePokemonTeam> gamePokemonTeams);
    }
}

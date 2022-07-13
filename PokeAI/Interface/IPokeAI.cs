using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeCommon.Interface;
using PokeCommon.Models;

namespace PokeAI.Interface
{
    public interface IPokeAI
    {
        /// <summary>
        /// 做出选出
        /// </summary>
        /// <param name="battle"></param>
        /// <returns></returns>
        Task<List<IBattlePokemon>> MakeTeamOrderAsync(IPokeBattle battle);
        /// <summary>
        /// 做出换人
        /// </summary>
        /// <param name="battle"></param>
        /// <returns></returns>
        Task<object> MakeSwitchAsync(IPokeBattle battle);
        /// <summary>
        /// 做出招式选择
        /// </summary>
        /// <param name="battle"></param>
        /// <returns></returns>
        Task<object> MakeMoveAsync(IPokeBattle battle);
        /// <summary>
        /// 做出回合最佳选择
        /// </summary>
        /// <param name="battle"></param>
        /// <returns></returns>
        Task<object> MakeTurnChooseAsync(IPokeBattle battle);
    }
}

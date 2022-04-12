using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeBattleEngine.Interface
{
    internal interface IPokeBattle
    {
        /// <summary>
        /// 回合数
        /// </summary>
        public int Turn { get; }
        public BattleType Type { get; }

    }
}

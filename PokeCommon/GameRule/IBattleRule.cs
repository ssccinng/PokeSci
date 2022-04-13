using PokeCommon.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.GameRule
{
    public interface IBattleRule
    {
        
        /// <summary>
        /// 对战引擎
        /// </summary>
        public IBattleEngine BattleEngine { get; }
        /// <summary>
        /// 队伍成员限制
        /// </summary>
        public int TeamMemberLimit { get; }
        /// <summary>
        /// 上场成员限制
        /// </summary>
        public int BattleMemberLimit { get; }
        /// <summary>
        /// 队伍可见性
        /// </summary>
        public bool TeamVisable { get; }
        /// <summary>
        /// 对战成员可见性
        /// </summary>
        public bool BattleMemberVisable { get; }
    }
}

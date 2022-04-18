using PokeCommon.BattleEngine;
using PokeCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.Interface
{
    //public enum
    public interface IPokeBattle
    {
        /// <summary>
        /// 回合数
        /// </summary>
        public int Turn { get; }
        public BattleType Type { get; }

        /// <summary>
        /// 初始化并启动
        /// </summary>
        /// <returns></returns>
        public bool Init();
        /// <summary>
        /// 运行此轮
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public BattleTurnResult RunCurrentTurn()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 结束对战
        /// </summary>
        /// <returns></returns>
        public bool End();

        // 对战结果，在end后保存

    }
}

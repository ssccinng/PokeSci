using PokeBattleEngine.BattleEngines;
using PokeCommon.Interface;
using PokeCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeBattleEngine
{
    public enum BattleType
    {
        /// <summary>
        /// 单打
        /// </summary>
        Single,
        /// <summary>
        /// 双打
        /// </summary>
        Double,
        /// <summary>
        /// 皇家对战
        /// </summary>
        RoyalBattle,
        /// <summary>
        /// 三打对战
        /// </summary>
        TripleBattle,
        /// <summary>
        /// 轮盘对战
        /// </summary>
        RotationBattle
    }
    public class PokeBattle: IPokeBattle
    {
        protected readonly BattleEngine _battleEngine;

        internal PokeBattle(BattleEngine battleEngine)
        {
            _battleEngine = battleEngine;
        }

        public int Turn { get; protected set; } = 0;
        public BattleType Type { get; }

        public BattlePokemon[,] BattlePokemons { get; protected set; }

        public BattleType BattleType { get; protected set; }

        //public 
    }

    public class SWSHBattle : PokeBattle
    {
        internal SWSHBattle(BattleEngine battleEngine) : base(battleEngine)
        {
        }

        public SWSHBattleEngine SWSHBattleEngine => (_battleEngine as SWSHBattleEngine)!;
    }
}

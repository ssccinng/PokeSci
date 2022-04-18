using PokeCommon.Interface;
using PokeCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.BattleEngine
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
    public class PokeBattle : IPokeBattle
    {
        protected readonly BattleEngine _battleEngine;

        internal PokeBattle(BattleEngine battleEngine, BattleType battleType)
        {
            _battleEngine = battleEngine;
            Type = battleType;
        }

        public int Turn { get; protected set; } = 0;
        public BattleType Type { get; }

        public BattlePokemon[] BattlePokemons { get; protected set; }
        public List<GamePokemonTeam> PlayerTeams { get; set; }

        //public BattleType BattleType { get; protected set; }

        public bool End()
        {
            throw new NotImplementedException();
        }

        public bool Init()
        {
            switch (Type)
            {
                case BattleType.Single:
                    BattlePokemons = new BattlePokemon[4];
                    break;
                case BattleType.Double:
                    BattlePokemons = new BattlePokemon[4];
                    break;
                case BattleType.RoyalBattle:
                    BattlePokemons = new BattlePokemon[4];
                    break;
                case BattleType.TripleBattle:
                    BattlePokemons = new BattlePokemon[6];
                    break;
                case BattleType.RotationBattle:
                    BattlePokemons = new BattlePokemon[6];
                    break;
                default:
                    break;
            }
            throw new NotImplementedException();
        }

        //public 
    }

    public class SWSHBattle : PokeBattle
    {
        internal SWSHBattle(BattleEngine battleEngine, BattleType battleType) : base(battleEngine, battleType)
        {
           //battleType = battleType;
        }

        public SWSHBattleEngine SWSHBattleEngine => (_battleEngine as SWSHBattleEngine)!;
    }
}

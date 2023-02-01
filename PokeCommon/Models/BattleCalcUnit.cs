using PokemonDataAccess.Models;

namespace PokeCommon.Models
{
    /// <summary>
    /// 招式计算单元
    /// </summary>
    public class BattleCalcUnit
    {
        /// <summary>
        /// 攻击方
        /// </summary>
        public BattlePokemon SourcePokemon
        {
            get;
        }
        /// <summary>
        /// 防御方
        /// </summary>
        public BattlePokemon TargetPokemon
        {
            get;
        }
        public Move AtkMove
        {
            get;
        }
    }
}

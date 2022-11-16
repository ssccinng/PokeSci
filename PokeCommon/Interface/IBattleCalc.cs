using PokeCommon.Models;

namespace PokeCommon.Interface
{
    /// <summary>
    /// 对战计算器
    /// </summary>
    public interface IBattleCalc
    {
        /// <summary>
        /// 状态计算器
        /// </summary>
        public IDamageCalc DamageCalc
        {
            get;
        }
        /// <summary>
        /// 能力计算器
        /// </summary>
        public IStatusCalc StatusCalc
        {
            get;
        }

        public BattleCalcResult Calc();
    }
}

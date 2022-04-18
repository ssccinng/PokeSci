using PokeBattleEngine.BattleEngines;
using PokeCommon.GameRule;
using PokeCommon.Interface;
using PokeCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.BattleEngine
{
    public enum BattleVersion
    {
        /// <summary>
        /// 红绿蓝黄
        /// </summary>
        RGBY,
        /// <summary>
        /// 金银水晶
        /// </summary>
        GSC,
        /// <summary>
        /// 红蓝宝石
        /// </summary>
        RS,
        /// <summary>
        /// 火红叶绿
        /// </summary>
        FRLG,
        /// <summary>
        /// 绿宝石
        /// </summary>
        E,
        /// <summary>
        /// 珍钻白金
        /// </summary>
        DPPt,
        /// <summary>
        /// 心金魂银
        /// </summary>
        HGSS,
        /// <summary>
        /// 黑白
        /// </summary>
        BW,
        /// <summary>
        /// 黑2白2
        /// </summary>
        B2W2,
        /// <summary>
        /// XY
        /// </summary>
        XY,
        /// <summary>
        /// 红蓝宝石重制
        /// </summary>
        ORAS,
        /// <summary>
        /// 日月
        /// </summary>
        SM,
        /// <summary>
        /// 究极日月
        /// </summary>
        USUM,
        /// <summary>
        /// Let's go P/E
        /// </summary>
        LGPE,
        /// <summary>
        /// 剑盾
        /// </summary>
        SWSH,
        /// <summary>
        /// 珍珠钻石重制
        /// </summary>
        BDSP,
        /// <summary>
        /// 传阿
        /// </summary>
        LA,
        /// <summary>
        /// 朱紫
        /// </summary>
        SV,
    }
    public abstract class BattleEngine : IBattleEngine
    {
        //private static readonly Dictionary<BattleVersion, BattleEngine> _;

        public BattleVersion BattleVersion { get; init; }
        public string GameName { get; init; }

        public IDamageCalc DamageCalc => throw new NotImplementedException();

        public IStatusCalc StatusCalc => throw new NotImplementedException();
        public IBattleRule BattleRule;

        private string GetGameName(BattleVersion battleVersion)
        {
            return battleVersion switch
            {
                BattleVersion.RGBY => "红绿蓝黄",
                BattleVersion.GSC => "金银水晶",
                BattleVersion.RS => "红/蓝宝石",
                BattleVersion.FRLG => "火红叶绿",
                BattleVersion.E => "绿宝石",
                BattleVersion.DPPt => "珍钻白金",
                BattleVersion.HGSS => "心金魂银",
                BattleVersion.BW => "黑白",
                BattleVersion.B2W2 => "黑2白2",
                BattleVersion.XY => "XY",
                BattleVersion.ORAS => "红蓝宝石重制",
                BattleVersion.SM => "日月",
                BattleVersion.USUM => "究极日月",
                BattleVersion.LGPE => "Let's go P/E",
                BattleVersion.SWSH => "剑盾",
                BattleVersion.BDSP => "珍珠钻石重制",
                BattleVersion.LA => "传阿",
                BattleVersion.SV => "朱紫",
                _ => throw new ArgumentException("未知的版本"),
            };
        }

        public static BattleEngine CreateBattleEngine(BattleVersion battleVersion)
        {
            BattleEngine battleEngine = null;
            switch (battleVersion)
            {
                case BattleVersion.RGBY:
                    break;
                case BattleVersion.GSC:
                    break;
                case BattleVersion.RS:
                    break;
                case BattleVersion.FRLG:
                    break;
                case BattleVersion.E:
                    break;
                case BattleVersion.DPPt:
                    break;
                case BattleVersion.HGSS:
                    break;
                case BattleVersion.BW:
                    break;
                case BattleVersion.B2W2:
                    break;
                case BattleVersion.XY:
                    break;
                case BattleVersion.ORAS:
                    break;
                case BattleVersion.SM:
                    break;
                case BattleVersion.USUM:
                    break;
                case BattleVersion.LGPE:
                    break;
                case BattleVersion.SWSH:
                    battleEngine = new SWSHBattleEngine(BattleVersion.SWSH);
                    break;
                case BattleVersion.BDSP:
                    break;
                case BattleVersion.LA:
                    break;
                case BattleVersion.SV:
                    break;
                default:
                    break;
            }

            throw new NotImplementedException();
        }
        protected BattleEngine(BattleVersion battleVersion)
        {
            BattleVersion = battleVersion;
            GameName = GetGameName(battleVersion);
        }
        public abstract IPokeBattle CreateBattle(List<GamePokemonTeam> gamePokemonTeams, BattleType battleType);

        //IPokeBattle IBattleEngine.CreateBattle(List<GamePokemonTeam> gamePokemonTeams)
        //{
        //    throw new NotImplementedException();
        //}

        //public PokeBattle CreateBattle(List<GamePokemonTeam> gamePokemonTeams)
        //{
        //    throw new NotImplementedException();
        //}
        //public void InitBa
    }
}

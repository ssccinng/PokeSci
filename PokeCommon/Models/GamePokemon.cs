using PokeCommon.GameRule;
using PokemonDataAccess.Models;

namespace PokeCommon.Models
{
    /// <summary>
    /// 游戏中的宝可梦
    /// </summary>
    public class GamePokemon
    {


        public GamePokemon(Pokemon pokemon, EV eV = null, IV iV = null)
        {
            MetaPokemon = pokemon;
            if (eV != null) EVs = eV;
            if (iV != null) IVs = iV;


        }

        // 需要内部获取一个计算接口
        /// <summary>
        /// 宝可梦元数据
        /// </summary>
        public readonly Pokemon MetaPokemon;
        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName
        {
            get; set;
        }
        /// <summary>
        /// 携带道具
        /// </summary>
        public Item Item
        {
            get; set;
        }
        /// <summary>
        /// 是否闪光
        /// </summary>
        public bool Shiny
        {
            get; set;
        }
        /// <summary>
        /// 性格
        /// </summary>
        public Nature Nature
        {
            get; set;
        }
        /// <summary>
        /// 特性
        /// </summary>
        public Ability Ability
        {
            get; set;
        }
        /// <summary>
        /// 是否能超级巨化
        /// </summary>
        public bool Gmax { get; set; } = false;
        /// <summary>
        /// 亲密度
        /// </summary>
        public int Happiness { get; set; } = 160;
        /// <summary>
        /// 招式表
        /// </summary>
        public List<GameMove> Moves { get; set; } = new();
        /// <summary>
        /// 宝可梦等级
        /// </summary>
        public int LV { get; set; } = 50;
        /// <summary>
        /// 努力值
        /// </summary>
        public EV EVs { get; } = new EV(0);
        /// <summary>
        /// 个体值
        /// </summary>
        public IV IVs { get; } = new IV(31);
        /// <summary>
        /// 能力值
        /// </summary>
        public SixDimension Stats
        {
            get; private set;
        }

        public Gender Gender { get; set; } = Gender.Random;
        public void UpdateStats()
        {
            Stats = new SixDimension(0)
            {
                HP = _gameRule.StatusCalc.GetHP(MetaPokemon.BaseHP, IVs.HP, EVs.HP, LV),
                Atk = _gameRule.StatusCalc.GetOtherStat(MetaPokemon.BaseAtk, IVs.Atk, EVs.Atk, LV),
                Def = _gameRule.StatusCalc.GetOtherStat(MetaPokemon.BaseDef, IVs.Def, EVs.Def, LV),
                Spa = _gameRule.StatusCalc.GetOtherStat(MetaPokemon.BaseSpa, IVs.Spa, EVs.Spa, LV),
                Spd = _gameRule.StatusCalc.GetOtherStat(MetaPokemon.BaseSpd, IVs.Spd, EVs.Spd, LV),
                Spe = _gameRule.StatusCalc.GetOtherStat(MetaPokemon.BaseSpe, IVs.Spe, EVs.Spe, LV),
            };
        }
        /// <summary>
        /// 当前血量
        /// </summary>
        public int NowHp
        {
            get; set;
        }
        /// <summary>
        /// 是否gg
        /// </summary>
        public bool IsDead => NowHp <= 0;

        public PokeType? TreaType { get; internal set; }

        /// <summary>
        /// 回到最开始的状态
        /// </summary>
        public void Reset()
        {
            UpdateStats();
            NowHp = Stats.HP;
        }

        // 这个要取决于什么game
        private IGameRule _gameRule;
    }
}

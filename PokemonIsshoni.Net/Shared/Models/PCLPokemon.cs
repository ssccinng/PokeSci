using PokeCommon.Models;

namespace PokemonIsshoni.Net.Shared.Models
{

    public enum Gender
    {
        Female,
        Male,
        None,
    }
    public class PCLPokemon
    {
        public int PokemonId { get; set; } = 730;
        /// <summary>
        /// 昵称
        /// </summary>
        public string? NickName
        {
            get; set;
        }
        /// <summary>
        /// 个体值
        /// </summary>
        //public PokeStatistic IV { get; set; } = new(31);
        public IV IVs { get; set; } = new(31);

        /// <summary>
        /// 努力值
        /// </summary>
        //public PokeStatistic EV { get; set; } = new();
        public EV EVs { get; set; } = new();

        //public Nature Nature;
        /// <summary>
        /// 性格ID
        /// </summary>
        public int? NatureId
        {
            get; set;
        }

        /// <summary>
        /// 特性ID
        /// </summary>
        public int? AbilityId
        {
            get; set;
        }

        /// <summary>
        /// 道具
        /// </summary>
        public int? ItemId
        {
            get; set;
        }

        /// <summary>
        /// 技能列表
        /// </summary>
        public List<int> MoveID { get; set; } = new List<int> { 0, 0, 0, 0 };

        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; } = 50;
        /// <summary>
        /// 亲密度
        /// </summary>
        public int Happiness { get; set; } = 180;
        /// <summary>
        /// 闪光
        /// </summary>
        public bool Shiny { get; set; } = false;

        public Gender Gender { get; set; } = Gender.None;

        public bool Gmax { get; set; } = false;
    }
}

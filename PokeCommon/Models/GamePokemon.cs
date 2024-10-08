﻿using MessagePack;
using PokeCommon.GameRule;
using PokeCommon.Utils;
using PokemonDataAccess.Models;
using System.Text.Json.Serialization;

namespace PokeCommon.Models
{
    /// <summary>
    /// 游戏中的宝可梦
    /// </summary>
    public class GamePokemon
    {
        public static GamePokemon Default() => new GamePokemon(new Pokemon { });
        public GamePokemon(Pokemon pokemon, EV eV = null, IV iV = null)
        {
            MetaPokemon = pokemon;
            if (eV != null) EVs = eV;
            if (iV != null) IVs = iV;


        }

        public GamePokemon()
        {
            
        }


        // 需要内部获取一个计算接口
        /// <summary>
        /// 宝可梦元数据
        /// </summary>
        public Pokemon? MetaPokemon { get; set; }

        public int PokemonId => MetaPokemon?.Id ?? 0;

        /// <summary>
        /// 昵称
        /// </summary>
        public string? NickName
        {
            get; set;
        } = string.Empty;
        /// <summary>
        /// 携带道具
        /// </summary>
        public Item? Item
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
        public Nature? Nature
        {
            get; set;
        }
        /// <summary>
        /// 特性
        /// </summary>
        public Ability? Ability
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
        public EV EVs { get; set; } = new EV(0);
        /// <summary>
        /// 个体值
        /// </summary>
        public IV IVs { get; set; } = new IV(31);

        [JsonIgnore]
        /// <summary>
        /// 能力值
        /// </summary>
        public SixDimension Stats
        {
            get; private set;
        } = new(0);

        public Gender Gender { get; set; } = Gender.Random;
        public void UpdateStats()
        {
            Stats = new SixDimension(0)
            {
                HP = GameRule.StatusCalc.GetHP(MetaPokemon.BaseHP, IVs.HP, EVs.HP, LV),
                Atk = GameRule.StatusCalc.GetOtherStat(MetaPokemon.BaseAtk, IVs.Atk, EVs.Atk, LV),
                Def = GameRule.StatusCalc.GetOtherStat(MetaPokemon.BaseDef, IVs.Def, EVs.Def, LV),
                Spa = GameRule.StatusCalc.GetOtherStat(MetaPokemon.BaseSpa, IVs.Spa, EVs.Spa, LV),
                Spd = GameRule.StatusCalc.GetOtherStat(MetaPokemon.BaseSpd, IVs.Spd, EVs.Spd, LV),
                Spe = GameRule.StatusCalc.GetOtherStat(MetaPokemon.BaseSpe, IVs.Spe, EVs.Spe, LV),
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

        public PokeType? TreaType { get; set; }

        /// <summary>
        /// 回到最开始的状态
        /// </summary>
        public void Reset()
        {
            UpdateStats();
            NowHp = Stats.HP;
        }

        // 这个要取决于什么game
        public IGameRule GameRule { get; set; }

        public SimpleGamePokemon ToSimple()
        {
            return new SimpleGamePokemon
            {
                PokemonId = PokemonId,
                DexId = MetaPokemon?.DexId ?? 0,
                NickName = NickName,
                LV = LV,
                Happiness = Happiness,
                Shiny = Shiny,
                Gmax = Gmax,
                Moves = Moves.Select(x => x.MoveId).ToArray(),
                EVs = EVs.ToSixArray(),
                IVs = IVs.ToSixArray(),
                NowHp = NowHp,
                Item = Item?.ItemId ?? 0,
                Nature = Nature?.NatureId ?? 0,
                Ability = Ability?.AbilityId ?? 0,
                TreaType = TreaType?.Id ?? 0,
            };
        }
    }

    [MessagePackObject]

    public class SimpleGamePokemon
    {
        [Key(0)] public int PokemonId { get; set; }
        [Key(1)] public int DexId { get; set; }
        [Key(2)] public string NickName { get; set; } = string.Empty;
        [Key(3)] public int LV { get; set; } = 50;
        [Key(4)] public int Happiness { get; set; } = 160;
        [Key(5)] public bool Shiny { get; set; }
        [Key(6)] public bool Gmax { get; set; } = false;

        [Key(7)] public int[] Moves { get; set; } = new int[4];
        [Key(8)] public int[] EVs { get; set; } = new int[6];
        [Key(9)] public int[] IVs { get; set; } = new int[6];
        [Key(10)] public int NowHp { get; set; }

        [Key(11)] public int Item { get; set; }
        [Key(12)] public int Nature { get; set; }
        [Key(13)] public int Ability { get; set; }

        [Key(14)] public int TreaType { get; set; }

        public async Task<GamePokemon> ToGamePokemon()
        {
            var pokemon = await PokemonToolsWithoutDB.GetPokemonAsync(PokemonId);
            var gamePokemon = new GamePokemon(pokemon)
            {
                NickName = NickName,
                LV = LV,
                Happiness = Happiness,
                Shiny = Shiny,
                Gmax = Gmax,
                NowHp = NowHp,
                Item = Item == 0 ? null : await PokemonToolsWithoutDB.GetItemAsync(Item),
                Nature = Nature == 0 ? null : await PokemonToolsWithoutDB.GetNatureAsync(Nature),
                Ability = Ability == 0 ? null : await PokemonToolsWithoutDB.GetAbilityAsync(Ability),
                TreaType = TreaType == 0 ? null : await PokemonToolsWithoutDB.GetTypeAsync(TreaType),
            };
            gamePokemon.Moves = (await Task.WhenAll(Moves.Select(async x => new GameMove(await PokemonToolsWithoutDB.GetMoveAsync(x))))).ToList();
            return gamePokemon;
        }
    }
}

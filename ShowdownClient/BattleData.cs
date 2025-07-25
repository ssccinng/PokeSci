
using PokeCommon.Models;
using PokeCommon.Utils;
using PokemonDataAccess.Models;
using Showdown;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using LanguageExt;
using static LanguageExt.Prelude;

namespace Showdown
{
    public interface BoInfo;

    public record SingleBattle(string Tag): BoInfo;
    public record BoBattle(string Tag, string BoTag, string[] BoExp): BoInfo;


    public record GameRule(int MaxChoose, int TeamSize, int BattleSize);

    public record PsChallange(PlayerData Challanger, string Rulename, int BO, ImmutableList<BattleData> BattleDatas);

    //public record BoBattle(string Tag, int BO, ImmutableList<BattleData> BattleDatas);

    public record  BattleData // 如果是bo3, 要继承经验/队伍推测
    {
        public BoInfo BattleInfo { get; init; }

        public bool OpenSheet { get; init; } = false;

        public int MySlot { get; init; } = 1; // 我的槽位
        public string MyName { get; init; } = string.Empty; // 我的名字

        public GameRule Rule { get; init; } = new(4, 6, 2);// 规则

        public ImmutableArray<string> MyOrderTeam { get; init; } = []; // 我选择的宝可梦顺序

        public ImmutableArray<PlayerData> PlayerDatas { get; init; } = [new PlayerData(), new PlayerData()]; // 玩家数据

        public ImmutableList<string> Historys { get; init; } = ImmutableList<string>.Empty; // 可能是对战信息

        // 推测出的对手队伍信息

        /// <summary>
        /// 默认选出4只
        /// </summary>
        public int ChooseSize { get; init; } = 4;
        public string BattleRule { get; init; } = string.Empty; // 规则
        // 对战 但一人队伍是已知的 也可能是两人
        //public GamePokemonTeam Player1Team { get; init; } = new GamePokemonTeam();
        //public GamePokemonTeam Player2Team { get; init; } = new GamePokemonTeam();

        public ImmutableArray<BattleTurnN> BattleTurns { get; init; } = [new()];

        public  bool Win { get; init; } = false; // 是否赢了


    }

    public record PlayerData
    {
        public string PlayerId { get; init; } = string.Empty; // 玩家ID
        public string PlayerName { get; init; } = string.Empty; // 玩家名字
        public Option<GamePokemonTeam> Team { get; init; } = None; // 我的队伍
        public int Score { get; init; } = 0; // 分数

    }

    public record BattleTurnN
    {
        public int Turn { get; init; } = 1;
        public ImmutableArray<BattleTeam> SideTeam { get; set; } = [new (), new()]; // 要维护 分两个side // 暗信息 // 或是除旁观者知道的信息 // 可能还需要有持久化信息
        // 是否信息明牌，

        public ImmutableArray<string> TurnLog = [];
        public BattleTeam MyTeam { get; init; } = new(); // 要维护 只有是对战才有

        public BattleField BattleField { get; init; } = new(); // 要维护

        public ImmutableArray<OneSideBattleField> SideField { get; set; } = [new(), new()]; // 要维护

        public ImmutableList<RequestData> Requests { get; init; } = ImmutableList<RequestData>.Empty; // 请求数据 // 可能是对战信息


    }


    public record BattleTeam
    {
        public bool CanTerastallize { get; init; } = true; // 是否可以太晶化
        public ImmutableArray<BattlePokemon> Pokemons { get; init; } = ImmutableArray<BattlePokemon>.Empty;
    }

    public record MyBattleTeam
    {
        public ImmutableArray<PlayerBattlePokemon> Pokemons { get; init; } = ImmutableArray<PlayerBattlePokemon>.Empty;
        // 可能需要一个队伍信息
        public GamePokemonTeam TeamConfig { get; init; } = new GamePokemonTeam(); // 可能需要一个队伍信息
    }

    // jsontype记得加一下
    public interface PsBattleStatus
    {
        public static UnKnown UnKnown { get; } = new UnKnown();
        public static IsDead IsDead { get; } = new IsDead();
        public static InBackField InBackField { get; } = new InBackField();
        public static InField InField { get; } = new InField();
        public static NotInBattleTeam NotInBattleTeam { get; } = new NotInBattleTeam();
    }
    public static class BoInfoExtensions
    {
        public static string GetTag(this BoInfo boInfo)
        {
            return boInfo switch
            {
                SingleBattle singleBattle => singleBattle.Tag,
                BoBattle boBattle => boBattle.BoTag,
                _ => throw new NotSupportedException("Unsupported BoInfo type")
            };
        }
    }
    public static class PsBattleStatusExtensions
    {


        public static string GetStatusPrompt(this PsBattleStatus status)
        {
            return status switch
            {
                UnKnown => "Possibly in backfield",
                IsDead => "Fainted",
                InBackField => "In backfield",
                InField => "On the field",
                NotInBattleTeam => "Not in battle team",
            };
        }

        public static string GetStatusPrompt(this TeratallizeStatus status)
        {
            return status switch
            {
                TeraStallized teraSallized => $"Terastallized, type: {teraSallized.Type.Name_Eng}",
                NotTeraStallized => "Not terastallized",
            };
        }
    }

    public record UnKnown: PsBattleStatus; // 未知状态
    public record IsDead: PsBattleStatus; // 死亡状态
    public record InBackField: PsBattleStatus; // 在战斗队伍中
    public record InField: PsBattleStatus; // 在场地上
    public record NotInBattleTeam: PsBattleStatus; // 不在战斗队伍中


    public interface TeratallizeStatus
    {
        public static NotTeraStallized NotTeraStallized { get; } = new NotTeraStallized();
    }

    public record TeraStallized(PokeType Type) : TeratallizeStatus; // 已经太晶化
    public record NotTeraStallized : TeratallizeStatus; // 未太晶化


    public record BattlePokemon
    {
        public PokemonStatus Status { get; init; } = new();
        public GamePokemon Pokemon { get; init; } // 宝可梦元数据 思考在不在这
        public int HpRemain { get; init; } = 100; // 生命值剩余百分比
        public int Position { get; init; } = -1; // 位置 0-2
        public string PsName { get; init; } = string.Empty; // 可能是PS的名字
        public TeratallizeStatus TeratallizeStatus { get; init; } = TeratallizeStatus.NotTeraStallized; // 太晶化状态
        public ImmutableArray<PokeCommon.Models.GameMove> Moves { get; init; } = ImmutableArray<PokeCommon.Models.GameMove>.Empty; // 可能是PS的名字
        public Option<Ability> Ability { get; init; } = None;
        public Option<Item> Item { get; init; } = Some(new Item());

        public PsBattleStatus BattleStatus { get; init; } = new UnKnown(); // 是否在战斗中




        public static async Task<BattlePokemon> CreateAsync(string psname)
        {
            var poke = await PokemonToolsWithoutDB.GetPokemonFromPsNameAsync(psname);
            return new BattlePokemon
            {
                Pokemon = new GamePokemon(poke),
                PsName = psname,
                HpRemain = 100, // 默认100%
                Position = -1, // 默认位置0
            };


        }
    }

    public record PlayerBattlePokemon
    {
        public GamePokemon Pokemon { get; init; } = new GamePokemon(); // 宝可梦元数据 思考在不在这
        public PokemonStatus Status { get; init; } = new();
        public int HpRemainRaw { get; init; } = 100; // 生命值剩余百分比
        public int Position { get; init; } = 0; // 位置 0-2
    }

    public record PokemonStatus
    {
        //private int protect;

        // 生成一个将自己成员依据SingleTurn特性修改自己的值的方法
        // 接口一下
        //public void NextTurn()
        //{
        //    foreach (var property in GetType().GetProperties())
        //    {
        //        var decrease = property.GetCustomAttribute<DecreaseAttribute>();
        //        if (decrease != null)
        //        {
        //            var value = (int)property.GetValue(this);
        //            if (value > 0)
        //                value = Math.Max(value + decrease.DeltaValue, 0);
        //            property.SetValue(this, value);
        //        }
        //        //var changeRefresh = property.GetCustomAttribute<ChangeRefreshAttribute>();
        //        //if (changeRefresh != null)
        //        //{
        //        //    property.SetValue(this, 0);
        //        //}

        //        // 生成一个将自己成员依据SingleTurn特性修改自己的值的方法

        //        var singleTurn = property.GetCustomAttribute<SingleTurnAttribute>();
        //        if (singleTurn != null)
        //        {
        //            property.SetValue(this, 0);
        //        }
        //    }
        //}
        //[Decrease(initValue: 1, decreaseValue: -1)]

        //public int Rounds_InField { get; set; } = 0; // 在场地上回合数

        // 利用反射 将所有成员映射到一个数组中
        #region  能力变化
        [ChangeRefresh]
        [Decrease(0, 0, 6, -6)]
        public int AtkBuff { get; set; }
        [ChangeRefresh]
        [Decrease(0, 0, 6, -6)]
        public int DefBuff { get; set; }
        [ChangeRefresh]
        [Decrease(0, 0, 6, -6)]
        public int SpaBuff { get; set; }
        [ChangeRefresh]
        [Decrease(0, 0, 6, -6)]
        public int SpdBuff { get; set; }
        [ChangeRefresh]
        [Decrease(0, 0, 6, -6)]
        public int SpeBuff { get; set; }

        [ChangeRefresh]
        [Decrease(0, 0, 6, -6)]
        public int AccBuff { get; set; }
        [ChangeRefresh]
        [Decrease(0, 0, 6, -6)]
        public int EvaBuff { get; set; }
        #endregion


        #region  能力变化
        [ChangeRefresh]

        public int protosynthesisatk { get; set; }
        [ChangeRefresh]
        public int protosynthesisdef { get; set; }
        [ChangeRefresh]

        public int protosynthesisspa { get; set; }
        [ChangeRefresh]

        public int protosynthesisspd { get; set; }
        [ChangeRefresh]

        public int protosynthesisspe { get; set; }

        #endregion

        #region 异常状态
        /// <summary>
        /// 烧伤
        /// </summary>
        public int Brn { get; set; }
        /// <summary>
        /// 睡觉
        /// </summary>
        [Decrease(3, 0, 3, 0)]
        public int Slp { get; set; }
        /// <summary>
        /// 冰冻
        /// </summary>
        public int Frz { get; set; }
        /// <summary>
        /// 麻痹
        /// </summary>
        public int Par { get; set; }
        /// <summary>
        /// 中毒
        /// </summary>
        public int Psn { get; set; }
        /// <summary>
        /// 剧毒 要回合++
        /// </summary>
        [Decrease(0, 1, 15, 0)]

        public int Tox { get; set; }
        #endregion

        #region 向对手施加的状态变化



        /// <summary>
        /// 混乱
        /// </summary>
        [ChangeRefresh]
        public int Confusion { get; set; }
        /// <summary>
        /// 着迷
        /// </summary>
        [ChangeRefresh]
        public int Infatuation { get; set; }
        /// <summary>
        /// 中毒剧毒需撕烤
        /// </summary>
        public int Poison { get; set; }
        /// <summary>
        /// 恶梦
        /// </summary>
        [ChangeRefresh]
        public int Nightmare { get; set; }
        /// <summary>
        /// 瞌睡
        /// </summary>
        [ChangeRefresh]
        public int Drowsy { get; set; }

        /// <summary>
        /// 再来一次
        /// </summary>
        [Decrease(initValue:3)]
        public int Encore { get; set; }
        /// <summary>
        /// 无特性
        /// </summary>
        [ChangeRefresh]
        public int NoAbility { get; set; }
        /// <summary>
        /// 无理取闹
        /// </summary>
        [ChangeRefresh]
        public int Torment { get; set; }
        /// <summary>
        /// 回复封锁
        /// </summary>
        [ChangeRefresh]
        public int HealBlock { get; set; }
        /// <summary>
        /// 被识破
        /// </summary>
        [ChangeRefresh]
        public int Identified { get; set; }
        /// <summary>
        /// 定身法
        /// </summary>
        [ChangeRefresh]
        [Decrease(initValue:3)]
        public int Disable { get; set; }
        /// <summary>
        /// 无法逃走
        /// </summary>
        [ChangeRefresh]
        public int CantEscape { get; set; }
        /// <summary>
        /// 锁定
        /// </summary>
        [ChangeRefresh]
        public int LockOn { get; set; }
        /// <summary>
        /// 查封
        /// </summary>
        [ChangeRefresh]
        public int Embargo { get; set; }
        /// <summary>
        /// 挑衅
        /// </summary>
        [ChangeRefresh]
        [Decrease(initValue:3)]
        public int Taunt { get; set; }
        /// <summary>
        /// 意念移物
        /// </summary>
        [ChangeRefresh]
        public int Telekinesis { get; set; }
        /// <summary>
        /// 诅咒
        /// </summary>
        [ChangeRefresh]
        public int Curse { get; set; }
        /// <summary>
        /// 万圣夜
        /// </summary>
        [ChangeRefresh]
        public int TrickorTreat { get; set; }
        /// <summary>
        /// 灭亡之歌
        /// </summary>
        [ChangeRefresh]
        [Decrease(initValue:3)]
        public int PerishSong { get; set; }
        /// <summary>
        /// 森林诅咒
        /// </summary>
        [ChangeRefresh]
        public int ForestsCurse { get; set; }
        /// <summary>
        /// 寄生种子
        /// </summary>
        [ChangeRefresh]
        public int LeechSeed { get; set; }
        /// <summary>
        /// 束缚
        /// </summary>
        [ChangeRefresh]
        public int Bound { get; set; }
        /// <summary>
        /// 未来攻击
        /// </summary>
        public int FutureAttack { get; set; }
        /// <summary>
        /// 击落
        /// </summary>
        [ChangeRefresh]
        public int SmackDown { get; set; }
        /// <summary>
        /// 地狱突刺（这个是不是要自己分析）
        /// </summary>
        [ChangeRefresh]
        [Decrease(initValue: 2)]
        public int ThroatChop { get; set; }
        /// <summary>
        /// 沥青射击
        /// </summary>
        [ChangeRefresh]
        public int TarShot { get; set; }
        /// <summary>
        /// 蛸固
        /// </summary>
        [ChangeRefresh]
        public int Octolock { get; set; }
        /// <summary>
        /// 碎片
        /// </summary>
        [ChangeRefresh]
        public int Splinters { get; set; }
        /// <summary>
        /// 盐腌
        /// </summary>
        [ChangeRefresh]
        public int SaltCure { get; set; }
        /// <summary>
        /// 引火
        /// </summary>
        public int FlashFire { get; set; }
        #endregion

        #region 向己方施加的状态变化
        /// <summary>
        /// 易中要害	击中对手要害的几率变高。
        /// </summary>
        [ChangeRefresh]
        public int CriticalHitsUP { get; set; }
        /// <summary>
        /// 怨念	因对手的招式而濒死时，该招式的ＰＰ会变为０。
        /// </summary>
        public int Grudge { get; set; }
        /// <summary>
        /// 充电	下次使出的电属性的招式威力会变为２倍。
        /// </summary>
        [ChangeRefresh]
        public int Charge { get; set; }
        /// <summary>
        /// 蓄力	在蓄力的时候，防御和特防会提高。
        /// </summary>
        [ChangeRefresh]
        // 蓄力1-3阶段
        public int Stockpile { get; set; }
        /// <summary>
        /// 电磁飘浮	在５回合内，地面属性的招式会变得无法击中。
        /// </summary>
        [ChangeRefresh]
        [Decrease(initValue: 5, decreaseValue: -1)]
        public int MagnetRise { get; set; }
        /// <summary>
        /// 扎根	在每回合结束时回复ＨＰ。扎根的宝可梦无法替换。
        /// </summary>
        [ChangeRefresh]
        public int Ingrain { get; set; }
        /// <summary>
        /// 封印	宝可梦使出封印后，其所学会的招式，对手的宝可梦将无法使出。
        /// </summary>
        [ChangeRefresh]
        public int Imprison { get; set; }
        /// <summary>
        /// 同命	让自己陷入濒死的对手，也会一同陷入濒死。
        /// </summary>
        [ChangeRefresh]
        [CancleWhenNextMove]
        public int DestinyBond { get; set; }
        /// <summary>
        /// 忍耐	在２回合内忍受攻击，受到的伤害会２倍返还给对手。
        /// </summary>
        [ChangeRefresh]
        public int Bide { get; set; }
        /// <summary>
        /// 逆鳞	在２～３回合内，乱打一气地进行攻击。大闹一番后自己会陷入混乱。
        /// </summary>
        [ChangeRefresh]
        public int Outrage { get; set; }
        /// <summary>
        /// 大闹一番	因处于大闹状态，在２～３回合内，乱打一气地进行攻击。大闹一番后自己会陷入混乱。
        /// </summary>
        [ChangeRefresh]
        public int Thrash { get; set; }
        /// <summary>
        /// 花瓣舞	在２～３回合内，乱打一气地进行攻击。大闹一番后自己会陷入混乱。
        /// </summary>
        [ChangeRefresh]
        public int PetalDance { get; set; }
        /// <summary>
        /// 大愤慨	在２～３回合内，乱打一气地进行攻击。大闹一番后自己会陷入混乱。
        /// </summary>
        [ChangeRefresh]
        public int RagingFury { get; set; }
        /// <summary>
        /// 水流环	在自己身体的周围覆盖用水制造的幕。每回合回复ＨＰ。
        /// </summary>
        [ChangeRefresh]
        public int AquaRing { get; set; }
        /// <summary>
        /// 幸运咒语 变得不会被对手的招式击中要害。
        /// </summary>
        [ChangeRefresh]
        public int LuckyChant { get; set; }
        /// <summary>
        /// 身体轻量化 速度会大幅度提高，体重也会变轻。
        /// </summary>
        [ChangeRefresh]
        public int Autotomize { get; set; }
        /// <summary>
        /// 磨砺 在使用磨砺的下一回合，招式必定会击中要害。
        /// </summary>
        [ChangeRefresh]
        public int LaserFocus { get; set; }

        // 仅限阿尔宙斯

        /// <summary>
        /// 热衷	会热衷于最近一次使用的招式。所热衷的招式造成的伤害会增加。同时自己受到的伤害也会增加。
        /// </summary>
        public int Fixated { get; set; }
        /// <summary>
        /// 烟幕	让自己被烟雾包裹，对手的招式将变得不易命中。
        /// </summary>
        public int Obscured { get; set; }
        /// <summary>
        /// 加倍	摆起强而有力的架势增加造成的伤害。
        /// </summary>
        public int Primed { get; set; }
        /// <summary>
        /// 攻守转换
        /// </summary>
        public int StanceSwap { get; set; }
        /// <summary>
        /// 慢启动
        /// </summary>
        public int SlowStart { get; set; }
        /// <summary>
        /// 狂猛之力
        /// </summary>
        public int FrenziedMight { get; set; }
        /// <summary>
        /// 伟大之力
        /// </summary>
        public int TerrificMight { get; set; }
        /// <summary>
        /// 野性之力
        /// </summary>
        public int WildMight { get; set; }
        /// <summary>
        /// 惊骇之力
        /// </summary>
        [ChangeRefresh]
        public int TerribleMight { get; set; }
        /// <summary>
        /// 替身 可以防住攻击，受到一定的伤害后就会消失。
        /// </summary>
        [ChangeRefresh]
        public int Substitute { get; set; }
        /// <summary>
        /// 变小 身体变小，被特定招式攻击时受到的伤害会变为２倍。
        /// </summary>
        [ChangeRefresh]
        public int Minimize { get; set; }
        /// <summary>
        /// 变圆 身体变圆，特定招式的威力会变为２倍
        /// </summary>
        [ChangeRefresh]
        public int DefenseCurl { get; set; }
        /// <summary>
        /// 愤怒 受到攻击时，会因愤怒的力量而提高攻击。
        /// </summary>
        [ChangeRefresh]
        public int Rage { get; set; }
        /// <summary>
        /// 魔法反射 可能不用
        /// </summary>
        [ChangeRefresh]
        public int MagicCoat { get; set; }
        /// <summary>
        /// 力量戏法 攻击和防御互换。
        /// </summary>
        [ChangeRefresh]
        public int PowerTrick { get; set; }
        /// <summary>
        /// 变身
        /// </summary>
        [ChangeRefresh]
        public int Transform { get; set; }
        /// <summary>
        /// 飞翔 了，对方的大部分招式不会命中自己。下一回合将进行攻击。
        /// </summary>
        [ChangeRefresh]
        public int Fly { get; set; }
        /// <summary>
        /// 挖洞 了，对方的大部分招式不会命中自己。下一回合将进行攻击。
        /// </summary>
        [ChangeRefresh]
        public int Dig { get; set; }
        /// <summary>
        /// 潜水 了，对方的大部分招式不会命中自己。下一回合将进行攻击。
        /// </summary>
        [ChangeRefresh]
        public int Dive { get; set; }
        /// <summary>
        /// 暗影潜袭 了，对方的大部分招式不会命中自己。下一回合将进行攻击。
        /// </summary>
        [ChangeRefresh]
        public int ShadowForce { get; set; }
        /// <summary>
        /// 吃饱 宝可梦吃掉了树果，可以使出打嗝
        /// </summary>
        public int Chibao { get; set; }

        #endregion
        /// <summary>
        /// 帮助
        /// </summary>
        [SingleTurn]
        public int HelpingHand { get; set; }
        /// <summary>
        /// 保护
        /// </summary>
        [SingleTurn]
        public int Protect
        {
            get => field; set
            {
                field = value;
                if (value != 0)
                {
                    ProtectCnt = 2;
                }
            }
        }
        [Decrease(initValue: 1)]
        public int ProtectCnt { get; set; }
        [SingleTurn]
        public int WideGuard {
            get => field; set
            {
                field = value;
                if (value != 0)
                {
                    ProtectCnt = 2;
                }
            }
        }
        public int QuickGuard { get; set; }
        [SingleTurn]
        public int Roost { get; set; }
        /// <summary>
        /// 万众瞩目
        /// </summary>
        [SingleTurn]
        public int CenterofAttention { get; set; }
        /// <summary>
        /// 粉末万众瞩目
        /// </summary>
        [SingleTurn]
        public int RagePowder { get; set; }

        [Decrease(initValue: 1, decreaseValue: -1)]
        public int InField_First_Turn { get; set; }  
    }

    public record BattleField
    {
        /// <summary>
        /// 戏法还剩几回合
        /// </summary>
        [Decrease(initValue: 5, maxValue: 5)]
        public int TrickRoom { get; set; }

        /// <summary>
        /// 奇妙空间
        /// </summary>
        [Decrease(initValue: 5, maxValue: 5)]
        public int WonderRoom { get; set; }

        /// <summary>
        /// 魔法空间
        /// </summary>
        [Decrease(initValue: 5, maxValue: 5)]
        public int MagicRoom { get; set; }

        /// <summary>
        /// 重力剩余
        /// </summary>
        [Decrease(initValue: 5, maxValue: 5)]
        public int Gravity { get; set; } = 0;

        /// <summary>
        /// 天气
        /// </summary>

        public Weather Weather { get; set; } // set none时需要清除WeatherRemain
        [Decrease(initValue: 5, maxValue: 8)]
        public int WeatherRemain { get; set; }

        /// <summary>
        /// 场地
        /// </summary>
        public Terrain Terrain { get; set; }
        [Decrease(initValue: 5, maxValue: 8)]
        public int TerrainRemain { get; set; }

        /// <summary>
        /// 玩泥巴
        /// </summary>
        [Decrease(initValue: 5, maxValue: 5)]
        public int MudSport { get; set; }

        /// <summary>
        /// 玩水
        /// </summary>
        [Decrease(initValue: 5, maxValue: 5)]
        public int WaterSport { get; set; }

        /// <summary>
        /// 吵闹
        /// </summary>
        [Decrease(initValue: 5, maxValue: 5)]
        public int Uproar { get; set; }
    }


    public record OneSideBattleField
    {
        // 生成一个将自己成员依据Decrease特性修改自己的值的方法

        //public void NextTurn()
        //{
        //    foreach (var property in GetType().GetProperties())
        //    {
        //        var decrease = property.GetCustomAttribute<DecreaseAttribute>();
        //        if (decrease != null)
        //        {
        //            var value = (int)property.GetValue(this);
        //            if (value > 0)
        //                value = Math.Max(value + decrease.DeltaValue, 0);
        //            property.SetValue(this, value);
        //        }
        //        //var changeRefresh = property.GetCustomAttribute<ChangeRefreshAttribute>();
        //        //if (changeRefresh != null)
        //        //{
        //        //    property.SetValue(this, 0);
        //        //}

        //        // 生成一个将自己成员依据SingleTurn特性修改自己的值的方法

        //        var singleTurn = property.GetCustomAttribute<SingleTurnAttribute>();
        //        if (singleTurn != null)
        //        {
        //            property.SetValue(this, 0);
        //        }
        //    }
        //}

        /// <summary>
        /// 撒钉层数
        /// </summary>
        [Decrease(initValue: 1, decreaseValue: 0, maxValue: 3)]
        public int Spikes { get; set; }
        /// <summary>
        /// 毒钉层数
        /// </summary>
        [Decrease(initValue: 1, decreaseValue: 0, maxValue: 2)]
        public int ToxicSpikes { get; set; }
        /// <summary>
        /// 岩钉层数
        /// </summary>
        [Decrease(initValue: 1, decreaseValue: 0, maxValue: 1)]
        public int StealthRock { get; set; }
        /// <summary>
        /// 是否有黏黏网
        /// </summary>
        [Decrease(initValue: 1, decreaseValue: 0)]
        public int StickyWeb { get; set; }
        /// <summary>
        /// 湿地
        /// </summary>
        [Decrease(initValue: 4)]
        public int Swamp { get; set; }
        /// <summary>
        /// 火海
        /// </summary>
        [Decrease(initValue: 4)]
        public int Seaoffire { get; set; }
        /// <summary>
        /// 超级巨地狱灭焰
        /// </summary>
        [Decrease(initValue: 4)]
        public int GMaxWildfire { get; set; }
        /// <summary>
        /// 超级草
        /// </summary>
        [Decrease(initValue: 4)]
        public int GMaxVineLash { get; set; }
        /// <summary>
        /// 超级巨水炮
        /// </summary>
        [Decrease(initValue: 4)]
        public int GMaxCannonade { get; set; }
        /// <summary>
        /// 超级岩
        /// </summary>
        [Decrease(initValue: 4)]
        public int GMaVolcalith { get; set; }
        /// <summary>
        /// 超级钢
        /// </summary>
        [Decrease(initValue: 1, decreaseValue: 0)]
        public int GMaxSteelsurge { get; set; }

        /// <summary>
        /// 白雾剩余回合
        /// </summary>
        [Decrease(initValue: 5)]
        public int Mist { get; set; }
        /// <summary>
        /// 神秘守护剩余回合
        /// </summary>
        [Decrease(initValue: 5)]
        public int Safeguard { get; set; }
        /// <summary>
        /// 光强
        /// </summary>
        [Decrease(initValue: 5, maxValue: 8)]
        public int LightScreen { get; set; }
        /// <summary>
        /// 反射壁
        /// </summary>
        [Decrease(initValue: 5, maxValue: 8)]
        public int Reflect { get; set; }
        /// <summary>
        /// 祈愿还有几回合生效
        /// </summary>
        [Decrease(initValue: 3)]
        public int Wish { get; set; }
        /// <summary>
        /// 顺风
        /// </summary>
        [Decrease(initValue: 4)]
        public int Tailwind { get; set; }
        /// <summary>
        /// 彩虹
        /// </summary>
        [Decrease(initValue: 4)]
        public int RainBow { get; set; }
        /// <summary>
        /// 激光木
        /// </summary>
        [Decrease(initValue: 5, maxValue: 8)]
        public int AuroraVeil { get; set; }
    }


}

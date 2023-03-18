﻿namespace PSReplayAnalysis;

/// <summary>
/// 对战单回合数据
/// </summary>
public class BattleTurn
{
    /// <summary>
    /// 回合数
    /// </summary>
    public int TurnId { get; set; }

    /// <summary>
    /// 本轮战斗是否结束
    /// </summary>
    public bool BattleEnd { get; set; }

    /// <summary>
    /// 全体状态
    /// </summary>
    public BattleField AllField { get; set; } = new();
    /// <summary>
    /// 玩家1场地状态
    /// </summary>
    public OneSideBattleField Side1Field { get; set; } = new();
    /// <summary>
    /// 玩家2场地状态
    /// </summary>
    public OneSideBattleField Side2Field { get; set; } = new();
    
    

    public BattleTurn NextTurn()
    {
        BattleTurn next = new();
        next.TurnId++;
        
        

        return next;
    }

}

public class BattleEvent
{
    
}
// 要考虑pp变化
public class PokemonStatus
{
    #region  能力变化

    public int AtkBuff { get; set; }
    public int DefBuff { get; set; }
    public int SpaBuff { get; set; }
    public int SpdBuff { get; set; }
    public int SpeBuff { get; set; }
    
    public int AccBuff { get; set; }
    public int EvaBuff { get; set; }
    #endregion

    #region 向对手施加的状态变化


    
    /// <summary>
    /// 混乱
    /// </summary>
    public int Confusion { get; set; }
    /// <summary>
    /// 着迷
    /// </summary>
    public int Infatuation { get; set; }
    /// <summary>
    /// 中毒剧毒需撕烤
    /// </summary>
    public int Poison { get; set; }
    /// <summary>
    /// 恶梦
    /// </summary>
    public int Nightmare { get; set; }
    /// <summary>
    /// 瞌睡
    /// </summary>
    public int Drowsy { get; set; }
    /// <summary>
    /// 再来一次
    /// </summary>
    public int Encore { get; set; }
    /// <summary>
    /// 无特性
    /// </summary>
    public int NoAbility { get; set; }
    /// <summary>
    /// 无理取闹
    /// </summary>
    public int Torment { get; set; }
    /// <summary>
    /// 回复封锁
    /// </summary>
    public int HealBlock { get; set; }
    /// <summary>
    /// 被识破
    /// </summary>
    public int Identified { get; set; }
    /// <summary>
    /// 定身法
    /// </summary>
    public int Disable { get; set; }
    /// <summary>
    /// 无法逃走
    /// </summary>
    public int CantEscape { get; set; }
    /// <summary>
    /// 锁定
    /// </summary>
    public int LockOn { get; set; }
    /// <summary>
    /// 查封
    /// </summary>
    public int Embargo { get; set; }
    /// <summary>
    /// 挑衅
    /// </summary>
    public int Taunt { get; set; }
    /// <summary>
    /// 意念移物
    /// </summary>
    public int Telekinesis { get; set; }
    /// <summary>
    /// 诅咒
    /// </summary>
    public int Curse { get; set; }
    /// <summary>
    /// 万圣夜
    /// </summary>
    public int TrickorTreat { get; set; }
    /// <summary>
    /// 灭亡之歌
    /// </summary>
    public int PerishSong { get; set; }
    /// <summary>
    /// 森林诅咒
    /// </summary>
    public int ForestsCurse { get; set; }
    /// <summary>
    /// 寄生种子
    /// </summary>
    public int LeechSeed { get; set; }
    /// <summary>
    /// 舒服
    /// </summary>
    public int Bound { get; set; }
    /// <summary>
    /// 未来攻击
    /// </summary>
    public int FutureAttack { get; set; }
    /// <summary>
    /// 击落
    /// </summary>
    public int SmackDown { get; set; }
    /// <summary>
    /// 地狱突刺（这个是不是要自己分析）
    /// </summary>
    public int ThroatChop { get; set; }
    /// <summary>
    /// 沥青射击
    /// </summary>
    public int TarShot { get; set; }
    /// <summary>
    /// 蛸固
    /// </summary>
    public int Octolock { get; set; }
    /// <summary>
    /// 碎片
    /// </summary>
    public int Splinters { get; set; }
    /// <summary>
    /// 盐腌
    /// </summary>
    public int SaltCure { get; set; }
    #endregion

    #region 向己方施加的状态变化
/// <summary>
/// 易中要害	击中对手要害的几率变高。
/// </summary>
    public int CriticalHitsUP { get; set; }
/// <summary>
/// 怨念	因对手的招式而濒死时，该招式的ＰＰ会变为０。
/// </summary>
    public int Grudge { get; set; }
/// <summary>
/// 充电	下次使出的电属性的招式威力会变为２倍。
/// </summary>
    public int Charge { get; set; }
/// <summary>
/// 蓄力	在蓄力的时候，防御和特防会提高。
/// </summary>
    public int Stockpile { get; set; }
/// <summary>
/// 电磁飘浮	在５回合内，地面属性的招式会变得无法击中。
/// </summary>
    public int MagnetRise { get; set; }
/// <summary>
/// 扎根	在每回合结束时回复ＨＰ。扎根的宝可梦无法替换。
/// </summary>
    public int Ingrain { get; set; }
/// <summary>
/// 封印	宝可梦使出封印后，其所学会的招式，对手的宝可梦将无法使出。
/// </summary>
    public int Imprison { get; set; }
/// <summary>
/// 同命	让自己陷入濒死的对手，也会一同陷入濒死。
/// </summary>
    public int DestinyBond { get; set; }
/// <summary>
/// 忍耐	在２回合内忍受攻击，受到的伤害会２倍返还给对手。
/// </summary>
    public int Bide { get; set; }
/// <summary>
/// 逆鳞	在２～３回合内，乱打一气地进行攻击。大闹一番后自己会陷入混乱。
/// </summary>
    public int Outrage { get; set; }
/// <summary>
/// 大闹一番	因处于大闹状态，在２～３回合内，乱打一气地进行攻击。大闹一番后自己会陷入混乱。
/// </summary>
    public int Thrash { get; set; }
/// <summary>
/// 花瓣舞	在２～３回合内，乱打一气地进行攻击。大闹一番后自己会陷入混乱。
/// </summary>
    public int PetalDance { get; set; }
/// <summary>
/// 大愤慨	在２～３回合内，乱打一气地进行攻击。大闹一番后自己会陷入混乱。
/// </summary>
    public int RagingFury { get; set; }
/// <summary>
/// 水流环	在自己身体的周围覆盖用水制造的幕。每回合回复ＨＰ。
/// </summary>
    public int AquaRing { get; set; }
/// <summary>
/// 幸运咒语 变得不会被对手的招式击中要害。
/// </summary>
    public int LuckyChant { get; set; }
/// <summary>
/// 身体轻量化 速度会大幅度提高，体重也会变轻。
/// </summary>
    public int Autotomize { get; set; }
/// <summary>
/// 磨砺 在使用磨砺的下一回合，招式必定会击中要害。
/// </summary>
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
/// 
/// </summary>
    public int StanceSwap { get; set; }
    
    public int SlowStart { get; set; }
    public int FrenziedMight { get; set; }
    public int TerrificMight { get; set; }
    public int WildMight { get; set; }
    public int TerribleMight { get; set; }
    
    public int Substitute { get; set; }
    public int Minimize { get; set; }
    public int DefenseCurl { get; set; }
    public int Rage { get; set; }
    /// <summary>
    /// 魔法反射 可能不用
    /// </summary>
    public int MagicCoat { get; set; }
    public int PowerTrick { get; set; }
    public int Transform { get; set; }
    public int Fly { get; set; }
    public int Dig { get; set; }
    public int Dive { get; set; }
    public int ShadowForce { get; set; }
    public int Chibao { get; set; }

    #endregion
    
}


/// <summary>
/// 单边场地状态 考虑struct
/// </summary>
public class OneSideBattleField
{
    /// <summary>
    /// 撒钉层数
    /// </summary>
    public int Spikes { get; set; }
    /// <summary>
    /// 毒钉层数
    /// </summary>
    public int ToxicSpikes { get; set; }
    /// <summary>
    /// 岩钉层数
    /// </summary>
    public int StealthRock { get; set; }
    /// <summary>
    /// 是否有黏黏网
    /// </summary>
    public bool StickyWeb { get; set; }
    /// <summary>
    /// 湿地
    /// </summary>
    public int Swamp { get; set; }
    /// <summary>
    /// 火海
    /// </summary>
    public int Seaoffire { get; set; }
    /// <summary>
    /// 超级巨地狱灭焰
    /// </summary>
    public int GMaxWildfire { get; set; }
    /// <summary>
    /// 超级草
    /// </summary>
    public int GMaxVineLash { get; set; }
    /// <summary>
    /// 超级巨水炮
    /// </summary>
    public int GMaxCannonade { get; set; }
    /// <summary>
    /// 超级岩
    /// </summary>
    public int GMaVolcalith { get; set; }
    /// <summary>
    /// 超级钢
    /// </summary>
    public bool GMaxSteelsurge { get; set; }

    /// <summary>
    /// 白雾剩余回合
    /// </summary>
    public int Mist { get; set; }
    /// <summary>
    /// 神秘守护剩余回合
    /// </summary>
    public int Safeguard { get; set; }
    /// <summary>
    /// 光强
    /// </summary>
    public int LightScreen { get; set; }
    /// <summary>
    /// 反射壁
    /// </summary>
    public int Reflect { get; set; }
    /// <summary>
    /// 祈愿还有几回合生效
    /// </summary>
    public int Wish { get; set; }
    /// <summary>
    /// 顺风
    /// </summary>
    public int TailWind { get; set; }
    /// <summary>
    /// 彩虹
    /// </summary>
    public int RainBow { get; set; }
    /// <summary>
    /// 激光木
    /// </summary>
    public int AuroraVeil { get; set; }



}



/// <summary>
/// 全体状态变化
/// </summary>
public class BattleField
{
    /// <summary>
    /// 戏法还剩几回合
    /// </summary>
    public int TrickRoomRemain { get; set; }

    /// <summary>
    /// 奇妙空间
    /// </summary>
    public int WonderRoomRemain { get; set; }

    /// <summary>
    /// 魔法空间
    /// </summary>
    public int MagicRoomRemain { get; set; }

    /// <summary>
    /// 重力剩余
    /// </summary>
    public int GravityRemain { get; set; }

    /// <summary>
    /// 天气
    /// </summary>
    public Weather Weather { get; set; }

    /// <summary>
    /// 场地
    /// </summary>
    public Terrain Terrain { get; set; }

    /// <summary>
    /// 玩泥巴
    /// </summary>
    public int MudSportRemain { get; set; }

    /// <summary>
    /// 玩水
    /// </summary>
    public int WaterSportRemain { get; set; }

    /// <summary>
    /// 吵闹
    /// </summary>
    public int UproarRemain { get; set; }
}

public enum Terrain
{
    None,
    Electric,
    Grass,
    Psychic,
    Misty,
}

public enum Weather
{
    None,

    /// <summary>
    /// 雨天
    /// </summary>
    Rain,

    /// <summary>
    /// 晴天
    /// </summary>
    Sunny,

    /// <summary>
    /// 下雪
    /// </summary>
    Snow,

    /// <summary>
    /// 起雾
    /// </summary>
    Fog,

    /// <summary>
    /// 冰雹
    /// </summary>
    Hail,

    /// <summary>
    /// 沙暴
    /// </summary>
    Sandstorm,

    /// <summary>
    /// 大雨
    /// </summary>
    HeavyRain,

    /// <summary>
    /// 大日照
    /// </summary>
    ExtremelyHarshSunlight,

    /// <summary>
    /// 乱流
    /// </summary>
    StrongWinds,
}
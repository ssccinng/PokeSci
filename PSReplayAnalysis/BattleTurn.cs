using System.Reflection;
using System.Text.Json.Serialization;
using static PSReplayAnalysis.ExporttoTrainData;

namespace PSReplayAnalysis;

/// <summary>
/// 对战单回合数据
/// </summary>
[Serializable]
public record BattleTurn
{
    public BattleTurn()
    {
    }

    /// <summary>
    /// 回合数
    /// </summary>
    public int TurnId { get; set; }

    /// <summary>
    /// 本轮战斗是否结束
    /// </summary>
    public bool BattleEnd { get; set; }
    [JsonInclude]
    /// <summary>
    /// 全体状态
    /// </summary>
    public BattleField AllField = new();
    /// <summary>
    /// 玩家1场地状态
    /// </summary>
    [JsonInclude]
    public OneSideBattleField Side1Field = new();
    /// <summary>
    /// 玩家2场地状态
    /// </summary>
    [JsonInclude]
    public OneSideBattleField Side2Field = new();

    public List<PokemonStatus> Side1Pokes { get; set; } = new List<PokemonStatus> { new(), new() };
    public List<PokemonStatus> Side2Pokes { get; set; } = new List<PokemonStatus> { new(), new() };
    [JsonInclude]

    public Team Player1Team = new();
    /// <summary>
    /// 玩家2队伍
    /// </summary>
    [JsonInclude]
    public Team Player2Team = new();

    public List<ChildTurn> ChildTurns { get; set; } = new();

    public BattleAction[] Battle1Actions { get; set; } = new BattleAction[2];
    public BattleAction[] Battle2Actions { get; set; } = new BattleAction[2];

    public float Reward1 { get; set; }
    public float Reward2 { get; set; }

    public BattleTurn NextTurn()
    {
        BattleTurn next = this with { };

        next.Battle1Actions = new BattleAction[2];
        next.Battle2Actions = new BattleAction[2];
        next.Reward1 = next.Reward2 = 0;

        next.TurnId++;
        next.AllField.NextTurn();
        next.Player1Team.Pokemons = Player1Team.Pokemons.Select(s => s with { }).ToList();
        next.Player2Team.Pokemons = Player2Team.Pokemons.Select(s => s with { }).ToList();
        // 切换 状态就需要清空

        //next.Player1Team.Pokemons[0].HPRemain--;
        next.Side1Pokes = Side1Pokes.Select(s => s with { }).ToList();
        next.Side2Pokes = Side2Pokes.Select(s => s with { }).ToList();
        next.Side1Field = Side1Field with { };
        next.Side2Field = Side2Field with { };
        next.Side1Field.NextTurn();
        next.Side2Field.NextTurn();
        for (int i = 0; i < next.Side1Pokes.Count; i++)
        {
            next.Side1Pokes[i].NextTurn();
            next.Side2Pokes[i].NextTurn();
        }
        // 将next.Side1Field中所有成员 依据Decrease特性修改其值





        //BattleField AllField1 = new();
        //AllField1.UproarRemain = 1;
        return next;
    }

}

public class BattleEvent
{
    public (int side, int pos) Source { get; set; }
    //public // 
    // target?
    // do what
    public string Action { get; set; } = "";
}
public record ChildTurn
{
    public Action<BattleTurn> aa;
    public void ApplyTurn(BattleTurn battleTurn)
    {
        // 应用修改论
        aa.Invoke(battleTurn);
    }
}


// 技能选择失败后 还需要再选 
// 要考虑pp变化
public record PokemonStatus
{
    // 生成一个将自己成员依据SingleTurn特性修改自己的值的方法
    // 接口一下
    public void NextTurn()
    {
        foreach (var property in GetType().GetProperties())
        {
            var decrease = property.GetCustomAttribute<DecreaseAttribute>();
            if (decrease != null)
            {
                var value = (int)property.GetValue(this);
                if (value > 0)
                    value = Math.Max(value + decrease.DeltaValue, 0);
                property.SetValue(this, value);
            }
            //var changeRefresh = property.GetCustomAttribute<ChangeRefreshAttribute>();
            //if (changeRefresh != null)
            //{
            //    property.SetValue(this, 0);
            //}

            // 生成一个将自己成员依据SingleTurn特性修改自己的值的方法

            var singleTurn = property.GetCustomAttribute<SingleTurnAttribute>();
            if (singleTurn != null)
            {
                property.SetValue(this, 0);
            }
        }
    }
    // 利用反射 将所有成员映射到一个数组中

    internal int[] Export()
    {
        var props = GetType().GetProperties();
        int[] result = new int[props.Length];

        for (int i = 0; i < props.Length; i++)
        {
            result[i] = (int)props[i].GetValue(this);
        }
        return result;
    }



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
    [SingleTurn]
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
    public int Protect { get; set; }
    [SingleTurn]
    public int WideGuard { get; set; }
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
}

/// <summary>
/// 单边场地状态 考虑struct
/// </summary>
public record OneSideBattleField
{
    // 生成一个将自己成员依据Decrease特性修改自己的值的方法

    public void NextTurn()
    {
        foreach (var property in GetType().GetProperties())
        {
            var decrease = property.GetCustomAttribute<DecreaseAttribute>();
            if (decrease != null)
            {
                var value = (int)property.GetValue(this);
                if (value > 0)
                    value = Math.Max(value + decrease.DeltaValue, 0);
                property.SetValue(this, value);
            }
            //var changeRefresh = property.GetCustomAttribute<ChangeRefreshAttribute>();
            //if (changeRefresh != null)
            //{
            //    property.SetValue(this, 0);
            //}

            // 生成一个将自己成员依据SingleTurn特性修改自己的值的方法

            var singleTurn = property.GetCustomAttribute<SingleTurnAttribute>();
            if (singleTurn != null)
            {
                property.SetValue(this, 0);
            }
        }
    }

    internal int[] Export()
    {
        var props = GetType().GetProperties();
        int[] result = new int[props.Length];

        for (int i = 0; i < props.Length; i++)
        {
            result[i] = (int)props[i].GetValue(this);
        }
        return result;
    }




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



/// <summary>
/// 全体状态变化
/// </summary>
public struct BattleField
{
    public BattleField()
    {
    }

    public void NextTurn()
    {
        if (TrickRoom > 0) TrickRoom--;
        if (WonderRoom > 0) WonderRoom--;
        if (MagicRoom > 0) MagicRoom--;
        if (Gravity > 0) Gravity--;
        if (WeatherRemain > 0) WeatherRemain--;
        if (TerrainRemain > 0) TerrainRemain--;
        if (MudSport > 0) MudSport--;
        if (WaterSport > 0) WaterSport--;
        if (Uproar > 0) Uproar--;

    }

    internal int[] Export()
    {
        var props = GetType().GetProperties();
        int[] result = new int[props.Length];

        for (int i = 0; i < props.Length; i++)
        {
            result[i] = (int)props[i].GetValue(this);
        }
        return result;
    }

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
    public int Gravity { get; set; } = 1;

    /// <summary>
    /// 天气
    /// </summary>

    public Weather Weather { get; set; }
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



public enum MoveTarget
{
    /// <summary>
    /// 任意
    /// </summary>
    Normal,
    AllAdjacentFoes,
    /// <summary>
    /// 自己
    /// </summary>
    Self,
    Any,
    AdjacentAllyOrSelf,
    AdjacentAlly,

}

public enum ContestType
{
    /// <summary>
    /// 美丽
    /// </summary>
    Beautiful,
    /// <summary>
    /// 聪明
    /// </summary>
    Clever,
    /// <summary>
    /// 强壮
    /// </summary>
    Tough,
    /// <summary>
    /// 可爱
    /// </summary>
    Cute,
    /// <summary>
    /// 帅气 
    /// </summary>
    Cool,
}
/// <summary>
/// 技能
/// </summary>
public struct Move
{
    public int MoveId
    {
        get;
        set;
    }

    /// <summary>
    /// 威力
    /// </summary>
    public int? Pow
    {
        get;
        set;
    }

    /// <summary>
    /// 命中率
    /// </summary>
    public int? Acc
    {
        get;
        set;
    }

    public int PP
    {
        get;
        set;
    }

    /// <summary>
    /// 招式属性
    /// </summary>
    public int MoveType
    {
        get;
        set;
    }

    /// <summary>
    /// 招式分类 
    /// </summary>
    public MoveCategory MoveCategory
    {
        get;
        set;
    }

    /// <summary>
    /// 优先级
    /// </summary>
    public int Priority
    {
        get;
        set;
    }




    // 加入z 超级巨等标记
}

public enum MoveCategory
{
    /// <summary>
    /// 物理
    /// </summary>
    Physical,

    /// <summary>
    /// 特殊
    /// </summary>
    Special,

    /// <summary>
    /// 变化招式
    /// </summary>
    Status,
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



public class PSMove
{
    public int num { get; set; }
    public bool accuracy { get; set; }
    public int basePower { get; set; }
    public string category { get; set; }
    public string isNonstandard { get; set; }
    public string name { get; set; }
    public int pp { get; set; }
    public int priority { get; set; }
    public Flags flags { get; set; }
    public string isZ { get; set; }
    public int critRatio { get; set; }
    public object secondary { get; set; }
    public string target { get; set; }
    public string type { get; set; }
    public string contestType { get; set; }
}

public class Flags
{
}

namespace Poke.Core.Models;

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
public class Move
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
    public PokeType MoveType
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

    /// <summary>
    /// 招式名字
    /// </summary>
    public Text? Name
    {
        get;
        set;
    }

    /// <summary>
    /// 招式描述 
    /// </summary>
    public Text? Description
    {
        get;
        set;
    }
    // 对战前招式


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
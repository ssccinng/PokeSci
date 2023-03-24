using System.Text.Json.Serialization;

namespace PSReplayAnalysis;

/// <summary>
/// 对战数据
/// </summary>
public record BattleData
{
    public BattleData()
    {
        
    }
    /// <summary>
    /// 玩家1分数
    /// </summary>
    public int Player1Score { get; set; }

    /// <summary>
    /// 玩家2分数
    /// </summary>
    public int Player2Score { get; set; }

    /// <summary>
    /// 每回合数据
    /// </summary>
    [JsonInclude]
    public List<BattleTurn> BattleTurns = new();

    /// <summary>
    /// 谁赢了
    /// </summary>
    public BattleResult WhoWin { get; set; }
    /// <summary>
    /// 玩家1队伍
    /// </summary>
    public Team Player1Team { get; set; } = new Team();
    /// <summary>
    /// 玩家2队伍
    /// </summary>
    public Team Player2Team { get; set; } = new Team();
    public BattleEndReason BattleEndResult { get; set; }
    public string Player1Id { get; set; }
    public string Player2Id { get; set; }
}

public enum BattleEndReason
{

}

public enum BattleResult
{
    Draw,
    Player1Win,
    Player2Win,
}

/// <summary>
/// 队伍
/// </summary>
public struct Team
{
    public Team()
    {
    }
    [JsonInclude]
    public List<Pokemon>  Pokemons= new();
}

public record Pokemon
{
    public Pokemon()
    {
    }

    /// <summary>
    /// 宝可梦id
    /// </summary>
    [JsonInclude]
    public int Id;
    /// <summary>
    /// 最高为48
    /// </summary>
    public int HPRemain { get; set; } = 100;
    /// <summary>
    /// shi'f可以太晶
    /// </summary>
    public bool CanTear { get; set; } = true;
    /// <summary>
    /// 已经太晶 （需要修改属性吗？
    /// </summary>
    public int IsTear { get; set; }
    [JsonInclude]
    public int NowPos;
    ///// <summary>
    ///// 默认为一个很垃圾的技能
    ///// </summary>
    //[JsonInclude]
    //public Move[] moves = new Move[4];

}


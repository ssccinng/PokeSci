namespace PSReplayAnalysis;

/// <summary>
/// 对战数据
/// </summary>
public class BattleData
{
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
    public List<BattleTurn> BattleTurns { get; set; } = new();

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
public class Team
{
    public List<Pokemon> Pokemons { get; set; } = new();
}

public class Pokemon
{
    /// <summary>
    /// 宝可梦id
    /// </summary>
    public int Id { get; set; }
}
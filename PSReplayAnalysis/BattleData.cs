using PSReplayAnalysis.PokeLib;
using System.Text.Json.Serialization;
using static PSReplayAnalysis.ExporttoTrainData;

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
    public List<Pokemon> Pokemons = new();


    public int[] Export()
    {
        int len = 12;
        int[] res = new int[Pokemons.Count * len];
        for (int i = 0; i < Pokemons.Count; i++)
        {
            var poke = PSReplayAnalysis.PsPokes1[Pokemons[i].Id];

            res[i * len] = Pokemons[i].Id;
            // poke.baseStats赋值res的前六个
            res[i * len + 1] = poke.baseStats.hp;
            res[i * len + 2] = poke.baseStats.atk;
            res[i * len + 3] = poke.baseStats.def;
            res[i * len + 4] = poke.baseStats.spa;
            res[i * len + 5] = poke.baseStats.spd;
            res[i * len + 6] = poke.baseStats.spe;

            // poke.types赋值res的第7-8

            for (int j = 0; j < poke.types.Length; j++)
            {
                res[i * len + 7 + j] = Pokemondata.GetEngTypeId(poke.types[j]);
            }

            res[i * len + 9] = Pokemons[i].HPRemain;
            res[i * len + 10] = Pokemons[i].NowPos;
            res[i * len + 11] = Pokemons[i].TeraType == null ? 0 : Pokemondata.GetEngTypeId(Pokemons[i].TeraType);

        }

        return res;
    }


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
    public int NowPos = -1;
    public string TeraType { get; set; }
    ///// <summary>
    ///// 默认为一个很垃圾的技能
    ///// </summary>
    //[JsonInclude]
    //public Move[] moves = new Move[4];

}


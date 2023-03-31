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

    public float[] ExportMove(int type)
    {
        int len = 36;
        float[] res = new float[6 * len];
        for (int i = 0; i < Pokemons.Count; i++)
        {

            for (int j = 0; j < 4;++j)
            {
                PSMove pSMove = new() { type = "Normal" };
                int vidx = i * len + j * 9;
                if (type == 0)
                {
                    res[vidx] = Pokemons[i].SelfMovesId[j];
                    if (Pokemons[i].SelfMovesId[j] != 0)
                    pSMove = PSReplayAnalysis.PsMove1[Pokemons[i].SelfMovesId[j]];
                }
                else 
                {
                    res[vidx] = Pokemons[i].MovesId[j];
                    if (Pokemons[i].MovesId[j] != 0)
                    pSMove = PSReplayAnalysis.PsMove1[Pokemons[i].MovesId[j]];

                }
                res[vidx + 1] = pSMove.priority;
                res[vidx + 2] = pSMove.critRatio ?? 0;
                res[vidx + 3] = pSMove.accuracy;
                res[vidx + 4] = pSMove.basePower;
                res[vidx + 5] = Pokemondata.GetEngTypeId(pSMove.type);
                switch (pSMove.category)
                {
                    case "Special":
                        res[vidx + 6] = 1;
                        break;
                    case "Physical":
                        res[vidx + 7] = 1;
                        break;
                    case "Status":
                        res[vidx + 8] = 1;
                        break;
                    default:
                        break;
                }
            }



        }

        return res;
    }
    public float[] Export()
    {
        int len = 12;
        float[] res = new float[6 * len];
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
    public string NickName { get; set; }

    // 自己已知
    public int[] SelfMovesId { get; set; } = new int[4];
    // 全局已知
    public int[] MovesId { get; set; } = new int[4];

    public int AddMove(int mid)
    {
        if (SelfMovesId.Any(s => s == 0) && SelfMovesId.All(s => s != mid))
        {
            while (true)
            {
                var rint = Random.Shared.Next(0, 4);
                if (SelfMovesId[rint] == 0)
                {
                    SelfMovesId[rint] = MovesId[rint] = mid;
                    return rint;
                }
                
            }
        }
        else
        {
            return -1;
        }
        // 可能要打乱move顺序？
    }

    public int GetMove(int mid)
    {
        return Array.IndexOf(SelfMovesId, mid);
        // 可能要打乱move顺序？
    }
    ///// <summary>
    ///// 默认为一个很垃圾的技能
    ///// </summary>
    //[JsonInclude]
    //public Move[] moves = new Move[4];

}


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
        int len = 36 + 17;
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
                res[vidx + 1] = pSMove.priority * 1f / 5;
                res[vidx + 2] = (pSMove.critRatio ?? 0) / 4.0f;
                res[vidx + 3] = pSMove.accuracy * 1f / 100;
                res[vidx + 4] = pSMove.basePower * 1f / 300;
                // 不行就把这个展开
                res[vidx + 5] = GetTargetId(pSMove.target);
                res[vidx + 2 + Pokemondata.GetEngTypeId(pSMove.type)] = 1;

                switch (pSMove.category)
                {
                    case "Special":
                        res[vidx + 23] = 1;
                        break;
                    case "Physical":
                        res[vidx + 24] = 1;
                        break;
                    case "Status":
                        res[vidx + 25] = 1;
                        break;
                    default:
                        break;
                }
            }



        }

        return res;
    }

    private float GetTargetId(string target) => target switch
    {
        "normal" => 1,
        "allAdjacentFoes" => 2,
        "self" => 3,
        "any" => 4,
        "adjacentAllyOrSelf" => 5,
        "adjacentAlly" => 6,


        _ => 0,
    };

    public float[] Export()
    {
        int len = 30;
        float[] res = new float[6 * len];
        Pokemons.OrderBy(s => s.NowPos);
        for (int i = 0; i < Pokemons.Count; i++)
        {
            var poke = PSReplayAnalysis.PsPokes1[Pokemons[i].Id];

            res[i * len] = Pokemons[i].Id;
            // poke.baseStats赋值res的前六个
            res[i * len + 1] = poke.baseStats.hp * 1f / 255;
            res[i * len + 2] = poke.baseStats.atk * 1f / 255;
            res[i * len + 3] = poke.baseStats.def * 1f / 255;
            res[i * len + 4] = poke.baseStats.spa * 1f / 255;
            res[i * len + 5] = poke.baseStats.spd* 1f / 255;
            res[i * len + 6] = poke.baseStats.spe * 1f / 255;

            // poke.types赋值res的第7-8

            for (int j = 0; j < poke.types.Length; j++)
            {
                res[i * len + 7 + Pokemondata.GetEngTypeId(poke.types[j]) - 1] = 1;
            }

            res[i * len + 25] = Pokemons[i].HPRemain * 1f / 100;
            //res[i * len + 9] = 1 << (Pokemons[i].NowPos + 2);
            if (Pokemons[i].NowPos == 0)
            {
                res[i * len + 26] = 1;

            }
            else if (Pokemons[i].NowPos == 1)
            {
                res[i * len + 27] = 1;
            }
            else if (Pokemons[i].NowPos == -2)
            {
                res[i * len + 28] = 1;
            }
            else
            {
                res[i * len + 29] = 1;
            }
            //res[i * len + 10] = 1 << (Pokemons[i].TeraType == null ? 0 : Pokemondata.GetEngTypeId(Pokemons[i].TeraType));

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


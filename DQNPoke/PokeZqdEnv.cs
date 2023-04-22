using Microsoft.VisualBasic;
using PokeCommon.Models;
using PokeCommon.PokemonShowdownTools;
using PokemonDataAccess.Migrations;
using PokePSCore;
using PSReplayAnalysis;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using static PSReplayAnalysis.ExporttoTrainData;
using static PSReplayAnalysis.PSReplayAnalysis;
using static System.Reflection.Metadata.BlobBuilder;

namespace DQNTorch;

/// <summary>
/// ZQDQN
/// </summary>


public class TrainBattle
{
    private readonly float discountFactor = 0.7f;

    public readonly List<BattleRecord> TempBuffer = new();

    public required string BattleTag;
    public required GamePokemonTeam GamePokemonTeam;
    public required double epsilon;
    public BattleStatus BattleStatus = BattleStatus.Waiting;
    public required PSReplayAnalysis.PSReplayAnalysis PSReplayAnalysis;
    // 纯攻击ai
    public bool AtkBot = false;
    private object _lockStatus = new();

    /// <summary>
    /// 反向传播胜利奖励
    /// </summary>
    public void BackWard()
    {
        int lastTurn = TempBuffer.Last().turn;
        float cf = lastTurn > 10 ? (lastTurn - 10.0f) / 10 : 10f / -(11 - lastTurn);
        cf *= lastTurn / 20 + 1;
        //lastT
        float lastReward = TempBuffer.Last().rewards - cf;
        //float lastTemp = 1f;
        for (int i = TempBuffer.Count - 2; i >= 0; --i)
        {
            if (TempBuffer[i].turn == lastTurn)
            {
                TempBuffer[i] = TempBuffer[i] with { rewards = lastReward };
            }
            else
            {
                lastTurn = TempBuffer[i].turn;
                lastReward = discountFactor * lastReward + TempBuffer[i].rewards;
                TempBuffer[i] = TempBuffer[i] with { rewards = lastReward };
            }
        }
    }

    public void SetStatus (BattleStatus status)
    {
            BattleStatus = status;
        //lock (_lockStatus)
        //{
        //}
    }

    internal void AddBuffer((float[] state, int, float, float[] nextStates, int, int TurnId) value)
    {
        // 这里最好需要锁？ 不 大概不需要吧
        TempBuffer.Add(value);
    }
}

public record struct BattleRecord(float[] states, long actions, float rewards, float[] next_states, float dones, int turn)
{
    public static implicit operator (float[] states, long actions, float rewards, float[] next_states, float dones)(BattleRecord value)
    {
        return (value.states, value.actions, value.rewards, value.next_states, value.dones);
    }

    public static implicit operator BattleRecord((float[] states, long actions, float rewards, float[] next_states, float dones, int turn) value)
    {
        return new BattleRecord(value.states, value.actions, value.rewards, value.next_states, value.dones, value.turn);
    }
}
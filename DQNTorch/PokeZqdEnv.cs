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
public class PokeZqdEnv
{
    private readonly ZQDQNAgent agent;

    public ConcurrentDictionary<string, TrainBattle> trainBattles = new();

    public PSClient PSClient { get; set; }

    public PokeZqdEnv(ZQDQNAgent agent)
    {
        this.agent = agent;
    }

    public static List<string> GamePokemonTeams = InitTeams();
    /// <summary>
    /// 暂存
    /// </summary>
    private GamePokemonTeam randTeam;
    public double epsilon;
    private object _lockTeam = new();

    private static List<string> InitTeams()
    {
        var data = File.ReadAllText("Team.txt");
        return data.Split("----").Select(s => s.Trim()).ToList();
    }

    public async Task Init(string name, string pwd, string wsUrl = "ws://localhost:8000/showdown/websocket")
    {
        PSClient = new PSClient(name, pwd, "ws://localhost:8000/showdown/websocket");
        //PSClient.LogTo(// Console.WriteLine);
        await PSClient.ConnectAsync();
        await Task.Delay(1000);
        await PSClient.LoginGuestAsync();

        await PSClient.SendJoinAsync("lobby");
        //await PSClient.SendCancle();


        PSClient.OnTeampreview += PSClient_OnTeampreview;
        PSClient.OnForceSwitch += PSClient_OnForceSwitch;
        PSClient.OnChooseMove += PSClient_OnChooseMove;
        PSClient.BattleEndAction += PSClient_BattleEndAction;
        PSClient.BattleErrorAction += PSClient_BattleErrorAction;
        PSClient.BattleStartAction += PSClient_BattleStartAction; ;
        PSClient.RequestsAction += PSClient_RequestsAction; ;
        PSClient.ChallengeAction += PSClient_ChallengeAction;
        PSClient.BattleInfo += PSClient_BattleInfo;
        
    }

    private void PSClient_BattleStartAction(PokePSCore.PsBattle obj)
    {
        //throw new NotImplementedException();
    }

    private void PSClient_RequestsAction(PokePSCore.PsBattle battle)
    {
        var trainBattle = trainBattles.GetValueOrDefault(battle.Tag);
        if (trainBattle != null)
        {
            trainBattle.SetStatus(BattleStatus.Requests);
            return;
            // 状态置为error
            //trainBattle.BattleStatus = BattleStatus.Error;
        }
        throw new NotImplementedException();

    }


    /// <summary>
    /// 数据更新
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void PSClient_BattleInfo(PokePSCore.PsBattle battle, string info)
    {
        var trainBattle = trainBattles.GetValueOrDefault(battle.Tag);
        if (info.Contains("|init|battle"))
        {
            // 是初始化
            trainBattle = new TrainBattle
            {
                BattleTag = battle.Tag,
                epsilon = epsilon,
                GamePokemonTeam = randTeam,
                PSReplayAnalysis = new PSReplayAnalysis.PSReplayAnalysis
                {
                    RoomId = battle.Tag
                }
            };

            trainBattle.PSReplayAnalysis.battle.BattleTurns.Add(new BattleTurn
            {
                TurnId = 0,

            });
            trainBattles.TryAdd(battle.Tag, trainBattle);
        }
        if (trainBattle == null)
        {
            return;
        }
        // 刷新本轮信息
        trainBattle.PSReplayAnalysis.Refresh(info);
    }

    /// <summary>
    /// 被挑战的时候
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    /// <exception cref="NotImplementedException"></exception>
    private async void PSClient_ChallengeAction(string player, string rule)
    {

        if (rule.StartsWith("gen9vgc2023"))
        {
            // log一下
            lock (_lockTeam)
            {
                randTeam =  GetRandomTeam().Result;
                 PSClient.ChangeYourTeamAsync( PSConverter.ConvertToPsOneLineAsync(randTeam).Result).Wait();
            }

              
            await PSClient.AcceptChallengeAsync(player);
        }
    }

    private void PSClient_BattleErrorAction(PokePSCore.PsBattle battle, string info)
    {
        var trainBattle = trainBattles.GetValueOrDefault(battle.Tag);
        if (trainBattle != null)
        {
            // Console.WriteLine(info);
            trainBattle.SetStatus(BattleStatus.Error);
            return;
            // 状态置为error
            //trainBattle.BattleStatus = BattleStatus.Error;
        }
        throw new NotImplementedException();
    }

    /// <summary>
    /// 对局结束
    /// </summary>
    /// <param name="battle"></param>
    /// <param name="info"></param>
    /// <exception cref="NotImplementedException"></exception>

    private async void PSClient_BattleEndAction(PokePSCore.PsBattle battle, bool info)
    {
        //battle.BattleStatus = BattleStatus.End;
        // 等待其他信息完毕 怎么实现

        var trainBattle = trainBattles.GetValueOrDefault(battle.Tag);

        if (trainBattle != null)
        {

            trainBattle.SetStatus(BattleStatus.End);
            // Console.WriteLine("等待TurnFinish");
            await Task.Delay(1000);

            // Console.WriteLine("等待TurnFinish完毕" + trainBattle.BattleStatus);

            trainBattle.BackWard();
            agent.AddBuffers(
                trainBattle.TempBuffer.Where(s => s.states != null).Select(s => (s.states, s.actions, s.rewards, s.next_states, s.dones))); ;

            trainBattle.SetStatus(BattleStatus.GameFinish);
            await battle.LeaveRoomAsync();
            trainBattles.TryRemove(battle.Tag, out var trainBattle1);
        }
        else
        {
            // gg
            throw new NotImplementedException();
            return;
        }

        // 消除他
     
    }
    int chooseOnce = 0;
    private async void PSClient_OnChooseMove(PokePSCore.PsBattle battle)
    {

        while (chooseOnce == 1)
        {
            await Task.Delay(100);
        }
        chooseOnce = 1;
         // Console.WriteLine("---------------{0}-PSClient_OnChooseMove---------------------", PSClient.UserName);

        List<ChooseData> chooseDatas = new List<ChooseData>();

        bool dm = false;
        var trainBattle = trainBattles.GetValueOrDefault(battle.Tag);
        if (trainBattle == null)
        {
            throw new Exception("dani");
        }

        if (battle.PlayerPos == PlayerPos.Player1)
        {
            await battle.SendMessageAsync($"reward: {trainBattle.PSReplayAnalysis.battle.BattleTurns[^2].Reward1}");

        }
        else
        {
            await battle.SendMessageAsync($"reward: {trainBattle.PSReplayAnalysis.battle.BattleTurns[^2].Reward2}");

        }
        BattleTurn lastTurn = trainBattle.PSReplayAnalysis.battle.BattleTurns.Last();

        foreach (var item in lastTurn.Player2Team.Pokemons)
        {
            // Console.WriteLine($"{item.NickName} HpRemain{item.HPRemain} NowPos: {item.NowPos}");
        }

        // 开始状态
        float[] state = ExportBattleTurn(lastTurn, (int)(battle.PlayerPos) + 1);
        // 不可换人
        List<int> banid = new();
        List<int> actions = new();

        for (int q = 0; q < 2; q++)
        {
            // Console.WriteLine("给你手动屏蔽在场宝");
            // 下场宝
            var aa = trainBattle.GamePokemonTeam.GamePokemons.FindIndex(s => s.MetaPokemon.DexId == battle.MySide[q].MetaPokemon.DexId);
            if (aa == -1)
            {
                int aaa = 2423;
            }
            banid.Add(aa);
        }
        // 死去的宝也是不可换的 确认一下是否更新
        if (battle.PlayerPos == PlayerPos.Player1)
        {
            for (int i = 0; i < lastTurn.Player1Team.Pokemons.Count; i++)
            {
                if (lastTurn.Player1Team.Pokemons[i].HPRemain == 0)
                {
                    banid.Add(i);

                }

            }
        }
        else
        {
            for (int i = 0; i < lastTurn.Player2Team.Pokemons.Count; i++)
            {
                if (lastTurn.Player2Team.Pokemons[i].HPRemain == 0)
                {
                    banid.Add(i);

                }
            }
        }

        for (int i = 0; i < battle.ActiveStatus.Length; i++)
        {
            // 这里ban一下招式
            // Console.WriteLine("battle.MySide[{2}].IsDead {0} || (battle.MySide[{2}]?.Commanding ?? false {1}", battle.MySide[i].IsDead, battle.MySide[i]?.Commanding, i);
            await battle.SendMessageAsync(string.Format("{3} battle.Actives[{2}] = {0}, (battle.MySide[{2}]?.Commanding == {1}", battle.Actives[i], battle.MySide[i]?.Commanding, i, battle.ActiveStatus.Length));
            if (battle.MySide[i].IsDead || (battle.MySide[i]?.Commanding ?? false)) continue;
            JsonElement movedata = battle.ActiveStatus[i].GetProperty("moves");
            List<int> banmove = new();

            if (battle.ActiveStatus[i].TryGetProperty("trapped", out var trap))
            {
                if (trap.GetBoolean())
                {
                    var aa = trainBattle.GamePokemonTeam.GamePokemons.FindIndex(s => s.MetaPokemon.DexId == battle.MySide[i].MetaPokemon.DexId);
                    if (aa != -1)
                        banmove.AddRange(new[] { 0, 1, 2, 3, 4, 5 });
                }

            }
            // 只可选择一个技能的时候
            if (movedata.GetArrayLength() == 1)
            {
                chooseDatas.Add(new MoveChooseData(1, dmax: false));
                //actions.Add()
                //// 这里不可选 无需加入状态
                continue;
            }

            for (int j = 0; j < movedata.GetArrayLength(); ++j)
            {
                if (movedata[j].GetProperty("disabled").GetBoolean())
                {
                    for (int k = 0; k < 4; k++)
                    {
                        // 按我的来，0 1 2 3代表用第一个技能带2 1 -1 -2位置 \
                        //  2  1  0  1
                        // -1 -2  2  3
                        banmove.Add(6 + j * 4 + k);
                    }
                    continue;
                }


                var target = movedata[j].GetProperty("target").GetString();
                //Thread.Sleep(5000);
                //MessageBox.Show("荤菜做好了");

                if (target == "any" || target == "normal")
                {
                    // 不能选到自己 || target == "AdjacentAllyOrSelf"
                    banmove.Add(6 + j * 4 + (i + 2));
                }
                else if (target == "adjacentFoe")
                {
                    // 选到自己人炸裂
                    banmove.Add(6 + j * 4 + 2);
                    banmove.Add(6 + j * 4 + 3);

                }
                else if (target == "adjacentAlly")
                {
                    banmove.Add(6 + j * 4 + 0);
                    banmove.Add(6 + j * 4 + 1);
                    banmove.Add(6 + j * 4 + i + 2);

                }
                else if (target == "adjacentAllyOrSelf")
                {
                    banmove.Add(6 + j * 4 + 0);
                    banmove.Add(6 + j * 4 + 1);
                }
                else
                {

                    //chooseDatas.Add(new MoveChooseData(moveid, dmax: dflag));

                }
            }

            var resx = agent.act(state, i, epsilon, banid.ToArray().Concat(banmove.ToArray()));
            // 加入动作选择
            actions.Add(resx + 22 * i);

            // 直接就是22
            if (resx < 6)
            {
                banid.Add(resx);

                var aa = Array.FindIndex(battle.MySide, s => s.MetaPokemon.DexId == trainBattle.GamePokemonTeam.GamePokemons[resx].MetaPokemon.DexId) + 1;
                chooseDatas.Add(new SwitchData { PokeId = aa });
                if (aa < 3)
                {
                    throw new Exception("行动时换人出错");
                }

            }
            else
            {
                resx -= 6;
                int moveid = resx / 4 + 1;
                int target2 = resx % 4 + 1; // 1 2 3 4 -> 2 1 -1 -2 
                if (target2 > 2) target2 = 2 - target2;
                else target2 = 3 - target2;
                string target;
                bool dflag = false;
                try
                {

                    target = movedata[moveid - 1].GetProperty("target").GetString()!;

                    // Console.WriteLine(target);
                    // Console.WriteLine(battle.ActiveStatus[i].GetProperty("moves")[moveid - 1].GetRawText());
                    if (target == "any" || target == "normal")
                    {
                        // 不能选到自己 || target == "AdjacentAllyOrSelf"

                        if (target2 == -(i + 1))
                        {
                            throw new Exception("你选了个寂寞 any");
                            return;
                        }
                        chooseDatas.Add(new MoveChooseData(moveid, dmax: dflag) { Target = target2 });

                    }
                    else if (target == "adjacentFoe")
                    {
                        // 选到自己人炸裂
                        if (target2 < 0)
                        {
                            throw new Exception("你选了个寂寞 adjacentFoe");

                            //await OnLose(battle, "招式选择到了自己人");
                            return;
                        }
                        chooseDatas.Add(new MoveChooseData(moveid, dmax: dflag) { Target = target2 });

                    }
                    else if (target == "adjacentAlly")
                    {
                        if (target2 > 0 || target2 == -(i + 1))
                        {
                            throw new Exception("你选了个寂寞 adjacentAlly");

                            return;
                        }
                        chooseDatas.Add(new MoveChooseData(moveid, dmax: dflag) { Target = target2 });

                    }
                    else if (target == "adjacentAllyOrSelf")
                    {
                        if (target2 > 0)
                        {
                            throw new Exception("你选了个寂寞 adjacentAllyOrSelf");

                            return;
                        }
                        // 选到对面 炸裂
                        chooseDatas.Add(new MoveChooseData(moveid, dmax: dflag) { Target = target2 });
                    }
                    else
                    {
                        
                        chooseDatas.Add(new MoveChooseData(moveid, dmax: dflag));

                    }
                }
                catch (global::System.Exception e)
                {
                    // Console.WriteLine(e.Message);
                    // Console.WriteLine(e.StackTrace);
                    // Console.WriteLine("异常了");
                    throw;
                    //chooseDatas.Add(new MoveChooseData(1));
                }
            }


        }
        //battle.BattleStatus = BattleStatus.Waiting;
        // Console.WriteLine("---------------{0}-PSClient_OnChooseMove--ENDDD1-------------", PSClient.UserName);


        trainBattle.SetStatus(BattleStatus.Waiting);
        await battle.SendMoveAsunc(chooseDatas.ToArray());
        // 要等待一下
        // Console.WriteLine("---------------{0}-PSClient_OnChooseMove--ENDDD2-------------", PSClient.UserName);
        await  WaitRequests(trainBattle);
        // Console.WriteLine("---------------{0}-PSClient_OnChooseMove--ENDDD3-------------", PSClient.UserName);

        // Console.WriteLine("等到了结果" + trainBattle.BattleStatus);
        if (trainBattle.BattleStatus == BattleStatus.Error)
        {
            //battle.BattleStatus
            // gg
            //await OnLose(battle, "出招问题");
            throw new Exception("出招出错 你就不该错");
        }
        else if (trainBattle.BattleStatus == BattleStatus.End || trainBattle.BattleStatus == BattleStatus.Requests)
        {
            await Task.Delay(100);

            var next = ExportBattleTurn(lastTurn, (int)battle.PlayerPos + 1);
            foreach (var item in actions)
            {
                // 这个reward也要给好
                trainBattle.AddBuffer(
                    (state, item, battle.PlayerPos == PlayerPos.Player1 ? lastTurn.Reward1 : lastTurn.Reward2
                    , next,
                    trainBattle.BattleStatus == BattleStatus.End ? 1 : 0, lastTurn.TurnId)

                );

            }
            trainBattle.SetStatus(BattleStatus.TurnFinish);

        }
        chooseOnce = 0;
    }
    int fors = 0;
    private async void PSClient_OnForceSwitch(PokePSCore.PsBattle battle, bool[] actives)
    {
        while (fors == 1)
        {
            await Task.Delay(100);
        }
        fors = 1;
         // Console.WriteLine("-----------------{0}-PSClient_OnForceSwitch------------", PSClient.UserName);
        var trainBattle = trainBattles.GetValueOrDefault(battle.Tag);
        if (trainBattle == null)
        {
            throw new Exception("trainBattle 为空");
        }
        //Console.
        //await Console.Out.WriteLineAsync(czcz);
        //return;
        // Console.WriteLine("让我康康你有没有触发");
        BattleTurn lastTurn = trainBattle.PSReplayAnalysis.battle.BattleTurns.Last();
        lastTurn = lastTurn with { };
        lastTurn.Player1Team.Pokemons = lastTurn.Player1Team.Pokemons.Select(s => s with { }).ToList();
        lastTurn.Player2Team.Pokemons = lastTurn.Player2Team.Pokemons.Select(s => s with { }).ToList();
        for (int i = 0; i < 4; i++)
        {

            // HPRemain好像有点问题
            var aa = trainBattle.GamePokemonTeam.GamePokemons.FindIndex(s => s.MetaPokemon.DexId == battle.MySide[i].MetaPokemon.DexId);
            var aa1 = lastTurn.Player2Team.Pokemons.FindIndex(s => s.NowPos == i);
            if (battle.PlayerPos == PlayerPos.Player1)
            {
                lastTurn.Player1Team.Pokemons[aa].HPRemain = (int)Math.Ceiling(battle.MySide[i].NowHp * 100.0 / battle.MySide[i].MaxHP);
            }
            else
            {
                lastTurn.Player2Team.Pokemons[aa].HPRemain = (int)Math.Ceiling(battle.MySide[i].NowHp * 100.0 / battle.MySide[i].MaxHP);

            }
        }
        float[] state = ExportBattleTurn(lastTurn, (int)(battle.PlayerPos) + 1);

        List<int> banids = new List<int>();

        List<ChooseData> chooseDatas = new List<ChooseData>();
        for (int i = 0; i < actives.Length; i++)
        {
            // Console.WriteLine("给你手动屏蔽在场宝");
            // 下场宝
            var aa = trainBattle.GamePokemonTeam.GamePokemons.FindIndex(s => s.MetaPokemon.DexId == battle.MySide[i].MetaPokemon.DexId);
            banids.Add(aa);

        }
        if (battle.PlayerPos == PlayerPos.Player1)
        {
            for (int i = 0; i < lastTurn.Player1Team.Pokemons.Count; i++)
            {
                if (lastTurn.Player1Team.Pokemons[i].HPRemain == 0)
                {
                    banids.Add(i);

                }

            }
        }
        else
        {
            for (int i = 0; i < lastTurn.Player2Team.Pokemons.Count; i++)
            {
                if (lastTurn.Player2Team.Pokemons[i].HPRemain == 0)
                {
                    banids.Add(i);

                }


            }
        }
        for (int i = 0; i < actives.Length; i++)
        {
            // 这个要不要加入回合 待考虑
            // 考虑致命双换问题
            if (actives[i])
            {
                // ban一下自己人？Todo
                //Console.WriteLine("banid = " + string.Concat(banids.Distinct()));
                if (banids.Distinct().Count() == 6)
                {
                    //Console.WriteLine("full banid = " + string.Concat(banids.Distinct()));

                    chooseDatas.Add(new SwitchData { IsPass = true });
                    continue;
                }
                var resx = agent.actSwitch(state, i, epsilon, banids.ToArray());
                banids.Add(resx);
                var aa = Array.FindIndex<PSBattlePokemon>(battle.MySide,
                    s => s.MetaPokemon.DexId == trainBattle.GamePokemonTeam.GamePokemons[resx].MetaPokemon.DexId);
                if (battle.MySide[aa].IsDead)
                {
                    int cc = 32424;
                }
                chooseDatas.Add(new SwitchData { PokeId = aa + 1  });
                
                
            }
        }

        trainBattle.SetStatus(BattleStatus.Waiting);
        await battle.SendMoveAsunc(chooseDatas.ToArray());
        await WaitRequests(trainBattle);
        // 这里要思考... 换人该不该计入状态 我想用回溯奖励 不知道有没有用
        if (trainBattle.BattleStatus == BattleStatus.Error)
        {

            throw new Exception("强制换人最终问题");

        }
        fors = 0;

    }
    /// <summary>
    /// 对局开始选人
    /// </summary>
    /// <param name="battle"></param>
    /// <exception cref="Exception"></exception>
    private async void PSClient_OnTeampreview(PokePSCore.PsBattle battle)
    {
        var trainBattle = trainBattles.GetValueOrDefault(battle.Tag);
        if (trainBattle != null)
        {
            BattleTurn lastTurn = trainBattle.PSReplayAnalysis.battle.BattleTurns.Last();

            // 赋予招式状态
            for (int i = 0; i < battle.MyTeam.Count; i++)
            {
                for (int j = 0; j < trainBattle.GamePokemonTeam.GamePokemons[i].Moves.Count; j++)
                {
                    if (battle.PlayerPos == PlayerPos.Player1)
                    {
                        lastTurn.Player1Team.Pokemons[i].SelfMovesId[j] = PsMoves[trainBattle.GamePokemonTeam.GamePokemons[i].Moves[j].NameEng].num;

                    }
                    else
                    {
                        lastTurn.Player2Team.Pokemons[i].SelfMovesId[j] = PsMoves[trainBattle.GamePokemonTeam.GamePokemons[i].Moves[j].NameEng].num;

                    }
                }
            }



            float[] state = ExportBattleTurn(lastTurn, (int)(battle.PlayerPos) + 1);
            List<int> ChooseTeam = new();
            // 取4个最适合的
            for (int i = 0; i < 4; ++i)
            {
                var a = agent.actSwitch(state,
                   i % 2,
                   epsilon, ChooseTeam);
                ChooseTeam.Add(a);
            }
            for (int i = 0; i < 6; i++)
            {
                if (!ChooseTeam.Contains(i))
                {
                    if (battle.PlayerPos == PlayerPos.Player1)
                    {

                        lastTurn.Player1Team.Pokemons[i].NowPos = -2;
                        lastTurn.Player1Team.Pokemons[i].HPRemain = 0;

                    }
                    else
                    {
                        lastTurn.Player2Team.Pokemons[i].NowPos = -2;
                        lastTurn.Player2Team.Pokemons[i].HPRemain = 0;
                    }
                    ChooseTeam.Add(i);
                }
                
            }
            // 可能不能投的过快 实际来说
            trainBattle.SetStatus(BattleStatus.Waiting);
            await battle.OrderTeamAsync(string.Concat(ChooseTeam.Select(s => s + 1)));

            await WaitRequests(trainBattle);
            if (trainBattle.BattleStatus == BattleStatus.Requests)
            {
                float[] nextStates = ExportBattleTurn(lastTurn, (int)battle.PlayerPos + 1);
                await Task.Delay(100);

                trainBattle.AddBuffer((state, ChooseTeam[0], 0, nextStates, 0, lastTurn.TurnId));
                trainBattle.AddBuffer((state, ChooseTeam[1] + 22, 0, nextStates, 0, lastTurn.TurnId));
                trainBattle.SetStatus(BattleStatus.TurnFinish);
                //await battle.ForfeitAsync();
                // 正常结束
            }
            else if (trainBattle.BattleStatus == BattleStatus.Error)
            {
                throw new Exception("让我看看你为什么有error");
            }
            else if (trainBattle.BattleStatus == BattleStatus.End)
            {
                throw new Exception("实际对战有可能，但ai对练不可能");
            }



        }
        else
        {
            throw new Exception("你怎么空了 OnTeampreview");
        }

    }

    private async Task WaitRequests(TrainBattle trainBattle)
    {
        // 在其他回合要等变finish?
        while (trainBattle.BattleStatus == BattleStatus.Waiting)
        {
            await Task.Delay(100);
        }
    }

    //public async Task 

    public async Task CreateBattleAsync(string name)
    {
        lock (_lockTeam)
        {

            randTeam =  GetRandomTeam().Result;

            PSClient.ChangeYourTeamAsync(PSConverter.ConvertToPsOneLineAsync(randTeam).Result).Wait();

        }
        await PSClient.ChallengeAsync(name, "gen9vgc2023series2");
    }
    /// <summary>
    /// 获取随机打乱队伍
    /// </summary>
    /// <returns></returns>
    public async Task<GamePokemonTeam> GetRandomTeam()
    {
        var team = await PSConverter.ConvertToPokemonsAsync(GamePokemonTeams[Random.Shared.Next(GamePokemonTeams.Count)]);
        team.GamePokemons = team.GamePokemons.OrderBy(s => Random.Shared.Next()).ToList();

        foreach (var item in team.GamePokemons)
        {
            item.Moves = item.Moves.OrderBy(s => Random.Shared.Next()).ToList();
        }
        return team;
    }
    public async Task WaitEnd()
    {
        await Task.Delay(5000);
        while (trainBattles.Count != 0)
        {
            await Task.Delay(100);
        }
    }



}



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
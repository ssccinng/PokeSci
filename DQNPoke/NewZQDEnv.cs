
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

namespace DQNTorch;

public class NewZQDEnv
{
    public NewZQDQNAgent agent;

    TrainBattle trainBattle;
    public PSClient PSClient { get => agent.PSClient; set => agent.PSClient = value; }

    public NewZQDEnv(NewZQDQNAgent agent)
    {
        this.agent = agent;
    }

    public static List<string> GamePokemonTeams = InitTeams();
    /// <summary>
    /// 暂存
    /// </summary>
    private GamePokemonTeam randTeam;
    public double epsilon = 1;
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
        // var trainBattle = trainBattles.GetValueOrDefault(battle.Tag);
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
        // var trainBattle = trainBattles.GetValueOrDefault(battle.Tag);
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
            // trainBattles.TryAdd(battle.Tag, trainBattle);
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
        if (trainBattle != null)
        {
            Console.WriteLine(info);
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

        if (trainBattle != null)
        {

            trainBattle.SetStatus(BattleStatus.End);
            // Console.WriteLine("等待TurnFinish");
            await Task.Delay(1000);

            // Console.WriteLine("等待TurnFinish完毕" + trainBattle.BattleStatus);

            trainBattle.BackWard();
            agent.AddBuffers(
                trainBattle.TempBuffer.Where(s => s.states != null).Select(s => (s.states, s.actions, s.rewards, s.next_states, s.dones))); ;

            await battle.LeaveRoomAsync();
            randTeam = null;
            trainBattle.SetStatus(BattleStatus.GameFinish);
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

        List<int>[] banMove = new List<int>[2] { new(), new() };

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
            // 只可选择一个技能的时候

            for (int j = 0; j < movedata.GetArrayLength(); ++j)
            {
                if (movedata.GetArrayLength() == 1)
                {
                    //// 此时锁死操作 排序？？
                    //chooseDatas.Add(new MoveChooseData(1, dmax: false));
                    //actions.Add()
                    //// 这里不可选 无需加入状态
                    continue;
                }
                if (movedata[j].GetProperty("disabled").GetBoolean())
                {
                    for (int k = 0; k < 4; k++)
                    {
                        // 按我的来，0 1 2 3代表用第一个技能带2 1 -1 -2位置 \
                        //  2  1  0  1
                        // -1 -2  2  3
                        banMove[i].Add(j * 4 + k);
                    }
                    continue;
                }


                var target = movedata[j].GetProperty("target").GetString();
                //Thread.Sleep(5000);
                //MessageBox.Show("荤菜做好了");

                if (target == "any" || target == "normal")
                {
                    // 不能选到自己 || target == "AdjacentAllyOrSelf"
                    banMove[i].Add(j * 4 + (i + 2));
                }
                else if (target == "adjacentFoe")
                {
                    // 选到自己人炸裂
                    banMove[i].Add(j * 4 + 2);
                    banMove[i].Add(j * 4 + 3);

                }
                else if (target == "adjacentAlly")
                {
                    banMove[i].Add(j * 4 + 0);
                    banMove[i].Add(j * 4 + 1);
                    banMove[i].Add(j * 4 + i + 2);

                }
                else if (target == "adjacentAllyOrSelf")
                {
                    banMove[i].Add(j * 4 + 0);
                    banMove[i].Add(j * 4 + 1);
                }
                else
                {

                    //chooseDatas.Add(new MoveChooseData(moveid, dmax: dflag));

                }
            }

        }

        var resx1 = agent.act(state, epsilon, banMove[0], banMove[1]);



        // 加入动作选择
        actions.Add(resx1);
        foreach (var (resx, i) in new[] { (resx1 % 16, 0), (resx1 / 16, 1) })
        {
            if (battle.MySide[i].IsDead || (battle.MySide[i]?.Commanding ?? false)) continue;
            Console.WriteLine($"battle.MySide[i].IsDead = {battle.MySide[i].IsDead}");
            JsonElement movedata = battle.ActiveStatus[i].GetProperty("moves");
            List<int> banmove = new();
            // 只可选择一个技能的时候
            if (movedata.GetArrayLength() == 1)
            {
                //// 此时锁死操作 排序？？
                chooseDatas.Add(new MoveChooseData(1, dmax: false));
                //actions.Add()
                //// 这里不可选 无需加入状态
                continue;
            }

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



        //battle.BattleStatus = BattleStatus.Waiting;
        // Console.WriteLine("---------------{0}-PSClient_OnChooseMove--ENDDD1-------------", PSClient.UserName);


        trainBattle.SetStatus(BattleStatus.Waiting);
        await battle.SendMoveAsunc(chooseDatas.ToArray());
        // 要等待一下
        // Console.WriteLine("---------------{0}-PSClient_OnChooseMove--ENDDD2-------------", PSClient.UserName);
        await WaitRequests(trainBattle);
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
        List<ChooseData> chooseDatas = new List<ChooseData>();
        int choose = 2;
        // 直接有谁换谁
        for (int i = 0; i < actives.Length; i++)
        {
            // 这个要不要加入回合 待考虑
            // 考虑致命双换问题
            if (actives[i])
            {
                bool flag = true;
                // ban一下自己人？Todo
                //Console.WriteLine("banid = " + string.Concat(banids.Distinct()));
                while (choose < 4)
                {
                    if (battle.MySide[choose].NowHp != 0)
                    {
                        chooseDatas.Add(new SwitchData() { PokeId = choose + 1 });
                        choose++;
                        flag = false;
                        break;
                    }
                    choose++;
                }
                if (choose == 4 && flag)
                {
                    chooseDatas.Add(new SwitchData() { IsPass = true });

                }


            }
        }
        await battle.SendMoveAsunc(chooseDatas.ToArray());

    }
    /// <summary>
    /// 对局开始选人
    /// </summary>
    /// <param name="battle"></param>
    /// <exception cref="Exception"></exception>
    private async void PSClient_OnTeampreview(PokePSCore.PsBattle battle)
    {
        if (trainBattle != null)
        {
            BattleTurn lastTurn = trainBattle.PSReplayAnalysis.battle.BattleTurns.Last();
            for (int i = 4; i < 6; i++)
            {
                if (battle.PlayerPos == PlayerPos.Player1)
                {

                    lastTurn.Player1Team.Pokemons[i].NowPos = -2;

                }
                else
                {
                    lastTurn.Player2Team.Pokemons[i].NowPos = -2;
                }
            }

            await battle.OrderTeamAsync("123456");
            // 赋予招式状态
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
        if (trainBattle != null)
        {
            trainBattle.BattleStatus = BattleStatus.Waiting;
        }
        lock (_lockTeam)
        {

            randTeam = GetRandomTeam().Result;

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
    public async Task<BattleResult> WaitEnd()
    {
        while (trainBattle == null || trainBattle.BattleStatus != BattleStatus.GameFinish)
        {
            await Task.Delay(100);
        }
        var aa = trainBattle.PSReplayAnalysis.battle.WhoWin;
        //trainBattle = null;
        // 返回谁赢了
        return aa;
    }


}

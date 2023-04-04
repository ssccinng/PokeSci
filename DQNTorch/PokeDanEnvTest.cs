using PSReplayAnalysis;
using System.Numerics;
using System.Text.Json;
using TorchSharp.Modules;
using static PSReplayAnalysis.ExporttoTrainData;
using static PSReplayAnalysis.PSReplayAnalysis;
using PSReplayAnalysis;
using PokePSCore;
using PokeCommon.Models;
using PokeCommon.PokemonShowdownTools;
using System.Data;
using Org.BouncyCastle.Asn1.X509;

namespace DQNTorch
{
    public class PokeDanEnvTest
    {
        public int BattleId { get; set; } = 0;
        public int Turn { get; set; } = 0;
        public float[] StateSpace { get; set; }
        public List<BattleTrainData> battleTrainDatas { get; set; }
        public PokeDanEnvTest(string path)
        {
            Path = path;
            battleTrainDatas = JsonSerializer.Deserialize<List<BattleTrainData>>(File.ReadAllText(path), new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }

        public string Path { get; }

        internal int[][] GetAction(int battleId, int player)
        {
            if (player == 0)
            {
                return battleTrainDatas[battleId].Player1Action[Turn + 1].ToArray();
            }
            else
            {
                return battleTrainDatas[battleId].Player2Action[Turn + 1].ToArray();
            }

        }

        internal (float[], int cnt) Reset(int battleId, int player)
        {
            BattleId = battleId;
            Turn = 0;
            var stateSpace = GetStateSpace(battleId, -1, player);
            return (stateSpace, battleTrainDatas[battleId].Player1Action.Count);
        }

        private float[] GetStateSpace(int battleId, int turn, int player)
        {
            var state = battleTrainDatas[battleId].StateSpace[turn + 1];
            if (player == 0)
            {
                return (new[] { state[1], state[4], state[2], state[7], state[3] }).SelectMany(s => s).ToArray();
            }
            else
            {
                return (new[] { state[2], state[6], state[1], state[5], state[3] }).SelectMany(s => s).ToArray();

            }
        }

        internal (float[], float, float) Step(int player)
        {
            var res = (GetStateSpace(BattleId, Turn, player), GetReward(BattleId, Turn, player),
            Turn + 2 >= battleTrainDatas[BattleId].Player1Action.Count ? 1 : 0);
            Turn++;
            return res;
        }

        private float GetReward(int battleId, int turn, int player)
        {
            if (player == 0)
            {
                return battleTrainDatas[BattleId].Reward1[Turn + 1];

            }
            else if (player == 1)
            {
                return battleTrainDatas[BattleId].Reward2[Turn + 1];

            }
            return 0;
        }
    }

    public class PokeDanEnvPs
    {
        public DQNAgent DQNAgent { get; set; }

        public static List<string> GamePokemonTeams = InitTeams();

        private static List<string> InitTeams()
        {
            var data = File.ReadAllText("Team.txt");
            return data.Split("----").Select(s => s.Trim()).ToList();
        }
        public GamePokemonTeam NowTeam;
        public Dictionary<string, PSReplayAnalysis.PSReplayAnalysis> replayAnalysis { get; set; } = new();
        // public Dictionary
        public PSClient Player;
        internal double epsilon;

        public PokeDanEnvPs(DQNAgent dQNAgent)
        {
            DQNAgent = dQNAgent;
        }

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

        public float[] CovToStatesSpace(BattleTrainData battleTrainData, PokePSCore.PsBattle psBattle)
        {
            var state = battleTrainData.StateSpace.Last();
            if (psBattle.PlayerPos == PlayerPos.Player1)
            {
                return (new[] { state[1], state[4], state[2], state[7], state[3] }).SelectMany(s => s).ToArray();
            }
            else
            {
                return (new[] { state[2], state[6], state[1], state[5], state[3] }).SelectMany(s => s).ToArray();

            }
        }

        public async Task Init(string name, string pwd)
        {
            Player = new PSClient(name, pwd, "ws://localhost:8000/showdown/websocket");
            Player.LogTo(Console.WriteLine);
            await Player.ConnectAsync();
            await Task.Delay(1000);
            await Player.LoginAsync();

            Player.OnTeampreview += async (PokePSCore.PsBattle battle) =>
            {
                var battlea = replayAnalysis.GetValueOrDefault(battle.Tag);



                // 这里被迫进行动作
                BattleTurn lastTurn = battlea.battle.BattleTurns.Last();

                for (int i = 0; i < battle.MyTeam.Count; i++)
                {
                    for (int j = 0; j < NowTeam.GamePokemons[i].Moves.Count; j++)
                    {
                        if (battle.PlayerPos == PlayerPos.Player1)
                        {
                            lastTurn.Player1Team.Pokemons[i].SelfMovesId[j] = PsMoves[NowTeam.GamePokemons[i].Moves[j].NameEng].num;

                        }
                        else
                        {
                            lastTurn.Player2Team.Pokemons[i].SelfMovesId[j] = PsMoves[NowTeam.GamePokemons[i].Moves[j].NameEng].num;

                        }
                    }
                }
                float[] state = ExportBattleTurn(lastTurn, (int)(battle.PlayerPos) + 1);

                var a = (int)DQNAgent.actSwitch(state,
                    0,
                    epsilon);
                if (battle.PlayerPos == PlayerPos.Player1)
                {
                    lastTurn.Player1Team.Pokemons[a].NowPos = 0;
                }
                else
                {
                    lastTurn.Player2Team.Pokemons[a].NowPos = 0;

                }
                float[] state2 = ExportBattleTurn(lastTurn, (int)(battle.PlayerPos) + 1);

                var b = (int)DQNAgent.actSwitch(state2,
                    1,
                    epsilon, a);
                if (battle.PlayerPos == PlayerPos.Player1)
                {
                    // 同名应该没问题吧
                    lastTurn.Player1Team.Pokemons[b].NowPos = 1;
                }
                else
                {
                    lastTurn.Player2Team.Pokemons[b].NowPos = 1;

                }
                if ( a == b)
                {
                    var aaa = lastTurn;
                    if (battle.PlayerPos == PlayerPos.Player1)
                    {
                        for (int i = 0; i < aaa.Player1Team.Pokemons.Count; i++)
                        {
                            aaa.Player1Team.Pokemons[i].HPRemain = 0;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < aaa.Player1Team.Pokemons.Count; i++)
                        {
                            aaa.Player2Team.Pokemons[i].HPRemain = 0;
                        }
                    }
                    DQNAgent.AddBuffer((state, a, (-1f),
                        state2, 1));
                    DQNAgent.AddBuffer((state2, b + 22, (-1f) ,
                        ExportBattleTurn(aaa, (int)(battle.PlayerPos) + 1), 1));
                    await OnLose(battle, $"选人存在问题 {a} {b}");

                    //DQNAgent.learn();
                    // 结束
                }
                b = b % 22;
                var aa = (new[] { a + 1, b + 1 }).Concat((new [] { 1, 2, 3, 4, 5, 6 })
                .OrderBy(s => Random.Shared.Next())).Distinct();

                for (int j = 4; j < 6; j++)
                {
                    var aaa = lastTurn;

                    if (battle.PlayerPos == PlayerPos.Player1)
                    {

                        aaa.Player1Team.Pokemons[(int)aa.ElementAt(j) - 1].NowPos = -2;
                        aaa.Player1Team.Pokemons[(int)aa.ElementAt(j) - 1].HPRemain =0;

                    }
                    else
                    {
                        aaa.Player2Team.Pokemons[(int)aa.ElementAt(j) - 1].HPRemain =0;
                    }
                }
                battle.BattleStatus = BattleStatus.Waiting;
                await battle.OrderTeamAsync(string.Concat(aa));
                await WaitRequests(battle);
                if (battle.BattleStatus == BattleStatus.End)
                {
                    Console.WriteLine("error");
                    await OnLose(battle, $"选人问题");
                }
                else if (battle.BattleStatus == BattleStatus.Requests)
                {
                    // 思考lastturn有没有问题
                    float[] floats = ExportBattleTurn(lastTurn, (int)battle.PlayerPos + 1);
                    DQNAgent.AddBuffer((state,
                        a,
                        (-10f ) / 10,
                        floats, 1));
                    DQNAgent.AddBuffer((state2,
                        b + 22,
                        (-10f ) / 10,
                        floats, 
                        1));
                    //await battle.ForfeitAsync();
                    // 进入下一轮
                }

            };
            Player.ChallengeAction += async (player, rule) =>
            {
                if (rule.StartsWith("gen9vgc2023"))
                {

                    NowTeam = await GetRandomTeam();
                    lock (_lockDb)
                    {
                        Player.ChangeYourTeamAsync(PSConverter.ConvertToPsOneLineAsync(NowTeam).Result).Wait();

                    }


                    await Player.AcceptChallengeAsync(player);
                }
            };
            Player.OnForceSwitch += async (battle, bools) =>
            {
                // 这里被迫进行动作
                var battlea = replayAnalysis.GetValueOrDefault(battle.Tag);

                //await Console.Out.WriteLineAsync(czcz);
                //return;
                Console.WriteLine("让我康康你有没有触发");
                BattleTurn lastTurn = battlea.battle.BattleTurns.Last();
                float[] state = ExportBattleTurn(lastTurn, (int)(battle.PlayerPos) + 1);
                List<int> ints = new List<int>();

                List<ChooseData> chooseDatas = new List<ChooseData>();
                for (int i = 0; i < bools.Length; i++)
                {
                    if (bools[i])
                    {
                        var resx = (int)DQNAgent.actSwitch(state, i, epsilon, ints.ToArray());
                        ints.Add(resx + i * 22);
                        var aa = Array.FindIndex<PSBattlePokemon>(battle.MySide, 
                            s => s.MetaPokemon.Id == NowTeam.GamePokemons[resx].MetaPokemon.Id);
                        
                        if (aa == -1)
                        {
                            DQNAgent.AddBuffer((state, ints.Last(), -1, state, 1));
                            if (battle.PlayerPos == PlayerPos.Player1)
                            {

                            await battle.SendMessageAsync
                                (string.Join(" ", lastTurn.Player1Team.Pokemons.Select(s => s.NowPos) + " " + resx));
                            }
                            else
                            {
                                await battle.SendMessageAsync(
                              string.Join(" ", lastTurn.Player2Team.Pokemons.Select(s => s.NowPos) + " " + resx
                              ));

                            }
                            await OnLose(battle, $"强制换人时出问题3 {aa + 1}");
                            return;
                        }
                        if (battle.Actives[aa] == false && !battle.MyTeam[aa].IsDead)
                        {
                            chooseDatas.Add(new SwitchData { PokeId = aa + 1 });
                        }
                        else
                        {
                            if (battle.MySide[3].IsDead && battle.MySide[2].IsDead)
                            // 这个pass有问题
                            chooseDatas.Add(new SwitchData { IsPass = true });
                            else
                            {
                                if (battle.PlayerPos == PlayerPos.Player1)
                                {

                                    await battle.SendMessageAsync(
                                        string.Join(" ", lastTurn.Player1Team.Pokemons.Select(s => s.NowPos) + " " + resx
                                        ));
                                }
                                else
                                {
                                    await battle.SendMessageAsync(
                                  string.Join(" ", lastTurn.Player2Team.Pokemons.Select(s => s.NowPos) + " " + resx
                                  ));

                                }
                                DQNAgent.AddBuffer((state, ints.Last(), -1, state, 1));
                                await OnLose(battle, $"强制换人时出问题2 {aa + 1}");
                                return;
                            }

                        }
                        //if (idx == -1)
                        //{
                        //    chooseDatas.Add(new SwitchData { IsPass = true });
                        //}
                        //else
                        //{
                        //    chooseDatas.Add(new SwitchData { PokeId = aa + 1 });

                        //}
                    }
                }

                battle.BattleStatus = BattleStatus.Waiting;
                await battle.SendMoveAsunc(chooseDatas.ToArray());
                await WaitRequests(battle);
                if (battle.BattleStatus == BattleStatus.End)
                {
                    DQNAgent.AddBuffer(((float[] states, long actions, float rewards, float[] next_states, float dones))
                                (state, ints.Last(), -1, state, 1));
                    await OnLose(battle, "强制换人有问题1");
                }
            };

            Player.OnChooseMove += async (PokePSCore.PsBattle battle) =>
            {

                List<ChooseData> chooseDatas = new List<ChooseData>();

                bool dm = false;
                PSReplayAnalysis.PSReplayAnalysis battlea = replayAnalysis.GetValueOrDefault(battle.Tag);
                if(battle.PlayerPos == PlayerPos.Player1)
                {
                await battle.SendMessageAsync($"reward: {battlea.battle.BattleTurns[^2].Reward1}");

                }
                else
                {
                    await battle.SendMessageAsync($"reward: {battlea.battle.BattleTurns[^2].Reward2}");

                }
                BattleTurn lastTurn = battlea.battle.BattleTurns.Last();
                foreach (var item in lastTurn.Player2Team.Pokemons)
                {
                    Console.WriteLine($"{item.NickName} HpRemain{item.HPRemain} NowPos: {item.NowPos}");
                }
                float[] state = ExportBattleTurn(lastTurn, (int)(battle.PlayerPos) + 1);
                List<int> ints = new List<int>();
                List<int> banid = new();

                //for (int i = 0; i < battle.ActiveStatus.Length; i++)
                //{
                //    if (battle.ActiveStatus[i].TryGetProperty("trapped", out var trap))
                //    {
                //        if (trap.GetBoolean())
                //        {
                //            var aa = NowTeam.GamePokemons.FindIndex(s => s.MetaPokemon.Id == battle.MySide[i].MetaPokemon.Id);
                //            if (aa != -1)
                //                banid.Add(aa);
                //        }
                     
                //    }
                //}
                for (int i = 0; i < battle.ActiveStatus.Length; i++)
                {
                    Console.WriteLine("battle.Actives[i] = {0}, (battle.MySide[i]?.Commanding == {1}", battle.Actives[i], battle.MySide[i]?.Commanding);
                    if (!battle.Actives[i] || (battle.MySide[i]?.Commanding ?? false)) continue;
                    JsonElement movedata = battle.ActiveStatus[i].GetProperty("moves");
                    List<int> banmove = new();

                    if (battle.ActiveStatus[i].TryGetProperty("trapped", out var trap))
                    {
                        if (trap.GetBoolean())
                        {
                            var aa = NowTeam.GamePokemons.FindIndex(s => s.MetaPokemon.Id == battle.MySide[i].MetaPokemon.Id);
                            if (aa != -1)
                                banmove.AddRange(new []{ 0, 1, 2, 3, 4, 5});
                        }

                    }
                    if (movedata.GetArrayLength() == 1)
                    {
                        chooseDatas.Add(new MoveChooseData(1, dmax: false));
                        continue;
                    }
                    for (int j = 0; j < movedata.GetArrayLength(); ++j)
                    {
                        if (movedata[j].GetProperty("disabled").GetBoolean())
                        {
                            for (int k = 0; k < 4; k++)
                            {
                                banmove.Add(6 + j + k * 4);
                            }
                        }
                    }
                    // 获取disable and trap
                    var resx = (int)DQNAgent.act(state, i, epsilon, banid.ToArray().Concat(banmove.ToArray()).ToArray());
                    ints.Add(resx + i * 22); // 写死了 很糟糕

                    if (resx < 6)
                    {
                        banid.Add(resx);
                    }
                    // 直接就是22
                    if (resx < 6)
                    {
                        // change
                        // 找到要换的人 从side拉上来
                        //如果换的不合理 直接触发lose
                        var aa = Array.FindIndex(battle.MySide, s => s.MetaPokemon.Id == NowTeam.GamePokemons[resx].MetaPokemon.Id) + 1;
                        chooseDatas.Add(new SwitchData { PokeId = aa });
                        if (aa < 3)
                        {
                            DQNAgent.AddBuffer(((float[] states, long actions, float rewards, float[] next_states, float dones))
                                (state, ints.Last(), -1, state, 1));
                            await OnLose(battle, "行动时换人出错");
                            return;
                            int c = 34; ;
                        }
                        if (aa < 3)
                        {
                            // 出error
                        }
                    }
                    else
                    {
                        resx -= 6;
                        int moveid = resx % 4 + 1;
                        int target2 = resx / 4 + 1;
                        if (target2 > 2) target2 = 2 - target2;
                        string target;
                        bool dflag = false;
                        Console.WriteLine('1');
                        try
                        {

                            target = movedata[moveid-1].GetProperty("target").GetString();

                            Console.WriteLine(target);
                            Console.WriteLine(battle.ActiveStatus[i].GetProperty("moves")[moveid - 1].GetRawText());
                            if (target == "any" || target == "normal")
                            {
                                // 不能选到自己 || target == "AdjacentAllyOrSelf"

                                if (target2 == -(i + 1))
                                {
                                    // 选到自己 直接炸裂 投降
                                    // 加入状态
                                    // 只加这个 其他就不用了
                                    DQNAgent.AddBuffer(((float[] states, long actions, float rewards, float[] next_states, float dones))
                                        (state, ints.Last(), -1, state, 1));
                                    await OnLose(battle, "招式选择到了自己");
                                    return;
                                }
                                chooseDatas.Add(new MoveChooseData(moveid, dmax: dflag) { Target = target2 });

                            }
                            else if (target == "adjacentFoe")
                            {
                                // 选到自己人炸裂
                                if (target2 < 0)
                                {
                                    DQNAgent.AddBuffer(((float[] states, long actions, float rewards, float[] next_states, float dones))
                                        (state, ints.Last(), -1, state, 1));
                                    await OnLose(battle, "招式选择到了自己人");
                                    return;
                                }
                                chooseDatas.Add(new MoveChooseData(moveid, dmax: dflag) { Target = target2 });

                            }
                            else if (target == "adjacentAlly")
                            {
                                if (target2 > 0 || target2 == -(i + 1))
                                {
                                    DQNAgent.AddBuffer(((float[] states, long actions, float rewards, float[] next_states, float dones))
                                        (state, ints.Last(), -1, state, 1));
                                    await OnLose(battle, "招式没选到队友");
                                    return;
                                }
                                chooseDatas.Add(new MoveChooseData(moveid, dmax: dflag) { Target = target2 });

                            }
                            else if (target == "adjacentAllyOrSelf" || target == "adjacentAlly")
                            {
                                if (target2 > 0)
                                {
                                    DQNAgent.AddBuffer(((float[] states, long actions, float rewards, float[] next_states, float dones))
                                        (state, ints.Last(), -1, state, 1));
                                    await OnLose(battle, "招式选择到了别人");
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
                            Console.WriteLine(e.Message);
                            Console.WriteLine(e.StackTrace);
                            Console.WriteLine("异常了");
                            //chooseDatas.Add(new MoveChooseData(1));
                        }
                    }


                }
                battle.BattleStatus = BattleStatus.Waiting;

                await battle.SendMoveAsunc(chooseDatas.ToArray());
                // 要等待一下
                await WaitRequests(battle);
                if (battle.BattleStatus == BattleStatus.End)
                {
                    // gg
                    //await OnLose(battle, "出招问题");
                    DQNAgent.AddBuffer((state, ints.Last(), -1, state, 1));
                    await OnLose(battle, $"出招问题");
                }
                else
                {
                    foreach (var item in ints)
                    {
                        // 这个reward也要给好
                        DQNAgent.AddBuffer(((float[] states, long actions, float rewards, float[] next_states, float dones))
                            (state, item, battle.PlayerPos == PlayerPos.Player1 ? lastTurn.Reward1 : lastTurn.Reward2
                            , ExportBattleTurn(lastTurn, (int)(battle.PlayerPos) + 1),
                            battle.BattleStatus == BattleStatus.End ? 1 : 0)
                        
                        );

                    }

                }
            };

            Player.BattleEndAction += async (PokePSCore.PsBattle battle, bool b) =>
            {
                finish = true; ;
                battle.BattleStatus = BattleStatus.End;

                replayAnalysis.Remove(battle.Tag);
                await battle.LeaveRoomAsync();

            };

            Player.BattleStartAction += battle =>
            {
            };

            Player.BattleErrorAction += async (PokePSCore.PsBattle battle, string msg) =>
            {
                battle.BattleStatus = BattleStatus.End;
                await battle.SendMessageAsync(msg);
                // 输了！
                //OnLose();
                //await battle.ForfeitAsync();

            };

            Player.RequestsAction += battle =>
            {
                battle.BattleStatus = BattleStatus.Requests;

            };

            Player.BattleInfo += async (battle, b) =>
            {
                // 刷新信息
                var battlea = replayAnalysis.GetValueOrDefault(battle.Tag) ?? new PSReplayAnalysis.PSReplayAnalysis() { RoomId = battle.Tag };
                replayAnalysis.TryAdd(battle.Tag, battlea);
                if (battlea.battle.BattleTurns.Count == 0)
                    battlea.battle.BattleTurns.Add(new BattleTurn
                    {
                        TurnId = 0,

                    });


                battlea.Refresh(b);
            };


        }

        private async Task OnLose(PokePSCore.PsBattle battle, string msg)
        {
            await Console.Out.WriteLineAsync($"{battle.Tag} 结束 {msg}");
            await battle.ForfeitAsync();
            await battle.SendMessageAsync(msg);
            //finish = true;
            //throw new NotImplementedException();
        }

        public async Task<(float[] states, float reward, bool done)> Step(string battleTag, int action)
        {
            var replay = replayAnalysis[battleTag];
            var battle = Player.Battles[battleTag];
            if (replay == null)
            {
                // 失败
                return (null, 0, true);
            }
            // 1. 
            var actions = GetChooseDataFromAction(replay, action);

                battle.BattleStatus = BattleStatus.Waiting;
            await battle.SendMoveAsunc(actions.ToArray());
            await WaitRequests(battle);


            // 等待反馈结果
            // 要在这里等到下一轮




            throw new NotImplementedException();
        }

        private async Task<BattleStatus> WaitRequests(PokePSCore.PsBattle battle)
        {
            // 记住！有可能是要重选招式
            // 如果error gg requests 过
            while (battle.BattleStatus == BattleStatus.Waiting)
            {
                await Task.Delay(10);
            }
            return battle.BattleStatus;
        }
        public List<ChooseData> GetChooseDataFromAction1(PSReplayAnalysis.PSReplayAnalysis replay, int action)
        {
            List<ChooseData> chooseDatas = new List<ChooseData>();

            int action1 = action / 2;
            int action2 = action % 22;


            return chooseDatas;
            //throw new NotImplementedException();

        }
        public ChooseData GetChooseData1(int action)
        {
            // 查到本地的 再从side查找对应编号
            throw new NotImplementedException();

        }
        public List<ChooseData> GetChooseDataFromAction(PSReplayAnalysis.PSReplayAnalysis replay, int action)
        {
            List<ChooseData> chooseDatas = new List<ChooseData>();

            int action1 = action % 22;
            int action2 = action / 22;
            throw new NotImplementedException();

        }

        public ChooseData GetChooseData(int action)
        {
            // 查到本地的 再从side查找对应编号
            throw new NotImplementedException();

        }
        bool finish = false;
        internal async Task WaitEnd()
        {
            while (!finish)
            {
                await Task.Delay(10);
            }
            return;
            throw new NotImplementedException();
        }
        object _lockDb = new object();
        internal async Task Challenage(PokeDanEnvPs env1)
        {
            finish = false;
            NowTeam = await GetRandomTeam();
            //team.GamePokemons = team.GamePokemons.OrderBy(s => Random.Shared.Next()).ToList();

            lock (_lockDb)
            {


                Player.ChangeYourTeamAsync(PSConverter.ConvertToPsOneLineAsync(NowTeam).Result).Wait();
                Player.ChallengeAsync(env1.Player.UserName, "gen9vgc2023series2").Wait();
            }

        }
    }

    public interface DQNEnv
    {
        (float[], int battleId) Reset();
        (float[], float, float) Step(int player);


    }
}
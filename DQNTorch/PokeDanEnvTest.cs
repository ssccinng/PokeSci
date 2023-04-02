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
                return battleTrainDatas[battleId].Player1Action[Turn+1].ToArray();
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

        public static List<GamePokemonTeam> GamePokemonTeams = InitTeams();

        private static List<GamePokemonTeam> InitTeams()
        {
            var data = File.ReadAllText("Team.txt");
            return data.Split("----").Select( s => PSConverter.ConvertToPokemonsAsync(s.Trim()).Result).ToList();
        }

        public Dictionary<string, PSReplayAnalysis.PSReplayAnalysis> replayAnalysis { get; set; } = new();
        // public Dictionary
        public PSClient Player;
        internal float epsilon;

        public PokeDanEnvPs(DQNAgent dQNAgent)
        {
            DQNAgent = dQNAgent;
        }

        public async Task<GamePokemonTeam> GetRandomTeam()
        {
            return GamePokemonTeams[Random.Shared.Next(GamePokemonTeams.Count)];
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
            await Player.ConnectAsync();
            await Task.Delay(1000);
            await Player.LoginAsync();

            Player.OnTeampreview += async (PokePSCore.PsBattle battle) =>
            {
                var battlea = replayAnalysis.GetValueOrDefault(battle.Tag) ?? new PSReplayAnalysis.PSReplayAnalysis() { RoomId = battle.Tag };

                // 这里被迫进行动作
                float[] state = ExportBattleTurn(battlea.battle.BattleTurns.Last(), (int)(battle.PlayerPos) + 1);
                var a = DQNAgent.act(state,
                    0,
                    epsilon);
                var b = DQNAgent.act(state,
                    1,
                    epsilon);
                
                if (a > 5 || b % 22 > 5)
                {
                    OnLose();
                    var aaa = battlea.battle.BattleTurns.Last();
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
                    DQNAgent.AddBuffer((state.ToArray(), a, (-10f + Random.Shared.NextSingle()) / 10, 
                        ExportBattleTurn(aaa, (int)(battle.PlayerPos) + 1), 1));
                    DQNAgent.AddBuffer((state.ToArray(), b, (-10f + Random.Shared.NextSingle()) / 10,
                        ExportBattleTurn(aaa, (int)(battle.PlayerPos) + 1), 1));
                    await battle.ForfeitAsync();
                    //DQNAgent.learn();
                    // 结束
                }
                b = b % 22;
                var aa =  new[] { a + 1, b + 1 }.Concat(new long[] { 1, 2, 3, 4,5,6}
                .OrderBy(s => Random.Shared.Next())).Distinct();
                await battle.OrderTeamAsync(string.Concat(aa));

            };
            Player.ChallengeAction += async (player, rule) =>
            {
                if (rule.StartsWith("gen9vgc2023"))
                {

                    var team = await GetRandomTeam();
                    team.GamePokemons = team.GamePokemons.OrderBy(s => Random.Shared.Next()).ToList();
                    lock (_lockDb)
                    {
                        Player.ChangeYourTeamAsync(PSConverter.ConvertToPsOneLineAsync(team).Result).Wait();

                    }


                    await Player.AcceptChallengeAsync(player);
                }
            };
            Player.OnForceSwitch += (battle, bools) =>
            {
                // 这里被迫进行动作

            };

            Player.OnChooseMove += battle =>
            {

            };

            Player.BattleEndAction += async (PokePSCore.PsBattle battle, bool b) =>
            {
                finish = true; ;
                replayAnalysis.Remove(battle.Tag);
                await battle.LeaveRoomAsync();

            };

            Player.BattleStartAction += battle =>
            {
            };

            Player.BattleErrorAction += battle =>
            {

            };

            Player.RequestsAction += battle =>
            {

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

        private void OnLose()
        {
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

            await battle.SendMoveAsunc(actions.ToArray());
            await WaitRequests(battle);


           // 等待反馈结果
           // 要在这里等到下一轮




            throw new NotImplementedException();
        }

        private Task WaitRequests(PokePSCore.PsBattle battle)
        {
            // 如果error gg requests 过
            throw new NotImplementedException();
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
        public List<ChooseData> GetChooseDataFromAction(PSReplayAnalysis.PSReplayAnalysis replay, int action) {
            List<ChooseData> chooseDatas = new List<ChooseData>();

            int action1 = action % 22;
            int action2 = action / 22;
            throw new NotImplementedException();

        }

        public ChooseData GetChooseData(int action) {
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
            var team = await GetRandomTeam();
            team.GamePokemons = team.GamePokemons.OrderBy(s => Random.Shared.Next()).ToList();

            lock (_lockDb)
            {
              

                Player.ChangeYourTeamAsync(PSConverter.ConvertToPsOneLineAsync(team).Result).Wait();
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
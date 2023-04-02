using PSReplayAnalysis;
using System.Numerics;
using System.Text.Json;
using TorchSharp.Modules;
using static PSReplayAnalysis.ExporttoTrainData;
using static PSReplayAnalysis.PSReplayAnalysis;
using PokePSCore;
using PokeCommon.Models;

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
        public Dictionary<string, PSReplayAnalysis.PSReplayAnalysis> replayAnalysis { get; set; }
        // public Dictionary
        public PSClient Player;
        public PokeDanEnvPs()
        {
            
        }

        public async Task<GamePokemonTeam> GetRandomTeam()
        {
            throw new NotImplementedException();
        }

        public async Task Init(string name, string pwd)
        {
            Player = new PSClient(name, pwd, "ws://localhost:8000/showdown/websocket");
            await Player.ConnectAsync();
            await Task.Delay(1000);
            await Player.LoginAsync();
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

           // 等待反馈结果
           // 要在这里等到下一轮



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
    }

    public interface DQNEnv
    {
        (float[], int battleId) Reset();
        (float[], float, float) Step(int player);


    }
}
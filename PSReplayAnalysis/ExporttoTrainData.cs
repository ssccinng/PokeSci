using PokeCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PSReplayAnalysis.ExporttoTrainData;

namespace PSReplayAnalysis
{
    public static class ExporttoTrainData
    {
        // 将BattleData导出为DQN状态空间

        public static GamePokemonTeam[] ExportTeam(BattleData battleData)
        {
            GamePokemonTeam team = new GamePokemonTeam();
            var turn= battleData.BattleTurns[0];
            foreach (var item in turn.Player1Team.Pokemons)
            {
                new GamePokemon(new PokemonDataAccess.Models.Pokemon { });
            }

            throw new NotImplementedException();
        }
        public static float[] ExportBattleTurn(BattleTurn turn, int p)
        { 
            var ss = new float[8][];
            // 可能要有turn的观念

            // 默认无动作
            //ss[0] = new int[] { 0, -1, -1 };
            var team1Space = turn.Player1Team.Export();
            var team2Space = turn.Player2Team.Export();

            var side1Space = turn.Side1Pokes.SelectMany(s => s.Export());
            var side2Space = turn.Side2Pokes.SelectMany(s => s.Export());

            var side1Field = turn.Side1Field.Export();
            var side2Field = turn.Side2Field.Export();

            // 将team1Space side1Space side1Field 合并
            ss[1] = team1Space.Concat(side1Space).Concat(side1Field).ToArray();
            ss[2] = team2Space.Concat(side2Space).Concat(side2Field).ToArray();
            ss[3] = turn.AllField.Export();

            ss[4] = turn.Player1Team.ExportMove(0);
            ss[5] = turn.Player1Team.ExportMove(1);
            ss[6] = turn.Player2Team.ExportMove(0);
            ss[7] = turn.Player2Team.ExportMove(1);


            //Array.Resize(ref ss[3], 267);
            // 将ss数组拼接为一个数组
            if (p == 1)
            {
                return (new [] { ss[1], ss[4], ss[2], ss[7], ss[3]}).SelectMany(s => s).ToArray();
            }
            else
            {
                return (new[] { ss[2], ss[6], ss[1], ss[5], ss[3] }).SelectMany(s => s).ToArray();

            }
            //return ss[1..].SelectMany(s => s).ToArray();

        }
        //public static BattleTrainData ExportBattleData1(BattleData battleData)
        //{
        //    BattleTrainData battleTrainData = new();
        //    // 将BattleData的每一个BattleTurn转化为状态空间
        //    foreach (var turn in battleData.BattleTurns)
        //    {
        //        // 轮数也放进去
        //        var ss = new int[4][];

        //        // 默认无动作
        //        ss[0] = new int[] { 0, -1, -1 };
        //        var team1Space = turn.Player1Team.Export();
        //        var team2Space = turn.Player2Team.Export();

        //        var side1Space = turn.Side1Pokes.SelectMany(s => s.Export());
        //        var side2Space = turn.Side2Pokes.SelectMany(s => s.Export());

        //        var side1Field = turn.Side1Field.Export();
        //        var side2Field = turn.Side2Field.Export();

        //        // 将team1Space side1Space side1Field 合并
        //        ss[1] = team1Space;
        //        ss[2] = team2Space;

        //        battleTrainData.Player1Action.Add(turn.Battle1Actions.Select((s, i) => new int[] { s?.choose ?? -1, s?.target ?? -1, i }).Where(s => s[0] != -1).ToList());
        //        battleTrainData.Player2Action.Add(turn.Battle2Actions.Select((s, i) => new int[] { s?.choose ?? -1, s?.target ?? -1, i }).Where(s => s[0] != -1).ToList());
        //        //battleTrainData.Player2Action.Add(turn.Battle2Actions.ToList());

        //        battleTrainData.Reward1.Add(turn.Reward1);
        //        battleTrainData.Reward2.Add(turn.Reward2);


        //        battleTrainData.StateSpace.Add(ss);
        //        //Console.WriteLine("{0} {1} {2}", side1Space.Count(), side1Field.Length, ss[1].Length);
        //        //Console.WriteLine("{0} {1} {2} {3}", side1Space.Count(), ss[1].Length, ss[2].Length, ss[3].Length);

        //    }

        //    return battleTrainData;
        //}
        public static BattleTrainData ExportBattleDatav2(BattleData battleData)
        {
            BattleTrainData battleTrainData = new();
            // 将BattleData的每一个BattleTurn转化为状态空间
            foreach (var turn in battleData.BattleTurns)
            {
                // 轮数也放进去
                var ss = new float[8][];

                // 默认无动作
                ss[0] = new float[] { 0, -1, -1 };
                var team1Space = turn.Player1Team.Export();
                var team2Space = turn.Player2Team.Export();

                var side1Space = turn.Side1Pokes.SelectMany(s => s.Export());
                var side2Space = turn.Side2Pokes.SelectMany(s => s.Export());

                var side1Field = turn.Side1Field.Export();
                var side2Field = turn.Side2Field.Export();

                // 将team1Space side1Space side1Field 合并
                ss[1] = team1Space.Concat(side1Space).Concat(side1Field).ToArray();
                ss[2] = team2Space.Concat(side2Space).Concat(side2Field).ToArray();
                ss[3] = turn.AllField.Export();

                ss[4] = turn.Player1Team.ExportMove(0);
                ss[5] = turn.Player1Team.ExportMove(1);
                ss[6] = turn.Player2Team.ExportMove(0);
                ss[7] = turn.Player2Team.ExportMove(1);


                battleTrainData.Player1Action.Add(turn.Battle1Actions.Select((s, i) => new int[] { s?.choose ?? -1, s?.target ?? -1, i }).Where(s => s[0] != -1).ToList());
                battleTrainData.Player2Action.Add(turn.Battle2Actions.Select((s, i) => new int[] { s?.choose ?? -1, s?.target ?? -1, i }).Where(s => s[0] != -1).ToList());
                //battleTrainData.Player2Action.Add(turn.Battle2Actions.ToList());

                battleTrainData.Reward1.Add(turn.Reward1);
                battleTrainData.Reward2.Add(turn.Reward2);


                battleTrainData.StateSpace.Add(ss);
                //Console.WriteLine("{0} {1} {2}", side1Space.Count(), side1Field.Length, ss[1].Length);
                //Console.WriteLine("{0} {1} {2} {3}", side1Space.Count(), ss[1].Length, ss[2].Length, ss[3].Length);

            }

            return battleTrainData;
        }
        //public static BattleTrainData ExportBattleData(BattleData battleData)
        //{
        //    BattleTrainData battleTrainData = new();
        //    // 将BattleData的每一个BattleTurn转化为状态空间
        //    foreach (var turn in battleData.BattleTurns)
        //    {
        //        // 轮数也放进去
        //        var ss = new int[4][];

        //        // 默认无动作
        //        ss[0] = new int[] { 0, -1, -1 };
        //        var team1Space = turn.Player1Team.Export();
        //        var team2Space = turn.Player2Team.Export();

        //        var side1Space =  turn.Side1Pokes.SelectMany(s => s.Export());
        //        var side2Space = turn.Side2Pokes.SelectMany(s => s.Export());

        //        var side1Field = turn.Side1Field.Export();
        //        var side2Field = turn.Side2Field.Export();

        //        // 将team1Space side1Space side1Field 合并
        //        ss[1] = team1Space.Concat(side1Space).Concat(side1Field).ToArray();
        //        ss[2] = team2Space.Concat(side2Space).Concat(side2Field).ToArray();
        //        ss[3] = turn.AllField.Export();

        //        battleTrainData.Player1Action.Add(turn.Battle1Actions.Select((s, i) => new int[] { s?.choose ?? -1, s?.target ?? -1, i }).Where(s => s[0] != -1).ToList());
        //        battleTrainData.Player2Action.Add(turn.Battle2Actions.Select((s, i) => new int[] { s?.choose ?? -1, s?.target ?? -1, i }).Where(s => s[0] != -1).ToList());
        //        //battleTrainData.Player2Action.Add(turn.Battle2Actions.ToList());

        //        battleTrainData.Reward1.Add(turn.Reward1);
        //        battleTrainData.Reward2.Add(turn.Reward2);


        //        battleTrainData.StateSpace.Add(ss);
        //        //Console.WriteLine("{0} {1} {2}", side1Space.Count(), side1Field.Length, ss[1].Length);
        //        //Console.WriteLine("{0} {1} {2} {3}", side1Space.Count(), ss[1].Length, ss[2].Length, ss[3].Length);

        //    }

        //    return battleTrainData;
        //}

        public static List<BattleTrainData> ExportBattleData(IEnumerable<BattleData> battleData)
        {
            List<BattleTrainData> battleTrainDatas = new();
            foreach (var battle in battleData)
            {
                battleTrainDatas.Add(ExportBattleDatav2(battle));
            }
            return battleTrainDatas;
        }
        public record BattleAction(int choose, int target);
        public class BattleTrainData
        {

            public int BattleId { get; set; }
            public int WhoWin { get; set; }
            /// <summary>
            /// 选择动作 只能是回合开始动作, 怎么处理回合中的换人捏
            /// </summary>
            //public List<List<BattleAction>> Player1Action { get; set; } = new();
            //public List<List<BattleAction>> Player2Action { get; set; } = new();
            
            public List<List<int[]>> Player1Action { get; set; } = new();
            public List<List<int[]>> Player2Action { get; set; } = new();

            // 状态空间
            public List<float[][]> StateSpace { get; set; } = new();
            public List<float> Reward1 { get; set; } = new();
            public List<float> Reward2 { get; set; } = new();

            //public int[] GetStateSpaceTenser()
            //{
            //    r
            //}


        }
        public class RewardData
        {
            public int Source { get; set; }
            public object BattleTurnEvent { get; set; } = new { type = "damage" };
        }
    }
    public class BattleTurnEvent
    {
        public string Type { get; set; } = "damage"; // cant

    }
}

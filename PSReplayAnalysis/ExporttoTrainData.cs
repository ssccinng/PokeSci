using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSReplayAnalysis
{
    public static class ExporttoTrainData
    {
        // 将BattleData导出为DQN状态空间

        public static BattleTrainData ExportBattleData(BattleData battleData)
        {
            BattleTrainData battleTrainData = new();
            // 将BattleData的每一个BattleTurn转化为状态空间
            foreach (var turn in battleData.BattleTurns)
            {
                // 轮数也放进去
                var ss = new int[4][];

                // 默认无动作
                ss[0] = new int[] { 0, -1, -1 };
                var team1Space = turn.Player1Team.Export();
                var team2Space = turn.Player2Team.Export();

                var side1Space =  turn.Side1Pokes.SelectMany(s => s.Export());
                var side2Space = turn.Side2Pokes.SelectMany(s => s.Export());

                var side1Field = turn.Side1Field.Export();
                var side2Field = turn.Side2Field.Export();

                // 将team1Space side1Space side1Field 合并
                ss[1] = team1Space.Concat(side1Space).Concat(side1Field).ToArray();
                ss[2] = team2Space.Concat(side2Space).Concat(side2Field).ToArray();
                ss[3] = turn.AllField.Export();

                battleTrainData.Player1Action.Add(turn.Battle1Actions.Where(s => s != null).Select(s => new int[] { s.choose, s.target }).ToList());
                battleTrainData.Player2Action.Add(turn.Battle2Actions.Where(s => s != null).Select(s => new int[] { s.choose, s.target }).ToList());
                //battleTrainData.Player2Action.Add(turn.Battle2Actions.ToList());

                battleTrainData.Reward1.Add(turn.Reward1);
                battleTrainData.Reward2.Add(turn.Reward2);


                battleTrainData.StateSpace.Add(ss);
                //Console.WriteLine("{0} {1} {2}", side1Space.Count(), side1Field.Length, ss[1].Length);
                //Console.WriteLine("{0} {1} {2} {3}", side1Space.Count(), ss[1].Length, ss[2].Length, ss[3].Length);

            }

            return battleTrainData;
        }

        public static List<BattleTrainData> ExportBattleData(List<BattleData> battleData)
        {
            List<BattleTrainData> battleTrainDatas = new();
            foreach (var battle in battleData)
            {
                battleTrainDatas.Add(ExportBattleData(battle));
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
            public List<int[][]> StateSpace { get; set; } = new();
            public List<int> Reward1 { get; set; } = new();
            public List<int> Reward2 { get; set; } = new();


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

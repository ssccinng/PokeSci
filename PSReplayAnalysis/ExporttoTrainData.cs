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

            
            return battleTrainData;
        }

        public static void ExportBattleData(List<BattleData> battleData, string path)
        {
            List<BattleTrainData> battleTrainDatas = new();
            foreach (var battle in battleData)
            {
                battleTrainDatas.Add(ExportBattleData(battle));
            }
        }

        public class BattleTrainData
        {

            public int BattleId { get; set; }
            public int WhoWin { get; set; }
            /// <summary>
            /// 选择动作 只能是回合开始动作, 怎么处理回合中的换人捏
            /// </summary>
            public List<(int turn, int choose, int target)> Player1Action { get; set; } = new();
            public List<(int turn, int choose, int target)> Player2Action { get; set; } = new();

            // 状态空间
            public List<int[]> StateSpace { get; set; } = new();
            public List<RewardData> Reward { get; set; } = new();


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

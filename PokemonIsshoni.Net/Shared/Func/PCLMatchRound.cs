using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;


namespace PokemonIsshoni.Net.Shared.Models
{
    public partial class PCLMatchRound
    {
        /// <summary>
        /// 个人赛瑞士轮胜率计算
        /// </summary>
        public void SwissRatioCalc()
        {
            if (PCLRoundType != RoundType.Swiss) return;
            if (PCLRoundPlayers is null) return;
            //PCLRoundPlayers.tos
            var playerDict = PCLRoundPlayers.ToDictionary(s => s.UserId);
            //Dictionary<playerdict.GetType(), int> a;
            foreach (PCLRoundPlayer pclRoundPlayer in PCLRoundPlayers)
            {
                // 要不要加入本体胜率
                //Parallel.ForEach(pclMatchPlayer.PCLBattles,
                //    (battle) =>
                //    {
                //    });
                if (pclRoundPlayer.PCLBattles is null) continue;
                decimal oppRatio = 0, oppoppRatio = 0;
                int oppCnt = pclRoundPlayer.PCLBattles.Count, oppoppCnt = 0;
                foreach (PCLBattle battle in pclRoundPlayer.PCLBattles)
                {
                    // 权宜之计
                    var OppRoundData = playerDict[battle.GetOppUserId(pclRoundPlayer.UserId)];
                    // 改成三元表达式（？
                    oppRatio += Math.Max(0.25m, OppRoundData switch
                    {
                        var x when x.IsDrop => Math.Min(0.75m, x.Ratio),
                        _ => OppRoundData.Ratio,
                    });
                    // 可能还是分开加 存在battle为空的可能性（？
                    oppoppCnt += OppRoundData.PCLBattles.Count;
                    // 计算对手的对手胜率
                    foreach (PCLBattle oppbattle in OppRoundData.PCLBattles)
                    {
                        // 权宜之计
                        var OppOppRoundData = playerDict[battle.GetOppUserId(OppRoundData.UserId)];
                        // 改成三元表达式（？
                        oppoppRatio += Math.Max(0.25m, OppOppRoundData switch
                        {
                            var x when x.IsDrop => Math.Min(0.75m, x.Ratio),
                            _ => OppRoundData.Ratio,
                        });
                    }

                }
            }
            //PCLRoundPlayers = PCLRoundPlayers.OrderByDescending(s => s.Score)
            //                                 .ThenByDescending(s => s.OppRatio)
            //                                 .ThenByDescending(s => s.OppOppRatio)
            //                                 .ToList();
            var rankTemp = PCLRoundPlayers.OrderByDescending(s => s.Score)
                                      .ThenByDescending(s => s.OppRatio)
                                      .ThenByDescending(s => s.OppOppRatio);
            int rank = 1;
            foreach (var item in rankTemp)
            {
                item.Rank = rank++;
            }
            //for (int i = 0; i < PCLRoundPlayers.Count; ++i)
            //{
            //    PCLRoundPlayers[i].Rank = i + 1;
            //}

        }
        // 利用swissidx 计算得出下一轮
        public List<PCLBattle> GenNextSwissRoundBattes()
        {
            List<PCLBattle> newBattleList = new();
            // 需要吗
            var rankTemp = PCLRoundPlayers.OrderByDescending(s => s.Score)
                                          .ThenByDescending(s => s.OppRatio)
                                          .ThenByDescending(s => s.OppOppRatio)
                                          .ToList();

            bool[] visit = new bool[rankTemp.Count];
            for (int i = 0; i < rankTemp.Count; ++i)
            {
                if (visit[i]) continue;
                for (int j = 0; j < rankTemp.Count; ++j)
                {
                    if (visit[j] || i == j)
                    {
                        continue;
                    }
                    // 看看他们有没有打过
                    rankTemp[i].PCLBattles.Any();
                }
            }
            //var playerList = 
            return null;
        }
    }
}

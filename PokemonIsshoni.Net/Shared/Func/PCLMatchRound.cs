namespace PokemonIsshoni.Net.Shared.Models
{
    public partial class PCLMatchRound
    {
        /// <summary>
        /// 个人赛瑞士轮胜率计算
        /// </summary>
        public bool SwissRatioCalc()
        {
            if (PCLRoundType != RoundType.Swiss) return false;
            if (PCLRoundPlayers is null) return false;
            if (PCLBattles is null) return false;

            //PCLRoundPlayers.tos
            var playerDict = PCLRoundPlayers.ToDictionary(s => s.UserId);
            foreach (PCLRoundPlayer pclRoundPlayer in PCLRoundPlayers)
            {
                pclRoundPlayer.PCLBattles = PCLBattles
                    .Where(s =>
                    s.Player1Id == pclRoundPlayer.UserId ||
                    s.Player2Id == pclRoundPlayer.UserId)
                    .ToList();
            }
            // 先维护胜率 才能维护对手胜率和对手的对手胜率
            //Dictionary<playerdict.GetType(), int> a;
            foreach (PCLRoundPlayer pclRoundPlayer in PCLRoundPlayers)
            {
                // 要不要加入本体胜率
                //Parallel.ForEach(pclMatchPlayer.PCLBattles,
                //    (battle) =>
                //    {
                //    });
                //pclRoundPlayer.PCLBattles = PCLBattles
                //    .Where(s => 
                //    s.Player1Id == pclRoundPlayer.UserId || 
                //    s.Player2Id == pclRoundPlayer.UserId)
                //    .ToList();
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
                            _ => OppOppRoundData.Ratio,
                        });
                    }

                }
                if (oppCnt > 0)
                {
                    pclRoundPlayer.OppRatio = oppRatio / oppCnt;
                    pclRoundPlayer.OppOppRatio = oppoppRatio / oppoppCnt;

                }
                else
                {
                    //pclRoundPlayer.OppRatio = oppRatio / oppCnt;

                }

                // then SaveChanges
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
            return true;

        }
        // 利用swissidx 计算得出下一轮
        public List<PCLBattle> GenNextSwissRoundBattes()
        {
            // 需要验证传入的id
            Random rnd = new Random();
            if (PCLRoundType != RoundType.Swiss) return null;
            // 未在运行
            if (PCLRoundState != RoundState.Running) return null;
            // 需要带入对战信息
            if (PCLBattles == null) return null;

            List<PCLBattle> newBattleList = new();
            SwissRatioCalc();
            // 需要吗
            var rankTemp = PCLRoundPlayers.Where(s => !s.IsDrop)
                                          .OrderByDescending(s => s.Score)
                                          .ThenBy(s => rnd.Next(2048))
                                          //.ThenByDescending(s => s.OppRatio)
                                          //.ThenByDescending(s => s.OppOppRatio)
                                          .ToList();
            // 已经结束了
            // 考虑一下id
            if (Swissidx >= SwissCount)
            {
                return null;
            }
            for (int gid = 0; gid < (IsGroup ? GroupCnt : 1); gid++)
            {
                // 遍历分组
                int tableId = 1;
                var groupPlayer = rankTemp.Where(s => s.GroupId == gid).ToList();
                bool[] visit = new bool[groupPlayer.Count + 50];

                for (int i = 0; i < groupPlayer.Count; ++i)
                {
                    if (visit[i]) continue;
                    visit[i] = true;
                    // 是否成功匹配
                    bool succ = false;
                    // j为想要对战的对手编号
                    // 这种算法会优先向后找
                    for (int j = 0; j < groupPlayer.Count; ++j)
                    {
                        // 对手打过了 或者自己不能和自己打
                        if (visit[j] || i == j)
                        {
                            continue;
                        }
                        // 看看他们有没有打过
                        var hadBattle = PCLBattles.Any(s =>
                            (s.Player1Id == groupPlayer[i].UserId && s.Player2Id == groupPlayer[j].UserId) ||
                            (s.Player1Id == groupPlayer[j].UserId && s.Player2Id == groupPlayer[i].UserId)
                        //var hadBattle = groupPlayer[i].PCLBattles.Any(s =>
                        //    (s.Player1Id == groupPlayer[i].UserId && s.Player2Id == groupPlayer[j].UserId) ||
                        //    (s.Player1Id == groupPlayer[j].UserId && s.Player2Id == groupPlayer[i].UserId)
                        );
                        // 如果没有对战过 则匹配成功
                        if (!hadBattle)
                        {
                            // 匹配成功 修改tag
                            succ = true;
                            visit[j] = true;
                            newBattleList.Add(
                                new PCLBattle
                                {
                                    Player1Id = groupPlayer[i].UserId,
                                    Player2Id = groupPlayer[j].UserId,
                                    GroupId = groupPlayer[i].GroupId,
                                    BO = BO,
                                    PCLMatchId = PCLMatchId,
                                    Player1TeamId = groupPlayer[i].BattleTeamId,
                                    Player2TeamId = groupPlayer[j].BattleTeamId,
                                    SwissRoundIdx = Swissidx,
                                    Tag = tableId++
                                }
                                );
                            // 成功匹配 跳出循环
                            break;
                        }

                    }

                    if (!succ)
                    {
                        Console.WriteLine($"已经统计过{visit.Count(s => s)}");

                        // 如果自己已经轮空过了
                        if (groupPlayer[i].HasBye || visit.Count(s => s) == groupPlayer.Count - 1)
                        {
                            // 这个count - 1 似乎是保证只剩两个人一定触发该机制 因为正在搜索的人visit已经被置true  所以-1就够
                            Console.WriteLine("不可避免的出现了轮空，运行补丁代码");
                            bool qiangDuo = false;
                            for (int k = i - 1; k >= 0; --k)
                            {
                                if (!groupPlayer[k].HasBye)
                                {
                                    // 没有轮空过，抢他对手
                                    // 当然 依然要确定之前是否打过
                                    var bt = newBattleList.FirstOrDefault(s => s.Player1Id == groupPlayer[k].UserId || s.Player2Id == groupPlayer[k].UserId);
                                    if (bt == null) continue;
                                    var oppId = bt.GetOppUserId(groupPlayer[k].UserId);
                                    var hadBattle = PCLBattles.Any(s =>
                                          (s.Player1Id == groupPlayer[i].UserId && s.Player2Id == oppId) ||
                                          (s.Player1Id == oppId && s.Player2Id == groupPlayer[i].UserId)
                                      );
                                    if (hadBattle)
                                    {
                                        // 打过了 抢了也没用 啊不
                                        continue;
                                    }
                                    // 抢夺完毕
                                    if (bt.Player1Id == oppId)
                                    {
                                        bt.Player2Id = groupPlayer[i].UserId;
                                        bt.Player2TeamId = groupPlayer[i].BattleTeamId;
                                    }
                                    else
                                    {
                                        bt.Player1Id = groupPlayer[i].UserId;
                                        bt.Player1TeamId = groupPlayer[i].BattleTeamId;
                                    }

                                    qiangDuo = true;
                                    // 进入假轮空 从头开始搜， 这里其实会不会 用队列更好 或者优先队列
                                    visit[k] = false;
                                    i = -1;
                                    break;
                                }


                            }
                            if (!qiangDuo)
                            {
                                // 没办法 二次轮空了只能
                                groupPlayer[i].Score += 3;
                                groupPlayer[i].Win++;
                                // 为什么没设置轮空
                            }

                        }
                        else
                        {
                            groupPlayer[i].Score += 3;
                            groupPlayer[i].Win++;
                            groupPlayer[i].HasBye = true;
                            // 受不了 直接轮空
                            Console.WriteLine("没办法 只能轮空了");
                        }
                    }

                }
                // 遍历一下 确保无误
                for (int i = 0; i < groupPlayer.Count; ++i)
                {
                    if (!visit[i])
                    {
                        // 所有没有visit过的 直接投降
                        groupPlayer[i].Win++;
                        groupPlayer[i].Score += 3;
                        groupPlayer[i].HasBye = true;
                    }
                }
            }
            return newBattleList;
        }
    }
}

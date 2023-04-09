using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokePSCore;

namespace DQNTorch;
// ai对战天梯
// 让ai自己对战，得出排位分
public class PokeDanLadder
{
    public NewZQDEnv[] Agents;
    public PokeDanLadder(int agent_num)
    {
        Agents = new NewZQDEnv[agent_num];
        for (int i = 0; i < agent_num; i++)
        {
            Agents[i] = new NewZQDEnv(new NewZQDQNAgent());
        }
    }

    public async Task train(int episode, int max_steps = 100, float epsilon_start = 1.0f,
        float epsilon_end = 0.1f, float epsilon_decay = 0.99f, int target_update = 10)
    {
        List<Task> tasks = new List<Task>();
        for (int i = 0; i < Agents.Length; i++)
        {
            tasks.Add(Agents[i].Init($"ZQD{i:00000}", ""));
        }
        Task.WaitAll(tasks.ToArray());
        tasks.Clear();

        for (int i = 0; i < episode; i++)
        {
            // 随机匹配Agents中的ai 两两进行对战

            Agents = Agents.OrderBy(x => Guid.NewGuid()).ToArray();
            for (int j = 0; j < Agents.Length; j += 2)
            {
                var a1 = Agents[j];
                var a2 = Agents[j + 1];
                tasks.Add(duel(a1, a2));

            }
            Task.WaitAll(tasks.ToArray()); tasks.Clear();
            await Task.Delay(1000);

            for (int j = 0; j < Agents.Length; j++)
            {
                Agents[j].epsilon *= epsilon_decay;
                Agents[j].epsilon = Math.Max(Agents[j].epsilon, epsilon_end);

                // Agents[j].agent.learn();
            }
            if (i % target_update == 0)
            {
                for (int j = 0; j < Agents.Length; j++)
                {
                    // 更新target模型
                    Agents[j].agent.update_target_model();
                }
            }

            // 输出一下排位分

            Agents = Agents.OrderByDescending(s => s.agent.LadderSocre).ToArray();
            for (int j = 0; j < Agents.Length; j++)
            {
                //Agents[j].agent.learn();
                Console.WriteLine($"{j + 1} {Agents[j].agent.PSClient.UserName} {Agents[j].agent.LadderSocre}");
            }

            //for (int j = 0; j < Agents.Length; j++)
            //{
            //    Agents[j].agent.model.save($"{Agents[j].agent.PSClient.UserName}.{i + 1}.dat");
            //}
            // 还要更新TargetModel
            //for (int j = 0; j < Agents.Length; j++)
            //{
            //    Agents[j].agent.model.save($"{Agents[j].agent.PSClient.UserName}.{i + 1}.dat");
            //}
            if (i % 100 == 99)
            {
                for (int j = 0; j < Agents.Length; j++)
                {
                    Agents[j].agent.model.save($"{Agents[j].agent.PSClient.UserName}.{i + 1}.dat");
                }
                Console.WriteLine($"episode {i} finished");
                // 淘汰

                //Agents = Agents.OrderByDescending(s => s.agent.LadderSocre).ToArray();
                for (int j = 24; j < 32; j++)
                {
                    var rnd = Random.Shared.Next(24);
                    if (j == 24) rnd = 0;
                    // 随机更新模型
                    var lastPSC = Agents[j].agent.PSClient;
                    Agents[j].agent = new NewZQDQNAgent($"{Agents[rnd].agent.PSClient.UserName}.{i + 1}.dat");
                    Agents[j].agent.PSClient = lastPSC;
                    Agents[j].agent.LadderSocre = 1000;
                }
                // 淘汰一批 复制一批
            }
 
        }
    }

    // 两个ai对战
    public async Task duel(NewZQDEnv a1, NewZQDEnv a2)
    {
        await a1.CreateBattleAsync(a2.PSClient.UserName);

        // 等待对战结束
        var res = await a1.WaitEnd();
        await a2.WaitEnd();
        var delta = a1.agent.LadderSocre - a2.agent.LadderSocre;
        

        // 根据胜负情况以及双方分数，调整ai的排位分
        if (res == PSReplayAnalysis.BattleResult.Player1Win) {
            a1.agent.LadderSocre += Math.Min(31, Math.Max(1, 16.0f - delta / 25));
            a2.agent.LadderSocre -= Math.Min(31, Math.Max(1, 16.0f - delta / 25));
        }
        else if (res == PSReplayAnalysis.BattleResult.Player2Win) {
            a1.agent.LadderSocre -= Math.Min(31, Math.Max(1, 16.0f + delta / 25));
            a2.agent.LadderSocre += Math.Min(31, Math.Max(1, 16.0f + delta / 25));
        }
        // 根据分数，调整ai的排位分




        // 两个ai对战
        // 一局结束后，根据胜负情况，调整ai的排位分
        // 一局结束后，根据胜负情况，调整ai的模型参数
    }
    /// <summary>
    /// 保存所有ai模型
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    internal void SaveAll()
    {
        for (int i = 0; i < Agents.Length; i++)
        {
            Agents[i].agent.model.save($"{Agents[i].agent.PSClient.UserName}.dat");
        }
    }
}

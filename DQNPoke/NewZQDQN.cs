﻿using NumSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PokePSCore;

using TorchSharp;
using TorchSharp.Modules;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;
using static TorchSharp.torch.nn.functional;

namespace DQNTorch;


/// <summary>
/// 新ZQDN网络
/// </summary>
public class NewZQDQN : Module
{
    private readonly int state_Size;
    private readonly int action_Size;
    private readonly int hidden_Size;
    private readonly Linear fc1;
    private readonly Linear fc2;
    private readonly Linear fc3;
    private readonly Linear fc4;

    public NewZQDQN(int state_size, int action_size, int hidden_size) : base("NewZQDN")
    {
        var device = torch.device(cuda.is_available() ? "cuda" : "cpu");
        state_Size = state_size;
        action_Size = action_size;
        hidden_Size = hidden_size;

        fc1 = Linear(state_size, hidden_size).to(device);
        fc2 = Linear(hidden_size, hidden_size / 2).to(device);
        fc3 = Linear(hidden_size / 2, hidden_size / 4).to(device);
        fc4 = Linear(hidden_size / 4, action_size).to(device);
            RegisterComponents();
    }

    public Tensor forward(Tensor x)
    {
        x = relu(fc1.forward(x));
        x = relu(fc2.forward(x));
        x = relu(fc3.forward(x));
        return fc4.forward(x);
    }
}


public class NewZQDQNAgent
{
    // 排位分
    public float LadderSocre = 1000;

    // PS客户端
    public PSClient PSClient { get; set; }
    // ai类型 纯攻击ai用于干扰环境 防止摆烂
    public AgentType AgentType { get; set; } = AgentType.Normal;
    private const int State_size = 1405;
    // 可能需要双环境
    private readonly int buffer_Size;
    private readonly int batch_Size;
    private readonly float gamma;
    private readonly float lr;
    private readonly List<(float[] states, long actions, float rewards, float[] next_states, float dones)> buffer;
    public readonly NewZQDQN model;
    public readonly NewZQDQN target_model;
    private readonly Adam optimizer;
    private Device device;
    private object _lockBuf = new();
    private static object _lockLearn = new();
    private const int single_action_size = 16;

    public NewZQDQNAgent(int battle_num = 1,
                      int buffer_size = 10000,
                      int batch_size = 32,
                      float gamma = 0.99f,
                      float lr = 0.001f)
    {
        device = torch.device(cuda.is_available() ? "cuda" : "cpu");
        buffer_Size = buffer_size;
        batch_Size = batch_size;
        this.gamma = gamma;
        this.lr = lr;

        buffer = new();
        model = new NewZQDQN(State_size, single_action_size * single_action_size, 512).to(device);
        target_model = new NewZQDQN(State_size, single_action_size * single_action_size, 512).to(device);
        optimizer = optim.Adam(model.parameters(), lr);
    }

    public NewZQDQNAgent(string modelPath, int battle_num = 1,
                      int buffer_size = 10000,
                      int batch_size = 32,
                      float gamma = 0.99f,
                      float lr = 0.001f)
    {
        device = torch.device(cuda.is_available() ? "cuda" : "cpu");
        buffer_Size = buffer_size;
        batch_Size = batch_size;
        this.gamma = gamma;
        this.lr = lr;

        buffer = new();
        model = new NewZQDQN(State_size, single_action_size * single_action_size, 512).to(device);
        model.load(modelPath);
        target_model = new NewZQDQN(State_size, single_action_size * single_action_size, 512).to(device);
        target_model.load(modelPath);

        optimizer = optim.Adam(model.parameters(), lr);
    }

    /// <summary>
    /// 根据状态做出动作
    /// </summary>
    /// <param name="states">当前状态</param>
    /// <param name="pos">宝可梦位置 取值0，1</param>
    /// <param name="epsilon"></param>
    /// <param name="banActions">封禁操作</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public int act(float[] state, double epsilon = 0.1f, IEnumerable<int>? banActions = null, IEnumerable<int>? banActions2 = null)

    {
        banActions ??= Array.Empty<int>();
        banActions2 ??= Array.Empty<int>();
        if (Random.Shared.NextDouble() < epsilon)
        {
            while (true)
            {
                var res = Random.Shared.Next(single_action_size * single_action_size);
                var res1 = res % 16;
                var res2 = res / 16;
                if (!banActions.Contains(res1) && !banActions2.Contains(res2))
                {
                    return res;
                }
            }
        }
        else
        {
            var states = FloatTensor(state).unsqueeze(0).to(device);

            using var a = no_grad();
            var q_values = model.forward(states);
            foreach (var item in banActions)
            {
                for (int i = 0; i < 16; i++)
                {
                    q_values[0][item + 16 * i] = float.MinValue;
                }
            }

            foreach (var item in banActions2)
            {
                for (int i = 0; i < 16; i++)
                {
                    q_values[0][item * 16 + i] = float.MinValue;

                }
            }
            var action = argmax(q_values, 1).cpu().item<long>();

            return (int)action;
        }
    }
    /// <summary>
    /// 返回换人结果
    /// </summary>
    /// <param name="states"></param>
    /// <param name="pos"></param>
    /// <param name="epsilon"></param>
    /// <param name="banActions"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public int actSwitch(float[] state, int pos, double epsilon = 0.1f, IEnumerable<int>? banActions = null)
    {
        banActions ??= Array.Empty<int>();
        //if (np.random.rand(1)[0].GetDouble() < epsilon)
        if (Random.Shared.NextDouble() < epsilon)
        {
            while (true)
            {
                //var res = np.random.randint(0, 6).GetInt32();
                var res  = Random.Shared.Next(6);

                if (!banActions.Contains(res))
                {
                    return res;
                }
            }
        }
        else
        {
            var states = FloatTensor(state).unsqueeze(0).to(device);
            using var a = no_grad();

            var q_values = model.forward(states);
            q_values = q_values.slice(1, pos * 22, 6 + pos * 22, 1);
            foreach (var item in banActions)
            {
                q_values[0][item] = float.MinValue;
            }
            var action = argmax(q_values, 1).cpu().item<long>();
            return (int)action;
        }
    }
    /// <summary>
    /// 学习
    /// </summary>
    public void learn()
    {
        if (buffer.Count < batch_Size)
        {
            return;
        }
        var samples = np.random.choice(buffer.Count, new Shape(batch_Size), replace: false);
        var bufferTuples = samples.ToArray<int>().Select(s => buffer[s]).ToArray();
        var states = from_array(GetMu(bufferTuples.Select(t => t.Item1).ToArray())).to(this.device);
        var actions = from_array(bufferTuples.Select(t => t.Item2).ToArray()).unsqueeze(1).to(this.device);
        var rewards = from_array(bufferTuples.Select(t => t.Item3).ToArray(), dtype: ScalarType.Float32).unsqueeze(1).to(this.device);
        var nextStates = from_array(GetMu(bufferTuples.Select(t => t.Item4).ToArray()), dtype: ScalarType.Float32).to(this.device);
        var dones = from_array(bufferTuples.Select(t => t.Item5).ToArray(), dtype: ScalarType.Float32).unsqueeze(1).to(this.device);

        // 计算损失函数
        var qValues = this.model.forward(states).gather(1, actions);
        var nextQValues = this.target_model.forward(nextStates).detach().max(1).values.unsqueeze(1);
        var targetQValues = rewards + (1 - dones) * this.gamma * nextQValues;
        var loss = nn.functional.mse_loss(qValues, targetQValues);
        // 反向传播更新网络参数
        this.optimizer.zero_grad();
        //torch.a
        //torch.autograd.
        loss.backward();
        this.optimizer.step();
    }
    // 获取多维数组
    public T[,] GetMu<T>(T[][] floats)
    {
        int x = floats.Length;
        int y = floats[0].Length;
        T[,] result = new T[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                result[i, j] = floats[i][j];
            }
        }
        return result;
    }


    //public async Task train(int episodes, int max_steps = 100, float epsilon_start = 1.0f,
    //    float epsilon_end = 0.1f, float epsilon_decay = 0.99f, int target_update = 10)
    //{
    //    //Console.
    //    List<Task> tasks = new List<Task>();
    //    for (int i = 0; i < Envs.Length; i++)
    //    {
    //        tasks.Add(Envs[i].Init($"ZQD{i:00000}", ""));
    //    }
    //    foreach (var item in tasks)
    //    {
    //        await item;
    //    }
    //    tasks.Clear();
    //    var epsilon = epsilon_start;

    //    for (int episode = 0; episode < episodes; episode++)
    //    {

    //        for (int gg = 0; gg < Envs.Length / 2; gg++)
    //        {
    //            int g = gg;

    //            tasks.Add(Task.Run(async () =>
    //            {
    //                Envs[2 * g].epsilon = Envs[2 * g + 1].epsilon = epsilon
    //                ;
    //                await Envs[2 * g].CreateBattleAsync(Envs[g * 2 + 1].PSClient.UserName);

    //                await Envs[2 * g].WaitEnd();
    //                await Envs[g * 2 + 1].WaitEnd();
    //            }));
    //        }
    //        foreach (var item in tasks)
    //        {
    //            await item;
    //        }
    //        tasks.Clear();

    //        if (episode % 100 == 99)
    //        {
    //            model.save($"temp2.{episode + 1}.data");

    //        }
    //        if (episode % 100 == 0)
    //        {
    //            Console.WriteLine($"Episode {episode}");
    //        }

    //        // 更新Target网络
    //        if (episode % target_update == 0)
    //        {
    //            //var aa = this.model.to("cpu").state_dict();
    //            model.save("temp");

    //            target_model.load("temp");
    //        }

    //        // 更新探索率
    //        if (epsilon > epsilon_end)
    //        {
    //            epsilon *= epsilon_decay;
    //        }
    //    }
        
    //}


    // 更新Target网络
    public void update_target_model()
    {
        model.save($"{PSClient.UserName}temp");

        this.target_model.load($"{PSClient.UserName}temp");
        //target_model = target_model.cuda();
        //model.save($"{PSClient.UserName}temp");
        //target_model.load($"{PSClient.UserName}temp");
    }

    [Obsolete]
    public void AddBuffer((float[] states, long actions, float rewards, float[] next_states, float dones) data)
    {
        if (data.states == null || data.next_states == null) return;
        lock (_lockBuf)
        {
            buffer.Add(data);
            if (buffer.Count > buffer_Size)
                buffer.RemoveAt(0);
        }
    }

    public void ClearBuffer()
    {
        lock (_lockBuf)
        {
            buffer.Clear();
        }
    }
    public void AddBuffers(IEnumerable<(float[] states, long actions, float rewards, float[] next_states, float dones)> datas)
    {
        lock (_lockBuf)
        {
            buffer.AddRange(datas);
            //while (buffer.Count > buffer_Size)
            if (buffer.Count > buffer_Size)
                buffer.RemoveRange(0, buffer.Count - buffer_Size);
        }

        lock (_lockLearn)
        {
            learn();
        }
        // 这里一波推

    }

}
using TorchSharp;
using TorchSharp.Modules;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;
using static TorchSharp.torch.nn.functional;
using static TorchSharp.torch.optim;

using NumSharp;
using static Tensorboard.TensorShapeProto.Types;
using System;
using System.Net;
using System.Security.Policy;
using static TorchSharp.torch.nn.utils;
using PokePSCore;

namespace DQNTorch
{
    public class DQN: Module
    {
        private int state_size;
        private int action_size;
        private int hidden_size;
        private Linear fc1;
        private Linear fc3;
        private Linear fc2;
        public DQN(int state_size, int action_size, int hidden_size) : base("test")
        {
            this.state_size = state_size;
            this.action_size = action_size;
            this.hidden_size = hidden_size;
            //fc1 = Linear(state_size, hidden_size);
            //fc2 = Linear(hidden_size, hidden_size);
            //fc3 = Linear(hidden_size, action_size);  
            
            //fc1 = Linear(state_size, 512);
            //fc2 = Linear(512, 256);
            //fc3 = Linear(256, action_size);


            fc1 = Linear(state_size, 256);
            fc2 = Linear(256, hidden_size);
            fc3 = Linear(hidden_size, action_size);
            RegisterComponents();
        }
        public torch.Tensor forward(torch.Tensor x)
        {
            x = relu(fc1.forward(x));
            x = relu(fc2.forward(x));
            return fc3.forward(x);
        }
    }

    public class DQNAgent
    {
        public PokeDanEnvPs Env;
        public PokeDanEnvTest Env2;
        // 可能需要双环境
        public PokeDanEnvPs Env1;
        private readonly int buffer_Size;
        private readonly int batch_Size;
        private readonly float gamma;
        private readonly float lr;
        private readonly List<(float[] states, long actions, float rewards, float[] next_states, float dones)> buffer;
        public readonly DQN model;
        private readonly DQN target_model;
        private readonly Adam optimizer;
        private torch.Device device;

        public DQNAgent(int buffer_size = 10000, int batch_size=32, float gamma = 0.99f, float lr = 0.001f)
        {
            device = torch.device( torch.cuda.is_available() ? "cuda" : "cpu");

            //Env1 = env1;
            buffer_Size = buffer_size;
            batch_Size = batch_size;
            this.gamma = gamma;
            this.lr = lr;
            buffer = new();
            model = new DQN(1401, 44, 128).to(device);
            target_model = new DQN(1401, 44, 128).to(device);

            optimizer = optim.Adam(model.parameters(), lr);
        }

        public int[][] select_action(int battleId, int player)
        {
            return Env2.GetAction(battleId, player);

        }

        // 选择一个动作 但要考虑 可能可以一个随机一个不随机
        public long actSwitch(float[] state, int pos, double epsilon = 0.1f, params int[] banactions)
        {
            if (np.random.rand(1)[0].GetDouble() < epsilon)
            {

                //return Random.Shared.Next(6);
                while (true)
                {
                    var res = np.random.randint(0, 4).GetInt32();
                    if (!banactions.Contains(res))
                    {
                        return res;
                    }
                }
                // 随机
            }
            else
            {
                var states = FloatTensor(state).unsqueeze(0).to(device);
                var q_values = model.forward(states);
                q_values = q_values.slice(1, pos * 22, 6 + pos * 22, 1);
                foreach (var item in banactions)
                {
                    //if (item < 6)
                        q_values[0][item] = float.MinValue;
                }
                // 这个item是什么
                //var action1 = argmax(q_values.slice(1, pos * 22, 22 + pos * 22, 1), 1);
                var action = argmax(q_values, 1).cpu().item<long>();
                //using var aa = torch.autograd.set_detect_anomaly(true);
                return action;
            }
            ;
        }
        public long act(float[] state, int pos, double epsilon = 0.1f, params int[] banactions)
        {
            if (np.random.rand(1)[0].GetDouble() < epsilon)
            {

                //return Random.Shared.Next(22) + 22 * pos;
                while (true)
                {
                    var res = np.random.randint(0, 22).GetInt32();
                    if (!banactions.Contains(res))
                    {
                        return res;
                    }
                }

                // 随机
            }
            else
            {
                var states = FloatTensor(state).unsqueeze(0).to(device);
                var q_values = model.forward(states);
                q_values = q_values.slice(1, pos * 22, 22 + pos * 22, 1);
                foreach (var item in banactions)
                {
                  
                        q_values[0][item] = float.MinValue;
                }
                // 这个item是什么
                //var action1 = argmax(q_values.slice(1, pos * 22, 22 + pos * 22, 1), 1);
                var action = argmax(q_values, 1).cpu().item<long>();
                //using var aa = torch.autograd.set_detect_anomaly(true);
                return action;
            }
            return 0
            ;
        }
        // 生成一个方法 将float[][]转为float[,]
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



        public void learn()
        {
            if (buffer.Count < batch_Size)
            {
                return;
            }
            var samples = np.random.choice(buffer.Count, new Shape(batch_Size), replace: false);
            var bufferTuples = samples.ToArray<int>().Select(s => buffer[s]).ToArray();
            //var states = tupleList.Select(t => t.Item1).ToList();
            //var actions = tupleList.Select(t => t.Item2).ToList();
            //var rewards = tupleList.Select(t => t.Item3).ToList();
            //var next_states = tupleList.Select(t => t.Item4).ToList();
            //var dones = tupleList.Select(t => t.Item5).ToList();

            //var q_values = model.forward(states).gather(1, actions);

            //var sampleIndices = new Random().Sample(Enumerable.Range(0, this.buffer.Count), this.batchSize);
            //var bufferTuples = sampleIndices.Select(i => this.buffer[i]).ToList();

            // 将元组中的各个元素分别赋值给不同的变量，并转换为张量

            // 生成一个从float[][]转到Tenser的方法
            var states = from_array(GetMu( bufferTuples.Select(t => t.Item1).ToArray())).to(this.device);
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
        public async Task train1(int episodes, int max_steps = 1000, float epsilon_start = 1.0f,
        float epsilon_end = 0.1f, float epsilon_decay = 0.999f, int target_update = 10)
        {
            Env = new PokeDanEnvPs(this);
            await Env.Init("scixing", "11998whs");
            Env1 = new PokeDanEnvPs(this);
            await Env1.Init("longkui", "11998whs");

            PSClient ob = new PSClient("kribyRBP", "11998whs", "ws://localhost:8000/showdown/websocket");

            //await  ob.ConnectAsync();
            //Task.Delay(1000);
            //await ob.LoginAsync();
            var epsilon = epsilon_start;

            // 训练循环
            for (int episode = 0; episode < episodes; episode++)
            {
                Env.epsilon = Env1.epsilon = epsilon;
                await Env.Challenage(Env1);
                if (episode % 10 == 0)
                {
                    //await ob.SendJoinAsync(Env.replayAnalysis.Keys.Last());
                }
                //CreateBattle(Env, Env1, epsilon);
                await Env.WaitEnd();
                await Env1.WaitEnd();

                float episodeReward = 0.0f;
                float[] nextState = null;
                learn();
                // 输出训练结果
                if (episode % 100 == 99)
                {
                    model.save($"temp.{episode+1}.data");

                }
                if (episode % 100 == 0)
                    {
                    Console.WriteLine($"Episode {episode}, Reward {episodeReward}");
                }

                // 更新Target网络
                if (episode % target_update == 0)
                {
                    //var aa = this.model.to("cpu").state_dict();
                    model.save("temp");

                    this.target_model.load("temp");
                }

                // 更新探索率
                if (epsilon > epsilon_end)
                {
                    epsilon *= epsilon_decay;
                }
            }
        }
        public void AddBuffer((float[] states, long actions, float rewards, float[] next_states, float dones) data)
        {
            buffer.Add(data);
            if (buffer.Count > buffer_Size) buffer.RemoveAt(0);

        }
        private void CreateBattle(PokeDanEnvPs env, PokeDanEnvPs env1, float epsilon)
        {
            throw new NotImplementedException();
        }

        private void CreateBattle(PokeDanEnvPs env, PokeDanEnvPs env1)
        {
            throw new NotImplementedException();
        }
        public void train(int episodes, int max_steps = 1000, float epsilon_start = 1.0f,
           float epsilon_end = 0.1f, float epsilon_decay = 0.99f, int target_update = 10)
        {
            var epsilon = epsilon_start;

            // 训练循环
            for (int episode = 0; episode < episodes; episode++)
            {
                (float[], int cnt) tuple = this.Env2.Reset(episode / 2, episode % 2);
                var state = tuple.Item1;
                var cnt = tuple.Item2;
                if (cnt < 2) continue;

                float episodeReward = 0.0f;
                float[] nextState = null;
                for (int step = 0; step < max_steps; step++)
                {
                    var actions = this.select_action(episode / 2, episode % 2);
                    var stepReward = 0.0f;
                    var nextTuple = Env2.Step(episode % 2);
                    var nextReward = nextTuple.Item2;
                    nextState = nextTuple.Item1;
                    float done = 0;
                    done = nextTuple.Item3;
                    foreach (var action in actions)
                    {
                        int a = 0;
                        switch (action[0])
                        {
                            case 0:
                                a = action[1] + 6 * action[2];
                                break;
                            default:
                                a = action[0] + action[1] * 4 + action[2] * 16 + 11;
                                break;
                        }
                        if (state == null)
                        {
                            int sada = 123;
                        }
                        this.buffer.Add((state, a, nextReward, nextState, done));
                        // 撕烤
                        if (buffer.Count > buffer_Size) { buffer.RemoveAt(0); }
                        stepReward += nextReward;

                    }
                    episodeReward += stepReward;

                    // 更新状态
                    state = nextState;

                    // 进行学习
                    this.learn();

                    // 如果游戏结束，就跳出这一步循环
                    if (done > 0.5)
                    {
                        break;
                    }
                }

                // 输出训练结果
                if (episode % 100 == 0)
                {
                    Console.WriteLine($"Episode {episode}, Reward {episodeReward}");
                }

                // 更新Target网络
                if (episode % target_update == 0)
                {
                    //var aa = this.model.to("cpu").state_dict();
                    model.save("temp");

                    this.target_model.load("temp");
                }

                // 更新探索率
                if (epsilon > epsilon_end)
                {
                    epsilon *= epsilon_decay;
                }
            }
        }


    }

}

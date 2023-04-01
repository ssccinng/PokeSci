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

namespace DQNTorch
{
    internal class DQN: Module
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
            
            fc1 = Linear(state_size, 512);
            fc2 = Linear(512, 256);
            fc3 = Linear(256, action_size);
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
        public PokeDanEnvTest Env;
        private readonly int buffer_Size;
        private readonly int batch_Size;
        private readonly float gamma;
        private readonly float lr;
        private readonly List<(float[] states, int actions, float rewards, float[] next_states, float dones)> buffer;
        private readonly DQN model;
        private readonly DQN target_model;
        private readonly Adam optimizer;
        private torch.Device device;

        public DQNAgent(PokeDanEnvTest env, int buffer_size = 10000, int batch_size=32, float gamma = 0.99f, float lr = 0.001f)
        {
            device = torch.device( torch.cuda.is_available() ? "cuda" : "cpu");
            Env = env;
            buffer_Size = buffer_size;
            batch_Size = batch_size;
            this.gamma = gamma;
            this.lr = lr;
            buffer = new();
            model = new DQN(1385, 44, 128).to(device);
            target_model = new DQN(1385, 44, 128).to(device);

            optimizer = optim.Adam(model.parameters(), lr);
        }

        public int[][] select_action(int battleId, int player)
        {
            return Env.GetAction(battleId, player);
            
        }

        // 选择一个动作 但要考虑 可能可以一个随机一个不随机
        public int act(Tensor state, float epsilon = 0.1f)
        {
            if (np.random.rand() < epsilon)
            {
                // 随机
            }
            else
            {
                var states = FloatTensor(state).unsqueeze(0).to(device);
                using var aa = torch.no_grad();
                var q_values = model.forward(state);
                // 这个item是什么
                var action = torch.argmax(q_values, 1).item<int>();
            }
            return 0
            ;
        }
        public void learn()
        {
            if (buffer.Count < batch_Size)
            {
                return;
            }
            var samples = np.random.choice(buffer.Count, new Shape(batch_Size), replace: false);
            var bufferTuples = samples.ToArray<int>().Select(s => buffer[s]);
            //var states = tupleList.Select(t => t.Item1).ToList();
            //var actions = tupleList.Select(t => t.Item2).ToList();
            //var rewards = tupleList.Select(t => t.Item3).ToList();
            //var next_states = tupleList.Select(t => t.Item4).ToList();
            //var dones = tupleList.Select(t => t.Item5).ToList();

            //var q_values = model.forward(states).gather(1, actions);

            //var sampleIndices = new Random().Sample(Enumerable.Range(0, this.buffer.Count), this.batchSize);
            //var bufferTuples = sampleIndices.Select(i => this.buffer[i]).ToList();

            // 将元组中的各个元素分别赋值给不同的变量，并转换为张量
            var states = from_array(bufferTuples.Select(t => t.Item1).ToArray()).to(this.device);
            var actions = from_array(bufferTuples.Select(t => t.Item2).ToArray()).unsqueeze(1).to(this.device);
            var rewards = from_array(bufferTuples.Select(t => t.Item3).ToArray()).unsqueeze(1).to(this.device);
            var nextStates = from_array(bufferTuples.Select(t => t.Item4).ToArray()).to(this.device);
            var dones = from_array(bufferTuples.Select(t => t.Item5).ToArray()).unsqueeze(1).to(this.device);

            // 计算损失函数
            var qValues = this.model.forward(states).gather(1, actions);
            var nextQValues = this.target_model.forward(nextStates).detach().max(1).values.unsqueeze(1);
            var targetQValues = rewards + (1 - dones) * this.gamma * nextQValues;
            var loss = nn.functional.mse_loss(qValues, targetQValues);
            // 反向传播更新网络参数
            this.optimizer.zero_grad();
            loss.backward();
            this.optimizer.step();
        }

        public void train(int episodes, int max_steps = 1000, float epsilon_start= 1.0f,
           float epsilon_end = 0.1f, float epsilon_decay = 0.99f, int target_update = 10)
        {
            var epsilon = epsilon_start;

            // 训练循环
            for (int episode = 0; episode < episodes; episode++)
            {
                (float[], int cnt) tuple = this.Env.Reset(episode / 2);
                var state = tuple.Item1;
                var cnt = tuple.Item2;
                if (cnt < 2) continue;

                float episodeReward = 0.0f;
                float[] nextState = null;
                for (int step = 0; step < max_steps; step++)
                {
                    var actions = this.select_action(episode / 2, episode % 2);
                    var stepReward = 0.0f;
                    float done = 0;
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
                        var nextTuple = Env.Step(episode % 2, a);
                        var nextReward = nextTuple.Item2;
                        nextState = nextTuple.Item1;
                         done = nextTuple.Item3;
                        this.buffer.Add((state, a, nextReward, nextState, done));
                        stepReward += nextReward;
                    }
                    episodeReward += stepReward;

                    // 更新状态
                    state = nextState;

                    // 进行学习
                    this.learn ();

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
                    this.target_model.load_state_dict(this.model.state_dict());
                }

                // 更新探索率
                if (epsilon > epsilon_end)
                {
                    epsilon *= epsilon_end;
                }
            }
        }


    }

}

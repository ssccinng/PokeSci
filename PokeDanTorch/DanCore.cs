using PokePSCore;
using PSReplayAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TorchSharp;
using TorchSharp.Modules;
using static TorchSharp.torch.nn;
using static TorchSharp.torch.nn.functional;


namespace PokeDanTorch
{
    public class DanCore
    {
        //private static torch.jit.ScriptModule zqd = torch.jit.load("F:\\VSProject\\PokeDanAI\\model.pt").cuda();
        private static DQN4 zqd = Init();
        public static DQN4 Init()
        {
            //var cc = new DQN2(144, 8 * 900 + 10, 2048);
            var cc = new DQN4();
            //cc.load("F:\\VSProject\\PokeDanAI\\model_weightsDans6.dat").cuda();
            cc.load("F:\\VSProject\\PokeDanAI\\model_weights2k.v5.dat").cuda();
            return cc;
        }
        public static IEnumerable<ChooseDanData> MakeChoose(BattleTurn battleTurn, int p)
        {
            float[] rawArray = ExporttoTrainData.ExportBattleTurn(battleTurn, p).Select(s => (float)s).ToArray();
            var aaa = torch.tensor(rawArray).cuda();
            //var ccc = torch.rand(144).cuda();
            var output = zqd.forward(aaa) as torch.Tensor;
            var chooses = ConvToChoose(output).OrderByDescending(s => s.EV);

            // switch后要更新状态 记得！

            return chooses;
            // 要判断合不合理
            //var output = zqd.forward(ExporttoTrainData.ExportBattleTurn(battleTurn)) as torch.Tensor ;
            //var chooses = ConvToChoose(output);

            // 失败返回默认
        }

        public static IEnumerable<ChooseDanData> MakeSwitch(BattleTurn battleTurn, int p)
        {
            float[] rawArray = ExporttoTrainData.ExportBattleTurn(battleTurn, p).Select(s => (float)s).ToArray();
            var aaa = torch.tensor(rawArray).cuda();
            //var ccc = torch.rand(144).cuda();
            var output = zqd.forward(aaa) as torch.Tensor;
            var chooses = ConvToChoose(output).Where(s => s.ChooseType == ChooseType.Switch).OrderByDescending(s => s.EV);
            // switch后要更新状态 记得！

            return chooses;
        }
        public static void MakeMove(BattleData battleData)
        {

        }


        public static List<ChooseDanData> ConvToChoose(torch.Tensor tensor)
        {
            List<ChooseDanData> res = new List<ChooseDanData>();
            for (int i = 0; i < tensor.shape[0]; i++)
            {
                if (i < 12)
                {
                    res.Add(new ChooseDanData { ChooseType = ChooseType.Switch, Target1 = i % 6, Target2 = i / 6, EV = tensor[i].ToSingle() });
                }
                else
                {
                    var ii = i - 12;
                    res.Add(new ChooseDanData { EV = tensor[i].ToSingle(), ChooseType = ChooseType.Move, Target1 = ii % 4, Target2 = ii / 4 % 4, Pos = ii / 16 });
                }
            }
            return res.OrderByDescending(s => s.EV).ToList();
            return res;
        }

        public static string ExportToPsCmd(ChooseDanData chooseData)
        {
            return string.Empty;
        }

        public static PSMove GetSimiarMove()
        {
            // 处理输出数据
            //Console.WriteLine(output.ToString());
            return null;
        } 
    }

    public class ChooseDanData
    {
        public ChooseType ChooseType { get; set; }
        public int Target1 { get; set; }
        public int Target2 { get; set; }
        public int Pos { get; set; }
        public float EV { get; set; }
    }

    public class DQN : torch.nn.Module
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
            
            fc1 = Linear(state_size, 256);
            fc2 = Linear(256, 128);
            fc3 = Linear(128, action_size);

            RegisterComponents();
        }

        public torch.Tensor forward(torch.Tensor x)
        {
            x = relu(fc1.forward(x));
            x = relu(fc2.forward(x));
            return fc3.forward(x);
        }



    }
    public class DQN4 : torch.nn.Module
    {

        private Linear fc1;
        private Linear fc3;
        private Linear fc2;

        public DQN4() : base("test")
        {


            //fc1 = Linear(state_size, hidden_size);
            //fc2 = Linear(hidden_size, hidden_size);
            //fc3 = Linear(hidden_size, action_size); 

            fc1 = Linear(977, 256);
            fc2 = Linear(256, 128);
            fc3 = Linear(128, 44);

            RegisterComponents();
        }

        public torch.Tensor forward(torch.Tensor x)
        {
            x = relu(fc1.forward(x));
            x = relu(fc2.forward(x));
            return fc3.forward(x);
        }



    }public class DQN3 : torch.nn.Module
    {

        private Linear fc1;
        private Linear fc3;
        private Linear fc2;

        public DQN3() : base("test")
        {


            //fc1 = Linear(state_size, hidden_size);
            //fc2 = Linear(hidden_size, hidden_size);
            //fc3 = Linear(hidden_size, action_size); 

            fc1 = Linear(801, 512);
            fc2 = Linear(512, 256);
            fc3 = Linear(256, 7210);

            RegisterComponents();
        }

        public torch.Tensor forward(torch.Tensor x)
        {
            x = relu(fc1.forward(x));
            x = relu(fc2.forward(x));
            return fc3.forward(x);
        }



    }
    public class DQN2 : torch.nn.Module
    {

        private Linear fc1;
        private Linear fc3;
        private Linear fc2;

        public DQN2() : base("test")
        {


            //fc1 = Linear(state_size, hidden_size);
            //fc2 = Linear(hidden_size, hidden_size);
            //fc3 = Linear(hidden_size, action_size); 

            fc1 = Linear(144, 128);
            fc2 = Linear(128, 64);
            fc3 = Linear(64, 7210);

            RegisterComponents();
        }

        public torch.Tensor forward(torch.Tensor x)
        {
            x = relu(fc1.forward(x));
            x = relu(fc2.forward(x));
            return fc3.forward(x);
        }



    }
}

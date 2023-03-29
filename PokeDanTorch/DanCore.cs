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
        private torch.jit.ScriptModule zqd = torch.jit.load("F:\\VSProject\\PokeDanAI\\model.pt").cuda();
        public void MakeChoose(BattleData battleData)
        {

        }

        public void MakeSwitch(BattleData battleData)
        {

        }
        public void MakeMove(BattleData battleData)
        {

        }


        public void ConvToChoose(torch.Tensor tensor)
        {

            for (int i = 0; i < tensor.shape[0]; i++)
            {

                var expectedValue = tensor[i].ToDouble;
            }
        }

        public string ExportToPsCmd(ChooseData chooseData)
        {
            return string.Empty;
        }

        public string Get
    }
}

using TorchSharp;
using TorchSharp.Modules;
using static TorchSharp.torch.nn;
using static TorchSharp.torch.nn.functional;
Console.WriteLine(torch.__version__);

//var mm = Module.Load("F:\\VSProject\\PokeDanAI\\model.pt");
var dQN1 = torch.jit.load("F:\\VSProject\\PokeDanAI\\model.pt").cuda();
////var dQ = torch.load("F:\\VSProject\\PokeDanAI\\model.pt").cuda();
////var dQN = DQN.Create<DQN>("F:\\VSProject\\PokeDanAI\\model.pt").cuda();
////dQN(torch.rand(328));
////dQN.;
//dQN1.forward(flo);
//dQ.for

var aa = dQN1.forward(torch.rand(548).cuda()) as torch.Tensor;

for (int i = 0; i < aa.shape[0]; i++)
{
    Console.Write(aa[i].ToDouble());
    Console.Write(" ");
}
return;






// 加载一个模型
class DQN : torch.nn.Module
{
    private readonly Conv2d _convLayer1;
    private readonly Conv2d _convLayer2;
    private readonly Flatten _flattenLayer;
    private readonly Linear _denseLayer1;
    private readonly Linear _outputLayer;
    protected DQN(int numActions) : base("")
    {
        _convLayer1 =  Conv2d(1, 16, kernelSize: 4, stride: 1, padding: 2);
        _convLayer2 =  Conv2d(16, 32, kernelSize: 4, stride: 2);
        _flattenLayer =  Flatten();
        _denseLayer1 =  Linear(4095, 256);
        _outputLayer =  Linear(256, numActions);
    }

    public torch.Tensor forward(torch.Tensor x)
    {
        x = relu(_convLayer1.forward(x.reshape(1, 1, 32, 548)));
        x = relu(_convLayer2.forward(x));
        x = _flattenLayer.forward(x);
        x = relu(_denseLayer1.forward(x));
        return _outputLayer.forward(x);
    }
}


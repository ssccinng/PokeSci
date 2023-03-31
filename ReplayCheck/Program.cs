// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using static PSReplayAnalysis.ExporttoTrainData;
using static PSReplayAnalysis.PSReplayAnalysis;
Console.WriteLine("Hello, World!");
var dd = JsonSerializer.Deserialize<List<BattleTrainData>>
    (File.ReadAllText(@"F:\VSProject\PokeSci\PokeSci\bin\Debug\net7.0\testdata.v4.json"), new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });



foreach (var item in dd)
{
    int tt = 1;
    foreach (var item1 in item.StateSpace)
    {
        Console.WriteLine($"第{tt}回合");
        Console.WriteLine($"玩家1");
        List<(string poke, int pos)> pokes1 = new();
        List<(string poke, int pos)> pokes2 = new();
        for (int i = 0; i < 6; i++)
        {
            pokes1.Add((PSReplayAnalysis.PSReplayAnalysis.PsPokes1[(int)item1[1][i * 12]].name, (int)item1[1][i * 12 + 10]));
            Console.WriteLine(
                $"{PSReplayAnalysis.PSReplayAnalysis.PsPokes1[(int)item1[1][i * 12]].name} HP: {item1[1][i * 12 + 9]} Now: {item1[1][i * 12 + 10]}");
        }

        Console.WriteLine($"玩家2");

        for (int i = 0; i < 6; i++)
        {
            pokes2.Add((PSReplayAnalysis.PSReplayAnalysis.PsPokes1[(int)item1[2][i * 12]].name, (int)item1[2][i * 12 + 10]));

            Console.WriteLine(
                $"{PSReplayAnalysis.PSReplayAnalysis.PsPokes1[(int)item1[2][i * 12]].name} HP: {item1[2][i * 12 + 9]} Now: {item1[2][i * 12 + 10]}");
        }

        var pa1 = item.Player1Action[tt - 1];
        int iidx = 0;
        foreach (var item2 in pa1)
        {
            if (item2[0] == 0)
            {
                Console.WriteLine($"换人 {PsPokes1[(int)item1[1][item2[1] * 12]].name}");
            }
            else
            {
                var poke = pokes1.FindIndex(s => s.pos == item2[2]);
                Console.WriteLine($"技能 {PsMove1[(int)item1[4][(item2[0] - 1) * 9 + poke * 36]].name} 指向 {item2[1]}");
            }
                iidx++;
        }
            
        var pa2 = item.Player2Action[tt - 1];

        Console.WriteLine();
        iidx = 0;
        foreach (var item2 in pa2)
        {
            if (item2[0] == 0)
            {
                Console.WriteLine($"换人 {PsPokes1[(int)item1[2][item2[1] * 12]].name}");
            }
            else
            {
                var poke = pokes2.FindIndex(s => s.pos == item2[2]);
                Console.WriteLine($"技能 {PsMove1[(int)item1[6][(item2[0] - 1) * 9 + poke * 36]].name} 指向 {item2[1]}");
            }
            iidx++;
        }
        Console.WriteLine();
        Console.ReadKey();
        tt++;
    }
}
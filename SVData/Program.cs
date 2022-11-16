// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using System.Text.RegularExpressions;
using PokeCommon.Utils;
using PokemonDataAccess.Models;
using SVData.Models;

//Console.WriteLine("Hello, World!");

string englishPath = "English";
string chinesePath = "Simp_Chinese";
string jpnPath = "JPN";
string wazaFileName = "common/wazaname.txt";
string pokemonFileName = "common/monsname.txt";
string zknFormName = "common/zkn_form.txt";
string tokuseiName = "common/tokusei.txt";

string basePath = "D:\\QQ\\1078995020\\FileRecv\\message";
//ReadData(tokuseiName, "tokusei");
//return;
//var chineseWazaData = File.ReadAllLines($"{basePath}/{chinesePath}/{wazaFileName}");
//var englishWazaData = File.ReadAllLines($"{basePath}/{englishPath}/{wazaFileName}");
//var jpnWazaData = File.ReadAllLines($"{basePath}/{jpnPath}/{wazaFileName}");
//List<Move> moves= new List<Move>();
//for (int i = 1; i < chineseWazaData.Length; i++)
//{
//    moves.Add(new Move
//    {
//        MoveId = i - 1,
//        Name_Chs = chineseWazaData[i].Trim(),
//        Name_Eng = englishWazaData[i].Trim(),
//        Name_Jpn = jpnWazaData[i].Trim(),
//    }) ;
//}
//File.WriteAllText("moves.json",
//JsonSerializer.Serialize(moves)
//    );
//return;

List<Move> moves = JsonSerializer.Deserialize<List<Move>>(File.ReadAllBytes("moves.json"));
List<Name> names = JsonSerializer.Deserialize<List<Name>>(File.ReadAllBytes("monsname.json"));
List<Name> tokuseis = JsonSerializer.Deserialize<List<Name>>(File.ReadAllBytes("tokusei.json"));
Console.WriteLine(1);
Dictionary<string, string> map = moves.DistinctBy(s => s.Name_Eng).ToDictionary(s => s.Name_Eng, s => s.Name_Chs);
Dictionary<string, string> pokenamemap = names.DistinctBy(s => s.Name_Eng).ToDictionary(s => s.Name_Eng, s => s.Name_Chs);
Dictionary<string, string> tokuseismap = tokuseis.DistinctBy(s => s.Name_Eng).ToDictionary(s => s.Name_Eng, s => s.Name_Chs);


var text = File.ReadAllText("learnset.txt");
var pokes = Regex.Split(text, @"\n\s*?\n");
foreach (var poke in pokes)
{
    var lines = poke.Trim().Split("\n");

    var basedata = lines[0].Split(" - ");
    var name = basedata[0];
    int form = 0;
    if (Regex.IsMatch(name, @"-\d*$"))
    {
        var idx = name.LastIndexOf('-');
        form = int.Parse(name[(idx + 1)..]);
        name = name[..idx];

    }
    Console.WriteLine("name: {0}", pokenamemap[name]);
    Console.WriteLine("form: {0}", form);
    Console.WriteLine("bv: {0}", basedata[1]);
    Console.WriteLine("type: {0}", string.Join('/', basedata[2].Trim()
        .Split('/')
        .Select( s => ( PokemonTools.GetTypeAsync(s)).Result.Name_Chs)));
    Console.WriteLine("ability: {0}",
        string.Join('/',
        basedata[3].Trim()
        .Split('/')
        .Select(s => tokuseismap[s])));
    var EggMove = Array.Empty<string>();
    var TMMove = Array.Empty<string>(); ;
    List<(string, string)> LvMove = new();
    List<EvolutionData> EvolutionDatas = new();
    for (var i = 1; i < lines.Length; i++)
    {
        if (lines[i].Contains("Evolutions"))
        {
            var cnt = Regex.Matches(poke, "Condition").Count;
            if (cnt > 1)
            {
                for (int k = 0; k < cnt; k++)
                {
                    ++i;
                    EvolutionData evolutionData = new EvolutionData();
                    for (var j = 1; j <= 5; j++)
                    {
                        var data = lines[i + j].Split(": ");
                        switch (j)
                        {
                            case 1:
                                evolutionData.Level = int.Parse(data[1].Trim());
                                Console.WriteLine("进化等级: {0}", data[1].Trim());
                                break;
                            case 2:
                                evolutionData.Condition = data[1].Trim();
                                Console.WriteLine("进化方式: {0}", data[1].Trim());

                                break;
                            case 3:
                                evolutionData.Parameters = data[1].Trim();
                                Console.WriteLine("进化参数: {0}", data[1].Trim());
                                break;
                            case 4:
                                evolutionData.Species = pokenamemap[data[1].Trim()]; // 这里可以查表
                                Console.WriteLine("进化为: {0}", pokenamemap[data[1].Trim()]);
                                break;
                            case 5:
                                evolutionData.Form = int.Parse(data[1].Trim());

                                Console.WriteLine("进化形态: {0}", data[1].Trim());
                                break;

                            default:
                                break;
                        }
                    }
                    EvolutionDatas.Add(evolutionData);
                    i += 5;
                }
            }
            else
            {
                EvolutionData evolutionData = new EvolutionData();
                for (var j = 1; j <= 5; j++)
                {
                    var data = lines[i + j].Split(": ");
                    switch (j)
                    {
                        case 1:
                            evolutionData.Level = int.Parse(data[1].Trim());
                            Console.WriteLine("进化等级: {0}", data[1].Trim());
                            break;
                        case 2:
                            evolutionData.Condition = data[1].Trim();
                            Console.WriteLine("进化方式: {0}", data[1].Trim());

                            break;
                        case 3:
                            evolutionData.Parameters = data[1].Trim();
                            Console.WriteLine("进化参数: {0}", data[1].Trim());
                            break;
                        case 4:
                            evolutionData.Species = pokenamemap[data[1].Trim()]; // 这里可以查表
                            Console.WriteLine("进化为: {0}", pokenamemap[data[1].Trim()]);
                            break;
                        case 5:
                            evolutionData.Form = int.Parse(data[1].Trim());

                            Console.WriteLine("进化形态: {0}", data[1].Trim());
                            break;

                        default:
                            break;
                    }
                }
                EvolutionDatas.Add(evolutionData);
                i += 5;

            }
        }
        else if (lines[i].Contains("Egg Moves"))
        {
            // 性能低低
            EggMove = lines[++i].Trim().Split(", ").Select(s => map[s]).ToArray();
        }
        else if (lines[i].Contains("TM Moves"))
        {
            TMMove = lines[++i].Trim().Split(", ").Select(s => map[s]).ToArray();

        }
        else if (lines[i].Contains("Learned Moves"))
        {
            while (i + 1 < lines.Length && lines[++i].Contains('@'))
            {
                var data = lines[i].Trim().Split(" @ ");
                LvMove.Add((map[data[0]], (data[1][3..])));
            }
            --i;
        }
    }
    Console.WriteLine();
    //Console.WriteLine("升级招式");
    //foreach (var item in LvMove)
    //{
    //    Console.WriteLine(item);
    //}
    //Console.WriteLine();
    //Console.WriteLine("遗传招式");
    //foreach (var item in EggMove)
    //{
    //    Console.WriteLine(item);
    //}
    //Console.WriteLine();
    //Console.WriteLine("TM招式");
    //foreach (var item in TMMove)
    //{
    //    Console.WriteLine(item);
    //}
    //Console.WriteLine();

    Console.ReadKey();
}



void ReadData(string filename, string dani)
{
    var chineseWazaData = File.ReadAllLines($"{basePath}/{chinesePath}/{filename}");
    var englishWazaData = File.ReadAllLines($"{basePath}/{englishPath}/{filename}");
    var jpnWazaData = File.ReadAllLines($"{basePath}/{jpnPath}/{filename}");
    List<object> moves = new();
    for (int i = 1; i < chineseWazaData.Length; i++)
    {
        moves.Add(new
        {
            MoveId = i - 1,
            Name_Chs = chineseWazaData[i].Trim(),
            Name_Eng = englishWazaData[i].Trim(),
            Name_Jpn = jpnWazaData[i].Trim(),
        });
    }
    File.WriteAllText($"{dani}.json",
    JsonSerializer.Serialize(moves)
        );
}
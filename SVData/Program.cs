// See https://aka.ms/new-console-template for more information
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
//using MySqlX.XDevAPI.Common;
using PokeCommon.Utils;
using PokemonDataAccess.Models;
using SVData;
using SVData.Models;

//Console.WriteLine("Hello, World!");

var englishPath = "English";
var chinesePath = "Simp_Chinese";
var chtPath = "Trad_Chinese";
var jpnPath = "JPN";
var itaPath = "Italian";
var korPath = "Korean";
var FrePath = "French";
var SpaPath = "Spanish";
var GerPath = "German";
var wazaFileName = "common/wazaname.txt";
var pokemonFileName = "common/monsname.txt";
var zknFormName = "common/zkn_form.txt";
var tokuseiName = "common/tokusei.txt";
var itemName = "common/itemname.txt";


var swshDataPath = "SWSHData.txt";

var basePath = "D:\\QQ\\1078995020\\FileRecv\\message";
ReadData1(wazaFileName, "wazanameall");
return;
//ReadData(itemName, "itemName");
//return;
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

//var aada = PokemonTools.PokemonContext.PokeTypes.Select(s => s.Name_Chs).ToArray();
//KurtAnas.RunImgPath(File.ReadAllText("learnsetsv"));
//return;
var moves = JsonSerializer.Deserialize<List<Move>>(File.ReadAllBytes("moves.json"));
var names = JsonSerializer.Deserialize<List<Name>>(File.ReadAllBytes("monsname.json"));
var tokuseis = JsonSerializer.Deserialize<List<Name>>(File.ReadAllBytes("tokusei.json"));
var Forms = JsonSerializer.Deserialize<List<Name>>(File.ReadAllBytes("zkn_form.json"));
JsonNode jsonObj = JsonNode.Parse(File.ReadAllBytes("idname.json"));

//var swshData = File.ReadAllLines($"{basePath}/{swshDataPath}");

var swshMap = new Dictionary<string, HashSet<string>>();

swshMap = JsonSerializer
    .Deserialize<Dictionary<string, HashSet<string>>>
    (File.ReadAllBytes("swshMap.json"));
//for (var i = 0; i < swshData.Length; i++)
//{
//    var data = swshData[i].Split(',');
//    var name = data[0];
//    Console.WriteLine(name);
//    swshMap[name] = new HashSet<string>();
//    for (var j = 1; j < data.Length; j++)
//    {
//        var move = data[j][..data[j].LastIndexOf('-')];
//        swshMap[name].Add(move);
//        //Console.WriteLine(move);
//    }
//}
//File.WriteAllText("swshMap.json", JsonSerializer.Serialize(swshMap));

//return;

List<Pokemon> pokemons = new();

//for (int i = 0; i < names.Count; i++)
//{
//    pokemons.Add(new Pokemon
//    {
//        NameChs = names[i].Name_Chs,
//        NameEng = names[i].Name_Chs,
//        nam = names[i].Name_Chs,
//    });
//}


Console.WriteLine(1);
var map = moves.DistinctBy(s => s.Name_Eng).ToDictionary(s => s.Name_Eng, s => s.Name_Chs);
var pokenamemap = names.DistinctBy(s => s.Name_Eng).ToDictionary(s => s.Name_Eng, s => s.Name_Chs);
var tokuseismap = tokuseis.DistinctBy(s => s.Name_Eng).ToDictionary(s => s.Name_Eng, s => s.Name_Chs);

Dictionary<string, PokeWithMove> pokeWithMoves = new();
Dictionary<string, SVPokemon> sv = new();
for (int i = 0; i < names.Count; i++)
{
    //Console.WriteLine($"{names[i].Name_Chs}-{Forms[i].Name_Chs}");
}
foreach (var item in names)
{
    sv.TryAdd(item.Name_Chs, new SVPokemon
    {
        NameChs = item.Name_Chs,
        NameEng= item.Name_Eng,
        NameJpn= item.Name_Chs,
    });
    pokeWithMoves.TryAdd(item.Name_Chs, new()
    {
        Pokemon = item
    });
}
var file = File.Open("chayi.txt", FileMode.Create, FileAccess.Write);
var pokelist = PokemonTools.PokemonContext.Pokemons.ToList();
var text = File.ReadAllText("learnset.txt");
var pokes = Regex.Split(text, @"\n\s*?\n");

Dictionary<string, string[]> danier = new();
int test = 1;

var engnames = names.Select(s => s.Name_Eng).ToList();
var tosnames = tokuseis.Select(s => s.Name_Eng).ToList();
Dictionary<string, List<EvolutionData>> EvolutionDatass = new();


//return;


var dada = File.Open("pokedatasv", FileMode.Create);

foreach (var poke in pokes)
{
    StringBuilder sb = new();
    Console.WriteLine(test++);
    var svpoke = new SVPokemon();
    var lines = poke.Trim().Split("\n");

    var basedata = lines[0].Split(" - ");
    var name = basedata[0];
    var form = 0;
    if (Regex.IsMatch(name, @"-\d*$"))
    {
        var idx = name.LastIndexOf('-');
        form = int.Parse(name[(idx + 1)..]);
        name = name[..idx];

    }
    sb.Append(pokenamemap[name]);
    Console.WriteLine("name: {0}", pokenamemap[name]);
    Console.WriteLine("form: {0}", form);
    Console.WriteLine("bv: {0}", basedata[1]);
    Console.WriteLine("type: {0}", string.Join('/', basedata[2].Trim()
        .Split('/')
        .Select(s => (PokemonTools.GetTypeAsync(s)).Result.Name_Chs)));

    int dexid = engnames.IndexOf(name) + 1;

    if (dexid == 999)
    {
        int aa = 0;


    }
    var formname = jsonObj[dexid.ToString("000")][form.ToString("000")];
    Console.WriteLine("formname: {0}", formname);
    if (formname.GetValue<string>() != "")
    sb.Append($"-{jsonObj[dexid.ToString("000")][form.ToString("000")]}");
    sb.Append(',');
    sb.Append(string.Join(',', basedata[2].Trim()
        .Split('/')
        .Select(s => (PokemonTools.GetTypeAsync(s)).Result.Id)));
    sb.Append(',');
    sb.Append(basedata[1].Split(' ')[0].Replace('/', ','));

    sb.Append(',');

    sb.Append(string.Join(',',
        basedata[3].Trim()
        .Split('/')
        .Select(s => tosnames.IndexOf(s))));
    sb.Append(',');
    sb.Append(dexid);


    Console.WriteLine("ability: {0}",
        string.Join('/',
        basedata[3].Trim()
        .Split('/')
        .Select(s => tokuseismap[s])));
    var EggMove = Array.Empty<string>();
    var TMMove = Array.Empty<string>(); ;
    List<(string, string)> LvMove = new();
    List<EvolutionData> EvolutionDatas = new();
    Console.WriteLine(sb.ToString());
    dada.Write(Encoding.UTF8.GetBytes(sb.ToString()));
    dada.Write("\n"u8);
    var cc = pokelist.FirstOrDefault(s => s.NameChs == pokenamemap[name] && s.PokeFormId == form);

    for (var i = 1; i < lines.Length; i++)
    {
        if (lines[i].Contains("Evolutions"))
        {
            var cnt = Regex.Matches(poke, "Condition").Count;
            if (cnt > 1)
            {
                for (var k = 0; k < cnt; k++)
                {
                    ++i;
                    var evolutionData = new EvolutionData();
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
                                evolutionData.DexId = engnames.IndexOf(data[1].Trim()) + 1;
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
                var evolutionData = new EvolutionData();
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
                            evolutionData.DexId = engnames.IndexOf(data[1].Trim()) + 1;

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
            //if (cc != null)
             

            foreach (var item in EvolutionDatas)
            {
                var cc1 = pokelist.FirstOrDefault(s => s.NameChs ==
                    item.Species && s.PokeFormId == item.Form);
                if (cc1 != null)
                danier.TryAdd(cc1.FullNameChs, EggMove);
            }
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
    EvolutionDatass[$"{dexid}+{form}"] = (EvolutionDatas);
}
Console.WriteLine(JsonSerializer.Serialize(pokeWithMoves, new JsonSerializerOptions
{
    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
}));

File.WriteAllText("Evo.json", JsonSerializer.Serialize(EvolutionDatass));

dada.Close();
return;
foreach (var poke in pokes)
{
    var lines = poke.Trim().Split("\n");

    var basedata = lines[0].Split(" - ");
    var name = basedata[0];
    var form = 0;
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
        .Select( s => (PokemonTools.GetTypeAsync(s)).Result.Name_Chs)));
    Console.WriteLine("ability: {0}",
        string.Join('/',
        basedata[3].Trim()
        .Split('/')
        .Select(s => tokuseismap[s])));
    var EggMove = Array.Empty<string>();
    var TMMove = Array.Empty<string>(); ;
    List<(string, string)> LvMove = new();
    List<EvolutionData> EvolutionDatas = new();
    var cc = pokelist.FirstOrDefault(s => s.NameChs == pokenamemap[name] && s.PokeFormId == form);

    for (var i = 1; i < lines.Length; i++)
    {
        if (lines[i].Contains("Evolutions"))
        {
            var cnt = Regex.Matches(poke, "Condition").Count;
            if (cnt > 1)
            {
                for (var k = 0; k < cnt; k++)
                {
                    ++i;
                    var evolutionData = new EvolutionData();
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
                var evolutionData = new EvolutionData();
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
    HashSet<string> result = new();
    foreach (var move in EggMove)
    {
        result.Add(move);
    }

    foreach (var move in TMMove)
    {
        result.Add(move);
    }
    foreach (var move in LvMove)
    {
        result.Add(move.Item1);
    }
    if (cc!= null && danier.ContainsKey(cc.FullNameChs))
    foreach (var move in danier[cc.FullNameChs])
    {
        result.Add(move);
    }
    if (cc != null)
    {
        if (swshMap.ContainsKey(cc.FullNameChs))
        {
            Console.WriteLine(cc.FullNameChs);
                file.Write(Encoding.UTF8.GetBytes(cc.FullNameChs));
            file.WriteByte((byte)'\n');
            var a = swshMap[cc.FullNameChs].Except(result);
            var b = result.Except(swshMap[cc.FullNameChs]);

            Console.WriteLine("-------");
                file.Write(Encoding.UTF8.GetBytes("-------\n"));
            foreach (var item in a)
            {
                
                file.Write(Encoding.UTF8.GetBytes(item));
                file.WriteByte((byte)'\n');
                Console.WriteLine(item);
            }
                file.WriteByte((byte)'\n');
            Console.WriteLine("+++++++");
                file.Write(Encoding.UTF8.GetBytes("+++++++\n"));
            foreach (var item in b)
            {
                file.Write(Encoding.UTF8.GetBytes(item));

                file.WriteByte((byte)'\n');
                Console.WriteLine(item);
            }
            file.WriteByte((byte)'\n');
            file.WriteByte((byte)'\n');

            //Console.ReadKey();
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

    //Console.ReadKey();
}


file.Close();
void ReadData(string filename, string dani)
{
    var chineseWazaData = File.ReadAllLines($"{basePath}/{chinesePath}/{filename}");
    var englishWazaData = File.ReadAllLines($"{basePath}/{englishPath}/{filename}");
    var jpnWazaData = File.ReadAllLines($"{basePath}/{jpnPath}/{filename}");
    List<object> moves = new();
    for (var i = 1; i < chineseWazaData.Length; i++)
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
void ReadData1(string filename, string dani)
{
    var chineseWazaData = File.ReadAllLines($"{basePath}/{chinesePath}/{filename}");
    var englishWazaData = File.ReadAllLines($"{basePath}/{englishPath}/{filename}");
    var jpnWazaData = File.ReadAllLines($"{basePath}/{jpnPath}/{filename}");
    var chtWazaData = File.ReadAllLines($"{basePath}/{chtPath}/{filename}");
    var KorWazaData = File.ReadAllLines($"{basePath}/{korPath}/{filename}");
    var GerWazaData = File.ReadAllLines($"{basePath}/{GerPath}/{filename}");
    var ItaWazaData = File.ReadAllLines($"{basePath}/{itaPath}/{filename}");
    var FreWazaData = File.ReadAllLines($"{basePath}/{FrePath}/{filename}");
    var SpaWazaData = File.ReadAllLines($"{basePath}/{SpaPath}/{filename}");
    List<object> moves = new();
    for (var i = 1; i < chineseWazaData.Length; i++)
    {
        moves.Add(new
        {
            MoveId = i - 1,
            Name_Chs = chineseWazaData[i].Trim(),
            Name_Cht = chtWazaData[i].Trim(),
            Name_Eng = englishWazaData[i].Trim(),
            Name_Jpn = jpnWazaData[i].Trim(),
            Name_Kor = KorWazaData[i].Trim(),
            Name_Ita = ItaWazaData[i].Trim(),
            Name_Fre = FreWazaData[i].Trim(),
            Name_Ger = GerWazaData[i].Trim(),
            Name_Span = SpaWazaData[i].Trim(),
        });
    }
    File.WriteAllText($"{dani}.json",
    JsonSerializer.Serialize(moves)
        );
}
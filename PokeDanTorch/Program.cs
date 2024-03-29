﻿using TorchSharp;
using TorchSharp.Modules;
using static TorchSharp.torch.nn;
using static TorchSharp.torch.nn.functional;


// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using PokeCommon.PokemonShowdownTools;
using PokePSCore;
using PokeDanTorch;
using PSReplayAnalysis;
using PokeCommon.Models;
using Tensorboard;
using System.Collections;
using PSReplayAnalysis.PokeLib;

using static PSReplayAnalysis.PSReplayAnalysis;
using PokeCommon.Utils;

//torch.load("F:\\VSProject\\PokeDanAI\\model_weights13.dat");
//Module.Load("F:\\VSProject\\PokeDanAI\\model_weights13.dat");
//var cc = new DQN(144, 8 * 900 + 10, 2048);
//cc.load("F:\\VSProject\\PokeDanAI\\model_weightsDans.dat");

//var cc = new DQN6();
//cc.load("F:\\VSProject\\PokeDanAI\\model_weights2w.1.v10.dat").cuda();
//while (true)
//{

//    var vv = cc.forward(torch.randn(1385).cuda());

//    var qq = DanCore.ConvToChoose(vv).Where(s => s.ChooseType == ChooseType.Move);
//    //var ccc = qq.Select(s => PSReplayAnalysis.PSReplayAnalysis.PsMoves.Values.FirstOrDefault(s1=>s1.num == s.Target1 )).ToArray();
//    //float[] aaa = new float[vv.shape[0]];
//    //for (int i = 0; i < vv.shape[0]; ++i)
//    //{
//    //    aaa[i] = vv[i].ToSingle();
//    //}

//}

////aa.

//return;
Dictionary<string, PSReplayAnalysis.PSReplayAnalysis> battleana = new();

AIConfig config = new AIConfig();

if (!File.Exists("AIConfig.json"))
{
    File.WriteAllText("AIConfig.json", JsonSerializer.Serialize(config, new JsonSerializerOptions()
    {
        WriteIndented = true,
    }));

    Console.WriteLine("请在AIConfig.json设置配置...");
    Console.ReadKey();
    return;
}
else
{
    Console.WriteLine("正在读取配置...");
    config = JsonSerializer.Deserialize<AIConfig>(File.ReadAllText("AIConfig.json"));

    Console.WriteLine("队伍:");
    if (config.Team == "" || config.Team == null)
    {
        Console.WriteLine("请输入队伍码.. 在最后输入一个q以确认队伍输入完毕");
        List<string> list = new List<string>();
        while (true)
        {
            var input = Console.ReadLine();
            if (input == "q")
            {
                config.Team = String.Join('\n', list);
                break;
            }
            list.Add(input);
        }
        File.WriteAllText("AIConfig.json", JsonSerializer.Serialize(config, new JsonSerializerOptions()
        {
            WriteIndented = true,
        }));
        Console.WriteLine("录入完毕...");
    }
    Console.WriteLine(config.Team);
    File.WriteAllText("AIConfig.json", JsonSerializer.Serialize(config, new JsonSerializerOptions()
    {
        WriteIndented = true,
    }));
}



var team1 = await PSConverter.ConvertToPokemonsAsync(config.Team);
//var team1 = PSReplayAnalysis.PokeLib.Pokemonshowdown.PStoPokemon(config.Team);
Console.WriteLine("准备登录");

//var pc2 = new PSClient("kirbyRBP", config.Password, "ws://localhost:8000/showdown/websocket").LogTo(Console.WriteLine);
//await pc2.ConnectAsync();

var pc = new PSClient(config.Username, config.Password, "ws://localhost:8000/showdown/websocket").LogTo(Console.WriteLine);
//var pc2 = new PSClient("KirbyRBP", config.Password, "ws://localhost:8000/showdown/websocket").LogTo(Console.WriteLine);
await pc.ConnectAsync();



await Task.Delay(500);
Console.WriteLine(await pc.LoginAsync());

//Console.WriteLine(await pc2.LoginAsync());


;

pc.ChallengeAction += async (player, rule) =>
{
    // if (rule == "gen8randombattle")
    //if (rule == "gen7vgc2019")
    if (rule.StartsWith("gen9vgc2023"))
    {
        //await pc.ChatWithIdAsync(player, "随机战斗，玩了");
        //await pc.ChatWithIdAsync(player, "就决定是你了");
        // await pc.ChangeYourTeamAsync("null");
        await pc.ChangeYourTeamAsync(await PSConverter.ConvertToPsOneLineAsync(team1));
        //await pc.ChangeYourTeamAsync( PSReplayAnalysis.PokeLib.Pokemonshowdown.PStoPokemon(team1));
        //await pc.ChangeYourTeamAsync("Dondozo||leftovers|unaware|protect,earthquake,orderup,wavecrash|Adamant|,252,,,4,252|||||,,,,,Dragon]Tatsugiri||choicescarf|commander|dracometeor,hydropump,icywind,muddywater|Timid|4,,,252,,252||,0,,,,|||,,,,,Dragon]Armarouge||safetygoggles|flashfire|wideguard,armorcannon,expandingforce,trickroom|Modest|228,,28,252,,||,0,,,,|||,,,,,Psychic]Indeedee-F||psychicseed|psychicsurge|followme,trickroom,helpinghand,dazzlinggleam|Bold|252,,252,,4,|F|,0,,,,|||,,,,,Psychic]Sylveon||throatspray|pixilate|protect,hypervoice,terablast,quickattack|Modest|164,,252,84,4,4|||||,,,,,Fire]Meowscarada||focussash|overgrow|protect,flowertrick,knockoff,suckerpunch|Jolly|4,252,,,,252|||||,,,,,Grass");
        //await pc.ChangeYourTeamAsync("Tatsugiri||choicescarf|commander|dracometeor,hydropump,icywind,muddywater|Timid|4,,,252,,252||,0,,,,|||,,,,,Dragon]Indeedee-F||psychicseed|psychicsurge|followme,trickroom,helpinghand,dazzlinggleam|Bold|252,,252,,4,|F|,0,,,,|||,,,,,Psychic]Dondozo||leftovers|unaware|protect,earthquake,orderup,wavecrash|Adamant|,252,,,4,252|||||,,,,,Dragon]Armarouge||safetygoggles|flashfire|wideguard,armorcannon,expandingforce,trickroom|Modest|228,,28,252,,||,0,,,,|||,,,,,Psychic]Sylveon||throatspray|pixilate|protect,hypervoice,terablast,quickattack|Modest|164,,252,84,4,4|||||,,,,,Fire]Meowscarada||focussash|overgrow|protect,flowertrick,knockoff,suckerpunch|Jolly|4,252,,,,252|||||,,,,,Grass");
        //await pc.ChangeYourTeamAsync("Pelipper||focussash|drizzle|hydropump,protect,wideguard,hurricane|Timid|4,,4,244,4,252||,0,,,,||50|,,,,,Flying]Flutter Mane||lifeorb|protosynthesis|moonblast,shadowball,dazzlinggleam,protect|Modest|68,,36,196,4,204||,0,,,,||50|,,,,,Fairy]Baxcalibur||loadeddice|thermalexchange|iciclecrash,iceshard,glaiverush,protect|Adamant|132,252,12,,28,84||||50|,,,,,Poison]Amoonguss||sitrusberry|regenerator|spore,protect,pollenpuff,ragepowder|Calm|236,,196,,76,||,0,,,,||50|,,,,,Steel]Palafin||mysticwater|zerotohero|haze,wavecrash,jetpunch,protect|Adamant|252,236,4,,4,12||||50|,,,,,Water]Iron Hands||assaultvest|quarkdrive|fakeout,closecombat,wildcharge,voltswitch|Adamant|76,156,12,,252,12||||50|,,,,,Grass");
        //await pc.ChangeYourTeamAsync("Chien-Pao||focussash|swordofruin|protect,sacredsword,icespinner,suckerpunch|Jolly|,252,4,,,252||||50|,,,,,Ghost]Dragonite||sharpbeak|multiscale|terablast,extremespeed,protect,stompingtantrum|Adamant|148,252,4,,4,100||||50|,,,,,Flying]Flutter Mane||choicespecs|protosynthesis|moonblast,dazzlinggleam,shadowball,psyshock|Modest|228,,92,36,4,148||,0,,,,||50|,,,,,Fairy]Talonflame||rockyhelmet|galewings|bravebird,tailwind,willowisp,quickguard|Jolly|172,4,236,,4,92||||50|,,,,,Ghost]Chi-Yu||choicescarf|beadsofruin|heatwave,darkpulse,overheat,snarl|Modest|4,,,252,,252||,0,,,,||50|,,,,,Ghost]Glimmora||assaultvest|toxicdebris|powergem,sludgebomb,mortalspin,earthpower|Modest|132,,4,100,20,252||||50|,,,,,Grass");
        await pc.AcceptChallengeAsync(player);
    }
};
string[] xc = new[] { "2345", "1623" };
pc.OnTeampreview += async (PokePSCore.PsBattle battle) =>
{
    //await pc2.ChangeYourTeamAsync("Pelipper||focussash|drizzle|hydropump,protect,wideguard,hurricane|Timid|4,,4,244,4,252||,0,,,,||50|,,,,,Flying]Flutter Mane||lifeorb|protosynthesis|moonblast,shadowball,dazzlinggleam,protect|Modest|68,,36,196,4,204||,0,,,,||50|,,,,,Fairy]Baxcalibur||loadeddice|thermalexchange|iciclecrash,iceshard,glaiverush,protect|Adamant|132,252,12,,28,84||||50|,,,,,Poison]Amoonguss||sitrusberry|regenerator|spore,protect,pollenpuff,ragepowder|Calm|236,,196,,76,||,0,,,,||50|,,,,,Steel]Palafin||mysticwater|zerotohero|haze,wavecrash,jetpunch,protect|Adamant|252,236,4,,4,12||||50|,,,,,Water]Iron Hands||assaultvest|quarkdrive|fakeout,closecombat,wildcharge,voltswitch|Adamant|76,156,12,,252,12||||50|,,,,,Grass");
    var battlea = battleana.GetValueOrDefault(battle.Tag);
    var lastTurn = battlea.battle.BattleTurns[0];
    for (int i = 0; i < battle.MyTeam.Count; i++)
    {
        for (int j = 0; j < team1.GamePokemons[i].Moves.Count; j++)
        {
            if (battle.PlayerPos ==  PlayerPos.Player1)
            {
                lastTurn.Player1Team.Pokemons[i].SelfMovesId[j] = PsMoves[team1.GamePokemons[i].Moves[j].NameEng].num;

            }
            else
            {
                lastTurn.Player2Team.Pokemons[i].SelfMovesId[j] = PsMoves[team1.GamePokemons[i].Moves[j].NameEng].num;

            }
        }
    }
    //for
    var resx = DanCore.MakeSwitch(battlea.battle.BattleTurns.Last(), (int)battle.PlayerPos + 1);
    var czcz = string.Concat(resx.Select(s => (s.Target1 + 1) + " " + s.Target2 + "\n"));
    var czcz1 = string.Concat(resx.DistinctBy(s => s.Target1).Select(s => (s.Target1 + 1)));
    await Console.Out.WriteLineAsync(czcz);
    await Console.Out.WriteLineAsync(string.Concat(resx.Select(s => s.EV + " ")));
    await battle.OrderTeamAsync(czcz1);
    // await battle.SendMessageAsync("让我康康");
    // await battle.OrderTeamAsync("123456");
    //await battle.OrderTeamAsync
    //(await AI.MakeTeamOrderAsync(config.TeamOrderPolicies, battle.OppTeam.ToArray(), battle.MyTeam.ToArray()));

    // await battle.OrderTeamAsync(xc[Random.Shared.Next(xc.Length)]);
};

pc.OnForceSwitch += async (battle, bools) =>
{
    var battlea = battleana.GetValueOrDefault(battle.Tag);

    var resx = DanCore.MakeSwitch(battlea.battle.BattleTurns.Last(), (int)battle.PlayerPos + 1);
    var czcz = string.Concat(resx.Select(s => (s.Target1 + 1) + " " + s.Target2 + "\n"));
    //await Console.Out.WriteLineAsync(czcz);
    //return;
    Console.WriteLine("让我康康你有没有触发");
    List<ChooseData> chooseDatas = new List<ChooseData>();
    for (int i = 0; i < bools.Length; i++)
    {
        if (bools[i])
        {
            int idx = -1;
            for (int j = 0; j < 4; j++)
            {
                if (battle.Actives[j] == false && !battle.MyTeam[j].IsDead)
                {
                    idx = j;
                    battle.Actives[j] = true;
                    break; ;
                }
            }
            if (idx == -1)
            {
                chooseDatas.Add(new SwitchData { IsPass = true });
            }
            else
            {
                chooseDatas.Add(new SwitchData { PokeId = idx + 1 });

            }
        }
    }

    await battle.SendMoveAsunc(chooseDatas.ToArray());
};

pc.OnChooseMove += async (PokePSCore.PsBattle battle) =>
{
    List<ChooseData> chooseDatas = new List<ChooseData>();
    
    bool dm = false;
    PSReplayAnalysis.PSReplayAnalysis battlea = battleana.GetValueOrDefault(battle.Tag);
    foreach (var item in battlea.battle.BattleTurns.Last().Player2Team.Pokemons)
    {
        Console.WriteLine($"{item.NickName} HpRemain{item.HPRemain} NowPos: {item.NowPos}");
    }
    for (int i = 0; i < battle.ActiveStatus.Length; i++)
    {
        if (!battle.Actives[i]) continue;
        var resx = DanCore.MakeChoose(battlea.battle.BattleTurns.Last(), (int)battle.PlayerPos + 1);
        var m = resx.Where(s => s.Pos == i).Take(10);

        int moveid = m.First(s => s.Pos == i && s.Target1 != 0).Target1;
        int target2 = m.First(s => s.Pos == i && s.Target1 != 0).Target2 + 1;
        if (target2 > 2) target2 = 2 - target2;
        string target;
        bool dflag = false;
        Console.WriteLine('1');
        try
        {

            target = battle.ActiveStatus[i].GetProperty("moves")[moveid - 1].GetProperty("target").GetString();

            Console.WriteLine(target);
            if (target == "any" || target == "normal" || target == "adjacentFoe")
            {
                chooseDatas.Add(new MoveChooseData(moveid , dmax: dflag) { Target = target2 });

            }
            else
            {

                chooseDatas.Add(new MoveChooseData(moveid , dmax: dflag));

            }
        }
        catch (global::System.Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("异常了");
            chooseDatas.Add(new MoveChooseData(1));
        }



    }
    await battle.SendMoveAsunc(chooseDatas.ToArray());
    return;
    //List<ChooseData> chooseDatas = new List<ChooseData>();
    //bool dm = false;
    //for (int i = 0; i < battle.ActiveStatus.Length; i++)
    //{
    //    int moveid = Random.Shared.Next(4);
    //    string target;
    //    bool dflag = false;
    //    Console.WriteLine('1');
    //    if (!dm)
    //    {
    //        if (battle.ActiveStatus[i].TryGetProperty("canDynamax", out var cdmj))
    //        {
    //            if (Random.Shared.Next(2) > 0 && cdmj.GetBoolean())
    //            {
    //                //(chooseDatas.Last() as MoveChooseData).Dmax = true;
    //                dm = true;
    //                dflag = true;
    //                battle.MySide[i].Dynamax = true;
    //            }
    //        }

    //    }
    //    try
    //    {

    //        if (battle.MySide[i].Dynamax)
    //        {
    //            Console.WriteLine(i + "这里dmax了");
    //            target = battle.ActiveStatus[i].GetProperty("maxMoves").GetProperty("maxMoves")[moveid].GetProperty("target").GetString();

    //        }
    //        else
    //        {
    //            target = battle.ActiveStatus[i].GetProperty("moves")[moveid].GetProperty("target").GetString();

    //        }
    //        Console.WriteLine(target);
    //        if (target == "any" || target == "normal" || target == "adjacentFoe")
    //        {
    //            chooseDatas.Add(new MoveChooseData(moveid + 1, dmax: dflag) { Target = Random.Shared.Next(2) + 1 });

    //        }
    //        else
    //        {

    //            chooseDatas.Add(new MoveChooseData(moveid + 1, dmax: dflag));

    //        }
    //    }
    //    catch (global::System.Exception e)
    //    {
    //        Console.WriteLine(e.Message);
    //        Console.WriteLine("异常了");
    //        chooseDatas.Add(new MoveChooseData(1));
    //    }



    //}
    //await battle.SendMoveAsunc(chooseDatas.ToArray());
    //chooseDatas.ForEach(s =>
    //{
    //    if (s is MoveChooseData)
    //    {
    //        (s as MoveChooseData).Target = Random.Shared.Next(2) + 1;
    //    }
    //});
    //await battle.SendMoveAsunc(chooseDatas.ToArray());
};
int idx = 0;
bool isSearching = false;
pc.BattleStartAction += async (PokePSCore.PsBattle battle) =>
{
    isSearching = false;
    //await battle.SendTimerOnAsync();

    //var battlea = battleana.GetValueOrDefault(battle.Tag) ?? new PSReplayAnalysis.PSReplayAnalysis() { RoomId = battle.Tag };


    //if (idx++ < 4)
    //{
    //    await pc.SearchBattleAsync("gen8vgc2022");

    //}
};
pc.BattleInfo += async (battle, b) =>
{
    // await s.Se
    var battlea = battleana.GetValueOrDefault(battle.Tag) ?? new PSReplayAnalysis.PSReplayAnalysis() { RoomId = battle.Tag };
    battleana.TryAdd(battle.Tag, battlea);
    if (battlea.battle.BattleTurns.Count == 0)
    battlea.battle.BattleTurns.Add(new BattleTurn
    {
        TurnId = 0,
        
    });
    

    battlea.Refresh(b);
};


pc.BattleEndAction += async (s, b) =>
{
    await s.LeaveRoomAsync();
    // await pc.SearchBattleAsync("gen8vgc2022");
    idx--;
};
int id = config.BattleCnt;

while (true)
{
   // await pc2.ChangeYourTeamAsync("Pelipper||focussash|drizzle|hydropump,protect,wideguard,hurricane|Timid|4,,4,244,4,252||,0,,,,||50|,,,,,Flying]Flutter Mane||lifeorb|protosynthesis|moonblast,shadowball,dazzlinggleam,protect|Modest|68,,36,196,4,204||,0,,,,||50|,,,,,Fairy]Baxcalibur||loadeddice|thermalexchange|iciclecrash,iceshard,glaiverush,protect|Adamant|132,252,12,,28,84||||50|,,,,,Poison]Amoonguss||sitrusberry|regenerator|spore,protect,pollenpuff,ragepowder|Calm|236,,196,,76,||,0,,,,||50|,,,,,Steel]Palafin||mysticwater|zerotohero|haze,wavecrash,jetpunch,protect|Adamant|252,236,4,,4,12||||50|,,,,,Water]Iron Hands||assaultvest|quarkdrive|fakeout,closecombat,wildcharge,voltswitch|Adamant|76,156,12,,252,12||||50|,,,,,Grass");
   // //await pc2.ChangeYourTeamAsync("null");
   //await  pc2.ChallengeAsync("scixing", "gen9vgc2023series2");
   //await  pc2.ChatWithIdAsync("scixing", "gen9vgc2023series2");
    await Task.Delay(200000000);
    continue;
    if (id > 0 && idx < config.OnlineCnt && !isSearching)
    {
        isSearching = true;
        //await pc.ChangeYourTeamAsync(await PSConverter.ConvertToPsOneLineAsync(team1));
        await pc.SearchBattleAsync("gen8vgc2022");
        idx++;
        id--; 
        await Task.Delay(5000);
    }
    if (id == 0 && idx == 0) break;
    //await Task.Delay(10000000);
    // await pc.GetRoomListAsync("gen8vgc2022", 1500);
    // await pc.SetAvatarAsync(id++.ToString());
}



//Console.WriteLine(torch.__version__);

////var mm = Module.Load("F:\\VSProject\\PokeDanAI\\model.pt");
//var dQN1 = torch.jit.load("F:\\VSProject\\PokeDanAI\\model.pt").cuda();
//////var dQ = torch.load("F:\\VSProject\\PokeDanAI\\model.pt").cuda();
//////var dQN = DQN.Create<DQN>("F:\\VSProject\\PokeDanAI\\model.pt").cuda();
//////dQN(torch.rand(328));
//////dQN.;
////dQN1.forward(flo);
////dQ.for

//var aa = dQN1.forward(torch.rand(548).cuda()) as torch.Tensor;
////var aaa = (aa.tolist() as Scalar[]).Select(s => s.ToDouble());
//for (int i = 0; i < aa.shape[0]; i++)
//{
//    Console.Write(aa[i].ToDouble());
//    Console.Write(" ");
//}
//return;






//// 加载一个模型
//class DQN : torch.nn.Module
//{
//    private readonly Conv2d _convLayer1;
//    private readonly Conv2d _convLayer2;
//    private readonly Flatten _flattenLayer;
//    private readonly Linear _denseLayer1;
//    private readonly Linear _outputLayer;
//    protected DQN(int numActions) : base("")
//    {
//        _convLayer1 =  Conv2d(1, 16, kernelSize: 4, stride: 1, padding: 2);
//        _convLayer2 =  Conv2d(16, 32, kernelSize: 4, stride: 2);
//        _flattenLayer =  Flatten();
//        _denseLayer1 =  Linear(4095, 256);
//        _outputLayer =  Linear(256, numActions);
//    }

//    public torch.Tensor forward(torch.Tensor x)
//    {
//        x = relu(_convLayer1.forward(x.reshape(1, 1, 32, 548)));
//        x = relu(_convLayer2.forward(x));
//        x = _flattenLayer.forward(x);
//        x = relu(_denseLayer1.forward(x));
//        return _outputLayer.forward(x);
//    }
//}


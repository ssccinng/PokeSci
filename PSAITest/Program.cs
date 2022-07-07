// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using PokeCommon.PokemonShowdownTools;
using PokePSCore;
using PSAITest;

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
}



var team1 = await PSConverter.ConvertToPokemonsAsync(config.Team);
Console.WriteLine("准备登录");

//var pc = new PSClient("scixing", "11998whs").LogTo(Console.WriteLine);
var pc = new PSClient(config.Username, config.Password).LogTo(Console.WriteLine);
await pc.ConnectAsync();
await Task.Delay(500);
Console.WriteLine(await pc.LoginAsync());
;



pc.ChallengeAction += async (player, rule) =>
{
    // if (rule == "gen8randombattle")
    //if (rule == "gen7vgc2019")
    if (rule == "gen8vgc2022")
    {
        await pc.ChatWithIdAsync(player, "随机战斗，玩了");
        await pc.ChatWithIdAsync(player, "就决定是你了");
        // await pc.ChangeYourTeamAsync("null");
        await pc.ChangeYourTeamAsync(await PSConverter.ConvertToPsOneLineAsync(team1));
        await pc.AcceptChallengeAsync(player);
    }
};
string[] xc = new[] { "234516", "162345" };
pc.OnTeampreview += async battle =>
{
    // await battle.SendMessageAsync("让我康康");
    // await battle.OrderTeamAsync("123456");
    await battle.OrderTeamAsync(xc[Random.Shared.Next(xc.Length)]);
};

pc.OnForceSwitch += async (battle, bools) =>
{
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
                    break;;
                }
            }
            if (idx == -1)
            {
                chooseDatas.Add(new SwitchData{IsPass = true});
            }
            else
            {
                chooseDatas.Add(new SwitchData{PokeId = idx + 1});

            }
        }
    }

    await battle.SendMoveAsunc(chooseDatas.ToArray());
};

pc.OnChooseMove += async battle =>
{
    List<ChooseData> chooseDatas = new List<ChooseData>();
    bool dm = false;
    for (int i = 0; i < battle.ActiveStatus.Length; i++)
    {
        int moveid = Random.Shared.Next(4);
        try
        {
            var target = battle.ActiveStatus[i].GetProperty("moves")[moveid].GetProperty("target").GetString();
            if (target == "any" || target == "normal")
            {
                chooseDatas.Add(new MoveChooseData(moveid + 1) { Target = Random.Shared.Next(2) + 1 });

            }
            else
            {

                chooseDatas.Add(new MoveChooseData(moveid + 1));

            }
        }
        catch (global::System.Exception)
        {
            chooseDatas.Add(new MoveChooseData(1));
        }
        

        //if (!dm)
        //{
        //    if (battle.ActiveStatus[i].TryGetProperty("canDynamax", out var cdmj))
        //    {
        //        if (Random.Shared.Next(2) > 0 && cdmj.GetBoolean())
        //        {
        //            (chooseDatas.Last() as MoveChooseData).Dmax = true;
        //            dm = true;
        //        }
        //    }
            
        //}
    }
    await battle.SendMoveAsunc(chooseDatas.ToArray());
    //chooseDatas.ForEach(s =>
    //{
    //    if (s is MoveChooseData)
    //    {
    //        (s as MoveChooseData).Target = Random.Shared.Next(2) + 1;
    //    }
    //});
    //await battle.SendMoveAsunc(chooseDatas.ToArray());
};
//int idx = 0;
//pc.BattleStartAction += async battle =>
//{
//    if (idx++ < 4)
//    {
//        await battle.SendTimerOnAsync();
//        await pc.SearchBattleAsync("gen8vgc2022");

//    }
//};
//await pc.ChangeYourTeamAsync(await PSConverter.ConvertToPsOneLineAsync(team1));
//// start再加一个事件
//await pc.SearchBattleAsync("gen8vgc2022");


//pc.BattleEndAction += async (s, b) =>
//{
//    await s.LeaveRoomAsync();
//    //await pc.SearchBattleAsync("gen8vgc2022");
//    idx--;
//};
int id = 200;
while (true)
{
    await Task.Delay(10000000);
    await pc.GetRoomListAsync("gen8vgc2022", 1500);
    // await pc.SetAvatarAsync(id++.ToString());
}
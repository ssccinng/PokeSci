﻿// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Masuda.Net.Models;
using Mirai.Net.Sessions;
using Mirai.Net.Sessions.Http.Managers;

using Masuda.Net;
using static System.Formats.Asn1.AsnWriter;
using System.Text.RegularExpressions;

BotSetting BotSetting = new BotSetting
{
    AppId = 101987320,
    AppKey = "",
    Token = "rVGzh6ulSTV3B9fOrgxNqt3bD4rC6mZz",
    Intents = new[] { Intent.NORMAL_MESSAGES, Intent.GUILD_MEMBERS, Intent.GUILDS, Intent.DIRECT_MESSAGE },
    SandBox = false,
    Log = true,
    // 为-1则默认不分片
    ShardId = -1
};

MasudaBot MasudaBot
    = new MasudaBot(BotSetting).LogTo(Console.WriteLine);
//using System.Text.Json.;
var miraiBot = new MiraiBot
{
    Address = "127.0.0.1:8080",
    //Address = "43.154.235.6:8080",
    QQ = "3084029286",
    VerifyKey = "INITKEYmpZ1FAXk",
};
await miraiBot.LaunchAsync();

PokePSCore.PSClient pSClient = new("testttt", "");
await pSClient.ConnectAsync();


Dictionary<string, Dictionary<string, double>> keys = new();
HashSet<string> ids = new();
HashSet<string> IdCheck = new HashSet<string>
{
    //"scixing",
    "Let's go veyle",
    //"moooob",
    //"mediocrisy",
    //"ZoryaPet",
//"justin tang fan",
//"Nakafuyuyuki",
//"zacianisdumb",
//"Sportaholic",
//"bigmansweep",
//"Feyette",
//"Yankiken",
//"sunside",
//"nguoimoi",
//"kennec"


"LenVGC",
"PokeAlex",
"Miraai1",
"KastyTP",
"SokaVGC",
"itakureig",
"JMOdri",
"Pipagoro",
"icebergvgc",
"pincurchienpao",
"Torviv",
"Rayko",
"RaykoVGC",
"scesere",
"idbgnf",
"tericorum",
"trickroomtest33",

//"julietterainha",
//"arnaldandtironi",
//"counterdoksvkids",
//"yansalibianvgc",
//"barcelonacheese",
//"vovomaxben10",
//"peidodesuvaco",
//"cabodaciolocrvg",
//"mistergxz",
//"pletesukovgc",



"fricassedefrango", "sekoeappa", "ohptdm", "queiroga", "worldscts", "zorieuq", "z2rqueiroz", "noonblast", "ksvstandproud", "dustpardon",

"paquiav", "dondopon",    "venyballester","kanchomesama",
"marianocuesta21", "nutria420"
    //"terabolin",
    //"Lordoki",
    //"Cykomore",
    //"thead",
    //"Electhor94",
    //"Mashazard",
    //"RdmH3",
    //"yale-a",
};
IdCheck = IdCheck.Select(x => Regex.Replace(x, "[^A-Za-z]", "").ToLower()).ToHashSet();


var guilds = await MasudaBot.GetMeGuildsAsync();
var channels = await MasudaBot.GetChannelsAsync(guilds.First().Id);

var sjb = channels.FirstOrDefault(s => s.Name.Contains("世界杯")).Id;

HashSet<string> Keys = new();
pSClient.UserDetailsAction += async (msg) =>
{
    var msg1 = JsonDocument.Parse(msg).RootElement;
    var rooms = msg1.GetProperty("rooms");
    var id = msg1.GetProperty("id").GetString();
    var userid = msg1.GetProperty("userid").GetString();
    await Console.Out.WriteLineAsync(msg);
    if (rooms.ValueKind == JsonValueKind.False)
    {
        // 下线
        if (keys.Keys.Contains(id))
        {

            keys.Remove(id);
            //await MessageManager.SendGroupMessageAsync("812028610", $"{id}下线");
            await MasudaBot.SendMessageAsync(sjb, $"{id}下线");
        }
    }
    else
    {
        foreach (var room in rooms.EnumerateObject())
        {
            if (room.Name.Contains("battle") && !Keys.Contains(room.Name))
            {
                Console.WriteLine(room.Name);
                Keys.Add(room.Name);
                //await MessageManager.SendGroupMessageAsync("812028610", $"{id}正在房间{room.Name}中！");
                await MessageManager.SendFriendMessageAsync("516408513", $"{id}正在房间{room.Name}中！");
                await MasudaBot.SendMessageAsync(sjb, $"{id}正在房间{room.Name}中！");


            }
        }

        var rankData = await pSClient.GetRankAsync(id);
        var ss = rankData.Where(s => s.FormatId.Contains("gen9vgc2023"));
        Console.WriteLine(ss.Count());
        //string score = ?.ELO ?? "0";
        // 在线
        if (!keys.Keys.Contains(id))
        {
            keys.Add(id, new());
            foreach (var s in ss)
            {
                keys[id][s.FormatId] = s.ELO;
            }
            //await MessageManager.SendGroupMessageAsync("812028610", $"{id}已上线！");
            //await MessageManager.SendGroupMessageAsync("812028610", $"{id}已上线！");
            await MasudaBot.SendMessageAsync(sjb, $"{id}已上线！");

        }
        //else
        {
            foreach (var s in ss)
            {
                if (!keys[id].ContainsKey(s.FormatId))
                {
                    keys[id].TryAdd(s.FormatId, s.ELO);

                }
                else
                {
                    var score = s.ELO;
                    Console.WriteLine(score);
                    if (score != keys[id][s.FormatId])
                    {
                        //await MessageManager.SendGroupMessageAsync("812028610", $"{id} {s.FormatId} 分数变化！{keys[id]} -> {score}");
                        await MessageManager.SendFriendMessageAsync("516408513", $"{id} {s.FormatId} 分数变化！{keys[id][s.FormatId]} -> {score}");
                        await MasudaBot.SendMessageAsync(sjb, $"{id} {s.FormatId} 分数变化！{keys[id][s.FormatId]} -> {score}");

                        await Console.Out.WriteLineAsync("分数变化");
                        keys[id][s.FormatId] = score;
                    }

                }
            }
            if (id != userid && !ids.Contains(userid))
            {
                ids.Add(userid);
                IdCheck.Add(userid);
                //await MessageManager.SendGroupMessageAsync("812028610", $"{id} id变化！{id} -> {userid}");
                await MessageManager.SendFriendMessageAsync("516408513", $"{id} id变化！{id} -> {userid}");

                await MasudaBot.SendMessageAsync(sjb, $"{id} id变化！{id} -> {userid}");

            }

        }
    }
};



while (true)
{
    //await pSClient.GetUserDetails("scixing, Ethox");
    ////await pSClient.GetUserDetails("Ethox");
    ////await pSClient.GetUserDetails("RedsilverFR");
    //await Task.Delay(1000 * 30);
    //continue;
    foreach (var item in IdCheck.ToList())
    {
        await pSClient.GetUserDetails(item);
        await Task.Delay(1000);


    }

}

Console.ReadLine();
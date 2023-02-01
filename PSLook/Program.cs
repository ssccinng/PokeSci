// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using Mirai.Net.Sessions;
using Mirai.Net.Sessions.Http.Managers;
//using System.Text.Json.;
var miraiBot = new MiraiBot
{
    Address = "43.154.235.6:8080",
    QQ = "3084029286",
    VerifyKey = "INITKEYmpZ1FAXk",
};
miraiBot.LaunchAsync().Wait();

PokePSCore.PSClient pSClient = new("testttt", "");
await pSClient.ConnectAsync();


Dictionary<string, string> keys = new();
HashSet<string> ids = new();
HashSet<string> IdCheck = new HashSet<string>
{
    "scixing",
    "Ethox",
    "RedsilverFR",
    "Lordoki",
    "Cykomore",
    "thead",
    "Electhor94",
    "Mashazard",
    "RdmH3",
    "yale-a",
};
pSClient.UserDetailsAction += async (msg) =>
{
    var msg1 = JsonDocument.Parse(msg).RootElement;
    var rooms = msg1.GetProperty("rooms");
    var id = msg1.GetProperty("id").GetString();
    var userid = msg1.GetProperty("userid").GetString();

    if (rooms.ValueKind == JsonValueKind.False)
    {
        // 下线
        if (keys.Keys.Contains(id))
        {

            keys.Remove(id);
            await MessageManager.SendGroupMessageAsync("770272713", $"{id}下线");

        }
    }
    else
    {
        var rankData = await pSClient.GetRankAsync(id);
        string score = rankData.FirstOrDefault(s => s.FormatId == "gen8vgc2022")?.ELO ?? "0";
        // 在线
        if (!keys.Keys.Contains(id))
        {
            keys.Add(id, score);
            await MessageManager.SendGroupMessageAsync("770272713", $"{id}已上线！");
        }
        else
        {
            if (score != keys[id])
            {
                await MessageManager.SendGroupMessageAsync("770272713", $"{id} 分数变化！{keys[id]} -> {score}");
                keys[id] = score;
            }
            if (id != userid && !ids.Contains(userid))
            {
                ids.Add(userid);
                IdCheck.Add(userid);
                await MessageManager.SendGroupMessageAsync("770272713", $"{id} id变化！{id} -> {userid}");
            }
        }
    }
};



while (true)
{
    //await pSClient.GetUserDetails("scixing");
    //await pSClient.GetUserDetails("Ethox");
    //await pSClient.GetUserDetails("RedsilverFR");

    foreach (var item in IdCheck.ToList())
    {
        await pSClient.GetUserDetails(item);
        await Task.Delay(1000);


    }
    await Task.Delay(1000 * 30);

}

Console.ReadLine();
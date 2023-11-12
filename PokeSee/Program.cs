// See https://aka.ms/new-console-template for more information



using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;

var topUsers = await GetTopUser();
foreach (var item in topUsers)
{
    Console.WriteLine(item);
}

PokePSCore.PSClient pSClient = new("testttt", "");
await pSClient.ConnectAsync();

SemaphoreSlim _semaphore = new(1);

Dictionary<string, string> keys = new();
HashSet<string> ids = new();


pSClient.UserDetailsAction += async (msg) =>
{
    await Console.Out.WriteLineAsync(msg);
    try
    {


    var msg1 = JsonDocument.Parse(msg).RootElement;
    var rooms = msg1.GetProperty("rooms");
    var id = msg1.GetProperty("id").GetString();
    var userid = msg1.GetProperty("userid").GetString();


        if (id != userid && !ids.Contains(userid))
        {
            ids.Add(userid);
            if (userid.StartsWith("guest", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }
            WriteToFile($"{DateTime.Now:yyyy-MM-dd}.txt", $"{id} id变化！{id} -> {userid}\n");
            //IdCheck.Add(userid);
            //await MessageManager.SendGroupMessageAsync("770272713", $"{id} id变化！{id} -> {userid}");
        }
        if (rooms.ValueKind == JsonValueKind.False)
    {
        // 下线
        if (keys.Keys.Contains(id))
        {

            keys.Remove(id);
            //await MessageManager.SendGroupMessageAsync("770272713", $"{id}下线");

        }
    }
    else
    {
        //var rankData = await pSClient.GetRankAsync(id);
        //string score = rankData.FirstOrDefault(s => s.FormatId == "gen9vgc2023regulationd")?.ELO ?? "0";
        string score = "0";
        // 在线
        if (!keys.Keys.Contains(id))
        {
            keys.Add(id, score);
            //await MessageManager.SendGroupMessageAsync("770272713", $"{id}已上线！");
        }
        else
        {
            if (score != keys[id])
            {
                //await MessageManager.SendGroupMessageAsync("770272713", $"{id} 分数变化！{keys[id]} -> {score}");
                keys[id] = score;
            }
            if (id != userid && !ids.Contains(userid))
            {
                ids.Add(userid);
                //WriteToFile("dani.txt", $"{id} id变化！{id} -> {userid}\n");
                WriteToFile($"{DateTime.Now:yyyy-MM-dd}.txt", $"{id} id变化！{id} -> {userid}\n");

                    //IdCheck.Add(userid);
                    //await MessageManager.SendGroupMessageAsync("770272713", $"{id} id变化！{id} -> {userid}");
                }
            }
    }
    }
    catch (Exception ex)
    {

        await Console.Out.WriteLineAsync(ex.Message);
    }
};




while (true)
{
    //await pSClient.GetUserDetails("scixing, Ethox");
    //await pSClient.GetUserDetails("Ethox");
    //await pSClient.GetUserDetails("RedsilverFR");
    //continue;
    foreach (var item in topUsers.ToList())
    {
        await pSClient.GetUserDetails(item);
        await Task.Delay(1500);


    }
    await Task.Delay(500 * 30);

    foreach (var item in topUsers.ToList())
    {
        await pSClient.GetUserDetails(item);
        await Task.Delay(1000);


    }
    await Task.Delay(500 * 30);

    topUsers = await GetTopUser();

}


static async Task<List<string>> GetTopUser()
{

    string[] forceUser = new[] { "BanquetforFoxes" };
    Regex reg = new Regex(@"<td>\d+</td><td>(.+?)</td>");

    using HttpClient client = new();
    HashSet<string> users = new();
    
    foreach (string user in forceUser)
    {
        users.Add(user);
    }
    string url = "https://play.pokemonshowdown.com/ladder.php?format=gen9vgc2023regulationd&server=showdown&output=html&prefix=";
    var res = await client.GetAsync(url);
    var html = await res.Content.ReadAsStringAsync();

    var matchRes = reg.Matches(html);
    foreach (Match item in matchRes)
    {
        var aa = Regex.Replace(item.Groups[1].Value, @"$#\d+?;", "");
        aa = Regex.Replace(aa.ToLower(), @"[^a-z0-9]", "");
        users.Add(aa);
    }
    url = "https://play.pokemonshowdown.com/ladder.php?format=gen9vgc2023regulatione&server=showdown&output=html&prefix=";

    res = await client.GetAsync(url);
    html = await res.Content.ReadAsStringAsync();

    matchRes = reg.Matches(html);
    foreach (Match item in matchRes)
    {
        var aa = Regex.Replace(item.Groups[1].Value, @"$#\d+?;", "");
        aa = Regex.Replace(aa.ToLower(), @"[^a-z0-9]", "");
        users.Add(aa);
    }

    url = "https://play.pokemonshowdown.com/ladder.php?format=gen9vgc2023regulationebo3&server=showdown&output=html&prefix=";

    res = await client.GetAsync(url);
    html = await res.Content.ReadAsStringAsync();

    matchRes = reg.Matches(html);
    foreach (Match item in matchRes)
    {
        var aa = Regex.Replace(item.Groups[1].Value, @"$#\d+?;", "");
        aa = Regex.Replace(aa.ToLower(), @"[^a-z0-9]", "");
        users.Add(aa);
    }

    return users.ToList();

}


 void WriteToFile(string path, string data)
{
    _semaphore.Wait();
    try
    {
        File.AppendAllText(path, data);
    }
    finally
    {
        _semaphore.Release();
    }
}
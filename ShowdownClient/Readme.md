# PokemonShowdown客户端

## 使用简介

```cs
AIConfig config = new AIConfig();
PokemonTools.PokemonContext = new PokemonContext();

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
Console.WriteLine("准备登录");

//var pc = new PSClient("scixing", "11998whs").LogTo(Console.WriteLine);
var pc = new ShowdownClient(ClientInfo.Create(config.Username, config.Password));//.LogTo(Console.WriteLine);
await pc.ConnectAsync();
await Task.Delay(500);
Console.WriteLine(await pc.LoginAsync());
```
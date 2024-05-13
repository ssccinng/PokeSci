// See https://aka.ms/new-console-template for more information
using Poke.Usage;
using PokeCommon.API.Data;
using PokeCommon.Models;
using PokeCommon.PokemonShowdownTools;
using PokeCommon.Utils;
using PokemonDataAccess;
using System.Collections.Concurrent;
using System.Text.Json;

PokemonTools.PokemonContext = new PokeDBContext();

//goto usage;

//var aa = await PokemonTools.GetPokemonAsync(791);
var pp = await RK9Tool.RK9Client.GetMatchPairings("EU02whGbj7Vqpe87mRZT");

return;


var players = await RK9Tool.RK9Client.GetMatchPlayers("PER02wgSg7uJW7d8FwUe");


var dd1 = players.Where(s => s.Division == "Masters");

GamePokemonTeam[] gamePokemonTeams = new GamePokemonTeam[players.Count()];
//var dd = await RK9Tool.RK9Client.GetPokemonTeamAsync("/teamlist/public/PER02wgSg7uJW7d8FwUe/1jMIAIT8W9x4Wm9HJKV3");
//return;
File.WriteAllText("players.json", JsonSerializer.Serialize(dd1));
//var aa = await PSConverter.ConvertToPsAsync(dd);


List<Task> tasks = new List<Task>();

int i = 0;

//foreach (var player in players.Where(s => s.Division == "Masters"))
foreach (var player in players)
{
    int ii = i;
    tasks.Add(Task.Run(async () =>
    {
        var team = await RK9Tool.RK9Client.GetPokemonTeamAsync(player.TeamListUrl);
        gamePokemonTeams[ii] = team;
    }));

    i++;

    if (i % 10 == 0)
    {
        await Task.WhenAll(tasks);
        tasks.Clear();
    }
    //var team = await RK9Tool.RK9Client.GetPokemonTeamAsync(player.TeamListUrl);
    //gamePokemonTeams.Add(team);
}

await Task.WhenAll(tasks);

File.WriteAllText("teams.json", JsonSerializer.Serialize(gamePokemonTeams));
return;
usage:


gamePokemonTeams = JsonSerializer.Deserialize<GamePokemonTeam[]>(File.ReadAllText("teams.json"));



var usage = UsageHelper.GetUsage(gamePokemonTeams.Where(s => s is not null));
var usageDex = UsageHelper.GetUsageByDexId(gamePokemonTeams.Where(s => s is not null));

File.WriteAllText("usage.txt", await usage.ToTextAsync());
File.WriteAllText("usageDex.txt", await usageDex.ToTextAsync(isDex: true));

//foreach (var pokemonUsage in usage.PokemonUsage)
//{
//    var pokemon = await PokemonTools.GetPokemonAsync(pokemonUsage.Id);
//    Console.WriteLine($"{pokemon.FullNameChs} {pokemonUsage.Count} {pokemonUsage.Percentage:P}");

//    Console.WriteLine("技能使用率:");
//    foreach (var moveUsage in pokemonUsage.MoveUsage)
//    {
//        var move = await PokemonTools.GetMoveAsync(moveUsage.Id);
//        Console.WriteLine($"    {move.Name_Chs} {moveUsage.Count} {moveUsage.Percentage:P}");
//    }
//    Console.WriteLine();
//    Console.WriteLine();
//    Console.WriteLine("同伴使用率:");
//    foreach (var aliyPokemonUsage in pokemonUsage.AliyPokemonUsage)
//    {
//        var aliyPokemon = await PokemonTools.GetPokemonAsync(aliyPokemonUsage.Id);
//        Console.WriteLine($"    {aliyPokemon.FullNameChs} {aliyPokemonUsage.Count} {aliyPokemonUsage.Percentage:P}");
//    }


//    Console.WriteLine();
//    Console.WriteLine();
//    Console.WriteLine("道具使用率:");
//    foreach (var itemUsage in pokemonUsage.ItemUsage)
//    {
//        var item = await PokemonTools.GetItemAsync(itemUsage.Id);
//        Console.WriteLine($"    {item.Name_Chs} {itemUsage.Count} {itemUsage.Percentage:P}");
//    }

//    Console.WriteLine();
//    Console.WriteLine();
//    Console.WriteLine("特性使用率:");
//    foreach (var abilityUsage in pokemonUsage.AbilityUsage)
//    {
//        var ability = await PokemonTools.GetAbilityAsync(abilityUsage.Id);
//        Console.WriteLine($"    {ability.Name_Chs} {abilityUsage.Count} {abilityUsage.Percentage:P}");
//    }
//    Console.WriteLine();
//    Console.WriteLine();
//    Console.WriteLine();
//    Console.WriteLine();
//    Console.WriteLine();
//    Console.WriteLine();
//}

File.WriteAllText("usage.json", JsonSerializer.Serialize(usage));





//var list = await RK9Tool.RK9Client.GetPokemonTeamAsync("/teamlist/public/IND02mCwIuhUMs3NdM7W/01nPWypsGF0sxi1CqtD6");
//Console.WriteLine(PSConverter.ConvertToPsAsync(list));



//File.WriteAllText("parings.json", JsonSerializer.Serialize(list));
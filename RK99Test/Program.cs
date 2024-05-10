// See https://aka.ms/new-console-template for more information
using Poke.Usage;
using PokeCommon.API.Data;
using PokeCommon.Models;
using PokeCommon.PokemonShowdownTools;
using PokeCommon.Utils;
using PokemonDataAccess;
using System.Text.Json;

PokemonTools.PokemonContext = new PokeDBContext();

//var aa = await PokemonTools.GetPokemonAsync(791);
var players = await RK9Tool.RK9Client.GetMatchPlayers("PER02wgSg7uJW7d8FwUe");

List<GamePokemonTeam> gamePokemonTeams = new List<GamePokemonTeam>();

foreach (var player in players.Where(s => s.Division == "Masters"))
{
    var team = await RK9Tool.RK9Client.GetPokemonTeamAsync(player.TeamListUrl);
    gamePokemonTeams.Add(team);
}

File.WriteAllText("teams.json", JsonSerializer.Serialize(gamePokemonTeams));


var usage = UsageHelper.GetUsage(gamePokemonTeams);

foreach (var pokemonUsage in usage.PokemonUsage)
{
    var pokemon = await PokemonTools.GetPokemonAsync(pokemonUsage.Id);
    Console.WriteLine($"{pokemon.NameChs} {pokemonUsage.Count}");
    foreach (var moveUsage in pokemonUsage.MoveUsage)
    {
        var move = await PokemonTools.GetMoveAsync(moveUsage.Id);
        Console.WriteLine($"    {move.Name_Chs} {moveUsage.Count}");
    }
    foreach (var aliyPokemonUsage in pokemonUsage.AliyPokemonUsage)
    {
        var aliyPokemon = await PokemonTools.GetPokemonAsync(aliyPokemonUsage.Id);
        Console.WriteLine($"    {aliyPokemon.NameChs} {aliyPokemonUsage.Count}");
    }

    foreach (var itemUsage in pokemonUsage.ItemUsage)
    {
        var item = await PokemonTools.GetItemAsync(itemUsage.Id);
        Console.WriteLine($"    {item.Name_Chs} {itemUsage.Count}");
    }

    foreach (var abilityUsage in pokemonUsage.AbilityUsage)
    {
        var ability = await PokemonTools.GetAbilityAsync(abilityUsage.Id);
        Console.WriteLine($"    {ability.Name_Chs} {abilityUsage.Count}");
    }

}

File.WriteAllText("usage.json", JsonSerializer.Serialize(usage));





//var list = await RK9Tool.RK9Client.GetPokemonTeamAsync("/teamlist/public/IND02mCwIuhUMs3NdM7W/01nPWypsGF0sxi1CqtD6");
//Console.WriteLine(PSConverter.ConvertToPsAsync(list));



//File.WriteAllText("parings.json", JsonSerializer.Serialize(list));
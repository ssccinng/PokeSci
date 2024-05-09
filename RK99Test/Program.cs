// See https://aka.ms/new-console-template for more information
using PokeCommon.API.Data;
using PokeCommon.PokemonShowdownTools;
using PokeCommon.Utils;
using PokemonDataAccess;
using System.Text.Json;

PokemonTools.PokemonContext = new PokeDBContext();

var aa = await PokemonTools.GetPokemonAsync(791);

var list = await RK9Tool.RK9Client.GetPokemonTeamAsync("/teamlist/public/IND02mCwIuhUMs3NdM7W/01nPWypsGF0sxi1CqtD6");
Console.WriteLine(PSConverter.ConvertToPsAsync(list));

//File.WriteAllText("parings.json", JsonSerializer.Serialize(list));
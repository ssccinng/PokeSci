// See https://aka.ms/new-console-template for more information
using PokeCommon.PokemonHome;

Console.WriteLine("Hello, World!");

var data = await PKHomeUtils.GetSVPokemonHomeSessionsAsync();

await PKHomeUtils.GetSVPokemonRankdataAsync(data.Last());

return;
//var aa = await pKHomeUtils.GetBundleAsync();

//File.WriteAllBytes("dani.js", aa);
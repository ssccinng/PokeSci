// See https://aka.ms/new-console-template for more information
using PokeCommon.PokemonHome;

Console.WriteLine("Hello, World!");
var aa = await PKHomeUtils.GetBundleAsync();

File.WriteAllBytes("bundle.js", aa);
return;

//await PKHomeUtils.UpdateAll();
//return;

var data = await PKHomeUtils.GetSVPokemonHomeSessionsAsync();

var cc = await PKHomeUtils.GetSVTrainerDataAsync(data[1]);

return;
//var aa = await pKHomeUtils.GetBundleAsync();

//File.WriteAllBytes("dani.js", aa);
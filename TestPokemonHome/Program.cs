// See https://aka.ms/new-console-template for more information
using PokeCommon.PokemonHome;

Console.WriteLine("Hello, World!");


PKHomeUtils pKHomeUtils = new PKHomeUtils();
var aa = await pKHomeUtils.GetBundleAsync();

File.WriteAllBytes("dani.js", aa);
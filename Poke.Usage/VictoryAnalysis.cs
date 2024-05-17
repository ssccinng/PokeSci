using PokeCommon.BattleEngine;
using PokeCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Poke.Usage
{
    public static class VictoryAnalysis
    {
        public static Dictionary<int, PokemonVictory> GetPokemonVictories(IEnumerable<PokeBattleInfo> pokeBattles)
        {
            var pokemonVictories = new Dictionary<int, PokemonVictory>();
            foreach (var item in pokeBattles)
            {
                foreach (var poke1 in item.Player1Team.GamePokemons)
                {
                    if (!pokemonVictories.ContainsKey(poke1.PokemonId))
                    {
                        pokemonVictories[poke1.PokemonId] = new PokemonVictory { Id = poke1.PokemonId };
                    }
                    var pokemonVictory = pokemonVictories[poke1.PokemonId];
                    pokemonVictory.WinCount += item.Result == 1 ? 1 : 0;
                    pokemonVictory.LoseCount += item.Result == 2 ? 1 : 0;
                    pokemonVictory.DrawCount += item.Result == 0 ? 1 : 0;

                    pokemonVictory.Players.Add(item.Player1Index);



                    foreach (var poke2 in item.Player2Team.GamePokemons)
                    {
                        if (!pokemonVictory.OppPokemon.Any(s => s.Id == poke2.PokemonId))
                        {
                            pokemonVictory.OppPokemon.Add(new VictoryItem { Id = poke2.PokemonId });
                        }
                        var oppPokemon = pokemonVictory.OppPokemon.First(s => s.Id == poke2.PokemonId);
                        oppPokemon.WinCount += item.Result == 2 ? 1 : 0;
                        oppPokemon.LoseCount += item.Result == 1 ? 1 : 0;
                        oppPokemon.DrawCount += item.Result == 0 ? 1 : 0;



                        if (!pokemonVictories.ContainsKey(poke2.PokemonId))
                        {
                            pokemonVictories[poke2.PokemonId] = new PokemonVictory { Id = poke2.PokemonId };
                        }
                        var pokemonVictory2 = pokemonVictories[poke2.PokemonId];

                        if (!pokemonVictory2.OppPokemon.Any(s => s.Id == poke1.PokemonId))
                        {
                            pokemonVictory2.OppPokemon.Add(new VictoryItem { Id = poke1.PokemonId });
                        }
                        var oppPokemon2 = pokemonVictory2.OppPokemon.First(s => s.Id == poke1.PokemonId);
                        oppPokemon2.WinCount += item.Result == 1 ? 1 : 0;
                        oppPokemon2.LoseCount += item.Result == 2 ? 1 : 0;
                        oppPokemon2.DrawCount += item.Result == 0 ? 1 : 0;



                    }
                
                    
                }

                foreach (var poke2 in item.Player2Team.GamePokemons)
                {
                    var pokemonVictory2 = pokemonVictories[poke2.PokemonId];

                    pokemonVictory2.WinCount += item.Result == 2 ? 1 : 0;
                    pokemonVictory2.LoseCount += item.Result == 1 ? 1 : 0;
                    pokemonVictory2.DrawCount += item.Result == 0 ? 1 : 0;

                    pokemonVictory2.Players.Add(item.Player2Index);

                }
            }
            
            foreach (var item in pokemonVictories.Values)
            {
                item.WinRate = item.WinCount * 1.0 / (item.WinCount + item.LoseCount + item.DrawCount);
                foreach (var opp in item.OppPokemon)
                {
                    opp.WinRate = opp.WinCount * 1.0 / (opp.WinCount + opp.LoseCount + opp.DrawCount);
                }
            }

            return pokemonVictories;
        }
    }

    public class PokemonVictory: VictoryItem
    {
        public List<VictoryItem> OppPokemon { get; set; } = [];

        public HashSet<int> Players { get; set; } = [];
        
    }

    public class VictoryItem
    {
        public int Id { get; set; }
        public int WinCount { get; set; }
        public int LoseCount { get; set; }
        public int DrawCount { get; set; }

        public double WinRate { get; set; }
    }
}

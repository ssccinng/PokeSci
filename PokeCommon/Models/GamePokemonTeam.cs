using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.Models
{
    public class GamePokemonTeam
    {
        public List<GamePokemon> GamePokemons { get; set; } = new();
        public string TrainerName { get; set; } = "zqd";
        public int Count => GamePokemons.Count;
    }
}

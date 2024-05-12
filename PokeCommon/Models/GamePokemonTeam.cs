using System.Text.Json.Serialization;

namespace PokeCommon.Models
{
    public class GamePokemonTeam
    {
        public List<GamePokemon> GamePokemons { get; set; } = new();
        public string TrainerName { get; set; } = "zqd";
        [JsonIgnore]
        public int Count => GamePokemons.Count;

        public SimpleGamePokemonTeam ToSimpleGamePokemonTeam()
        {
            var simpleGamePokemonTeam = new SimpleGamePokemonTeam();
            foreach (var gamePokemon in GamePokemons)
            {
                simpleGamePokemonTeam.GamePokemons.Add(gamePokemon.ToSimple());
            }
            simpleGamePokemonTeam.TrainerName = TrainerName;
            return simpleGamePokemonTeam;
        }

        public List<SimpleGamePokemon> ToSimpleGamePokemons()
        {
            var simpleGamePokemons = new List<SimpleGamePokemon>();
            foreach (var gamePokemon in GamePokemons)
            {
                simpleGamePokemons.Add(gamePokemon.ToSimple());
            }
            return simpleGamePokemons;
        }
    }

    public class SimpleGamePokemonTeam
    {
        public List<SimpleGamePokemon> GamePokemons { get; set; } = new();
        public string TrainerName { get; set; } = "zqd";
        [JsonIgnore]

        public int Count => GamePokemons.Count;
    }
}

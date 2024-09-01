using MessagePack;
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
    [MessagePackObject]

    public class SimpleGamePokemonTeam
    {
        [Key(0)] public List<SimpleGamePokemon> GamePokemons { get; set; } = new();
        [Key(1)] public string TrainerName { get; set; } = "zqd";
        [JsonIgnore]
        [Key(2)]
        public int Count => GamePokemons.Count;

        public async Task<GamePokemonTeam> ToGamePokemonTeam()
        {
            var gamePokemonTeam = new GamePokemonTeam();
            foreach (var simpleGamePokemon in GamePokemons)
            {
                gamePokemonTeam.GamePokemons.Add(await simpleGamePokemon.ToGamePokemon());
            }
            gamePokemonTeam.TrainerName = TrainerName;
            return gamePokemonTeam;
        }
    }
}

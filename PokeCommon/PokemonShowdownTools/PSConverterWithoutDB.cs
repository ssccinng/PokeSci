using PokeCommon.Models;
using PokeCommon.Utils;

namespace PokeCommon.PokemonShowdownTools
{
    public class PSConverterWithoutDB
    {
        private static readonly ShowdownDataResolver Resolver = new(
            PokemonToolsWithoutDB.GetPokemonFromPsNameAsync,
            PokemonToolsWithoutDB.GetItemAsync,
            PokemonToolsWithoutDB.GetAbilityAsync,
            PokemonToolsWithoutDB.GetTypeAsync,
            PokemonToolsWithoutDB.GetMoveAsync,
            PokemonToolsWithoutDB.GetNatureAsync,
            PokemonToolsWithoutDB.GetNatureAsync);

        public static async ValueTask<string> ConvertToPsAsync(GamePokemon gamePokemon)
        {
            return await ShowdownFormatSerializer.SerializePokemonAsync(
                gamePokemon,
                PokemonToolsWithoutDB.GetPsPokemonAsync,
                ShowdownTextLanguage.English);
        }

        public static async ValueTask<string> ConvertToPsAsync(GamePokemonTeam gamePokemonTeam)
        {
            return await ShowdownFormatSerializer.SerializeTeamAsync(
                gamePokemonTeam,
                pokemon => ConvertToPsAsync(pokemon!));
        }

        public static async Task<GamePokemon?> ConvertToPokemonAsync(string PStext)
        {
            return await ShowdownFormatParser.ParsePokemonAsync(PStext, Resolver);
        }

        public static async Task<GamePokemonTeam> ConvertToPokemonsAsync(string PStext, int pad = 0)
        {
            return await ShowdownFormatParser.ParseTeamAsync(PStext, pad, Resolver);
        }

        public static async ValueTask<string> ConvertToPsOneLineAsync(GamePokemonTeam gamePokemonTeam)
        {
            return await ShowdownFormatSerializer.SerializePackedTeamAsync(
                gamePokemonTeam,
                pokemon => ConvertToPsOneLineAsync(pokemon!));
        }

        public static async ValueTask<string> ConvertToPsOneLineAsync(GamePokemon gamePokemon)
        {
            return await ShowdownFormatSerializer.SerializePackedPokemonAsync(
                gamePokemon,
                PokemonToolsWithoutDB.GetPsPokemonAsync);
        }

        public static async ValueTask<GamePokemon> ConvertFromPsOneLineAsync(string oneLine)
        {
            return await ShowdownFormatParser.ParsePackedPokemonAsync(oneLine, Resolver);
        }

        public static async ValueTask<GamePokemonTeam> ConvertTeamFromPsOneLineAsync(string oneLine)
        {
            return await ShowdownFormatParser.ParsePackedTeamAsync(oneLine, Resolver);
        }
    }
}

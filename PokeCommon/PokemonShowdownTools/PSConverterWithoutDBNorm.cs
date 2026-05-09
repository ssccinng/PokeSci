using PokeCommon.Models;
using PokeCommon.Utils;

namespace PokeCommon.PokemonShowdownTools
{
    public class PSConverterWithoutDBNorm
    {
        private static readonly ShowdownDataResolver Resolver = new(
            PokemonToolsWithoutDBNorm.GetPokemonFromPsNameAsync,
            PokemonToolsWithoutDBNorm.GetItemAsync,
            PokemonToolsWithoutDBNorm.GetAbilityAsync,
            PokemonToolsWithoutDBNorm.GetTypeAsync,
            PokemonToolsWithoutDBNorm.GetMoveAsync,
            PokemonToolsWithoutDBNorm.GetNatureAsync,
            PokemonToolsWithoutDBNorm.GetNatureAsync);

        public static async ValueTask<string> ConvertToPsAsync(GamePokemon gamePokemon)
        {
            return await ShowdownFormatSerializer.SerializePokemonAsync(
                gamePokemon,
                PokemonToolsWithoutDBNorm.GetPsPokemonAsync,
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
                PokemonToolsWithoutDBNorm.GetPsPokemonAsync);
        }

        public static async ValueTask<GamePokemon> ConvertFromPsOneLineAsync(string oneLine)
        {
            return await ShowdownFormatParser.ParsePackedPokemonAsync(oneLine, Resolver, NormalizePackedAbility);
        }

        public static async ValueTask<GamePokemonTeam> ConvertTeamFromPsOneLineAsync(string oneLine)
        {
            return await ShowdownFormatParser.ParsePackedTeamAsync(oneLine, Resolver, NormalizePackedAbility);
        }

        private static string NormalizePackedAbility(string abilityName)
        {
            return abilityName.StartsWith("AsOne", StringComparison.Ordinal) ? "AsOne" : abilityName;
        }
    }
}

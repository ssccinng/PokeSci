using PokeCommon.Interface;
using PokeCommon.Models;
using PokeCommon.Utils;

namespace PokeCommon.PokemonShowdownTools
{
    public class PSConverter : IPSConverter
    {
        private static readonly ShowdownDataResolver Resolver = new(
            PokemonTools.GetPokemonFromPsNameAsync,
            PokemonTools.GetItemAsync,
            PokemonTools.GetAbilityAsync,
            PokemonTools.GetTypeAsync,
            PokemonTools.GetMoveAsync,
            PokemonTools.GetNatureAsync,
            PokemonTools.GetNatureAsync);

        public static async ValueTask<string> ConvertToChsPsAsync(GamePokemon gamePokemon)
        {
            return await ShowdownFormatSerializer.SerializePokemonAsync(
                gamePokemon,
                PokemonTools.GetPsPokemonAsync,
                ShowdownTextLanguage.Chinese);
        }

        public static async ValueTask<string> ConvertToPsAsync(GamePokemon gamePokemon)
        {
            return await ShowdownFormatSerializer.SerializePokemonAsync(
                gamePokemon,
                PokemonTools.GetPsPokemonAsync,
                ShowdownTextLanguage.English);
        }

        public static async ValueTask<string> ConvertToPsAsync(GamePokemonTeam gamePokemonTeam, LanguageType languageType = LanguageType.ENG)
        {
            return await ShowdownFormatSerializer.SerializeTeamAsync(
                gamePokemonTeam,
                languageType == LanguageType.CHS
                    ? pokemon => ConvertToChsPsAsync(pokemon!)
                    : pokemon => ConvertToPsAsync(pokemon!));
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
                PokemonTools.GetPsPokemonAsync);
        }
    }
}

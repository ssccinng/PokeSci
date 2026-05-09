using PokemonDataAccess.Models;

namespace PokeCommon.Utils
{
    public static class PokemonToolsWithoutDBNorm
    {
        public static string NormalizeString(string input)
        {
            return PokemonDBInMemory.NormalizeName(input);
        }

        public static ValueTask<PokeType?> GetTypeAsync(int id)
        {
            return LookupById(PokemonDBInMemory.TypeById, id);
        }

        public static ValueTask<PokeType?> GetTypeAsync(string name)
        {
            return LookupByNormalizedName(PokemonDBInMemory.TypeByNormalizedName, name);
        }

        public static ValueTask<Nature?> GetNatureAsync(int id)
        {
            return LookupById(PokemonDBInMemory.NatureById, id);
        }

        public static ValueTask<Nature?> GetNatureAsync(string name)
        {
            return LookupByNormalizedName(PokemonDBInMemory.NatureByNormalizedName, name);
        }

        public static ValueTask<Pokemon?> GetPokemonFromPsNameAsync(string name)
        {
            var normalizedName = NormalizeString(name);
            if (normalizedName.Length == 0 || !PokemonDBInMemory.PokemonIdByNormalizedPsName.TryGetValue(normalizedName, out var id))
            {
                return new ValueTask<Pokemon?>((Pokemon?)null);
            }

            return GetPokemonAsync(id);
        }

        public static ValueTask<PSPokemon?> GetPsPokemonAsync(int pokemonId)
        {
            return LookupById(PokemonDBInMemory.PsPokemonByPokemonId, pokemonId);
        }

        public static ValueTask<Ability?> GetAbilityAsync(string name)
        {
            return LookupByNormalizedName(PokemonDBInMemory.AbilityByNormalizedName, name);
        }

        public static ValueTask<Ability?> GetAbilityAsync(int id)
        {
            return LookupById(PokemonDBInMemory.AbilityById, id);
        }

        public static ValueTask<Move?> GetMoveAsync(string name)
        {
            return LookupByNormalizedName(PokemonDBInMemory.MoveByNormalizedName, name);
        }

        public static ValueTask<Move?> GetMoveAsync(int id)
        {
            return LookupById(PokemonDBInMemory.MoveById, id);
        }

        public static ValueTask<Pokemon?> GetPokemonAsync(string name)
        {
            return LookupByNormalizedName(PokemonDBInMemory.PokemonByNormalizedName, name);
        }

        public static ValueTask<Pokemon?> GetPokemonAsync(int id)
        {
            return LookupById(PokemonDBInMemory.PokemonById, id);
        }

        public static ValueTask<Item?> GetItemAsync(string name)
        {
            return LookupByNormalizedName(PokemonDBInMemory.ItemByNormalizedName, name);
        }

        public static ValueTask<Item?> GetItemAsync(int id)
        {
            return LookupById(PokemonDBInMemory.ItemById, id);
        }

        private static ValueTask<T?> LookupById<T>(IReadOnlyDictionary<int, T> index, int id)
            where T : class
        {
            return new ValueTask<T?>(index.TryGetValue(id, out var value) ? value : null);
        }

        private static ValueTask<T?> LookupByNormalizedName<T>(IReadOnlyDictionary<string, T> index, string name)
            where T : class
        {
            var normalizedName = NormalizeString(name);
            if (normalizedName.Length == 0)
            {
                return new ValueTask<T?>((T?)null);
            }

            return new ValueTask<T?>(index.TryGetValue(normalizedName, out var value) ? value : null);
        }
    }
}

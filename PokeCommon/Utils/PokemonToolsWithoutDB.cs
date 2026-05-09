using PokemonDataAccess.Models;

namespace PokeCommon.Utils
{
    public static class PokemonToolsWithoutDB
    {
        public static ValueTask<PokeType?> GetTypeAsync(int id)
        {
            return LookupById(PokemonDBInMemory.TypeById, id);
        }

        public static ValueTask<PokeType?> GetTypeAsync(string name)
        {
            return LookupByName(PokemonDBInMemory.TypeByName, name);
        }

        public static ValueTask<Nature?> GetNatureAsync(int id)
        {
            return LookupById(PokemonDBInMemory.NatureById, id);
        }

        public static ValueTask<Nature?> GetNatureAsync(string name)
        {
            return LookupByName(PokemonDBInMemory.NatureByName, name);
        }

        public static ValueTask<Pokemon?> GetPokemonFromPsNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name) || !PokemonDBInMemory.PokemonIdByPsName.TryGetValue(name, out var id))
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
            return LookupByName(PokemonDBInMemory.AbilityByName, name);
        }

        public static ValueTask<Ability?> GetAbilityAsync(int id)
        {
            return LookupById(PokemonDBInMemory.AbilityById, id);
        }

        public static ValueTask<Move?> GetMoveAsync(string name)
        {
            return LookupByName(PokemonDBInMemory.MoveByName, name);
        }

        public static ValueTask<Move?> GetMoveAsync(int id)
        {
            return LookupById(PokemonDBInMemory.MoveById, id);
        }

        public static ValueTask<Pokemon?> GetPokemonAsync(string name)
        {
            return LookupByName(PokemonDBInMemory.PokemonByName, name);
        }

        public static ValueTask<Pokemon?> GetPokemonAsync(int id)
        {
            return LookupById(PokemonDBInMemory.PokemonById, id);
        }

        public static ValueTask<Item?> GetItemAsync(string name)
        {
            return LookupByName(PokemonDBInMemory.ItemByName, name);
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

        private static ValueTask<T?> LookupByName<T>(IReadOnlyDictionary<string, T> index, string name)
            where T : class
        {
            if (string.IsNullOrEmpty(name))
            {
                return new ValueTask<T?>((T?)null);
            }

            return new ValueTask<T?>(index.TryGetValue(name, out var value) ? value : null);
        }
    }
}

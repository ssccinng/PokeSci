using System.Reflection;
using System.Text.Json;
using PokeCommon.Models;
using PokemonDataAccess.Models;

namespace PokeCommon.Utils
{
    public static class PokemonDBInMemory
    {
        private static readonly List<Pokemon> PokemonList = LoadResource<List<Pokemon>>("Data.Pokemons.json");

        public static IQueryable<Pokemon> Pokemons = PokemonList.AsQueryable();
        public static List<PokeType> Types = LoadResource<List<PokeType>>("Data.PokeTypes.json");
        public static List<Ability> Abilities = LoadResource<List<Ability>>("Data.Abilities.json");
        public static List<Move> Moves = LoadResource<List<Move>>("Data.Moves.json");
        public static List<Item> Items = LoadResource<List<Item>>("Data.Items.json");
        public static List<PokeDex> PokeDex = LoadResource<List<PokeDex>>("Data.PokeDex.json");
        public static List<TypeEffect> TypeEffect = LoadResource<List<TypeEffect>>("Data.TypeEffect.json");
        public static List<Nature> Natures = LoadResource<List<Nature>>("Data.Natures.json");
        public static List<Statistic> Statistics = LoadResource<List<Statistic>>("Data.Statistics.json");
        public static List<Flavor> Flavors = LoadResource<List<Flavor>>("Data.Flavors.json");
        public static List<Condition> Conditions = LoadResource<List<Condition>>("Data.Conditions.json");
        public static List<Performance> Performances = LoadResource<List<Performance>>("Data.Performances.json");
        public static List<EggGroup> Egg_Groups = LoadResource<List<EggGroup>>("Data.Egg_Groups.json");
        public static List<PSPokemon> PSPokemons = LoadResource<List<PSPokemon>>("Data.PSPokemons.json");
        public static Dictionary<string, JsonElement> PSMoveData = LoadResource<Dictionary<string, JsonElement>>("Data.PSData.moves.json");

        public static IReadOnlyDictionary<int, Pokemon> PokemonById { get; } =
            BuildIdIndex(PokemonList, pokemon => pokemon.Id);

        public static IReadOnlyDictionary<string, Pokemon> PokemonByName { get; } =
            BuildNameIndex(PokemonList, StringComparer.Ordinal, pokemon => pokemon.NameChs, pokemon => pokemon.NameEng, pokemon => pokemon.NameJpn);

        public static IReadOnlyDictionary<string, Pokemon> PokemonByNormalizedName { get; } =
            BuildNameIndex(PokemonList, StringComparer.Ordinal, pokemon => NormalizeName(pokemon.NameChs), pokemon => NormalizeName(pokemon.NameEng), pokemon => NormalizeName(pokemon.NameJpn));

        public static IReadOnlyDictionary<int, PokeType> TypeById { get; } =
            BuildIdIndex(Types, type => type.Id);

        public static IReadOnlyDictionary<string, PokeType> TypeByName { get; } =
            BuildNameIndex(Types, StringComparer.Ordinal, type => type.Name_Chs, type => type.Name_Eng, type => type.Name_Jpn);

        public static IReadOnlyDictionary<string, PokeType> TypeByNormalizedName { get; } =
            BuildNameIndex(Types, StringComparer.Ordinal, type => NormalizeName(type.Name_Chs), type => NormalizeName(type.Name_Eng), type => NormalizeName(type.Name_Jpn));

        public static IReadOnlyDictionary<int, Ability> AbilityById { get; } =
            BuildIdIndex(Abilities, ability => ability.AbilityId);

        public static IReadOnlyDictionary<string, Ability> AbilityByName { get; } =
            BuildNameIndex(Abilities, StringComparer.Ordinal, ability => ability.Name_Chs, ability => ability.Name_Eng, ability => ability.Name_Jpn);

        public static IReadOnlyDictionary<string, Ability> AbilityByNormalizedName { get; } =
            BuildNameIndex(Abilities, StringComparer.Ordinal, ability => NormalizeName(ability.Name_Chs), ability => NormalizeName(ability.Name_Eng), ability => NormalizeName(ability.Name_Jpn));

        public static IReadOnlyDictionary<int, Move> MoveById { get; } =
            BuildIdIndex(Moves, move => move.MoveId);

        public static IReadOnlyDictionary<string, Move> MoveByName { get; } =
            BuildNameIndex(Moves, StringComparer.Ordinal, move => move.Name_Chs, move => move.Name_Eng, move => move.Name_Jpn);

        public static IReadOnlyDictionary<string, Move> MoveByNormalizedName { get; } =
            BuildNameIndex(Moves, StringComparer.Ordinal, move => NormalizeName(move.Name_Chs), move => NormalizeName(move.Name_Eng), move => NormalizeName(move.Name_Jpn));

        public static IReadOnlyDictionary<int, Item> ItemById { get; } =
            BuildIdIndex(Items, item => item.ItemId);

        public static IReadOnlyDictionary<string, Item> ItemByName { get; } =
            BuildNameIndex(Items, StringComparer.Ordinal, item => item.Name_Chs, item => item.Name_Eng, item => item.Name_Jpn);

        public static IReadOnlyDictionary<string, Item> ItemByNormalizedName { get; } =
            BuildNameIndex(Items, StringComparer.Ordinal, item => NormalizeName(item.Name_Chs), item => NormalizeName(item.Name_Eng), item => NormalizeName(item.Name_Jpn));

        public static IReadOnlyDictionary<int, Nature> NatureById { get; } =
            BuildIdIndex(Natures, nature => nature.NatureId);

        public static IReadOnlyDictionary<string, Nature> NatureByName { get; } =
            BuildNameIndex(Natures, StringComparer.Ordinal, nature => nature.Name_Chs, nature => nature.Name_Eng, nature => nature.Name_Jpn);

        public static IReadOnlyDictionary<string, Nature> NatureByNormalizedName { get; } =
            BuildNameIndex(Natures, StringComparer.Ordinal, nature => NormalizeName(nature.Name_Chs), nature => NormalizeName(nature.Name_Eng), nature => NormalizeName(nature.Name_Jpn));

        public static IReadOnlyDictionary<int, PSPokemon> PsPokemonByPokemonId { get; } =
            BuildPsPokemonByPokemonId();

        public static IReadOnlyDictionary<string, int> PokemonIdByPsName { get; } =
            BuildPsNameIndex(static value => value ?? string.Empty);

        public static IReadOnlyDictionary<string, int> PokemonIdByNormalizedPsName { get; } =
            BuildPsNameIndex(NormalizeName);

        public static IReadOnlyDictionary<int, List<TypeEffect>> TypeEffectsByTargetTypeId { get; } =
            BuildTypeEffectIndex();

        public static string NormalizeName(string? input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            var length = 0;
            foreach (var c in input)
            {
                if (c is not (' ' or '-'))
                {
                    length++;
                }
            }

            return string.Create(length, input, static (destination, source) =>
            {
                var index = 0;
                foreach (var c in source)
                {
                    if (c is ' ' or '-')
                    {
                        continue;
                    }

                    destination[index++] = char.ToLowerInvariant(c);
                }
            });
        }

        internal static T LoadResource<T>(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fullPath = $"{assembly.GetName().Name}.{path}";
            using var stream = assembly.GetManifestResourceStream(fullPath)
                ?? throw new InvalidOperationException($"Embedded resource '{fullPath}' was not found.");

            return JsonSerializer.Deserialize<T>(stream)
                ?? throw new InvalidOperationException($"Embedded resource '{fullPath}' could not be deserialized as {typeof(T).Name}.");
        }

        private static IReadOnlyDictionary<int, T> BuildIdIndex<T>(IEnumerable<T> source, Func<T, int> idSelector)
            where T : class
        {
            var index = new Dictionary<int, T>();
            foreach (var item in source)
            {
                index.TryAdd(idSelector(item), item);
            }

            return index;
        }

        private static IReadOnlyDictionary<string, T> BuildNameIndex<T>(
            IEnumerable<T> source,
            IEqualityComparer<string> comparer,
            params Func<T, string?>[] nameSelectors)
            where T : class
        {
            var index = new Dictionary<string, T>(comparer);
            foreach (var item in source)
            {
                foreach (var selector in nameSelectors)
                {
                    var name = selector(item);
                    if (!string.IsNullOrEmpty(name))
                    {
                        index.TryAdd(name, item);
                    }
                }
            }

            return index;
        }

        private static IReadOnlyDictionary<int, PSPokemon> BuildPsPokemonByPokemonId()
        {
            var index = new Dictionary<int, PSPokemon>();
            foreach (var psPokemon in PSPokemons)
            {
                if (psPokemon.PokemonId is int pokemonId)
                {
                    index.TryAdd(pokemonId, psPokemon);
                }
            }

            return index;
        }

        private static IReadOnlyDictionary<string, int> BuildPsNameIndex(Func<string?, string> normalize)
        {
            var index = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var psPokemon in PSPokemons)
            {
                if (psPokemon.PokemonId is not int pokemonId)
                {
                    continue;
                }

                var name = normalize(psPokemon.PSName);
                if (!string.IsNullOrEmpty(name))
                {
                    index.TryAdd(name, pokemonId);
                }
            }

            return index;
        }

        private static IReadOnlyDictionary<int, List<TypeEffect>> BuildTypeEffectIndex()
        {
            var index = new Dictionary<int, List<TypeEffect>>();
            foreach (var effect in TypeEffect)
            {
                if (effect.Type2 == null)
                {
                    continue;
                }

                if (!index.TryGetValue(effect.Type2.Id, out var effects))
                {
                    effects = new List<TypeEffect>();
                    index.Add(effect.Type2.Id, effects);
                }

                effects.Add(effect);
            }

            return index;
        }
    }
}

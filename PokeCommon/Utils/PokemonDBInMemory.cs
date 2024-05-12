using PokemonDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PokeCommon.Utils
{
    public static class PokemonDBInMemory
    {
        public static IQueryable<Pokemon> Pokemons = LoadResourceToString<IEnumerable<Pokemon>>("Data.Pokemons.json").AsQueryable();
        public static List<PokeType> Types = LoadResourceToString<List<PokeType>>("Data.PokeTypes.json");
        public static List<Ability> Abilities = LoadResourceToString<List<Ability>>("Data.Abilities.json");
        public static List<Move> Moves = LoadResourceToString<List<Move>>("Data.Moves.json");
        public static List<Item> Items = LoadResourceToString<List<Item>>("Data.Items.json");
        public static List<PokeDex> PokeDex = LoadResourceToString<List<PokeDex>>("Data.PokeDex.json");
        public static List<TypeEffect> TypeEffect = LoadResourceToString<List<TypeEffect>>("Data.TypeEffect.json");
        public static List<Nature> Natures = LoadResourceToString<List<Nature>>("Data.Natures.json");
        public static List<Statistic> Statistics = LoadResourceToString<List<Statistic>>("Data.Statistics.json");
        public static List<Flavor> Flavors = LoadResourceToString<List<Flavor>>("Data.Flavors.json");
        public static List<Condition> Conditions = LoadResourceToString<List<Condition>>("Data.Conditions.json");
        public static List<Performance> Performances = LoadResourceToString<List<Performance>>("Data.Performances.json");
        public static List<EggGroup> Egg_Groups = LoadResourceToString<List<EggGroup>>("Data.Egg_Groups.json");
        public static List<PSPokemon> PSPokemons = LoadResourceToString<List<PSPokemon>>("Data.PSPokemons.json");




        static T LoadResourceToString<T>(string path)
        { 
            var assembly = Assembly.GetExecutingAssembly(); 
            var full_path = $"{assembly.FullName.Split(',')[0]}.{path}"; 
            var rs = assembly.GetManifestResourceStream(full_path);
            return JsonSerializer.Deserialize<T>(rs);
            //var ss = new StreamReader(rs); var str = ss.ReadToEnd(); return str;
        }
    }


}
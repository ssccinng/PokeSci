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
using Microsoft.EntityFrameworkCore;
using PokemonDataAccess;
using PokemonDataAccess.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeUI3.MVVM.ViewModel
{
    public class PokedexViewModel
    {
        public PokemonContext Context { get; set; }
        public List<Pokemon> Pokemons { get; set; }
        //public List<Pokemon> PokemonDisplays { get; set; }

        public PokedexViewModel()
        {
            Context = new PokemonContext();
            //var ss = Environment.CurrentDirectory;
            Pokemons = Context.Pokemons
                .Include(s => s.Ability1)
                .Include(s => s.Ability2)
                .Include(s => s.AbilityH)
                .Include(s => s.Type1)
                .Include(s => s.Type2)
                .Include(s => s.EggGroup1)
                .Include(s => s.EggGroup2)
                .AsSplitQuery()
                .ToList();

        }
    }
}

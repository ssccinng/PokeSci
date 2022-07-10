using PokeCommon.Models;
using PokemonDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokePSCore
{
    public class PSBattlePokemon : BattlePokemon
    {
        public PSBattlePokemon(Pokemon pokemon, string psName) : base(pokemon)
        {
            PSName = psName;
        }
        public string PSName { get; set; }
        public int ActiveId { get; set; } = -1;

    }
}

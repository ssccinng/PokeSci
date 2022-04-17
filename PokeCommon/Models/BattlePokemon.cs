using PokemonDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.Models
{
    public class BattlePokemon: GamePokemon
    {
        public BattlePokemon(Pokemon pokemon) : base(pokemon)
        {
        }
        /// <summary>
        /// 携带的道具 可能丢失
        /// </summary>
        public Item Item { get; }
    }
}

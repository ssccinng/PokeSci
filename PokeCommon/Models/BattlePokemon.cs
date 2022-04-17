using PokemonDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.Models
{
    public class BattlePokemon
    {
        public GamePokemon Pokemon { get; }
        /// <summary>
        /// 携带的道具
        /// </summary>
        public Item Item { get; }
    }
}

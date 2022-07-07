using PokemonDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.Models
{
    public enum Status
    {
        Normal,
        PSN = 1,
        TOX = 2,
        PAR = 3,
        BRN = 4,
        SLP = 5,
        FRZ = 6,
    }
    public class BattlePokemon: GamePokemon
    {
        public BattlePokemon(Pokemon pokemon) : base(pokemon)
        {
        }
        /// <summary>
        /// 携带的道具 可能丢失
        /// </summary>
        public Item Item { get; }
        public bool Active { get; set; }
        public bool Dynamax { get; set; } = false;
        public bool CanDynamax { get; set; } = true;
        public Status Status { get; set; } = Status.Normal;
        
        public int[][] Buffs { get; set; }
    }
}

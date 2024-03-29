﻿using PokemonDataAccess.Models;

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

    // 

    public class BattlePokemon : GamePokemon
    {
        public BattlePokemon(Pokemon pokemon) : base(pokemon)
        {
        }
        /// <summary>
        /// 携带的道具 可能丢失
        /// </summary>
        public Item Item
        {
            get;
        }
        public bool Active
        {
            get; set;
        }
        // 剑盾独有 这个需重新设计
        public bool Dynamax { get; set; } = false;
        public bool CanDynamax { get; set; } = true;
        public Status Status { get; set; } = Status.Normal;

        public int[][] Buffs
        {
            get; set;
        }

        public void UpdatePokemon()
        {

        }
    }
}

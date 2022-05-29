using PokemonDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.Utils
{
    public static class PokemonToolsWithoutDB
    {
        private static Dictionary<int, Ability> _abilities { get; set; } = new();
        private static Dictionary<string, int> _abilityNameId { get; set; } = new();

        private static Dictionary<int, Move> _moves { get; set; } = new();
        private static Dictionary<string, int> _moveNameId { get; set; } = new();

        private static Dictionary<int, Pokemon> _pokemons { get; set; } = new();
        //private static Pokemon[] _pokemons { get; set; } = new Pokemon[4000];
        private static Dictionary<string, int> _pokemonNameId { get; set; } = new();
        private static Dictionary<int, Item> _items { get; set; } = new();
        //private static Item[] _items { get; set; } = new Item[2000];
        private static Dictionary<string, int> _itemNameId { get; set; } = new();

        private static Dictionary<string, int> _psPokemonId { get; set; } = new();
        private static Dictionary<int, PSPokemon> _pokemonIdPSName { get; set; } = new();

        private static Dictionary<int, Nature> _natures { get; set; } = new();
        private static Dictionary<string, int> _natureNameId { get; set; } = new();
    }
}

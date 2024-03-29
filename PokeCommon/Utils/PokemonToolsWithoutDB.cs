﻿using PokemonDataAccess.Models;

namespace PokeCommon.Utils
{
    public static class PokemonToolsWithoutDB
    {
        // TODO: 整一个本地数据源
        // 整一个本地数据源
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

        public static async ValueTask<Nature?> GetNatureAsync(int id)
        {
            if (_natures.TryGetValue(id, out var nature))
            {
                return nature;
            }
            else
            {
                return null;
                //var nNature = await PokemonContext.Natures.FindAsync(id);
                //if (nNature != null)
                //{
                //    _natures.Add(id, nNature);
                //    _natureNameId.Add(nNature.Name_Chs, id);
                //    _natureNameId.Add(nNature.Name_Eng, id);
                //    _natureNameId.Add(nNature.Name_Jpn, id);
                //    return nNature;
                //}
                //else
                //{
                //    return null;
                //}
            }
        }
        public static async ValueTask<Nature?> GetNatureAsync(string name)
        {
            if (_natureNameId.TryGetValue(name, out var id))
            {
                return await GetNatureAsync(id);
            }
            else
            {
                return null;

                //var nNature = await PokemonContext.Natures.FirstOrDefaultAsync(n => n.Name_Chs == name || n.Name_Eng == name || n.Name_Jpn == name);
                //if (nNature != null)
                //{
                //    _natures.Add(nNature.NatureId, nNature);
                //    _natureNameId.Add(nNature.Name_Chs, nNature.NatureId);
                //    _natureNameId.Add(nNature.Name_Eng, nNature.NatureId);
                //    _natureNameId.Add(nNature.Name_Jpn, nNature.NatureId);
                //    return nNature;
                //}
                //else
                //{
                //    return null;
                //}
            }
        }

        public static async ValueTask<Pokemon?> GetPokemonFromPsNameAsync(string name)
        {
            if (_psPokemonId.TryGetValue(name, out var id))
            {
                return await GetPokemonAsync(id);
            }
            else
            {
                return null;

                //var nPSPoke = await PokemonContext.PSPokemons.FirstOrDefaultAsync(p => p.PSName == name);
                //if (nPSPoke != null)
                //{
                //    _psPokemonId.Add(name, nPSPoke.PokemonId ?? 0);
                //    return await GetPokemonAsync(nPSPoke.PokemonId ?? 0);
                //}
                //else
                //{
                //    return null;
                //}
            }
        }
        public static async ValueTask<PSPokemon?> GetPsPokemonAsync(int pokemonId)
        {
            if (_pokemonIdPSName.TryGetValue(pokemonId, out var name))
            {
                return name;
            }
            else
            {
                return null;

                //var nPSName = await PokemonContext.PSPokemons.FirstOrDefaultAsync(s => s.PokemonId == pokemonId);
                //if (nPSName != null)
                //{
                //    _pokemonIdPSName.Add(pokemonId, nPSName);
                //}
                //return nPSName;
            }
        }

        /// <summary>
        /// 获取特性
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static async ValueTask<Ability?> GetAbilityAsync(string name)
        {
            if (_abilityNameId.TryGetValue(name, out var abilityId))
            {
                return await GetAbilityAsync(abilityId);
            }
            else
            {
                return null;

                //var nAbility = await PokemonContext.Abilities.FirstOrDefaultAsync(s => s.Name_Chs == name || s.Name_Eng == name || s.Name_Jpn == name);
                //if (nAbility != null)
                //{
                //    _abilities[nAbility.AbilityId] = nAbility;
                //    _abilityNameId[nAbility.Name_Chs] = nAbility.AbilityId;
                //    _abilityNameId[nAbility.Name_Jpn] = nAbility.AbilityId;
                //    _abilityNameId[nAbility.Name_Eng] = nAbility.AbilityId;
                //}
                //return nAbility;
            }

        }
        /// <summary>
        /// 获取特性
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async ValueTask<Ability?> GetAbilityAsync(int id)
        {
            if (_abilities.TryGetValue(id, out var ability))
            {
                return ability;
            }
            else
            {
                return null;
                //var nAbility = await PokemonContext.Abilities.FindAsync(id);
                //if (nAbility != null)
                //{
                //    _abilities[nAbility.AbilityId] = nAbility;
                //    _abilityNameId[nAbility.Name_Chs] = nAbility.AbilityId;
                //    _abilityNameId[nAbility.Name_Jpn] = nAbility.AbilityId;
                //    _abilityNameId[nAbility.Name_Eng] = nAbility.AbilityId;
                //}
                //return nAbility;
            }

        }

        public static async ValueTask<Move?> GetMoveAsync(string name)
        {
            if (_moveNameId.TryGetValue(name, out var moveId))
            {
                return await GetMoveAsync(moveId);
            }
            else
            {
                return null;

                //var nMove = await PokemonContext.Moves.FirstOrDefaultAsync(s => s.Name_Chs == name || s.Name_Eng == name || s.Name_Jpn == name);
                //if (nMove != null)
                //{
                //    _moves[nMove.MoveId] = nMove;
                //    _moveNameId[nMove.Name_Chs] = nMove.MoveId;
                //    _moveNameId[nMove.Name_Jpn] = nMove.MoveId;
                //    _moveNameId[nMove.Name_Eng] = nMove.MoveId;
                //}
                //return nMove;
            }
        }
        /// <summary>
        /// 获取招式
        /// </summary>
        public static async ValueTask<Move?> GetMoveAsync(int id)
        {
            if (_moves.TryGetValue(id, out var move))
            {
                return move;
            }
            else
            {
                return null;
                //var nMove = await PokemonContext.Moves.FindAsync(id);
                //if (nMove != null)
                //{
                //    _moves[nMove.MoveId] = nMove;
                //    _moveNameId[nMove.Name_Chs] = nMove.MoveId;
                //    _moveNameId[nMove.Name_Jpn] = nMove.MoveId;
                //    _moveNameId[nMove.Name_Eng] = nMove.MoveId;
                //}
                //return nMove;
            }
        }

        public static async ValueTask<Pokemon?> GetPokemonAsync(string name)
        {
            if (_pokemonNameId.TryGetValue(name, out var pokemonId))
            {
                return await GetPokemonAsync(pokemonId);
            }
            else
            {
                return null;
                //var nPokemon = await PokemonContext.Pokemons.FirstOrDefaultAsync(s => s.NameChs == name || s.NameEng == name || s.NameJpn == name);
                //if (nPokemon != null)
                //{
                //    _pokemons[nPokemon.Id] = nPokemon;
                //    _pokemonNameId[nPokemon.NameChs] = nPokemon.Id;
                //    _pokemonNameId[nPokemon.NameJpn] = nPokemon.Id;
                //    _pokemonNameId[nPokemon.NameEng] = nPokemon.Id;
                //}
                //return nPokemon;
            }
        }
        public static async ValueTask<Pokemon?> GetPokemonAsync(int id)
        {
            if (_pokemons.TryGetValue(id, out var pokemon))
            //if (_pokemons[id] != null)
            {
                //return _pokemons[id];
                return pokemon;
            }
            else
            {
                return null;
                //var nPokemon = await PokemonContext.Pokemons.FindAsync(id);
                //if (nPokemon != null)
                //{
                //    _pokemons[nPokemon.Id] = nPokemon;
                //    _pokemonNameId[nPokemon.NameChs] = nPokemon.Id;
                //    _pokemonNameId[nPokemon.NameJpn] = nPokemon.Id;
                //    _pokemonNameId[nPokemon.NameEng] = nPokemon.Id;
                //}
                //return nPokemon;
            }
        }
        public static async ValueTask<Item?> GetItemAsync(string name)
        {
            if (_itemNameId.TryGetValue(name, out var itemId))
            {
                return await GetItemAsync(itemId);
            }
            else
            {
                return null;
                //var nItem = await PokemonContext.Items.FirstOrDefaultAsync(s => s.Name_Chs == name || s.Name_Eng == name || s.Name_Jpn == name);
                //if (nItem != null)
                //{
                //    _items[nItem.ItemId] = nItem;
                //    _itemNameId[nItem.Name_Chs] = nItem.ItemId;
                //    _itemNameId[nItem.Name_Jpn] = nItem.ItemId;
                //    _itemNameId[nItem.Name_Eng] = nItem.ItemId;
                //}
                //return nItem;
            }
        }
        public static async ValueTask<Item?> GetItemAsync(int id)
        {
            if (_items.TryGetValue(id, out var item))
            //if (_items[id] != null)
            {
                //return _items[id];
                return item;
            }
            else
            {
                return null;
                //var nItem = await PokemonContext.Items.FindAsync(id);
                //if (nItem != null)
                //{
                //    _items[nItem.ItemId] = nItem;
                //    _itemNameId[nItem.Name_Chs] = nItem.ItemId;
                //    _itemNameId[nItem.Name_Jpn] = nItem.ItemId;
                //    _itemNameId[nItem.Name_Eng] = nItem.ItemId;
                //}
                //return nItem;
            }
        }

    }
}

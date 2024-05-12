using PokemonDataAccess;
using PokemonDataAccess.Models;

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

        private static Dictionary<int, PokeType> _pokeTypes { get; set; } = new();
        private static Dictionary<string, int> _pokeTypeNameId { get; set; } = new();

        public static async ValueTask<PokeType?> GetTypeAsync(int id)
        {
            if (_pokeTypes.TryGetValue(id, out var type))
            {
                return type;
            }
            else
            {
                var nType = PokemonDBInMemory.Types.Find(s => s.Id == id);
                if (nType != null)
                {
                    _pokeTypes.TryAdd(id, nType);
                    _pokeTypeNameId.TryAdd(nType.Name_Chs, id);
                    _pokeTypeNameId.TryAdd(nType.Name_Eng, id);
                    _pokeTypeNameId.TryAdd(nType.Name_Jpn, id);
                    return nType;
                }
                else
                {
                    return null;
                }
            }
        }
        public static async ValueTask<PokeType?> GetTypeAsync(string name)
        {
            if (_pokeTypeNameId.TryGetValue(name, out var type))
            {
                return await GetTypeAsync(type);
            }
            else
            {
                {
                    var nType = PokemonDBInMemory.Types.FirstOrDefault
                        (n => n.Name_Chs == name || n.Name_Eng == name || n.Name_Jpn == name);
                    if (nType != null)
                    {
                        _pokeTypes.TryAdd(nType.Id, nType);
                        _pokeTypeNameId.TryAdd(nType.Name_Chs, nType.Id);
                        _pokeTypeNameId.TryAdd(nType.Name_Eng, nType.Id);
                        _pokeTypeNameId.TryAdd(nType.Name_Jpn, nType.Id);
                        return nType;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
        public static async ValueTask<Nature?> GetNatureAsync(int id)
        {
            if (_natures.TryGetValue(id, out var nature))
            {
                return nature;
            }
            else
            {
                var nNature = PokemonDBInMemory.Natures.Find(s => s.NatureId == id);
                if (nNature != null)
                {
                    _natures.TryAdd(id, nNature);
                    _natureNameId.TryAdd(nNature.Name_Chs, id);
                    _natureNameId.TryAdd(nNature.Name_Eng, id);
                    _natureNameId.TryAdd(nNature.Name_Jpn, id);
                    return nNature;
                }
                else
                {
                    return null;
                }
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
                {

                    var nNature = PokemonDBInMemory.Natures
                            .FirstOrDefault(n => n.Name_Chs == name || n.Name_Eng == name || n.Name_Jpn == name);
                    if (nNature != null)
                    {
                        _natures.TryAdd(nNature.NatureId, nNature);
                        _natureNameId.TryAdd(nNature.Name_Chs, nNature.NatureId);
                        _natureNameId.TryAdd(nNature.Name_Eng, nNature.NatureId);
                        _natureNameId.TryAdd(nNature.Name_Jpn, nNature.NatureId);
                        return nNature;
                    }
                    else
                    {
                        return null;
                    }
                }
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
                {

                    var nPSPoke = PokemonDBInMemory.PSPokemons.FirstOrDefault(p => p.PSName == name);
                    if (nPSPoke != null)
                    {
                        // Todo: 为啥要try
                        _psPokemonId.TryAdd(name, nPSPoke.PokemonId ?? 0);
                        return GetPokemonAsync(nPSPoke.PokemonId ?? 0).Result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        //static object lockdb = new object();
        public static async ValueTask<PSPokemon?> GetPsPokemonAsync(int pokemonId)
        {
            if (_pokemonIdPSName.TryGetValue(pokemonId, out var name))
            {
                return name;
            }
            else
            {
                PSPokemon? nPSName;
                {
                    nPSName = PokemonDBInMemory.PSPokemons.FirstOrDefault(s => s.PokemonId == pokemonId);
                    if (nPSName != null)
                    {
                        _pokemonIdPSName.TryAdd(pokemonId, nPSName);
                    }
                }


                return nPSName;
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
                {

                    var nAbility = PokemonDBInMemory.Abilities.FirstOrDefault(s => s.Name_Chs == name || s.Name_Eng == name || s.Name_Jpn == name);
                    if (nAbility != null)
                    {
                        _abilities[nAbility.AbilityId] = nAbility;
                        _abilityNameId[nAbility.Name_Chs] = nAbility.AbilityId;
                        _abilityNameId[nAbility.Name_Jpn] = nAbility.AbilityId;
                        _abilityNameId[nAbility.Name_Eng] = nAbility.AbilityId;
                    }
                    return nAbility;
                }
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
                var nAbility = PokemonDBInMemory.Abilities.FirstOrDefault(s => s.AbilityId == id);
                if (nAbility != null)
                {
                    _abilities[nAbility.AbilityId] = nAbility;
                    _abilityNameId[nAbility.Name_Chs] = nAbility.AbilityId;
                    _abilityNameId[nAbility.Name_Jpn] = nAbility.AbilityId;
                    _abilityNameId[nAbility.Name_Eng] = nAbility.AbilityId;
                }
                return nAbility;
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
                {

                    var nMove =
                           PokemonDBInMemory.Moves.FirstOrDefault(s => s.Name_Chs == name || s.Name_Eng == name || s.Name_Jpn == name);
                    if (nMove != null)
                    {
                        _moves[nMove.MoveId] = nMove;
                        _moveNameId[nMove.Name_Chs] = nMove.MoveId;
                        _moveNameId[nMove.Name_Jpn] = nMove.MoveId;
                        _moveNameId[nMove.Name_Eng] = nMove.MoveId;
                    }
                    return nMove;
                }
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
                var nMove = PokemonDBInMemory.Moves.Find(s => s.MoveId == id);
                if (nMove != null)
                {
                    _moves[nMove.MoveId] = nMove;
                    _moveNameId[nMove.Name_Chs] = nMove.MoveId;
                    _moveNameId[nMove.Name_Jpn] = nMove.MoveId;
                    _moveNameId[nMove.Name_Eng] = nMove.MoveId;
                }
                return nMove;
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
                var nPokemon = PokemonDBInMemory.Pokemons.FirstOrDefault(s => s.NameChs == name || s.NameEng == name || s.NameJpn == name);
                if (nPokemon != null)
                {
                    _pokemons[nPokemon.Id] = nPokemon;
                    _pokemonNameId[nPokemon.NameChs] = nPokemon.Id;
                    _pokemonNameId[nPokemon.NameJpn] = nPokemon.Id;
                    _pokemonNameId[nPokemon.NameEng] = nPokemon.Id;
                }
                return nPokemon;
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
                var nPokemon = PokemonDBInMemory.Pokemons.FirstOrDefault(s => s.Id == id);
                if (nPokemon != null)
                {
                    _pokemons[nPokemon.Id] = nPokemon;
                    _pokemonNameId.TryAdd(nPokemon.NameChs ?? string.Empty, nPokemon.Id);
                    _pokemonNameId.TryAdd(nPokemon.NameJpn ?? string.Empty, nPokemon.Id);
                    _pokemonNameId.TryAdd(nPokemon.NameJpn ?? string.Empty, nPokemon.Id);
                    //_pokemonNameId[nPokemon.NameChs] = nPokemon.Id;
                    //_pokemonNameId[nPokemon.NameJpn] = nPokemon.Id;
                    //_pokemonNameId[nPokemon.NameEng] = nPokemon.Id;
                }
                return nPokemon;
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


                    var nItem = PokemonDBInMemory.Items.FirstOrDefault(s => s.Name_Chs == name || s.Name_Eng == name || s.Name_Jpn == name);
                    if (nItem != null)
                    {
                        _items[nItem.ItemId] = nItem;
                        _itemNameId[nItem.Name_Chs] = nItem.ItemId;
                        _itemNameId[nItem.Name_Jpn] = nItem.ItemId;
                        _itemNameId[nItem.Name_Eng] = nItem.ItemId;
                    }
                    return nItem;
                
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
                var nItem = PokemonDBInMemory.Items.Find(s => s.ItemId == id);
                if (nItem != null)
                {
                    _items[nItem.ItemId] = nItem;
                    _itemNameId[nItem.Name_Chs] = nItem.ItemId;
                    _itemNameId[nItem.Name_Jpn] = nItem.ItemId;
                    _itemNameId[nItem.Name_Eng] = nItem.ItemId;
                }
                return nItem;
            }
        }

    }
}

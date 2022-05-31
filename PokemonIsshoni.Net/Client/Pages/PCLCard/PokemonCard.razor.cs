//using PokemonDataAccess.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace NewPokemonChineseLink.Client.Pages.PCLCard
//{
//    public partial class PokemonCard
//    {
//        private async Task<IEnumerable<Item>> SearchItem(string value)
//        {
//            // if (flagItem) return null;
//            // flagItem = true;
//            // In real life use an asynchronous function for fetching data from an api.
//            await Task.Delay(300);

//            // if text is null or empty, don't return values (drop-down will not open)
//            if (string.IsNullOrEmpty(value))
//                return null;
//            // flagItem = false;
//            return _items.Where(s => s.Name_Chs.Contains(value) || s.Name_Eng.Contains(value, StringComparison.OrdinalIgnoreCase));
//            //return new UserData[0];
//            //return _userDatas.Where(x => x.NickName.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.Email.Contains(value, StringComparison.InvariantCultureIgnoreCase));
//        }
//        private async Task<IEnumerable<Ability>> SearchAbility(string value)
//        {
//            // In real life use an asynchronous function for fetching data from an api.
//            await Task.Delay(300);

//            // if text is null or empty, don't return values (drop-down will not open)
//            if (string.IsNullOrEmpty(value))
//                return null;
//            return _abilitys.Where(s => s.Name_Chs.Contains(value) || s.Name_Eng.Contains(value, StringComparison.OrdinalIgnoreCase));
//        }

//        private async Task<IEnumerable<Move>> SearchMove(string value)
//        {
//            // In real life use an asynchronous function for fetching data from an api.
//            await Task.Delay(300);

//            // if text is null or empty, don't return values (drop-down will not open)
//            if (string.IsNullOrEmpty(value))
//                return null;
//            return _moves.Where(s => s.Name_Chs.Contains(value) || s.Name_Eng.Contains(value, StringComparison.OrdinalIgnoreCase));
//        }
//        private async Task<IEnumerable<Pokemon>> SearchPoke(string value)
//        {
//            // In real life use an asynchronous function for fetching data from an api.
//            await Task.Delay(300);

//            // if text is null or empty, don't return values (drop-down will not open)
//            if (string.IsNullOrEmpty(value))
//                return null;
//            return _pokemons.Where(s => s.FullNameChs.Contains(value) || s.FullNameEng.Contains(value, StringComparison.OrdinalIgnoreCase));
//        }
//    }
//}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Text.RegularExpressions;

//using NewPokemonChineseLink.Shared.Models;
//using PokemonDataAccess.Models;

//using Microsoft.AspNetCore.Components;
////using NewPokemonChineseLink.Client.Services;

//namespace PokemonIsshoni.Net.Client.Lib
//{
//    public class PSInterpreter
//    {
//        [Inject]
//        protected PokemonDataServices _pokemonDataServices { get; set; }

//        //public PSInterpreter()
//        //{
//        //    _items =  _pokemonDataServices.GetItems().Result;
//        //    _abilitys =  _pokemonDataServices.GetAbilities().Result;
//        //    _natures =  _pokemonDataServices.GetNatures().Result;
//        //    _moves =  _pokemonDataServices.GetMoves().Result;
//        //    _pokemons =  _pokemonDataServices.GetPokemons().Result;
//        //    _psPokemons = _pokemonDataServices.GetPSPokemons().Result.GroupBy(s => s.PSChsName, s => s.PSImgName).ToDictionary(s => s.Key, s => s.First());
//        //}

//        public PSInterpreter()
//        {
//        }

//        public static PCLPokemon PSTextToPoke(string text)
//        {
//            PCLPokemon poke = new();
//            string[] data = Regex.Split(text.Trim(), "\r*\n");

//            if (data.Length < 1) return null;
//            string[] NameandItem = Regex.Split(data[0].Trim(), @"\s+@\s+"); // 昵称

//            if (NameandItem.Length > 1)
//            {
//                //poke.ItemId = _items.FirstOrDefault(s=>s.Name_Eng == NameandItem[1]).ItemId;
//            }
//            for (int i = 0; i < data.Length; ++i)
//            {

//            }


//            return null;
//        }


//        public static PCLPokeTeam PSTextToPokeTeam(string text)
//        {
//            string[] pokeText = Regex.Split(text.Trim(), "(?:[\r ]*\n){2,}");

//            PCLPokeTeam pCLPokeTeam = new PCLPokeTeam();
//            return pCLPokeTeam;
//        }
//    }
//}

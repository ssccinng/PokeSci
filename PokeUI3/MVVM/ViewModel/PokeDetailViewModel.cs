using PokemonDataAccess.Models;
using PokeUI3.Core;
using PokeUI3.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PokeUI3.MVVM.ViewModel
{
    internal class PokeDetailViewModel: ObservableObject
    {
        public Pokemon Pokemon { get; set; }
        private BSTValue _bstValue = new BSTValue();
            
        public BSTValue BSTValue
        {
            get { return _bstValue; }
            set { _bstValue = value; OnPropertyChanged(); }
        }
        public void BSTViewChange()
        {
            OnPropertyChanged("BSTValue");
        }
        public string PokeImage => $"/Image/spr_pokemonday2021/{Pokemon.DexId}.png";
        //public string PokeColor => Pokemon.Color switch
        //{
        //    0 => "Red",
        //    1 => "Blue",
        //    2 => "Green",
        //    3 => "Yellow",
        //    4 => "Purple",
        //    5 => "Pink",
        //    6 => "Grown",
        //    7 => "Black",
        //    8 => "Gray",
        //    9 => "White",
        //    _ => "Lime"
        //};
        private int myVar = 200;

        public int MyProperty
        {
            get { return myVar; }
            set { myVar  = value; OnPropertyChanged(); }
        }

        public string PokeColor => "#1096E0";
        public PokeDetailViewModel(int id)
        {

        }

        public PokeDetailViewModel(Pokemon pokemon)
        {
            Pokemon = pokemon;
        }
    }
}

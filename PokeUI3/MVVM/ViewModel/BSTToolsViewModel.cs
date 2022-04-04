using PokemonDataAccess.Models;
using PokeUI3.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PokeMath;

namespace PokeUI3.MVVM.ViewModel
{
    public class BSTToolsViewModel: ObservableObject
    {
        public Pokemon Pokemon { get; set; }
        public SWSHTools SWSHTools { get; set; } = new SWSHTools();
        public int StatHP => SWSHTools.GetHP(Pokemon.BaseHP, IVHP, EVHP);

        private int _evHP;
            
        public int EVHP
        {
            get { return _evHP; }
            set { _evHP = value; OnPropertyChanged("StatHP"); OnPropertyChanged(); }
        }
        private int _ivHP = 31;
        public int IVHP
        {
            get { return _ivHP; }
            set { _ivHP = value; OnPropertyChanged("StatHP"); OnPropertyChanged(); }
        }
        public BSTToolsViewModel(Pokemon pokemon)
        {
            Pokemon = pokemon;
        }
    }
}

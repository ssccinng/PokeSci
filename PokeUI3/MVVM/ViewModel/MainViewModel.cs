using System;
using System.IO;
using PokemonDataAccess;
using PokeUI3.Assets;
using PokeUI3.Core;

namespace PokeUI3.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        public PokemonContext Context
        {
            get; set;
        }
        public PokedexViewModel PokemonVM
        {
            get; set;
        }

        public static MainViewModel Instance
        {
            get; set;
        }


        public MainViewModel()
        {
            Instance = this;
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            var DbPath = System.IO.Path.Join(path, "PokemonDataBase.db");
            if (!File.Exists(DbPath))
            {
                var a = PokeRes.ResourceManager.GetObject("PokemonDataBase");
                File.WriteAllBytes(DbPath, a as byte[]);
            }
            Context = new PokemonContext();
            PokemonVM = new();
        }
    }
}

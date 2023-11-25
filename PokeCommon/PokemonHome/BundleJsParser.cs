using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PokeCommon.PokemonHome
{
    public class BundleJsParser
    {
        public static readonly string BundleUrl = "https://resource.pokemon-home.com/battledata/js/bundle.js";

        public BundleData ParseAsync(string data)
        {
            Regex regex3 = new Regex(@"this.dex = (((?'Open'\{)[^\{\}]*)+((?'Close-Open'\})[^\{\}]*)+)*(?(Open)(?!))");

            return new BundleData();
        }
    }

    public class BundleData
    {

    }
}

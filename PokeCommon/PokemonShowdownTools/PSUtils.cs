using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PokeCommon.PokemonShowdownTools
{
    public static class PSUtils
    {
        public static string GetPurePSId(string id) => Regex.Replace(id, @"[^A-Za-z0-9]", "").ToLower();
    }
}

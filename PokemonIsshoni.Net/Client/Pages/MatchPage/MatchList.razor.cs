using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using PokemonIsshoni.Net.Shared.Models;

namespace PokemonIsshoni.Net.Client.Pages.MatchPage
{
    public partial class MatchList
    {
        private IEnumerable<PCLMatch> _pclMatches;

        private bool FilterFunc(PCLMatch element)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            //if ($"{element.Number} {element.Position} {element.Molar}".Contains(searchString))
            //    return true;
            return false;
        }
    }
}

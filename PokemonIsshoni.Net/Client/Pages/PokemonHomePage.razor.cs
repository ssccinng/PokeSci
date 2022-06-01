using PokeCommon.Models;

namespace PokemonIsshoni.Net.Client.Pages
{
    public partial class PokemonHomePage
    {
        private List<PokemonHomeTrainerRankData> _pokemonHomeTrainerRankDatas;
        private bool FilterFunc(PokemonHomeTrainerRankData element)
        {
            if (string.IsNullOrWhiteSpace(searchString1))
                return true;
            if (element.Name.Contains(searchString1, StringComparison.OrdinalIgnoreCase))
                return true;
            if (searchString1.Length == 3 && element.LanguageType.ToString() == searchString1)
                return true;
            return false;
        }
    }
}

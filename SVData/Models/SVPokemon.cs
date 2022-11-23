using PokemonDataAccess.Models;

namespace SVData.Models;

public class SVPokemon: Pokemon
{
    public string[] EggMoves
    {
        get; set;
    }
    public string[] TMMoves
    {
        get; set;
    }
    public string[] LearnMoves
    {
        get; set;
    }
}
using PokemonDataAccess.Models;

namespace SVData.Models;
internal class PokeWithMove
{
    public Pokemon Pokemon
    {
        get; set;
    }

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

    public HashSet<string> OtherMoves
    {
        get; set;
    } = new();

    public HashSet<string> GetMoveSet()
    {
        HashSet<string> result = new HashSet<string>();
        foreach (var move in EggMoves)
        {
            result.Add(move);
        }

        foreach (var move in TMMoves)
        {
            result.Add(move);
        }
        foreach (var move in LearnMoves)
        {
            result.Add(move);
        }
        foreach (var move in OtherMoves)
        {
            result.Add(move);
        }
        return result;
    }
}

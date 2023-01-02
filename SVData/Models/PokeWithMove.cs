using PokemonDataAccess.Models;

namespace SVData.Models;
internal class PokeWithMove
{
    public Name Pokemon
    {
        get; set;
    }
    public string Name;

    public List<(string, string)> EggMoves
    {
        get; set;
    } = new List<(string, string)>();
    public List<(string, string)> TMMoves
    {
        get; set;
    } = new List<(string, string)>();
    public List<(string, string)> LearnMoves { get; set; } = new List<(string, string)>();

    public HashSet<string> OtherMoves
    {
        get; set;
    } = new();
    public HashSet<string> BeforeMoves
    {
        get; set;
    } = new();
    public HashSet<string> GetMoveSet()
    {
        HashSet<string> result = new HashSet<string>();
        foreach (var move in EggMoves)
        {
            result.Add(move.Item1);
        }

        foreach (var move in TMMoves)
        {
            result.Add(move.Item1);
        }
        foreach (var move in LearnMoves)
        {
            result.Add(move.Item1);
        }
        foreach (var move in OtherMoves)
        {
            result.Add(move);
        }
        return result;
    }
}

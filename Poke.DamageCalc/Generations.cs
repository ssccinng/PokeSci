using Poke.DamageCalc.Data;

namespace Poke.DamageCalc;

public sealed class Generation
{
    internal Generation(int number)
    {
        Number = number;
    }

    public int Number { get; }

    public SpeciesData GetSpecies(string name) => DamageData.GetSpecies(name);

    public MoveData GetMove(string name) => DamageData.GetMove(name);
}

public static class Generations
{
    public static Generation Get(int number)
    {
        if (number is < 1 or > 9)
        {
            throw new ArgumentOutOfRangeException(nameof(number), number, "Generation must be between 1 and 9.");
        }

        return new Generation(number);
    }
}

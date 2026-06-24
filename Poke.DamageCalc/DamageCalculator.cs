namespace Poke.DamageCalc;

/// <summary>
/// Entry point for Smogon-style damage calculations.
/// </summary>
public static class DamageCalculator
{
    public static DamageResult Calculate(
        int gen,
        Pokemon attacker,
        Pokemon defender,
        Move move,
        Field? field = null)
    {
        var generation = Generations.Get(gen);
        var battleField = field?.Clone() ?? new Field();

        return generation.Number switch
        {
            9 => Mechanics.Gen789.ModernDamageCalculator.Calculate(generation, attacker.Clone(), defender.Clone(), move.Clone(), battleField),
            7 or 8 => Mechanics.Gen789.ModernDamageCalculator.Calculate(generation, attacker.Clone(), defender.Clone(), move.Clone(), battleField),
            _ => throw new NotSupportedException($"Generation {gen} is not implemented yet. Gen9 is the supported first slice.")
        };
    }
}

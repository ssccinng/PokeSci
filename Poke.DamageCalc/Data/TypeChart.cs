namespace Poke.DamageCalc.Data;

public static partial class TypeChart
{
    private static readonly Dictionary<string, Dictionary<string, double>> Chart = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Normal"] = Row(("Rock", 0.5), ("Ghost", 0), ("Steel", 0.5)),
        ["Fire"] = Row(("Fire", 0.5), ("Water", 0.5), ("Grass", 2), ("Ice", 2), ("Bug", 2), ("Rock", 0.5), ("Dragon", 0.5), ("Steel", 2)),
        ["Water"] = Row(("Fire", 2), ("Water", 0.5), ("Grass", 0.5), ("Ground", 2), ("Rock", 2), ("Dragon", 0.5)),
        ["Electric"] = Row(("Water", 2), ("Electric", 0.5), ("Grass", 0.5), ("Ground", 0), ("Flying", 2), ("Dragon", 0.5)),
        ["Grass"] = Row(("Fire", 0.5), ("Water", 2), ("Grass", 0.5), ("Poison", 0.5), ("Ground", 2), ("Flying", 0.5), ("Bug", 0.5), ("Rock", 2), ("Dragon", 0.5), ("Steel", 0.5)),
        ["Ice"] = Row(("Fire", 0.5), ("Water", 0.5), ("Grass", 2), ("Ice", 0.5), ("Ground", 2), ("Flying", 2), ("Dragon", 2), ("Steel", 0.5)),
        ["Fighting"] = Row(("Normal", 2), ("Ice", 2), ("Poison", 0.5), ("Flying", 0.5), ("Psychic", 0.5), ("Bug", 0.5), ("Rock", 2), ("Ghost", 0), ("Dark", 2), ("Steel", 2), ("Fairy", 0.5)),
        ["Poison"] = Row(("Grass", 2), ("Poison", 0.5), ("Ground", 0.5), ("Rock", 0.5), ("Ghost", 0.5), ("Steel", 0), ("Fairy", 2)),
        ["Ground"] = Row(("Fire", 2), ("Electric", 2), ("Grass", 0.5), ("Poison", 2), ("Flying", 0), ("Bug", 0.5), ("Rock", 2), ("Steel", 2)),
        ["Flying"] = Row(("Electric", 0.5), ("Grass", 2), ("Fighting", 2), ("Bug", 2), ("Rock", 0.5), ("Steel", 0.5)),
        ["Psychic"] = Row(("Fighting", 2), ("Poison", 2), ("Psychic", 0.5), ("Dark", 0), ("Steel", 0.5)),
        ["Bug"] = Row(("Fire", 0.5), ("Grass", 2), ("Fighting", 0.5), ("Poison", 0.5), ("Flying", 0.5), ("Psychic", 2), ("Ghost", 0.5), ("Dark", 2), ("Steel", 0.5), ("Fairy", 0.5)),
        ["Rock"] = Row(("Fire", 2), ("Ice", 2), ("Fighting", 0.5), ("Ground", 0.5), ("Flying", 2), ("Bug", 2), ("Steel", 0.5)),
        ["Ghost"] = Row(("Normal", 0), ("Psychic", 2), ("Ghost", 2), ("Dark", 0.5)),
        ["Dragon"] = Row(("Dragon", 2), ("Steel", 0.5), ("Fairy", 0)),
        ["Dark"] = Row(("Fighting", 0.5), ("Psychic", 2), ("Ghost", 2), ("Dark", 0.5), ("Fairy", 0.5)),
        ["Steel"] = Row(("Fire", 0.5), ("Water", 0.5), ("Electric", 0.5), ("Ice", 2), ("Rock", 2), ("Steel", 0.5), ("Fairy", 2)),
        ["Fairy"] = Row(("Fire", 0.5), ("Fighting", 2), ("Poison", 0.5), ("Dragon", 2), ("Dark", 2), ("Steel", 0.5))
    };

    public static double Effectiveness(string attackingType, IEnumerable<string> defendingTypes)
    {
        var modifier = 1.0;
        foreach (var defendingType in defendingTypes)
        {
            if (TryGetGeneratedEffectiveness(attackingType, defendingType, out var generatedValue))
            {
                modifier *= generatedValue;
            }
            else if (Chart.TryGetValue(attackingType, out var row) && row.TryGetValue(defendingType, out var value))
            {
                modifier *= value;
            }
        }

        return modifier;
    }

    public static double FlyingPressEffectiveness(IEnumerable<string> defendingTypes) =>
        Effectiveness("Fighting", defendingTypes) * Effectiveness("Flying", defendingTypes);

    private static Dictionary<string, double> Row(params (string Type, double Multiplier)[] entries) =>
        entries.ToDictionary(e => e.Type, e => e.Multiplier, StringComparer.OrdinalIgnoreCase);

    private static partial bool TryGetGeneratedEffectiveness(string attackingType, string defendingType, out double value);
}

namespace Poke.DamageCalc;

public readonly record struct DamageRange(int Min, int Max)
{
    public DamageRange((int Min, int Max) range)
        : this(range.Min, range.Max)
    {
    }
}

public enum DamageMatchMode
{
    ExactRange,
    ContainsDamage,
    OverlapsRange
}

public sealed record ReverseDamageCandidate(
    StatId Stat,
    int ActualStat,
    int Ev,
    int Iv,
    string Nature,
    int Boost,
    DamageRange ExpectedRange,
    IReadOnlyList<int> Rolls);

public sealed class ReverseDamageOptions
{
    public StatId? Stat { get; set; }

    public DamageMatchMode MatchMode { get; set; } = DamageMatchMode.ExactRange;

    public int[] Evs { get; set; } = LegalEvs();

    public int[] Ivs { get; set; } = [31];

    public int[] Boosts { get; set; } = [0];

    public string[]? Natures { get; set; }

    public int? ObservedDamage { get; set; }

    public static int[] LegalEvs()
    {
        var evs = new int[64];
        for (var i = 0; i < evs.Length; i++)
        {
            evs[i] = i * 4;
        }

        return evs;
    }
}

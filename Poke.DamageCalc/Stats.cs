namespace Poke.DamageCalc;

public static class Stats
{
    private static readonly Dictionary<string, (StatId? Up, StatId? Down)> NatureModifiers = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Hardy"] = (null, null),
        ["Lonely"] = (StatId.Atk, StatId.Def),
        ["Brave"] = (StatId.Atk, StatId.Spe),
        ["Adamant"] = (StatId.Atk, StatId.Spa),
        ["Naughty"] = (StatId.Atk, StatId.Spd),
        ["Bold"] = (StatId.Def, StatId.Atk),
        ["Docile"] = (null, null),
        ["Relaxed"] = (StatId.Def, StatId.Spe),
        ["Impish"] = (StatId.Def, StatId.Spa),
        ["Lax"] = (StatId.Def, StatId.Spd),
        ["Timid"] = (StatId.Spe, StatId.Atk),
        ["Hasty"] = (StatId.Spe, StatId.Def),
        ["Serious"] = (null, null),
        ["Jolly"] = (StatId.Spe, StatId.Spa),
        ["Naive"] = (StatId.Spe, StatId.Spd),
        ["Modest"] = (StatId.Spa, StatId.Atk),
        ["Mild"] = (StatId.Spa, StatId.Def),
        ["Quiet"] = (StatId.Spa, StatId.Spe),
        ["Bashful"] = (null, null),
        ["Rash"] = (StatId.Spa, StatId.Spd),
        ["Calm"] = (StatId.Spd, StatId.Atk),
        ["Gentle"] = (StatId.Spd, StatId.Def),
        ["Sassy"] = (StatId.Spd, StatId.Spe),
        ["Careful"] = (StatId.Spd, StatId.Spa),
        ["Quirky"] = (null, null)
    };

    public static StatsTable CalculateAll(StatsTable baseStats, StatsTable ivs, StatsTable evs, int level, string nature) =>
        new(
            CalcStat(StatId.Hp, baseStats.Hp, ivs.Hp, evs.Hp, level, nature),
            CalcStat(StatId.Atk, baseStats.Atk, ivs.Atk, evs.Atk, level, nature),
            CalcStat(StatId.Def, baseStats.Def, ivs.Def, evs.Def, level, nature),
            CalcStat(StatId.Spa, baseStats.Spa, ivs.Spa, evs.Spa, level, nature),
            CalcStat(StatId.Spd, baseStats.Spd, ivs.Spd, evs.Spd, level, nature),
            CalcStat(StatId.Spe, baseStats.Spe, ivs.Spe, evs.Spe, level, nature));

    public static int CalcStat(StatId stat, int baseStat, int iv, int ev, int level, string nature)
    {
        if (stat == StatId.Hp)
        {
            if (baseStat == 1)
            {
                return 1;
            }

            return (int)Math.Floor(((2 * baseStat + iv + Math.Floor(ev / 4.0)) * level) / 100.0) + level + 10;
        }

        var value = (int)Math.Floor(((2 * baseStat + iv + Math.Floor(ev / 4.0)) * level) / 100.0) + 5;
        var (up, down) = NatureModifiers.TryGetValue(nature, out var modifiers) ? modifiers : (null, null);

        if (up == stat)
        {
            value = (int)Math.Floor(value * 1.1);
        }
        else if (down == stat)
        {
            value = (int)Math.Floor(value * 0.9);
        }

        return value;
    }

    public static int ModifyByStage(int stat, int stage)
    {
        stage = Math.Clamp(stage, -6, 6);
        if (stage >= 0)
        {
            return (int)Math.Floor(stat * (2 + stage) / 2.0);
        }

        return (int)Math.Floor(stat * 2.0 / (2 - stage));
    }

    public static string EffortText(Pokemon pokemon, StatId stat)
    {
        var ev = pokemon.Evs[stat];
        var suffix = NatureSuffix(pokemon.Nature, stat);
        return $"{ev}{suffix}";
    }

    public static string NatureSuffix(string? nature, StatId stat)
    {
        var (up, down) = !string.IsNullOrWhiteSpace(nature) && NatureModifiers.TryGetValue(nature, out var modifiers)
            ? modifiers
            : (null, null);
        if (up == stat)
        {
            return "+";
        }

        return down == stat ? "-" : string.Empty;
    }
}

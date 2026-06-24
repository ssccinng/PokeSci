namespace Poke.DamageCalc;

public static class ReverseDamageCalculator
{
    private static readonly Dictionary<StatId, string[]> NaturesByPositiveStat = new()
    {
        [StatId.Atk] = ["Hardy", "Lonely", "Brave", "Adamant", "Naughty", "Bold", "Timid", "Modest", "Calm"],
        [StatId.Def] = ["Hardy", "Bold", "Relaxed", "Impish", "Lax", "Lonely", "Hasty", "Mild", "Gentle"],
        [StatId.Spa] = ["Hardy", "Modest", "Mild", "Quiet", "Rash", "Adamant", "Impish", "Jolly", "Careful"],
        [StatId.Spd] = ["Hardy", "Calm", "Gentle", "Sassy", "Careful", "Naughty", "Lax", "Naive", "Rash"],
        [StatId.Spe] = ["Hardy", "Timid", "Hasty", "Jolly", "Naive", "Brave", "Relaxed", "Quiet", "Sassy"],
        [StatId.Hp] = ["Hardy"]
    };

    public static IReadOnlyList<ReverseDamageCandidate> InferAttackerSpread(
        int gen,
        DamageRange observedDamage,
        Pokemon attacker,
        Pokemon defender,
        Move move,
        Field? field = null,
        ReverseDamageOptions? options = null)
    {
        options ??= new ReverseDamageOptions();
        var stat = options.Stat ?? OffensiveStat(attacker, move);

        return Infer(
            gen,
            observedDamage,
            attacker,
            defender,
            move,
            field,
            options,
            stat,
            mutateAttacker: true);
    }

    public static IReadOnlyList<ReverseDamageCandidate> InferDefenderSpread(
        int gen,
        DamageRange observedDamage,
        Pokemon attacker,
        Pokemon defender,
        Move move,
        Field? field = null,
        ReverseDamageOptions? options = null)
    {
        options ??= new ReverseDamageOptions();
        var stat = options.Stat ?? DefensiveStat(attacker, move);

        return Infer(
            gen,
            observedDamage,
            attacker,
            defender,
            move,
            field,
            options,
            stat,
            mutateAttacker: false);
    }

    private static IReadOnlyList<ReverseDamageCandidate> Infer(
        int gen,
        DamageRange observedDamage,
        Pokemon attacker,
        Pokemon defender,
        Move move,
        Field? field,
        ReverseDamageOptions options,
        StatId stat,
        bool mutateAttacker)
    {
        var candidates = new List<ReverseDamageCandidate>();
        var natures = options.Natures ?? NaturesFor(stat);

        foreach (var ev in options.Evs.Distinct().Where(IsLegalEv))
        foreach (var iv in options.Ivs.Distinct().Where(iv => iv is >= 0 and <= 31))
        foreach (var nature in natures.Distinct(StringComparer.OrdinalIgnoreCase))
        foreach (var boost in options.Boosts.Distinct().Where(boost => boost is >= -6 and <= 6))
        {
            var candidateAttacker = attacker.Clone();
            var candidateDefender = defender.Clone();
            var target = mutateAttacker ? candidateAttacker : candidateDefender;

            target.Evs[stat] = ev;
            target.Ivs[stat] = iv;
            target.Boosts[stat] = boost;
            if (stat != StatId.Hp)
            {
                target.Nature = nature;
            }

            var result = DamageCalculator.Calculate(gen, candidateAttacker, candidateDefender, move, field);
            var expected = new DamageRange(result.Range());
            if (!Matches(observedDamage, expected, result.Damage, options))
            {
                continue;
            }

            candidates.Add(new ReverseDamageCandidate(
                stat,
                target.RawStats[stat],
                ev,
                iv,
                stat == StatId.Hp ? target.Nature : nature,
                boost,
                expected,
                result.Damage.ToArray()));
        }

        return candidates
            .OrderBy(c => c.Ev)
            .ThenBy(c => c.Iv)
            .ThenBy(c => c.Nature, StringComparer.OrdinalIgnoreCase)
            .ThenBy(c => c.Boost)
            .ToArray();
    }

    private static bool Matches(
        DamageRange observed,
        DamageRange expected,
        IReadOnlyList<int> rolls,
        ReverseDamageOptions options)
    {
        return options.MatchMode switch
        {
            DamageMatchMode.ExactRange => observed == expected,
            DamageMatchMode.ContainsDamage => options.ObservedDamage.HasValue
                ? rolls.Contains(options.ObservedDamage.Value)
                : rolls.Any(roll => roll >= observed.Min && roll <= observed.Max),
            DamageMatchMode.OverlapsRange => observed.Min <= expected.Max && expected.Min <= observed.Max,
            _ => throw new ArgumentOutOfRangeException(nameof(options.MatchMode), options.MatchMode, null)
        };
    }

    private static bool IsLegalEv(int ev) => ev is >= 0 and <= 252 && ev % 4 == 0;

    private static string[] NaturesFor(StatId stat) => NaturesByPositiveStat[stat];

    private static StatId OffensiveStat(Pokemon attacker, Move move)
    {
        var candidate = move.Clone();
        if (candidate.Named("Tera Blast") && !string.IsNullOrEmpty(attacker.TeraType))
        {
            var stats = attacker.RawStats;
            var atk = Stats.ModifyByStage(stats.Atk, attacker.Boosts.Atk);
            var spa = Stats.ModifyByStage(stats.Spa, attacker.Boosts.Spa);
            return atk > spa ? StatId.Atk : StatId.Spa;
        }

        return candidate.Category == MoveCategory.Physical ? StatId.Atk : StatId.Spa;
    }

    private static StatId DefensiveStat(Pokemon attacker, Move move) =>
        OffensiveStat(attacker, move) == StatId.Atk ? StatId.Def : StatId.Spd;
}

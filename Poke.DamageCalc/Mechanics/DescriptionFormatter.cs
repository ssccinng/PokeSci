using System.Globalization;

namespace Poke.DamageCalc.Mechanics;

internal static class DescriptionFormatter
{
    public static string FullDescription(DamageResult result, string notation)
    {
        var (min, max) = result.Range();
        var attackerStat = result.Move.Category == MoveCategory.Physical ? StatId.Atk : StatId.Spa;
        var defenderStat = result.Move.Category == MoveCategory.Physical ? StatId.Def : StatId.Spd;

        var prefix = result.Move.Category == MoveCategory.Status
            ? result.Attacker.Name
            : $"{Stats.EffortText(result.Attacker, attackerStat)} {ShortStat(attackerStat)} {AttackerItemPrefix(result)}{result.Attacker.Name}";

        var defender = result.Move.Category == MoveCategory.Status
            ? result.Defender.Name
            : $"{Stats.EffortText(result.Defender, StatId.Hp)} HP / {Stats.EffortText(result.Defender, defenderStat)} {ShortStat(defenderStat)} {DefenderItemPrefix(result)}{result.Defender.Name}";

        var moveText = result.Move.Name;
        if (result.RawDescription.MoveBasePower is > 0 && NeedsPowerText(result.Move))
        {
            moveText += $" ({result.RawDescription.MoveBasePower} BP";
            if (!string.Equals(result.RawDescription.MoveType, result.Move.Data.Type, StringComparison.OrdinalIgnoreCase) ||
                result.Move.Named("Weather Ball"))
            {
                moveText += $" {result.RawDescription.MoveType}";
            }

            moveText += ")";
        }

        var fieldText = FieldText(result);
        var critText = result.RawDescription.IsCritical ? " on a critical hit" : string.Empty;
        var damageText = $"{min}-{max} ({Percent(min, result.Defender.MaxHp())} - {Percent(max, result.Defender.MaxHp())}%)";
        var ko = KoChance(result);

        return $"{prefix} {moveText} vs. {defender}{fieldText}{critText}: {damageText}{(string.IsNullOrEmpty(ko) ? string.Empty : " -- " + ko)}";
    }

    public static string MoveDescription(DamageResult result, string notation)
    {
        var (min, max) = result.Range();
        return $"{result.Move.Name}: {min}-{max}";
    }

    public static string KoChance(DamageResult result)
    {
        var (min, max) = result.Range();
        var hp = result.Defender.MaxHp();

        if (max <= 0)
        {
            return string.Empty;
        }

        if (min >= hp)
        {
            return "guaranteed OHKO";
        }

        var hits = (int)Math.Ceiling(hp / (double)Math.Max(1, max));
        if (hits <= 1)
        {
            return string.Empty;
        }

        return $"guaranteed {hits}HKO";
    }

    private static string AttackerItemPrefix(DamageResult result) =>
        string.IsNullOrEmpty(result.RawDescription.AttackerItem) ? string.Empty : result.RawDescription.AttackerItem + " ";

    private static string DefenderItemPrefix(DamageResult result) =>
        string.IsNullOrEmpty(result.RawDescription.DefenderItem) ? string.Empty : result.RawDescription.DefenderItem + " ";

    private static bool NeedsPowerText(Move move) =>
        move.Named("Weather Ball", "Grass Knot", "Low Kick", "Comet Punch", "Tera Blast") || move.BasePower != move.Data.BasePower;

    private static string FieldText(DamageResult result)
    {
        if (!string.IsNullOrEmpty(result.Field.Weather))
        {
            return $" in {result.Field.Weather}";
        }

        if (!string.IsNullOrEmpty(result.Field.Terrain))
        {
            return $" in {result.Field.Terrain}";
        }

        return string.Empty;
    }

    private static string ShortStat(StatId stat) => stat switch
    {
        StatId.Atk => "Atk",
        StatId.Def => "Def",
        StatId.Spa => "SpA",
        StatId.Spd => "SpD",
        StatId.Spe => "Spe",
        _ => "HP"
    };

    private static string Percent(int damage, int hp)
    {
        var value = Math.Floor(damage * 1000.0 / hp) / 10.0;
        return value.ToString("0.#", CultureInfo.InvariantCulture);
    }
}

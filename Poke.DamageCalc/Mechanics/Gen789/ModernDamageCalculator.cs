using Poke.DamageCalc.Data;

namespace Poke.DamageCalc.Mechanics.Gen789;

internal static class ModernDamageCalculator
{
    public static DamageResult Calculate(
        Generation generation,
        Pokemon attacker,
        Pokemon defender,
        Move move,
        Field field)
    {
        var desc = new ResultDescription
        {
            Weather = field.Weather,
            Terrain = field.Terrain,
            AttackerTera = attacker.TeraType,
            DefenderTera = defender.TeraType
        };

        if (move.IsStatus)
        {
            return Result(generation, attacker, defender, move, field, [0], desc);
        }

        NormalizeMove(attacker, move, field, desc);

        if (field.DefenderSide.IsProtected && !BreaksProtect(attacker, move))
        {
            desc.IsProtected = true;
            return Result(generation, attacker, defender, move, field, [0], desc);
        }

        var fixedDamage = TryFixedDamage(attacker, move);
        if (fixedDamage.HasValue)
        {
            return Result(generation, attacker, defender, move, field, [fixedDamage.Value], desc);
        }

        var typeEffectiveness = GetTypeEffectiveness(defender, move);
        desc.TypeEffectiveness = typeEffectiveness;
        if (typeEffectiveness == 0)
        {
            return Result(generation, attacker, defender, move, field, [0], desc);
        }

        var category = move.Category;
        var attackStat = category == MoveCategory.Physical ? StatId.Atk : StatId.Spa;
        var defenseStat = category == MoveCategory.Physical ? StatId.Def : StatId.Spd;

        MarkRelevantItems(attacker, defender, category, defenseStat, desc);

        var attack = GetEffectiveStat(attacker, attackStat, move, field, true);
        var defense = GetEffectiveStat(defender, defenseStat, move, field, false);
        var basePower = GetBasePower(attacker, defender, move, field, desc);

        if (basePower == 0)
        {
            return Result(generation, attacker, defender, move, field, [0], desc);
        }

        var baseDamage = (int)Math.Floor(
            Math.Floor(Math.Floor((2 * attacker.Level / 5.0 + 2) * basePower * attack / defense) / 50.0) + 2);

        var rolls = new List<int>(16);
        for (var roll = 85; roll <= 100; roll++)
        {
            var damage = baseDamage;

            if (move.IsCrit)
            {
                desc.IsCritical = true;
                damage = Rounding.ApplyMod(damage, 3, 2);
            }

            damage = Rounding.ApplyRandom(damage, roll);
            damage = ApplyWeather(damage, move, field, desc);
            damage = ApplyStab(damage, attacker, move);
            damage = Rounding.ApplyMod(damage, (int)(typeEffectiveness * 100), 100);
            damage = ApplyBurn(damage, attacker, move, desc);
            damage = ApplyScreens(damage, move, field, desc);
            damage = ApplyFinalModifiers(damage, attacker, defender, move, field, typeEffectiveness, desc);

            rolls.Add(Math.Max(1, damage));
        }

        return Result(generation, attacker, defender, move, field, rolls, desc);
    }

    private static DamageResult Result(
        Generation generation,
        Pokemon attacker,
        Pokemon defender,
        Move move,
        Field field,
        IReadOnlyList<int> rolls,
        ResultDescription desc) =>
        new(generation, attacker, defender, move, field, rolls, desc);

    private static void NormalizeMove(Pokemon attacker, Move move, Field field, ResultDescription desc)
    {
        if (move.Named("Weather Ball") && !string.IsNullOrEmpty(field.Weather))
        {
            move.BasePower = 100;
            move.Type = field.Weather switch
            {
                "Sun" or "Harsh Sunshine" => "Fire",
                "Rain" or "Heavy Rain" => "Water",
                "Sand" => "Rock",
                "Hail" or "Snow" => "Ice",
                _ => move.Type
            };
        }

        if (attacker.HasAbility("Forecast") && attacker.Named("Castform") && !string.IsNullOrEmpty(field.Weather))
        {
            attacker.SpeciesOverride = field.Weather switch
            {
                "Sun" or "Harsh Sunshine" => new SpeciesData(attacker.Species.Name, attacker.Species.BaseStats, ["Fire"], attacker.WeightKg, attacker.Species.Abilities),
                "Rain" or "Heavy Rain" => new SpeciesData(attacker.Species.Name, attacker.Species.BaseStats, ["Water"], attacker.WeightKg, attacker.Species.Abilities),
                "Hail" or "Snow" => new SpeciesData(attacker.Species.Name, attacker.Species.BaseStats, ["Ice"], attacker.WeightKg, attacker.Species.Abilities),
                _ => attacker.SpeciesOverride
            };
            attacker.RefreshSpecies();
        }

        if (move.Named("Judgment") && DamageData.TryGetPlateType(attacker.Item, out var plateType))
        {
            move.Type = plateType;
            if (attacker.HasAbility("Multitype"))
            {
                attacker.SpeciesOverride = new SpeciesData(attacker.Species.Name, attacker.Species.BaseStats, [plateType], attacker.WeightKg, attacker.Species.Abilities);
                attacker.RefreshSpecies();
            }
        }

        if (move.Named("Tera Blast") && !string.IsNullOrEmpty(attacker.TeraType))
        {
            move.Type = attacker.TeraType;
            var stats = attacker.RawStats;
            var atk = Stats.ModifyByStage(stats.Atk, attacker.Boosts.Atk);
            var spa = Stats.ModifyByStage(stats.Spa, attacker.Boosts.Spa);
            move.Category = atk > spa ? MoveCategory.Physical : MoveCategory.Special;
        }

        desc.MoveType = move.Type;
        desc.MoveBasePower = move.BasePower;
    }

    private static bool BreaksProtect(Pokemon attacker, Move move) =>
        attacker.IsDynamaxed ||
        attacker.HasAbility("Unseen Fist", "Piercing Drill") && move.Flags.Contact;

    private static int? TryFixedDamage(Pokemon attacker, Move move)
    {
        if (move.Named("Seismic Toss", "Night Shade"))
        {
            return attacker.Level;
        }

        return null;
    }

    private static void MarkRelevantItems(
        Pokemon attacker,
        Pokemon defender,
        MoveCategory category,
        StatId defenseStat,
        ResultDescription desc)
    {
        if ((category == MoveCategory.Physical && attacker.HasItem("Choice Band")) ||
            (category == MoveCategory.Special && attacker.HasItem("Choice Specs")))
        {
            desc.AttackerItem = attacker.Item;
        }

        if (defender.HasItem("Eviolite") && defender.Species.CanEvolve ||
            defenseStat == StatId.Spd && defender.HasItem("Assault Vest"))
        {
            desc.DefenderItem = defender.Item;
        }
    }

    private static double GetTypeEffectiveness(Pokemon defender, Move move)
    {
        if (move.Named("Flying Press"))
        {
            return TypeChart.FlyingPressEffectiveness(defender.Types);
        }

        if (move.Named("Thousand Arrows") && defender.HasOriginalType("Flying"))
        {
            var nonFlyingTypes = defender.Types.Where(t => !string.Equals(t, "Flying", StringComparison.OrdinalIgnoreCase)).ToArray();
            return nonFlyingTypes.Length == 0 ? 1 : TypeChart.Effectiveness(move.Type, nonFlyingTypes);
        }

        return TypeChart.Effectiveness(move.Type, defender.Types);
    }

    private static int GetEffectiveStat(
        Pokemon pokemon,
        StatId stat,
        Move move,
        Field field,
        bool offensive)
    {
        var raw = pokemon.RawStats[stat];
        var stage = pokemon.Boosts[stat];

        if (move.IsCrit)
        {
            if (offensive && stage < 0)
            {
                stage = 0;
            }
            else if (!offensive && stage > 0)
            {
                stage = 0;
            }
        }

        var value = Stats.ModifyByStage(raw, stage);

        if (offensive)
        {
            if (stat == StatId.Atk && pokemon.HasAbility("Huge Power", "Pure Power"))
            {
                value *= 2;
            }

            if (stat == StatId.Atk && pokemon.HasAbility("Guts") && !string.IsNullOrEmpty(pokemon.Status))
            {
                value = Rounding.ApplyMod(value, 3, 2);
            }

            if ((stat == StatId.Atk && pokemon.HasItem("Choice Band")) ||
                (stat == StatId.Spa && pokemon.HasItem("Choice Specs")))
            {
                value = Rounding.ApplyMod(value, 3, 2);
            }
        }
        else
        {
            if (field.IsWonderRoom)
            {
                var swapped = stat == StatId.Def ? StatId.Spd : stat == StatId.Spd ? StatId.Def : stat;
                value = Stats.ModifyByStage(pokemon.RawStats[swapped], pokemon.Boosts[swapped]);
            }

            if (stat is StatId.Def or StatId.Spd && pokemon.HasItem("Eviolite") && pokemon.Species.CanEvolve)
            {
                value = Rounding.ApplyMod(value, 3, 2);
            }

            if (stat == StatId.Spd && pokemon.HasItem("Assault Vest"))
            {
                value = Rounding.ApplyMod(value, 3, 2);
            }

            if (stat == StatId.Def && pokemon.HasOriginalType("Ice") && string.Equals(field.Weather, "Snow", StringComparison.OrdinalIgnoreCase))
            {
                value = Rounding.ApplyMod(value, 3, 2);
            }
        }

        return Math.Max(1, value);
    }

    private static int GetBasePower(Pokemon attacker, Pokemon defender, Move move, Field field, ResultDescription desc)
    {
        var power = move.BasePower;

        if (move.Named("Grass Knot", "Low Kick"))
        {
            power = defender.WeightKg switch
            {
                < 10 => 20,
                < 25 => 40,
                < 50 => 60,
                < 100 => 80,
                < 200 => 100,
                _ => 120
            };
        }

        if (move.Named("Comet Punch") && move.Hits.HasValue)
        {
            power *= move.Hits.Value;
        }
        else if (move.Named("Comet Punch"))
        {
            power *= 3;
            move.Hits = 3;
        }

        if (attacker.HasAbility("Technician") && power <= 60)
        {
            power = Rounding.ApplyMod(power, 3, 2);
            desc.AttackerAbility = attacker.Ability;
        }

        if (DamageData.IsTypeBoostingItem(attacker.Item, move.Type) ||
            DamageData.TryGetPlateType(attacker.Item, out var plateType) &&
            string.Equals(plateType, move.Type, StringComparison.OrdinalIgnoreCase))
        {
            power = Rounding.ApplyMod(power, 4915, 4096);
            desc.AttackerItem = attacker.Item;
        }

        if (field.AttackerSide.IsHelpingHand)
        {
            power = Rounding.ApplyMod(power, 3, 2);
            desc.IsHelpingHand = true;
        }

        desc.MoveBasePower = power;
        return power;
    }

    private static int ApplyWeather(int damage, Move move, Field field, ResultDescription desc)
    {
        if ((field.Weather is "Sun" or "Harsh Sunshine" && move.HasType("Fire")) ||
            (field.Weather is "Rain" or "Heavy Rain" && move.HasType("Water")))
        {
            desc.Weather = field.Weather;
            return Rounding.ApplyMod(damage, 3, 2);
        }

        if ((field.Weather is "Sun" or "Harsh Sunshine" && move.HasType("Water")) ||
            (field.Weather is "Rain" or "Heavy Rain" && move.HasType("Fire")))
        {
            desc.Weather = field.Weather;
            return Rounding.ApplyMod(damage, 1, 2);
        }

        return damage;
    }

    private static int ApplyStab(int damage, Pokemon attacker, Move move)
    {
        var originalStab = attacker.HasOriginalType(move.Type);
        var teraStab = !string.IsNullOrEmpty(attacker.TeraType) &&
            string.Equals(attacker.TeraType, move.Type, StringComparison.OrdinalIgnoreCase);

        if (!originalStab && !teraStab)
        {
            return damage;
        }

        if (attacker.HasAbility("Adaptability"))
        {
            return Rounding.ApplyMod(damage, teraStab && originalStab ? 9 : 2, teraStab && originalStab ? 4 : 1);
        }

        if (teraStab && originalStab)
        {
            return Rounding.ApplyMod(damage, 2, 1);
        }

        return Rounding.ApplyMod(damage, 3, 2);
    }

    private static int ApplyBurn(int damage, Pokemon attacker, Move move, ResultDescription desc)
    {
        if (move.Category == MoveCategory.Physical &&
            attacker.HasStatus("brn", "Burn", "Burned") &&
            !attacker.HasAbility("Guts") &&
            !move.Named("Facade"))
        {
            desc.IsBurned = true;
            return Rounding.ApplyMod(damage, 1, 2);
        }

        return damage;
    }

    private static int ApplyScreens(int damage, Move move, Field field, ResultDescription desc)
    {
        if (move.IsCrit)
        {
            return damage;
        }

        var screen = move.Category == MoveCategory.Physical
            ? field.DefenderSide.IsReflect
            : field.DefenderSide.IsLightScreen;

        if (screen || field.DefenderSide.IsAuroraVeil)
        {
            desc.IsReflect = field.DefenderSide.IsReflect;
            desc.IsLightScreen = field.DefenderSide.IsLightScreen;
            desc.IsAuroraVeil = field.DefenderSide.IsAuroraVeil;
            return Rounding.ApplyMod(damage, field.IsDoubles ? 2732 : 2048, 4096);
        }

        return damage;
    }

    private static int ApplyFinalModifiers(
        int damage,
        Pokemon attacker,
        Pokemon defender,
        Move move,
        Field field,
        double typeEffectiveness,
        ResultDescription desc)
    {
        if (attacker.HasItem("Life Orb"))
        {
            damage = Rounding.ApplyMod(damage, 5324, 4096);
            desc.AttackerItem = attacker.Item;
        }

        if (attacker.HasItem("Expert Belt") && typeEffectiveness > 1)
        {
            damage = Rounding.ApplyMod(damage, 4915, 4096);
            desc.AttackerItem = attacker.Item;
        }

        if (move.Flags.Contact && attacker.HasAbility("Tough Claws"))
        {
            damage = Rounding.ApplyMod(damage, 5325, 4096);
            desc.AttackerAbility = attacker.Ability;
        }

        if (attacker.HasAbility("Supreme Overlord") && attacker.AlliesFainted > 0)
        {
            damage = Rounding.ApplyMod(damage, 10 + Math.Min(5, attacker.AlliesFainted), 10);
            desc.AttackerAbility = attacker.Ability;
        }

        if (typeEffectiveness > 1 && defender.HasAbility("Filter", "Solid Rock", "Prism Armor"))
        {
            damage = Rounding.ApplyMod(damage, 3, 4);
            desc.DefenderAbility = defender.Ability;
        }

        if (defender.HasAbility("Multiscale", "Shadow Shield") && defender.CurHp() == defender.MaxHp())
        {
            damage = Rounding.ApplyMod(damage, 1, 2);
            desc.DefenderAbility = defender.Ability;
        }

        if (field.DefenderSide.IsFriendGuard)
        {
            damage = Rounding.ApplyMod(damage, 3, 4);
            desc.IsFriendGuard = true;
        }

        if (field.IsDoubles && move.Target is MoveTarget.AllAdjacent or MoveTarget.AllAdjacentFoes)
        {
            damage = Rounding.ApplyMod(damage, 3, 4);
        }

        return damage;
    }
}

using Poke.DamageCalc;
using Poke.DamageCalc.Data;

namespace Poke.DamageCalc.Tests;

public sealed class ReverseDamageCalculatorTests
{
    [Fact]
    public void InferAttackerSpreadFindsKnownSpecialAttackCandidate()
    {
        var attacker = Pokemon.FromName("耿鬼", new PokemonOptions
        {
            Item = "Choice Specs",
            Nature = "Timid",
            Boosts = new StatsTable(0, 0, 0, 1, 0, 0)
        });
        var defender = Pokemon.FromName("吉利蛋", new PokemonOptions
        {
            Item = "Eviolite",
            Nature = "Calm",
            Evs = new StatsTable(252, 0, 0, 0, 252, 0)
        });

        var candidates = ReverseDamageCalculator.InferAttackerSpread(
            9,
            new DamageRange(274, 324),
            attacker,
            defender,
            Move.FromName("真气弹"),
            options: new ReverseDamageOptions
            {
                Stat = StatId.Spa,
                Boosts = [1],
                Ivs = [31],
                Natures = ["Timid", "Modest"]
            });

        Assert.Contains(candidates, c =>
            c.Stat == StatId.Spa &&
            c.Ev == 252 &&
            c.Iv == 31 &&
            c.Boost == 1 &&
            c.Nature == "Timid" &&
            c.ActualStat == 359 &&
            c.ExpectedRange == new DamageRange(274, 324));
    }

    [Fact]
    public void InferDefenderSpreadFindsKnownSpecialDefenseCandidate()
    {
        var attacker = Pokemon.Of(SpeciesId.Gengar, new PokemonOptions
        {
            Item = "Choice Specs",
            Nature = "Timid",
            Evs = new StatsTable(0, 0, 0, 252, 0, 0),
            Boosts = new StatsTable(0, 0, 0, 1, 0, 0)
        });
        var defender = Pokemon.Of(SpeciesId.Chansey, new PokemonOptions
        {
            Item = "Eviolite",
            Nature = "Calm",
            Evs = new StatsTable(252, 0, 0, 0, 0, 0)
        });

        var candidates = ReverseDamageCalculator.InferDefenderSpread(
            9,
            new DamageRange(274, 324),
            attacker,
            defender,
            Moves.FocusBlast,
            options: new ReverseDamageOptions
            {
                Stat = StatId.Spd,
                Boosts = [0],
                Ivs = [31],
                Natures = ["Calm", "Serious"]
            });

        Assert.Contains(candidates, c =>
            c.Stat == StatId.Spd &&
            c.Ev == 252 &&
            c.Iv == 31 &&
            c.Boost == 0 &&
            c.Nature == "Calm" &&
            c.ActualStat == 339 &&
            c.ExpectedRange == new DamageRange(274, 324));
    }

    [Fact]
    public void ContainsDamageModeMatchesSingleObservedRoll()
    {
        var candidates = ReverseDamageCalculator.InferAttackerSpread(
            9,
            new DamageRange(300, 300),
            Pokemon.Of(SpeciesId.Gengar, new PokemonOptions
            {
                Item = "Choice Specs",
                Nature = "Timid",
                Boosts = new StatsTable(0, 0, 0, 1, 0, 0)
            }),
            Pokemon.Of(SpeciesId.Chansey, new PokemonOptions
            {
                Item = "Eviolite",
                Nature = "Calm",
                Evs = new StatsTable(252, 0, 0, 0, 252, 0)
            }),
            Move.Of(MoveId.FocusBlast),
            options: new ReverseDamageOptions
            {
                Stat = StatId.Spa,
                MatchMode = DamageMatchMode.ContainsDamage,
                ObservedDamage = 300,
                Boosts = [1],
                Ivs = [31],
                Natures = ["Timid"]
            });

        Assert.Contains(candidates, c => c.Ev == 252 && c.Rolls.Contains(300));
    }
}

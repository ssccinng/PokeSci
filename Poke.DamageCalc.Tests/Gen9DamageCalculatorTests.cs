using Poke.DamageCalc;
using Poke.DamageCalc.Data;

namespace Poke.DamageCalc.Tests;

public sealed class Gen9DamageCalculatorTests
{
    [Fact]
    public void ReadmeStyleScenarioCalculatesDamageRange()
    {
        var result = DamageCalculator.Calculate(
            9,
            new Pokemon("Gengar", new PokemonOptions
            {
                Item = "Choice Specs",
                Nature = "Timid",
                Evs = new StatsTable(0, 0, 0, 252, 0, 0),
                Boosts = new StatsTable(0, 0, 0, 1, 0, 0)
            }),
            new Pokemon("Chansey", new PokemonOptions
            {
                Item = "Eviolite",
                Nature = "Calm",
                Evs = new StatsTable(252, 0, 0, 0, 252, 0)
            }),
            new Move("Focus Blast"));

        Assert.Equal((274, 324), result.Range());
        Assert.Equal(
            "252 SpA Choice Specs Gengar Focus Blast vs. 252 HP / 252+ SpD Eviolite Chansey: 274-324 (38.9 - 46%) -- guaranteed 3HKO",
            result.FullDescription());
    }

    [Fact]
    public void FixedDamageMovesUseAttackerLevel()
    {
        var result = DamageCalculator.Calculate(
            9,
            new Pokemon("Mew", new PokemonOptions { Level = 50 }),
            new Pokemon("Vulpix"),
            new Move("Seismic Toss"));

        Assert.Equal((50, 50), result.Range());
        Assert.Equal(
            "0 Atk Mew Seismic Toss vs. 0 HP / 0 Def Vulpix: 50-50 (23 - 23%) -- guaranteed 5HKO",
            result.FullDescription());
    }

    [Fact]
    public void ProtectPreventsDamage()
    {
        var result = DamageCalculator.Calculate(
            9,
            new Pokemon("Snorlax"),
            new Pokemon("Chansey"),
            new Move("Hyper Beam"),
            new Field { DefenderSide = new Side { IsProtected = true } });

        Assert.Equal((0, 0), result.Range());
        Assert.True(result.RawDescription.IsProtected);
    }

    [Fact]
    public void WeatherBallChangesTypeAndBenefitsFromForecastStab()
    {
        var result = DamageCalculator.Calculate(
            9,
            new Pokemon("Castform"),
            new Pokemon("Bulbasaur"),
            new Move("Weather Ball"),
            new Field { Weather = "Sun" });

        Assert.Equal((344, 408), result.Range());
        Assert.Equal(
            "0 SpA Castform Weather Ball (100 BP Fire) vs. 0 HP / 0 SpD Bulbasaur in Sun: 344-408 (148.9 - 176.6%) -- guaranteed OHKO",
            result.FullDescription());
    }

    [Fact]
    public void FlyingPressCombinesFightingAndFlyingEffectiveness()
    {
        var result = DamageCalculator.Calculate(
            9,
            new Pokemon("Hawlucha"),
            new Pokemon("Cacturne"),
            new Move("Flying Press"));

        Assert.Equal((612, 720), result.Range());
        Assert.Equal(4, result.RawDescription.TypeEffectiveness);
    }

    [Fact]
    public void TeraBlastUsesTeraTypeAndBestOffensiveCategory()
    {
        var result = DamageCalculator.Calculate(
            9,
            new Pokemon("Gengar", new PokemonOptions
            {
                TeraType = "Ghost",
                Nature = "Timid",
                Evs = new StatsTable(0, 0, 0, 252, 0, 0)
            }),
            new Pokemon("Mew"),
            new Move("Tera Blast"));

        Assert.Equal((352, 416), result.Range());
        Assert.Equal(MoveCategory.Special, result.Move.Category);
        Assert.Equal("Ghost", result.Move.Type);
    }

    [Fact]
    public void CriticalHitIgnoresReflectAndBurnReductionIsAppliedOtherwise()
    {
        var field = new Field { DefenderSide = new Side { IsReflect = true } };

        var normal = DamageCalculator.Calculate(
            9,
            new Pokemon("Mew", new PokemonOptions { Status = "brn" }),
            new Pokemon("Vulpix"),
            new Move("Explosion"),
            field);

        var critical = DamageCalculator.Calculate(
            9,
            new Pokemon("Mew", new PokemonOptions { Status = "brn" }),
            new Pokemon("Vulpix"),
            new Move("Explosion", new MoveOptions { IsCrit = true }),
            field);

        Assert.Equal((91, 107), normal.Range());
        Assert.True(normal.RawDescription.IsBurned);
        Assert.True(normal.RawDescription.IsReflect);
        Assert.Equal((273, 321), critical.Range());
        Assert.True(critical.RawDescription.IsCritical);
        Assert.False(critical.RawDescription.IsReflect);
    }

    [Fact]
    public void SnapshotRecordsSmogonSource()
    {
        Assert.Equal("https://github.com/smogon/damage-calc", DamageData.UpstreamRepository);
        Assert.Equal("49d4d8696bf138b101cc47be8432489c3ac192aa", DamageData.UpstreamCommit);
        Assert.Equal("MIT", DamageData.UpstreamLicense);
    }
}

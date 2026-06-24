using Poke.DamageCalc;
using Poke.DamageCalc.Data;

namespace Poke.DamageCalc.Tests;

public sealed class GeneratedDataTests
{
    [Fact]
    public void GeneratedSnapshotExposesFullGen9SmogonSpeciesAndMoves()
    {
        Assert.Equal("@smogon/calc", DamageData.GeneratedPackage);
        Assert.Equal("0.11.0", DamageData.GeneratedPackageVersion);
        Assert.Equal(9, DamageData.GeneratedGeneration);

        Assert.Equal(1451, DamageData.ListSpecies().Count);
        Assert.Equal(942, DamageData.ListMoves().Count);

        Assert.Equal("Charizard", DamageData.GetSpecies("Charizard").Name);
        Assert.Equal("Miraidon", DamageData.GetSpecies("miraidon").Name);
        Assert.Equal("Fire Blast", DamageData.GetMove("Fire Blast").Name);
        Assert.Equal("Collision Course", DamageData.GetMove("collisioncourse").Name);
    }

    [Fact]
    public void GeneratedSnapshotExposesCoreLookupTables()
    {
        Assert.Contains("Life Orb", DamageData.KnownItems);
        Assert.Contains("Beads of Ruin", DamageData.KnownAbilities);
        Assert.Contains("Timid", DamageData.AllNatures);
        Assert.Contains("Stellar", DamageData.AllTypes);

        Assert.True(TypeChart.Effectiveness("Fire", ["Grass"]) == 2);
        Assert.True(TypeChart.Effectiveness("Normal", ["Ghost"]) == 0);
    }

    [Fact]
    public void GeneratedSnapshotExposesCommonPokemonSets()
    {
        Assert.True(DamageData.ListPokemonSets().Count >= 3900);

        var gengarSets = DamageData.ListPokemonSets("Gengar");
        Assert.Contains(gengarSets, set => set.Name == "RU Choice Scarf");

        var set = DamageData.GetPokemonSet("Gengar", "RU Choice Scarf");
        Assert.Equal("gengar", set.SpeciesId);
        Assert.Equal("Cursed Body", set.Ability);
        Assert.Equal("Choice Scarf", set.Item);
        Assert.Equal("Timid", set.Nature);
        Assert.Equal("Ghost", set.TeraType);
        Assert.Equal(252, set.Evs.Spa);
        Assert.Equal(0, set.Ivs.Atk);
        Assert.Equal(["Shadow Ball", "Sludge Wave", "Trick", "Toxic Spikes"], set.Moves);
    }

    [Fact]
    public void GeneratedSpeciesAndMovesAreUsableByCalculator()
    {
        var result = DamageCalculator.Calculate(
            9,
            new Pokemon("Miraidon", new PokemonOptions
            {
                Ability = "Hadron Engine",
                Evs = new StatsTable(0, 0, 0, 252, 0, 252),
                Nature = "Timid"
            }),
            new Pokemon("Charizard"),
            new Move("Electro Drift"));

        Assert.Equal((386, 456), result.Range());
        Assert.Contains("Miraidon Electro Drift", result.FullDescription());
    }

    [Fact]
    public void GeneratedCommonSetsCanCreatePokemonOptions()
    {
        var attackerSet = DamageData.GetPokemonSet("Miraidon", "Anything Goes Choice Specs");
        var defenderSet = DamageData.GetPokemonSet("Charizard", "PU Sun Wallbreaker");

        var result = DamageCalculator.Calculate(
            9,
            attackerSet.ToPokemon(),
            defenderSet.ToPokemon(),
            new Move(attackerSet.Moves[0]));

        Assert.Equal("Choice Specs", result.Attacker.Item);
        Assert.Equal("Electro Drift", result.Move.Name);
        Assert.Equal((632, 746), result.Range());
    }
}

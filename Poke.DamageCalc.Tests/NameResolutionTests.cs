using Poke.DamageCalc;
using Poke.DamageCalc.Data;

namespace Poke.DamageCalc.Tests;

public sealed class NameResolutionTests
{
    [Fact]
    public void EnglishNameShowdownIdAndChineseAliasResolveToSameSpecies()
    {
        var english = DamageCalculator.Calculate(9, new Pokemon("Gengar"), new Pokemon("Mew"), new Move("Shadow Ball"));
        var showdownId = DamageCalculator.Calculate(9, Pokemon.FromId("gengar"), new Pokemon("mew"), Move.FromId("shadowball"));
        var chinese = DamageCalculator.Calculate(9, Pokemon.FromName("耿鬼"), Pokemon.FromName("梦幻"), Move.FromName("暗影球"));

        Assert.Equal(english.Range(), showdownId.Range());
        Assert.Equal(english.Range(), chinese.Range());
        Assert.Equal("Gengar", chinese.Attacker.Name);
        Assert.Equal("Shadow Ball", chinese.Move.Name);
    }

    [Fact]
    public void DexIdAndEnumFactoriesResolveCanonicalSpecies()
    {
        var byDex = Pokemon.OfDex(94);
        var byEnum = Pokemon.Of(SpeciesId.Gengar);

        Assert.Equal("gengar", byDex.SpeciesName);
        Assert.Equal(byEnum.SpeciesName, byDex.SpeciesName);
        Assert.Equal(Species.Gengar.Name, byDex.Species.Name);
    }

    [Fact]
    public void MoveEnumAndStaticFactoriesResolveCanonicalMoves()
    {
        var fromEnum = Move.Of(MoveId.FocusBlast);
        var fromStatic = Moves.FocusBlast;
        var fromChinese = Move.FromName("真气弹");

        Assert.Equal("focusblast", fromEnum.OriginalName);
        Assert.Equal(fromEnum.OriginalName, fromStatic.OriginalName);
        Assert.Equal(fromEnum.OriginalName, fromChinese.OriginalName);
        Assert.Equal("Focus Blast", fromChinese.Name);
    }

    [Fact]
    public void ChineseWeatherBallAliasStillTriggersSpecialMoveLogic()
    {
        var result = DamageCalculator.Calculate(
            9,
            Pokemon.FromName("漂浮泡泡"),
            Pokemon.FromName("妙蛙种子"),
            Move.FromName("气象球"),
            new Field { Weather = "Sun" });

        Assert.Equal((344, 408), result.Range());
        Assert.Equal("Weather Ball", result.Move.Name);
        Assert.Equal("Fire", result.Move.Type);
    }

    [Fact]
    public void GeneratedSnapshotExposesFullUiOptionData()
    {
        Assert.True(DamageData.AllSpecies.Count > 1000);
        Assert.True(DamageData.AllMoves.Count > 800);
        Assert.Contains(DamageData.KnownItems, item => item == "Heavy-Duty Boots");
        Assert.Contains(DamageData.KnownAbilities, ability => ability == "Toxic Debris");
        Assert.Contains(DamageData.ListPokemonSets(), set => set.SpeciesId == "glimmora" && set.Name == "BSS Reg J Debris Lead");
    }

    [Fact]
    public void GeneratedPokemonSetCanCreateUsablePokemonAndMove()
    {
        var set = DamageData.GetPokemonSet("glimmora", "BSS Reg J Debris Lead");
        var pokemon = set.ToPokemon();
        var move = new Move(set.Moves[0]);

        Assert.Equal("glimmora", pokemon.SpeciesName);
        Assert.Equal(50, pokemon.Level);
        Assert.Equal("Toxic Debris", pokemon.Ability);
        Assert.Equal("Focus Sash", pokemon.Item);
        Assert.Equal("Stealth Rock", move.Name);
    }

    [Theory]
    [InlineData("gengar", "National Dex Showdown Usage")]
    [InlineData("dragonite", "OU Choice Band")]
    [InlineData("glimmora", "BSS Reg J Debris Lead")]
    [InlineData("blissey", "OU Defensive")]
    [InlineData("dondozo", "OU Wall")]
    public void BlazorQuickPresetSetsExist(string speciesId, string setName)
    {
        var set = DamageData.GetPokemonSet(speciesId, setName);

        Assert.NotEmpty(set.Moves);
        Assert.False(string.IsNullOrWhiteSpace(set.Nature));
    }
}

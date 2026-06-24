using Poke.DamageCalc.Data;

namespace Poke.DamageCalc;

public sealed class Pokemon
{
    private SpeciesData? species;

    public Pokemon(string name, PokemonOptions? options = null)
    {
        options ??= new PokemonOptions();

        SpeciesName = DamageData.ResolveSpeciesId(name);
        Name = options.DisplayName ?? DamageData.GetSpecies(SpeciesName).Name;
        Level = options.Level ?? 100;
        Ability = options.Ability ?? Species.Abilities.FirstOrDefault();
        Item = options.Item;
        Nature = options.Nature ?? "Serious";
        Gender = options.Gender ?? "M";
        IsDynamaxed = options.IsDynamaxed;
        DynamaxLevel = options.DynamaxLevel ?? (IsDynamaxed ? 10 : 0);
        AlliesFainted = options.AlliesFainted;
        BoostedStat = options.BoostedStat;
        TeraType = options.TeraType;
        Status = options.Status ?? string.Empty;
        ToxicCounter = options.ToxicCounter;
        Ivs = options.Ivs?.Clone() ?? new StatsTable(31);
        Evs = options.Evs?.Clone() ?? new StatsTable(0);
        Boosts = options.Boosts?.Clone() ?? new StatsTable(0);
        Moves = options.Moves is null ? [] : [.. options.Moves];
        OriginalCurHp = options.CurrentHp;
        SpeciesOverride = options.SpeciesOverride;
    }

    public string Name { get; set; }

    public string SpeciesName { get; }

    public int Level { get; set; }

    public string Gender { get; set; }

    public string? Ability { get; set; }

    public bool AbilityOn { get; set; }

    public string? Item { get; set; }

    public string? DisabledItem { get; set; }

    public string Nature { get; set; }

    public string? TeraType { get; set; }

    public bool IsDynamaxed { get; set; }

    public int DynamaxLevel { get; set; }

    public int AlliesFainted { get; set; }

    public StatId? BoostedStat { get; set; }

    public string Status { get; set; }

    public int ToxicCounter { get; set; }

    public StatsTable Ivs { get; }

    public StatsTable Evs { get; }

    public StatsTable Boosts { get; }

    public IReadOnlyList<string> Moves { get; }

    public int? OriginalCurHp { get; set; }

    public SpeciesData? SpeciesOverride { get; set; }

    public SpeciesData Species => species ??= SpeciesOverride ?? DamageData.GetSpecies(SpeciesName);

    public IReadOnlyList<string> Types => Species.Types;

    public double WeightKg => Species.WeightKg;

    public StatsTable RawStats => Stats.CalculateAll(Species.BaseStats, Ivs, Evs, Level, Nature);

    public bool HasAbility(params string[] abilities) =>
        Ability is not null && abilities.Any(a => string.Equals(a, Ability, StringComparison.OrdinalIgnoreCase));

    public bool HasItem(params string[] items) =>
        Item is not null && DisabledItem is null && items.Any(i => string.Equals(i, Item, StringComparison.OrdinalIgnoreCase));

    public bool HasStatus(params string[] statuses) =>
        !string.IsNullOrEmpty(Status) && statuses.Any(s => string.Equals(s, Status, StringComparison.OrdinalIgnoreCase));

    public bool Named(params string[] names) =>
        names.Any(n => string.Equals(n, Name, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(n, Species.Name, StringComparison.OrdinalIgnoreCase));

    public bool HasOriginalType(params string[] types) =>
        Types.Any(t => types.Any(input => string.Equals(input, t, StringComparison.OrdinalIgnoreCase)));

    public bool HasType(params string[] types)
    {
        if (!string.IsNullOrEmpty(TeraType) && !string.Equals(TeraType, "Stellar", StringComparison.OrdinalIgnoreCase))
        {
            return types.Any(t => string.Equals(t, TeraType, StringComparison.OrdinalIgnoreCase));
        }

        return HasOriginalType(types);
    }

    public int MaxHp(bool original = false)
    {
        var hp = RawStats.Hp;
        if (!original && IsDynamaxed && Species.BaseStats.Hp != 1)
        {
            return (int)Math.Floor(hp * (150 + 5 * DynamaxLevel) / 100.0);
        }

        return hp;
    }

    public int CurHp(bool original = false)
    {
        var current = OriginalCurHp.GetValueOrDefault(RawStats.Hp);
        if (!original && IsDynamaxed && Species.BaseStats.Hp != 1)
        {
            return (int)Math.Ceiling(current * (150 + 5 * DynamaxLevel) / 100.0);
        }

        return current;
    }

    public void RefreshSpecies() => species = null;

    public static Pokemon Of(SpeciesId speciesId, PokemonOptions? options = null) =>
        new(DamageData.ResolveSpeciesId(speciesId), options);

    public static Pokemon OfDex(int nationalDexId, PokemonOptions? options = null) =>
        new(DamageData.ResolveSpeciesIdByDex(nationalDexId), options);

    public static Pokemon FromId(string showdownId, PokemonOptions? options = null) =>
        new(showdownId, options);

    public static Pokemon FromName(string nameOrAlias, PokemonOptions? options = null) =>
        new(nameOrAlias, options);

    public Pokemon Clone() => new(SpeciesName, new PokemonOptions
    {
        DisplayName = Name,
        Level = Level,
        Gender = Gender,
        Ability = Ability,
        Item = Item,
        Nature = Nature,
        TeraType = TeraType,
        IsDynamaxed = IsDynamaxed,
        DynamaxLevel = DynamaxLevel,
        AlliesFainted = AlliesFainted,
        BoostedStat = BoostedStat,
        Status = Status,
        ToxicCounter = ToxicCounter,
        Ivs = Ivs.Clone(),
        Evs = Evs.Clone(),
        Boosts = Boosts.Clone(),
        Moves = [.. Moves],
        CurrentHp = OriginalCurHp,
        SpeciesOverride = SpeciesOverride
    })
    {
        AbilityOn = AbilityOn,
        DisabledItem = DisabledItem
    };
}

public sealed class PokemonOptions
{
    public string? DisplayName { get; set; }

    public int? Level { get; set; }

    public string? Gender { get; set; }

    public string? Ability { get; set; }

    public string? Item { get; set; }

    public string? Nature { get; set; }

    public string? TeraType { get; set; }

    public bool IsDynamaxed { get; set; }

    public int? DynamaxLevel { get; set; }

    public int AlliesFainted { get; set; }

    public StatId? BoostedStat { get; set; }

    public string? Status { get; set; }

    public int ToxicCounter { get; set; }

    public StatsTable? Ivs { get; set; }

    public StatsTable? Evs { get; set; }

    public StatsTable? Boosts { get; set; }

    public string[]? Moves { get; set; }

    public int? CurrentHp { get; set; }

    public SpeciesData? SpeciesOverride { get; set; }
}

namespace Poke.DamageCalc.Data;

public sealed record SpeciesOption(
    string Id,
    string Name,
    int? NationalDexNumber,
    IReadOnlyList<string> Aliases,
    IReadOnlyList<string> Types);

public sealed record MoveOption(
    string Id,
    string Name,
    IReadOnlyList<string> Aliases,
    string Type,
    MoveCategory Category,
    int BasePower);

public sealed record PokemonSetOption(
    string SpeciesId,
    string SpeciesName,
    string Name,
    string? Format);

public sealed class SpeciesData
{
    public SpeciesData(
        string name,
        StatsTable baseStats,
        string[] types,
        double weightKg,
        string[]? abilities = null,
        bool canEvolve = false)
    {
        Name = name;
        BaseStats = baseStats;
        Types = types;
        WeightKg = weightKg;
        Abilities = abilities ?? [];
        CanEvolve = canEvolve;
    }

    public string Name { get; }

    public StatsTable BaseStats { get; }

    public string[] Types { get; }

    public double WeightKg { get; }

    public string[] Abilities { get; }

    public bool CanEvolve { get; }
}

public sealed class PokemonSetData
{
    public PokemonSetData(
        string speciesId,
        string speciesName,
        string name,
        string? ability = null,
        string? item = null,
        string? nature = null,
        string? teraType = null,
        int? level = null,
        StatsTable? evs = null,
        StatsTable? ivs = null,
        string[]? moves = null,
        string? format = null)
    {
        SpeciesId = speciesId;
        SpeciesName = speciesName;
        Name = name;
        Ability = ability;
        Item = item;
        Nature = nature;
        TeraType = teraType;
        Level = level;
        Evs = evs ?? new StatsTable(0);
        Ivs = ivs ?? new StatsTable(31);
        Moves = moves ?? [];
        Format = format;
    }

    public string SpeciesId { get; }

    public string SpeciesName { get; }

    public string Name { get; }

    public string? Ability { get; }

    public string? Item { get; }

    public string? Nature { get; }

    public string? TeraType { get; }

    public int? Level { get; }

    public StatsTable Evs { get; }

    public StatsTable Ivs { get; }

    public string[] Moves { get; }

    public string? Format { get; }

    public PokemonOptions ToPokemonOptions() => new()
    {
        Ability = Ability,
        Item = Item,
        Nature = Nature,
        TeraType = TeraType,
        Level = Level,
        Evs = Evs.Clone(),
        Ivs = Ivs.Clone(),
        Moves = [.. Moves]
    };

    public Pokemon ToPokemon() => new(SpeciesId, ToPokemonOptions());
}

public sealed class MoveData
{
    public MoveData(
        string name,
        int basePower,
        string type,
        MoveCategory category,
        MoveFlags? flags = null,
        MoveTarget target = MoveTarget.Any,
        int priority = 0,
        (int Min, int Max)? multiHit = null,
        (int Numerator, int Denominator)? recoil = null,
        (int Numerator, int Denominator)? drain = null)
    {
        Name = name;
        BasePower = basePower;
        Type = type;
        Category = category;
        Flags = flags ?? new MoveFlags();
        Target = target;
        Priority = priority;
        MultiHit = multiHit;
        Recoil = recoil;
        Drain = drain;
    }

    public string Name { get; private set; }

    public int BasePower { get; private set; }

    public string Type { get; private set; }

    public MoveCategory Category { get; private set; }

    public MoveFlags Flags { get; }

    public MoveTarget Target { get; }

    public int Priority { get; }

    public (int Min, int Max)? MultiHit { get; }

    public (int Numerator, int Denominator)? Recoil { get; }

    public (int Numerator, int Denominator)? Drain { get; }

    public MoveData WithBasePower(int basePower)
    {
        BasePower = basePower;
        return this;
    }

    public MoveData WithType(string type)
    {
        Type = type;
        return this;
    }

    public MoveData WithCategory(MoveCategory category)
    {
        Category = category;
        return this;
    }

    public MoveData Clone() => new(Name, BasePower, Type, Category, Flags.Clone(), Target, Priority, MultiHit, Recoil, Drain);
}

public sealed class MoveFlags
{
    public bool Contact { get; init; }

    public bool Punch { get; init; }

    public bool Sound { get; init; }

    public bool Bite { get; init; }

    public bool Bullet { get; init; }

    public bool Pulse { get; init; }

    public bool Slicing { get; init; }

    public bool Wind { get; init; }

    public MoveFlags Clone() => new()
    {
        Contact = Contact,
        Punch = Punch,
        Sound = Sound,
        Bite = Bite,
        Bullet = Bullet,
        Pulse = Pulse,
        Slicing = Slicing,
        Wind = Wind
    };
}

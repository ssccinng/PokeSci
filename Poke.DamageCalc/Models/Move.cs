using Poke.DamageCalc.Data;

namespace Poke.DamageCalc;

public sealed class Move
{
    private readonly MoveData? overrideData;

    public Move(string name, MoveOptions? options = null)
    {
        options ??= new MoveOptions();

        OriginalName = DamageData.ResolveMoveId(name);
        Name = options.DisplayName ?? DamageData.GetMove(OriginalName).Name;
        overrideData = options.OverrideData;
        Ability = options.Ability;
        Item = options.Item;
        Species = options.Species;
        UseZ = options.UseZ;
        UseMax = options.UseMax;
        IsCrit = options.IsCrit;
        IsStellarFirstUse = options.IsStellarFirstUse;
        Hits = options.Hits;
        TimesUsed = options.TimesUsed ?? 1;
        TimesUsedWithMetronome = options.TimesUsedWithMetronome;
    }

    public string Name { get; set; }

    public string OriginalName { get; }

    public string? Ability { get; set; }

    public string? Item { get; set; }

    public string? Species { get; set; }

    public bool UseZ { get; set; }

    public bool UseMax { get; set; }

    public bool IsCrit { get; set; }

    public bool IsStellarFirstUse { get; set; }

    public int? Hits { get; set; }

    public int TimesUsed { get; set; }

    public int? TimesUsedWithMetronome { get; set; }

    public MoveData Data => overrideData ?? DamageData.GetMove(OriginalName);

    public int BasePower
    {
        get => Data.BasePower;
        set => Data.WithBasePower(value);
    }

    public string Type
    {
        get => Data.Type;
        set => Data.WithType(value);
    }

    public MoveCategory Category
    {
        get => Data.Category;
        set => Data.WithCategory(value);
    }

    public MoveFlags Flags => Data.Flags;

    public MoveTarget Target => Data.Target;

    public bool IsStatus => Category == MoveCategory.Status;

    public bool Named(params string[] names) =>
        names.Any(n => string.Equals(n, Name, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(n, Data.Name, StringComparison.OrdinalIgnoreCase));

    public bool HasType(params string[] types) =>
        types.Any(t => string.Equals(t, Type, StringComparison.OrdinalIgnoreCase));

    public Move Clone() => new(OriginalName, new MoveOptions
    {
        DisplayName = Name,
        Ability = Ability,
        Item = Item,
        Species = Species,
        UseZ = UseZ,
        UseMax = UseMax,
        IsCrit = IsCrit,
        IsStellarFirstUse = IsStellarFirstUse,
        Hits = Hits,
        TimesUsed = TimesUsed,
        TimesUsedWithMetronome = TimesUsedWithMetronome,
        OverrideData = Data.Clone()
    });

    public static Move Of(MoveId moveId, MoveOptions? options = null) =>
        new(DamageData.ResolveMoveId(moveId), options);

    public static Move FromId(string showdownId, MoveOptions? options = null) =>
        new(showdownId, options);

    public static Move FromName(string nameOrAlias, MoveOptions? options = null) =>
        new(nameOrAlias, options);
}

public sealed class MoveOptions
{
    public string? DisplayName { get; set; }

    public string? Ability { get; set; }

    public string? Item { get; set; }

    public string? Species { get; set; }

    public bool UseZ { get; set; }

    public bool UseMax { get; set; }

    public bool IsCrit { get; set; }

    public bool IsStellarFirstUse { get; set; }

    public int? Hits { get; set; }

    public int? TimesUsed { get; set; }

    public int? TimesUsedWithMetronome { get; set; }

    public MoveData? OverrideData { get; set; }
}

public enum MoveCategory
{
    Physical,
    Special,
    Status
}

public enum MoveTarget
{
    Any,
    Normal,
    AllAdjacent,
    AllAdjacentFoes,
    Self
}

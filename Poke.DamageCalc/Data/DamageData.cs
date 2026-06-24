using System.Collections.ObjectModel;

namespace Poke.DamageCalc.Data;

public static partial class DamageData
{
    public const string UpstreamRepository = "https://github.com/smogon/damage-calc";

    public const string UpstreamCommit = "49d4d8696bf138b101cc47be8432489c3ac192aa";

    public const string UpstreamLicense = "MIT";

    private static readonly Dictionary<string, SpeciesData> SpeciesById = new(StringComparer.OrdinalIgnoreCase)
    {
        ["abomasnow"] = Species("Abomasnow", 90, 92, 75, 92, 85, 60, ["Grass", "Ice"], 135.5, ["Snow Warning"]),
        ["arceus"] = Species("Arceus", 120, 120, 120, 120, 120, 120, ["Normal"], 320.0, ["Multitype"]),
        ["blastoise"] = Species("Blastoise", 79, 83, 100, 85, 105, 78, ["Water"], 85.5, ["Torrent"]),
        ["bulbasaur"] = Species("Bulbasaur", 45, 49, 49, 65, 65, 45, ["Grass", "Poison"], 6.9, ["Overgrow"], true),
        ["cacturne"] = Species("Cacturne", 70, 115, 60, 115, 60, 55, ["Grass", "Dark"], 77.4, ["Sand Veil"]),
        ["castform"] = Species("Castform", 70, 70, 70, 70, 70, 70, ["Normal"], 0.8, ["Forecast"]),
        ["chansey"] = Species("Chansey", 250, 5, 5, 35, 105, 50, ["Normal"], 34.6, ["Natural Cure"], true),
        ["dragonite"] = Species("Dragonite", 91, 134, 95, 100, 100, 80, ["Dragon", "Flying"], 210.0, ["Inner Focus"]),
        ["gengar"] = Species("Gengar", 60, 65, 60, 130, 75, 110, ["Ghost", "Poison"], 40.5, ["Cursed Body"]),
        ["groudon"] = Species("Groudon", 100, 150, 140, 100, 90, 90, ["Ground"], 950.0, ["Drought"]),
        ["hawlucha"] = Species("Hawlucha", 78, 92, 75, 74, 63, 118, ["Fighting", "Flying"], 21.5, ["Limber"]),
        ["mew"] = Species("Mew", 100, 100, 100, 100, 100, 100, ["Psychic"], 4.0, ["Synchronize"]),
        ["primarina"] = Species("Primarina", 80, 74, 74, 126, 116, 60, ["Water", "Fairy"], 44.0, ["Torrent"]),
        ["spiritomb"] = Species("Spiritomb", 50, 92, 108, 92, 108, 35, ["Ghost", "Dark"], 108.0, ["Pressure"]),
        ["snorlax"] = Species("Snorlax", 160, 110, 65, 65, 110, 30, ["Normal"], 460.0, ["Immunity"]),
        ["swampertmega"] = Species("Swampert-Mega", 100, 150, 110, 95, 110, 70, ["Water", "Ground"], 102.0, ["Swift Swim"]),
        ["swellow"] = Species("Swellow", 60, 85, 60, 75, 50, 125, ["Normal", "Flying"], 19.8, ["Guts"]),
        ["vulpix"] = Species("Vulpix", 38, 41, 40, 50, 65, 65, ["Fire"], 9.9, ["Flash Fire"], true),
        ["zygarde"] = Species("Zygarde", 108, 100, 121, 81, 95, 95, ["Dragon", "Ground"], 305.0, ["Aura Break"])
    };

    private static readonly Dictionary<SpeciesId, string> SpeciesIdMap = new()
    {
        [SpeciesId.Abomasnow] = "abomasnow",
        [SpeciesId.Arceus] = "arceus",
        [SpeciesId.Blastoise] = "blastoise",
        [SpeciesId.Bulbasaur] = "bulbasaur",
        [SpeciesId.Cacturne] = "cacturne",
        [SpeciesId.Castform] = "castform",
        [SpeciesId.Chansey] = "chansey",
        [SpeciesId.Dragonite] = "dragonite",
        [SpeciesId.Gengar] = "gengar",
        [SpeciesId.Groudon] = "groudon",
        [SpeciesId.Hawlucha] = "hawlucha",
        [SpeciesId.Mew] = "mew",
        [SpeciesId.Primarina] = "primarina",
        [SpeciesId.Spiritomb] = "spiritomb",
        [SpeciesId.Snorlax] = "snorlax",
        [SpeciesId.SwampertMega] = "swampertmega",
        [SpeciesId.Swellow] = "swellow",
        [SpeciesId.Vulpix] = "vulpix",
        [SpeciesId.Zygarde] = "zygarde"
    };

    private static readonly Dictionary<int, string> DefaultSpeciesByDex = new()
    {
        [1] = "bulbasaur",
        [9] = "blastoise",
        [37] = "vulpix",
        [94] = "gengar",
        [113] = "chansey",
        [143] = "snorlax",
        [149] = "dragonite",
        [151] = "mew",
        [277] = "swellow",
        [332] = "cacturne",
        [351] = "castform",
        [383] = "groudon",
        [442] = "spiritomb",
        [460] = "abomasnow",
        [493] = "arceus",
        [701] = "hawlucha",
        [718] = "zygarde",
        [730] = "primarina"
    };

    private static readonly Dictionary<string, string> SpeciesAliases = new(StringComparer.OrdinalIgnoreCase)
    {
        ["暴雪王"] = "abomasnow",
        ["阿尔宙斯"] = "arceus",
        ["水箭龟"] = "blastoise",
        ["妙蛙种子"] = "bulbasaur",
        ["梦歌仙人掌"] = "cacturne",
        ["漂浮泡泡"] = "castform",
        ["吉利蛋"] = "chansey",
        ["快龙"] = "dragonite",
        ["耿鬼"] = "gengar",
        ["固拉多"] = "groudon",
        ["摔角鹰人"] = "hawlucha",
        ["梦幻"] = "mew",
        ["西狮海壬"] = "primarina",
        ["花岩怪"] = "spiritomb",
        ["卡比兽"] = "snorlax",
        ["巨沼怪Mega"] = "swampertmega",
        ["超级巨沼怪"] = "swampertmega",
        ["大王燕"] = "swellow",
        ["六尾"] = "vulpix",
        ["基格尔德"] = "zygarde"
    };

    private static readonly Dictionary<string, MoveData> MovesById = new(StringComparer.OrdinalIgnoreCase)
    {
        ["amnesia"] = Move("Amnesia", 0, "Psychic", MoveCategory.Status),
        ["closecombat"] = Move("Close Combat", 120, "Fighting", MoveCategory.Physical, new MoveFlags { Contact = true }),
        ["cometpunch"] = Move("Comet Punch", 18, "Normal", MoveCategory.Physical, new MoveFlags { Contact = true, Punch = true }, multiHit: (2, 5)),
        ["earthquake"] = Move("Earthquake", 100, "Ground", MoveCategory.Physical, target: MoveTarget.AllAdjacent),
        ["explosion"] = Move("Explosion", 250, "Normal", MoveCategory.Physical),
        ["flamethrower"] = Move("Flamethrower", 90, "Fire", MoveCategory.Special),
        ["flyingpress"] = Move("Flying Press", 100, "Fighting", MoveCategory.Physical, new MoveFlags { Contact = true }),
        ["focusblast"] = Move("Focus Blast", 120, "Fighting", MoveCategory.Special, new MoveFlags { Bullet = true }),
        ["grassknot"] = Move("Grass Knot", 0, "Grass", MoveCategory.Special, new MoveFlags { Contact = true }),
        ["hydropump"] = Move("Hydro Pump", 110, "Water", MoveCategory.Special),
        ["hyperbeam"] = Move("Hyper Beam", 150, "Normal", MoveCategory.Special),
        ["judgment"] = Move("Judgment", 100, "Normal", MoveCategory.Special),
        ["moonblast"] = Move("Moonblast", 95, "Fairy", MoveCategory.Special),
        ["nightshade"] = Move("Night Shade", 0, "Ghost", MoveCategory.Special),
        ["seismictoss"] = Move("Seismic Toss", 0, "Fighting", MoveCategory.Physical),
        ["shadowball"] = Move("Shadow Ball", 80, "Ghost", MoveCategory.Special),
        ["surf"] = Move("Surf", 90, "Water", MoveCategory.Special, target: MoveTarget.AllAdjacent),
        ["terablast"] = Move("Tera Blast", 80, "Normal", MoveCategory.Special),
        ["thousandarrows"] = Move("Thousand Arrows", 90, "Ground", MoveCategory.Physical),
        ["weatherball"] = Move("Weather Ball", 50, "Normal", MoveCategory.Special)
    };

    private static readonly Dictionary<MoveId, string> MoveIdMap = new()
    {
        [MoveId.Amnesia] = "amnesia",
        [MoveId.CloseCombat] = "closecombat",
        [MoveId.CometPunch] = "cometpunch",
        [MoveId.Earthquake] = "earthquake",
        [MoveId.Explosion] = "explosion",
        [MoveId.Flamethrower] = "flamethrower",
        [MoveId.FlyingPress] = "flyingpress",
        [MoveId.FocusBlast] = "focusblast",
        [MoveId.GrassKnot] = "grassknot",
        [MoveId.HydroPump] = "hydropump",
        [MoveId.HyperBeam] = "hyperbeam",
        [MoveId.Judgment] = "judgment",
        [MoveId.Moonblast] = "moonblast",
        [MoveId.NightShade] = "nightshade",
        [MoveId.SeismicToss] = "seismictoss",
        [MoveId.ShadowBall] = "shadowball",
        [MoveId.Surf] = "surf",
        [MoveId.TeraBlast] = "terablast",
        [MoveId.ThousandArrows] = "thousandarrows",
        [MoveId.WeatherBall] = "weatherball"
    };

    private static readonly Dictionary<string, string> MoveAliases = new(StringComparer.OrdinalIgnoreCase)
    {
        ["瞬间失忆"] = "amnesia",
        ["近身战"] = "closecombat",
        ["连续拳"] = "cometpunch",
        ["地震"] = "earthquake",
        ["大爆炸"] = "explosion",
        ["喷射火焰"] = "flamethrower",
        ["飞身重压"] = "flyingpress",
        ["真气弹"] = "focusblast",
        ["气合弹"] = "focusblast",
        ["打草结"] = "grassknot",
        ["水炮"] = "hydropump",
        ["加农光炮"] = "hyperbeam",
        ["破坏光线"] = "hyperbeam",
        ["制裁光砾"] = "judgment",
        ["月亮之力"] = "moonblast",
        ["黑夜魔影"] = "nightshade",
        ["地球上投"] = "seismictoss",
        ["暗影球"] = "shadowball",
        ["冲浪"] = "surf",
        ["太晶爆发"] = "terablast",
        ["千箭齐发"] = "thousandarrows",
        ["气象球"] = "weatherball"
    };

    private static readonly ReadOnlyDictionary<string, string> PlateTypes = new(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["Draco Plate"] = "Dragon",
        ["Dread Plate"] = "Dark",
        ["Earth Plate"] = "Ground",
        ["Fist Plate"] = "Fighting",
        ["Flame Plate"] = "Fire",
        ["Icicle Plate"] = "Ice",
        ["Insect Plate"] = "Bug",
        ["Iron Plate"] = "Steel",
        ["Meadow Plate"] = "Grass",
        ["Mind Plate"] = "Psychic",
        ["Pixie Plate"] = "Fairy",
        ["Sky Plate"] = "Flying",
        ["Splash Plate"] = "Water",
        ["Spooky Plate"] = "Ghost",
        ["Stone Plate"] = "Rock",
        ["Toxic Plate"] = "Poison",
        ["Zap Plate"] = "Electric"
    });

    private static readonly ReadOnlyDictionary<string, string> TypeBoostItems = new(new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    {
        ["Black Belt"] = "Fighting",
        ["Black Glasses"] = "Dark",
        ["Charcoal"] = "Fire",
        ["Dragon Fang"] = "Dragon",
        ["Hard Stone"] = "Rock",
        ["Magnet"] = "Electric",
        ["Metal Coat"] = "Steel",
        ["Miracle Seed"] = "Grass",
        ["Mystic Water"] = "Water",
        ["Never-Melt Ice"] = "Ice",
        ["Poison Barb"] = "Poison",
        ["Sharp Beak"] = "Flying",
        ["Silk Scarf"] = "Normal",
        ["Silver Powder"] = "Bug",
        ["Soft Sand"] = "Ground",
        ["Spell Tag"] = "Ghost",
        ["Twisted Spoon"] = "Psychic",
        ["Fairy Feather"] = "Fairy"
    });

    private static readonly string[] BattleTypes =
    [
        "Normal",
        "Fire",
        "Water",
        "Electric",
        "Grass",
        "Ice",
        "Fighting",
        "Poison",
        "Ground",
        "Flying",
        "Psychic",
        "Bug",
        "Rock",
        "Ghost",
        "Dragon",
        "Dark",
        "Steel",
        "Fairy",
        "Stellar"
    ];

    private static readonly string[] BattleNatures =
    [
        "Hardy",
        "Lonely",
        "Brave",
        "Adamant",
        "Naughty",
        "Bold",
        "Docile",
        "Relaxed",
        "Impish",
        "Lax",
        "Timid",
        "Hasty",
        "Serious",
        "Jolly",
        "Naive",
        "Modest",
        "Mild",
        "Quiet",
        "Bashful",
        "Rash",
        "Calm",
        "Gentle",
        "Sassy",
        "Careful",
        "Quirky"
    ];

    private static readonly string[] BattleWeathers =
    [
        "",
        "Sun",
        "Harsh Sunshine",
        "Rain",
        "Heavy Rain",
        "Sand",
        "Hail",
        "Snow"
    ];

    private static readonly string[] BattleTerrains =
    [
        "",
        "Electric",
        "Grassy",
        "Misty",
        "Psychic"
    ];

    private static readonly string[] BattleStatuses =
    [
        "",
        "brn",
        "par",
        "psn",
        "tox",
        "slp",
        "frz"
    ];

    private static readonly string[] ExtraKnownItems =
    [
        "Assault Vest",
        "Choice Band",
        "Choice Specs",
        "Eviolite",
        "Expert Belt",
        "Life Orb"
    ];

    private static readonly string[] ExtraKnownAbilities =
    [
        "Adaptability",
        "Aura Break",
        "Cursed Body",
        "Drought",
        "Filter",
        "Flash Fire",
        "Forecast",
        "Guts",
        "Huge Power",
        "Inner Focus",
        "Immunity",
        "Limber",
        "Multiscale",
        "Multitype",
        "Natural Cure",
        "Overgrow",
        "Piercing Drill",
        "Pressure",
        "Prism Armor",
        "Pure Power",
        "Sand Veil",
        "Shadow Shield",
        "Solid Rock",
        "Supreme Overlord",
        "Swift Swim",
        "Synchronize",
        "Technician",
        "Torrent",
        "Tough Claws",
        "Unseen Fist"
    ];

    public static IReadOnlyList<SpeciesOption> AllSpecies => ListSpecies();

    public static IReadOnlyList<MoveOption> AllMoves => ListMoves();

    public static IReadOnlyList<string> AllTypes => GetGeneratedTypes()
        .Concat(BattleTypes)
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .OrderBy(type => type, StringComparer.OrdinalIgnoreCase)
        .ToArray();

    public static IReadOnlyList<string> AllNatures => GetGeneratedNatures()
        .Concat(BattleNatures)
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .OrderBy(nature => nature, StringComparer.OrdinalIgnoreCase)
        .ToArray();

    public static IReadOnlyList<string> AllWeathers { get; } = BattleWeathers;

    public static IReadOnlyList<string> AllTerrains { get; } = BattleTerrains;

    public static IReadOnlyList<string> AllStatuses { get; } = BattleStatuses;

    public static IReadOnlyList<string> KnownItems => GetGeneratedItems()
        .Concat(ExtraKnownItems)
        .Concat(PlateTypes.Keys)
        .Concat(TypeBoostItems.Keys)
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .OrderBy(item => item, StringComparer.OrdinalIgnoreCase)
        .ToArray();

    public static IReadOnlyList<string> KnownAbilities => GetGeneratedAbilities()
        .Concat(SpeciesById.Values
        .SelectMany(species => species.Abilities)
        .Concat(ExtraKnownAbilities))
        .Where(ability => !string.IsNullOrWhiteSpace(ability))
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .OrderBy(ability => ability, StringComparer.OrdinalIgnoreCase)
        .ToArray();

    public static SpeciesData GetSpecies(string name)
    {
        var id = ResolveSpeciesId(name);
        return TryGetGeneratedSpecies(id, out var generatedSpecies)
            ? generatedSpecies
            : SpeciesById.TryGetValue(id, out var species)
            ? species
            : throw new KeyNotFoundException($"Species '{name}' is not in the current Gen9 damage data snapshot.");
    }

    public static SpeciesData GetSpecies(SpeciesId speciesId) => GetSpecies(ResolveSpeciesId(speciesId));

    public static SpeciesData GetSpeciesByDex(int nationalDexId) => GetSpecies(ResolveSpeciesIdByDex(nationalDexId));

    public static MoveData GetMove(string name)
    {
        var id = ResolveMoveId(name);
        return TryGetGeneratedMove(id, out var generatedMove)
            ? generatedMove.Clone()
            : MovesById.TryGetValue(id, out var move)
            ? move.Clone()
            : throw new KeyNotFoundException($"Move '{name}' is not in the current Gen9 damage data snapshot.");
    }

    public static MoveData GetMove(MoveId moveId) => GetMove(ResolveMoveId(moveId));

    public static PokemonSetData GetPokemonSet(string speciesNameOrAlias, string setName)
    {
        var speciesId = ResolveSpeciesId(speciesNameOrAlias);
        return TryGetGeneratedPokemonSet(speciesId, setName, out var set)
            ? set
            : throw new KeyNotFoundException($"Pokemon set '{speciesNameOrAlias} / {setName}' is not in the current Gen9 set snapshot.");
    }

    public static string ResolveSpeciesId(SpeciesId speciesId) => SpeciesIdMap[speciesId];

    public static string ResolveSpeciesIdByDex(int nationalDexId)
    {
        return TryResolveGeneratedSpeciesDex(nationalDexId, out var generatedId)
            ? generatedId
            : DefaultSpeciesByDex.TryGetValue(nationalDexId, out var id)
            ? id
            : throw new KeyNotFoundException($"National Dex id '{nationalDexId}' is not in the current Gen9 damage data snapshot.");
    }

    public static string ResolveSpeciesId(string nameOrAlias)
    {
        if (TryResolveGeneratedSpeciesAlias(nameOrAlias, out var generatedAliasId))
        {
            return generatedAliasId;
        }

        if (SpeciesAliases.TryGetValue(nameOrAlias, out var aliasId))
        {
            return aliasId;
        }

        var id = Ids.ToId(nameOrAlias);
        return HasGeneratedSpecies(id) || SpeciesById.ContainsKey(id)
            ? id
            : throw new KeyNotFoundException($"Species '{nameOrAlias}' is not in the current Gen9 damage data snapshot.");
    }

    public static string ResolveMoveId(MoveId moveId) => MoveIdMap[moveId];

    public static string ResolveMoveId(string nameOrAlias)
    {
        if (TryResolveGeneratedMoveAlias(nameOrAlias, out var generatedAliasId))
        {
            return generatedAliasId;
        }

        if (MoveAliases.TryGetValue(nameOrAlias, out var aliasId))
        {
            return aliasId;
        }

        var id = Ids.ToId(nameOrAlias);
        return HasGeneratedMove(id) || MovesById.ContainsKey(id)
            ? id
            : throw new KeyNotFoundException($"Move '{nameOrAlias}' is not in the current Gen9 damage data snapshot.");
    }

    public static bool TryGetPlateType(string? item, out string type)
    {
        if (item is not null && PlateTypes.TryGetValue(item, out var found))
        {
            type = found;
            return true;
        }

        type = string.Empty;
        return false;
    }

    public static bool IsTypeBoostingItem(string? item, string type) =>
        item is not null && TypeBoostItems.TryGetValue(item, out var boostType) &&
        string.Equals(boostType, type, StringComparison.OrdinalIgnoreCase);

    public static IReadOnlyList<SpeciesOption> ListSpecies() =>
        SpeciesById.Select(kvp => new SpeciesOption(
                kvp.Key,
                kvp.Value.Name,
                NationalDexNumberFor(kvp.Key),
                SpeciesAliases.Where(alias => string.Equals(alias.Value, kvp.Key, StringComparison.OrdinalIgnoreCase))
                    .Select(alias => alias.Key)
                    .OrderBy(alias => alias, StringComparer.OrdinalIgnoreCase)
                    .ToArray(),
                kvp.Value.Types))
            .Concat(GetGeneratedSpeciesOptions())
            .GroupBy(option => option.Id, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.First())
            .OrderBy(option => option.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();

    public static IReadOnlyList<MoveOption> ListMoves() =>
        MovesById.Select(kvp => new MoveOption(
                kvp.Key,
                kvp.Value.Name,
                MoveAliases.Where(alias => string.Equals(alias.Value, kvp.Key, StringComparison.OrdinalIgnoreCase))
                    .Select(alias => alias.Key)
                    .OrderBy(alias => alias, StringComparer.OrdinalIgnoreCase)
                    .ToArray(),
                kvp.Value.Type,
                kvp.Value.Category,
                kvp.Value.BasePower))
            .Concat(GetGeneratedMoveOptions())
            .GroupBy(option => option.Id, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.First())
            .OrderBy(option => option.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();

    public static IReadOnlyList<PokemonSetOption> ListPokemonSets() => GetGeneratedPokemonSetOptions()
        .OrderBy(option => option.SpeciesName, StringComparer.OrdinalIgnoreCase)
        .ThenBy(option => option.Name, StringComparer.OrdinalIgnoreCase)
        .ToArray();

    public static IReadOnlyList<PokemonSetOption> ListPokemonSets(string speciesNameOrAlias)
    {
        var speciesId = ResolveSpeciesId(speciesNameOrAlias);
        return GetGeneratedPokemonSetOptions()
            .Where(option => string.Equals(option.SpeciesId, speciesId, StringComparison.OrdinalIgnoreCase))
            .OrderBy(option => option.Name, StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }

    private static partial bool TryGetGeneratedSpecies(string id, out SpeciesData species);

    private static partial bool TryGetGeneratedMove(string id, out MoveData move);

    private static partial bool TryGetGeneratedPokemonSet(string speciesId, string setName, out PokemonSetData set);

    private static partial bool HasGeneratedSpecies(string id);

    private static partial bool HasGeneratedMove(string id);

    private static partial bool TryResolveGeneratedSpeciesAlias(string nameOrAlias, out string id);

    private static partial bool TryResolveGeneratedMoveAlias(string nameOrAlias, out string id);

    private static partial bool TryResolveGeneratedSpeciesDex(int nationalDexId, out string id);

    private static partial IReadOnlyList<SpeciesOption> GetGeneratedSpeciesOptions();

    private static partial IReadOnlyList<MoveOption> GetGeneratedMoveOptions();

    private static partial IReadOnlyList<PokemonSetOption> GetGeneratedPokemonSetOptions();

    private static partial IReadOnlyList<string> GetGeneratedItems();

    private static partial IReadOnlyList<string> GetGeneratedAbilities();

    private static partial IReadOnlyList<string> GetGeneratedNatures();

    private static partial IReadOnlyList<string> GetGeneratedTypes();

    private static int? NationalDexNumberFor(string speciesId)
    {
        foreach (var (dex, id) in DefaultSpeciesByDex)
        {
            if (string.Equals(id, speciesId, StringComparison.OrdinalIgnoreCase))
            {
                return dex;
            }
        }

        return null;
    }

    private static SpeciesData Species(
        string name,
        int hp,
        int atk,
        int def,
        int spa,
        int spd,
        int spe,
        string[] types,
        double weightKg,
        string[]? abilities = null,
        bool canEvolve = false) =>
        new(name, new StatsTable(hp, atk, def, spa, spd, spe), types, weightKg, abilities, canEvolve);

    private static MoveData Move(
        string name,
        int basePower,
        string type,
        MoveCategory category,
        MoveFlags? flags = null,
        MoveTarget target = MoveTarget.Any,
        int priority = 0,
        (int Min, int Max)? multiHit = null) =>
        new(name, basePower, type, category, flags, target, priority, multiHit);
}

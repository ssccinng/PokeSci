using Microsoft.EntityFrameworkCore;
using PokemonDataAccess.Models;

namespace DatabaseTool;

internal sealed class DatabaseMaintenanceService
{
    private readonly PokeCommon.API.Data.PokeDBContext pokeDb;
    private readonly ShitLibDataSource source;
    private readonly CliOptions options;

    public DatabaseMaintenanceService(
        PokeCommon.API.Data.PokeDBContext pokeDb,
        ShitLibDataSource source,
        CliOptions options)
    {
        this.pokeDb = pokeDb;
        this.source = source;
        this.options = options;
    }

    public async Task<int> RunAsync()
    {
        Console.WriteLine($"Command: {options.Command}");
        Console.WriteLine(options.Apply ? "Mode: apply" : "Mode: dry-run");
        Console.WriteLine($"ShitLib data: {source.DataDirectory}");

        switch (options.Command.ToLowerInvariant())
        {
            case "summary":
                await PrintSummaryAsync();
                break;
            case "sync-all":
                await SyncAbilitiesAsync();
                await SyncItemsAsync();
                await SyncMovesAsync();
                await SyncPokemonAsync();
                await LinkPsPokemonAsync();
                break;
            case "sync-abilities":
                await SyncAbilitiesAsync();
                break;
            case "sync-items":
                await SyncItemsAsync();
                break;
            case "sync-moves":
                await SyncMovesAsync();
                break;
            case "sync-pokemon":
                await SyncPokemonAsync();
                break;
            case "link-ps":
                await LinkPsPokemonAsync();
                break;
            default:
                throw new ArgumentException($"Unsupported command: {options.Command}");
        }

        return 0;
    }

    private async Task PrintSummaryAsync()
    {
        Console.WriteLine();
        Console.WriteLine("Source counts:");
        Console.WriteLine($"  Abilities : {source.Abilities.Count}");
        Console.WriteLine($"  Items     : {source.Items.Count}");
        Console.WriteLine($"  Moves     : {source.Moves.Count}");
        Console.WriteLine($"  Pokemon   : {source.Pokemons.Count}");

        Console.WriteLine();
        Console.WriteLine("Database counts:");
        try
        {
            Console.WriteLine($"  Abilities : {await pokeDb.Abilities.CountAsync()}");
            Console.WriteLine($"  Items     : {await pokeDb.Items.CountAsync()}");
            Console.WriteLine($"  Moves     : {await pokeDb.Moves.CountAsync()}");
            Console.WriteLine($"  Pokemon   : {await pokeDb.Pokemons.CountAsync()}");
            Console.WriteLine($"  PSPokemon : {await pokeDb.PSPokemons.CountAsync()}");
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Missing database connection string", StringComparison.Ordinal))
        {
            Console.WriteLine("  skipped: pass --connection or set POKEMON_DB_CONNECTION");
        }
    }

    private async Task SyncAbilitiesAsync()
    {
        var existing = await pokeDb.Abilities.ToListAsync();
        var byId = existing.ToDictionary(static x => x.AbilityId);
        var stats = new ChangeStats("abilities");

        foreach (var item in source.Abilities)
        {
            if (item.SourceId <= 0)
            {
                continue;
            }

            if (!byId.TryGetValue(item.SourceId, out var ability))
            {
                ability = new Ability
                {
                    AbilityId = item.SourceId,
                    Name_Chs = item.Chinese,
                    Name_Eng = item.English,
                    Name_Jpn = item.Japanese,
                    description_Chs = string.Empty,
                    description_Eng = string.Empty,
                    description_Jpn = string.Empty,
                };
                pokeDb.Abilities.Add(ability);
                byId[item.SourceId] = ability;
                stats.Added++;
                continue;
            }

            var changed = false;
            changed |= SetText(ability.Name_Chs, item.Chinese, value => ability.Name_Chs = value);
            changed |= SetText(ability.Name_Eng, item.English, value => ability.Name_Eng = value);
            changed |= SetText(ability.Name_Jpn, item.Japanese, value => ability.Name_Jpn = value);

            if (changed)
            {
                stats.Updated++;
            }
        }

        await SaveAndPrintAsync(stats);
    }

    private async Task SyncItemsAsync()
    {
        var existing = await pokeDb.Items.ToListAsync();
        var byId = existing.ToDictionary(static x => x.ItemId);
        var stats = new ChangeStats("items");

        foreach (var item in source.Items)
        {
            if (item.SourceId <= 0)
            {
                continue;
            }

            if (!byId.TryGetValue(item.SourceId, out var dbItem))
            {
                dbItem = new Item
                {
                    ItemId = item.SourceId,
                    Name_Chs = item.Chinese,
                    Name_Eng = item.English,
                    Name_Jpn = item.Japanese,
                    description_Chs = string.Empty,
                    description_Eng = string.Empty,
                    description_Jpn = string.Empty,
                    Item_Type = ItemType.Others,
                };
                pokeDb.Items.Add(dbItem);
                byId[item.SourceId] = dbItem;
                stats.Added++;
                continue;
            }

            var changed = false;
            changed |= SetText(dbItem.Name_Chs, item.Chinese, value => dbItem.Name_Chs = value);
            changed |= SetText(dbItem.Name_Eng, item.English, value => dbItem.Name_Eng = value);
            changed |= SetText(dbItem.Name_Jpn, item.Japanese, value => dbItem.Name_Jpn = value);

            if (changed)
            {
                stats.Updated++;
            }
        }

        await SaveAndPrintAsync(stats);
    }

    private async Task SyncMovesAsync()
    {
        var moves = await pokeDb.Moves.Include(static x => x.MoveType).ToListAsync();
        var types = await pokeDb.PokeTypes.ToListAsync();
        var byId = moves.ToDictionary(static x => x.MoveId);
        var typeByChinese = types
            .Where(static x => !string.IsNullOrWhiteSpace(x.Name_Chs))
            .GroupBy(static x => NameNormalizer.Normalize(x.Name_Chs))
            .ToDictionary(static x => x.Key, static x => x.First());
        var stats = new ChangeStats("moves");

        foreach (var item in source.Moves)
        {
            if (item.SourceId <= 0)
            {
                continue;
            }

            if (!typeByChinese.TryGetValue(NameNormalizer.Normalize(item.TypeChinese), out var moveType))
            {
                stats.Warn($"Move type not found: {item.English} -> {item.TypeChinese}");
                continue;
            }

            if (!byId.TryGetValue(item.SourceId, out var move))
            {
                move = new Move
                {
                    MoveId = item.SourceId,
                    Name_Chs = item.Chinese,
                    Name_Eng = item.English,
                    Name_Jpn = item.Japanese,
                    description_Chs = item.ChineseDescription,
                    description_Eng = string.Empty,
                    description_Jpn = string.Empty,
                    Pow = item.Power,
                    Acc = item.Accuracy,
                    PP = item.Pp,
                    MoveType = moveType,
                    Damage_Type = item.DamageType,
                };
                pokeDb.Moves.Add(move);
                byId[item.SourceId] = move;
                stats.Added++;
                continue;
            }

            var changed = false;
            changed |= SetText(move.Name_Chs, item.Chinese, value => move.Name_Chs = value);
            changed |= SetText(move.Name_Eng, item.English, value => move.Name_Eng = value);
            changed |= SetText(move.Name_Jpn, item.Japanese, value => move.Name_Jpn = value);
            changed |= SetText(move.description_Chs, item.ChineseDescription, value => move.description_Chs = value);
            changed |= SetNullableInt(move.Pow, item.Power, value => move.Pow = value);
            changed |= SetNullableInt(move.Acc, item.Accuracy, value => move.Acc = value);
            changed |= SetInt(move.PP, item.Pp, value => move.PP = value);
            changed |= SetText(move.Damage_Type, item.DamageType, value => move.Damage_Type = value);

            if (move.MoveType is null || options.Overwrite)
            {
                if (!ReferenceEquals(move.MoveType, moveType))
                {
                    move.MoveType = moveType;
                    changed = true;
                }
            }

            if (changed)
            {
                stats.Updated++;
            }
        }

        await SaveAndPrintAsync(stats);
    }

    private async Task SyncPokemonAsync()
    {
        var pokemons = await pokeDb.Pokemons
            .Include(static x => x.Ability1)
            .Include(static x => x.Ability2)
            .Include(static x => x.AbilityH)
            .Include(static x => x.Type1)
            .Include(static x => x.Type2)
            .ToListAsync();
        await pokeDb.Abilities.LoadAsync();
        await pokeDb.PokeTypes.LoadAsync();
        var abilities = pokeDb.Abilities.Local.ToList();
        var types = pokeDb.PokeTypes.Local.ToList();

        var pokemonByFullEng = pokemons
            .Where(static x => !string.IsNullOrWhiteSpace(x.FullNameEng))
            .GroupBy(static x => NameNormalizer.Normalize(x.FullNameEng))
            .ToDictionary(static x => x.Key, static x => x.First());
        var pokemonByFullChs = pokemons
            .Where(static x => !string.IsNullOrWhiteSpace(x.FullNameChs))
            .GroupBy(static x => NameNormalizer.Normalize(x.FullNameChs))
            .ToDictionary(static x => x.Key, static x => x.First());
        var pokemonByDexForm = pokemons
            .GroupBy(static x => (x.DexId, x.PokeFormId))
            .Where(static x => x.Count() == 1)
            .ToDictionary(static x => x.Key, static x => x.First());

        var abilityById = abilities.ToDictionary(static x => x.AbilityId);
        var abilityByEnglish = abilities
            .Where(static x => !string.IsNullOrWhiteSpace(x.Name_Eng))
            .GroupBy(static x => NameNormalizer.Normalize(x.Name_Eng))
            .ToDictionary(static x => x.Key, static x => x.First());
        var typeByChinese = types
            .Where(static x => !string.IsNullOrWhiteSpace(x.Name_Chs))
            .GroupBy(static x => NameNormalizer.Normalize(x.Name_Chs))
            .ToDictionary(static x => x.Key, static x => x.First());

        var nextId = pokemons.Count == 0 ? 0 : pokemons.Max(static x => x.Id);
        var stats = new ChangeStats("pokemon");

        foreach (var item in source.Pokemons)
        {
            var ability1 = ResolveAbility(item.Ability1Id, abilityById, abilityByEnglish);
            var type1 = ResolveType(item.Type1Id, typeByChinese);

            if (ability1 is null || type1 is null)
            {
                stats.Warn($"Pokemon dependency missing: {item.FullNameEng}");
                continue;
            }

            var ability2 = ResolveAbility(item.Ability2Id, abilityById, abilityByEnglish) ?? ability1;
            var abilityH = ResolveAbility(item.HiddenAbilityId, abilityById, abilityByEnglish) ?? ability1;
            var type2 = ResolveType(item.Type2Id, typeByChinese) ?? type1;

            var pokemon = FindPokemon(item, pokemonByFullEng, pokemonByFullChs, pokemonByDexForm);
            if (pokemon is null)
            {
                pokemon = new Pokemon
                {
                    Id = ++nextId,
                    DexId = item.DexId,
                    PokeFormId = item.FormId,
                    NameChs = item.NameChs,
                    NameEng = item.NameEng,
                    NameJpn = item.NameChs,
                    FormNameChs = item.FormNameChs,
                    FormNameEng = item.FormNameEng,
                    FormNameJpn = item.FormNameChs,
                    FullNameChs = item.FullNameChs,
                    FullNameEng = item.FullNameEng,
                    FullNameJpn = item.FullNameChs,
                    Stage = 0,
                    DMax = item.FullNameEng.EndsWith("-Gmax", StringComparison.OrdinalIgnoreCase),
                    BaseHP = item.Hp,
                    BaseAtk = item.Atk,
                    BaseDef = item.Def,
                    BaseSpa = item.Spa,
                    BaseSpd = item.Spd,
                    BaseSpe = item.Spe,
                    Ability1 = ability1,
                    Ability2 = ability2,
                    AbilityH = abilityH,
                    Type1 = type1,
                    Type2 = type2,
                    GenderRatio = 0,
                    CatchRate = 0,
                    EXPGroup = 0,
                    HatchCycles = 0,
                    Height = 0,
                    Weight = 0,
                    Color = 0,
                };

                pokeDb.Pokemons.Add(pokemon);
                pokemonByFullEng[NameNormalizer.Normalize(pokemon.FullNameEng)] = pokemon;
                pokemonByFullChs[NameNormalizer.Normalize(pokemon.FullNameChs)] = pokemon;
                pokemonByDexForm[(pokemon.DexId, pokemon.PokeFormId)] = pokemon;
                stats.Added++;
                continue;
            }

            var changed = false;
            changed |= SetInt(pokemon.DexId, item.DexId, value => pokemon.DexId = value);
            changed |= SetInt(pokemon.PokeFormId, item.FormId, value => pokemon.PokeFormId = value);
            changed |= SetText(pokemon.NameChs, item.NameChs, value => pokemon.NameChs = value);
            changed |= SetText(pokemon.NameEng, item.NameEng, value => pokemon.NameEng = value);
            changed |= SetText(pokemon.FormNameChs, item.FormNameChs, value => pokemon.FormNameChs = value);
            changed |= SetText(pokemon.FormNameEng, item.FormNameEng, value => pokemon.FormNameEng = value);
            changed |= SetText(pokemon.FullNameChs, item.FullNameChs, value => pokemon.FullNameChs = value);
            changed |= SetText(pokemon.FullNameEng, item.FullNameEng, value => pokemon.FullNameEng = value);
            changed |= SetInt(pokemon.BaseHP, item.Hp, value => pokemon.BaseHP = value);
            changed |= SetInt(pokemon.BaseAtk, item.Atk, value => pokemon.BaseAtk = value);
            changed |= SetInt(pokemon.BaseDef, item.Def, value => pokemon.BaseDef = value);
            changed |= SetInt(pokemon.BaseSpa, item.Spa, value => pokemon.BaseSpa = value);
            changed |= SetInt(pokemon.BaseSpd, item.Spd, value => pokemon.BaseSpd = value);
            changed |= SetInt(pokemon.BaseSpe, item.Spe, value => pokemon.BaseSpe = value);
            changed |= SetBool(pokemon.DMax, item.FullNameEng.EndsWith("-Gmax", StringComparison.OrdinalIgnoreCase), value => pokemon.DMax = value);
            changed |= SetReference(pokemon.Ability1, ability1, value => pokemon.Ability1 = value);
            changed |= SetReference(pokemon.Ability2, ability2, value => pokemon.Ability2 = value);
            changed |= SetReference(pokemon.AbilityH, abilityH, value => pokemon.AbilityH = value);
            changed |= SetReference(pokemon.Type1, type1, value => pokemon.Type1 = value);
            changed |= SetReference(pokemon.Type2, type2, value => pokemon.Type2 = value);

            if (changed)
            {
                stats.Updated++;
            }
        }

        await SaveAndPrintAsync(stats);
    }

    private async Task LinkPsPokemonAsync()
    {
        var pokemons = await pokeDb.Pokemons.ToListAsync();
        var psPokemons = await pokeDb.PSPokemons.Include(static x => x.Pokemon).ToListAsync();

        var byFullEng = pokemons
            .Where(static x => !string.IsNullOrWhiteSpace(x.FullNameEng))
            .GroupBy(static x => NameNormalizer.ShowdownKey(x.FullNameEng))
            .ToDictionary(static x => x.Key, static x => x.First());
        var byNameEng = pokemons
            .Where(static x => !string.IsNullOrWhiteSpace(x.NameEng))
            .GroupBy(static x => NameNormalizer.ShowdownKey(x.NameEng))
            .ToDictionary(static x => x.Key, static x => x.First());
        var byFullChs = pokemons
            .Where(static x => !string.IsNullOrWhiteSpace(x.FullNameChs))
            .GroupBy(static x => NameNormalizer.Normalize(x.FullNameChs))
            .ToDictionary(static x => x.Key, static x => x.First());

        var stats = new ChangeStats("ps links");
        foreach (var psPokemon in psPokemons)
        {
            if (psPokemon.PokemonId is not null && !options.Overwrite)
            {
                continue;
            }

            var englishKey = NameNormalizer.ShowdownKey(psPokemon.PSName);
            var chineseKey = NameNormalizer.Normalize(psPokemon.PSChsName);
            if (!byFullEng.TryGetValue(englishKey, out var pokemon) &&
                !byNameEng.TryGetValue(englishKey, out pokemon) &&
                !byFullChs.TryGetValue(chineseKey, out pokemon))
            {
                stats.Skipped++;
                continue;
            }

            var changed = false;
            if (!ReferenceEquals(psPokemon.Pokemon, pokemon))
            {
                psPokemon.Pokemon = pokemon;
                changed = true;
            }

            changed |= SetText(psPokemon.PSChsName, pokemon.FullNameChs, value => psPokemon.PSChsName = value);

            if (changed)
            {
                stats.Updated++;
            }
        }

        await SaveAndPrintAsync(stats);
    }

    private Pokemon? FindPokemon(
        PokemonSource item,
        IReadOnlyDictionary<string, Pokemon> byFullEng,
        IReadOnlyDictionary<string, Pokemon> byFullChs,
        IReadOnlyDictionary<(int DexId, int FormId), Pokemon> byDexForm)
    {
        if (byFullEng.TryGetValue(NameNormalizer.Normalize(item.FullNameEng), out var pokemon))
        {
            return pokemon;
        }

        if (byFullChs.TryGetValue(NameNormalizer.Normalize(item.FullNameChs), out pokemon))
        {
            return pokemon;
        }

        return byDexForm.GetValueOrDefault((item.DexId, item.FormId));
    }

    private Ability? ResolveAbility(
        int sourceAbilityId,
        IReadOnlyDictionary<int, Ability> abilityById,
        IReadOnlyDictionary<string, Ability> abilityByEnglish)
    {
        if (sourceAbilityId <= 0)
        {
            return null;
        }

        if (abilityById.TryGetValue(sourceAbilityId, out var ability))
        {
            return ability;
        }

        var sourceAbility = source.Abilities.FirstOrDefault(x => x.SourceId == sourceAbilityId);
        return sourceAbility is null
            ? null
            : abilityByEnglish.GetValueOrDefault(NameNormalizer.Normalize(sourceAbility.English));
    }

    private PokeType? ResolveType(
        int sourceTypeId,
        IReadOnlyDictionary<string, PokeType> typeByChinese)
    {
        var typeName = ShitLibDataSource.GetTypeName(sourceTypeId);
        return string.IsNullOrWhiteSpace(typeName)
            ? null
            : typeByChinese.GetValueOrDefault(NameNormalizer.Normalize(typeName));
    }

    private async Task SaveAndPrintAsync(ChangeStats stats)
    {
        if (options.Apply)
        {
            stats.Saved = await pokeDb.SaveChangesAsync();
        }

        stats.Print();
    }

    private bool SetText(string? current, string? sourceValue, Action<string> setter)
    {
        if (string.IsNullOrWhiteSpace(sourceValue))
        {
            return false;
        }

        if (!options.Overwrite && !string.IsNullOrWhiteSpace(current))
        {
            return false;
        }

        if (string.Equals(current, sourceValue, StringComparison.Ordinal))
        {
            return false;
        }

        setter(sourceValue);
        return true;
    }

    private bool SetInt(int current, int sourceValue, Action<int> setter)
    {
        if (sourceValue == 0)
        {
            return false;
        }

        if (!options.Overwrite && current != 0)
        {
            return false;
        }

        if (current == sourceValue)
        {
            return false;
        }

        setter(sourceValue);
        return true;
    }

    private bool SetBool(bool current, bool sourceValue, Action<bool> setter)
    {
        if (!options.Overwrite && current)
        {
            return false;
        }

        if (current == sourceValue)
        {
            return false;
        }

        setter(sourceValue);
        return true;
    }

    private bool SetNullableInt(int? current, int? sourceValue, Action<int?> setter)
    {
        if (sourceValue is null)
        {
            return false;
        }

        if (!options.Overwrite && current is not null)
        {
            return false;
        }

        if (current == sourceValue)
        {
            return false;
        }

        setter(sourceValue);
        return true;
    }

    private bool SetReference<T>(T? current, T sourceValue, Action<T> setter)
        where T : class
    {
        if (!options.Overwrite && current is not null)
        {
            return false;
        }

        if (ReferenceEquals(current, sourceValue))
        {
            return false;
        }

        setter(sourceValue);
        return true;
    }

    private sealed class ChangeStats
    {
        private readonly List<string> warnings = new();

        public ChangeStats(string label)
        {
            Label = label;
        }

        public string Label { get; }

        public int Added { get; set; }

        public int Updated { get; set; }

        public int Skipped { get; set; }

        public int Saved { get; set; }

        public void Warn(string message)
        {
            warnings.Add(message);
        }

        public void Print()
        {
            Console.WriteLine(
                $"{Label}: added={Added}, updated={Updated}, skipped={Skipped}, warnings={warnings.Count}, saved={Saved}");

            foreach (var warning in warnings.Take(20))
            {
                Console.WriteLine($"  warning: {warning}");
            }

            if (warnings.Count > 20)
            {
                Console.WriteLine($"  ... {warnings.Count - 20} more warnings");
            }
        }
    }
}

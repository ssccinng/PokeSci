using System.Globalization;
using System.Text;
using System.Text.Json;

internal static class Program
{
    private static readonly Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

    private static readonly Dictionary<string, int> TypeIdByEnglish = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Fire"] = 1,
        ["Water"] = 2,
        ["Grass"] = 3,
        ["Electric"] = 4,
        ["Normal"] = 5,
        ["Fighting"] = 6,
        ["Flying"] = 7,
        ["Bug"] = 8,
        ["Poison"] = 9,
        ["Rock"] = 10,
        ["Ground"] = 11,
        ["Steel"] = 12,
        ["Ice"] = 13,
        ["Psychic"] = 14,
        ["Dark"] = 15,
        ["Ghost"] = 16,
        ["Dragon"] = 17,
        ["Fairy"] = 18,
        ["Stellar"] = 19,
    };

    private static readonly Dictionary<string, string> TypeChineseByEnglish = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Normal"] = "一般",
        ["Fighting"] = "格斗",
        ["Flying"] = "飞行",
        ["Poison"] = "毒",
        ["Ground"] = "地面",
        ["Rock"] = "岩石",
        ["Bug"] = "虫",
        ["Ghost"] = "幽灵",
        ["Steel"] = "钢",
        ["Fire"] = "火",
        ["Water"] = "水",
        ["Grass"] = "草",
        ["Electric"] = "电",
        ["Psychic"] = "超能力",
        ["Ice"] = "冰",
        ["Dragon"] = "龙",
        ["Dark"] = "恶",
        ["Fairy"] = "妖精",
        ["Stellar"] = "星晶",
    };

    private static readonly Dictionary<string, string> CategoryChineseByEnglish = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Physical"] = "物理",
        ["Special"] = "特殊",
        ["Status"] = "变化",
    };

    private static readonly Dictionary<string, string> PokemonSourceAliases = new()
    {
        [NormalizeKey("Tauros-Paldea-Fire")] = "Tauros-Paldea-Blaze",
        [NormalizeKey("Tauros-Paldea-Water")] = "Tauros-Paldea-Aqua",
        [NormalizeKey("Tauros-Paldea-Combat")] = "Tauros-Paldea",
        [NormalizeKey("Gastrodon")] = "Gastrodon-East",
    };

    private static readonly (int Number, string Chinese, string Japanese, string English, string Description, int Flag1, int Flag2)[]
        AbilityPatches =
        [
            (
                311,
                "贯穿钻",
                "かんつうドリル",
                "Piercing Drill",
                "使用接触类招式时，会无视对手的防守效果，给予原本的1/4的伤害。对手的防守以外的效果会发动。",
                1,
                0),
            (
                312,
                "龙皮肤",
                "ドラゴンスキン",
                "Dragonize",
                "一般属性的招式会变为龙属性，威力会变为1.2倍。",
                1,
                0),
            (
                315,
                "超级日光",
                "メガソーラー",
                "Mega Sol",
                "即使天气不是大晴天状态，也能以大晴天状态使用招式。",
                1,
                0),
            (
                318,
                "辣椒喷发",
                "とびだすハバネロ",
                "Spicy Spray",
                "受到招式的伤害时，会让对手陷入灼伤状态。",
                1,
                0),
        ];

    private static readonly Dictionary<string, string> AbilityChinesePatchByEnglish = AbilityPatches
        .GroupBy(static x => NormalizeKey(x.English))
        .ToDictionary(static x => x.Key, static x => x.First().Chinese, StringComparer.Ordinal);

    private static readonly Dictionary<string, string> PokemonChineseOverrides = new()
    {
        [NormalizeKey("Greninja-Bond")] = "甲贺忍蛙-牵绊",
        [NormalizeKey("Rockruff-Dusk")] = "岩狗狗-黄昏",
    };

    private static readonly Dictionary<string, string> ItemChineseOverrides = new()
    {
        [NormalizeKey("Scovillainite")] = "狠辣椒进化石",
    };

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    private static int Main()
    {
        try
        {
            var builderRoot = Directory.GetCurrentDirectory();
            var oldTxtDataRoot = @"G:\CSharpDev\ssccinng\PokeSci\PokeShitLib\txtdata";
            var pokemonSourcePath = Path.Combine(builderRoot, "pokedex11.json");
            var homeRoot = @"G:\CSharpDev\ssccinng\PokeSci\PokeCommon\Data\Home";
            var outputRoot = Path.Combine(builderRoot, "txtdata");

            Directory.CreateDirectory(outputRoot);

            Console.WriteLine("Loading old txtdata baseline...");
            var oldAbilities = LoadNamedEntries(
                Path.Combine(oldTxtDataRoot, "ability.txt"),
                Path.Combine(oldTxtDataRoot, "abilityengname.txt"));
            var oldItems = LoadNamedEntries(
                Path.Combine(oldTxtDataRoot, "itemname.txt"),
                Path.Combine(oldTxtDataRoot, "itemengname.txt"));
            var oldMoves = LoadMoveRecords(Path.Combine(oldTxtDataRoot, "movedata.txt"));
            var oldPokemon = LoadOldPokemonEntries(
                Path.Combine(oldTxtDataRoot, "pokeengname.txt"),
                Path.Combine(oldTxtDataRoot, "pokedata.txt"));

            Console.WriteLine("Loading new sources...");
            var abilitySources = LoadJsonList<AbilitySource>(Path.Combine(builderRoot, "abilities11.json"));
            var itemSources = LoadJsonList<ItemSource>(Path.Combine(builderRoot, "items11.json"));
            var moveSources = LoadJsonList<MoveSource>(Path.Combine(builderRoot, "moves11.json"));
            var pokemonSources = LoadJsonList<PokemonSource>(pokemonSourcePath);
            var requiredItemNames = CollectRequiredItemNames(pokemonSources);
            var requiredItemOwnerByKey = BuildRequiredItemOwnerMap(pokemonSources);
            var oldPokemonChineseByName = BuildOldPokemonChineseMap(oldPokemon);

            Console.WriteLine("Loading HOME name tables...");
            var abilityHomeEntries = LoadHomeNameEntries(Path.Combine(homeRoot, "tokuseiTable.json"));
            var itemHomeEntries = LoadHomeNameEntries(Path.Combine(homeRoot, "itemTable.json"));
            var moveHomeEntries = LoadHomeNameEntries(Path.Combine(homeRoot, "wazaTable.json"));
            var abilityHomeNames = BuildNameMap(abilityHomeEntries);
            var itemHomeNames = BuildNameMap(itemHomeEntries);
            var moveHomeNames = BuildNameMap(moveHomeEntries);

            Console.WriteLine("Building ability tables...");
            var abilityPatchNames = AbilityPatches.Select(static x => x.English);
            var finalAbilities = BuildFinalNamedEntries(
                oldAbilities,
                abilityHomeEntries
                    .Select(static x => x.English)
                    .Concat(abilitySources.Select(static x => x.Name))
                    .Concat(abilityPatchNames),
                abilityHomeNames,
                ResolveAbilityChineseFallback);
            var abilityIdByName = BuildIdMap(finalAbilities);

            Console.WriteLine("Building item tables...");
            var finalItems = BuildFinalNamedEntries(
                oldItems,
                itemHomeEntries
                    .Select(static x => x.English)
                    .Concat(itemSources.Select(static x => x.Name))
                    .Concat(requiredItemNames),
                itemHomeNames,
                name => ResolveItemChineseFallback(name, oldPokemonChineseByName, requiredItemOwnerByKey));

            Console.WriteLine("Building move tables...");
            var finalMoves = BuildFinalMoves(oldMoves, moveSources, moveHomeNames);

            Console.WriteLine("Writing ability/item/move txtdata...");
            WriteCommaFile(Path.Combine(outputRoot, "ability.txt"), finalAbilities.Select(static x => x.Chinese));
            WriteCommaFile(Path.Combine(outputRoot, "abilityengname.txt"), finalAbilities.Select(static x => x.English));
            WriteCommaFile(Path.Combine(outputRoot, "itemname.txt"), finalItems.Select(static x => x.Chinese));
            WriteCommaFile(Path.Combine(outputRoot, "itemengname.txt"), finalItems.Select(static x => x.English));
            WriteCommaFile(Path.Combine(outputRoot, "movename.txt"), finalMoves.Select(static x => x.Chinese));
            WriteCommaFile(
                Path.Combine(outputRoot, "moveengname.txt"),
                finalMoves.Select(static x => x.English.Replace(",", string.Empty, StringComparison.Ordinal)));
            WriteMovedata(Path.Combine(outputRoot, "movedata.txt"), finalMoves);

            Console.WriteLine("Rebuilding pokeengname/pokedata after ability ids are finalized...");
            var finalPokemon = BuildFinalPokemon(oldPokemon, pokemonSources, abilityIdByName);

            WriteCommaFile(Path.Combine(outputRoot, "pokeengname.txt"), finalPokemon.Select(static x => x.EnglishName));
            WritePokedata(Path.Combine(outputRoot, "pokedata.txt"), finalPokemon.Select(static x => x.Row));

            Console.WriteLine($"Done. Output: {outputRoot}");
            Console.WriteLine(
                $"ability={finalAbilities.Count}, item={finalItems.Count}, move={finalMoves.Count}, pokemon={finalPokemon.Count}");
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            return 1;
        }
    }

    private static List<NamedEntry> LoadNamedEntries(string chinesePath, string englishPath)
    {
        var chinese = ReadCommaValues(chinesePath);
        var english = ReadCommaValues(englishPath);
        var count = Math.Min(chinese.Count, english.Count);
        var entries = new List<NamedEntry>(count);

        for (var i = 0; i < count; i++)
        {
            entries.Add(new NamedEntry(chinese[i], english[i]));
        }

        return entries;
    }

    private static List<string> ReadCommaValues(string path)
    {
        return File
            .ReadAllText(path, Encoding.UTF8)
            .Split(',', StringSplitOptions.None)
            .Select(static x => x.Trim())
            .Where(static x => !string.IsNullOrWhiteSpace(x))
            .ToList();
    }

    private static List<MoveRecord> LoadMoveRecords(string path)
    {
        var result = new List<MoveRecord>();

        foreach (var rawLine in File.ReadLines(path, Encoding.UTF8))
        {
            if (string.IsNullOrWhiteSpace(rawLine))
            {
                continue;
            }

            var cols = rawLine.Split('\t');
            if (cols.Length < 10 || !int.TryParse(cols[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var number))
            {
                continue;
            }

            result.Add(
                new MoveRecord(
                    number,
                    cols[1],
                    cols[2],
                    cols[3],
                    cols[4],
                    cols[5],
                    cols[6],
                    cols[7],
                    cols[8],
                    cols[9]));
        }

        return result;
    }

    private static List<OldPokemonEntry> LoadOldPokemonEntries(string namePath, string dataPath)
    {
        var names = ReadCommaValues(namePath);
        var rows = File
            .ReadAllLines(dataPath, Encoding.UTF8)
            .Where(static x => !string.IsNullOrWhiteSpace(x))
            .ToArray();

        var count = Math.Min(names.Count, rows.Length);
        var result = new List<OldPokemonEntry>(count);

        for (var i = 0; i < count; i++)
        {
            var cols = rows[i].Split(',');
            if (cols.Length < 13)
            {
                continue;
            }

            result.Add(new OldPokemonEntry(names[i], PokemonRow.FromColumns(cols)));
        }

        return result;
    }

    private static List<T> LoadJsonList<T>(string path)
    {
        return JsonSerializer.Deserialize<List<T>>(File.ReadAllText(path, Encoding.UTF8), JsonOptions) ?? new List<T>();
    }

    private static List<NamedEntry> LoadHomeNameEntries(string path)
    {
        var raw = JsonSerializer.Deserialize<Dictionary<string, HomeNameEntry>>(
                      File.ReadAllText(path, Encoding.UTF8),
                      JsonOptions)
                  ?? new Dictionary<string, HomeNameEntry>();

        var result = new List<(int SortKey, NamedEntry Entry)>();
        foreach (var pair in raw)
        {
            var entry = pair.Value;
            if (string.IsNullOrWhiteSpace(entry.Name_Eng) || string.IsNullOrWhiteSpace(entry.Name_Chs))
            {
                continue;
            }

            result.Add((ParseSortKey(pair.Key), new NamedEntry(entry.Name_Chs, entry.Name_Eng)));
        }

        return result
            .OrderBy(static x => x.SortKey)
            .Select(static x => x.Entry)
            .ToList();
    }

    private static Dictionary<string, string> BuildNameMap(IEnumerable<NamedEntry> entries)
    {
        var result = new Dictionary<string, string>();
        foreach (var entry in entries)
        {
            var key = NormalizeKey(entry.English);
            result.TryAdd(key, entry.Chinese);
        }

        return result;
    }

    private static Dictionary<string, string> BuildOldPokemonChineseMap(IReadOnlyList<OldPokemonEntry> oldPokemon)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var entry in oldPokemon)
        {
            if (string.IsNullOrWhiteSpace(entry.EnglishName) || string.IsNullOrWhiteSpace(entry.Row.ChineseName))
            {
                continue;
            }

            result.TryAdd(NormalizeKey(entry.EnglishName), entry.Row.ChineseName);
        }

        return result;
    }

    private static List<string> CollectRequiredItemNames(IReadOnlyList<PokemonSource> pokemonSources)
    {
        var result = new List<string>();
        var seen = new HashSet<string>(StringComparer.Ordinal);

        static void TryAddItem(string? itemName, List<string> target, ISet<string> seenKeys)
        {
            if (string.IsNullOrWhiteSpace(itemName))
            {
                return;
            }

            var trimmed = itemName.Trim();
            var key = NormalizeKey(trimmed);
            if (seenKeys.Add(key))
            {
                target.Add(trimmed);
            }
        }

        foreach (var source in pokemonSources)
        {
            TryAddItem(source.RequiredItem, result, seen);
            if (source.RequiredItems is null || source.RequiredItems.Length == 0)
            {
                continue;
            }

            foreach (var requiredItem in source.RequiredItems)
            {
                TryAddItem(requiredItem, result, seen);
            }
        }

        return result;
    }

    private static Dictionary<string, string> BuildRequiredItemOwnerMap(IReadOnlyList<PokemonSource> pokemonSources)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var source in pokemonSources)
        {
            var owner = !string.IsNullOrWhiteSpace(source.BaseSpecies)
                ? source.BaseSpecies.Trim()
                : TryExtractFormBaseLabel(source.Name);

            if (string.IsNullOrWhiteSpace(owner))
            {
                continue;
            }

            if (!string.IsNullOrWhiteSpace(source.RequiredItem))
            {
                result.TryAdd(NormalizeKey(source.RequiredItem), owner);
            }

            if (source.RequiredItems is null || source.RequiredItems.Length == 0)
            {
                continue;
            }

            foreach (var requiredItem in source.RequiredItems)
            {
                if (!string.IsNullOrWhiteSpace(requiredItem))
                {
                    result.TryAdd(NormalizeKey(requiredItem), owner);
                }
            }
        }

        return result;
    }

    private static List<NamedEntry> BuildFinalNamedEntries(
        IReadOnlyList<NamedEntry> oldEntries,
        IEnumerable<string?> sourceNames,
        IReadOnlyDictionary<string, string> homeChineseByName,
        Func<string, string?> extraChineseResolver)
    {
        var finalEntries = new List<NamedEntry>(oldEntries);
        var seen = new HashSet<string>(oldEntries.Select(static x => NormalizeKey(x.English)));
        var oldChineseByName = oldEntries
            .GroupBy(static x => NormalizeKey(x.English))
            .ToDictionary(static x => x.Key, static x => x.First().Chinese);

        foreach (var rawName in sourceNames)
        {
            if (string.IsNullOrWhiteSpace(rawName))
            {
                continue;
            }

            var name = rawName.Trim();
            var key = NormalizeKey(name);
            if (!seen.Add(key))
            {
                continue;
            }

            var chinese = ResolveChineseName(name, homeChineseByName, oldChineseByName, extraChineseResolver);
            finalEntries.Add(new NamedEntry(chinese, name));
        }

        return finalEntries;
    }

    private static string ResolveChineseName(
        string englishName,
        IReadOnlyDictionary<string, string> homeChineseByName,
        IReadOnlyDictionary<string, string> oldChineseByName,
        Func<string, string?> extraChineseResolver)
    {
        var key = NormalizeKey(englishName);

        if (homeChineseByName.TryGetValue(key, out var chinese))
        {
            return chinese;
        }

        if (oldChineseByName.TryGetValue(key, out chinese))
        {
            return chinese;
        }

        chinese = extraChineseResolver(englishName);
        return string.IsNullOrWhiteSpace(chinese) ? englishName : chinese;
    }

    private static string? ResolveAbilityChineseFallback(string englishName)
    {
        if (AbilityChinesePatchByEnglish.TryGetValue(NormalizeKey(englishName), out var patchedChinese))
        {
            return patchedChinese;
        }

        if (englishName.StartsWith("As One", StringComparison.OrdinalIgnoreCase))
        {
            return "人马一体";
        }

        if (englishName.StartsWith("Embody Aspect", StringComparison.OrdinalIgnoreCase))
        {
            return "面影辉映";
        }

        return null;
    }

    private static string? ResolveItemChineseFallback(
        string englishName,
        IReadOnlyDictionary<string, string> oldPokemonChineseByName,
        IReadOnlyDictionary<string, string> requiredItemOwnerByKey)
    {
        var key = NormalizeKey(englishName);
        if (ItemChineseOverrides.TryGetValue(key, out var patchedChinese))
        {
            return patchedChinese;
        }

        if (!requiredItemOwnerByKey.TryGetValue(key, out var ownerEnglishName))
        {
            return null;
        }

        if (englishName.IndexOf("ite", StringComparison.OrdinalIgnoreCase) < 0)
        {
            return null;
        }

        if (!oldPokemonChineseByName.TryGetValue(NormalizeKey(ownerEnglishName), out var ownerChineseName) ||
            string.IsNullOrWhiteSpace(ownerChineseName))
        {
            return null;
        }

        var iteIndex = englishName.IndexOf("ite", StringComparison.OrdinalIgnoreCase);
        var suffix = iteIndex >= 0 && iteIndex + 3 < englishName.Length
            ? englishName[(iteIndex + 3)..].Trim()
            : string.Empty;

        return string.IsNullOrWhiteSpace(suffix)
            ? $"{ownerChineseName}进化石"
            : $"{ownerChineseName}进化石{suffix}";
    }

    private static Dictionary<string, int> BuildIdMap(IReadOnlyList<NamedEntry> entries)
    {
        var result = new Dictionary<string, int>();
        for (var i = 0; i < entries.Count; i++)
        {
            var key = NormalizeKey(entries[i].English);
            result.TryAdd(key, i + 1);
        }

        return result;
    }

    private static List<MoveRecord> BuildFinalMoves(
        IReadOnlyList<MoveRecord> oldMoves,
        IReadOnlyList<MoveSource> moveSources,
        IReadOnlyDictionary<string, string> moveHomeNames)
    {
        var result = new List<MoveRecord>(oldMoves);
        var seen = new HashSet<string>(oldMoves.Select(static x => NormalizeKey(x.English)));
        var oldByName = oldMoves
            .GroupBy(static x => NormalizeKey(x.English))
            .ToDictionary(static x => x.Key, static x => x.First());

        foreach (var move in moveSources.OrderBy(static x => x.Num).ThenBy(static x => x.Name, StringComparer.Ordinal))
        {
            if (string.IsNullOrWhiteSpace(move.Name))
            {
                continue;
            }

            var key = NormalizeKey(move.Name);
            if (!seen.Add(key))
            {
                continue;
            }

            oldByName.TryGetValue(key, out var oldMove);

            var chinese = moveHomeNames.TryGetValue(key, out var chs)
                ? chs
                : oldMove?.Chinese ?? move.Name;
            var japanese = oldMove?.Japanese ?? chinese;
            var typeChinese = TypeChineseByEnglish.GetValueOrDefault(move.Type, move.Type);
            var categoryChinese = CategoryChineseByEnglish.GetValueOrDefault(move.Category, move.Category);
            var power = move.Category.Equals("Status", StringComparison.OrdinalIgnoreCase) || move.BasePower <= 0
                ? "—"
                : move.BasePower.ToString(CultureInfo.InvariantCulture);
            var accuracy = FormatAccuracy(move.Accuracy);
            var pp = move.Pp > 0 ? move.Pp.ToString(CultureInfo.InvariantCulture) : "—";
            var description = CleanText(move.ShortDesc ?? move.Desc ?? string.Empty);

            result.Add(
                new MoveRecord(
                    move.Num,
                    chinese,
                    japanese,
                    move.Name,
                    typeChinese,
                    categoryChinese,
                    power,
                    accuracy,
                    pp,
                    description));
        }

        return result;
    }

    private static string FormatAccuracy(JsonElement accuracy)
    {
        return accuracy.ValueKind switch
        {
            JsonValueKind.Number => accuracy.GetRawText(),
            JsonValueKind.True => "—",
            JsonValueKind.False => "—",
            JsonValueKind.Null => "—",
            _ => "—",
        };
    }

    private static string CleanText(string value)
    {
        return value
            .Replace("\r", " ", StringComparison.Ordinal)
            .Replace("\n", " ", StringComparison.Ordinal)
            .Replace("\t", " ", StringComparison.Ordinal)
            .Trim();
    }

    private static List<FinalPokemonEntry> BuildFinalPokemon(
        IReadOnlyList<OldPokemonEntry> oldPokemon,
        IReadOnlyList<PokemonSource> pokemonSources,
        IReadOnlyDictionary<string, int> abilityIdByName)
    {
        var sourceMap = BuildPokemonSourceMap(pokemonSources);
        var oldPokemonByKey = oldPokemon
            .GroupBy(static x => NormalizeKey(x.EnglishName))
            .ToDictionary(static x => x.Key, static x => x.First().Row, StringComparer.Ordinal);
        var finalEntries = new List<FinalPokemonEntry>(oldPokemon.Count + pokemonSources.Count);
        var finalKeys = new HashSet<string>(StringComparer.Ordinal);

        foreach (var oldEntry in oldPokemon)
        {
            var key = NormalizeKey(oldEntry.EnglishName);
            PokemonRow row;
            if (sourceMap.TryGetValue(key, out var source))
            {
                row = UpdatePokemonRowFromSource(oldEntry.Row, source, abilityIdByName);
            }
            else
            {
                row = oldEntry.Row;
            }

            row = ApplyPokemonSpecificAbilityOverrides(oldEntry.EnglishName, row, abilityIdByName);
            finalEntries.Add(new FinalPokemonEntry(oldEntry.EnglishName, row));
            finalKeys.Add(key);
        }

        foreach (var source in pokemonSources)
        {
            if (string.IsNullOrWhiteSpace(source.Name))
            {
                continue;
            }

            var sourceKey = NormalizeKey(source.Name);
            if (finalKeys.Contains(sourceKey) || IsCoveredByAlias(sourceKey, finalKeys))
            {
                continue;
            }

            var chineseName = ResolveChineseNameForNewPokemon(source, oldPokemonByKey);
            var row = CreatePokemonRowFromSource(chineseName, source, abilityIdByName);
            row = ApplyPokemonSpecificAbilityOverrides(source.Name, row, abilityIdByName);

            finalEntries.Add(new FinalPokemonEntry(source.Name, row));
            finalKeys.Add(sourceKey);
        }

        return finalEntries;
    }

    private static bool IsCoveredByAlias(string sourceKey, IReadOnlySet<string> existingKeys)
    {
        if (!PokemonSourceAliases.TryGetValue(sourceKey, out var aliasName))
        {
            return false;
        }

        return existingKeys.Contains(NormalizeKey(aliasName));
    }

    private static Dictionary<string, PokemonSource> BuildPokemonSourceMap(IReadOnlyList<PokemonSource> pokemonSources)
    {
        var result = new Dictionary<string, PokemonSource>();

        foreach (var source in pokemonSources)
        {
            if (string.IsNullOrWhiteSpace(source.Name))
            {
                continue;
            }

            var sourceKey = NormalizeKey(source.Name);
            result[sourceKey] = source;

            if (PokemonSourceAliases.TryGetValue(sourceKey, out var aliasName))
            {
                result[NormalizeKey(aliasName)] = source;
            }
        }

        return result;
    }

    private static PokemonRow UpdatePokemonRowFromSource(
        PokemonRow oldRow,
        PokemonSource source,
        IReadOnlyDictionary<string, int> abilityIdByName)
    {
        var types = source.Types ?? Array.Empty<string>();
        var stats = source.BaseStats ?? new PokemonBaseStats();
        var abilities = source.Abilities ?? new Dictionary<string, string>();
        var hasType1 = types.Length >= 1 && !string.IsNullOrWhiteSpace(types[0]);
        var hasType2 = types.Length >= 2 && !string.IsNullOrWhiteSpace(types[1]);

        var type1 = hasType1 && TypeIdByEnglish.TryGetValue(types[0], out var type1Id)
            ? type1Id
            : oldRow.Type1Id;
        var type2 = hasType2 && TypeIdByEnglish.TryGetValue(types[1], out var type2Id)
            ? type2Id
            : (hasType1 ? type1 : oldRow.Type2Id);
        var ability1 = ResolveAbilityId(abilities, "0", oldRow.Ability1Id, abilityIdByName);
        var ability2 = ResolveAbilityId(abilities, "1", oldRow.Ability2Id, abilityIdByName);
        if (HasAbilityValue(abilities, "0") && !HasAbilityValue(abilities, "1"))
        {
            ability2 = ability1;
        }

        return oldRow with
        {
            Type1Id = type1,
            Type2Id = type2,
            Hp = stats.hp > 0 ? stats.hp : oldRow.Hp,
            Atk = stats.atk > 0 ? stats.atk : oldRow.Atk,
            Def = stats.def > 0 ? stats.def : oldRow.Def,
            Spa = stats.spa > 0 ? stats.spa : oldRow.Spa,
            Spd = stats.spd > 0 ? stats.spd : oldRow.Spd,
            Spe = stats.spe > 0 ? stats.spe : oldRow.Spe,
            Ability1Id = ability1,
            Ability2Id = ability2,
            HiddenAbilityId = ResolveAbilityId(abilities, "H", oldRow.HiddenAbilityId, abilityIdByName),
            DexNumber = source.Num > 0 ? source.Num : oldRow.DexNumber,
        };
    }

    private static PokemonRow CreatePokemonRowFromSource(
        string chineseName,
        PokemonSource source,
        IReadOnlyDictionary<string, int> abilityIdByName)
    {
        var types = source.Types ?? Array.Empty<string>();
        var stats = source.BaseStats ?? new PokemonBaseStats();
        var abilities = source.Abilities ?? new Dictionary<string, string>();
        var hasType1 = types.Length >= 1 && !string.IsNullOrWhiteSpace(types[0]);
        var hasType2 = types.Length >= 2 && !string.IsNullOrWhiteSpace(types[1]);

        var type1 = hasType1 && TypeIdByEnglish.TryGetValue(types[0], out var type1Id) ? type1Id : 0;
        var type2 = hasType2 && TypeIdByEnglish.TryGetValue(types[1], out var type2Id)
            ? type2Id
            : (hasType1 ? type1 : 0);
        var ability1 = ResolveAbilityId(abilities, "0", 0, abilityIdByName);
        var ability2 = ResolveAbilityId(abilities, "1", 0, abilityIdByName);
        if (HasAbilityValue(abilities, "0") && !HasAbilityValue(abilities, "1"))
        {
            ability2 = ability1;
        }

        return new PokemonRow(
            chineseName,
            type1,
            type2,
            stats.hp,
            stats.atk,
            stats.def,
            stats.spa,
            stats.spd,
            stats.spe,
            ability1,
            ability2,
            ResolveAbilityId(abilities, "H", 0, abilityIdByName),
            source.Num > 0 ? source.Num : 0);
    }

    private static string ResolveChineseNameForNewPokemon(
        PokemonSource source,
        IReadOnlyDictionary<string, PokemonRow> oldPokemonByKey)
    {
        var key = NormalizeKey(source.Name);
        if (PokemonChineseOverrides.TryGetValue(key, out var overrideChinese))
        {
            return overrideChinese;
        }

        if (TryBuildMegaChineseName(source, oldPokemonByKey, out var megaChinese))
        {
            return megaChinese;
        }

        if (TryResolveBaseChineseName(source, oldPokemonByKey, out var baseChineseName))
        {
            var formLabel = !string.IsNullOrWhiteSpace(source.Forme)
                ? source.Forme
                : TryExtractFormLabel(source.Name, source.BaseSpecies);
            if (!string.IsNullOrWhiteSpace(formLabel))
            {
                return $"{baseChineseName}-{formLabel}";
            }
        }

        return source.Name;
    }

    private static bool TryBuildMegaChineseName(
        PokemonSource source,
        IReadOnlyDictionary<string, PokemonRow> oldPokemonByKey,
        out string chineseName)
    {
        chineseName = string.Empty;
        if (string.IsNullOrWhiteSpace(source.Name) ||
            source.Name.IndexOf("-Mega", StringComparison.OrdinalIgnoreCase) < 0)
        {
            return false;
        }

        var anchorEnglishName = source.Name;
        var suffix = string.Empty;
        if (source.Name.EndsWith("-Mega", StringComparison.OrdinalIgnoreCase))
        {
            anchorEnglishName = source.Name[..^"-Mega".Length];
        }
        else
        {
            var megaIndex = source.Name.IndexOf("-Mega-", StringComparison.OrdinalIgnoreCase);
            if (megaIndex >= 0)
            {
                anchorEnglishName = source.Name[..megaIndex];
                suffix = source.Name[(megaIndex + "-Mega-".Length)..];
            }
        }

        if (!TryResolveChineseByEnglishName(anchorEnglishName, oldPokemonByKey, out var baseChineseName) &&
            !TryResolveBaseChineseName(source, oldPokemonByKey, out baseChineseName))
        {
            return false;
        }

        chineseName = "超级" + baseChineseName + suffix;
        return true;
    }

    private static bool TryResolveBaseChineseName(
        PokemonSource source,
        IReadOnlyDictionary<string, PokemonRow> oldPokemonByKey,
        out string chineseName)
    {
        chineseName = string.Empty;

        if (!string.IsNullOrWhiteSpace(source.BaseSpecies) &&
            TryResolveChineseByEnglishName(source.BaseSpecies, oldPokemonByKey, out chineseName))
        {
            return true;
        }

        var trimmed = TryExtractFormBaseLabel(source.Name);
        return TryResolveChineseByEnglishName(trimmed, oldPokemonByKey, out chineseName);
    }

    private static bool TryResolveChineseByEnglishName(
        string? englishName,
        IReadOnlyDictionary<string, PokemonRow> oldPokemonByKey,
        out string chineseName)
    {
        chineseName = string.Empty;
        if (string.IsNullOrWhiteSpace(englishName))
        {
            return false;
        }

        var key = NormalizeKey(englishName);
        if (!oldPokemonByKey.TryGetValue(key, out var row) || string.IsNullOrWhiteSpace(row.ChineseName))
        {
            return false;
        }

        chineseName = row.ChineseName;
        return true;
    }

    private static string TryExtractFormLabel(string? englishName, string? baseSpecies)
    {
        if (string.IsNullOrWhiteSpace(englishName))
        {
            return string.Empty;
        }

        if (!string.IsNullOrWhiteSpace(baseSpecies) &&
            englishName.StartsWith(baseSpecies + "-", StringComparison.OrdinalIgnoreCase))
        {
            return englishName[(baseSpecies.Length + 1)..];
        }

        var splitIndex = englishName.IndexOf('-', StringComparison.Ordinal);
        return splitIndex >= 0 && splitIndex + 1 < englishName.Length
            ? englishName[(splitIndex + 1)..]
            : string.Empty;
    }

    private static string TryExtractFormBaseLabel(string? englishName)
    {
        if (string.IsNullOrWhiteSpace(englishName))
        {
            return string.Empty;
        }

        var splitIndex = englishName.IndexOf('-', StringComparison.Ordinal);
        return splitIndex > 0 ? englishName[..splitIndex] : englishName;
    }

    private static int ResolveAbilityId(
        IReadOnlyDictionary<string, string> abilities,
        string slot,
        int fallback,
        IReadOnlyDictionary<string, int> abilityIdByName)
    {
        if (!abilities.TryGetValue(slot, out var abilityName) || string.IsNullOrWhiteSpace(abilityName))
        {
            return slot == "1" ? 0 : fallback;
        }

        var key = NormalizeKey(abilityName);
        return abilityIdByName.TryGetValue(key, out var id) ? id : fallback;
    }

    private static bool HasAbilityValue(IReadOnlyDictionary<string, string> abilities, string slot)
    {
        return abilities.TryGetValue(slot, out var abilityName) && !string.IsNullOrWhiteSpace(abilityName);
    }

    private static PokemonRow ApplyPokemonSpecificAbilityOverrides(
        string englishName,
        PokemonRow row,
        IReadOnlyDictionary<string, int> abilityIdByName)
    {
        string? overrideAbilityName = englishName switch
        {
            "Ogerpon-Teal-Tera" => "Embody Aspect (Teal)",
            "Ogerpon-Wellspring-Tera" => "Embody Aspect (Wellspring)",
            "Ogerpon-Hearthflame-Tera" => "Embody Aspect (Hearthflame)",
            "Ogerpon-Cornerstone-Tera" => "Embody Aspect (Cornerstone)",
            _ => null,
        };

        if (overrideAbilityName is null)
        {
            return row;
        }

        var key = NormalizeKey(overrideAbilityName);
        if (!abilityIdByName.TryGetValue(key, out var id))
        {
            return row;
        }

        return row with
        {
            Ability1Id = id,
            Ability2Id = id,
            HiddenAbilityId = id,
        };
    }

    private static void WriteCommaFile(string path, IEnumerable<string> values)
    {
        var content = "," + string.Join(",", values) + ",";
        File.WriteAllText(path, content, Utf8NoBom);
    }

    private static void WriteMovedata(string path, IEnumerable<MoveRecord> moves)
    {
        var lines = moves.Select(
            static x => string.Join(
                '\t',
                x.Number.ToString(CultureInfo.InvariantCulture),
                x.Chinese,
                x.Japanese,
                x.English,
                x.TypeChinese,
                x.CategoryChinese,
                x.Power,
                x.Accuracy,
                x.Pp,
                x.Description));

        File.WriteAllText(path, string.Join(Environment.NewLine, lines), Utf8NoBom);
    }

    private static void WritePokedata(string path, IEnumerable<PokemonRow> rows)
    {
        var lines = rows.Select(static x => x.ToCsv());
        File.WriteAllText(path, string.Join(Environment.NewLine, lines), Utf8NoBom);
    }

    private static string NormalizeKey(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        return value
            .Trim()
            .Replace("’", "'", StringComparison.Ordinal)
            .Replace("‘", "'", StringComparison.Ordinal)
            .Replace("`", "'", StringComparison.Ordinal)
            .Replace("´", "'", StringComparison.Ordinal)
            .Replace("“", "\"", StringComparison.Ordinal)
            .Replace("”", "\"", StringComparison.Ordinal)
            .Replace("–", "-", StringComparison.Ordinal)
            .Replace("—", "-", StringComparison.Ordinal)
            .Replace("－", "-", StringComparison.Ordinal)
            .Replace(",", string.Empty, StringComparison.Ordinal)
            .Replace("♀", "-F", StringComparison.Ordinal)
            .Replace("♂", "-M", StringComparison.Ordinal)
            .ToLowerInvariant();
    }

    private static int ParseSortKey(string rawKey)
    {
        return int.TryParse(rawKey, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed)
            ? parsed
            : int.MaxValue;
    }

    private sealed record NamedEntry(string Chinese, string English);

    private sealed record MoveRecord(
        int Number,
        string Chinese,
        string Japanese,
        string English,
        string TypeChinese,
        string CategoryChinese,
        string Power,
        string Accuracy,
        string Pp,
        string Description);

    private sealed record OldPokemonEntry(string EnglishName, PokemonRow Row);

    private sealed record FinalPokemonEntry(string EnglishName, PokemonRow Row);

    private sealed record PokemonRow(
        string ChineseName,
        int Type1Id,
        int Type2Id,
        int Hp,
        int Atk,
        int Def,
        int Spa,
        int Spd,
        int Spe,
        int Ability1Id,
        int Ability2Id,
        int HiddenAbilityId,
        int DexNumber)
    {
        public static PokemonRow FromColumns(string[] cols)
        {
            return new PokemonRow(
                cols[0],
                ParseInt(cols[1]),
                ParseInt(cols[2]),
                ParseInt(cols[3]),
                ParseInt(cols[4]),
                ParseInt(cols[5]),
                ParseInt(cols[6]),
                ParseInt(cols[7]),
                ParseInt(cols[8]),
                ParseInt(cols[9]),
                ParseInt(cols[10]),
                ParseInt(cols[11]),
                ParseInt(cols[12]));
        }

        public string ToCsv()
        {
            return string.Join(
                ",",
                ChineseName,
                Type1Id.ToString(CultureInfo.InvariantCulture),
                Type2Id.ToString(CultureInfo.InvariantCulture),
                Hp.ToString(CultureInfo.InvariantCulture),
                Atk.ToString(CultureInfo.InvariantCulture),
                Def.ToString(CultureInfo.InvariantCulture),
                Spa.ToString(CultureInfo.InvariantCulture),
                Spd.ToString(CultureInfo.InvariantCulture),
                Spe.ToString(CultureInfo.InvariantCulture),
                Ability1Id.ToString(CultureInfo.InvariantCulture),
                Ability2Id.ToString(CultureInfo.InvariantCulture),
                HiddenAbilityId.ToString(CultureInfo.InvariantCulture),
                DexNumber.ToString(CultureInfo.InvariantCulture));
        }

        private static int ParseInt(string value)
        {
            return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) ? parsed : 0;
        }
    }

    private sealed class HomeNameEntry
    {
        public string Name_Chs { get; set; } = string.Empty;

        public string Name_Eng { get; set; } = string.Empty;
    }

    private sealed class AbilitySource
    {
        public string Name { get; set; } = string.Empty;
    }

    private sealed class ItemSource
    {
        public string Name { get; set; } = string.Empty;
    }

    private sealed class MoveSource
    {
        public int Num { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public int BasePower { get; set; }

        public JsonElement Accuracy { get; set; }

        public int Pp { get; set; }

        public string? Desc { get; set; }

        public string? ShortDesc { get; set; }
    }

    private sealed class PokemonSource
    {
        public int Num { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? BaseSpecies { get; set; }

        public string? Forme { get; set; }

        public string[]? Types { get; set; }

        public PokemonBaseStats? BaseStats { get; set; }

        public Dictionary<string, string>? Abilities { get; set; }

        public string? RequiredItem { get; set; }

        public string[]? RequiredItems { get; set; }
    }

    private sealed class PokemonBaseStats
    {
        public int hp { get; set; }

        public int atk { get; set; }

        public int def { get; set; }

        public int spa { get; set; }

        public int spd { get; set; }

        public int spe { get; set; }
    }
}

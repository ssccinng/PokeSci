using System.Globalization;
using System.Text;

namespace DatabaseTool;

internal sealed class ShitLibDataSource
{
    private static readonly IReadOnlyDictionary<int, string> TypeNamesById = new Dictionary<int, string>
    {
        [1] = "火",
        [2] = "水",
        [3] = "草",
        [4] = "电",
        [5] = "一般",
        [6] = "格斗",
        [7] = "飞行",
        [8] = "虫",
        [9] = "毒",
        [10] = "岩石",
        [11] = "地面",
        [12] = "钢",
        [13] = "冰",
        [14] = "超能力",
        [15] = "恶",
        [16] = "幽灵",
        [17] = "龙",
        [18] = "妖精",
        [19] = "星晶",
    };

    private ShitLibDataSource(
        string dataDirectory,
        IReadOnlyList<LocalizedName> abilities,
        IReadOnlyList<LocalizedName> items,
        IReadOnlyList<MoveSource> moves,
        IReadOnlyList<PokemonSource> pokemons)
    {
        DataDirectory = dataDirectory;
        Abilities = abilities;
        Items = items;
        Moves = moves;
        Pokemons = pokemons;
    }

    public string DataDirectory { get; }

    public IReadOnlyList<LocalizedName> Abilities { get; }

    public IReadOnlyList<LocalizedName> Items { get; }

    public IReadOnlyList<MoveSource> Moves { get; }

    public IReadOnlyList<PokemonSource> Pokemons { get; }

    public static ShitLibDataSource Load(string? configuredPath)
    {
        var dataDirectory = ResolveDataDirectory(configuredPath);
        var abilities = LoadLocalizedNames(
            Path.Combine(dataDirectory, "ability.txt"),
            Path.Combine(dataDirectory, "abilityengname.txt"));
        var items = LoadLocalizedNames(
            Path.Combine(dataDirectory, "itemname.txt"),
            Path.Combine(dataDirectory, "itemengname.txt"));
        var moves = LoadMoves(Path.Combine(dataDirectory, "movedata.txt"));
        var pokemons = LoadPokemons(
            Path.Combine(dataDirectory, "pokedata.txt"),
            Path.Combine(dataDirectory, "pokeengname.txt"));

        return new ShitLibDataSource(dataDirectory, abilities, items, moves, pokemons);
    }

    public static string? GetTypeName(int sourceTypeId)
    {
        return TypeNamesById.GetValueOrDefault(sourceTypeId);
    }

    private static string ResolveDataDirectory(string? configuredPath)
    {
        if (!string.IsNullOrWhiteSpace(configuredPath))
        {
            var fullPath = Path.GetFullPath(configuredPath);
            var txtDataPath = Path.Combine(fullPath, "txtdata");
            if (Directory.Exists(txtDataPath))
            {
                return txtDataPath;
            }

            if (Directory.Exists(fullPath))
            {
                return fullPath;
            }

            throw new DirectoryNotFoundException($"ShitLib data path does not exist: {configuredPath}");
        }

        foreach (var root in EnumerateSearchRoots())
        {
            var candidate = Path.Combine(root, "PokeShitLib", "txtdata");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }

            candidate = Path.Combine(root, "txtdata");
            if (Directory.Exists(candidate))
            {
                return candidate;
            }
        }

        throw new DirectoryNotFoundException("Cannot find PokeShitLib txtdata. Pass --shitlib <path>.");
    }

    private static IEnumerable<string> EnumerateSearchRoots()
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var seed in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
        {
            var current = new DirectoryInfo(seed);
            while (current is not null)
            {
                if (seen.Add(current.FullName))
                {
                    yield return current.FullName;
                }

                current = current.Parent;
            }
        }
    }

    private static IReadOnlyList<LocalizedName> LoadLocalizedNames(string chinesePath, string englishPath)
    {
        var chinese = ReadCommaValues(chinesePath);
        var english = ReadCommaValues(englishPath);
        var count = Math.Min(chinese.Count, english.Count);
        var result = new List<LocalizedName>(count);

        for (var i = 0; i < count; i++)
        {
            var chs = chinese[i].Trim();
            var eng = english[i].Trim();
            if (string.IsNullOrWhiteSpace(eng) || eng == "???")
            {
                continue;
            }

            result.Add(new LocalizedName(i, chs, eng, chs));
        }

        return result;
    }

    private static List<string> ReadCommaValues(string path)
    {
        return File
            .ReadAllText(path, Encoding.UTF8)
            .Split(',', StringSplitOptions.None)
            .Select(static x => x.Trim())
            .ToList();
    }

    private static IReadOnlyList<MoveSource> LoadMoves(string path)
    {
        var result = new List<MoveSource>();

        foreach (var rawLine in File.ReadLines(path, Encoding.UTF8))
        {
            if (string.IsNullOrWhiteSpace(rawLine))
            {
                continue;
            }

            var cols = rawLine.Split('\t');
            if (cols.Length < 10 ||
                !int.TryParse(cols[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
            {
                continue;
            }

            var english = cols[3].Trim();
            if (string.IsNullOrWhiteSpace(english) || english == "???")
            {
                continue;
            }

            result.Add(
                new MoveSource(
                    id,
                    cols[1].Trim(),
                    cols[2].Trim(),
                    english,
                    cols[4].Trim(),
                    cols[5].Trim(),
                    ParseNullableInt(cols[6]),
                    ParseNullableInt(cols[7]),
                    ParseInt(cols[8]),
                    cols[9].Trim()));
        }

        return result;
    }

    private static IReadOnlyList<PokemonSource> LoadPokemons(string dataPath, string englishPath)
    {
        var rows = File
            .ReadAllLines(dataPath, Encoding.UTF8)
            .Where(static x => !string.IsNullOrWhiteSpace(x))
            .ToArray();
        var englishNames = ReadCommaValues(englishPath)
            .Where(static x => !string.IsNullOrWhiteSpace(x))
            .ToArray();

        var rowCount = Math.Min(rows.Length, englishNames.Length);
        var parsedRows = new List<PokemonRawRow>(rowCount);
        var baseChineseByDex = new Dictionary<int, string>();
        var baseEnglishByDex = new Dictionary<int, string>();

        for (var i = 0; i < rowCount; i++)
        {
            var cols = rows[i].Split(',');
            if (cols.Length < 13)
            {
                continue;
            }

            var rawRow = new PokemonRawRow(
                i,
                cols[0].Trim(),
                englishNames[i].Trim(),
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

            parsedRows.Add(rawRow);
            baseChineseByDex.TryAdd(rawRow.DexId, rawRow.FullNameChs);
            baseEnglishByDex.TryAdd(rawRow.DexId, rawRow.FullNameEng);
        }

        var nextFormIdByDex = new Dictionary<int, int>();
        var result = new List<PokemonSource>(parsedRows.Count);
        foreach (var row in parsedRows)
        {
            var formId = nextFormIdByDex.GetValueOrDefault(row.DexId);
            nextFormIdByDex[row.DexId] = formId + 1;

            var baseChinese = baseChineseByDex.GetValueOrDefault(row.DexId, row.FullNameChs);
            var baseEnglish = baseEnglishByDex.GetValueOrDefault(row.DexId, row.FullNameEng);

            result.Add(
                new PokemonSource(
                    row.SourceIndex,
                    row.DexId,
                    formId,
                    baseChinese,
                    baseEnglish,
                    BuildFormName(row.FullNameChs, baseChinese),
                    BuildFormName(row.FullNameEng, baseEnglish),
                    row.FullNameChs,
                    row.FullNameEng,
                    row.Type1Id,
                    row.Type2Id,
                    row.Hp,
                    row.Atk,
                    row.Def,
                    row.Spa,
                    row.Spd,
                    row.Spe,
                    row.Ability1Id,
                    row.Ability2Id,
                    row.HiddenAbilityId));
        }

        return result;
    }

    private static string BuildFormName(string fullName, string baseName)
    {
        if (string.Equals(fullName, baseName, StringComparison.OrdinalIgnoreCase))
        {
            return baseName;
        }

        var prefix = baseName + "-";
        return fullName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)
            ? fullName[prefix.Length..]
            : fullName;
    }

    private static int ParseInt(string value)
    {
        return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) ? parsed : 0;
    }

    private static int? ParseNullableInt(string value)
    {
        return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) ? parsed : null;
    }

    private sealed record PokemonRawRow(
        int SourceIndex,
        string FullNameChs,
        string FullNameEng,
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
        int DexId);
}

internal sealed record LocalizedName(int SourceId, string Chinese, string English, string Japanese);

internal sealed record MoveSource(
    int SourceId,
    string Chinese,
    string Japanese,
    string English,
    string TypeChinese,
    string DamageType,
    int? Power,
    int? Accuracy,
    int Pp,
    string ChineseDescription);

internal sealed record PokemonSource(
    int SourceIndex,
    int DexId,
    int FormId,
    string NameChs,
    string NameEng,
    string FormNameChs,
    string FormNameEng,
    string FullNameChs,
    string FullNameEng,
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
    int HiddenAbilityId);

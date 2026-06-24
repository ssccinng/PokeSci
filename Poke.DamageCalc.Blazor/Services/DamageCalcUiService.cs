using System.Globalization;
using System.Text;
using Poke.DamageCalc;
using Poke.DamageCalc.Data;

namespace Poke.DamageCalc.Blazor.Services;

public sealed class DamageCalcUiService
{
    private static readonly StringComparer TextComparer = StringComparer.OrdinalIgnoreCase;
    private static readonly IReadOnlyDictionary<string, string> TypeChinese = new Dictionary<string, string>(TextComparer)
    {
        ["???"] = "未知",
        ["Normal"] = "一般",
        ["Fire"] = "火",
        ["Water"] = "水",
        ["Electric"] = "电",
        ["Grass"] = "草",
        ["Ice"] = "冰",
        ["Fighting"] = "格斗",
        ["Poison"] = "毒",
        ["Ground"] = "地面",
        ["Flying"] = "飞行",
        ["Psychic"] = "超能力",
        ["Bug"] = "虫",
        ["Rock"] = "岩石",
        ["Ghost"] = "幽灵",
        ["Dragon"] = "龙",
        ["Dark"] = "恶",
        ["Steel"] = "钢",
        ["Fairy"] = "妖精",
        ["Stellar"] = "星晶"
    };

    private static readonly IReadOnlyDictionary<string, string> WeatherChinese = new Dictionary<string, string>(TextComparer)
    {
        ["Sun"] = "晴天",
        ["Harsh Sunshine"] = "大晴天",
        ["Rain"] = "雨天",
        ["Heavy Rain"] = "大雨",
        ["Sand"] = "沙暴",
        ["Hail"] = "冰雹",
        ["Snow"] = "雪天",
        ["Strong Winds"] = "乱流"
    };

    private static readonly IReadOnlyDictionary<string, string> TerrainChinese = new Dictionary<string, string>(TextComparer)
    {
        ["Electric"] = "电气场地",
        ["Grassy"] = "青草场地",
        ["Misty"] = "薄雾场地",
        ["Psychic"] = "精神场地"
    };

    private static readonly IReadOnlyDictionary<string, string> StatusChinese = new Dictionary<string, string>(TextComparer)
    {
        ["brn"] = "灼伤",
        ["Burned"] = "灼伤",
        ["par"] = "麻痹",
        ["Paralyzed"] = "麻痹",
        ["psn"] = "中毒",
        ["Poisoned"] = "中毒",
        ["tox"] = "剧毒",
        ["Badly Poisoned"] = "剧毒",
        ["slp"] = "睡眠",
        ["Asleep"] = "睡眠",
        ["frz"] = "冰冻",
        ["Frozen"] = "冰冻"
    };

    private static readonly IReadOnlyDictionary<string, string> NatureChinese = new Dictionary<string, string>(TextComparer)
    {
        ["Hardy"] = "勤奋",
        ["Lonely"] = "怕寂寞",
        ["Brave"] = "勇敢",
        ["Adamant"] = "固执",
        ["Naughty"] = "顽皮",
        ["Bold"] = "大胆",
        ["Docile"] = "坦率",
        ["Relaxed"] = "悠闲",
        ["Impish"] = "淘气",
        ["Lax"] = "乐天",
        ["Timid"] = "胆小",
        ["Hasty"] = "急躁",
        ["Serious"] = "认真",
        ["Jolly"] = "爽朗",
        ["Naive"] = "天真",
        ["Modest"] = "内敛",
        ["Mild"] = "慢吞吞",
        ["Quiet"] = "冷静",
        ["Bashful"] = "害羞",
        ["Rash"] = "马虎",
        ["Calm"] = "温和",
        ["Gentle"] = "温顺",
        ["Sassy"] = "自大",
        ["Careful"] = "慎重",
        ["Quirky"] = "浮躁"
    };

    private static readonly IReadOnlyDictionary<string, string> FormatChinese = new Dictionary<string, string>(TextComparer)
    {
        ["Other"] = "其他",
        ["OU"] = "OU",
        ["Ubers"] = "Ubers",
        ["UU"] = "UU",
        ["RU"] = "RU",
        ["NU"] = "NU",
        ["PU"] = "PU",
        ["ZU"] = "ZU",
        ["LC"] = "LC",
        ["Doubles OU"] = "双打 OU",
        ["National Dex"] = "全国图鉴",
        ["National Dex Doubles"] = "全国图鉴双打",
        ["Monotype"] = "单属性",
        ["BSS Reg J"] = "BSS 规则 J",
        ["VGC"] = "VGC",
        ["1v1"] = "1v1",
        ["Anything Goes"] = "Anything Goes",
        ["Balanced Hackmons"] = "Balanced Hackmons"
    };

    private static readonly IReadOnlyDictionary<string, int> ItemSearchPriority = new Dictionary<string, int>(TextComparer)
    {
        ["Leftovers"] = 1000,
        ["Heavy-Duty Boots"] = 990,
        ["Choice Specs"] = 980,
        ["Choice Band"] = 970,
        ["Choice Scarf"] = 960,
        ["Life Orb"] = 950,
        ["Assault Vest"] = 940,
        ["Focus Sash"] = 930,
        ["Rocky Helmet"] = 920,
        ["Booster Energy"] = 910,
        ["Eviolite"] = 900,
        ["Sitrus Berry"] = 890,
        ["Black Sludge"] = 880,
        ["Expert Belt"] = 870,
        ["Clear Amulet"] = 860,
        ["Covert Cloak"] = 850,
        ["Loaded Dice"] = 840,
        ["Lum Berry"] = 830,
        ["Weakness Policy"] = 820,
        ["Air Balloon"] = 810
    };

    private readonly IReadOnlyList<SpeciesOption> speciesOptions;
    private readonly IReadOnlyList<MoveOption> moveOptions;
    private readonly IReadOnlyList<string> items;
    private readonly IReadOnlyList<string> abilities;
    private readonly IReadOnlyList<PokemonSetOption> setOptions;
    private readonly IReadOnlyDictionary<string, SpeciesOption> speciesById;
    private readonly IReadOnlyDictionary<string, MoveOption> moveById;
    private readonly IReadOnlyDictionary<string, string> speciesChineseById;
    private readonly IReadOnlyDictionary<string, string> speciesIdByChinese;
    private readonly IReadOnlyDictionary<string, string> formChineseByName;
    private readonly IReadOnlyDictionary<string, string> moveChineseById;
    private readonly IReadOnlyDictionary<string, string> moveIdByChinese;
    private readonly IReadOnlyDictionary<string, string> itemChineseByName;
    private readonly IReadOnlyDictionary<string, string> itemNameByChinese;
    private readonly IReadOnlyDictionary<string, string> abilityChineseByName;
    private readonly IReadOnlyDictionary<string, string> abilityNameByChinese;

    public UiLanguage Language { get; set; } = UiLanguage.Chinese;

    public IReadOnlyList<PokemonSetOption> SetOptions => setOptions;

    public IReadOnlyList<FormatOption> FormatOptions { get; }

    public IReadOnlyList<QuickMatchup> QuickMatchups { get; } =
    [
        new("ou-dragapult-blissey", "OU 眼镜多龙巴鲁托 vs 幸福蛋 / OU Specs Dragapult vs Blissey", "dragapult|ouchoicespecs", "blissey|oudefensive", "singles"),
        new("ou-kingambit-dondozo", "OU 仆刀将军 vs 吃吼霸 / OU Kingambit vs Dondozo", "kingambit|ouswordsdance", "dondozo|ouwall", "singles"),
        new("ou-gholdengo-greattusk", "OU 赛富豪 vs 雄伟牙 / OU Gholdengo vs Great Tusk", "gholdengo|ouchoicescarf", "greattusk|oudefensive", "singles"),
        new("ou-ironvaliant-garganacl", "OU 铁武者 vs 盐石巨灵 / OU Iron Valiant vs Garganacl", "ironvaliant|ouchoicespecs", "garganacl|oustealthrock", "singles"),
        new("ou-greattusk-gliscor", "OU 雄伟牙 vs 天蝎王 / OU Great Tusk vs Gliscor", "greattusk|ouoffensiveutility", "gliscor|ouutility", "singles"),
        new("demo-gengar-blissey", "眼镜耿鬼 vs 幸福蛋 / Specs Gengar vs Blissey", "gengar|nationaldexshowdownusage", "blissey|oudefensive", "singles")
    ];

    public IReadOnlyList<FieldPreset> FieldPresets { get; } =
    [
        new("singles", "单打 / Singles", "无天气 / No weather", new FieldFormModel()),
        new("doubles", "双打 / Doubles", "双打，无天气 / Doubles, no weather", new FieldFormModel { IsDoubles = true }),
        new("vgc50", "VGC Lv50", "双打，等级 50 / Doubles, Lv50", new FieldFormModel { IsDoubles = true }),
        new("sun", "晴天 / Sun", "单打，晴天 / Singles, Sun", new FieldFormModel { Weather = "Sun" }),
        new("rain", "雨天 / Rain", "单打，雨天 / Singles, Rain", new FieldFormModel { Weather = "Rain" }),
        new("snowveil", "雪天 + 极光幕 / Snow + Aurora Veil", "雪天并开启极光幕 / Snow with Aurora Veil", new FieldFormModel { Weather = "Snow", DefenderAuroraVeil = true }),
        new("screens", "反射壁 + 光墙 / Reflect + Light Screen", "双墙防守 / Both defensive screens", new FieldFormModel { DefenderReflect = true, DefenderLightScreen = true }),
        new("doubles-helping-hand", "双打帮助 / Doubles Helping Hand", "双打并使用帮助 / Doubles with Helping Hand", new FieldFormModel { IsDoubles = true, AttackerHelpingHand = true }),
        new("protected", "守住目标 / Protected Target", "目标使用守住 / Target uses Protect", new FieldFormModel { DefenderProtected = true })
    ];

    public DamageCalcUiService()
    {
        speciesOptions = DamageData.AllSpecies;
        moveOptions = DamageData.AllMoves;
        items = DamageData.KnownItems;
        abilities = DamageData.KnownAbilities;
        setOptions = DamageData.ListPokemonSets();
        speciesById = speciesOptions.ToDictionary(option => option.Id, TextComparer);
        moveById = moveOptions.ToDictionary(option => option.Id, TextComparer);
        (speciesChineseById, speciesIdByChinese) = BuildNameMaps(
            speciesOptions.Select(option => (option.Id, option.Name)),
            BilingualNameData.Species);
        formChineseByName = BuildDirectChineseMap(BilingualNameData.Forms);
        (moveChineseById, moveIdByChinese) = BuildNameMaps(
            moveOptions.Select(option => (option.Id, option.Name)),
            BilingualNameData.Moves);
        (itemChineseByName, itemNameByChinese) = BuildNameMaps(
            items.Select(item => (item, item)),
            BilingualNameData.Items);
        (abilityChineseByName, abilityNameByChinese) = BuildNameMaps(
            abilities.Select(ability => (ability, ability)),
            BilingualNameData.Abilities);

        FormatOptions = setOptions
            .Select(set => string.IsNullOrWhiteSpace(set.Format) ? "Other" : set.Format)
            .Distinct(TextComparer)
            .OrderBy(format => format, TextComparer)
            .Select(format => new FormatOption(format, format))
            .ToArray();
    }

    public IReadOnlyList<SpeciesOption> SpeciesOptions => speciesOptions;

    public IReadOnlyList<MoveOption> MoveOptions => moveOptions;

    public IReadOnlyList<string> Types => DamageData.AllTypes;

    public IReadOnlyList<string> Natures => DamageData.AllNatures;

    public IReadOnlyList<string> Items => items;

    public IReadOnlyList<string> Abilities => abilities;

    public IReadOnlyList<string> Weathers => DamageData.AllWeathers;

    public IReadOnlyList<string> Terrains => DamageData.AllTerrains;

    public IReadOnlyList<string> Statuses => DamageData.AllStatuses;

    public string Text(string chinese, string english) =>
        Language == UiLanguage.Chinese ? chinese : english;

    public IReadOnlyList<SearchOption> SpeciesSearchOptions => speciesOptions
        .Select(species =>
        {
            var chinese = ChineseSpeciesName(species.Id, species.Name);
            var label = SpeciesDisplayName(species.Id, species.Name);
            var detail = Language == UiLanguage.Chinese ? species.Name : chinese;
            return new SearchOption(
                species.Id,
                label,
                DetailWithId(detail, species.Id),
                SearchKeywords(species.Id, species.Name, chinese, species.Aliases));
        })
        .ToArray();

    public IReadOnlyList<SearchOption> MoveSearchOptions => moveOptions
        .Select(move =>
        {
            var chinese = ChineseMoveName(move.Id, move.Name);
            var label = MoveDisplayName(move.Id, move.Name);
            var detailName = Language == UiLanguage.Chinese ? move.Name : chinese;
            var detail = string.Join(" · ", new[]
            {
                DetailWithId(detailName, move.Id),
                TypeDisplayName(move.Type),
                CategoryDisplayName(move.Category),
                move.BasePower > 0 ? move.BasePower.ToString(CultureInfo.InvariantCulture) : null
            }.Where(value => !string.IsNullOrWhiteSpace(value)));

            return new SearchOption(
                move.Id,
                label,
                detail,
                SearchKeywords(move.Id, move.Name, chinese, move.Aliases));
        })
        .ToArray();

    public IReadOnlyList<SearchOption> ItemSearchOptions => items
        .OrderByDescending(item => ItemSearchPriority.TryGetValue(item, out var priority) ? priority : 0)
        .ThenBy(item => item, TextComparer)
        .Select(item =>
        {
            var chinese = ChineseItemName(item);
            var label = ItemDisplayName(item);
            var detail = Language == UiLanguage.Chinese ? item : chinese;
            return new SearchOption(item, label, detail, SearchKeywords(item, chinese));
        })
        .ToArray();

    public IReadOnlyList<SearchOption> AbilitySearchOptions => abilities
        .Select(ability =>
        {
            var chinese = ChineseAbilityName(ability);
            var label = AbilityDisplayName(ability);
            var detail = Language == UiLanguage.Chinese ? ability : chinese;
            return new SearchOption(ability, label, detail, SearchKeywords(ability, chinese));
        })
        .ToArray();

    public string SpeciesDisplayName(string speciesIdOrName, string? fallbackEnglish = null)
    {
        var english = ResolveSpeciesDisplayEnglish(speciesIdOrName, fallbackEnglish);
        if (string.IsNullOrWhiteSpace(english))
        {
            return "";
        }

        var chinese = ChineseSpeciesName(speciesIdOrName, english);
        return Localized(chinese, english);
    }

    public string? ChineseSpeciesName(string speciesIdOrName, string? fallbackEnglish = null)
    {
        if (!string.IsNullOrWhiteSpace(fallbackEnglish) &&
            TryGetDirectChineseName(fallbackEnglish, formChineseByName, out var formChinese))
        {
            return formChinese;
        }

        if (TryGetDirectChineseName(speciesIdOrName, formChineseByName, out formChinese))
        {
            return formChinese;
        }

        if (TryGetChineseName(speciesIdOrName, speciesChineseById, speciesIdByChinese, out var chinese))
        {
            return chinese;
        }

        return !string.IsNullOrWhiteSpace(fallbackEnglish) &&
            TryGetChineseName(fallbackEnglish, speciesChineseById, speciesIdByChinese, out chinese)
            ? chinese
            : null;
    }

    public string MoveDisplayName(string moveIdOrName, string? fallbackEnglish = null)
    {
        var english = ResolveMoveDisplayEnglish(moveIdOrName, fallbackEnglish);
        if (string.IsNullOrWhiteSpace(english))
        {
            return "";
        }

        var chinese = ChineseMoveName(moveIdOrName, english);
        return Localized(chinese, english);
    }

    public string? ChineseMoveName(string moveIdOrName, string? fallbackEnglish = null)
    {
        if (TryGetChineseName(moveIdOrName, moveChineseById, moveIdByChinese, out var chinese))
        {
            return chinese;
        }

        return !string.IsNullOrWhiteSpace(fallbackEnglish) &&
            TryGetChineseName(fallbackEnglish, moveChineseById, moveIdByChinese, out chinese)
            ? chinese
            : null;
    }

    public string ItemDisplayName(string? item)
    {
        if (string.IsNullOrWhiteSpace(item))
        {
            return Text("无", "None");
        }

        var english = ResolveByChineseOrSelf(item, itemNameByChinese);
        return Localized(ChineseItemName(english), english);
    }

    public string? ChineseItemName(string? item) =>
        TryGetChineseName(item, itemChineseByName, itemNameByChinese, out var chinese) ? chinese : null;

    public string AbilityDisplayName(string? ability)
    {
        if (string.IsNullOrWhiteSpace(ability))
        {
            return Text("无", "None");
        }

        var english = ResolveByChineseOrSelf(ability, abilityNameByChinese);
        return Localized(ChineseAbilityName(english), english);
    }

    public string? ChineseAbilityName(string? ability) =>
        TryGetChineseName(ability, abilityChineseByName, abilityNameByChinese, out var chinese) ? chinese : null;

    public string TypeDisplayName(string? type)
    {
        if (string.IsNullOrWhiteSpace(type))
        {
            return Text("无", "None");
        }

        return Localized(TypeChinese.TryGetValue(type.Trim(), out var chinese) ? chinese : null, type.Trim());
    }

    public string WeatherDisplayName(string? weather)
    {
        if (string.IsNullOrWhiteSpace(weather))
        {
            return Text("无天气", "No weather");
        }

        return Localized(WeatherChinese.TryGetValue(weather.Trim(), out var chinese) ? chinese : null, weather.Trim());
    }

    public string TerrainDisplayName(string? terrain)
    {
        if (string.IsNullOrWhiteSpace(terrain))
        {
            return Text("无场地", "No terrain");
        }

        return Localized(TerrainChinese.TryGetValue(terrain.Trim(), out var chinese) ? chinese : null, terrain.Trim());
    }

    public string StatusDisplayName(string? status)
    {
        if (string.IsNullOrWhiteSpace(status))
        {
            return Text("健康", "Healthy");
        }

        return Localized(StatusChinese.TryGetValue(status.Trim(), out var chinese) ? chinese : null, status.Trim());
    }

    public string NatureDisplayName(string? nature)
    {
        if (string.IsNullOrWhiteSpace(nature))
        {
            return Text("认真", "Serious");
        }

        return Localized(NatureChinese.TryGetValue(nature.Trim(), out var chinese) ? chinese : null, nature.Trim());
    }

    public string CategoryDisplayName(MoveCategory category) => category switch
    {
        MoveCategory.Physical => Text("物理", "Physical"),
        MoveCategory.Special => Text("特殊", "Special"),
        MoveCategory.Status => Text("变化", "Status"),
        _ => category.ToString()
    };

    public string StatDisplayName(StatId stat) => stat switch
    {
        StatId.Hp => "HP",
        StatId.Atk => Text("攻击", "Atk"),
        StatId.Def => Text("防御", "Def"),
        StatId.Spa => Text("特攻", "SpA"),
        StatId.Spd => Text("特防", "SpD"),
        StatId.Spe => Text("速度", "Spe"),
        _ => stat.ToString()
    };

    public string StatShortName(StatId stat) => stat switch
    {
        StatId.Hp => "HP",
        StatId.Atk => "Atk",
        StatId.Def => "Def",
        StatId.Spa => "SpA",
        StatId.Spd => "SpD",
        StatId.Spe => "Spe",
        _ => stat.ToString()
    };

    public string NatureSuffix(string? nature, StatId stat) =>
        stat == StatId.Hp ? "" : Stats.NatureSuffix(nature ?? "Serious", stat);

    public int EffortTotal(PokemonFormModel form) =>
        form.HpEv + form.AtkEv + form.DefEv + form.SpaEv + form.SpdEv + form.SpeEv;

    public StatLineModel GetStatLine(PokemonFormModel form, StatId stat)
    {
        var baseStat = ResolveBaseStat(form.Species, stat);
        var ev = GetEv(form, stat);
        var iv = GetIv(form, stat);
        var boost = stat == StatId.Hp ? 0 : GetBoost(form, stat);
        int? raw = baseStat is null
            ? null
            : Stats.CalcStat(stat, baseStat.Value, ClampIv(iv), ClampEv(ev), Math.Clamp(form.Level, 1, 100), form.Nature);
        int? modified = raw.HasValue && stat != StatId.Hp
            ? Stats.ModifyByStage(raw.Value, boost)
            : raw;

        return new StatLineModel(stat, baseStat, raw, modified, ev, iv, boost);
    }

    public IReadOnlyList<StatLineModel> GetStatLines(PokemonFormModel form) =>
    [
        GetStatLine(form, StatId.Hp),
        GetStatLine(form, StatId.Atk),
        GetStatLine(form, StatId.Def),
        GetStatLine(form, StatId.Spa),
        GetStatLine(form, StatId.Spd),
        GetStatLine(form, StatId.Spe)
    ];

    public void SetEv(PokemonFormModel form, StatId stat, int value) =>
        SetEvRaw(form, stat, ClampEv(value));

    public void SetIv(PokemonFormModel form, StatId stat, int value) =>
        SetIvRaw(form, stat, ClampIv(value));

    public void SetBoost(PokemonFormModel form, StatId stat, int value)
    {
        if (stat != StatId.Hp)
        {
            SetBoostRaw(form, stat, ClampBoost(value));
        }
    }

    public bool TrySetEvFromActualStat(PokemonFormModel form, StatId stat, int desiredActual)
    {
        var baseStat = ResolveBaseStat(form.Species, stat);
        if (baseStat is null)
        {
            return false;
        }

        var iv = ClampIv(GetIv(form, stat));
        var level = Math.Clamp(form.Level, 1, 100);
        var nature = form.Nature;
        var bestEv = 0;
        var bestDistance = int.MaxValue;
        var foundExact = false;

        for (var ev = 0; ev <= 252; ev += 4)
        {
            var actual = Stats.CalcStat(stat, baseStat.Value, iv, ev, level, nature);
            var distance = Math.Abs(actual - desiredActual);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                bestEv = ev;
                foundExact = distance == 0;
            }

            if (distance == 0)
            {
                break;
            }
        }

        SetEvRaw(form, stat, bestEv);
        return foundExact;
    }

    public string ReverseTargetDisplayName(ReverseTarget target) => target switch
    {
        ReverseTarget.Attacker => Text("攻击方", "Attacker"),
        ReverseTarget.Defender => Text("防守方", "Defender"),
        _ => target.ToString()
    };

    public string DamageMatchModeDisplayName(DamageMatchMode mode) => mode switch
    {
        DamageMatchMode.ExactRange => Text("完全等于范围", "Exact range"),
        DamageMatchMode.ContainsDamage => Text("包含单次伤害", "Contains damage"),
        DamageMatchMode.OverlapsRange => Text("范围有交集", "Overlaps range"),
        _ => mode.ToString()
    };

    public string ReverseNatureModeDisplayName(ReverseNatureMode mode) => mode switch
    {
        ReverseNatureMode.Recommended => Text("推荐性格", "Recommended"),
        ReverseNatureMode.Selected => Text("指定性格", "Selected"),
        ReverseNatureMode.All => Text("全部性格", "All"),
        _ => mode.ToString()
    };

    public string FormatDisplayName(string? format)
    {
        var english = string.IsNullOrWhiteSpace(format) ? "Other" : format.Trim();
        var chinese = FormatChinese.TryGetValue(english, out var found) ? found : null;
        return Localized(chinese, english);
    }

    public string SetDisplayName(PokemonSetSearchResult set) =>
        set.IsBlank ? Text("空白配置", "Blank Set") : set.Name;

    public string BlankSetLabel(string speciesIdOrName, string? fallbackEnglish = null) =>
        $"{SpeciesDisplayName(speciesIdOrName, fallbackEnglish)} ({Text("空白配置", "Blank Set")})";

    public string QuickMatchupDisplayName(QuickMatchup matchup) =>
        LocalizeSlashPair(matchup.Name);

    public string FieldPresetDisplayName(FieldPreset preset) =>
        LocalizeSlashPair(preset.Name);

    public string FieldPresetDescription(FieldPreset preset) =>
        LocalizeSlashPair(preset.Description);

    public string ResolveSpeciesInput(string? value) =>
        ResolveByChineseOrSelf(value, speciesIdByChinese);

    public string ResolveMoveInput(string? value) =>
        ResolveByChineseOrSelf(value, moveIdByChinese);

    public string ResolveItemInput(string? value) =>
        ResolveByChineseOrSelf(value, itemNameByChinese);

    public string ResolveAbilityInput(string? value) =>
        ResolveByChineseOrSelf(value, abilityNameByChinese);

    public IReadOnlyList<PokemonSetOption> SetsForSpecies(string species)
    {
        if (string.IsNullOrWhiteSpace(species))
        {
            return [];
        }

        try
        {
            return DamageData.ListPokemonSets(ResolveSpeciesInput(species));
        }
        catch (KeyNotFoundException)
        {
            return [];
        }
    }

    public IReadOnlyList<PokemonSetSearchResult> SearchPokemonSets(string query, string? format, int limit = 24)
    {
        var trimmedQuery = query?.Trim() ?? "";
        var trimmedFormat = format?.Trim() ?? "";
        var querySpeciesId = TryResolveSpeciesForSelector(trimmedQuery, out var resolvedQuerySpecies)
            ? resolvedQuerySpecies.SpeciesId
            : null;
        limit = Math.Clamp(limit, 1, 80);

        return SetOptions
            .Where(set => MatchesFormat(set, trimmedFormat))
            .Select(set => new { Set = set, Score = ScoreSet(set, trimmedQuery, querySpeciesId) })
            .Where(candidate => candidate.Score >= 0)
            .OrderByDescending(candidate => candidate.Score)
            .ThenBy(candidate => candidate.Set.SpeciesName, TextComparer)
            .ThenBy(candidate => candidate.Set.Name, TextComparer)
            .Take(limit)
            .Select(candidate => PokemonSetSearchResult.From(candidate.Set))
            .ToArray();
    }

    public IReadOnlyList<PokemonSetGroup> SearchPokemonSetGroups(string query, string? currentSpecies, string? format, int speciesLimit = 8, int setsPerSpecies = 8)
    {
        var trimmedQuery = query?.Trim() ?? "";
        var trimmedFormat = format?.Trim() ?? "";
        speciesLimit = Math.Clamp(speciesLimit, 1, 30);
        setsPerSpecies = Math.Clamp(setsPerSpecies, 1, 20);

        var hasCurrentSpecies = TryResolveSpeciesForSelector(currentSpecies, out var current);
        var hasQuerySpecies = TryResolveSpeciesForSelector(trimmedQuery, out var querySpecies);
        var currentSpeciesId = hasCurrentSpecies ? current.SpeciesId : null;
        var querySpeciesId = hasQuerySpecies ? querySpecies.SpeciesId : null;

        if (string.IsNullOrWhiteSpace(trimmedQuery) && hasCurrentSpecies)
        {
            return
            [
                BuildSetGroup(
                    current.SpeciesId,
                    current.SpeciesName,
                    SetOptions
                        .Where(set => string.Equals(set.SpeciesId, current.SpeciesId, StringComparison.OrdinalIgnoreCase))
                        .Where(set => MatchesFormat(set, trimmedFormat)),
                    setsPerSpecies)
            ];
        }

        var setCandidates = SetOptions
            .Where(set => MatchesFormat(set, trimmedFormat))
            .Select(set => new { Set = set, Score = ScoreSetForSelector(set, trimmedQuery, currentSpeciesId, querySpeciesId) })
            .Where(candidate => candidate.Score >= 0)
            .GroupBy(candidate => candidate.Set.SpeciesId, StringComparer.OrdinalIgnoreCase)
            .Select(group => new
            {
                SpeciesId = group.Key,
                SpeciesName = group.First().Set.SpeciesName,
                Score = group.Max(candidate => candidate.Score),
                Sets = group
                    .OrderByDescending(candidate => candidate.Score)
                    .ThenBy(candidate => candidate.Set.Name, TextComparer)
                    .Take(setsPerSpecies)
                    .Select(candidate => PokemonSetSearchResult.From(candidate.Set))
                    .ToArray()
            })
            .OrderByDescending(group => group.Score)
            .ThenBy(group => group.SpeciesName, TextComparer)
            .Take(speciesLimit)
            .ToList();

        if (setCandidates.Count == 0)
        {
            if (hasQuerySpecies)
            {
                return [BuildSetGroup(querySpecies.SpeciesId, querySpecies.SpeciesName, [], setsPerSpecies)];
            }

            if (hasCurrentSpecies)
            {
                return [BuildSetGroup(current.SpeciesId, current.SpeciesName, [], setsPerSpecies)];
            }
        }

        return setCandidates
            .Select(group => new PokemonSetGroup(
                group.SpeciesId,
                group.SpeciesName,
                [PokemonSetSearchResult.Blank(group.SpeciesId, group.SpeciesName), .. group.Sets]))
            .ToArray();
    }

    private static PokemonSetGroup BuildSetGroup(
        string speciesId,
        string speciesName,
        IEnumerable<PokemonSetOption> sets,
        int setsPerSpecies)
    {
        var orderedSets = sets
            .OrderByDescending(set => PreferredFormatScore(set.Format))
            .ThenBy(set => string.IsNullOrWhiteSpace(set.Format) ? "Other" : set.Format, TextComparer)
            .ThenBy(set => set.Name, TextComparer)
            .Take(setsPerSpecies)
            .Select(PokemonSetSearchResult.From)
            .ToArray();

        return new PokemonSetGroup(
            speciesId,
            speciesName,
            [PokemonSetSearchResult.Blank(speciesId, speciesName), .. orderedSets]);
    }

    public bool TryApplyPokemonSet(PokemonFormModel target, string setKey, out string? error, MoveFormModel? move = null)
    {
        error = null;

        try
        {
            var (speciesId, setName) = SplitSetKey(setKey);
            var set = DamageData.GetPokemonSet(speciesId, setName);
            ApplySet(target, set);

            if (move is not null && target.Moves.Length > 0 && !string.IsNullOrWhiteSpace(target.Moves[0].Name))
            {
                move.CopyFrom(target.Moves[0]);
            }

            return true;
        }
        catch (Exception ex) when (ex is ArgumentException or KeyNotFoundException)
        {
            error = ex.Message;
            return false;
        }
    }

    public bool TryApplyFieldPreset(DamageCalcFormModel form, string presetId, out string? error)
    {
        error = null;
        var preset = FieldPresets.FirstOrDefault(p => string.Equals(p.Id, presetId, StringComparison.OrdinalIgnoreCase));
        if (preset is null)
        {
            error = $"Field preset '{presetId}' was not found.";
            return false;
        }

        form.Field = preset.Field.CloneModel();
        if (string.Equals(preset.Id, "vgc50", StringComparison.OrdinalIgnoreCase))
        {
            form.Attacker.Level = 50;
            form.Defender.Level = 50;
        }

        return true;
    }

    public UiDamageResult Calculate(DamageCalcFormModel form)
    {
        try
        {
            var result = CalculateRaw(form);
            return UiDamageResult.Success(result);
        }
        catch (Exception ex) when (ex is ArgumentException or KeyNotFoundException or NotSupportedException)
        {
            return UiDamageResult.Failure(ex.Message);
        }
    }

    public IReadOnlyList<UiMoveDamageResult> CalculateAllMoves(DamageCalcFormModel form)
    {
        var rows = new List<UiMoveDamageResult>();

        try
        {
            var attacker = BuildPokemon(form.Attacker);
            var defender = BuildPokemon(form.Defender);
            var field = BuildField(form.Field);

            AddMoveRows(rows, "Attacker", form.Attacker.Moves, attacker, defender, field);
            AddMoveRows(rows, "Defender", form.Defender.Moves, defender, attacker, FlipSides(field));

            return rows;
        }
        catch (Exception ex) when (ex is ArgumentException or KeyNotFoundException or NotSupportedException)
        {
            return [UiMoveDamageResult.Failure("Setup", 0, "", ex.Message)];
        }
    }

    public UiReverseResult Infer(DamageCalcFormModel form)
    {
        try
        {
            var attacker = BuildPokemon(form.Attacker);
            var defender = BuildPokemon(form.Defender);
            var move = BuildMove(form.Move);
            var field = BuildField(form.Field);
            var reverse = form.Reverse;

            var min = Math.Min(reverse.ObservedMin, reverse.ObservedMax);
            var max = Math.Max(reverse.ObservedMin, reverse.ObservedMax);
            var observed = reverse.ObservedDamage.HasValue
                ? new DamageRange(reverse.ObservedDamage.Value, reverse.ObservedDamage.Value)
                : new DamageRange(min, max);

            var options = new ReverseDamageOptions
            {
                Stat = reverse.Stat,
                MatchMode = reverse.MatchMode,
                Evs = LegalEvs(reverse.EvMin, reverse.EvMax),
                Ivs = Range(reverse.IvMin, reverse.IvMax, 0, 31),
                Boosts = Range(reverse.BoostMin, reverse.BoostMax, -6, 6),
                ObservedDamage = reverse.MatchMode == DamageMatchMode.ContainsDamage ? reverse.ObservedDamage : null,
                Natures = reverse.NatureMode switch
                {
                    ReverseNatureMode.All => DamageData.AllNatures.ToArray(),
                    ReverseNatureMode.Selected when !string.IsNullOrWhiteSpace(reverse.Nature) => [reverse.Nature.Trim()],
                    _ => null
                }
            };

            var candidates = reverse.Target == ReverseTarget.Attacker
                ? ReverseDamageCalculator.InferAttackerSpread(9, observed, attacker, defender, move, field, options)
                : ReverseDamageCalculator.InferDefenderSpread(9, observed, attacker, defender, move, field, options);

            return UiReverseResult.Success(candidates.Take(Math.Clamp(reverse.MaxResults, 1, 1000)).ToArray());
        }
        catch (Exception ex) when (ex is ArgumentException or KeyNotFoundException or NotSupportedException)
        {
            return UiReverseResult.Failure(ex.Message);
        }
    }

    public DamageResult CalculateRaw(DamageCalcFormModel form) =>
        DamageCalculator.Calculate(9, BuildPokemon(form.Attacker), BuildPokemon(form.Defender), BuildMove(form.Move), BuildField(form.Field));

    private void AddMoveRows(
        List<UiMoveDamageResult> rows,
        string source,
        IReadOnlyList<MoveFormModel> moveSlots,
        Pokemon attacker,
        Pokemon defender,
        Field field)
    {
        for (var i = 0; i < moveSlots.Count; i++)
        {
            var slot = moveSlots[i];
            if (slot is null || string.IsNullOrWhiteSpace(slot.Name))
            {
                continue;
            }

            try
            {
                var move = BuildMove(slot);
                var result = DamageCalculator.Calculate(9, attacker, defender, move, field);
                rows.Add(UiMoveDamageResult.Success(source, i + 1, result.Move.Name, result));
            }
            catch (Exception ex) when (ex is ArgumentException or KeyNotFoundException or NotSupportedException)
            {
                rows.Add(UiMoveDamageResult.Failure(source, i + 1, slot.Name.Trim(), ex.Message));
            }
        }
    }

    public Pokemon BuildPokemon(PokemonFormModel form) =>
        new(ResolveSpeciesInput(form.Species), new PokemonOptions
        {
            Level = Math.Clamp(form.Level, 1, 100),
            Ability = EmptyToNull(ResolveAbilityInput(form.Ability)),
            Item = EmptyToNull(ResolveItemInput(form.Item)),
            Nature = EmptyToNull(form.Nature) ?? "Serious",
            TeraType = EmptyToNull(form.TeraType),
            Status = EmptyToNull(form.Status),
            IsDynamaxed = form.IsDynamaxed,
            DynamaxLevel = Math.Clamp(form.DynamaxLevel, 0, 10),
            AlliesFainted = Math.Clamp(form.AlliesFainted, 0, 5),
            CurrentHp = form.CurrentHp is > 0 ? form.CurrentHp : null,
            Ivs = new StatsTable(
                ClampIv(form.HpIv),
                ClampIv(form.AtkIv),
                ClampIv(form.DefIv),
                ClampIv(form.SpaIv),
                ClampIv(form.SpdIv),
                ClampIv(form.SpeIv)),
            Evs = new StatsTable(
                ClampEv(form.HpEv),
                ClampEv(form.AtkEv),
                ClampEv(form.DefEv),
                ClampEv(form.SpaEv),
                ClampEv(form.SpdEv),
                ClampEv(form.SpeEv)),
            Boosts = new StatsTable(
                0,
                ClampBoost(form.AtkBoost),
                ClampBoost(form.DefBoost),
                ClampBoost(form.SpaBoost),
                ClampBoost(form.SpdBoost),
                ClampBoost(form.SpeBoost))
        });

    public Move BuildMove(MoveFormModel form) =>
        new(ResolveMoveInput(form.Name), new MoveOptions
        {
            IsCrit = form.IsCrit,
            IsStellarFirstUse = form.IsStellarFirstUse,
            Hits = form.Hits is > 0 ? Math.Clamp(form.Hits.Value, 1, 10) : null
        });

    public Field BuildField(FieldFormModel form) =>
        new()
        {
            Weather = EmptyToNull(form.Weather),
            Terrain = EmptyToNull(form.Terrain),
            IsDoubles = form.IsDoubles,
            IsMagicRoom = form.IsMagicRoom,
            IsWonderRoom = form.IsWonderRoom,
            AttackerSide = new Side
            {
                IsHelpingHand = form.AttackerHelpingHand
            },
            DefenderSide = new Side
            {
                IsReflect = form.DefenderReflect,
                IsLightScreen = form.DefenderLightScreen,
                IsAuroraVeil = form.DefenderAuroraVeil,
                IsProtected = form.DefenderProtected,
                IsFriendGuard = form.DefenderFriendGuard
            }
        };

    private static Field FlipSides(Field field) => new()
    {
        Weather = field.Weather,
        Terrain = field.Terrain,
        IsDoubles = field.IsDoubles,
        IsMagicRoom = field.IsMagicRoom,
        IsWonderRoom = field.IsWonderRoom,
        AttackerSide = field.DefenderSide.Clone(),
        DefenderSide = field.AttackerSide.Clone()
    };

    private static string? EmptyToNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private int? ResolveBaseStat(string? species, StatId stat)
    {
        if (string.IsNullOrWhiteSpace(species))
        {
            return null;
        }

        try
        {
            var speciesId = DamageData.ResolveSpeciesId(ResolveSpeciesInput(species));
            return DamageData.GetSpecies(speciesId).BaseStats[stat];
        }
        catch (Exception ex) when (ex is ArgumentException or KeyNotFoundException)
        {
            return null;
        }
    }

    private static int GetEv(PokemonFormModel form, StatId stat) => stat switch
    {
        StatId.Hp => form.HpEv,
        StatId.Atk => form.AtkEv,
        StatId.Def => form.DefEv,
        StatId.Spa => form.SpaEv,
        StatId.Spd => form.SpdEv,
        StatId.Spe => form.SpeEv,
        _ => 0
    };

    private static void SetEvRaw(PokemonFormModel form, StatId stat, int value)
    {
        switch (stat)
        {
            case StatId.Hp:
                form.HpEv = value;
                break;
            case StatId.Atk:
                form.AtkEv = value;
                break;
            case StatId.Def:
                form.DefEv = value;
                break;
            case StatId.Spa:
                form.SpaEv = value;
                break;
            case StatId.Spd:
                form.SpdEv = value;
                break;
            case StatId.Spe:
                form.SpeEv = value;
                break;
        }
    }

    private static int GetIv(PokemonFormModel form, StatId stat) => stat switch
    {
        StatId.Hp => form.HpIv,
        StatId.Atk => form.AtkIv,
        StatId.Def => form.DefIv,
        StatId.Spa => form.SpaIv,
        StatId.Spd => form.SpdIv,
        StatId.Spe => form.SpeIv,
        _ => 31
    };

    private static void SetIvRaw(PokemonFormModel form, StatId stat, int value)
    {
        switch (stat)
        {
            case StatId.Hp:
                form.HpIv = value;
                break;
            case StatId.Atk:
                form.AtkIv = value;
                break;
            case StatId.Def:
                form.DefIv = value;
                break;
            case StatId.Spa:
                form.SpaIv = value;
                break;
            case StatId.Spd:
                form.SpdIv = value;
                break;
            case StatId.Spe:
                form.SpeIv = value;
                break;
        }
    }

    private static int GetBoost(PokemonFormModel form, StatId stat) => stat switch
    {
        StatId.Atk => form.AtkBoost,
        StatId.Def => form.DefBoost,
        StatId.Spa => form.SpaBoost,
        StatId.Spd => form.SpdBoost,
        StatId.Spe => form.SpeBoost,
        _ => 0
    };

    private static void SetBoostRaw(PokemonFormModel form, StatId stat, int value)
    {
        switch (stat)
        {
            case StatId.Atk:
                form.AtkBoost = value;
                break;
            case StatId.Def:
                form.DefBoost = value;
                break;
            case StatId.Spa:
                form.SpaBoost = value;
                break;
            case StatId.Spd:
                form.SpdBoost = value;
                break;
            case StatId.Spe:
                form.SpeBoost = value;
                break;
        }
    }

    private static int ClampEv(int value)
    {
        value = Math.Clamp(value, 0, 252);
        return value - value % 4;
    }

    private static int ClampIv(int value) => Math.Clamp(value, 0, 31);

    private static int ClampBoost(int value) => Math.Clamp(value, -6, 6);

    private static int[] LegalEvs(int min, int max)
    {
        min = ClampEv(Math.Min(min, max));
        max = ClampEv(Math.Max(min, max));
        return Enumerable.Range(0, 64)
            .Select(i => i * 4)
            .Where(ev => ev >= min && ev <= max)
            .ToArray();
    }

    private static int[] Range(int min, int max, int floor, int ceiling)
    {
        min = Math.Clamp(Math.Min(min, max), floor, ceiling);
        max = Math.Clamp(Math.Max(min, max), floor, ceiling);
        return Enumerable.Range(min, max - min + 1).ToArray();
    }

    private static bool MatchesFormat(PokemonSetOption set, string format)
    {
        if (string.IsNullOrWhiteSpace(format))
        {
            return true;
        }

        var setFormat = string.IsNullOrWhiteSpace(set.Format) ? "Other" : set.Format;
        return string.Equals(setFormat, format, StringComparison.OrdinalIgnoreCase);
    }

    private int ScoreSet(PokemonSetOption set, string query, string? querySpeciesId = null)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return PreferredFormatScore(set.Format);
        }

        var haystacks = new[]
        {
            set.SpeciesId,
            set.SpeciesName,
            ChineseSpeciesName(set.SpeciesId, set.SpeciesName) ?? "",
            SpeciesDisplayName(set.SpeciesId, set.SpeciesName),
            set.Name,
            set.Format ?? "",
            $"{set.SpeciesName} {set.Name} {set.Format}",
            $"{ChineseSpeciesName(set.SpeciesId, set.SpeciesName)} {set.Name} {set.Format}",
            $"{set.SpeciesId} {set.Name} {set.Format}"
        };

        var normalizedQuery = NormalizeSearch(query);
        var best = -1;

        if (!string.IsNullOrWhiteSpace(querySpeciesId) &&
            string.Equals(set.SpeciesId, querySpeciesId, StringComparison.OrdinalIgnoreCase))
        {
            best = Math.Max(best, 480);
        }

        foreach (var haystack in haystacks)
        {
            var normalizedHaystack = NormalizeSearch(haystack);
            if (normalizedHaystack == normalizedQuery)
            {
                best = Math.Max(best, 500);
            }
            else if (normalizedHaystack.StartsWith(normalizedQuery, StringComparison.OrdinalIgnoreCase))
            {
                best = Math.Max(best, 400);
            }
            else if (normalizedHaystack.Contains(normalizedQuery, StringComparison.OrdinalIgnoreCase))
            {
                best = Math.Max(best, 250);
            }
        }

        if (best < 0)
        {
            return -1;
        }

        return best + PreferredFormatScore(set.Format);
    }

    private int ScoreSetForSelector(PokemonSetOption set, string query, string? currentSpeciesId, string? querySpeciesId)
    {
        var currentSpeciesScore = 0;
        if (!string.IsNullOrWhiteSpace(currentSpeciesId) &&
            string.Equals(currentSpeciesId, set.SpeciesId, StringComparison.OrdinalIgnoreCase))
        {
            currentSpeciesScore = 80;
        }

        if (string.IsNullOrWhiteSpace(query))
        {
            return currentSpeciesScore + PreferredFormatScore(set.Format);
        }

        var baseScore = ScoreSet(set, query, querySpeciesId);
        return baseScore < 0 ? -1 : baseScore + currentSpeciesScore;
    }

    private bool TryResolveSpeciesForSelector(string? value, out (string SpeciesId, string SpeciesName) species)
    {
        species = default;
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        try
        {
            var speciesId = DamageData.ResolveSpeciesId(ResolveSpeciesInput(value));
            species = (speciesId, DamageData.GetSpecies(speciesId).Name);
            return true;
        }
        catch (Exception ex) when (ex is ArgumentException or KeyNotFoundException)
        {
            return false;
        }
    }

    private static int PreferredFormatScore(string? format) =>
        format?.ToUpperInvariant() switch
        {
            "OU" => 70,
            "BSS REG J" => 60,
            "DOUBLES OU" => 55,
            "UBERS" => 45,
            "UU" => 40,
            "RU" => 35,
            "NATIONAL DEX" => 30,
            "NATIONAL DEX DOUBLES" => 25,
            _ => 0
        };

    private static IReadOnlyList<string> SearchKeywords(
        string id,
        string english,
        string? chinese,
        IReadOnlyList<string>? aliases = null)
    {
        var values = new List<string> { id, english };
        if (!string.IsNullOrWhiteSpace(chinese))
        {
            values.Add(chinese);
        }

        if (aliases is not null)
        {
            values.AddRange(aliases.Where(alias => !string.IsNullOrWhiteSpace(alias)));
        }

        return values
            .Distinct(TextComparer)
            .ToArray();
    }

    private static IReadOnlyList<string> SearchKeywords(string english, string? chinese) =>
        SearchKeywords(english, english, chinese);

    private static string? DetailWithId(string? text, string id)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return id;
        }

        return string.Equals(text.Trim(), id, StringComparison.OrdinalIgnoreCase)
            ? id
            : $"{text.Trim()} · {id}";
    }

    private static string NormalizeSearch(string value)
    {
        var builder = new StringBuilder(value.Length);
        foreach (var ch in value)
        {
            if (ch == '♀')
            {
                builder.Append('f');
            }
            else if (ch == '♂')
            {
                builder.Append('m');
            }
            else if (char.IsLetterOrDigit(ch))
            {
                builder.Append(char.ToLowerInvariant(ch));
            }
        }

        return builder.ToString();
    }

    private static (string SpeciesId, string SetName) SplitSetKey(string setKey)
    {
        var parts = setKey.Split('|', 2);
        if (parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
        {
            throw new ArgumentException("Pokemon set key must be in 'speciesId|setName' format.", nameof(setKey));
        }

        return (parts[0], parts[1]);
    }

    private string ResolveSpeciesDisplayEnglish(string speciesIdOrName, string? fallbackEnglish)
    {
        if (string.IsNullOrWhiteSpace(speciesIdOrName))
        {
            return fallbackEnglish ?? "";
        }

        var resolved = ResolveSpeciesInput(speciesIdOrName);
        if (speciesById.TryGetValue(resolved, out var option))
        {
            return option.Name;
        }

        try
        {
            return DamageData.GetSpecies(DamageData.ResolveSpeciesId(resolved)).Name;
        }
        catch (Exception ex) when (ex is ArgumentException or KeyNotFoundException)
        {
            return fallbackEnglish ?? speciesIdOrName.Trim();
        }
    }

    private string ResolveMoveDisplayEnglish(string moveIdOrName, string? fallbackEnglish)
    {
        if (string.IsNullOrWhiteSpace(moveIdOrName))
        {
            return fallbackEnglish ?? "";
        }

        var resolved = ResolveMoveInput(moveIdOrName);
        if (moveById.TryGetValue(resolved, out var option))
        {
            return option.Name;
        }

        try
        {
            return DamageData.GetMove(DamageData.ResolveMoveId(resolved)).Name;
        }
        catch (Exception ex) when (ex is ArgumentException or KeyNotFoundException)
        {
            return fallbackEnglish ?? moveIdOrName.Trim();
        }
    }

    private static (IReadOnlyDictionary<string, string> ChineseById, IReadOnlyDictionary<string, string> IdByChinese) BuildNameMaps(
        IEnumerable<(string Id, string English)> options,
        IEnumerable<LocalizedName> names)
    {
        var localizedByEnglish = new Dictionary<string, string>(TextComparer);
        foreach (var name in names)
        {
            AddIfMissing(localizedByEnglish, NormalizeNameKey(name.English), name.Chinese);
            AddIfMissing(localizedByEnglish, name.English, name.Chinese);
        }

        var chineseById = new Dictionary<string, string>(TextComparer);
        var idByChinese = new Dictionary<string, string>(TextComparer);

        foreach (var option in options)
        {
            var chinese = FindChineseName(option.Id, option.English, localizedByEnglish);
            if (string.IsNullOrWhiteSpace(chinese))
            {
                continue;
            }

            AddIfMissing(chineseById, option.Id, chinese);
            AddIfMissing(chineseById, option.English, chinese);
            AddIfMissing(chineseById, NormalizeNameKey(option.Id), chinese);
            AddIfMissing(chineseById, NormalizeNameKey(option.English), chinese);
            AddIfMissing(idByChinese, chinese, option.Id);
            AddIfMissing(idByChinese, NormalizeNameKey(chinese), option.Id);
        }

        return (chineseById, idByChinese);
    }

    private static IReadOnlyDictionary<string, string> BuildDirectChineseMap(IEnumerable<LocalizedName> names)
    {
        var map = new Dictionary<string, string>(TextComparer);
        foreach (var name in names)
        {
            AddIfMissing(map, name.English, name.Chinese);
            AddIfMissing(map, NormalizeNameKey(name.English), name.Chinese);
        }

        return map;
    }

    private static string? FindChineseName(string id, string english, IReadOnlyDictionary<string, string> localizedByEnglish)
    {
        foreach (var candidate in EnglishNameCandidates(id, english))
        {
            if (localizedByEnglish.TryGetValue(NormalizeNameKey(candidate), out var chinese) ||
                localizedByEnglish.TryGetValue(candidate, out chinese))
            {
                return chinese;
            }
        }

        return null;
    }

    private static IEnumerable<string> EnglishNameCandidates(string id, string english)
    {
        yield return english;
        yield return id;

        var hyphen = english.IndexOf('-', StringComparison.Ordinal);
        if (hyphen > 0)
        {
            var baseName = english[..hyphen];
            var suffix = english[(hyphen + 1)..];
            yield return baseName;
            yield return suffix switch
            {
                "Alola" => $"{baseName} Alola",
                "Galar" => $"{baseName} Galar",
                "Hisui" => $"{baseName} Hisui",
                "Paldea" => $"{baseName} Paldea",
                "Mega" => $"Mega {baseName}",
                "Mega-X" => $"Mega {baseName} X",
                "Mega-Y" => $"Mega {baseName} Y",
                "Gmax" => $"Gigantamax {baseName}",
                _ => $"{suffix} {baseName}"
            };
        }
    }

    private static bool TryGetChineseName(
        string? value,
        IReadOnlyDictionary<string, string> chineseByIdOrName,
        IReadOnlyDictionary<string, string> idOrNameByChinese,
        out string chinese)
    {
        chinese = "";
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var trimmed = value.Trim();
        if (chineseByIdOrName.TryGetValue(trimmed, out var directChinese))
        {
            chinese = directChinese;
            return true;
        }

        if (idOrNameByChinese.TryGetValue(trimmed, out var resolved) &&
            chineseByIdOrName.TryGetValue(resolved, out var resolvedChinese))
        {
            chinese = resolvedChinese;
            return true;
        }

        var normalized = NormalizeNameKey(trimmed);
        if (chineseByIdOrName.TryGetValue(normalized, out var normalizedChinese))
        {
            chinese = normalizedChinese;
            return true;
        }

        return false;
    }

    private static bool TryGetDirectChineseName(
        string? value,
        IReadOnlyDictionary<string, string> chineseByName,
        out string chinese)
    {
        chinese = "";
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var trimmed = value.Trim();
        if (chineseByName.TryGetValue(trimmed, out var directChinese) ||
            chineseByName.TryGetValue(NormalizeNameKey(trimmed), out directChinese))
        {
            chinese = directChinese;
            return true;
        }

        return false;
    }

    private static string ResolveByChineseOrSelf(string? value, IReadOnlyDictionary<string, string> englishByChinese)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "";
        }

        var trimmed = ExtractPrimaryValue(value.Trim());
        if (englishByChinese.TryGetValue(trimmed, out var english))
        {
            return english;
        }

        var normalized = NormalizeNameKey(trimmed);
        return englishByChinese.TryGetValue(normalized, out english) ? english : trimmed;
    }

    private static string ExtractPrimaryValue(string value)
    {
        var separatorIndex = value.IndexOf(" / ", StringComparison.Ordinal);
        return separatorIndex > 0 ? value[..separatorIndex].Trim() : value;
    }

    private string Localized(string? chinese, string english) =>
        Language == UiLanguage.Chinese && !string.IsNullOrWhiteSpace(chinese)
            ? chinese
            : english;

    private string LocalizeSlashPair(string value)
    {
        var parts = value.Split(" / ", 2, StringSplitOptions.None);
        return parts.Length == 2
            ? Text(parts[0], parts[1])
            : value;
    }

    private static string NormalizeNameKey(string value)
    {
        var builder = new StringBuilder(value.Length);
        foreach (var ch in value)
        {
            if (ch == '♀')
            {
                builder.Append('f');
            }
            else if (ch == '♂')
            {
                builder.Append('m');
            }
            else if (char.IsLetterOrDigit(ch))
            {
                builder.Append(char.ToLowerInvariant(ch));
            }
        }

        return builder.ToString();
    }

    private static void AddIfMissing(IDictionary<string, string> dictionary, string key, string value)
    {
        if (!string.IsNullOrWhiteSpace(key) && !dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
    }

    private void ApplySet(PokemonFormModel target, PokemonSetData set)
    {
        target.Species = set.SpeciesId;
        target.Level = Math.Clamp(set.Level ?? target.Level, 1, 100);
        target.Ability = set.Ability;
        target.Item = set.Item;
        target.Nature = set.Nature ?? "Serious";
        target.TeraType = set.TeraType;
        target.Status = "";
        target.CurrentHp = null;

        ApplyStats(target, set.Evs, evs: true);
        ApplyStats(target, set.Ivs, evs: false);
        ApplyMoves(target, set.Moves);
        target.SelectedSetName = set.Name;
    }

    private static void ApplyMoves(PokemonFormModel target, IReadOnlyList<string> moves)
    {
        EnsureMoveSlots(target);
        for (var i = 0; i < target.Moves.Length; i++)
        {
            target.Moves[i].Name = i < moves.Count ? moves[i] : "";
            target.Moves[i].Hits = null;
            target.Moves[i].IsCrit = false;
            target.Moves[i].IsStellarFirstUse = false;
        }
    }

    private static void EnsureMoveSlots(PokemonFormModel target)
    {
        if (target.Moves.Length == 4 && target.Moves.All(slot => slot is not null))
        {
            return;
        }

        var slots = new MoveFormModel[4];
        for (var i = 0; i < slots.Length; i++)
        {
            slots[i] = i < target.Moves.Length && target.Moves[i] is not null ? target.Moves[i] : new MoveFormModel();
        }

        target.Moves = slots;
    }

    private static void ApplyStats(PokemonFormModel target, StatsTable stats, bool evs)
    {
        if (evs)
        {
            target.HpEv = stats.Hp;
            target.AtkEv = stats.Atk;
            target.DefEv = stats.Def;
            target.SpaEv = stats.Spa;
            target.SpdEv = stats.Spd;
            target.SpeEv = stats.Spe;
            return;
        }

        target.HpIv = stats.Hp;
        target.AtkIv = stats.Atk;
        target.DefIv = stats.Def;
        target.SpaIv = stats.Spa;
        target.SpdIv = stats.Spd;
        target.SpeIv = stats.Spe;
    }
}

public sealed class DamageCalcFormModel
{
    public DamageCalcFormModel()
    {
        Attacker.Moves[0].Name = "真气弹";
        Attacker.Moves[1].Name = "暗影球";
        Defender.Moves[0].Name = "Body Slam";
        Move.CopyFrom(Attacker.Moves[0]);
    }

    public PokemonFormModel Attacker { get; set; } = new()
    {
        Species = "耿鬼",
        Ability = "Cursed Body",
        Nature = "Timid",
        SpaEv = 252,
        SpeEv = 252,
        SpaBoost = 0
    };

    public PokemonFormModel Defender { get; set; } = new()
    {
        Species = "卡比兽",
        Ability = "Immunity",
        Nature = "Careful",
        HpEv = 252,
        SpdEv = 252
    };

    public MoveFormModel Move { get; set; } = new()
    {
        Name = "真气弹"
    };

    public FieldFormModel Field { get; set; } = new();

    public ReverseCalcFormModel Reverse { get; set; } = new();
}

public sealed class PokemonFormModel
{
    public string Species { get; set; } = "";

    public string? SelectedSetName { get; set; }

    public int Level { get; set; } = 100;

    public string Nature { get; set; } = "Serious";

    public string? Ability { get; set; }

    public string? Item { get; set; }

    public string? TeraType { get; set; }

    public string? Status { get; set; }

    public int? CurrentHp { get; set; }

    public MoveFormModel[] Moves { get; set; } = [new(), new(), new(), new()];

    public bool IsDynamaxed { get; set; }

    public int DynamaxLevel { get; set; } = 10;

    public int AlliesFainted { get; set; }

    public int HpEv { get; set; }

    public int AtkEv { get; set; }

    public int DefEv { get; set; }

    public int SpaEv { get; set; }

    public int SpdEv { get; set; }

    public int SpeEv { get; set; }

    public int HpIv { get; set; } = 31;

    public int AtkIv { get; set; } = 31;

    public int DefIv { get; set; } = 31;

    public int SpaIv { get; set; } = 31;

    public int SpdIv { get; set; } = 31;

    public int SpeIv { get; set; } = 31;

    public int AtkBoost { get; set; }

    public int DefBoost { get; set; }

    public int SpaBoost { get; set; }

    public int SpdBoost { get; set; }

    public int SpeBoost { get; set; }
}

public sealed class MoveFormModel
{
    public string Name { get; set; } = "";

    public bool IsCrit { get; set; }

    public bool IsStellarFirstUse { get; set; }

    public int? Hits { get; set; }

    public MoveFormModel CloneModel() => new()
    {
        Name = Name,
        IsCrit = IsCrit,
        IsStellarFirstUse = IsStellarFirstUse,
        Hits = Hits
    };

    public void CopyFrom(MoveFormModel source)
    {
        Name = source.Name;
        IsCrit = source.IsCrit;
        IsStellarFirstUse = source.IsStellarFirstUse;
        Hits = source.Hits;
    }
}

public sealed class FieldFormModel
{
    public string? Weather { get; set; }

    public string? Terrain { get; set; }

    public bool IsDoubles { get; set; }

    public bool IsMagicRoom { get; set; }

    public bool IsWonderRoom { get; set; }

    public bool AttackerHelpingHand { get; set; }

    public bool DefenderReflect { get; set; }

    public bool DefenderLightScreen { get; set; }

    public bool DefenderAuroraVeil { get; set; }

    public bool DefenderProtected { get; set; }

    public bool DefenderFriendGuard { get; set; }

    public FieldFormModel CloneModel() => new()
    {
        Weather = Weather,
        Terrain = Terrain,
        IsDoubles = IsDoubles,
        IsMagicRoom = IsMagicRoom,
        IsWonderRoom = IsWonderRoom,
        AttackerHelpingHand = AttackerHelpingHand,
        DefenderReflect = DefenderReflect,
        DefenderLightScreen = DefenderLightScreen,
        DefenderAuroraVeil = DefenderAuroraVeil,
        DefenderProtected = DefenderProtected,
        DefenderFriendGuard = DefenderFriendGuard
    };
}

public sealed record FieldPreset(string Id, string Name, string Description, FieldFormModel Field);

public sealed record FormatOption(string Id, string Name);

public sealed record QuickMatchup(string Id, string Name, string AttackerSetKey, string DefenderSetKey, string? FieldPresetId);

public sealed record StatLineModel(
    StatId Stat,
    int? Base,
    int? Actual,
    int? Modified,
    int Ev,
    int Iv,
    int Boost);

public sealed record SearchOption(
    string Value,
    string Label,
    string? Detail = null,
    IReadOnlyList<string>? Keywords = null)
{
    public IReadOnlyList<string> Keywords { get; } = Keywords ?? [];
}

public sealed record PokemonSetGroup(
    string SpeciesId,
    string SpeciesName,
    IReadOnlyList<PokemonSetSearchResult> Sets);

public sealed record PokemonSetSearchResult(
    string Key,
    string SpeciesId,
    string SpeciesName,
    string Name,
    string Format,
    string Label,
    bool IsBlank = false)
{
    public static PokemonSetSearchResult Blank(string speciesId, string speciesName) =>
        new($"{speciesId}|", speciesId, speciesName, "Blank Set", "Custom", $"{speciesName} - Blank Set [Custom]", true);

    public static PokemonSetSearchResult From(PokemonSetOption set)
    {
        var format = string.IsNullOrWhiteSpace(set.Format) ? "Other" : set.Format;
        return new(
            $"{set.SpeciesId}|{set.Name}",
            set.SpeciesId,
            set.SpeciesName,
            set.Name,
            format,
            $"{set.SpeciesName} - {set.Name} [{format}]");
    }
}

public sealed class ReverseCalcFormModel
{
    public ReverseTarget Target { get; set; } = ReverseTarget.Attacker;

    public DamageMatchMode MatchMode { get; set; } = DamageMatchMode.ExactRange;

    public StatId Stat { get; set; } = StatId.Spa;

    public int ObservedMin { get; set; } = 176;

    public int ObservedMax { get; set; } = 208;

    public int? ObservedDamage { get; set; }

    public int EvMin { get; set; }

    public int EvMax { get; set; } = 252;

    public int IvMin { get; set; } = 31;

    public int IvMax { get; set; } = 31;

    public int BoostMin { get; set; }

    public int BoostMax { get; set; }

    public ReverseNatureMode NatureMode { get; set; } = ReverseNatureMode.Recommended;

    public string Nature { get; set; } = "Timid";

    public int MaxResults { get; set; } = 100;
}

public enum ReverseTarget
{
    Attacker,
    Defender
}

public enum ReverseNatureMode
{
    Recommended,
    Selected,
    All
}

public enum UiLanguage
{
    Chinese,
    English
}

public sealed class UiDamageResult
{
    private UiDamageResult(DamageResult? result, string? error)
    {
        Result = result;
        Error = error;
    }

    public DamageResult? Result { get; }

    public string? Error { get; }

    public bool HasResult => Result is not null;

    public static UiDamageResult Success(DamageResult result) => new(result, null);

    public static UiDamageResult Failure(string error) => new(null, error);

    public string RangeText()
    {
        if (Result is null)
        {
            return "";
        }

        var (min, max) = Result.Range();
        return min == max ? min.ToString(CultureInfo.InvariantCulture) : $"{min}-{max}";
    }

    public string PercentRangeText()
    {
        if (Result is null)
        {
            return "";
        }

        var (min, max) = Result.Range();
        var hp = Math.Max(1, Result.Defender.MaxHp());
        return $"{Percent(min, hp)} - {Percent(max, hp)}";
    }

    public string RollsText() =>
        Result is null ? "" : string.Join(", ", Result.Damage);

    private static string Percent(int value, int max) =>
        (value * 100.0 / max).ToString("0.0", CultureInfo.InvariantCulture) + "%";
}

public sealed class UiMoveDamageResult
{
    private UiMoveDamageResult(string source, int slot, string moveName, DamageResult? result, string? error)
    {
        Source = source;
        Slot = slot;
        MoveName = moveName;
        Result = result;
        Error = error;
    }

    public string Source { get; }

    public int Slot { get; }

    public string MoveName { get; }

    public DamageResult? Result { get; }

    public string? Error { get; }

    public bool HasResult => Result is not null;

    public string SlotLabel => Slot <= 0 ? Source : $"{Source} {Slot}";

    public static UiMoveDamageResult Success(string source, int slot, string moveName, DamageResult result) =>
        new(source, slot, moveName, result, null);

    public static UiMoveDamageResult Failure(string source, int slot, string moveName, string error) =>
        new(source, slot, moveName, null, error);

    public string RangeText()
    {
        if (Result is null)
        {
            return "";
        }

        var (min, max) = Result.Range();
        return min == max ? min.ToString(CultureInfo.InvariantCulture) : $"{min}-{max}";
    }

    public string PercentRangeText()
    {
        if (Result is null)
        {
            return "";
        }

        var (min, max) = Result.Range();
        var hp = Math.Max(1, Result.Defender.MaxHp());
        return $"{Percent(min, hp)} - {Percent(max, hp)}";
    }

    public string KoChanceText() => Result?.KoChance() ?? "";

    public string DescriptionText() => Result?.FullDescription() ?? Error ?? "";

    private static string Percent(int value, int max) =>
        (value * 100.0 / max).ToString("0.0", CultureInfo.InvariantCulture) + "%";
}

public sealed class UiReverseResult
{
    private UiReverseResult(IReadOnlyList<ReverseDamageCandidate> candidates, string? error)
    {
        Candidates = candidates;
        Error = error;
    }

    public IReadOnlyList<ReverseDamageCandidate> Candidates { get; }

    public string? Error { get; }

    public bool HasResult => Error is null;

    public static UiReverseResult Success(IReadOnlyList<ReverseDamageCandidate> candidates) => new(candidates, null);

    public static UiReverseResult Failure(string error) => new([], error);
}

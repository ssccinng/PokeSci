using System.Text;
using PokeCommon.Models;
using PokemonDataAccess.Models;

namespace PokeCommon.PokemonShowdownTools;

internal enum ShowdownTextLanguage
{
    English,
    Chinese
}

internal static class ShowdownFormatSerializer
{
    private const int StatCount = 6;
    private const int DefaultLevel = 50;
    private const int DefaultHappiness = 160;
    private const int DefaultIndividualValue = 31;

    private static readonly string[] EnglishStatNames = { "HP", "Atk", "Def", "SpA", "SpD", "Spe" };
    private static readonly string[] ChineseStatNames = { "HP", "攻击", "防御", "特攻", "特防", "速度" };

    internal static async ValueTask<string> SerializePokemonAsync(
        GamePokemon? gamePokemon,
        Func<int, ValueTask<PSPokemon?>> psPokemonResolver,
        ShowdownTextLanguage language)
    {
        if (gamePokemon?.MetaPokemon == null)
        {
            return string.Empty;
        }

        var sb = new StringBuilder(256);
        var psPokemon = await psPokemonResolver(gamePokemon.MetaPokemon.Id);
        var statNames = language == ShowdownTextLanguage.Chinese ? ChineseStatNames : EnglishStatNames;

        AppendHeader(sb, gamePokemon, psPokemon, language);
        AppendDetails(sb, gamePokemon, language);
        AppendEvSpread(sb, gamePokemon.EVs, statNames, language);
        AppendCommonDetails(sb, gamePokemon, language);
        AppendIvSpread(sb, gamePokemon.IVs, statNames, language);
        AppendMoves(sb, gamePokemon, language);

        return sb.ToString();
    }

    internal static async ValueTask<string> SerializeTeamAsync(
        GamePokemonTeam? gamePokemonTeam,
        Func<GamePokemon?, ValueTask<string>> pokemonSerializer)
    {
        if (gamePokemonTeam?.GamePokemons == null || gamePokemonTeam.GamePokemons.Count == 0)
        {
            return string.Empty;
        }

        var sb = new StringBuilder(gamePokemonTeam.GamePokemons.Count * 256);
        var wrotePokemon = false;
        foreach (var gamePokemon in gamePokemonTeam.GamePokemons)
        {
            var serializedPokemon = await pokemonSerializer(gamePokemon);
            if (wrotePokemon)
            {
                sb.AppendLine();
                sb.AppendLine();
            }

            sb.Append(serializedPokemon.TrimEnd('\r', '\n'));
            wrotePokemon = true;
        }

        return sb.ToString().Trim();
    }

    internal static async ValueTask<string> SerializePackedPokemonAsync(
        GamePokemon? gamePokemon,
        Func<int, ValueTask<PSPokemon?>> psPokemonResolver)
    {
        if (gamePokemon?.MetaPokemon == null)
        {
            return string.Empty;
        }

        var data = new string[12];
        Array.Fill(data, string.Empty);

        var psPokemon = await psPokemonResolver(gamePokemon.MetaPokemon.Id);
        var psName = NormalizeShowdownId(psPokemon?.PSName);
        if (!string.IsNullOrEmpty(gamePokemon.NickName))
        {
            data[0] = gamePokemon.NickName;
            data[1] = psName;
        }
        else
        {
            data[0] = psName;
        }

        if (gamePokemon.Item != null)
        {
            data[2] = NormalizeShowdownId(gamePokemon.Item.Name_Eng);
        }

        if (gamePokemon.Ability != null)
        {
            data[3] = NormalizeShowdownId(gamePokemon.Ability.Name_Eng);
        }

        if (gamePokemon.Moves is { Count: > 0 })
        {
            data[4] = JoinPackedMoves(gamePokemon.Moves);
        }

        if (gamePokemon.Nature != null)
        {
            data[5] = gamePokemon.Nature.Name_Eng;
        }

        if (gamePokemon.EVs.Sum > 0)
        {
            data[6] = BuildPackedStats(gamePokemon.EVs, static value => value > 0);
        }

        data[7] = gamePokemon.Gender switch
        {
            Gender.Female => "F",
            Gender.Male => "M",
            _ => string.Empty,
        };

        if (gamePokemon.IVs.Sum < DefaultIndividualValue * StatCount)
        {
            data[8] = BuildPackedStats(gamePokemon.IVs, static value => value < DefaultIndividualValue);
        }

        if (gamePokemon.Shiny)
        {
            data[9] = "S";
        }

        if (gamePokemon.LV != DefaultLevel)
        {
            data[10] = gamePokemon.LV.ToString();
        }

        if (gamePokemon.TreaType != null)
        {
            data[11] = $",,,,,{gamePokemon.TreaType.Name_Eng}";
        }

        return string.Join("|", data);
    }

    internal static async ValueTask<string> SerializePackedTeamAsync(
        GamePokemonTeam? gamePokemonTeam,
        Func<GamePokemon?, ValueTask<string>> pokemonSerializer)
    {
        if (gamePokemonTeam?.GamePokemons == null || gamePokemonTeam.GamePokemons.Count == 0)
        {
            return string.Empty;
        }

        var sb = new StringBuilder(gamePokemonTeam.GamePokemons.Count * 128);
        for (var i = 0; i < gamePokemonTeam.GamePokemons.Count; i++)
        {
            if (i > 0)
            {
                sb.Append(']');
            }

            sb.Append(await pokemonSerializer(gamePokemonTeam.GamePokemons[i]));
        }

        return sb.ToString();
    }

    internal static string NormalizeShowdownId(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        var length = 0;
        foreach (var c in value)
        {
            if (c is not (' ' or '-'))
            {
                length++;
            }
        }

        return string.Create(length, value, static (destination, source) =>
        {
            var index = 0;
            foreach (var c in source)
            {
                if (c is ' ' or '-')
                {
                    continue;
                }

                destination[index++] = char.ToLowerInvariant(c);
            }
        });
    }

    private static void AppendHeader(
        StringBuilder sb,
        GamePokemon gamePokemon,
        PSPokemon? psPokemon,
        ShowdownTextLanguage language)
    {
        if (language == ShowdownTextLanguage.Chinese)
        {
            sb.Append(psPokemon?.PSChsName);
            if (gamePokemon.Gmax)
            {
                sb.Append("-超极巨");
            }

            if (gamePokemon.Item != null)
            {
                sb.Append(" @ ").Append(gamePokemon.Item.Name_Chs);
            }
        }
        else
        {
            sb.Append(psPokemon?.PSName);
            if (gamePokemon.Gmax)
            {
                sb.Append("-Gmax");
            }

            if (gamePokemon.Item != null)
            {
                sb.Append(" @ ").Append(gamePokemon.Item.Name_Eng);
            }
        }

        sb.AppendLine();
    }

    private static void AppendDetails(StringBuilder sb, GamePokemon gamePokemon, ShowdownTextLanguage language)
    {
        if (language == ShowdownTextLanguage.Chinese)
        {
            if (gamePokemon.Ability != null)
            {
                sb.Append("特性: ").AppendLine(gamePokemon.Ability.Name_Chs);
            }

            sb.Append("等级: ").Append(gamePokemon.LV).AppendLine();
            if (gamePokemon.TreaType != null)
            {
                sb.Append("太晶属性: ").AppendLine(gamePokemon.TreaType.Name_Chs);
            }

            if (gamePokemon.Happiness != DefaultHappiness)
            {
                sb.Append("亲密度: ").Append(gamePokemon.Happiness).AppendLine();
            }
        }
        else
        {
            if (gamePokemon.Ability != null)
            {
                sb.Append("Ability: ").AppendLine(gamePokemon.Ability.Name_Eng);
            }

            sb.Append("Level: ").Append(gamePokemon.LV).AppendLine();
            if (gamePokemon.TreaType != null)
            {
                sb.Append("Tera Type: ").AppendLine(gamePokemon.TreaType.Name_Eng);
            }

            if (gamePokemon.Happiness != DefaultHappiness)
            {
                sb.Append("Happiness: ").Append(gamePokemon.Happiness).AppendLine();
            }
        }
    }

    private static void AppendCommonDetails(StringBuilder sb, GamePokemon gamePokemon, ShowdownTextLanguage language)
    {
        if (gamePokemon.Shiny)
        {
            sb.AppendLine(language == ShowdownTextLanguage.Chinese ? "闪光: Yes" : "Shiny: Yes");
        }

        if (gamePokemon.Nature != null)
        {
            if (language == ShowdownTextLanguage.Chinese)
            {
                sb.Append(gamePokemon.Nature.Name_Chs).AppendLine(" 性格");
            }
            else
            {
                sb.Append(gamePokemon.Nature.Name_Eng).AppendLine(" Nature");
            }
        }
    }

    private static void AppendEvSpread(
        StringBuilder sb,
        SixDimension values,
        IReadOnlyList<string> statNames,
        ShowdownTextLanguage language)
    {
        var wroteHeader = false;
        for (var i = 0; i < StatCount; i++)
        {
            var value = values[i];
            if (value <= 0)
            {
                continue;
            }

            AppendStatPrefix(sb, ref wroteHeader, language == ShowdownTextLanguage.Chinese ? "努力值: " : "EVs: ");
            sb.Append(value).Append(' ').Append(statNames[i]);
        }

        if (wroteHeader)
        {
            sb.AppendLine();
        }
    }

    private static void AppendIvSpread(
        StringBuilder sb,
        SixDimension values,
        IReadOnlyList<string> statNames,
        ShowdownTextLanguage language)
    {
        var wroteHeader = false;
        for (var i = 0; i < StatCount; i++)
        {
            var value = values[i];
            if (value == DefaultIndividualValue)
            {
                continue;
            }

            AppendStatPrefix(sb, ref wroteHeader, language == ShowdownTextLanguage.Chinese ? "个体值: " : "IVs: ");
            sb.Append(value).Append(' ').Append(statNames[i]);
        }

        if (wroteHeader)
        {
            sb.AppendLine();
        }
    }

    private static void AppendStatPrefix(StringBuilder sb, ref bool wroteHeader, string label)
    {
        if (wroteHeader)
        {
            sb.Append(" / ");
        }
        else
        {
            sb.Append(label);
            wroteHeader = true;
        }
    }

    private static void AppendMoves(StringBuilder sb, GamePokemon gamePokemon, ShowdownTextLanguage language)
    {
        if (gamePokemon.Moves == null)
        {
            return;
        }

        foreach (var move in gamePokemon.Moves)
        {
            if (move == null || move.NameChs.StartsWith("觉醒力量", StringComparison.Ordinal))
            {
                continue;
            }

            sb.Append("- ").AppendLine(language == ShowdownTextLanguage.Chinese ? move.NameChs : move.NameEng);
        }
    }

    private static string JoinPackedMoves(IEnumerable<GameMove?> moves)
    {
        var sb = new StringBuilder(64);
        var wroteMove = false;
        foreach (var move in moves)
        {
            if (move == null)
            {
                continue;
            }

            if (wroteMove)
            {
                sb.Append(',');
            }

            sb.Append(NormalizeShowdownId(move.NameEng));
            wroteMove = true;
        }

        return sb.ToString();
    }

    private static string BuildPackedStats(SixDimension values, Func<int, bool> shouldWrite)
    {
        var sb = new StringBuilder(20);
        for (var i = 0; i < StatCount; i++)
        {
            if (i > 0)
            {
                sb.Append(',');
            }

            var value = values[i];
            if (shouldWrite(value))
            {
                sb.Append(value);
            }
        }

        return sb.ToString();
    }
}

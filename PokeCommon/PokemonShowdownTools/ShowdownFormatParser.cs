using System.Text;
using PokeCommon.Models;
using PokemonDataAccess.Models;

namespace PokeCommon.PokemonShowdownTools;

internal sealed record ShowdownDataResolver(
    Func<string, ValueTask<Pokemon?>> GetPokemonFromPsNameAsync,
    Func<string, ValueTask<Item?>> GetItemAsync,
    Func<string, ValueTask<Ability?>> GetAbilityAsync,
    Func<string, ValueTask<PokeType?>> GetTypeAsync,
    Func<string, ValueTask<Move?>> GetMoveAsync,
    Func<string, ValueTask<Nature?>> GetNatureByNameAsync,
    Func<int, ValueTask<Nature?>> GetNatureByIdAsync);

internal static class ShowdownFormatParser
{
    private const int DefaultLevel = 50;
    private const int PackedFieldCount = 12;

    internal static async Task<GamePokemon?> ParsePokemonAsync(string? psText, ShowdownDataResolver resolver)
    {
        if (string.IsNullOrWhiteSpace(psText))
        {
            return new GamePokemon();
        }

        var lines = SplitLines(psText);
        if (lines.Length == 0)
        {
            return new GamePokemon();
        }

        var (pokemonName, nickName, itemName) = ParseNameAndItem(lines[0]);
        var gmax = false;
        if (pokemonName.EndsWith("-Gmax", StringComparison.Ordinal))
        {
            pokemonName = pokemonName[..^5];
            gmax = true;
        }

        var pokemon = await resolver.GetPokemonFromPsNameAsync(pokemonName);
        if (pokemon == null)
        {
            return new GamePokemon();
        }

        var gamePokemon = new GamePokemon(pokemon)
        {
            Gmax = gmax,
            TreaType = pokemon.Type1,
            NickName = nickName
        };

        if (!string.IsNullOrEmpty(itemName))
        {
            gamePokemon.Item = await resolver.GetItemAsync(itemName);
        }

        for (var i = 1; i < lines.Length; i++)
        {
            await ParsePokemonLineAsync(lines[i], gamePokemon, resolver);
        }

        return gamePokemon;
    }

    internal static async Task<GamePokemonTeam> ParseTeamAsync(string? psText, int pad, ShowdownDataResolver resolver)
    {
        var team = new GamePokemonTeam();
        if (!string.IsNullOrWhiteSpace(psText))
        {
            foreach (var pokemonText in SplitTeamText(psText))
            {
                team.GamePokemons.Add(await ParsePokemonAsync(pokemonText, resolver) ?? new GamePokemon());
            }
        }

        while (team.GamePokemons.Count < pad)
        {
            team.GamePokemons.Add(new GamePokemon());
        }

        return team;
    }

    internal static async ValueTask<GamePokemon> ParsePackedPokemonAsync(
        string? oneLine,
        ShowdownDataResolver resolver,
        Func<string, string>? normalizeAbilityName = null)
    {
        if (string.IsNullOrEmpty(oneLine))
        {
            return new GamePokemon();
        }

        var data = NormalizePackedFields(oneLine);
        var gamePokemon = new GamePokemon();
        var pokemonName = string.IsNullOrEmpty(data[1]) ? data[0] : data[1];

        if (!string.IsNullOrEmpty(pokemonName))
        {
            var pokemon = await resolver.GetPokemonFromPsNameAsync(pokemonName);
            if (pokemon != null)
            {
                gamePokemon = new GamePokemon(pokemon)
                {
                    TreaType = pokemon.Type1
                };
            }
        }

        if (!string.IsNullOrEmpty(data[1]))
        {
            gamePokemon.NickName = data[0];
        }

        if (!string.IsNullOrEmpty(data[2]))
        {
            gamePokemon.Item = await resolver.GetItemAsync(data[2]);
        }

        if (!string.IsNullOrEmpty(data[3]))
        {
            var abilityName = normalizeAbilityName?.Invoke(data[3]) ?? data[3];
            gamePokemon.Ability = await resolver.GetAbilityAsync(abilityName);
        }

        if (!string.IsNullOrEmpty(data[4]))
        {
            gamePokemon.Moves.Clear();
            foreach (var moveName in data[4].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                var move = await resolver.GetMoveAsync(moveName);
                if (move != null)
                {
                    gamePokemon.Moves.Add(new GameMove(move));
                }
            }
        }

        if (!string.IsNullOrEmpty(data[5]))
        {
            gamePokemon.Nature = await resolver.GetNatureByNameAsync(data[5]);
        }

        ApplyPackedStats(data[6], gamePokemon.EVs);

        gamePokemon.Gender = data[7] switch
        {
            "F" => Gender.Female,
            "M" => Gender.Male,
            _ => gamePokemon.Gender
        };

        ApplyPackedStats(data[8], gamePokemon.IVs);

        if (!string.IsNullOrEmpty(data[9]))
        {
            gamePokemon.Shiny = data[9] == "S";
        }

        gamePokemon.LV = int.TryParse(data[10], out var level) ? level : DefaultLevel;
        gamePokemon.TreaType = await ParsePackedTeraTypeAsync(data[11], resolver) ?? gamePokemon.TreaType;

        return gamePokemon;
    }

    internal static async ValueTask<GamePokemonTeam> ParsePackedTeamAsync(
        string? oneLine,
        ShowdownDataResolver resolver,
        Func<string, string>? normalizeAbilityName = null)
    {
        var team = new GamePokemonTeam();
        if (string.IsNullOrEmpty(oneLine))
        {
            return team;
        }

        foreach (var pokemonLine in oneLine.Split(']', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var pokemon = await ParsePackedPokemonAsync(pokemonLine, resolver, normalizeAbilityName);
            if (pokemon.MetaPokemon != null)
            {
                team.GamePokemons.Add(pokemon);
            }
        }

        return team;
    }

    private static async ValueTask ParsePokemonLineAsync(
        string line,
        GamePokemon gamePokemon,
        ShowdownDataResolver resolver)
    {
        var colonIndex = line.IndexOf(':');
        if (colonIndex > 0)
        {
            var key = line[..colonIndex].Trim();
            var value = line[(colonIndex + 1)..].Trim();
            switch (key)
            {
                case "Ability":
                    gamePokemon.Ability = await resolver.GetAbilityAsync(StripParentheticalSuffix(value));
                    break;
                case "Gigantamax":
                    gamePokemon.Gmax |= value == "Yes";
                    break;
                case "Tera Type":
                    gamePokemon.TreaType = await resolver.GetTypeAsync(value);
                    break;
                case "IVs":
                    ApplyShowdownStats(value, gamePokemon.IVs);
                    break;
                case "EVs":
                    ApplyShowdownStats(value, gamePokemon.EVs);
                    break;
                case "Level":
                    if (int.TryParse(value, out var level))
                    {
                        gamePokemon.LV = level;
                    }
                    break;
                case "Shiny":
                    gamePokemon.Shiny = value == "Yes";
                    break;
            }

            return;
        }

        if (line.EndsWith(" Nature", StringComparison.Ordinal))
        {
            var natureName = line[..^7].Trim();
            gamePokemon.Nature = await resolver.GetNatureByNameAsync(natureName)
                ?? await resolver.GetNatureByIdAsync(1);
            return;
        }

        if (!line.StartsWith("-", StringComparison.Ordinal))
        {
            return;
        }

        var moveName = line[1..].Trim();
        if (moveName.Length == 0 || moveName.Contains("Hidden Power", StringComparison.Ordinal))
        {
            return;
        }

        var move = await resolver.GetMoveAsync(moveName);
        if (move != null)
        {
            gamePokemon.Moves.Add(new GameMove(move));
        }
    }

    private static string[] SplitLines(string text)
    {
        return text
            .Trim()
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n')
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    }

    private static IEnumerable<string> SplitTeamText(string text)
    {
        var current = new StringBuilder();
        foreach (var rawLine in text.Trim().Replace("\r\n", "\n", StringComparison.Ordinal).Replace('\r', '\n').Split('\n'))
        {
            if (string.IsNullOrWhiteSpace(rawLine))
            {
                if (current.Length > 0)
                {
                    yield return current.ToString();
                    current.Clear();
                }

                continue;
            }

            current.AppendLine(rawLine.Trim());
        }

        if (current.Length > 0)
        {
            yield return current.ToString();
        }
    }

    private static (string Name, string? NickName, string? Item) ParseNameAndItem(string line)
    {
        var namePart = line.Trim();
        string? item = null;
        var itemIndex = namePart.IndexOf('@');
        if (itemIndex >= 0)
        {
            item = namePart[(itemIndex + 1)..].Trim();
            namePart = namePart[..itemIndex].TrimEnd();
        }

        var parenIndex = namePart.IndexOf(" (", StringComparison.Ordinal);
        if (parenIndex < 0)
        {
            return (namePart, null, item);
        }

        var beforeParen = namePart[..parenIndex];
        var contentStart = parenIndex + 2;
        var contentEnd = namePart.IndexOf(')', contentStart);
        if (contentEnd < 0)
        {
            return (namePart, null, item);
        }

        var content = namePart[contentStart..contentEnd];
        var nextParenIndex = namePart.IndexOf(" (", contentEnd, StringComparison.Ordinal);
        if (nextParenIndex >= 0)
        {
            return (content, beforeParen, item);
        }

        return content.Length <= 3
            ? (beforeParen, null, item)
            : (content, beforeParen, item);
    }

    private static string StripParentheticalSuffix(string value)
    {
        var parenIndex = value.IndexOf(" (", StringComparison.Ordinal);
        return parenIndex < 0 ? value : value[..parenIndex].TrimEnd();
    }

    private static void ApplyShowdownStats(string value, SixDimension stats)
    {
        foreach (var statPart in value.Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var separatorIndex = statPart.IndexOf(' ');
            if (separatorIndex <= 0)
            {
                continue;
            }

            if (int.TryParse(statPart[..separatorIndex], out var statValue))
            {
                stats.SetValue(statPart[(separatorIndex + 1)..].Trim(), statValue);
            }
        }
    }

    private static string[] NormalizePackedFields(string oneLine)
    {
        var data = oneLine.TrimEnd(']').Split('|');
        if (data.Length >= PackedFieldCount)
        {
            return data;
        }

        Array.Resize(ref data, PackedFieldCount);
        for (var i = 0; i < PackedFieldCount; i++)
        {
            data[i] ??= string.Empty;
        }

        return data;
    }

    private static void ApplyPackedStats(string value, SixDimension stats)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        var values = value.Split(',');
        var count = Math.Min(values.Length, 6);
        for (var i = 0; i < count; i++)
        {
            if (int.TryParse(values[i], out var statValue))
            {
                stats[i] = statValue;
            }
        }
    }

    private static async ValueTask<PokeType?> ParsePackedTeraTypeAsync(string value, ShowdownDataResolver resolver)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }

        var teraData = value.Split(',');
        return teraData.Length >= 6 && !string.IsNullOrEmpty(teraData[5])
            ? await resolver.GetTypeAsync(teraData[5])
            : null;
    }
}

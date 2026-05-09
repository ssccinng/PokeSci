using System.Globalization;
using System.Text;

namespace DatabaseTool;

internal static class NameNormalizer
{
    public static string Normalize(string? value)
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
            .Replace("♀", "-F", StringComparison.Ordinal)
            .Replace("♂", "-M", StringComparison.Ordinal)
            .ToLower(CultureInfo.InvariantCulture);
    }

    public static string ShowdownKey(string? value)
    {
        var normalized = Normalize(value);
        if (normalized.Length == 0)
        {
            return string.Empty;
        }

        var builder = new StringBuilder(normalized.Length);
        foreach (var ch in normalized)
        {
            if (char.IsLetterOrDigit(ch))
            {
                builder.Append(ch);
            }
        }

        return builder.ToString();
    }
}

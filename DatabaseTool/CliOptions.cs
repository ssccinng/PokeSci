namespace DatabaseTool;

internal sealed class CliOptions
{
    private static readonly HashSet<string> KnownCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "help",
        "summary",
        "sync-all",
        "sync-abilities",
        "sync-items",
        "sync-moves",
        "sync-pokemon",
        "link-ps",
    };

    public string Command { get; private init; } = "summary";

    public string? ConnectionString { get; private init; }

    public string? ShitLibDataPath { get; private init; }

    public bool Apply { get; private init; }

    public bool Overwrite { get; private init; }

    public bool Verbose { get; private init; }

    public bool ShowHelp { get; private init; }

    public static string HelpText =>
        """
        Pokemon database maintenance tool

        Usage:
          dotnet run --project DatabaseTool -- <command> [options]

        Commands:
          summary          Show database/source counts.
          sync-all         Sync abilities, items, moves, Pokemon, then PS links.
          sync-abilities   Add/update abilities from PokeShitLib txtdata.
          sync-items       Add/update items from PokeShitLib txtdata.
          sync-moves       Add/update move metadata from PokeShitLib txtdata.
          sync-pokemon     Add/update Pokemon base data from PokeShitLib txtdata.
          link-ps          Link PSPokemon rows to Pokemon rows by Showdown names.

        Options:
          --connection <value>  MySQL connection string. Env fallback: POKEMON_DB_CONNECTION.
          --shitlib <path>      Path to txtdata directory or PokeShitLib root.
          --apply              Write changes. Without this, commands run as dry-run.
          --overwrite          Replace existing non-empty values with source values.
          --verbose            Print full exception details.
          --help               Show help.
        """;

    public static CliOptions Parse(string[] args)
    {
        var command = "summary";
        string? connectionString = null;
        string? shitLibDataPath = null;
        var apply = false;
        var overwrite = false;
        var verbose = false;
        var showHelp = false;

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            if (arg is "-h" or "--help" or "/?")
            {
                showHelp = true;
                continue;
            }

            if (arg.Equals("--apply", StringComparison.OrdinalIgnoreCase))
            {
                apply = true;
                continue;
            }

            if (arg.Equals("--overwrite", StringComparison.OrdinalIgnoreCase))
            {
                overwrite = true;
                continue;
            }

            if (arg.Equals("--verbose", StringComparison.OrdinalIgnoreCase))
            {
                verbose = true;
                continue;
            }

            if (arg.Equals("--connection", StringComparison.OrdinalIgnoreCase))
            {
                connectionString = ReadOptionValue(args, ref i, arg);
                continue;
            }

            if (arg.StartsWith("--connection=", StringComparison.OrdinalIgnoreCase))
            {
                connectionString = arg["--connection=".Length..];
                continue;
            }

            if (arg.Equals("--shitlib", StringComparison.OrdinalIgnoreCase))
            {
                shitLibDataPath = ReadOptionValue(args, ref i, arg);
                continue;
            }

            if (arg.StartsWith("--shitlib=", StringComparison.OrdinalIgnoreCase))
            {
                shitLibDataPath = arg["--shitlib=".Length..];
                continue;
            }

            if (arg.StartsWith("-", StringComparison.Ordinal))
            {
                throw new ArgumentException($"Unknown option: {arg}");
            }

            if (!KnownCommands.Contains(arg))
            {
                throw new ArgumentException($"Unknown command: {arg}");
            }

            command = arg;
        }

        if (command.Equals("help", StringComparison.OrdinalIgnoreCase))
        {
            showHelp = true;
        }

        return new CliOptions
        {
            Command = command,
            ConnectionString = connectionString,
            ShitLibDataPath = shitLibDataPath,
            Apply = apply,
            Overwrite = overwrite,
            Verbose = verbose,
            ShowHelp = showHelp,
        };
    }

    private static string ReadOptionValue(string[] args, ref int index, string optionName)
    {
        if (index + 1 >= args.Length)
        {
            throw new ArgumentException($"Missing value for {optionName}.");
        }

        index++;
        return args[index];
    }
}

# DatabaseTool

Pokemon database maintenance console tool.

The tool uses the EF model from `PokemonDataAccess`/`DatabaseExport` and uses
`PokeShitLib/txtdata` as the baseline for new names, moves, and Pokemon rows.
Commands are dry-run by default; pass `--apply` to write to the database.

## Usage

```powershell
dotnet run --project DatabaseTool -- summary --connection "<mysql-connection-string>"
dotnet run --project DatabaseTool -- sync-all --apply --connection "<mysql-connection-string>"
```

You can also set the connection string once:

```powershell
$env:POKEMON_DB_CONNECTION = "<mysql-connection-string>"
dotnet run --project DatabaseTool -- sync-moves --apply
```

## Commands

- `summary`: show database/source counts.
- `sync-abilities`: add/update ability names from `PokeShitLib`.
- `sync-items`: add/update item names from `PokeShitLib`.
- `sync-moves`: add/update move type, category, power, accuracy, PP, and Chinese description.
- `sync-pokemon`: add/update Pokemon base stats, types, abilities, names, and form ids.
- `link-ps`: link `PSPokemon` rows to `Pokemon` rows by Showdown-style names.
- `sync-all`: run all sync commands in dependency order.

Use `--overwrite` when existing non-empty fields should be replaced by source
data. Without it, existing non-empty values are preserved.

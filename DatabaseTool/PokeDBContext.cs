using Microsoft.EntityFrameworkCore;
using PokemonDataAccess.Interfaces;
using PokemonDataAccess.Models;

namespace PokeCommon.API.Data;

public sealed class PokeDBContext : DbContext, IPokemonContext
{
    private readonly string? connectionString;

    public PokeDBContext()
    {
    }

    public PokeDBContext(string? connectionString)
    {
        this.connectionString = string.IsNullOrWhiteSpace(connectionString) ? null : connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
        {
            return;
        }

        var resolvedConnectionString = connectionString
                                       ?? Environment.GetEnvironmentVariable("POKEMON_DB_CONNECTION")
                                       ?? Environment.GetEnvironmentVariable("PokemonDb__ConnectionString");

        if (string.IsNullOrWhiteSpace(resolvedConnectionString))
        {
            throw new InvalidOperationException(
                "Missing database connection string. Pass --connection or set POKEMON_DB_CONNECTION.");
        }

        optionsBuilder.UseMySql(resolvedConnectionString, ServerVersion.AutoDetect(resolvedConnectionString));
    }

    public DbSet<Pokemon> Pokemons { get; set; }

    public DbSet<PSPokemon> PSPokemons { get; set; }

    public DbSet<Ability> Abilities { get; set; }

    public DbSet<Move> Moves { get; set; }

    public DbSet<Item> Items { get; set; }

    public DbSet<PokeDex> PokeDex { get; set; }

    public DbSet<PokeType> PokeTypes { get; set; }

    public DbSet<TypeEffect> TypeEffect { get; set; }

    public DbSet<Nature> Natures { get; set; }

    public DbSet<Statistic> Statistics { get; set; }

    public DbSet<Flavor> Flavors { get; set; }

    public DbSet<Condition> Conditions { get; set; }

    public DbSet<Performance> Performances { get; set; }

    public DbSet<EggGroup> Egg_Groups { get; set; }
}

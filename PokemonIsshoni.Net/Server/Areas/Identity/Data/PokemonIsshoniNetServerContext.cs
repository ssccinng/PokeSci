using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PokemonDataAccess.Interfaces;
using PokemonDataAccess.Models;
using PokemonIsshoni.Net.Shared.Models;

namespace PokemonIsshoni.Net.Server.Areas.Identity.Data;

//public class PokemonIsshoniNetServerContext : IdentityDbContext<PokemonIsshoniNetServerUser>
public class PokemonIsshoniNetServerContext : ApiAuthorizationDbContext<PokemonIsshoniNetServerUser>, IPokemonContext
{
    //public PokemonIsshoniNetServerContext(DbContextOptions<PokemonIsshoniNetServerContext> options)
    //    : base(options)
    //{
    //} 
    //public PokemonIsshoniNetServerContext(DbContextOptions<PokemonIsshoniNetServerContext> options)
    //    : base(options)
    //{
    //}
    public PokemonIsshoniNetServerContext(
           DbContextOptions options,
           IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
    #region PokemonData
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
    #endregion

    /// <summary>
    /// 比赛表
    /// </summary>
    public DbSet<PCLMatch> PCLMatchs { get; set; }
    /// <summary>
    /// 比赛阶段表
    /// </summary>
    public DbSet<PCLMatchRound> PCLMatchRounds { get; set; }
    /// <summary>
    /// 比赛成员表
    /// </summary>
    public DbSet<PCLMatchPlayer> PCLMatchPlayers { get; set; }
    /// <summary>
    /// 阶段成员表
    /// </summary>
    public DbSet<PCLRoundPlayer> PCLRoundPlayers { get; set; }
    public DbSet<PCLBattle> PCLBattles { get; set; }
    /// <summary>
    /// 队伍表
    /// </summary>
    public DbSet<PCLPokeTeam> PCLPokeTeams { get; set; }
    public DbSet<Referee> PCLReferees { get; set; }
}

using Microsoft.EntityFrameworkCore;
using PokemonDataAccess.Interfaces;
using PokemonDataAccess.Models;

namespace PokeCommon.API.Data
{
    public class PokeDBContext: DbContext, IPokemonContext
    {

        public PokeDBContext()
        {
                
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                //.UseLazyLoadingProxies()
                .UseMySql("server=81.68.139.14; Database=pokedata; User ID=PokeData; Password=bnHsbRK4hiASbKRL;", 
                ServerVersion.AutoDetect("server=81.68.139.14; Database=pokedata; User ID=PokeData; Password=bnHsbRK4hiASbKRL;"));
        public DbSet<Pokemon> Pokemons {get;set;}
        public DbSet<PSPokemon> PSPokemons {get;set;}
        public DbSet<Ability> Abilities {get;set;}
        public DbSet<Move> Moves {get;set;}
        public DbSet<Item> Items {get;set;}
        public DbSet<PokeDex> PokeDex {get;set;}
        public DbSet<PokeType> PokeTypes {get;set;}
        public DbSet<TypeEffect> TypeEffect {get;set;}
        public DbSet<Nature> Natures {get;set;}
        public DbSet<Statistic> Statistics {get;set;}
        public DbSet<Flavor> Flavors {get;set;}
        public DbSet<Condition> Conditions {get;set;}
        public DbSet<Performance> Performances {get;set;}
        public DbSet<EggGroup> Egg_Groups {get;set;}
    }
}

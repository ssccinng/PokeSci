using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using PokemonDataAccess.Models;
using System.Linq;
using System;
using PokemonDataAccess.Interfaces;
// using M

namespace PokemonDataAccess
{
    public class PokemonContext : DbContext, IPokemonContext
    {
        // public PokemonContext(DbContextOptions options) : base(options) {

        // }

        // protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer(@"Data Source=(localdb)\ProjectsV13;Initial Catalog=PokemonDataBase;Integrated Security=True");
        // protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(@"Data Source=E:\vscode hub\PokemonHub\PokemonDataAccess\PokemonDataBase.db");
        //protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseMySQL(@"server=47.97.153.197;port=37793;Database=PokemonDataBase;User ID=PokemonDataBase;Password=5YEfwhkaftWD7M8k");
        //protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseMySQL(@"server=47.97.153.197;port=37793;Database=PokemonDataBase;User ID=PokemonDataBase;Password=5YEfwhkaftWD7M8k");
        // protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseMySQL(@"server=47.97.153.197;port=37793;Database=PSDisplay;User ID=PSDisplay;Password=Ld2BMCXXFpLf2aJY");
        //protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(@"Data Source=E:\vs code\PokeSci\PokeUI3\DB\PokemonDataBase.db");
        //protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite(@"Data Source=PokemonDataBase.db");
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
        //protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseMySQL("server=124.222.254.54;port=3306;Database=newpcl;User ID=NewPCL; Password=ScXL3jtMZEe7Kc6R");

        /*         protected override void OnModelCreating(ModelBuilder modelBuilder)
                {
                    modelBuilder.Entity<Pokemon>().Property(s => s.Base_Value)
                    .HasConversion(
                        v => string.Join(",", v),
                        v => v.Split().Select(int.Parse).ToArray()
                    );
                    modelBuilder.Entity<Pokemon>().Property(s => s.Get_EV)
                    .HasConversion(
                        v => string.Join(",", v),
                        v => v.Split().Select(int.Parse).ToArray()
                    );

                } */
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
        string DbPath { get; set; }
        public PokemonContext(string? dbPath = "PokeDB.db")
        {
            if (dbPath == null)
            {
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);
                DbPath = System.IO.Path.Join(path, "PokemonDataBase.db");
            }
            else
            {
                DbPath = dbPath;
            }
        }
    }
}
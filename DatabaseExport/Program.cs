// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System;
using System.Text.Json;
using PokeCommon.API.Data;

var pokemonContext = new PokeDBContext();

var cc1 = pokemonContext.Pokemons
    .Include(s => s.Ability1)
    .Include(s=>s.Ability2)
    .Include(s=>s.AbilityH)
    .Include(s=>s.Type1)
    .Include(s=>s.Type2)
    .ToList();
File.WriteAllText("Pokemons.json", JsonSerializer.Serialize(cc1));
return;

var prop = pokemonContext.GetType().GetProperties();

MethodInfo toListMethod = typeof(Enumerable).GetMethod("ToList");
foreach (var property in prop)
{
    if (property.PropertyType.IsGenericType)
    {
        MethodInfo genericToListMethod = toListMethod.MakeGenericMethod(property.PropertyType.GetGenericArguments()[0]);

        Console.WriteLine(property.Name);
        var db = property.GetValue(pokemonContext, null);

        var cc = genericToListMethod.Invoke(null, [db]);

         var tt =  JsonSerializer.Serialize(cc);
        File.WriteAllText($"{property.Name}.json", tt);
        //var cc = db.ToList();
    }
    //Console.WriteLine(property.PropertyType);
    //Console.WriteLine(property.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));
    //Console.WriteLine(property.DeclaringType);
}
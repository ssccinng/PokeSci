// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System;
using System.Text.Json;

PokemonDataAccess.PokemonContext pokemonContext = new PokemonDataAccess.PokemonContext();

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
﻿using PokeCommon.Models;
using PokeCommon.Utils;
using System.Text;

namespace Poke.Usage
{
    /// <summary>
    /// 总体使用率
    /// </summary>
    public class PokeUsage
    {
        public List<PokemonUsage> PokemonUsage { get; set; } = [];

        public async Task<string> ToTextAsync(LanguageType languageType = LanguageType.CHS, bool isDex = false)
        {
            StringBuilder sb = new StringBuilder();

            var pokemons = PokemonTools.PokemonContext.Pokemons.ToArray();

            switch (languageType)
            {
                case LanguageType.JPN:
                    break;
                case LanguageType.ENG:
                    break;
                case LanguageType.FRA:
                    break;
                case LanguageType.ITA:
                    break;
                case LanguageType.GER:
                    break;
                case LanguageType.SPA:
                    break;
                case LanguageType.KOR:
                    break;
                case LanguageType.CHS:

                    foreach (var pokemonUsage in PokemonUsage)
                    {
                        var pokemon = isDex ? pokemons.FirstOrDefault(s => s.DexId == pokemonUsage.Id) : await PokemonTools.GetPokemonAsync(pokemonUsage.Id);
                        sb.AppendLine($"{(isDex ? pokemon.NameChs : pokemon.FullNameChs)} {pokemonUsage.Count} {pokemonUsage.Percentage:P}");

                        sb.AppendLine("技能使用率:");
                        foreach (var moveUsage in pokemonUsage.MoveUsage)
                        {
                            var move = await PokemonTools.GetMoveAsync(moveUsage.Id);
                            sb.AppendLine($"    {move.Name_Chs} {moveUsage.Count} {moveUsage.Percentage:P}");
                        }
                        sb.AppendLine();
                        sb.AppendLine();
                        sb.AppendLine("同伴使用率:");
                        foreach (var aliyPokemonUsage in pokemonUsage.AliyPokemonUsage)
                        {
                            var aliyPokemon = isDex ? pokemons.FirstOrDefault(s => s.DexId == aliyPokemonUsage.Id) : await PokemonTools.GetPokemonAsync(aliyPokemonUsage.Id);
                            sb.AppendLine($"    {(isDex ? aliyPokemon.NameChs : aliyPokemon.FullNameChs)} {aliyPokemonUsage.Count} {aliyPokemonUsage.Percentage:P}");
                        }


                        sb.AppendLine();
                        sb.AppendLine();
                        sb.AppendLine("道具使用率:");
                        foreach (var itemUsage in pokemonUsage.ItemUsage)
                        {
                            var item = await PokemonTools.GetItemAsync(itemUsage.Id);
                            sb.AppendLine($"    {item.Name_Chs} {itemUsage.Count} {itemUsage.Percentage:P}");
                        }

                        sb.AppendLine();
                        sb.AppendLine();
                        sb.AppendLine("特性使用率:");
                        foreach (var abilityUsage in pokemonUsage.AbilityUsage)
                        {
                            var ability = await PokemonTools.GetAbilityAsync(abilityUsage.Id);
                            sb.AppendLine($"    {ability.Name_Chs} {abilityUsage.Count} {abilityUsage.Percentage:P}");
                        }
                        sb.AppendLine();
                        sb.AppendLine();
                        sb.AppendLine();
                        sb.AppendLine();
                    }
                    break;
                case LanguageType.CHT:
                    break;
                default:
                    break;
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// 单一宝可梦使用率
    /// </summary>
    public class PokemonUsage: UsageItem
    {

        public List<UsageItem> MoveUsage { get; set; } = [];
        public List<UsageItem> ItemUsage { get; set; } = [];
        public List<UsageItem> AbilityUsage { get; set; } = [];

        public List<UsageItem> NatureUsage { get; set; } = [];

        public List<UsageItem> AliyPokemonUsage { get; set; } = [];
    }

    public class UsageItem
    {
        public int Id { get; set; }

        public int Count { get; set; }

        public decimal Percentage { get; set; }
    }
}

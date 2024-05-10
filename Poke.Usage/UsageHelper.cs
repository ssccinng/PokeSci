using PokeCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poke.Usage
{
    public static class UsageHelper
    {
        public static PokeUsage GetUsage(IEnumerable<GamePokemonTeam> gamePokemonTeams)
        {
            PokeUsage usage = new PokeUsage();
            foreach (var gamePokemonTeam in gamePokemonTeams)
            {
                foreach (var gamePokemon in gamePokemonTeam.GamePokemons)
                {
                    var pokemon = usage.PokemonUsage.FirstOrDefault(p => p.Id == gamePokemon.PokemonId);
                    if (pokemon == null)
                    {
                        pokemon = new PokemonUsage
                        {
                            Id = gamePokemon.PokemonId,
                            Count = 1
                        };
                        usage.PokemonUsage.Add(pokemon);
                    }
                    else
                    {
                        pokemon.Count++;
                    }

                    foreach (var gamePokemonMove in gamePokemon.Moves)
                    {
                        var move = pokemon.MoveUsage.FirstOrDefault(m => m.Id == gamePokemonMove.MoveId);
                        if (move == null)
                        {
                            move = new UsageItem
                            {
                                Id = gamePokemonMove.MoveId,
                                Count = 1
                            };
                            pokemon.MoveUsage.Add(move);
                        }
                        else
                        {
                            move.Count++;
                        }
                    }


                    foreach (var poke in gamePokemonTeam.GamePokemons)
                    {
                        if (poke.PokemonId != gamePokemon.PokemonId)
                        {
                            var aliyPokemon = pokemon.AliyPokemonUsage.FirstOrDefault(p => p.Id == poke.PokemonId);
                            if (aliyPokemon == null)
                            {
                                aliyPokemon = new UsageItem
                                {
                                    Id = poke.PokemonId,
                                    Count = 1
                                };
                                pokemon.AliyPokemonUsage.Add(aliyPokemon);
                            }
                            else
                            {
                                aliyPokemon.Count++;
                            }
                        }
                        
                    }



                    var heldItem = pokemon.ItemUsage.FirstOrDefault(i => i.Id == gamePokemon.Item?.ItemId);
                    if (heldItem == null)
                    {
                        if (gamePokemon.Item != null)
                        {
                            heldItem = new UsageItem
                            {
                                Id = gamePokemon.Item.ItemId,
                                Count = 1
                            };
                            pokemon.ItemUsage.Add(heldItem);
                        }
                    }
                    else
                    {
                        heldItem.Count++;
                    }

                    var ability = pokemon.AbilityUsage.FirstOrDefault(a => a.Id == gamePokemon.Ability?.AbilityId);
                    if (ability == null)
                    {
                        if (gamePokemon.Ability != null)
                        {
                            ability = new UsageItem
                            {
                                Id = gamePokemon.Ability.AbilityId,
                                Count = 1
                            };
                            pokemon.AbilityUsage.Add(ability);
                        }
                    }
                    else
                    {
                        ability.Count++;
                    }

                    var nature = pokemon.NatureUsage.FirstOrDefault(n => n.Id == gamePokemon.Nature?.NatureId);
                    if (nature == null)
                    {
                        if (gamePokemon.Nature != null)
                        {
                            nature = new UsageItem
                            {
                                Id = gamePokemon.Nature.NatureId,
                                Count = 1
                            };
                            pokemon.NatureUsage.Add(nature);
                        }
                    }
                    else
                    {
                        nature.Count++;
                    }
                }
            }

            foreach (var pokemon in usage.PokemonUsage)
            {
                pokemon.Percentage = (decimal)pokemon.Count / gamePokemonTeams.Count();
                foreach (var move in pokemon.MoveUsage)
                {
                    move.Percentage = (decimal)move.Count / pokemon.Count;
                }
                foreach (var item in pokemon.ItemUsage)
                {
                    item.Percentage = (decimal)item.Count / pokemon.Count;
                }
                foreach (var ability in pokemon.AbilityUsage)
                {
                    ability.Percentage = (decimal)ability.Count / pokemon.Count;
                }
                foreach (var nature in pokemon.NatureUsage)
                {
                    nature.Percentage = (decimal)nature.Count / pokemon.Count;
                }
                foreach (var aliyPokemon in pokemon.AliyPokemonUsage)
                {
                    aliyPokemon.Percentage = (decimal)aliyPokemon.Count / pokemon.AliyPokemonUsage.Sum(p => p.Count);
                }

                pokemon.MoveUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.ItemUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.AbilityUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.NatureUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.AliyPokemonUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
            }

            usage.PokemonUsage.Sort((a, b) => b.Count.CompareTo(a.Count));



            return usage;
        }
    }
}

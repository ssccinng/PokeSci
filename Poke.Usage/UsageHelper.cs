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


        public static PokeUsage GetUsageByDexId(IEnumerable<GamePokemonTeam> gamePokemonTeams)
        {
            PokeUsage usage = new PokeUsage();
            int i = 0;
            foreach (var gamePokemonTeam in gamePokemonTeams)
            {
                i++;
                foreach (var gamePokemon in gamePokemonTeam.GamePokemons)
                {
                    var pokemon = usage.PokemonUsage.FirstOrDefault(p => p.Id == gamePokemon.MetaPokemon?.DexId);
                    if (pokemon == null)
                    {
                        pokemon = new PokemonUsage
                        {
                            Id = gamePokemon.MetaPokemon?.DexId ?? 0,
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
                            var aliyPokemon = pokemon.AliyPokemonUsage.FirstOrDefault(p => p.Id == poke.MetaPokemon?.DexId);
                            if (aliyPokemon == null)
                            {
                                aliyPokemon = new UsageItem
                                {
                                    Id = poke.MetaPokemon?.DexId ?? 0,
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

                    var treaType = pokemon.TreaUsage.FirstOrDefault(t => t.Id == gamePokemon.TreaType?.Id);
                    if (treaType == null)
                    {
                        if (gamePokemon.TreaType != null)
                        {
                            treaType = new UsageItem
                            {
                                Id = gamePokemon.TreaType.Id,
                                Count = 1
                            };
                            pokemon.TreaUsage.Add(treaType);
                        }
                    }
                    else
                    {
                        treaType.Count++;
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
                    aliyPokemon.Percentage = (decimal)aliyPokemon.Count / pokemon.Count;
                }
                foreach (var treaType in pokemon.TreaUsage)
                {
                    treaType.Percentage = (decimal)treaType.Count / pokemon.Count;
                }


                pokemon.MoveUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.ItemUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.AbilityUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.NatureUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.AliyPokemonUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.TreaUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
            }

            usage.PokemonUsage.Sort((a, b) => b.Count.CompareTo(a.Count));



            return usage;
        } 
        
        
        public static PokeUsage GetUsage(IEnumerable<GamePokemonTeam> gamePokemonTeams)
        {
            PokeUsage usage = new PokeUsage();
            int i = 0;
            foreach (var gamePokemonTeam in gamePokemonTeams)
            {
                i++;
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

                    var treaType = pokemon.TreaUsage.FirstOrDefault(t => t.Id == gamePokemon.TreaType?.Id);
                    if (treaType == null)
                    {
                        if (gamePokemon.TreaType != null)
                        {
                            treaType = new UsageItem
                            {
                                Id = gamePokemon.TreaType.Id,
                                Count = 1
                            };
                            pokemon.TreaUsage.Add(treaType);
                        }
                    }
                    else
                    {
                        treaType.Count++;
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
                    aliyPokemon.Percentage = (decimal)aliyPokemon.Count / pokemon.Count;
                }
                foreach (var treaType in pokemon.TreaUsage)
                {
                    treaType.Percentage = (decimal)treaType.Count / pokemon.Count;
                }

                pokemon.MoveUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.ItemUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.AbilityUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.NatureUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.AliyPokemonUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.TreaUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
            }

            usage.PokemonUsage.Sort((a, b) => b.Count.CompareTo(a.Count));



            return usage;
        }


        public static PokeUsage GetUsageByDexId(IEnumerable<SimpleGamePokemonTeam> gamePokemonTeams)
        {
            PokeUsage usage = new PokeUsage();
            int i = 0;
            foreach (var gamePokemonTeam in gamePokemonTeams)
            {
                i++;
                foreach (var gamePokemon in gamePokemonTeam.GamePokemons)
                {
                    var pokemon = usage.PokemonUsage.FirstOrDefault(p => p.Id == gamePokemon.DexId);
                    if (pokemon == null)
                    {
                        pokemon = new PokemonUsage
                        {
                            Id = gamePokemon.DexId,
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
                        var move = pokemon.MoveUsage.FirstOrDefault(m => m.Id == gamePokemonMove);
                        if (move == null)
                        {
                            move = new UsageItem
                            {
                                Id = gamePokemonMove,
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
                            var aliyPokemon = pokemon.AliyPokemonUsage.FirstOrDefault(p => p.Id == poke.DexId);
                            if (aliyPokemon == null)
                            {
                                aliyPokemon = new UsageItem
                                {
                                    Id = poke.DexId,
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

                    var heldItem = pokemon.ItemUsage.FirstOrDefault(i => i.Id == gamePokemon.Item);
                    if (heldItem == null)
                    {
                        if (gamePokemon.Item != null)
                        {
                            heldItem = new UsageItem
                            {
                                Id = gamePokemon.Item,
                                Count = 1
                            };
                            pokemon.ItemUsage.Add(heldItem);
                        }
                    }
                    else
                    {
                        heldItem.Count++;
                    }

                    var ability = pokemon.AbilityUsage.FirstOrDefault(a => a.Id == gamePokemon.Ability);
                    if (ability == null)
                    {
                        if (gamePokemon.Ability != null)
                        {
                            ability = new UsageItem
                            {
                                Id = gamePokemon.Ability,
                                Count = 1
                            };
                            pokemon.AbilityUsage.Add(ability);
                        }
                    }
                    else
                    {
                        ability.Count++;
                    }

                    var nature = pokemon.NatureUsage.FirstOrDefault(n => n.Id == gamePokemon.Nature);
                    if (nature == null)
                    {
                        if (gamePokemon.Nature != null)
                        {
                            nature = new UsageItem
                            {
                                Id = gamePokemon.Nature,
                                Count = 1
                            };
                            pokemon.NatureUsage.Add(nature);
                        }
                    }
                    else
                    {
                        nature.Count++;
                    }

                    var treaType = pokemon.TreaUsage.FirstOrDefault(t => t.Id == gamePokemon.TreaType);
                    if (treaType == null)
                    {
                        if (gamePokemon.TreaType != null)
                        {
                            treaType = new UsageItem
                            {
                                Id = gamePokemon.TreaType,
                                Count = 1
                            };
                            pokemon.TreaUsage.Add(treaType);
                        }
                    }
                    else
                    {
                        treaType.Count++;
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
                    aliyPokemon.Percentage = (decimal)aliyPokemon.Count / pokemon.Count;
                }
                foreach (var treaType in pokemon.TreaUsage)
                {
                    treaType.Percentage = (decimal)treaType.Count / pokemon.Count;
                }

                pokemon.MoveUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.ItemUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.AbilityUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.NatureUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.AliyPokemonUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.TreaUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
            }
            usage.PokemonUsage.Sort((a, b) => b.Count.CompareTo(a.Count));

            return usage;
        }
        
        public static PokeUsage GetUsage(IEnumerable<SimpleGamePokemonTeam> gamePokemonTeams)
        {
            PokeUsage usage = new PokeUsage();
            int i = 0;
            foreach (var gamePokemonTeam in gamePokemonTeams)
            {
                i++;
                if (gamePokemonTeam == null)
                {
                    continue;
                }

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
                        var move = pokemon.MoveUsage.FirstOrDefault(m => m.Id == gamePokemonMove);
                        if (move == null)
                        {
                            move = new UsageItem
                            {
                                Id = gamePokemonMove,
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

                    var heldItem = pokemon.ItemUsage.FirstOrDefault(i => i.Id == gamePokemon.Item);

                    if (heldItem == null)
                    {
                        if (gamePokemon.Item != 0)
                        {
                            heldItem = new UsageItem
                            {
                                Id = gamePokemon.Item,
                                Count = 1
                            };
                            pokemon.ItemUsage.Add(heldItem);
                        }
                    }
                    else
                    {
                        heldItem.Count++;
                    }

                    var ability = pokemon.AbilityUsage.FirstOrDefault(a => a.Id == gamePokemon.Ability);
                    if (ability == null)
                    {
                        if (gamePokemon.Ability != 0)
                        {
                            ability = new UsageItem
                            {
                                Id = gamePokemon.Ability,
                                Count = 1
                            };
                            pokemon.AbilityUsage.Add(ability);
                        }
                    }
                    else
                    {
                        ability.Count++;
                    }

                    var nature = pokemon.NatureUsage.FirstOrDefault(n => n.Id == gamePokemon.Nature);
                    if (nature == null)
                    {
                        if (gamePokemon.Nature != 0)
                        {
                            nature = new UsageItem
                            {
                                Id = gamePokemon.Nature,
                                Count = 1
                            };
                            pokemon.NatureUsage.Add(nature);
                        }
                    }
                    else
                    {
                        nature.Count++;
                    }

                    var treaType = pokemon.TreaUsage.FirstOrDefault(t => t.Id == gamePokemon.TreaType);
                    if (treaType == null)
                    {
                        if (gamePokemon.TreaType != null)
                        {
                            treaType = new UsageItem
                            {
                                Id = gamePokemon.TreaType,
                                Count = 1
                            };
                            pokemon.TreaUsage.Add(treaType);
                        }
                    }
                    else
                    {
                        treaType.Count++;
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
                    aliyPokemon.Percentage = (decimal)aliyPokemon.Count / pokemon.Count;
                }
                foreach (var treaType in pokemon.TreaUsage)
                {
                    treaType.Percentage = (decimal)treaType.Count / pokemon.Count;
                }

                pokemon.MoveUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.ItemUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.AbilityUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.NatureUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.AliyPokemonUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
                pokemon.TreaUsage.Sort((a, b) => b.Count.CompareTo(a.Count));
            }
            usage.PokemonUsage.Sort((a, b) => b.Count.CompareTo(a.Count));

            return usage;
        }
    }
}

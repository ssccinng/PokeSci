using PokeCommon.Models;
using Showdown;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Showdown
{
    public static class BattleDataExtension
    {
        public static int GetSub1(int value) => Math.Max(value - 1, 0);

        public static (int side, int pos) GetSidePos(string data)
        {
            if (data.StartsWith("p1a"))
            {
                return (1, 0);
                //battle.p
            }
            else if (data.StartsWith("p1b"))
            {
                return (1, 1);
            }
            else if (data.StartsWith("p2a"))
            {
                return (2, 0);
            }
            else if (data.StartsWith("p2b"))
            {
                return (2, 1);

            }
            //return (-1, -1);
            // 怪异
            return (1, 0);
        }

        extension(BattleTurnN battleTurnN)
        {
            public BattleTurnN NextTurn()
            {
                var cc = battleTurnN.SideField.Select(x => x.NextTurn());
                var newTurn = battleTurnN with
                {
                    Turn = battleTurnN.Turn + 1,
                    BattleField = battleTurnN.BattleField.NextTurn(),
                    SideField = [.. battleTurnN.SideField.Select(x => x.NextTurn())],
                };
                return newTurn;
            }
        }

        extension(BattleField battleField)
        {
            public BattleField NextTurn()
            {
                return battleField with
                {
                    WeatherRemain = GetSub1(battleField.WeatherRemain),
                    TerrainRemain = GetSub1(battleField.TerrainRemain),
                    MagicRoom = GetSub1(battleField.MagicRoom),
                    TrickRoom = GetSub1(battleField.TrickRoom),
                    WonderRoom = GetSub1(battleField.WonderRoom),
                    Gravity = GetSub1(battleField.Gravity),
                    MudSport = GetSub1(battleField.MudSport),
                    Uproar = GetSub1(battleField.Uproar),
                    WaterSport = GetSub1(battleField.WaterSport),

                };
            }
        }

        extension(OneSideBattleField oneSideBattleField)
        {
            public OneSideBattleField NextTurn()
            {
                var newField = oneSideBattleField with { };
                foreach (var property in newField.GetType().GetProperties())
                {
                    var decrease = property.GetCustomAttribute<DecreaseAttribute>();
                    if (decrease != null)
                    {
                        var value = (int)property.GetValue(newField);
                        if (value > 0)
                            value = Math.Max(value + decrease.DeltaValue, 0);
                        property.SetValue(newField, value);
                    }
                    var singleTurn = property.GetCustomAttribute<SingleTurnAttribute>();
                    if (singleTurn != null)
                    {
                        property.SetValue(newField, 0);
                    }
                }

                return newField;
            }
        }

        extension(BattleData battleData)
        {
            public BattleData UpdateLastTurn(BattleTurnN newTurn)
            {
                return battleData with
                {
                    BattleTurns = battleData.BattleTurns.SetItem(battleData.BattleTurns.Count() - 1, newTurn)
                };
            }


            public BattleData SetMyTeam(GamePokemonTeam gamePokemonTeam)
            {
                return battleData with
                {
                    Player1Team = gamePokemonTeam,
                    Player2Team = gamePokemonTeam
                };
            }



            public async Task<BattleData> ApplyLog(string cmd, string[] lines)
            {
                return cmd switch
                {
                    "player" => battleData.ApplyPlayer(lines),
                    "turn" => battleData.ApplyTurn(lines),
                    "teampreview" => battleData with { ChooseSize = int.Parse( lines[0]) }, // 这个不需要处理
                    //"win" => battleData.ApplyWin(lines),
                    "poke" => await battleData.ApplyPoke(lines),
                    "switch" => battleData.ApplySwitch(lines),
                    //"teampreview" => battleData.ApplyTeamPreview(lines),
                    _ => battleData
                };
            }

            public BattleData ApplyPlayer(string[] lines)
            {
                if (lines.Length > 4)
                {
                    int.TryParse(lines[3], out int score);
                    if (lines[0] == "p1")
                    {
                        return battleData with { Player1Id = lines[2], Player1Score = score };
                    }
                    else
                    {
                        return battleData with { Player2Id = lines[2], Player2Score = score };
                    }
                }
                return battleData;
            }

            public BattleTurnN GetLastTurn()
            {
                return battleData.BattleTurns.Last(); // 断言一下 问题不大
            }

            public BattleData ApplyTurn(string[] lines)
            {
                // 开局的turn要看好了
                var lastTurn = battleData.GetLastTurn();
                if (lines[0] != "1")
                {
                    return battleData with { BattleTurns = battleData.BattleTurns.Add(lastTurn.NextTurn()) };
                }
                else
                {
                    return battleData;
                }
            }

            
            public async Task<BattleData> ApplyPoke(string[] lines)
            {
                var pokemonName = lines[1].Split(',')[0];
                var lastTurn = battleData.BattleTurns.Last()!;
                if (lines[0] == "p1")
                {
                    var sideTeam = lastTurn.SideTeam[0];
                    var newPokes = sideTeam.Pokemons.Add(await BattlePokemon.CreateAsync(pokemonName));
                    var newTurn = lastTurn with
                    {
                        SideTeam = lastTurn.SideTeam.SetItem(0, sideTeam with { Pokemons = newPokes })
                    };
                    return battleData with
                    {
                        BattleTurns = battleData.BattleTurns.SetItem(battleData.BattleTurns.Count() - 1, newTurn)
                    };

                }
                else
                {
                    var sideTeam = lastTurn.SideTeam[1];
                    var newPokes = sideTeam.Pokemons.Add(await BattlePokemon.CreateAsync(pokemonName));
                    var newTurn = lastTurn with
                    {
                        SideTeam = lastTurn.SideTeam.SetItem(1, sideTeam with { Pokemons = newPokes })
                    };
                    return battleData.UpdateLastTurn(newTurn);
                }
            }


            public BattleData ApplySwitch(string[] lines)
            {
                // 开局的turn要看好了
                var switchPokemonName = lines[1].Split(',')[0];
                var lastTurn = battleData.BattleTurns.Last()!;
                //var switchNickName = lines[1].Split(',')[0];
                var switchData = GetSidePos(lines[0][..3]);

                // 对公开信息处理

                if (switchData.side == 1)
                {
                    var sideTeam = lastTurn.SideTeam[0];
                    var newPokes = sideTeam.Pokemons
                        .Select(x => 
                        x.Position == switchData.pos 
                        ? x with { Position = -1, BattleStatus = PsBattleStatus.InBackField } 
                        : x).ToImmutableArray();
                    newPokes = sideTeam.Pokemons
                        .Select(x => 
                        x.PsName == switchPokemonName 
                        ? x with { Position = switchData.pos, BattleStatus = PsBattleStatus.InField }
                        : x)
                        .ToImmutableArray();

                    var newTurn = lastTurn with
                    {
                        SideTeam = lastTurn.SideTeam.SetItem(0, sideTeam with { Pokemons = newPokes })
                    };

                    return battleData.UpdateLastTurn(newTurn);

                }
                else
                {
                    var sideTeam = lastTurn.SideTeam[1];
                    var newPokes = sideTeam.Pokemons
                        .Select(x => 
                        x.Position == switchData.pos 
                        ? x with { Position = -1, BattleStatus = PsBattleStatus.InBackField } 
                        : x).ToImmutableArray();
                    newPokes = sideTeam.Pokemons
                        .Select(x => 
                        x.PsName == switchPokemonName 
                        ? x with { Position = switchData.pos, BattleStatus = PsBattleStatus.InField }
                        : x)
                        .ToImmutableArray();
                    var newTurn = lastTurn with
                    {
                        SideTeam = lastTurn.SideTeam.SetItem(1, sideTeam with { Pokemons = newPokes })
                    };

                    return battleData.UpdateLastTurn(newTurn);
                }

            }

        }

    }



}





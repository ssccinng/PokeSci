using PokeCommon.Models;
using PokeCommon.Utils;
using Serilog;
using Showdown;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        public static int GetPlayer(string data)
        {
            if (data.Trim() == "p1")
            {
                return 0;
            }
            return 1;
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
                    SideTeam = battleTurnN.SideTeam.Select(x => x with { Pokemons = x.Pokemons.Select(p => p.NextTurn()).ToImmutableArray() }).ToImmutableArray(),
                };
                return newTurn;
            }

            public BattleTurnN WithUpdatedSidePokemons(int sideIndex, ImmutableArray<BattlePokemon> newPokes)
            {
                var newSideTeam = battleTurnN.SideTeam.SetItem(sideIndex, battleTurnN.SideTeam[sideIndex] with { Pokemons = [.. newPokes] });
                return battleTurnN with { SideTeam = newSideTeam };
            }
            /// <summary>
            /// 更新血量百分比
            /// </summary>
            /// <param name="sideData"></param>
            /// <param name="hp">变化血量（百分比）</param>
            /// <returns></returns>
            public BattleTurnN UpdatePokemonHp((int side, int pos) sideData, int hp)
            {
                var newPokes = battleTurnN.SideTeam[sideData.side - 1].Pokemons
                   .Select(x => x.Position == sideData.pos ? x with { HpRemain = hp } : x).ToImmutableArray();

                return battleTurnN.WithUpdatedSidePokemons(sideData.side - 1, newPokes);

            }
        }

        extension(BattleField battleField)
        {
            public BattleField NextTurn() =>
                battleField with
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

        extension(BattlePokemon pokemon)
        {
            public BattlePokemon SwitchIn()
            {
                return pokemon with { Status = pokemon.Status with { InField_First_Turn = 1 } };
            }

            public BattlePokemon SwitchOut()
            {
                var newPoke = pokemon with { TeratallizeStatus = TeratallizeStatus.NotTeraSallized };
                // 反射修改其中changRefresh的
                foreach (var property in newPoke.GetType().GetProperties())
                {

                    var changeRefresh = property.GetCustomAttribute<ChangeRefreshAttribute>();
                    if (changeRefresh != null)
                    {
                        property.SetValue(newPoke, 0);
                    }


                    var singleTurn = property.GetCustomAttribute<SingleTurnAttribute>();
                    if (singleTurn != null)
                    {
                        property.SetValue(newPoke, 0);
                    }
                }
                // 太晶清理一下
                


                return newPoke;
            }

            public BattlePokemon NextTurn()
            {
                return pokemon with { Status = pokemon.Status.NextTurn() };
            }
        }
        extension (PokemonStatus pokemonStatus)
        {
            public PokemonStatus NextTurn()
            {
                var newStatus = pokemonStatus with { };
                foreach (var property in newStatus.GetType().GetProperties())
                {
                    var decrease = property.GetCustomAttribute<DecreaseAttribute>();
                    if (decrease != null)
                    {
                        var value = (int)property.GetValue(newStatus);
                        if (value > 0)
                            value = Math.Max(value + decrease.DeltaValue, 0);
                        property.SetValue(newStatus, value);
                    }
                    var singleTurn = property.GetCustomAttribute<SingleTurnAttribute>();
                    if (singleTurn != null)
                    {
                        property.SetValue(newStatus, 0);
                    }
                }
                return newStatus;
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

            public BattleData SetMyName(string name)
            {
                int slot = battleData.PlayerDatas[0].PlayerName == name ? 0 : 1;
                return battleData with
                {
                    MySlot = slot,
                    MyName = name,
                };
            }


            public BattleData SetMyTeam(GamePokemonTeam gamePokemonTeam)
            {
                var myData = battleData.PlayerDatas[battleData.MySlot];
                return battleData with
                {
                    PlayerDatas = battleData.PlayerDatas.SetItem(battleData.MySlot, myData with
                    {
                        Team = gamePokemonTeam
                    })
                };
            }

            public BattleData SetMyOrderTeam(string[] myOrder)
            {
                return battleData with { MyOrderTeam = [..myOrder] };
            }

            public async Task<BattleData> ApplyLog(string cmd, string[] lines) => cmd switch
            {
                "player" => battleData.ApplyPlayer(lines),
                "turn" => battleData.ApplyTurn(lines),
                "teampreview" => battleData with { ChooseSize = int.Parse(lines[0]) }, // 这个不需要处理
                "poke" => await battleData.ApplyPoke(lines),
                "switch" => battleData.ApplySwitch(lines),
                "drag" => battleData.ApplyDrag(lines),
                "detailschange" => await battleData.ApplyDetailsChange(lines),
                "move" => await battleData.ApplyMove(lines),
                "faint" => battleData.ApplyFaint(lines),
                "request" => battleData.ApplyRequest(lines),


                "-ability" => battleData.ApplyAbility(lines),
                "-terastallize" => await battleData.ApplyTerastallize(lines),
                "-singleturn" => battleData.ApplySingleturn(lines),
                "-damage" => battleData.ApplyDamage(lines),
                "-heal" => battleData.ApplyHeal(lines),
                "-weather" => battleData.ApplyWeather(lines),
                "-sidestart" => battleData.ApplySide(lines, true),
                "-sideend" => battleData.ApplySide(lines, false),
                "-fieldstart" => battleData.ApplyField(lines, true),
                "-fieldend" => battleData.ApplyField(lines, false),
                "-status" => battleData.ApplyStatus(lines, true),
                "-curestatus" => battleData.ApplyStatus(lines, false),
                "-boost" => battleData.ApplyBoost(lines, true),
                "-unboost" => battleData.ApplyBoost(lines, false),

                _ => battleData
            };

            public BattleData ApplyPlayer(string[] lines)
            {
                if (lines.Length > 4)
                {
                    int.TryParse(lines[3], out int score);
                    int pos = lines[0] == "p1" ? 0 : 1; // p1是0 p2是1

                    return battleData with
                    {
                        PlayerDatas = battleData.PlayerDatas.SetItem(pos, new PlayerData
                        {
                            PlayerId = lines[2],
                            PlayerName = lines[1],
                            Score = score
                        })
                    }
                    ;

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
                pokemonName = pokemonName.Replace("-*", "");
                var lastTurn = battleData.GetLastTurn()!;
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

            /// <summary>
            /// 处理Switch命令 //记得刷新宝的状态
            /// </summary>
            /// <param name="lines"></param>
            /// <returns></returns>
            public BattleData ApplySwitch(string[] lines)
            {
                Func<BattlePokemon, BattlePokemon> SetUnkonwnToNoInBattle = x =>
                            x.BattleStatus is UnKnown
                            ? x with { BattleStatus = PsBattleStatus.NotInBattleTeam }
                            : x;
                // 开局的turn要看好了
                var switchPokemonName = lines[1].Split(',')[0];
                var lastTurn = battleData.GetLastTurn()!;
                //var switchNickName = lines[1].Split(',')[0];
                var switchData = GetSidePos(lines[0][..3]);

                // 对公开信息处理

                if (switchData.side == 1)
                {
                    var sideTeam = lastTurn.SideTeam[0];
                    var newPokes = sideTeam.Pokemons
                        .Select(x =>
                        x.Position == switchData.pos
                        ? (x with { Position = -1, BattleStatus = PsBattleStatus.InBackField }).SwitchOut()
                        : x)
                        .Select(x => // 设置后排宝可梦上场
                        x.PsName == switchPokemonName
                        ? (x with { Position = switchData.pos, BattleStatus = PsBattleStatus.InField }).SwitchIn()
                        : x);
                    //.ToImmutableArray();

                    if (newPokes.Count(x => x.BattleStatus is not UnKnown) == battleData.ChooseSize)
                    {
                        newPokes = newPokes.Select(SetUnkonwnToNoInBattle);
                    }

                    var newTurn = lastTurn with
                    {
                        SideTeam = lastTurn.SideTeam.SetItem(0, sideTeam with { Pokemons = [.. newPokes] })
                    };

                    return battleData.UpdateLastTurn(newTurn);

                }
                else
                {
                    var sideTeam = lastTurn.SideTeam[1];
                    var newPokes = sideTeam.Pokemons
                        .Select(x =>
                        x.Position == switchData.pos
                        ? (x with { Position = -1, BattleStatus = PsBattleStatus.InBackField }).SwitchOut()
                        : x)
                        .Select(x =>
                        x.PsName == switchPokemonName
                        ? (x with { Position = switchData.pos, BattleStatus = PsBattleStatus.InField }).SwitchIn()
                        : x);
                    //.ToImmutableArray();

                    if (newPokes.Count(x => x.BattleStatus is not UnKnown) == battleData.ChooseSize)
                    {
                        newPokes = newPokes.Select(SetUnkonwnToNoInBattle);
                    }
                    var newTurn = lastTurn with
                    {
                        SideTeam = lastTurn.SideTeam.SetItem(1, sideTeam with { Pokemons = [.. newPokes] })
                    };

                    return battleData.UpdateLastTurn(newTurn);
                }

            }

            /// <summary>
            /// 宝可梦形态发生改变
            /// </summary>
            /// <param name="lines"></param>
            /// <returns></returns>
            public async Task<BattleData> ApplyDetailsChange(string[] lines)
            {
                // 开局的turn要看好了
                var lastTurn = battleData.GetLastTurn()!;
                var sideData = GetSidePos(lines[0][..3]);
                var pokemonName = lines[1].Split(',')[0];

                var pokemon = await PokemonToolsWithoutDB.GetPokemonFromPsNameAsync(pokemonName);

                if (sideData.side == 1)
                {
                    var sideTeam = lastTurn.SideTeam[0];
                    var newPokes = sideTeam.Pokemons
                        .Select(x =>
                        x.Position == sideData.pos
                        ? x with { Pokemon = x.Pokemon with { MetaPokemon = pokemon } }
                        : x).ToImmutableArray();
                    var newTurn = lastTurn with
                    {
                        SideTeam = lastTurn.SideTeam.SetItem(0, sideTeam with { Pokemons = [.. newPokes] })
                    };
                    return battleData.UpdateLastTurn(newTurn);
                }
                else
                {
                    var sideTeam = lastTurn.SideTeam[1];
                    var newPokes = sideTeam.Pokemons
                        .Select(x =>
                        x.Position == sideData.pos
                        ? x with { Pokemon = x.Pokemon with { MetaPokemon = pokemon } }
                        : x).ToImmutableArray();
                    var newTurn = lastTurn with
                    {
                        SideTeam = lastTurn.SideTeam.SetItem(1, sideTeam with { Pokemons = [.. newPokes] })
                    };
                    return battleData.UpdateLastTurn(newTurn);
                }

                //return battleData;
            }

            public BattleData ApplyAbility(string[] lines)
            {
                var sideData = GetSidePos(lines[0]);
                var abilityName = lines[1];

                // 想想特性放哪里

                return battleData;
            }

            public async Task<BattleData> ApplyTerastallize(string[] lines)
            {
                var teraType = lines[1];
                var sideData = GetSidePos(lines[0]);

                var lastTurn = battleData.GetLastTurn()!;

                var sideTeam = lastTurn.SideTeam[sideData.side - 1];
                var teraType1 = await PokemonToolsWithoutDB.GetTypeAsync(teraType);


                var newPokes = sideTeam.Pokemons
                    .Select(x =>
                    x.Position == sideData.pos
                    ? x with { TeratallizeStatus = new TeraSallized(teraType1) } // 不能再太晶了
                    : x).ToImmutableArray();


                var newTurn = lastTurn with { SideTeam = lastTurn.SideTeam.SetItem(sideData.side - 1, sideTeam with { CanTerastallize = false }) };



                // 修改宝可梦的teraType
                return battleData.UpdateLastTurn(newTurn);

            }
            public BattleData ApplyDrag(string[] lines)
            {
                var sideData = GetSidePos(lines[0]);
                var pokemonName = lines[1].Split(',')[0];
                var lastTurn = battleData.GetLastTurn();
                var newPokes = lastTurn.SideTeam[sideData.side - 1].Pokemons
                    .Select(x =>
                    x.Position == sideData.pos
                    ? (x with { BattleStatus = PsBattleStatus.InBackField }).SwitchOut()
                    : x)
                    .ToImmutableArray();
                return battleData.UpdateLastTurn(lastTurn.WithUpdatedSidePokemons(sideData.side - 1, newPokes));
            }
            public BattleData ApplySingleturn(string[] lines)
            {
                var sideData = GetSidePos(lines[0]);
                var singleTurnStatus = lines[1].Split(":").Last().Replace(" ", "").Trim();
                var prop = typeof(PokemonStatus).GetProperty(singleTurnStatus);

                if (prop == null)
                {
                    Log.Logger.Error($"Unknown single turn status: {singleTurnStatus}");
                    return battleData;

                }

                else
                {
                    // 开局的turn要看好了
                    var lastTurn = battleData.BattleTurns.Last()!;
                    var status = lastTurn.SideTeam[sideData.side - 1].Pokemons.FirstOrDefault(x => x.Position == sideData.pos)!.Status with { };
                    prop.SetValue(status, 1); // 设置为1

                    var newPokes = lastTurn.SideTeam[sideData.side - 1].Pokemons
                        .Select(x => x.Position == sideData.pos
                        ? x with { Status = status }
                        : x
                    )!;

                    var newTurn = lastTurn with
                    {
                        SideTeam = lastTurn.SideTeam.SetItem(sideData.side - 1, lastTurn.SideTeam[sideData.side - 1] with { Pokemons = [.. newPokes] })
                    };
                    return battleData.UpdateLastTurn(newTurn);

                }


            }
            public BattleData ApplyDamage(string[] lines)
            {
                var hpRemain = lines[1].Split('/');
                var hpNumber = int.Parse(hpRemain[0].Replace(" fnt", ""));
                var maxHp = hpRemain.Length == 1 ? 100 : int.Parse(hpRemain[1].Split(' ')[0]);


                var sideData = GetSidePos(lines[0]);
                var lastTurn = battleData.GetLastTurn()!.UpdatePokemonHp(sideData, hpNumber * 100 / maxHp);



                return battleData.UpdateLastTurn(lastTurn);
            }



            public BattleData ApplyHeal(string[] lines)
            {
                var hpRemain = lines[1].Split('/');
                var hpNumber = int.Parse(hpRemain[0].Replace(" fnt", "")); 
                var maxHp = int.Parse(hpRemain[1].Split(' ')[0]);
                // 这两个要改 结算出百分比
                var sideData = GetSidePos(lines[0]);
                var lastTurn = battleData.GetLastTurn()!.UpdatePokemonHp(sideData, hpNumber * 100 / maxHp);


                return battleData.UpdateLastTurn(lastTurn);
            }


           

            public async Task<BattleData> ApplyMove(string[] lines)
            {
                var sideData = GetSidePos(lines[0]);
                var targetSideData = GetSidePos(lines[2]);
                var moveName = lines[1].Split(',')[0];
                var lastTurn = battleData.GetLastTurn()!;
                var move = await PokemonToolsWithoutDB.GetMoveAsync(moveName);
                var newPokes = battleData.GetLastTurn().SideTeam[sideData.side - 1].Pokemons
                    .Select(x =>
                    x.Position == sideData.pos
                    ? x with { Moves = [.. x.Moves, new GameMove(move)] }
                    : x).ToImmutableArray();

                var newTurn = lastTurn.WithUpdatedSidePokemons(sideData.side - 1, newPokes);

                return battleData.UpdateLastTurn(newTurn);
            }

            public BattleData ApplyFaint(string[] lines)
            {
                var sideData = GetSidePos(lines[0]);
                var lastTurn = battleData.GetLastTurn()!;

                var newPokes = lastTurn.SideTeam[sideData.side - 1].Pokemons
                    .Select(x =>
                    x.Position == sideData.pos
                    ? (x with { BattleStatus = PsBattleStatus.IsDead
                    //, Position = -1 
                    })//.SwitchOut()
                    : x).ToImmutableArray();

                var newTurn = lastTurn.WithUpdatedSidePokemons(sideData.side - 1, newPokes);

                return battleData.UpdateLastTurn(newTurn);
            }
            public BattleData ApplyRequest(string[] lines)
            {
                var requestData = System.Text.Json.JsonSerializer.Deserialize<RequestData>(lines[0]);

                var lastTurn = battleData.GetLastTurn()!;

                var newTurn = lastTurn with
                {
                    Requests = lastTurn.Requests.Add(requestData),
                };


                return battleData.UpdateLastTurn(newTurn);
            }

           
            public BattleData ApplyWeather(string[] lines)
            {
                var lastTurn = battleData.GetLastTurn();
                var weather = lines[0];
                if (weather == "none")
                {
                    lastTurn = lastTurn with { BattleField = lastTurn.BattleField with { Weather = Weather.None } };
                }
                else
                {
                    if (Enum.TryParse(weather, true, out Weather parsedWeather))
                    {

                        var fsDecr = (DecreaseAttribute)(typeof(BattleField).GetProperty("WeatherRemain")).GetCustomAttribute(typeof(DecreaseAttribute));

                        lastTurn = lastTurn with
                        {
                            BattleField = lastTurn.BattleField with
                            {
                                Weather = Weather.None,
                                WeatherRemain = fsDecr.InitValue // 或者max
                            }
                        };
                    }

                }
                return battleData.UpdateLastTurn(lastTurn);
            }

            public BattleData ApplySide(string[] lines, bool start)
            {
                var lastTurn = battleData.GetLastTurn()!;
                var side = lines[1].Split(":").Last().Trim().Replace(" ", "");
                var reasonLines = lines[2..];
                var sideData = GetPlayer(lines[0][..2]);

                var sideFieldProperty = typeof(OneSideBattleField).GetProperty(side);
                if (sideFieldProperty == null)
                {
                    Log.Logger.Error($"Unknown side field property: {side}");
                    return battleData;
                }
                var fsDecr = (DecreaseAttribute)sideFieldProperty.GetCustomAttribute(typeof(DecreaseAttribute));
                var newSideField = lastTurn.SideField[sideData] with { };

                
                if (start)
                {
                    sideFieldProperty.SetValue(newSideField, fsDecr.InitValue); // 或者max
                }
                else
                {
                    sideFieldProperty.SetValue(newSideField, 0);
                }
                lastTurn = lastTurn with
                {
                    SideField = lastTurn.SideField.SetItem(sideData, newSideField)
                };

                return battleData.UpdateLastTurn(lastTurn);
            }

            public BattleData ApplyField(string[] lines, bool start)
            {
                // yysy // 这里后面还有产生原因


                var lastTurn = battleData.GetLastTurn()!;
                var field = lines[0].Split(":").Last().Trim().Replace(" ", "");
                var reasonLines = lines[1..];
                if (field.EndsWith("Terrain"))
                {
                    if (Enum.TryParse(field, true, out Terrain terrain))
                    {
                        var fsDecr = (DecreaseAttribute)(typeof(BattleField).GetProperty("TerrainRemain")).GetCustomAttribute(typeof(DecreaseAttribute));

                        lastTurn = lastTurn with
                        {
                            BattleField = lastTurn.BattleField with
                            {
                                Terrain = terrain,
                                TerrainRemain = fsDecr.InitValue // 或者max
                            }
                        };
                    }
                }
                else
                {
                    var fieldroperty = typeof(BattleField).GetProperty(field);
                    if (fieldroperty == null)
                    {
                        Log.Logger.Error($"Unknown field property: {field}");
                        return battleData;
                    }
                    var fsDecr = (DecreaseAttribute)fieldroperty.GetCustomAttribute(typeof(DecreaseAttribute));
                    lastTurn = lastTurn with { };
                    if (start)
                    {
                        fieldroperty.SetValue(lastTurn.BattleField, fsDecr.InitValue); // 或者max
                    }
                    else
                    {
                        fieldroperty.SetValue(lastTurn.BattleField, 0);
                    }
                }



                return battleData.UpdateLastTurn(lastTurn);
            }
            public BattleData ApplyStatus(string[] lines, bool start)
            {
                var sideData = GetSidePos(lines[0]);
                var status = typeof(PokemonStatus).GetProperty(
                                                           $"{new CultureInfo("en").TextInfo.ToTitleCase(lines[1].ToLower())}");


                var lastTurn = battleData.GetLastTurn()!;
                var sideTeam = lastTurn.SideTeam[sideData.side - 1];
                if (status == null)
                {
                    Log.Logger.Error($"Unknown status property: {status}");
                    return battleData;
                }

                else
                {
                    // 开局的turn要看好了


                    var status1 = lastTurn.SideTeam[sideData.side - 1].Pokemons.FirstOrDefault(x => x.Position == sideData.pos)!.Status with { }; 

                    if (start)
                    {
                        status.SetValue(status1, lines[1] == "slp" ? 3 : 1); // 设置为1
                    }
                    else
                    {
                        status.SetValue(status1, 0); // 设置为1
                    }

                    var newPokes = lastTurn.SideTeam[sideData.side - 1].Pokemons
                        .Select(x => x.Position == sideData.pos
                        ? x with { Status = status1 }
                        : x
                    )!;

                    var newTurn = lastTurn with
                    {
                        SideTeam = lastTurn.SideTeam.SetItem(sideData.side - 1, lastTurn.SideTeam[sideData.side - 1] with { Pokemons = [.. newPokes] })
                    };
                    return battleData.UpdateLastTurn(newTurn);

                }

            }
            public BattleData ApplyBoost(string[] lines, bool boost)
            {


                return battleData;
            }
            public BattleData ApplyTemplate(string[] lines)
            {


                return battleData;
            }



            public GamePokemon GetMyPokemonDetail(BattlePokemon battlePokemon)
            {
                // 获取我宝可梦的完整数据 也许不用
                return new();
            }
        }

    }



}


public class RequestData
{
    public Active[] active { get; set; }
    public Side side { get; set; }
    public int rqid { get; set; }
    public bool wait { get; set; }
}

public class Side
{
    public string name { get; set; }
    public string id { get; set; }
    public Pokemon1[] pokemon { get; set; }
}

public class Pokemon1
{
    public string ident { get; set; }
    public string details { get; set; }
    public string condition { get; set; }
    public bool active { get; set; }
    public Stats stats { get; set; }
    public string[] moves { get; set; }
    public string baseAbility { get; set; }
    public string item { get; set; }
    public string pokeball { get; set; }
    public string ability { get; set; }
    public bool commanding { get; set; }
    public bool reviving { get; set; }
    public string teraType { get; set; }
    public string terastallized { get; set; }
}

public class Stats
{
    public int atk { get; set; }
    public int def { get; set; }
    public int spa { get; set; }
    public int spd { get; set; }
    public int spe { get; set; }
}

public class Active
{
    public Move[] moves { get; set; }
    public string canTerastallize { get; set; }
}

public class Move
{
    public string move { get; set; }
    public string id { get; set; }
    public int pp { get; set; }
    public int maxpp { get; set; }
    public string target { get; set; }
    public bool disabled { get; set; }
}




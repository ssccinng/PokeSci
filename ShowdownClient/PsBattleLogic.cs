using PokeCommon.Utils;
using System.Text.Json;

namespace Showdown
{
    public partial class PSBattle
    {
        public BattleStatus BattleStatus { get; set; }

        public void LogParse(string cmd, string[] lines)
        {
            if (cmd.StartsWith('-'))
            {
                MinorActions(cmd, lines);
            }
            else
            {
                MajorActions(cmd, lines);
            }
        }

        public void MajorActions(string cmd, string[] lines)
        {
            switch (cmd)
            {
                case "move":
                    break;
                case "switch":
                    // Enemy pokemon has switched in
                    if (lines[0][1] == '1')
                    {
                        if (Side1[0] != null)
                        {
                            if (lines[0][2] == 'a')
                            {
                                //Side1[0].Faint();
                                //Side1[0].NowHp = 0;
                            }
                            else
                            {
                                //Side1[1].Faint();

                                //Side1[1].Dynamax = false;
                                //Side1[0].NowHp = 0;

                            }
                        }
                    }
                    else
                    {
                        if (Side2[0] != null)
                        {
                            if (lines[0][2] == 'a')
                            {
                                //Side2[0].Dynamax = false;
                                //Side2[0].Faint();

                            }
                            else
                            {
                                //Side2[1].Dynamax = false;
                                //Side2[1].Faint();

                            }
                        }

                    }
                    // regex = re.compile(r'p\da: (.*?)\|(.*?), (?:L(\d+), )?.*')
                    // name, variant, level = regex.match('|'.join(split_line[0:2])).groups()
                    // battle.update_enemy(name, split_line[2], variant, level if level else '100')
                    break;
                case "swap":
                    break;
                case "detailschange":
                    break;
                case "cant":
                    break;
                case "faint":
                    if (lines[0][1] == '1')
                    {
                        if (Side1[0] != null)

                            if (lines[0][2] == 'a')
                            {
                                Side1[0].Faint();
                                //Side1[0].NowHp = 0;
                            }
                            else
                            {
                                Side1[1].Faint();

                                //Side1[1].Dynamax = false;
                                //Side1[0].NowHp = 0;

                            }
                    }
                    else
                    {
                        if (Side2[0] != null)

                            if (lines[0][2] == 'a')
                            {
                                //Side2[0].Dynamax = false;
                                Side2[0].Faint();

                            }
                            else
                            {
                                //Side2[1].Dynamax = false;
                                Side2[1].Faint();

                            }
                    }
                    break;
                default:
                    break;
            }
        }
        // 针对双打
        public void MinorActions(string cmd, string[] lines)
        {
            switch (cmd)
            {
                case "-fail":
                    break;
                case "-damage":
                    // 更新血量
                    if (lines[0][1] == '1')
                    {
                        if (Side1[0] != null)
                            if (lines[0][2] == 'a')
                            {
                                //Side1[0].NowHp = 0;
                            }
                            else
                            {
                                //Side1[1].Dynamax = true;
                            }
                    }
                    else
                    {
                        if (Side2[0] != null)

                            if (lines[0][2] == 'a')
                            {
                                //Side2[0].Dynamax = true;

                            }
                            else
                            {
                                //Side2[1].Dynamax = true;

                            }
                    }
                    // if battle.player_id not in split_line[0]:
                    // name = re.match(r'p\da: (.*)', split_line[0]).group(1)
                    // battle.update_enemy(name, split_line[1])
                    break;
                case "-heal":
                    break;
                case "-status":
                    // battle.update_status(battle.get_team(split_line[0]).active(), split_line[1])
                    break;
                case "-curestatus":
                    // battle.update_status(battle.get_team(split_line[0]).active())
                    break;
                case "-cureteam":
                    break;
                case "-boost":
                    // battle.set_buff(battle.get_team(split_line[0]).active(), split_line[1], int(split_line[2]))
                    break;
                case "-unboost":
                    // battle.set_buff(battle.get_team(split_line[0]).active(), split_line[1], - int(split_line[2]))
                    break;
                case "-weather":
                    var weather = lines[0];
                    if (weather == "none")
                    {
                        NowTurn.AllField.Weather = Weather.None;
                        return;
                    }
                    break;
                case "-fieldstart":
                    // battle.fields.append(split_line[0])
                    // print("** " + battle.fields)
                    break;
                case "-fieldend":
                    // battle.fields.remove(split_line[0])
                    // print("** " + battle.fields)
                    break;
                case "-sidestart":
                    // battle.side_condition.append(split_line[1])
                    // print("** " + battle.side_condition)
                    break;
                case "-sideend":
                    // battle.side_condition.remove(split_line[1])
                    // print("** " + battle.side_condition)
                    break;
                case "-crit":
                    break;
                case "-supereffective":
                    break;
                case "-resisted":
                    break;
                case "-immune":
                    break;
                case "-item":
                    // battle.get_team(split_line[0]).active().item = split_line[1].lower().replace(" ", "")
                    break;
                case "-enditem":
                    // battle.get_team(split_line[0]).active().item = None
                    break;
                case "-ability":
                    break;
                case "-endability":
                    break;
                case "-transform":
                    break;
                case "-mega":
                    break;
                case "-activate":
                    break;
                case "-hint":
                    break;
                case "-center":
                    break;
                case "-message":
                    break;
                case "-start":
                    //Console.WriteLine(lines[0]);
                    //Console.WriteLine(lines[1]);
                    //if (lines[2] == "Dynamax") { }
                    if (lines[0][1] == '1')
                    {
                        if (Side1[0] != null)
                            if (lines[0][2] == 'a')
                            {
                                Side1[0].Dynamax = true;
                            }
                            else
                            {
                                Side1[1].Dynamax = true;
                            }
                    }
                    else
                    {
                        if (Side2[0] != null)

                            if (lines[0][2] == 'a')
                            {
                                Side2[0].Dynamax = true;

                            }
                            else
                            {
                                Side2[1].Dynamax = true;

                            }
                    }
                    // 通过nickname判断
                    // dynamax状态
                    break;
                case "-end":
                    //Console.WriteLine(lines[0]);
                    //Console.WriteLine(lines[1]);
                    if (lines[0][1] == '1')
                    {
                        if (Side1[0] != null)

                            if (lines[0][2] == 'a')
                            {
                                Side1[0].Dynamax = false;
                            }
                            else
                            {
                                Side1[1].Dynamax = false;
                            }
                    }
                    else
                    {
                        if (Side2[0] != null)

                            if (lines[0][2] == 'a')
                            {
                                Side2[0].Dynamax = false;

                            }
                            else
                            {
                                Side2[1].Dynamax = false;

                            }
                    }
                    break;
                default:
                    break;
            }
        }

        public async Task RefreshByRequestAsync(string request)
        {
            var data = JsonDocument.Parse(request).RootElement;
            // 获取操作id
            if (data.TryGetProperty("rqid", out var jsonId))
            {
                Turn = jsonId.GetInt32();
                // // Console.WriteLine(Turn);
            }


            if (data.TryGetProperty("side", out var side))
            {
                //return;
                if (side.TryGetProperty("id", out var pid))
                {
                    if (pid.GetString() == "p1")
                    {
                        PlayerPosition = PlayerPosition.Player1;
                    }
                    else
                    {
                        PlayerPosition = PlayerPosition.Player2;

                    }
                    // // Console.WriteLine(Turn);
                }
                // Console.WriteLine("side");
                var pokes = side.GetProperty("pokemon");
                // Console.WriteLine(pokes.GetArrayLength());

                // side没问题？？
                for (int i = 0; i < pokes.GetArrayLength(); i++)
                {
          
                    var detail = pokes[i].GetProperty("details").GetString().Split(", ");

 
                    string[] condition = pokes[i].GetProperty("condition").GetString().Split('/');
                    int hp = 0;
                    int maxhp = 100;
                    if (condition.Length > 1)
                    {
                        hp = int.Parse(condition[0]);
                        maxhp = int.Parse(condition[1].Split(" ")[0]);
                    }

         
                    bool pokeActive = pokes[i].GetProperty("active").GetBoolean();
                    Actives[i] = pokeActive;
                    if (InitId == 0)
                    {
                        // 主要是这里

                        MyTeam[i] = (new PSBattlePokemon(await PokemonTools.GetPokemonFromPsNameAsync(detail[0]), detail[0]));


                        // 要记录nickname 虽然也可自取
                    }

                    else
                    {
                        for (int j = 0; j < MyTeam.Count; j++)
                        {
                            if (MyTeam[j].PSName == detail[0])
                            {
                                (MyTeam[i], MyTeam[j]) = (MyTeam[j], MyTeam[i]);
                                break;
                            }
                        }

                        // 只需更新状态
                    }
                    //MyTeam[i].Commanding = pokes[i].GetProperty("commanding").GetBoolean();

                    // todo: 暂不用commanding

                    //MyTeam[i].MetaPokemon = await PokemonTools.GetPokemonFromPsNameAsync(detail[0]);
                    //MyTeam[i] = (new BattlePokemon(await PokemonTools.GetPokemonFromPsNameAsync(detail[0])));
                    MyTeam[i].NowHp = hp;
                    if (MyTeam[i].MaxHP == 0)
                        MyTeam[i].MaxHP = maxhp;



                }
                for (int i = 0; i < MySide.Length; i++)
                {
                    MySide[i] = MyTeam[i];
                }
                InitId++;
            }
            if (data.TryGetProperty("active", out var active))
            {
                for (int i = 0; i < active.GetArrayLength(); i++)
                {
                    ActiveStatus[i] = active[i];
                }
                // 更新技能信息
            }
            if (data.TryGetProperty("forceSwitch", out var jsongFSwitch))
            {
                bool[] fArray;
                if (jsongFSwitch.ValueKind == JsonValueKind.Array)
                {
                    fArray = new bool[jsongFSwitch.GetArrayLength()];
                    for (int i = 0; i < jsongFSwitch.GetArrayLength(); i++)
                    {
                        fArray[i] = jsongFSwitch[i].GetBoolean();
                    }
                    // Console.WriteLine(fArray.Length);

                }
                else
                {
                    fArray = new bool[] { jsongFSwitch.GetBoolean() };
                }
                // 可能不是array
                // Console.WriteLine("检测到需要换人");
                // 需要换人
                OnForceSwitch?.Invoke(this, fArray);
            }
            else if (data.TryGetProperty("teamPreview", out var tt))
            {
                OnTeampreview?.Invoke(this);
            }
            else
            {
                OnChooseMove?.Invoke(this);

            }
            //else
            //{
            //    Client.OnChooseMove?.Invoke(this, );

            //}

        }

        public async Task OrderTeamAsync(string order)
        {
            await Client.SendTeamOrderAsync(Tag, order, Turn);
        }

        public async Task SendMoveAsunc(ChooseData[] chooseDatas)
        {
            await Client.SendMoveAsync(Tag, Turn, chooseDatas);
        }
        public async Task LeaveRoomAsync()
        {
            await Client.SendLeaveAsync(Tag);
        }
        public async Task ForfeitAsync()
        {
            await Client.SendForfeitAsync(Tag);
        }
        public async Task SendTimerOnAsync()
        {
            await Client.SendAsync(Tag, "/timer on");
        }

        int oppTeamIdx = 0;
        public async Task InitOppTeamAsync(string name)
        {
            name = name.Replace("-*", "");
            OppTeam[oppTeamIdx++] = new PSBattlePokemon(await PokemonToolsWithoutDB.GetPokemonFromPsNameAsync(name), name);
        }
    }
    public enum BattleStatus
    {
        Waiting,
        Requests,
        Error,
        End,
        TurnFinish,
        GameFinish,
    }
}

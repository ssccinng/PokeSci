﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokePSCore
{
    public partial class PsBattle
    {
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
                    string opPos = PlayerPos == PlayerPos.Player1 ? "p2" : "p1";
                    if (lines[0] == opPos)
                    {
                        
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
                    break;
                default:
                    break;
            }
        }
        public void MinorActions(string cmd, string[] lines)
        {
            switch (cmd)
            {
                case "-fail":
                    break;
                case "-damage":
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
                    // battle.weather = split_line[0]
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
                default:
                    break;
            }
        }
    }
}

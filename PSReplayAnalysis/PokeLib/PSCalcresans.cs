using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSReplayAnalysis.PokeLib
{
    public class resstruct
    {
        public string pokemon = null;
        public string item = null;
        public string terrain = null;
        public string move = null;
        public string weather = null;
        public string ability = null;

        public bool auroraveil = false;
        public bool reflect = false;
        public bool lightscreen = false;
        public bool friendguard = false;
        public bool protect = false;
        public bool helpinghand = false;
        public bool foresight = false;
        public bool gravity = false;
        public bool crit = false;
        public bool burn = false;
        public bool battery = false; // 蓄电池
        public bool dynamax = false; // 极具化

        public int spaplus = 0;
        public int spdplus = 0;
        public int atkplus = 0;
        public int defplus = 0;

        public int stat = 0;
        public int movepow = 0;

        public Racial IVs = new Racial(-1);

        public bool miehuo;
        public bool miecao;
        public bool mieshui;
        public bool mieyan;



        public string damagenum = null;
        public string damagebfb = null;
        public string damagedec = null;

        public int Lv;

    }
    public class PSCalcresans
    {
        public static resstruct[,] dmgcodetodata(string text)
        {
            string[] kadigo = Regex.Split(text.Trim(), "\r*\n");
            resstruct[,] ret = new resstruct[kadigo.Length, 2];
            for (int i = 0; i < kadigo.Length; ++i)
            {
                if (kadigo[i] == "") continue;
                resstruct[] tt = new resstruct[2];
                tt = dmgcodetodata_once(kadigo[i].Trim());
                ret[i, 0] = tt[0];
                ret[i, 1] = tt[1];
            }
            return ret;
        }

        public static resstruct anc(string text)
        {
            resstruct qs = new resstruct();
            string heidi = text;
            string sci = "";
            bool sword = false;



            if (heidi[0] == '-' || heidi[0] == '+')
            {
                sci = heidi.Split(' ')[0];
                heidi = heidi.Substring(3);
                qs.stat = int.Parse(sci);
            }

            if (heidi.Substring(0, 4) == "Lvl ")
            {
                heidi = heidi.Substring(heidi.Split(' ')[0].Length + 1);

                qs.Lv = int.Parse(heidi.Split(' ')[0]);
                heidi = heidi.Substring(heidi.Split(' ')[0].Length + 1);
            }


            if (heidi.Contains("SpA"))
            {
                sword = true;
                sci = Regex.Split(heidi, " SpA ")[0];
                heidi = Regex.Split(heidi, " SpA ")[1];
                if (sci.Contains("+"))
                {

                    qs.spaplus = 1;
                    sci = sci.Substring(0, sci.Length - 1);
                }
                else if (sci.Contains("-"))
                {
                    qs.spaplus = -1;
                    sci = sci.Substring(0, sci.Length - 1);
                }

                qs.IVs.SetSpa(int.Parse(sci));
            }
            else if (heidi.Contains("Atk"))
            {
                sword = true;
                sci = Regex.Split(heidi, " Atk ")[0];
                heidi = Regex.Split(heidi, " Atk ")[1];
                if (sci.Contains("+"))
                {

                    qs.atkplus = 1;
                    sci = sci.Substring(0, sci.Length - 1);
                }
                else if (sci.Contains("-"))
                {
                    qs.atkplus = -1;
                    sci = sci.Substring(0, sci.Length - 1);
                }

                qs.IVs.SetAtk(int.Parse(sci));
            }
            else if (heidi.Contains("Def"))
            {
                sci = Regex.Split(heidi, " Def ")[0];
                heidi = Regex.Split(heidi, " Def ")[1];
                string[] moob = Regex.Split(sci, " / ");
                qs.IVs.SetHP(int.Parse(moob[0].Split(' ')[0]));
                if (moob[1].Contains("+"))
                {

                    qs.defplus = 1;
                    moob[1] = moob[1].Substring(0, moob[1].Length - 1);
                }
                else if (moob[1].Contains("-"))
                {
                    qs.defplus = -1;
                    moob[1] = moob[1].Substring(0, moob[1].Length - 1);
                }
                qs.IVs.SetDef(int.Parse(moob[1]));
            }
            else
            {

                sci = Regex.Split(heidi, " SpD ")[0];
                heidi = Regex.Split(heidi, " SpD ")[1];
                string[] moob = Regex.Split(sci, " / ");
                qs.IVs.SetHP(int.Parse(moob[0].Split(' ')[0]));
                if (moob[1].Contains("+"))
                {

                    qs.spdplus = 1;
                    moob[1] = moob[1].Substring(0, moob[1].Length - 1);
                }
                else if (moob[1].Contains("-"))
                {
                    qs.spdplus = -1;
                    moob[1] = moob[1].Substring(0, moob[1].Length - 1);
                }
                qs.IVs.SetSpf(int.Parse(moob[1]));
            }
            sci = "";
            while (heidi != "")
            {
                if (sci != "")
                {
                    sci += " ";
                }
                if (heidi.Contains(" "))
                {
                    sci = sci + heidi.Split(' ')[0];

                    heidi = heidi.Substring(heidi.Split(' ')[0].Length + 1);
                }
                else
                {
                    sci += heidi;
                    heidi = "";
                }
                if (Pokemondata.EngItemID[sci] != null)
                {
                    qs.item = sci;
                    sci = "";
                    continue;
                }
                if (Pokemondata.EngAbilityID[sci] != null)
                {
                    qs.ability = sci;
                    sci = "";
                    continue;
                }
                if (sci == "protected")
                {
                    qs.protect = true;
                    sci = "";
                    continue;
                }
                if (sci == "Dynamax")
                {
                    qs.dynamax = true;
                    sci = "";
                    continue;
                }

                if (sci == "burned")
                {
                    qs.burn = true;
                    sci = "";
                    continue;
                }
                string asfl = sci;
                int asfl1 = 0;
                while (true)
                {
                    if (Pokemondata.EngPokemonID[asfl] != null)
                    {
                        qs.pokemon = asfl;
                        asfl1 = 1;
                        break;
                    }
                    if (asfl.Contains('-'))
                    {
                        asfl = asfl.Substring(0, asfl.LastIndexOf('-'));
                    }
                    else
                    {
                        break;
                    }
                }
                if (asfl1 == 1) break;

            }
            if (sword)
            {
                sci = "";
                while (heidi != "")
                {
                    if (sci != "")
                    {
                        sci += " ";
                    }
                    if (heidi.Contains(" "))
                    {
                        sci = sci + heidi.Split(' ')[0];

                        heidi = heidi.Substring(heidi.Split(' ')[0].Length + 1);
                    }
                    else
                    {
                        sci += heidi;
                        heidi = "";
                    }

                    if (sci == "Helping Hand")
                    {
                        qs.helpinghand = true;
                        sci = "";
                        continue;
                    }
                    if (sci == "Battery boosted")
                    {
                        qs.battery = true;
                        sci = "";
                        continue;
                    }
                    if (Pokemondata.EngMoveID[sci.Split('(')[0]] != null && (heidi == "" || Pokemondata.EngMoveID[(sci + " " + heidi.Split(' ')[0]).Split('(')[0]] == null))
                    {
                        qs.move = sci.Split('(')[0];
                        if (heidi.Contains("BP"))
                        {
                            qs.movepow = int.Parse(heidi.Split('B')[0].Substring(1));
                        }
                        break;
                    }

                }
            }
            else
            {
                if (heidi.Contains("through Light Screen"))
                {
                    qs.lightscreen = true;
                }

                if (heidi.Contains("through Reflect"))
                {
                    qs.reflect = true;
                }
                if (heidi.Contains("with an ally's Friend Guard"))
                {
                    qs.friendguard = true;
                }
                if (heidi.Contains("with an ally's Aurora Veil"))
                {
                    qs.auroraveil = true;
                }
            }
            return qs;
        }
        // Lvl 1 252+ SpA Abomasnow Helping Hand Max Overgrowth vs. Lvl 50 140 HP / 0 SpD protected Dynamax Abomasnow in Grassy Terrain with an ally's Friend Guard with an ally's Aurora Veil: 1-1 (0.2 - 0.2%) -- guaranteed 2HKO after Stealth Rock, Steelsurge, 3 layers of Spikes, sandstorm damage, Leech Seed damage, Leech Seed recovery, Grassy Terrain recovery, Wildfire damage, Cannonade damage, and Volcalith damage
        // 252+ SpA Abomasnow Battery boosted Fire Spin vs. 140 HP / 0 SpD Abomasnow: 204-240 (57.3 - 67.4%) -- guaranteed 2HKO after trapping damage
        public static resstruct[] dmgcodetodata_once(string text)
        {
            string[] icefairy = Regex.Split(text, " *vs. *");

            string[] scixing = Regex.Split(icefairy[1], " *-- *");
            string[] ret = new string[] { icefairy[0], scixing[0], scixing[1] };
            resstruct[] p = new resstruct[] { new resstruct(), new resstruct() };

            p[0] = anc(ret[0]);
            p[1] = anc(ret[1].Split(':')[0]);

            p[0].damagedec = p[1].damagedec = scixing[1];
            //p[0].damagenum = p[1].damagenum = ret[1].Split(':')[1].Split('(')[0].Substring(1);
            p[0].damagenum = p[1].damagenum = ret[1].Split(':')[1].Split('(')[0].Trim();
            p[0].damagebfb = p[1].damagebfb = ret[1].Split(':')[1].Split('(')[1].Substring(0, ret[1].Split(':')[1].Split('(')[1].Length - 1);

            string[] wealist = new string[] { "Sun", "Rain", "Sand", "Hail", "Harsh Sunshine", "Heavy Rain", "Strong Winds" };
            string[] weachi = new string[] { "大晴天", "下雨", "沙暴", "冰雹", "大日照", "暴雨", "乱气流" };
            int index = 0;
            foreach (string wea in wealist)
            {
                string heidijj = "in " + wea;
                if (ret[1].Contains(heidijj))
                {
                    p[0].weather = p[1].weather = weachi[index];

                }
                index += 1;
            }
            string[] tealist = new string[] { "Electric", "Grassy", "Misty", "Psychic" };
            string[] teachi = new string[] { "电气场地", "青草场地", "薄雾场地", "精神场地" };
            index = 0;
            foreach (string tea in tealist)
            {
                string heidijj = "in " + tea + " terrain";
                if (ret[1].Contains(heidijj))
                {
                    p[0].terrain = p[1].terrain = teachi[index];
                }
                index += 1;
            }
            if (ret[1].Contains("on a critical hit"))
            {
                p[0].crit = p[1].crit = true;
            }
            return p;
        }
    }
}

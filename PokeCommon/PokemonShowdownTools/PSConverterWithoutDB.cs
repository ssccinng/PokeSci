﻿using PokeCommon.Models;
using PokeCommon.Utils;
using System.Text.RegularExpressions;
using System.Text;

namespace PokeCommon.PokemonShowdownTools
{
    public class PSConverterWithoutDB
    {
        private static Regex reg = new("[ -]");
        static string GetLowerLetter(string val)
        {
            return reg.Replace(val, "").ToLower();
        }
        public static async ValueTask<string> ConvertToPsAsync(GamePokemon gamePokemon)
        {
            StringBuilder sb = new();
            if (gamePokemon == null || gamePokemon.MetaPokemon == null)
            {
                return "";
            }
            // 思考昵称加在哪里
            // 这里要获取PS名字
            sb.Append((await PokemonToolsWithoutDB.GetPsPokemonAsync(gamePokemon.MetaPokemon.Id))?.PSName);
            if (gamePokemon.Gmax) sb.Append("-Gmax");
            if (gamePokemon.Item != null) sb.Append($" @ {gamePokemon.Item.Name_Eng}");
            sb.AppendLine();
            if (gamePokemon.Ability != null) sb.AppendLine($"Ability: {gamePokemon.Ability.Name_Eng}");
            sb.AppendLine($"Level: {gamePokemon.LV}");
            if (gamePokemon.TreaType != null)
            {
                sb.AppendLine("Tera Type: " + gamePokemon.TreaType.Name_Eng);

            }
            if (gamePokemon.Happiness != 160) sb.AppendLine($"Happiness: {gamePokemon.Happiness}");
            string[] orz = { "HP", "Atk", "Def", "SpA", "SpD", "Spe" };
            bool flag = false;
            if (gamePokemon.EVs.ToSixArray().Any(s => s != 0))
            {
                sb.Append("EVs: ");
                for (int i = 0; i < 6; i++)
                {
                    if (gamePokemon.EVs[i] > 0)
                    {
                        if (flag)
                        {
                            sb.Append(" / ");
                        }
                        else
                        {
                            flag = true;
                        }
                        sb.Append($"{gamePokemon.EVs[i]} {orz[i]}");
                    }
                }
                sb.AppendLine();
            }



            if (gamePokemon.Shiny)
            {
                sb.AppendLine("Shiny: Yes");
            }
            if (gamePokemon.Nature != null)
            {
                sb.AppendLine($"{gamePokemon.Nature.Name_Eng} Nature");
            }

            flag = false;
            if (gamePokemon.IVs.ToSixArray().Any(s => s != 31))
            {
                sb.Append("IVs: ");
                for (int i = 0; i < 6; i++)
                {
                    if (gamePokemon.IVs[i] != 31)
                    {
                        if (flag)
                        {
                            sb.Append(" / ");
                        }
                        else
                        {
                            flag = true;
                        }
                        sb.Append($"{gamePokemon.IVs[i]} {orz[i]}");
                    }
                }
                sb.AppendLine();
            }
            foreach (var move in gamePokemon.Moves)
            {
                if (move != null)
                {
                    // 这个地方 用id特判
                    if (move.NameChs.StartsWith("觉醒力量"))
                    {

                    }
                    else
                    {
                        sb.AppendLine($"- {move.NameEng}");
                    }
                }

            }
            return sb.ToString();
        }

        public static async ValueTask<string> ConvertToPsAsync(GamePokemonTeam gamePokemonTeam)
        {
            var sb = new StringBuilder();
            foreach (var item in gamePokemonTeam.GamePokemons)
            {
                sb.AppendLine(await ConvertToPsAsync(item));
                sb.AppendLine();
            }
            return sb.ToString().Trim();
        }
        /// <summary>
        /// 转换PS格式文本至单独的宝可梦
        /// </summary>
        /// <param name="PStext"></param>
        /// <returns></returns>
        public static async Task<GamePokemon?> ConvertToPokemonAsync(string PStext)
        {

            PStext = PStext.Trim();
            string[] data = Regex.Split(PStext, "\r*\n");
            (string pokeName, string nickName, string itemName) = GetNameAndItem(data[0]);
            bool gmax = false;
            if (pokeName.EndsWith("-Gmax"))
            {
                pokeName = pokeName[..^5];
                gmax = true;
            }
            var poke = await PokemonToolsWithoutDB.GetPokemonFromPsNameAsync(pokeName);
            if (poke == null)
            {
                // 返回空宝可梦还是一个null
                return new GamePokemon();
            }
            GamePokemon gamePokemon = new(poke);
            gamePokemon.Gmax = gmax;
            gamePokemon.TreaType = poke.Type1;
            gamePokemon.NickName = nickName;
            if (itemName != null)
            {
                gamePokemon.Item = await PokemonToolsWithoutDB.GetItemAsync(itemName);
            }
            //gamePokemon.Item = await PokemonToolsWithoutDB.GetItemAsync(itemName);
            for (int i = 1; i < data.Length; i++)
            {
                string[] temp = Regex.Split(data[i].Trim(), @"\s*:\s*");
                switch (temp[0])
                {
                    case "Ability":
                        gamePokemon.Ability = await PokemonToolsWithoutDB.GetAbilityAsync(Regex.Split(temp[1], @"\s+\(")[0]);
                        break;
                    case "Gigantamax":
                        gamePokemon.Gmax |= (temp[1] == "Yes");
                        break;
                    case "Tera Type":
                        gamePokemon.TreaType = (await PokemonToolsWithoutDB.GetTypeAsync(temp[1]));
                        //PB.TreaType = Pokemondata.GetTypeName(PB.TreaTypeId);
                        break;
                    case "IVs":
                        string[] IVs = Regex.Split(temp[1], @"\s*/\s*");
                        foreach (string iv in IVs)
                        {
                            string[] ivtemp = Regex.Split(iv, @"\s+");
                            // gamePokemon.IVs[(int)Enum.Parse(typeof(SixDimensionValueType), ivtemp[0])] = int.Parse(ivtemp[1]);
                            gamePokemon.IVs.SetValue(ivtemp[1], int.Parse(ivtemp[0]));

                        }
                        break;
                    case "EVs":
                        string[] EVs = Regex.Split(temp[1], @"\s*/\s*");
                        foreach (string ev in EVs)
                        {
                            string[] evtemp = Regex.Split(ev, @"\s+");
                            // gamePokemon.IVs[(int)Enum.Parse(typeof(SixDimensionValueType), ivtemp[0])] = int.Parse(ivtemp[1]);
                            gamePokemon.EVs.SetValue(evtemp[1], int.Parse(evtemp[0]));

                        }
                        break;
                    case "Level":
                        gamePokemon.LV = int.Parse(temp[1]);
                        break;
                    case "Shiny":
                        gamePokemon.Shiny = temp[1] == "Yes";
                        break;
                    case "Happiness":
                        break;
                    default:
                        string[] temp1 = data[i].Trim().Split(' ');
                        if (temp1.Length > 1 && temp1[1] == "Nature")
                        {
                            gamePokemon.Nature = await PokemonToolsWithoutDB.GetNatureAsync(temp1[0]) ?? (await PokemonToolsWithoutDB.GetNatureAsync(1)!);
                        }
                        else if (temp1[0][0] == '-')
                        {
                            //string move1 = Regex.Replace(data[i].Trim(), @"\s*-\s+", "");
                            string move1 = data[i][1..].Trim();

                            if (string.IsNullOrEmpty(move1)) continue;

                            if (move1.Contains("Hidden Power"))
                            {

                            }
                            else
                            {
                                gamePokemon.Moves.Add(new GameMove(await PokemonToolsWithoutDB.GetMoveAsync(move1)));
                            }
                            //if (move1.Contains("Hidden Power"))
                            //{
                            //    if (move1.Contains("["))
                            //    {
                            //        string wqeeq = move1.Substring(move1.IndexOf("[")
                            //            + 1, move1.Length - move1.IndexOf("[") - 2);
                            //        move1 = "觉醒力量-" + Pokemondata.GetTypeName
                            //            (Pokemondata.GetEngTypeId(move1.Substring(move1.IndexOf("[")
                            //            + 1, move1.Length - move1.IndexOf("[") - 2)));
                            //    }
                            //    else
                            //    {
                            //        move1 = "觉醒力量-" + Pokemondata.GetTypeName(Pokemondata.GetEngTypeId(IVsfin.getHiddenPowerType()));
                            //    }
                            //    if (move == "") move = move1;
                            //    else move += "," + (move1);
                            //}
                            //else
                            //{
                            //    if (move == "") move = Pokemondata.GetMoveName(Pokemondata.EnglishNametoMoveID(move1));
                            //    else move += "," + Pokemondata.GetMoveName(Pokemondata.EnglishNametoMoveID(move1));
                            //}
                        }
                        break;
                }
            }
            //Console.WriteLine(gamePokemon.NickName);
            //Console.WriteLine(gamePokemon.Ability.Name_Chs);
            return gamePokemon;
        }

        public static async Task<GamePokemonTeam> ConvertToPokemonsAsync(string PStext, int pad = 0)
        {
            string[] team = Regex.Split(PStext.Trim(), "(?:\\r*\n){2,}", RegexOptions.IgnoreCase);
            GamePokemonTeam gamePokeTeam = new();
            //for (int i = 0; i < team.Length; i++)
            //{
            //    gamePokeTeam.GamePokemons.Add(null);
            //}
            //Parallel.For(0, team.Length, i => gamePokeTeam.GamePokemons[i] =  ConvertToPokemonAsync(team[i]).Result);
            //Parallel.ForEach(team, async poke => gamePokeTeam.GamePokemons.Add(await ConvertToPokemonAsync(poke)));
            foreach (var poke in team)
            {
                gamePokeTeam.GamePokemons.Add(await ConvertToPokemonAsync(poke));
            }

            while (gamePokeTeam.GamePokemons.Count < pad)
            {
                gamePokeTeam.GamePokemons.Add(new GamePokemon());
            }

            return gamePokeTeam;
        }

        public static async ValueTask<string> ConvertToPsOneLineAsync(GamePokemonTeam gamePokemonTeam)
        {
            List<string> psoneline = new();
            Console.WriteLine(gamePokemonTeam.GamePokemons.Count);
            for (int i = 0; i < gamePokemonTeam.GamePokemons.Count; i++)
            {
                //Console.WriteLine(1);
                psoneline.Add(await ConvertToPsOneLineAsync(gamePokemonTeam.GamePokemons[i]));
                //Console.WriteLine($"({psoneline[i]})");
            }

            return string.Join("]", psoneline);
        }

        /// <summary>
        /// 转换为一行ps格式
        /// </summary>
        /// <param name="gamePokemon"></param>
        /// <returns></returns>
        public static async ValueTask<string> ConvertToPsOneLineAsync(GamePokemon gamePokemon)
        {

            if (gamePokemon == null || gamePokemon.MetaPokemon == null)
            {
                return "";
            }
            // 0：昵称
            // 1：实际宝 如果没有昵称 这里是空的
            // 2：道具
            // 3: 特性
            // 4: 招式，中间用逗号隔开
            // 5：性格，有大写
            // 6：努力值，中间用逗号隔开 全0则为空
            // 7：性别
            // 8：个体值，中间用逗号隔开 全31则为空
            // 9：是否闪光
            // 10：登记
            string oneLineFormat = "{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}]";

            string[] data = new string[12];
            for (int i = 0; i < 12; i++)
            {
                data[i] = "";
            }

            StringBuilder sb = new();
            if (gamePokemon.NickName != null)
            {
                data[0] = gamePokemon.NickName;
                data[1] = GetLowerLetter((await PokemonToolsWithoutDB.GetPsPokemonAsync(gamePokemon.MetaPokemon.Id))?.PSName);
            }
            else
            {
                data[0] = GetLowerLetter((await PokemonToolsWithoutDB.GetPsPokemonAsync(gamePokemon.MetaPokemon.Id))?.PSName);
            }

            if (gamePokemon.Item != null)
            {
                data[2] = GetLowerLetter(gamePokemon.Item.Name_Eng);
            }

            if (gamePokemon.Ability != null)
            {
                data[3] = GetLowerLetter(gamePokemon.Ability.Name_Eng);
            }

            if (gamePokemon.Moves.Count > 0)
            {
                data[4] = string.Join(',', gamePokemon.Moves.Select(s => GetLowerLetter(s.NameEng)));
            }

            if (gamePokemon.Nature != null)
            {
                data[5] = gamePokemon.Nature.Name_Eng;
            }

            if (gamePokemon.EVs.Sum > 0)
            {
                data[6] = string.Join(',', gamePokemon.EVs.ToSixArray().Select(s => s > 0 ? s.ToString() : ""));
            }


            data[7] = gamePokemon.Gender switch
            {
                Gender.Female => "F",
                Gender.Male => "M",
                _ => "",
            };

            if (gamePokemon.IVs.Sum < 186)
            {
                data[8] = string.Join(',', gamePokemon.IVs.ToSixArray().Select(s => s < 31 ? s.ToString() : ""));
            }
            if (gamePokemon.Shiny)
            {
                data[9] = "S";
            }
            if (gamePokemon.LV != 50)
            {
                data[10] = gamePokemon.LV.ToString();

            }
            if (gamePokemon.TreaType != null)
            {
                data[10] = $",,,,,{gamePokemon.TreaType.Name_Eng}";

            }
            // if (gamePokemon.)
            // 思考昵称加在哪里
            // 这里要获取PS名字
            return string.Join("|", data);
        }

        private static (string Name, string NickName, string Item) GetNameAndItem(string data)
        {
            var NI = Regex.Split(data.Trim(), @"\s+@\s+"); // 昵称
            string item = null;
            string name = NI[0];
            string nickname = null;

            if (NI.Length > 1)
            {
                item = NI[1].Trim();
            }
            string[] ntemp = Regex.Split(name, @"\s+\(");

            if (ntemp.Length == 3)
            {
                nickname = ntemp[0];
                name = ntemp[1][..^1];
            }
            else if (ntemp.Length == 2)
            {
                if (ntemp[1].Length <= 4)
                {
                    name = ntemp[0];
                }
                else
                {
                    name = ntemp[1][..^1];
                    nickname = ntemp[0];
                }
            }

            return (name, nickname, item);
        }

    }
}

using PokeCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
using PokeCommon.Utils;

namespace PokeCommon.PokemonShowdownTools
{
    public class PSConverter
    {
        public static string ConvertToPs(GamePokemon gamePokemon)
        {
            return null;
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
            var poke = await PokemonTools.GetPsPokemonAsync(pokeName);
            if (poke == null)
            {
                // 返回空宝可梦还是一个null
                return null;
            }
            GamePokemon gamePokemon = new(poke);
            gamePokemon.Gmax = gmax;
            gamePokemon.NickName = nickName;
            gamePokemon.Item = await PokemonTools.GetItemAsync(itemName);
            for (int i = 1; i < data.Length; i++)
            {
                string[] temp = Regex.Split(data[i].Trim(), @"\s*:\s*");
                switch (temp[0])
                {
                    case "Ability":
                        gamePokemon.Ability = await PokemonTools.GetAbilityAsync(Regex.Split(temp[1], @"\s+\(")[0]);
                        break;
                    case "Gigantamax":
                        gamePokemon.Gmax |= (temp[1] == "Yes");
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
                        if (temp1[1] == "Nature")
                        {
                            gamePokemon.Nature = await PokemonTools.GetNatureAsync(temp1[0]) ?? (await PokemonTools.GetNatureAsync(1)!);
                        }
                        else if(temp1[0][0] == '-')
                        {
                            //string move1 = Regex.Replace(data[i].Trim(), @"\s*-\s+", "");
                            string move1 = data[i][1..].Trim();
                            if (move1.Contains("Hidden Power"))
                            {

                            }
                            else
                            {
                                gamePokemon.Moves.Add(new GameMove(await PokemonTools.GetMoveAsync(move1)));
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

        public static async Task<GamePokemonTeam> ConvertToPokemonsAsync(string PStext)
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
            return gamePokeTeam;
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

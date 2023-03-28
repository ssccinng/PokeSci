using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PSReplayAnalysis.PokeLib
{

    class Pokemondata
    {
        static int a = 1;

        public static string[] Type = { null, "火", "水", "草", "电", "一般", "格斗", "飞行", "虫", "毒", "岩石", "地面", "钢", "冰", "超能力", "恶", "幽灵", "龙", "妖精" };
        public static int[,] Typecolor = GetTypecolor();
        public static string[,] Typestringcolor = GetTypecolorstring();

        public static double[,] Typeeffect = TypetoType();


        private static string[] EngType = { null, "Fire", "Water", "Grass", "Electric", "Normal", "Fighting", "Flying", "Bug", "Poison", "Rock", "Ground", "Steel", "Ice", "Psychic", "Dark", "Ghost", "Dragon", "Fairy" };
        public static string[] Ability = database.Ability.Split(',');
        public static string[] EnglishName = database.EnglishName.Split(',');

        public static string[] AbilityEngName = database.AbilityEngName.Split(',');

        //public static string[] MoveName = database.MoveName.Split(',');
        //public static string[] MoveEngName = database.MoveEngName.Split(',');
        public static string[] MoveName;
        public static string[] MoveEngName;
        public static string[] ItemName = database.ItemName.Split(',');
        public static string[] ItemEngName = database.ItemEngName.Split(',');

        public static Hashtable TypeId = DeHashtable(Type);
        public static Hashtable EngTypeId = DeHashtable(EngType);
        public static Hashtable EngAbilityID = DeHashtable(AbilityEngName);

        public static Hashtable EngItemID = DeHashtable(ItemEngName);
        public static Hashtable ItemID = DeHashtable(ItemName);

        public static string Pokedatasour = database.Pokedata;
        public static string[,] Pokedata = Pokedatainit();
        public static MOVE[] MOVEDATA = Movedatainit();
        public static Hashtable MoveID = DeHashtable(MoveName);
        public static Hashtable EngMoveID = DeHashtable(MoveEngName);

        public static Hashtable pokemonListId = PokemonListIDinit(); // 通过中文找到内部id
        public static Hashtable PokemonID = PokemonIDinit(); // 通过中文找到真图鉴id
        public static Hashtable PokemonEngID = PokemonEngIDinit(); // 通过英文文找到真图鉴id 未完成

        public static Hashtable PokemonIDdexID; // 通过英文文找到真图鉴id 未完成


        public static Hashtable PokemonnameID = PokemonnameIDinit(); //  通过中文找到内部id ？ 怎么重复了
        public static Hashtable EngPokemonID = DeHashtable(EnglishName);   //  通过英文找到内部id ？ 怎么重复了

        public static string[] PokeNamelist = PokeNamelistinit(); // 分开 加入英文

        public static string[] dexpokename = Pokedexnameinit(); // 图鉴id找中文名
        public static string[] engdexpokename; // 图鉴id找英文名
        public static Hashtable AbilityId = DeHashtable(Ability);
        public static PokemonBase[] POKEMON = Pokemoninit();
        public static NatureClass[] naturedata = NatureClassinit();
        public static string[] Engnature = { "Hardy", "Lonely", "Brave", "Adamant", "Naughty", "Bold", "Docile", "Relaxed",
            "Impish", "Lax", "Timid", "Hasty", "Serious", "Jolly", "Naive", "Modest", "Mild", "Quiet", "Bashful",
            "Rash", "Calm", "Gentle", "Sassy", "Careful", "Quirky" };
        public static Hashtable EngnatureId = DeHashtable(Engnature);





        private static Hashtable DeHashtable(string[] a)
        {
            Hashtable temp = new Hashtable();
            int index = 0;
            foreach (string item in a)
            {
                if (item != null)
                    temp[item] = index;
                index++;
            }
            return temp;
        }

        private static string[] Pokedexnameinit()
        {
            string[] poke = new string[PokeNamelist.Length];
            engdexpokename = new string[PokeNamelist.Length];
            for (int i = 1; i < PokeNamelist.Length; ++i)
            {
                var data1 = (PokeNamelist[i] ?? "").Split(',');
                poke[i] = data1.Length > 0 ? data1[0] : "";
                var aa = GetPokemonListID(poke[i]);
                if (aa == -1) aa = 1;
                engdexpokename[i] = EnglishName[aa];

            }
            return poke;
        }


        private static string[,] GetTypecolorstring()
        {
            string aaa = @"火,9,F08030,9C531F
水,10,6890F0,445E9C
草,11,78C850,4E8234
电,12,F8D030,A1871F
一般,0,A8A878,6D6D4E
格斗,1,C03028,7D1F1A
飞行,2,A890F0,6D5E9C
虫,6,A8B820,6D7815
毒,3,A040A0,682A68
岩石,5,B8A038,786824
地面,4,E0C068,927D44
钢,8,B8B8D0,787887
冰,14,98D8D8,638D8D
超能力,13,F85888,A13959
恶,16,705848,49392F
幽灵,7,705898,493963
龙,15,7038F8,4924A1
妖精,17,EE99AC,9B6470";
            string[] qq = aaa.Split('\n');
            string[,] res = new string[19, 2];

            for (int i = 0; i < 18; ++i)
            {
                string[] qqq = qq[i].Split(',');
                res[i + 1, 0] = qqq[2].Trim();
                res[i + 1, 1] = qqq[3].Trim();
            }

            return res;
        }

        private static int[,] GetTypecolor()
        {
            string aaa = @"火,9,F08030,9C531F
水,10,6890F0,445E9C
草,11,78C850,4E8234
电,12,F8D030,A1871F
一般,0,A8A878,6D6D4E
格斗,1,C03028,7D1F1A
飞行,2,A890F0,6D5E9C
虫,6,A8B820,6D7815
毒,3,A040A0,682A68
岩石,5,B8A038,786824
地面,4,E0C068,927D44
钢,8,B8B8D0,787887
冰,14,98D8D8,638D8D
超能力,13,F85888,A13959
恶,16,705848,49392F
幽灵,7,705898,493963
龙,15,7038F8,4924A1
妖精,17,EE99AC,9B6470";
            string[] qq = aaa.Split('\n');
            int[,] res = new int[19, 2];

            for (int i = 0; i < 18; ++i)
            {
                string[] qqq = qq[i].Split(',');
                res[i + 1, 0] = Convert.ToInt32("0x" + qqq[2].Trim(), 16);
                res[i + 1, 1] = Convert.ToInt32("0x" + qqq[3].Trim(), 16);
            }

            return res;
        }
        private static double[,] TypetoType()
        {
            string ff = "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0\n0,0.5,0.5,2,1,1,1,1,2,1,0.5,1,2,2,1,1,1,0.5,1\n0,2,0.5,0.5,1,1,1,1,1,1,2,2,1,1,1,1,1,0.5,1\n0,0.5,2,0.5,1,1,1,0.5,0.5,0.5,2,2,0.5,1,1,1,1,0.5,1\n0,1,2,0.5,0.5,1,1,2,1,1,1,0,1,1,1,1,1,0.5,1\n0,1,1,1,1,1,1,1,1,1,0.5,1,0.5,1,1,1,0,1,1\n0,1,1,1,1,2,1,0.5,0.5,0.5,2,1,2,2,0.5,2,0,1,0.5\n0,1,1,2,0.5,1,2,1,2,1,0.5,1,0.5,1,1,1,1,1,1\n0,0.5,1,2,1,1,0.5,0.5,1,0.5,1,1,0.5,1,2,2,0.5,1,0.5\n0,1,1,2,1,1,1,1,1,0.5,1,0.5,0,1,1,1,0.5,1,2\n0,2,1,1,1,1,0.5,2,2,1,1,0.5,0.5,2,1,1,1,1,1\n0,2,1,0.5,2,1,1,0,0.5,2,2,1,2,1,1,1,1,1,1\n0,0.5,0.5,1,0.5,1,1,1,1,1,2,1,0.5,2,1,1,1,1,2\n0,0.5,0.5,2,1,1,1,2,1,1,1,2,0.5,0.5,1,1,1,2,1\n0,1,1,1,1,1,2,1,1,2,1,1,0.5,1,0.5,0,1,1,1\n0,1,1,1,1,1,0.5,1,1,1,1,1,1,1,2,0.5,2,1,0.5\n0,1,1,1,1,0,1,1,1,1,1,1,1,1,2,0.5,2,1,1\n0,1,1,1,1,1,1,1,1,1,1,1,0.5,1,1,1,1,2,0\n0,0.5,1,1,1,1,2,1,1,0.5,1,1,0.5,1,1,2,1,2,1";

            double[,] list = new double[19, 19];
            string[] zxh = ff.Split('\n');
            int i = 0;
            foreach (string zx in zxh)
            {
                string[] hh = zx.Split(',');
                int j = 0;
                foreach (string cc in hh)
                {
                    list[i, j] = double.Parse(cc);
                    j++;
                }
                i++;
            }
            return list;
        }
        private static string[,] Pokedatainit()
        {
            string[] data = Regex.Split(Pokedatasour, "\r*\n");
            string[,] Pokedata = new string[data.Length, 13];
            int index = 0;
            foreach (string item in data)
            {
                try
                {
                    string[] poke = item.Split(',');
                    for (int i = 0; i < 13; ++i)
                    {
                        Pokedata[index, i] = poke[i];
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    throw;
                }

                index++;
            }
            return Pokedata;
        }


        private static MOVE[] Movedatainit()
        {
            MoveName = Regex.Split(database.MoveName, ",");
            MoveEngName = Regex.Split(database.MoveEngName, ",");

            string[] data = Regex.Split(database.Movedata, "\n");
            MOVE[] Pokedata = new MOVE[data.Length + 100];
            int index = 1;
            //MoveName = new string[data.Length + 1];
            //MoveEngName = new string[data.Length + 1];
            foreach (string item in data)
            {
                if (index % 50 == 0)
                {
                    int teag = 5641654;

                }
                string[] poke = item.Split('\t');
                Pokedata[index] = new MOVE(poke);
                //MoveName[index] = Pokedata[index].chiname;
                //MoveEngName[index] = Pokedata[index].engname;
                index++;
            }
            while (index < Pokedata.Length)
            {
                Pokedata[index++] = new(data[0].Split('\t'));
            }
            return Pokedata;
        }
        private static void Movedatainit1()
        {
            MoveName = Regex.Split(database.MoveEngName, "\n");
            MoveEngName = Regex.Split(database.MoveName, "\n");
            //MOVE[] Pokedata = new MOVE[data.Length + 1];
            //int index = 1;
            //MoveName = new string[data.Length + 1];
            //MoveEngName = new string[data.Length + 1];
            //foreach (string item in data)
            //{
            //    if (index % 50 == 0)
            //    {
            //        int teag = 5641654;
            //    }
            //    string[] poke = item.Split('\t');
            //    Pokedata[index] = new MOVE(poke);
            //    MoveName[index] = Pokedata[index].chiname;
            //    MoveEngName[index] = Pokedata[index].engname;
            //    index++;
            //}
            //return Pokedata;
        }
        private static Hashtable PokemonIDinit()
        {
            Hashtable PokemonID = new Hashtable();
            PokemonIDdexID = new Hashtable();
            for (int i = 0; i < Pokedata.GetLength(0); ++i)
            {
                PokemonID[Pokedata[i, 0]] = int.Parse(Pokedata[i, 12]);
                PokemonIDdexID[i] = int.Parse(Pokedata[i, 12]);
            }
            return PokemonID;
        }
        private static Hashtable PokemonEngIDinit()
        {
            Hashtable PokemonID = new Hashtable();
            for (int i = 0; i < Pokedata.GetLength(0); ++i)
            {
                PokemonID[EnglishName[GetPokemonListID(Pokedata[i, 0])]] = int.Parse(Pokedata[i, 12]);
            }
            return PokemonID;
        }

        private static Hashtable PokemonnameIDinit()
        {
            Hashtable PokemonID = new Hashtable();

            for (int i = 0; i < Pokedata.GetLength(0); ++i)
            {
                PokemonID[Pokedata[i, 0]] = i;
            }
            return PokemonID;
        }

        private static Hashtable EngPokemonIDinit()
        {
            Hashtable PokemonID = new Hashtable();
            for (int i = 0; i < EnglishName.Length; ++i)
            {
                PokemonID[EnglishName[i]] = i;
            }
            return PokemonID;
        }

        private static string[] PokeNamelistinit()
        {
            int max = 0;
            foreach (object item in PokemonID.Values)
            {
                max = Max(max, (int)item);
            }
            string[] PokeNamelist = new string[max + 1];
            for (int i = 0; i < Pokedata.GetLength(0); ++i)
            {
                if (PokeNamelist[int.Parse(Pokedata[i, 12])] == null)
                {
                    PokeNamelist[int.Parse(Pokedata[i, 12])] = Pokedata[i, 0];
                }
                else
                {
                    PokeNamelist[int.Parse(Pokedata[i, 12])] += "," + Pokedata[i, 0];
                }
            }
            return PokeNamelist;
        }

        private static int Max(int max, int item)
        {
            return max > item ? max : item;
        }

        private static PokemonBase[] Pokemoninit()
        {
            PokemonBase[] POKEMON = new PokemonBase[Pokedata.GetLength(0)];
            int[] index = new int[PokeNamelist.Length];
            for (int i = 0; i < index.Length; ++i)
            {
                index[i] = 0;
            }
            for (int i = 0; i < Pokedata.GetLength(0); ++i)
            {
                Racial temp = new Racial(Pokedata[i, 3], Pokedata[i, 4], Pokedata[i, 5], Pokedata[i, 6], Pokedata[i, 7],
                        Pokedata[i, 8]);
                int[] TypeList = { int.Parse(Pokedata[i, 1]), int.Parse(Pokedata[i, 2]) };
                int[] AbilityList = { int.Parse(Pokedata[i,9]), int.Parse(Pokedata[i,10]),
                    int.Parse(Pokedata[i,11]) };
                POKEMON[i] = new PokemonBase(Pokedata[i, 0], int.Parse(Pokedata[i, 12]),
                        PokeNamelist[int.Parse(Pokedata[i, 12])].Split(','), TypeList, AbilityList,
                        index[int.Parse(Pokedata[i, 12])], temp);
                index[int.Parse(Pokedata[i, 12])]++;
            }
            return POKEMON;
        }

        private static Hashtable PokemonListIDinit()
        {
            Hashtable pokemonlistId = new Hashtable();
            for (int i = 0; i < Pokedata.GetLength(0); ++i)
            {
                pokemonlistId[Pokedata[i, 0]] = i;
            }
            return pokemonlistId;
        }

        private static NatureClass[] NatureClassinit()
        {
            NatureClass[] Naturelist = new NatureClass[26];
            int i = 0;
            foreach (string s in database.Naturedata)
            {
                string[] ss = s.Split(',');
                Naturelist[i++] = new NatureClass(ss[0], int.Parse(ss[1]), int.Parse(ss[2]),
                        int.Parse(ss[3]), i - 1);
            }
            return Naturelist;
        }


        /// <summary>
        /// 从中文获取图鉴id
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static int GetPokemonID(string Name)
        {
            if (Name == null || Name == "" || !PokemonID.ContainsKey(Name)) return -1;
            return (int)PokemonID[Name];
        }
        /// <summary>
        /// 从图鉴id获取形态列表
        /// </summary>
        /// <param name="PkId"></param>
        /// <returns></returns>
        public static string[] GetPokemonFrom(int PkId)
        {

            return PokeNamelist[PkId].Split(',');
        }
        /// <summary>
        /// 从中文名获取内部id
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public static int GetPokemonListID(string Name)
        {
            if (Name == null || Name == "") return -1;
            return (int)pokemonListId[Name];
        }
        /// <summary>
        /// 从中文获取宝可梦属性id列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int[] GetPokemonType(string name)
        {
            return GetPokemonType(GetPokemonID(name));
        }
        /// <summary>
        /// 从id获取宝可梦属性id列表
        /// </summary>
        /// <param name="PkId"></param>
        /// <returns></returns>
        public static int[] GetPokemonType(int PkId)
        {
            return POKEMON[PkId].TypeId;
        }
        /// <summary>
        /// 从图鉴标准中文获取形态列表
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string[] GetPokemonFrom(string name)
        {
            return GetPokemonFrom(GetPokemonID(name));
        }

        /// <summary>
        /// 通过id获取属性名字
        /// </summary>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        public static string GetTypeName(int TypeId)
        {
            return Type[TypeId];
        }
        /// <summary>
        /// 通过id列表获取属性名字列表
        /// </summary>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        public static string[] GetTypeName(int[] TypeId)
        {
            string[] Typename = new string[TypeId.Length];
            for (int i = 0; i < TypeId.Length; ++i)
            {
                Typename[i] = GetTypeName(TypeId[i]);
            }
            return Typename;
        }
        /// <summary>
        /// 通过中文名获取属性id
        /// </summary>
        /// <param name="Typename"></param>
        /// <returns></returns>
        public static int GetTypeId(string Typename)
        {
            return (int)TypeId[Typename];
        }
        /// <summary>
        /// 通过中文名列表获取属性id列表
        /// </summary>
        /// <param name="Typename"></param>
        /// <returns></returns>
        public static int[] GetTypeId(string[] Typename)
        {
            int[] TypeId = new int[Typename.Length];
            for (int i = 0; i < Typename.Length; ++i)
            {
                TypeId[i] = GetTypeId(Typename[i]);
            }
            return TypeId;
        }
        /// <summary>
        /// 通过id获取属性英文
        /// </summary>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        public static string GetEngTypeName(int TypeId)
        {
            return EngType[TypeId];
        }
        /// <summary>
        /// 通过id列表获取属性英文列表
        /// </summary>
        /// <param name="TypeId"></param>
        /// <returns></returns>
        public static string[] GetEngTypeName(int[] TypeId)
        {
            string[] EngTypename = new string[TypeId.Length];
            for (int i = 0; i < TypeId.Length; ++i)
            {
                EngTypename[i] = GetEngTypeName(TypeId[i]);
            }
            return EngTypename;
        }
        /// <summary>
        /// 通过英文获取属性id
        /// </summary>
        /// <param name="Typename"></param>
        /// <returns></returns>
        public static int GetEngTypeId(string Typename)
        {
            return (int)EngTypeId[Typename];
        }
        /// <summary>
        /// 通过英文列表获取属性id列表
        /// </summary>
        /// <param name="Typename"></param>
        /// <returns></returns>
        public static int[] GetEngTypeId(string[] Typename)
        {
            int[] EngTypeId = new int[Typename.Length];
            for (int i = 0; i < Typename.Length; ++i)
            {
                EngTypeId[i] = GetEngTypeId(Typename[i]);
            }
            return EngTypeId;
        }
        /// <summary>
        /// 通过特性中文名获取特性id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetAbilityId(string name)
        {
            if (AbilityId.Contains(name))
                return (int)AbilityId[name];
            return 0;
        }
        /// <summary>
        /// 通过id获取特性中文名
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static string GetAbilityName(int Id)
        {
            return Ability[Id];
        }

        /// <summary>
        /// 通过中文名获取道具id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetItemId(string name)
        {
            if (ItemID.Contains(name))
                return (int)ItemID[name];
            return 0;
        }
        /// <summary>
        /// 通过中文名获取技能id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static int GetMoveId(string name)
        {
            if (MoveID.Contains(name))
                return (int)MoveID[name];
            return 0;
        }
        /// <summary>
        /// 通过id获取技能中文名
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static string GetMoveName(int Id)
        {
            return MoveName[Id];
        }
        /// <summary>
        /// 通过id获取道具中文名
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static string GetItemName(int Id)
        {
            return ItemName[Id];
        }

        public static PokemonBase GetPokemonBase(int Id)
        {

            if (Id == -1) return null;
            if (Id < POKEMON.Length)
                return POKEMON[Id];
            else
                return null;
        }

        public static PokemonBase GetPokemonBase(string name)
        {
            if (pokemonListId.Contains(name))
                return GetPokemonBase(GetPokemonListID(name));
            return null;
        }

        public static NatureClass getNatureClass(int id)
        {
            return naturedata[id];
        }

        public static NatureClass getNatureClass(string name)
        {
            return getNatureClass(GetEngnatureId(name));
        }

        public static int GetEngnatureId(string name)
        {
            if (EngnatureId.Contains(name))
                return (int)EngnatureId[name];
            return 0;
        }

        public static PokemonInfo GetpPokemonUSE(string name)
        {
            if (pokemonListId.Contains(name))
            {
                PokemonInfo b = new PokemonInfo(GetPokemonBase(name));
                b.Pokemonextend();
                return b;
            }
            return null;
        }

        public static int EnglishNametopokeID(string name)
        {
            return (int)EngPokemonID[name];
        }

        public static int EnglishNametoAbilityID(string name)
        {
            return (int)EngAbilityID[name];
        }

        public static int EnglishNametoMoveID(string name)
        {
            return (int)EngMoveID[name];
        }

        public static int EnglishNametoItemID(string name)
        {
            return (int)EngItemID[name];
        }

        public static PokemonBase[] Getpokemon()
        {
            return POKEMON;
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PokeShitLib
{

    public class database
    {
        public static string readToString(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);
            StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312"));

            string re = sr.ReadToEnd();
            sr.Close();
            fs.Close();
            fs.Dispose();

            
            return re;
        }

        public static string ReadFile(string fileName) => File.ReadAllText(fileName, Encoding.UTF8);

        public static string Ability = ReadFile("txtdata/ability.txt");
        public static string AbilityEngName = ReadFile("txtdata/abilityengname.txt");
        public static string EnglishName = ReadFile("txtdata/pokeengname.txt");
        public static string ItemEngName = ReadFile("txtdata/itemengname.txt");
        public static string ItemName = ReadFile("txtdata/itemname.txt");
        public static string MoveEngName = ReadFile("txtdata/moveengname.txt");
        public static string MoveName = ReadFile("txtdata/movename.txt");
        //public static string Pokedata = readToString("pokedata");
        public static string Pokedata = ReadFile("txtdata/pokedata.txt");
        public static string[] Naturedata = "勤奋,0,0,0\n怕寂寞,1,2,5\n勇敢,1,5,3\n固执,1,3,2\n顽皮,1,4,4\n大胆,2,1,1\n坦率,0,0,0\n悠闲,2,5,3\n淘气,2,3,2\n乐天,2,4,4\n胆小,5,1,1\n急躁,5,2,5\n认真,0,0,0\n爽朗,5,3,2\n天真,5,4,4\n内敛,3,1,1\n慢吞吞,3,2,5\n冷静,3,5,3\n害羞,0,0,0\n马虎,3,4,4\n温和,4,1,1\n温顺,4,2,5\n自大,4,5,3\n慎重,4,3,2\n浮躁,0,0,0".Split('\n');
        public static string Movedata = ReadFile("txtdata/movedata.txt");
    }
}

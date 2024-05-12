using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.Utils
{
    public static class ColorHelper
    {
        public static string[,] TypeColorString = GetTypecolorstring();
        private static string[,] GetTypecolorstring()
        {
            string aaa =

@"一般,0,A8A878,6D6D4E
格斗,1,C03028,7D1F1A
飞行,2,A890F0,6D5E9C
毒,3,A040A0,682A68
地面,4,E0C068,927D44
岩石,5,B8A038,786824
虫,6,A8B820,6D7815
幽灵,7,705898,493963
钢,8,B8B8D0,787887
火,9,F08030,9C531F
水,10,6890F0,445E9C
草,11,78C850,4E8234
电,12,F8D030,A1871F
超能力,13,F85888,A13959
冰,14,98D8D8,638D8D
龙,15,7038F8,4924A1
恶,16,705848,49392F
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
    }
}

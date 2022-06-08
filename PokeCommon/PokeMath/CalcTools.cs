using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.PokeMath
{
    public static class Calcer
    {
        /// <summary>
        /// 获取招式几ko的概率
        /// </summary>
        /// <param name="damage"></param>
        /// <returns></returns>
        public static int GetNKoRate(int hp, int damage)
        {
            int[] damageList = new int[16];
            for (int i = 85; i <= 100; ++i)
            {
                damageList[i - 85] = damage * i / 100;
            }
            return 0;
        }
    }
}

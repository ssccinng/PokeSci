using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.Interface
{
    public interface IStatusCalc
    {
        /// <summary>
        /// 计算HP能力值
        /// </summary>
        /// <param name="baseHP"></param>
        /// <param name="IV"></param>
        /// <param name="EV"></param>
        /// <param name="lv"></param>
        /// <returns></returns>
        public int GetHP(int baseHP, int IV, int EV, int lv = 50);
        /// <summary>
        /// 计算其他的基础能力值
        /// </summary>
        /// <param name="baseValue"></param>
        /// <param name="IV"></param>
        /// <param name="EV"></param>
        /// <param name="lv"></param>
        /// <param name="natureRevise"></param>
        /// <returns></returns>
        public int GetOtherStat(int baseValue, int IV, int EV, int lv = 50, double natureRevise = 1);
        /// <summary>
        /// 获取等效HP种族值
        /// </summary>
        /// <param name="statValue"></param>
        /// <param name="IV"></param>
        /// <param name="lv"></param>
        /// <returns></returns>
        public int GetPureBaseHP(int statValue, int IV, int EV, int lv = 50);
        /// <summary>
        /// 获取等效其他能力种族值
        /// </summary>
        /// <param name="statValue"></param>
        /// <param name="IV"></param>
        /// <param name="lv"></param>
        /// <returns></returns>
        public int GetPureBaseOtherStat(int statValue, int IV, int EV, int lv = 50);
    }
}

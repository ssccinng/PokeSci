using PokeMath.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeMath
{
    internal class CalcUnits
    {
    }

    public class CalcTools : IStatusCalc
    {
        public virtual int GetHP(int baseHP, int IV, int EV, int lv = 50)
        {
            if (baseHP == 1) return 1;
            return ((int)(baseHP + IV + Math.Sqrt(EV) / 8f) * lv) / 50 + 10 + lv;
        }

        public virtual int GetOtherStat(int baseValue, int IV, int EV, int lv = 50, double natureRevise = 1)
        {
            return ((int)(baseValue + IV + Math.Sqrt(EV) / 8f) * lv) / 50 + 5;
        }

        public virtual int GetPureBaseHP(int statValue, int IV, int EV, int lv = 50)
        {
            throw new NotImplementedException();
        }

        public virtual int GetPureBaseOtherStat(int statValue, int IV, int EV, int lv = 50)
        {
            throw new NotImplementedException();
        }
    }

    public class SWSHTools: CalcTools
    {
        public override int GetHP(int baseHP, int IV, int EV, int lv = 50)
        {
            if (baseHP == 1) return 1;
            return ((int)(baseHP * 2 + IV + EV / 4f) * lv) / 100 + 10 + lv;
            //return base.GetHP(baseHP, EV, IV);
        }

        public override int GetOtherStat(int baseValue, int IV, int EV, int lv = 50, double natureRevise = 1)
        {
            return (int)((
                (int)(baseValue * 2 + IV + EV / 4f) * 
                lv / 100.0 + 5 + lv
                ) * natureRevise
                );
        }

        public override int GetPureBaseHP(int statValue, int IV, int EV = 0, int lv = 50)
        {
            if (statValue == 1) return 1;
            double temp = statValue - 10 - lv;
            temp *= 100;
            temp /= lv;

            temp -= EV / 4f + IV;
            temp /= 2;
            return (int)temp;
        }

        public override int GetPureBaseOtherStat(int statValue, int IV, int EV = 0, int lv = 50)
        {
            double temp = statValue - 5;
            temp *= 100;
            temp /= lv;
            temp -= EV / 4f + IV;
            temp /= 2;
            return (int)temp;
        }
    }


}

using PokeCommon.Interface;
using PokeCommon.Models;

namespace PokeCommon.PokeMath
{
    public static class CalcUnits
    {
        public static SWSHCalc SWSHTools = new SWSHCalc();
    }

    public class CalcTools : IStatusCalc, IDamageCalc
    {
        public int GetDamage(int lv, int atk, int def, int power, DamageEff? eff = null)
        {
            eff ??= new DamageEff();
            int dmg = (int)((2.0 * lv / 5 + 2) * power * (atk / def) / 50 + 2);
            dmg = (int)(dmg * eff.Targets);
            dmg = (int)(dmg * eff.Weather);
            dmg = (int)(dmg * eff.Badge);
            dmg = (int)(dmg * eff.Critical);
            dmg = (int)(dmg * eff.Random);
            dmg = (int)(dmg * eff.STAB);
            dmg = (int)(dmg * eff.Type);
            dmg = (int)(dmg * eff.Burn);
            dmg = Math.Max(1, dmg);
            dmg = (int)(dmg * eff.Other);
            return dmg;
        }

        public virtual int GetHP(int baseHP, int IV, int EV, int lv = 50)
        {
            if (baseHP == 1) return 1;
            return (int)(baseHP + IV + Math.Sqrt(EV) / 8f) * lv / 50 + 10 + lv;
        }

        public virtual int GetOtherStat(int baseValue, int IV, int EV, int lv = 50, double natureRevise = 1)
        {
            return (int)(baseValue + IV + Math.Sqrt(EV) / 8f) * lv / 50 + 5;
        }

        public virtual int GetPureBaseHP(int statValue, int IV, int EV, int lv = 50)
        {
            throw new NotImplementedException();
        }

        public virtual int GetPureBaseOtherStat(int statValue, int IV, int EV, int lv = 50, double natureRevise = 1)
        {
            throw new NotImplementedException();
        }
    }

    public class SWSHCalc : CalcTools
    {
        public override int GetHP(int baseHP, int IV, int EV, int lv = 50)
        {
            if (baseHP == 1) return 1;
            return (int)(baseHP * 2 + IV + EV / 4f) * lv / 100 + 10 + lv;
            //return base.GetHP(baseHP, EV, IV);
        }

        public override int GetOtherStat(int baseValue, int IV, int EV, int lv = 50, double natureRevise = 1)
        {
            return (int)((
                (int)(baseValue * 2 + IV + EV / 4f) *
                lv / 100.0 + 5
                ) * natureRevise
                );
        }
        // 再加一个性格
        public override int GetPureBaseHP(int statValue, int IV, int EV = 0, int lv = 50)
        {
            if (statValue == 1) return 1;
            double temp = statValue - 10 - lv;
            temp *= 100;
            temp /= lv;

            temp -= EV / 4f + IV;
            temp /= 2;
            return (int)Math.Ceiling(temp);
        }

        public override int GetPureBaseOtherStat(int statValue, int IV, int EV = 0, int lv = 50, double natureRevise = 1)
        {
            double temp = statValue / natureRevise - 5;
            temp *= 100;
            temp /= lv;
            temp -= EV / 4f + IV;
            temp /= 2;
            return (int)Math.Ceiling(temp);
        }

        public int GetEVHP(int statValue, int baseValue, int IV, int lv = 50)
        {
            if (statValue == 1) return 1;
            double temp = statValue - 10 - lv;
            temp *= 100;
            temp /= lv;

            temp -= baseValue * 2 + IV;
            Console.WriteLine(temp);
            temp *= 4;
            return (int)Math.Ceiling(temp);
        }
        /// <summary>
        /// 通过能力反推种族
        /// </summary>
        /// <param name="statValue"></param>
        /// <param name="baseValue"></param>
        /// <param name="IV"></param>
        /// <param name="lv"></param>
        /// <param name="natureRevise"></param>
        /// <returns></returns>
        public int GetEVOtherStat(int statValue, int baseValue, int IV, int lv = 50, double natureRevise = 1)
        {
            double temp = statValue / natureRevise - 5;
            temp *= 100;
            temp /= lv;
            temp -= baseValue * 2 + IV;
            temp *= 4;
            return (int)Math.Ceiling(temp);
        }


    }




}

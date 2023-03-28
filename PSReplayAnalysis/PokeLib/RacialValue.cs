using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSReplayAnalysis.PokeLib
{
    public class Racial
    {
        public int[] Value = new int[6];
        public int GetSumRacial()
        {
            return Value[0] + Value[1] + Value[2] + Value[3] + Value[4] + Value[5];
        }
        public Racial()
        {
            clear();
        }
        public Racial(int all)
        {
            fill(all);
        }
        public string getHiddenPowerType()
        {
            string rtn = "";
            int type = Value[0] % 2 + Value[1] % 2 * 2 + Value[2] % 2 * 4 + Value[5] % 2 * 8 + Value[3] % 2 * 16 + Value[4] % 2 * 32;
            type = type * 15 / 63;
            switch (type)
            {
                case 0: rtn = "Fighting"; break;
                case 1: rtn = "Flying"; break;
                case 2: rtn = "Poison"; break;
                case 3: rtn = "Ground"; break;
                case 4: rtn = "Rock"; break;
                case 5: rtn = "Bug"; break;
                case 6: rtn = "Ghost"; break;
                case 7: rtn = "Steel"; break;
                case 8: rtn = "Fire"; break;
                case 9: rtn = "Water"; break;
                case 10: rtn = "Grass"; break;
                case 11: rtn = "Electric"; break;
                case 12: rtn = "Psychic"; break;
                case 13: rtn = "Ice"; break;
                case 14: rtn = "Dragon"; break;
                case 15: rtn = "Dark"; break;
            }
            return rtn;
        }
        // public Racial(Racial f)
        // {
        //     SetValue(f.Value[0],f.Value[1],f.Value[2],f.Value[3],f.Value[4], f.Value[5]);
        // }
        public Racial(int HP, int Atk, int Def, int Spa, int Spf, int Spe)
        {
            SetValue(HP, Atk, Def, Spa, Spf, Spe);
        }
        public Racial(string HP, string Atk, string Def, string Spa, string Spf, string Spe)
        {
            SetValue(int.Parse(HP), int.Parse(Atk), int.Parse(Def), int.Parse(Spa), int.Parse(Spf), int.Parse(Spe));
        }
        public Racial(string[] Value)
        {
            int[] gg = new int[6];
            for (int i = 0; i < 6; ++i)
            {
                gg[i] = int.Parse(Value[i]);
            }
            SetValue(gg);
        }
        public Racial(int[] Value)
        {
            SetValue(Value);
        }
        public void SetValue(int HP, int Atk, int Def, int Spa, int Spf, int Spe)
        {
            Value[0] = HP;
            Value[1] = Atk;
            Value[2] = Def;
            Value[3] = Spa;
            Value[4] = Spf;
            Value[5] = Spe;
        }
        public void SetValue(int[] Value)
        {
            for (int i = 0; i < 6; ++i)
            {
                this.Value[i] = Value[i];
            }
        }
        public void SetHP(int x)
        {
            Value[0] = x;
        }
        public void SetAtk(int x)
        {
            Value[1] = x;
        }
        public void SetDef(int x)
        {
            Value[2] = x;
        }
        public void SetSpa(int x)
        {
            Value[3] = x;
        }
        public void SetSpf(int x)
        {
            Value[4] = x;
        }
        public void SetSpe(int x)
        {
            Value[5] = x;
        }
        public void FromnumToSet(int x, int y)
        {
            Value[x] = y;
        }
        public void clear()
        {
            for (int i = 0; i < 6; ++i)
            {
                Value[i] = 0;
            }
        }

        public void fill()
        {
            for (int i = 0; i < 6; ++i)
            {
                Value[i] = 31;
            }
        }

        public void fill(int temp)
        {
            for (int i = 0; i < 6; ++i)
            {
                Value[i] = temp;
            }
        }
    }
}
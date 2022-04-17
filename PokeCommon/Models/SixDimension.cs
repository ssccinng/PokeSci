using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.Models
{
    public enum SixDimensionValueType
    {
        HP,
        Atk,
        Def,
        Spa,
        Spd,
        Spe,
        All,
    }
    /// <summary>
    /// 六维数值
    /// </summary>
    public class SixDimension
    {

        public SixDimension(int value = 0)
        {
            SetAllValue(value);
        }

        public SixDimension(int hp = 0, int atk = 0, int def = 0, int spa = 0, int spd = 0, int spe = 0)
        {
            HP = hp;
            Atk = atk;
            Def = def;
            Spa = spa;
            Spd = spd;
            Spe = spe;
        }
        public int this[int Index]
        {
            get => Index switch
            {
                0 => HP,
                1 => Atk,
                2 => Def,
                3 => Spa,
                4 => Spd,
                5 => Spe,
                _ => 0
            };
            set
            {
                switch (Index)
                {
                    case 0: HP = value; 
                        //ValueChange?.Invoke(SixDimensionValueType.HP); 
                        break;
                    case 1: Atk = value; 
                        //ValueChange?.Invoke(SixDimensionValueType.Atk); 
                        break;
                    case 2: Def = value; 
                        //ValueChange?.Invoke(SixDimensionValueType.Def); 
                        break;
                    case 3: Spa = value; 
                        //ValueChange?.Invoke(SixDimensionValueType.Spa); 
                        break;
                    case 4: Spd = value; 
                        //ValueChange?.Invoke(SixDimensionValueType.Spd);
                        break;
                    case 5: Spe = value; 
                        //ValueChange?.Invoke(SixDimensionValueType.Spe); 
                        break;
                    default:
                        break;
                }
                
            }
        }
        /// <summary>
        /// HP  
        /// </summary>
        public int HP { get; set; }
        /// <summary>
        /// 攻击
        /// </summary>
        public int Atk { get; set; }
        /// <summary>
        /// 防御
        /// </summary>
        public int Def { get; set; }
        /// <summary>
        /// 特攻
        /// </summary>
        public int Spa { get; set; }
        /// <summary>
        /// 特防
        /// </summary>
        public int Spd { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        public int Spe { get; set; }
        /// <summary>
        /// 总和
        /// </summary>
        public int Sum => HP + Atk + Def + Spa + Spd + Spe;
        /// <summary>
        /// 获取liu
        /// </summary>
        /// <returns></returns>
        public int[] ToSixArray() => new[] { HP, Atk, Def, Spa, Spd, Spe };
        /// <summary>
        /// 设置全部值
        /// </summary>
        /// <param name="value"></param>
        public void SetAllValue(int value)
        {
            HP = Atk = Def = Spa = Spd = Spe = value;
            //ValueChange?.Invoke(SixDimensionValueType.All);
        }
        public void SetValue(string name, int value)
        {
            switch (name.ToLower())
            {
                case "hp":
                    HP = value;
                    break;
                case "atk":
                    Atk = value;
                    break;
                case "def":
                    Def = value;
                    break;
                case "spa":
                    Spa = value;
                    break;
                case "spd":
                    Spd = value;
                    break;
                case "spe":
                    Spe = value;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 值变化事件
        /// </summary>
        //public event Action<SixDimensionValueType> ValueChange;
        /// <summary>
        /// 根据个体计算出
        /// </summary>
        /// <returns></returns>
       
    }

    /// <summary>
    /// 个体值
    /// </summary>
    public class IV: SixDimension
    {
        public IV(int value) : base(value) { }
        public IV(int hp = 0, int atk = 0, int def = 0, int spa = 0, int spd = 0, int spe = 0) : base(hp, atk, def, spa, spd, spe) { }

        public string GetHiddenPowerType()
        {
            string rtn = "";
            int type = this[0] % 2 + (this[1] % 2) * 2 + (this[2] % 2) * 4 + (this[5] % 2) * 8 + (this[3] % 2) * 16 + (this[4] % 2) * 32;
            type = (type * 15) / 63;
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
    }

    public class EV: SixDimension
    {
        public EV(int value) : base(value) { }
        public EV(int hp = 0, int atk = 0, int def = 0, int spa = 0, int spd = 0, int spe = 0): base(hp, atk, def, spa, spd, spe) { }
    }
}

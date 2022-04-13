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

        public SixDimension(int hp, int atk, int def, int spa, int spd, int spe)
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
        /// <summary>
        /// 值变化事件
        /// </summary>
        public event Action<SixDimensionValueType> ValueChange;
    }
}

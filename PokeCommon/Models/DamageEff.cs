using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.Models
{
    public enum TargetType
    {
        Single,
        Double,

    }
    public class DamageEff
    {
        /// <summary>
        /// 目标数加成
        /// </summary>
        public double Targets { get; set; } = 1;
        /// <summary>
        /// 天气加成
        /// </summary>
        public double Weather { get; set; } = 1;
        /// <summary>
        /// 徽章加成（仅限于第二世代）
        /// </summary>
        public double Badge { get; set; } = 1;
        /// <summary>
        /// 暴击加成
        /// </summary>
        public double Critical { get; set; } = 1;
        /// <summary>
        /// 随机数 0.85-1
        /// </summary>
        public double Random { get; set; } = 1;
        /// <summary>
        /// 本系加成
        /// </summary>
        public double STAB { get; set; } = 1;
        /// <summary>
        /// 属性克制加成
        /// </summary>
        public double Type { get; set; } = 1;
        /// <summary>
        /// 烧伤加成
        /// </summary>
        public double Burn { get; set; } = 1;
        /// <summary>
        /// 其他加成 特性等
        /// </summary>
        public double Other { get; set; } = 1;


    }
}

using PokeMath.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeMath.Interface
{
    public interface IDamageCalc
    {
        /// <summary>
        /// 计算基础伤害公式
        /// 攻击与防御 分别是攻击方的攻击或者特攻等（取决于招式是物理招式、特殊招式或特殊情况）
        /// 和防御方的防御或特防（取决于是物理招式、特殊招式或特殊情况）。特殊情况例如：如果攻击方招式为欺诈时，攻击取防御方的攻击。
        /// 加成 使用如下的公式计算：加成=属性一致加成×属性相克造成的倍率×击中要害的倍率×其他加成×随机数（随机数∈[0.85，1]）
        /// </summary>
        /// <param name="lv"></param>
        /// <param name="atk"></param>
        /// <param name="def"></param>
        /// <param name="power"></param>
        /// <returns></returns>
        public int GetDamage(int lv, int atk, int def, int power, DamageEff? eff = null);
    }
}

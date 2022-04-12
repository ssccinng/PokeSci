using PokeCommon.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.GameRule
{
    public interface IBattleRule
    {
        public IDamageCalc DamageCalc { get; }
    }
}

using PokeCommon.Interface;
using PokeCommon.PokeMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.GameRule
{
    public class SVGameRule : IGameRule
    {
        public static SVGameRule Default = new SVGameRule();
        public IStatusCalc StatusCalc { get; set; } = new SWSHCalc();
    }
}

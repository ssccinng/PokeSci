using PokeCommon.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.GameRule
{
    public interface IGameRule
    {
        public IStatusCalc StatusCalc { get; }
    }
}

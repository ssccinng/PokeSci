using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokePSCore
{
    public struct MoveChooseData
    {
        public int MoveId { get; set; } = 0;
        public int Target { get; set; } = -1;
        public bool Dmax { get; set; } = false;
        public MoveChooseData(int moveId, int target = -1, bool dmax = false)
        {
            MoveId = moveId;
            Target = target;
            Dmax = dmax;
        }
    }
}

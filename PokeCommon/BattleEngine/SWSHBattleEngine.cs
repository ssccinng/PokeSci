using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeBattleEngine.BattleEngines
{
    public class SWSHBattleEngine : BattleEngine
    {
        public SWSHBattleEngine(BattleVersion battleVersion) : base(battleVersion)
        {
        }

        public override SWSHBattle CreateBattle()
        {
            return new SWSHBattle(this);
        }
    }
}

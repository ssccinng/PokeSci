using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.Models
{
    public class PokeBattleInfo
    {
        public SimpleGamePokemonTeam Player1Team { get; set; }
        public SimpleGamePokemonTeam Player2Team { get; set; }

        public int Player1Index { get; set; }
        public int Player2Index { get; set; }

        public int Result { get; set; }
    }
}

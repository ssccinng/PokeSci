using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.Models
{
    public class GamePlayerTeamInfo
    {
        public string PlayerName { get; set; }
        public SimpleGamePokemon GamePokemonTeam { get; set; }
        public string Score { get; set; }
    }
}

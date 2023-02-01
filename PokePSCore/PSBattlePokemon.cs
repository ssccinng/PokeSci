using PokeCommon.Models;
using PokemonDataAccess.Models;

namespace PokePSCore
{
    public class PSBattlePokemon : BattlePokemon
    {
        public PSBattlePokemon(Pokemon pokemon, string psName) : base(pokemon)
        {
            PSName = psName;
        }
        public string PSName
        {
            get; set;
        }
        public int ActiveId { get; set; } = -1;

        public void Faint()
        {
            Dynamax = false;
            NowHp = 0;
        }

    }
}

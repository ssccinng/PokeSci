using PokeCommon.Interface;
using PokemonDataAccess.Models;

namespace PokeCommon.Models.SWSH;

public class SWSHBattlePokemon : SWSHGamePokemon, IBattlePokemon
{
    public SWSHBattlePokemon(Pokemon pokemon, EV eV = null, IV iV = null) : base(pokemon, eV, iV)
    {
    }
}
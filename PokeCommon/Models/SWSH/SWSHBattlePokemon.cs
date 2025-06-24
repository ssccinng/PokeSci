using PokeCommon.Interface;
using PokemonDataAccess.Models;

namespace PokeCommon.Models.SWSH;

public record SWSHBattlePokemon : SWSHGamePokemon, IBattlePokemon
{
    public SWSHBattlePokemon(Pokemon pokemon, EV eV = null, IV iV = null) : base(pokemon, eV, iV)
    {
    }
}
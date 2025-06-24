using PokemonDataAccess.Models;

namespace PokeCommon.Models.SWSH;

public record SWSHGamePokemon : GamePokemon
{
    // Caisikujie
    public SWSHGamePokemon(Pokemon pokemon, EV eV = null, IV iV = null) : base(pokemon, eV, iV)
    {
    }
}
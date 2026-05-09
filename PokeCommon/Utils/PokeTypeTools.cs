using PokemonDataAccess.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.Utils
{
    public record TypeEffectMap (ImmutableDictionary<int, decimal> Map)
    {
        public static TypeEffectMap Empty = new 
            TypeEffectMap(
                PokemonDBInMemory.Types.ToDictionary(t => t.Id, t => 1.0m).ToImmutableDictionary()

            );
    }
    public static class PokeTypeTools
    {
        public static TypeEffectMap GetTypeEffectMap(params IEnumerable<PokeType> types)
        {
            var map = TypeEffectMap.Empty.Map;
            foreach (var type in types)
            {
                if (!PokemonDBInMemory.TypeEffectsByTargetTypeId.TryGetValue(type.Id, out var effects))
                {
                    continue;
                }

                foreach (var kv in effects)
                {
                    map = map.SetItem(kv.Type1.Id, map[kv.Type1.Id] * kv.Effect);
                }
            }
            return new TypeEffectMap(map);
        }

        public static TypeEffectMap GetPokemonEffectMap(Pokemon poke)
        {
            if (poke is null) return TypeEffectMap.Empty;
            PokeType[] types = [poke.Type1, poke.Type2];
            return GetTypeEffectMap(types.Where(t => t != null)!);
        }

    }
}

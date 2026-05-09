using PokeCommon.Models;

namespace PokeCommon.Utils
{
    public static class PokeHomeData
    {
        public static Dictionary<int, PokeModel> WazaTable =
            PokemonDBInMemory.LoadResource<Dictionary<int, PokeModel>>("Data.Home.wazaTable.json");

        public static Dictionary<int, PokeModel> TokuseiTable =
            PokemonDBInMemory.LoadResource<Dictionary<int, PokeModel>>("Data.Home.tokuseiTable.json");

        public static Dictionary<int, PokeModel> ItemTable =
            PokemonDBInMemory.LoadResource<Dictionary<int, PokeModel>>("Data.Home.itemTable.json");

        public static List<PokeModel> SeikakuNames =
            PokemonDBInMemory.LoadResource<List<PokeModel>>("Data.Home.seikakunameall.json");
    }
}

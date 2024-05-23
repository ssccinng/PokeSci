using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeCommon.PokemonHome
{

    public class HomePokemonRankData
    {
        public int id { get; set; }
        public int form { get; set; }
    }

    public class HomeUsageItem
    {
        public string id { get; set; }
        public string val { get; set; }
    }

    public class HomePokemonRankDetailData
    {
        public Dictionary<string, RankData1> Data { get; set; }
    }

    public class RankData1
    {
        public Temoti temoti { get; set; }
        public Lose lose { get; set; }
        public Win win { get; set; }
    }

    public class Temoti
    {
        public HomeUsageItem[] waza { get; set; }
        public HomeUsageItem[] tokusei { get; set; }
        public HomeUsageItem[] seikaku { get; set; }
        public HomeUsageItem[] motimono { get; set; }
        public HomePokemonRankData[] pokemon { get; set; }
        public HomeUsageItem[] terastal { get; set; }
    }


    public class Lose
    {
        public HomePokemonRankData[] waza { get; set; }
        public HomePokemonRankData[] pokemon { get; set; }
    }





    public class Win
    {
        public HomePokemonRankData[] waza { get; set; }
        public HomePokemonRankData[] pokemon { get; set; }
    }






}

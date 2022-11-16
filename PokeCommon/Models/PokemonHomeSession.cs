using System.Text.Json.Serialization;
using PokeCommon.BattleEngine;

namespace PokeCommon.Models
{
    /// <summary>
    /// 赛季信息
    /// </summary>
    public class PokemonHomeSession
    {
        public int SeasonId
        {
            get; set;
        }
        [JsonPropertyName("name")]
        public string Name
        {
            get; set;
        }
        [JsonPropertyName("season")]
        public int Season
        {
            get; set;
        }
        [JsonPropertyName("rule")]
        public int Rule
        {
            get; set;
        }
        [JsonPropertyName("ts1")]
        public int TS1
        {
            get; set;
        }
        // [JsonPropertyName("user")]
        [JsonPropertyName("ts2")]
        public int TS2
        {
            get; set;
        }
        [JsonPropertyName("rst")]
        public int RST
        {
            get; set;
        }
        [JsonPropertyName("cnt")]
        public int TrainerCnt
        {
            get; set;
        }
        [JsonPropertyName("type")]
        public BattleType Type => Rule == 0 ? BattleType.Single : BattleType.Double;
        [JsonPropertyName("start")]
        public string Start
        {
            get; set;
        }
        [JsonPropertyName("end")]
        public string End
        {
            get; set;
        }
    }
}

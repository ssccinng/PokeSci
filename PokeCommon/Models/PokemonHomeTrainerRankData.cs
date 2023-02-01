using System.Text.Json.Serialization;

namespace PokeCommon.Models
{
    public enum LanguageType
    {
        JPN = 1,
        ENG = 2,
        FRA = 3,
        ITA = 4,
        GER = 5,
        SPA = 7,
        KOR = 8,
        CHS = 9,
        CHT = 10,
    }
    public class PokemonHomeTrainerRankData
    {
        [JsonPropertyName("rank")]
        public int Rank
        {
            get; set;
        }
        public int RatingValue
        {
            get; set;
        }
        [JsonPropertyName("rating_value")]

        public string rating_value
        {
            get => RatingValue.ToString(); set => RatingValue = int.Parse(value);
        }
        [JsonPropertyName("icon")]
        public string Icon
        {
            get; set;
        }
        [JsonPropertyName("name")]
        public string Name
        {
            get; set;
        }
        //[JsonPropertyName("lng")]
        public LanguageType LanguageType
        {
            get; set;
        }
        [JsonPropertyName("lng")]
        public string lng
        {
            get => ((int)LanguageType).ToString(); set => LanguageType = (LanguageType)int.Parse(value);
        }
    }
}

using System.Text.Json;
using System.Text.Json.Serialization;

namespace PokemonisshoniZ.Components.PokemonShowdown
{
    public class UserDetail
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("userid")]
        public string? UserId { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("avatar")]
        public string? Avatar { get; set; }
        [JsonPropertyName("group")]
        public string? Group { get; set; }
        [JsonPropertyName("autoconfirmed")]
        public bool AutoConfirmed { get; set; }

        [JsonPropertyName("rooms")]
        public JsonElement Rooms { get; set; }
    }
}

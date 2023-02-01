using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonIsshoni.Net.Shared.Models
{
    public class PCLUserTeam
    {
        public int Id
        {
            get; set;
        }
        [Column(TypeName = "varchar(270)")]
        public string UserId
        {
            get; set;
        }
        public PCLPokeTeam PokeTeam
        {
            get; set;
        }
    }
}

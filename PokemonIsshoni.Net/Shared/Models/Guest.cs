using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonIsshoni.Net.Shared.Models
{
    public class Guest
    {
        public long Id
        {
            get; set;
        }
        [Column(TypeName = "varchar(270)")]
        public string UserId
        {
            get; set;
        }
    }
}

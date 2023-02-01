using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonDataAccess.Models
{
    public class TypeEffect
    {
        public int Id
        {
            get; set;
        }

        public PokeType Type1
        {
            get; set;
        }
        public PokeType Type2
        {
            get; set;
        }

        [Column(TypeName = "decimal(2, 1)")]
        public decimal Effect
        {
            get; set;
        }

    }
}
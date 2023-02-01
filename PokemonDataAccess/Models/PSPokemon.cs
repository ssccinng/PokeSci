using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonDataAccess.Models
{
    public class PSPokemon
    {
        public int Id
        {
            get; set;
        }

        [Column(TypeName = "nvarchar(50)")]
        public string PSName
        {
            get; set;
        }
        [Column(TypeName = "nvarchar(50)")]
        public string PSImgName
        {
            get; set;
        }
        [Column(TypeName = "nvarchar(30)")]
        public string PSChsName
        {
            get; set;
        }
        public int AllValue
        {
            get; set;
        }

        public Pokemon Pokemon
        {
            get; set;
        }
        public int? PokemonId
        {
            get; set;
        }
    }
}

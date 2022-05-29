//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PokemonIsshoni.Net.Shared.Models
//{
//    public class PSPokemon
//    {
//        public int Id { get; set; }

//        [Column(TypeName = "nvarchar(50)")]
//        public string PSName { get; set; }
//        [Column(TypeName = "nvarchar(50)")]
//        public string PSImgName { get; set; }
//        [Column(TypeName = "nvarchar(30)")]
//        public string PSChsName { get; set; }
//        public int AllValue { get; set; }

//        public PokemonDataAccess.Models.Pokemon Pokemon { get; set; }
//        public int? PokemonId { get; set; }
//    }
//}

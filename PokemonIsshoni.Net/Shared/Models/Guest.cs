using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonIsshoni.Net.Shared.Models
{
    public class Guest
    {
        public long Id { get; set; }
        [Column(TypeName = "varchar(270)")]
        public string UserId { get; set; }
    }
}

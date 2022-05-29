using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonIsshoni.Net.Shared.Models
{
    public enum RefereeType
    {
        // 主裁
        Main,
        // 普通裁判
        Normal
    }
    public class Referee
    {
        public int Id { get; set; }

        //public ApplicationUser User { get; set; }
        //[NotMapped]
        //public UserData UserData { get; set; }
        [Column(TypeName = "varchar(270)")]
        public string UserId { get; set; }

        //public PCLMatch Match { get; set; }
        public int PCLMatchId { get; set; } = -1;

        public RefereeType refereeType = RefereeType.Normal;
    }
}

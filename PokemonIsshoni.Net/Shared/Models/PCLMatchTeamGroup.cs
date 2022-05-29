using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonIsshoni.Net.Shared.Models
{
    public class PCLMatchTeamGroup
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(30)")]
        public string Name { get; set; }
        //public ApplicationUser Captain { get; set; }
        [Column(TypeName = "varchar(270)")]
        public string CaptainId { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string Description { get; set; } = "";

        public List<PCLMatchPlayer> PCLMatchPlayers { get; set; } = new List<PCLMatchPlayer>();
    }
}

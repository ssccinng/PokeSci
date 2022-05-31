using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonIsshoni.Net.Shared.Models
{
    public class PCLPokeTeam
    {
        public int Id { get; set; }
        // 需要吗
        /// <summary>
        /// PS格式数据
        /// </summary>
        [Column(TypeName = "longtext")]
        public string PSText { get; set; } = "";
        [Column(TypeName = "longtext")]
        /// <summary>
        /// 队伍json格式数据
        /// </summary>
        public string TeamData { get; set; } = "";

        [NotMapped]
        [NonSerialized]
        public List<PCLPokemon> Team = new List<PCLPokemon>() { new(), new(), new(), new(), new(), new() };

        //public PSPokemon poke1 { get; set; }
        //public PSPokemon poke2 { get; set; }
        //public PSPokemon poke3 { get; set; }
        //public PSPokemon poke4 { get; set; }
        //public PSPokemon poke5 { get; set; }
        //public PSPokemon poke6 { get; set; }
        //[NotMapped]
        //public List<PSPokemon> pokemons => new List<PSPokemon> { poke1, poke2, poke3, poke4, poke5, poke6 };
        //[NotMapped]
        //public List<int> pokemonids => new List<int> { poke1Id, poke2Id, poke3Id, poke4Id, poke5Id, poke6Id };
        //public int poke1Id { get; set; }
        //public int poke2Id { get; set; }
        //public int poke3Id { get; set; }
        //public int poke4Id { get; set; }
        //public int poke5Id { get; set; }
        //public int poke6Id { get; set; }
    }
}

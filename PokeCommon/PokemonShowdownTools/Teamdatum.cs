using System;
using System.Collections.Generic;

#nullable disable

namespace PokeCommon.PokemonShowdownTools
{
    public partial class Teamdatum
    {
        public int Id { get; set; }
        public string Poke1 { get; set; }
        public string Poke2 { get; set; }
        public string Poke3 { get; set; }
        public string Poke4 { get; set; }
        public string Poke5 { get; set; }
        public string Poke6 { get; set; }
        public int Win { get; set; }
        public int Highestrating { get; set; }
        public DateTime Teamdate { get; set; }
        public int Totalcnt { get; set; }
    }
}

using System.Collections.Generic;

namespace PokemonDataAccess.Models
{
    public class PokeDex
    {
        public int PokeDexId
        {
            get; set;
        }

        public int National_Dex_Id
        {
            get; set;
        }

        public List<Pokemon> Pokemon_Form_List
        {
            get; set;
        }
    }
}
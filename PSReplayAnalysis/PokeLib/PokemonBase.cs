using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSReplayAnalysis.PokeLib
{
    class PokemonBase
    {

        public string name { get; set; }
        public int PokemonID;
        public string[] NameList;
        public int[] TypeId;
        public int[] AbilityList;
        public string[] movelist; // 未完�?
        public int FormId;
        public Racial RacialValue;

        public PokemonBase(string name, int PokemonID, string[] NameList, int[] TypeId, int[] AbilityList, int FormId,
                Racial racial)
        {
            this.name = name;
            this.PokemonID = PokemonID;
            this.NameList = NameList;
            this.TypeId = TypeId;
            this.AbilityList = AbilityList;
            this.FormId = FormId;
            RacialValue = racial;
        }

        public PokemonBase()
        {
            RacialValue = new Racial();
        }



    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PSReplayAnalysis.PokeLib
{
    // mega对mega 形态归形态

    class PokemonInfo : PokemonBase
    {

        // private String name;
        public string Name;

        public string getname()
        {
            return NameList[FormId];
        }
        // private String[] NameList;

        public string Item;
        public string Ability;
        public bool Gmax = false;
        public string[] move = new string[4];
        public int Level;
        public int Happiness = 160;
        public bool Shiny;
        public Racial EVS;
        public Racial IVS;

        public Racial stats;

        public NatureClass Nature;

        public int TreaTypeId = 1;
        public string TreaType
        {
            get => Pokemondata.GetTypeName(TreaTypeId);
            set => TreaTypeId = Pokemondata.GetTypeId(value);
        }

        public PokemonInfo() : base()
        {
            EVS = new Racial();
            stats = new Racial();
            IVS = new Racial(31);
            Nature = Pokemondata.getNatureClass(0);
            Level = 50;
            Shiny = false;
        }

        public Racial getstat()
        {
            if (name == "脱壳忍者")
            {
                stats.SetHP(1);
            }
            else
            {
                stats.SetHP((RacialValue.Value[0] * 2 + IVS.Value[0] + EVS.Value[0] / 4) * Level / 100 + 10 + Level);
            }
            for (int i = 1; i < 6; ++i)
            {
                stats.Value[i] = (RacialValue.Value[i] * 2 + IVS.Value[i] + EVS.Value[i] / 4) * Level / 100 + 5;
                if (i == Nature.up) stats.Value[i] = (int)(stats.Value[i] * 1.1);
                if (i == Nature.down) stats.Value[i] = (int)(stats.Value[i] * 0.9);
            }
            return stats;
        }


        public PokemonInfo(PokemonBase f) : base(f.name, f.PokemonID, f.NameList, f.TypeId, f.AbilityList, f.FormId, f.RacialValue)
        {
            stats = new Racial();
        }

        public void Pokemonextend()
        {
            Setmove(move);
            Item = null;
            //this.Ability = Pokemondata.GetAbilityName(AbilityList[0]);
            Shiny = false;
            EVS = new Racial();
            stats = new Racial();
            IVS = new Racial(31);
            Nature = Pokemondata.getNatureClass(0);
            Level = 50;
        }
        public string getHiddenPowerType()
        {
            string rtn = "";
            int type = IVS.Value[0] % 2 + IVS.Value[1] % 2 * 2 + IVS.Value[2] % 2 * 4 + IVS.Value[5] % 2 * 8 + IVS.Value[3] % 2 * 16 + IVS.Value[4] % 2 * 32;
            type = type * 15 / 63;
            switch (type)
            {
                case 0: rtn = "Fighting"; break;
                case 1: rtn = "Flying"; break;
                case 2: rtn = "Poison"; break;
                case 3: rtn = "Ground"; break;
                case 4: rtn = "Rock"; break;
                case 5: rtn = "Bug"; break;
                case 6: rtn = "Ghost"; break;
                case 7: rtn = "Steel"; break;
                case 8: rtn = "Fire"; break;
                case 9: rtn = "Water"; break;
                case 10: rtn = "Grass"; break;
                case 11: rtn = "Electric"; break;
                case 12: rtn = "Psychic"; break;
                case 13: rtn = "Ice"; break;
                case 14: rtn = "Dragon"; break;
                case 15: rtn = "Dark"; break;
            }
            return rtn;
        }
        // public Pokemon(Pokemon f) {
        //     super(f.name, f.PokemonID, f.NameList, f.TypeId, f.AbilityList, f.FormId, f.RacialValue);
        //     Pokemonextend(f.movelist, f.Item, f.Ability, f.Shiny, f.EVS, f.IVS, f.Level, f.Nature);
        // }
        public void Pokemonextend(string[] move, string Item, string Ability, bool Shiny, Racial EVS, Racial IVS,
                int Level, NatureClass Nature)
        {
            Setmove(move);
            this.Item = Item;
            this.Ability = Ability;
            this.Shiny = Shiny;

            // this.EVS = new Racial(EVS) ;
            // this.IVS = new Racial(IVS);
            this.EVS = EVS;
            this.IVS = IVS;
            this.Level = Level;
            this.Nature = Nature == null ? Pokemondata.getNatureClass(0) : Nature;
            // this.Nature = new NatureClass(Nature);
        }
        // public Pokemon(/*String Name, String Item, String Ability,
        // String Nature, String[] move, int FormId = 0*/): base()
        // {
        // // this.name = Name;
        // // this.PokemonID = Pokemondata.GetPokemonID(Name);
        // // this.NameList = Pokemondata.GetPokemonFrom(Name);
        // // this.TypeId = Pokemondata.GetPokemonType(this.PokemonID);

        // // // this.Limit = GetLimit(Pokemondata.GetPokeLimit(this.PokemonID));
        // // this.Item = Item;
        // // this.Ability = Ability;
        // // this.Nature = Nature;
        // // this.FormId = FormId;
        // // this.move = new String[move.length];
        // // for (int i = 0; i < move.length; ++i)
        // // {
        // // this.move[i] = move[i];
        // // }
        // }

        public bool Setmove(string[] move)
        {
            if (move == null)
                return true;
            if (move.Length > 4)
                return false;
            else
            {
                this.move = new string[move.Length];
                for (int i = 0; i < move.Length; ++i)
                {
                    this.move[i] = move[i];
                }
                return true;
            }
        }
        // private PokeLimit GetLimit(int LimitId)
        // {
        // switch (LimitId)
        // {
        // case 0: return PokeLimit.NORMAL;
        // case 1: return PokeLimit.LEGEND;
        // case 2: return PokeLimit.MYSTERY;
        // default:
        // return PokeLimit.NORMAL;
        // }
        // }

        // public void print() {
        // Console.WriteLine("����: " + Name);
        // Console.WriteLine("����: " + Ability);
        // Console.WriteLine("����: " + Item);
        // Console.WriteLine("��ʽ: ");
        // foreach (String item in move)
        // {
        // Console.WriteLine(item);
        // }
        // Console.WriteLine("Ŭ��ֵ: ");
        // EVS.print();
        // Console.WriteLine("����ֵ: ");
        // IVS.print();

        //

    }
}

namespace PokemonIsshoni.Net.Shared.Models
{
    public class PokeStatistic
    {
        public int HP
        {
            get; set;
        }
        public int Atk
        {
            get; set;
        }
        public int Def
        {
            get; set;
        }
        public int Spa
        {
            get; set;
        }
        public int Spd
        {
            get; set;
        }
        public int Spe
        {
            get; set;
        }
        //[NonSerialized]

        public PokeStatistic(int val = 0)
        {
            HP = Atk = Def = Spa = Spd = Spe = val;
        }

        public int Sum => HP + Atk + Def + Spa + Spd + Spe;
    }
}

#nullable disable

namespace PokeCommon.PokemonShowdownTools
{
    public partial class PsBattle
    {
        public int Id
        {
            get; set;
        }
        public string Title
        {
            get; set;
        }
        public string Player1
        {
            get; set;
        }
        public string Player2
        {
            get; set;
        }
        public string Filename
        {
            get; set;
        }
        public DateTime Battledate
        {
            get; set;
        }
        public string Player1poke1 { get; set; } = "";
        public string Player1poke2 { get; set; } = "";
        public string Player1poke3 { get; set; } = "";
        public string Player1poke4 { get; set; } = "";
        public string Player1poke5 { get; set; } = "";
        public string Player1poke6 { get; set; } = "";
        public string Player2poke1 { get; set; } = "";
        public string Player2poke2 { get; set; } = "";
        public string Player2poke3 { get; set; } = "";
        public string Player2poke4 { get; set; } = "";
        public string Player2poke5 { get; set; } = "";
        public string Player2poke6 { get; set; } = "";
        public int Whowin
        {
            get; set;
        }
        public int BattleId
        {
            get; set;
        }
        public int Rating1
        {
            get; set;
        }
        public int Rating2
        {
            get; set;
        }
    }
}

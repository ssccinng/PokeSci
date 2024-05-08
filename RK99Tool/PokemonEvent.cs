namespace RK9Tool
{
    public class PokemonEvent
    {
        
        public string Name  { get; set; }
        public string Date { get; set; }
        public string Location { get; set; }
        public string EventUrl { get; set; }

        public Dictionary<string, string> FormatUrl { get; set; } = [];

        public bool Past { get; set; }

    }
}
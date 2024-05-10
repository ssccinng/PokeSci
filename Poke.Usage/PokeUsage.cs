namespace Poke.Usage
{
    public class PokeUsage
    {
        public List<PokemonUsage> PokemonUsage { get; set; } = [];
    }

    public class PokemonUsage: UsageItem
    {

        public List<UsageItem> MoveUsage { get; set; } = [];
        public List<UsageItem> ItemUsage { get; set; } = [];
        public List<UsageItem> AbilityUsage { get; set; } = [];

        public List<UsageItem> NatureUsage { get; set; } = [];

        public List<UsageItem> AliyPokemonUsage { get; set; } = [];
    }

    public class UsageItem
    {
        public int Id { get; set; }

        public int Count { get; set; }

        public decimal Percentage { get; set; }
    }
}

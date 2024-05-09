namespace RK9Tool
{
    public class MatchPairing
    {
        public int Division { get; set; } = 0;
        public List<PairingRound> PairingRounds { get; set; } = new();
        
    }


    public class PairingRound
    {
        public int Round { get; set; } = 0;
        public List<PairingTable> Pairings { get; set; } = new();
    }

    public class PairingTable
    {
        public string Player1Name { get; set; } = string.Empty;
        public string Player2Name { get; set; } = string.Empty;

        public string Player1Country { get; set; } = string.Empty;
        public string Player2Country { get; set; } = string.Empty;
        public int Table { get; set; } = 114514;
        public bool Player1Win { get; set; } = false;
        public bool Player2Win { get; set; } = false;
        public string Player1Score { get; set; } = string.Empty;
        public string Player2Score { get; set; } = string.Empty;
    }

    public enum Division
    {
        Junior = 0,
        Senior = 1,
        Master = 2
    }
}
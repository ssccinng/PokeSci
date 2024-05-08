namespace RK9Tool
{
    public class MatchPairing
    {
        public string Player1Name { get; set; } = string.Empty;
        public string Player2Name { get; set; } = string.Empty;
        public int Table { get; set; } = 114514;
        public bool Player1Win { get; set; } = false;
        public bool Player2Win { get; set; } = false;
        public string Player1Score { get; set; } = string.Empty;
        public string Player2Score { get; set; } = string.Empty;
    }
}
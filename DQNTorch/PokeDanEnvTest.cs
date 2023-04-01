namespace DQNTorch
{
    public class PokeDanEnvTest
    {
        public int BattleId { get; set; } = 0;
        public int Turn { get; set; } = 0;
        public float[] StateSpace { get; set; }
        public PokeDanEnvTest(string path)
        {
            Path = path;

        }

        public string Path { get; }
        public int Turn { get; }

        internal int[][] GetAction(int battleId, int player)
        {
            throw new NotImplementedException();
        }

        internal (float[], int cnt) Reset(int v)
        {
            throw new NotImplementedException();
        }

        internal (float[], float, float) Step(int v, int a)
        {
            throw new NotImplementedException();
        }
    }
}
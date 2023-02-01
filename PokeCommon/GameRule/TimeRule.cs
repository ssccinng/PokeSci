namespace PokeCommon.GameRule
{
    public class TimeRule
    {
        /// <summary>
        /// 对战时间是否限制
        /// </summary>
        public bool IsBattleTimeLimit
        {
            get; init;
        }
        /// <summary>
        /// 对局时间是否限制
        /// </summary>
        public bool IsTurnTimeLimit
        {
            get; init;
        }
        /// <summary>
        /// 对战总时间
        /// </summary>
        public int BattleTime
        {
            get; init;
        }
        /// <summary>
        /// 回合总时间
        /// </summary>
        public int TurnTime
        {
            get; init;
        }
    }
}

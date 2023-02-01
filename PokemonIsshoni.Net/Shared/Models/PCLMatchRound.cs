using System.ComponentModel.DataAnnotations;

namespace PokemonIsshoni.Net.Shared.Models
{

    public enum RoundType
    {
        /// <summary>
        /// 瑞士
        /// </summary>
        Swiss,
        // 循环
        Robin,
        // 淘汰
        Elimination,
    }
    // 需要细化
    public enum RoundState
    {
        /// <summary>
        /// 等待开始
        /// </summary>
        Waiting,
        /// <summary>
        /// 分组确认
        /// </summary>
        WaitConfirm,
        /// <summary>
        /// 轮进行时
        /// </summary>
        Running,
        /// <summary>
        /// 出线人员确认
        /// </summary>
        TopConfirm,
        /// <summary>
        /// 轮结束
        /// </summary>
        Finished,
    }

    public enum PromotionType
    {
        Scorecut,
        Topcut
    }
    public enum EliminationType
    {
        Single,
        Double
    }
    public partial class PCLMatchRound
    {
        public int Id
        {
            get; set;
        }
        //public PCLMatch PCLMatch { get; set; }
        public int PCLMatchId
        {
            get; set;
        }


        public RoundType PCLRoundType
        {
            get; set;
        }
        /// <summary>
        /// 是否是小组赛
        /// </summary>
        [ConcurrencyCheck]
        public bool IsGroup
        {
            get; set;
        }
        /// <summary>
        /// 分多少组
        /// </summary>
        public int GroupCnt { get; set; } = 2;

        public List<PCLRoundPlayer> PCLRoundPlayers { get; set; } = new List<PCLRoundPlayer>();
        public List<PCLBattle> PCLBattles { get; set; } = new();

        /// <summary>
        /// 比赛状态
        /// </summary>、
        [ConcurrencyCheck]
        public RoundState PCLRoundState
        {
            get; set;
        }
        /// <summary>
        /// 是否锁队
        /// </summary>
        public bool LockTeam
        {
            get; set;
        }
        /// <summary>
        /// 允许提交队伍
        /// </summary>
        public bool AcceptTeamSubmit
        {
            get; set;
        }
        // 加入比赛标识（？
        /// <summary>
        /// 是否剋以看对手队伍
        /// </summary>
        public bool CanSeeOppTeam
        {
            get; set;
        }
        /// <summary>
        /// BO几定胜负
        /// </summary>
        public int BO { get; set; } = 3;


        public int RoundPromotion { get; set; } = 8; // 出线人数  or 分数


        // 胜平负所获得的大分 对瑞士轮和循环赛有用
        public int WinScore { get; set; } = 3;
        public int DrawScore { get; set; } = 1;
        public int LoseScore { get; set; } = 0;
        #region 瑞士轮
        /// <summary>
        /// 瑞士轮数
        /// </summary>
        public int SwissCount { get; set; } = 1;// 瑞士轮数 // 系统推荐
        public int Swissidx { get; set; } = 0;
        public PromotionType SwissPromotionType { get; set; } = PromotionType.Topcut;
        #endregion

        #region 淘汰赛
        public EliminationType EliminationType { get; set; } = EliminationType.Single;
        #endregion

        #region 循环赛
        // 出线人数 与之前的成员公用
        #endregion

    }
}

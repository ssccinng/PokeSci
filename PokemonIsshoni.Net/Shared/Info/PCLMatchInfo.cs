using PokemonIsshoni.Net.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchType = PokemonIsshoni.Net.Shared.Models.MatchType;

namespace PokemonIsshoni.Net.Shared.Info
{
    /// <summary>
    /// 比赛信息的暴露
    /// </summary>
    public class PCLMatchInfo
    {
        public PCLMatchInfo(PCLMatch pCLMatch)
        {
            Id = pCLMatch.Id;
            MatchStartDate = pCLMatch.MatchStartDate;

            MatchEndDate = pCLMatch.MatchEndDate;
            MatchType = pCLMatch.MatchType;
            Description = pCLMatch.Description;
            AllowGuest = pCLMatch.AllowGuest;
            IsTeamCompeition = pCLMatch.IsTeamCompeition;
            CanCancelSign = pCLMatch.CanCancelSign;
            MatchType = pCLMatch.MatchType;
            MatchOnline = pCLMatch.MatchOnline;
            MatchState = pCLMatch.MatchState;
            IsPrivate = pCLMatch.IsPrivate;
            Logo = pCLMatch.Logo;
            PCLMatchRoundList = pCLMatch.PCLMatchRoundList;
            PCLMatchPlayerList = pCLMatch.PCLMatchPlayerList;
            PCLMatchRefereeList = pCLMatch.PCLMatchRefereeList;
            RoundIdx = pCLMatch.RoundIdx;
            NeedCheck = pCLMatch.NeedCheck;
            LimitPlayer = pCLMatch.LimitPlayer;
        }
        public UserInfo MatchHostUser { get; set; }


        public int Id { get; set; }
        /// <summary>
        /// 创办者
        /// </summary>
        //public ApplicationUser User { get; set; }
        /// <summary>
        /// 创办者Id
        /// </summary>
        //public string UserId { get; set; } = "";
        public string Name { get; set; }

        public DateTime? MatchStartDate { get; set; } = DateTime.Now; // 比赛日期
        public DateTime? MatchEndDate { get; set; } = DateTime.Now;// 预计结束时间
        public string Description { get; set; } = "";// 比赛描述
        /// <summary>
        /// 允许游客参赛
        /// </summary>
        public bool AllowGuest { get; set; } = false;
        /// <summary>
        /// 是否是团赛
        /// </summary>
        public bool IsTeamCompeition { get; set; } = false;

        public bool CanCancelSign { get; set; } = false;
        public MatchType MatchType { get; set; } = MatchType.Double;
        public MatchOnline MatchOnline { get; set; } = MatchOnline.Online;
        public MatchState MatchState { get; set; } = MatchState.Registering;

        /// <summary>
        /// 是否是私密比赛
        /// </summary>
        public bool IsPrivate { get; set; } = false;

        /// <summary>
        /// 比赛图标路径
        /// </summary>
        public string Logo { get; set; } = "ServerImages/Default/matchdouble.png";

        public List<PCLMatchRound> PCLMatchRoundList { get; set; } = new List<PCLMatchRound>();
        public List<PCLMatchPlayer> PCLMatchPlayerList { get; set; } = new List<PCLMatchPlayer>();
        public List<Referee> PCLMatchRefereeList { get; set; } = new List<Referee>();

        /// <summary>
        /// 进行到的阶段数
        /// </summary>
        public int RoundIdx { get; set; } = -1;

        public bool NeedCheck { get; set; } = false;

        public int LimitPlayer { get; set; } = -1;
    }
}

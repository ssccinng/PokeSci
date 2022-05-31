using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace PokemonIsshoni.Net.Shared.Models
{
    public enum MatchType
    {
        Single,
        Double,
    }

    public enum MatchOnline
    {
        Online,
        Offline,
    }
    public enum MatchState
    {
        Registering,
        Running,
        Finished
    }

    public class PCLMatch
    {

        public int Id { get; set; }
        /// <summary>
        /// 创办者
        /// </summary>
        //public ApplicationUser User { get; set; }
        /// <summary>
        /// 创办者Id
        /// </summary>
        [Column(TypeName = "varchar(270)")]
        public string UserId { get; set; } = "";
        [Column(TypeName = "nvarchar(30)")]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime? MatchStartDate { get; set; } = DateTime.Now; // 比赛日期
        [DataType(DataType.Date)]
        public DateTime? MatchEndDate { get; set; } = DateTime.Now;// 预计结束时间
        [Column(TypeName = "longtext")]
        public string Description { get; set; } = "";// 比赛描述
        /// <summary>
        /// 允许游客参赛
        /// </summary>
        [ConcurrencyCheck]
        public bool AllowGuest { get; set; } = false;
        /// <summary>
        /// 是否是团赛
        /// </summary>
        [ConcurrencyCheck]
        public bool IsTeamCompeition { get; set; } = false;

        [ConcurrencyCheck]
        public bool CanCancelSign { get; set; } = false;
        // 加个是否可取消报名（？
        public MatchType MatchType { get; set; } = MatchType.Double;
        public MatchOnline MatchOnline { get; set; } = MatchOnline.Online;
        public MatchState MatchState { get; set; } = MatchState.Registering;

        /// <summary>
        /// 是否是私密比赛
        /// </summary>
        public bool IsPrivate { get; set; } = false;
        /// <summary>
        /// 比赛密语
        /// </summary>
        [Column(TypeName = "varchar(50)")]
        public string Password { get; set; } = "";
        /// <summary>
        /// 比赛图标路径
        /// </summary>
        [Column(TypeName = "varchar(100)")]
        public string Logo { get; set; } = "ServerImages/Default/matchdouble.png";

        public List<PCLMatchRound> PCLMatchRoundList { get; set; } = new List<PCLMatchRound>();
        public List<PCLMatchPlayer> PCLMatchPlayerList { get; set; } = new List<PCLMatchPlayer>();
        public List<Referee> PCLMatchRefereeList { get; set; } = new List<Referee>();

        /// <summary>
        /// 进行到的阶段数
        /// </summary>
        [ConcurrencyCheck]
        public int RoundIdx { get; set; } = -1;

        public bool NeedCheck { get; set; } = false;

        public int LimitPlayer { get; set; } = -1;
        // 是否显示在主界面
    }
}

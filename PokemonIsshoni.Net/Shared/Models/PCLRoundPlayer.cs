﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonIsshoni.Net.Shared.Models
{
    public class PCLRoundPlayer
    {
        [NotMapped]
        private static Random _rnd = new();
        public int Id
        {
            get; set;
        }
        //public ApplicationUser User { get; set; }
        [Column(TypeName = "varchar(270)")]
        public string UserId
        {
            get; set;
        }

        //public PCLMatchRound PCLMatchRound { get; set; }
        public int PCLMatchRoundId
        {
            get; set;
        }
        [NotMapped]
        public List<PCLBattle> PCLBattles { get; set; } = new List<PCLBattle>();
        /// <summary>
        /// 分组编号
        /// </summary>
        public int GroupId
        {
            get; set;
        }
        [ConcurrencyCheck]
        public int Win
        {
            get; set;
        }
        [ConcurrencyCheck]
        public int Draw
        {
            get; set;
        }
        [ConcurrencyCheck]
        public int Lose
        {
            get; set;
        }
        [NotMapped]
        public decimal Ratio
        {
            get
            {
                if (Win + Draw + Lose == 0) return 0;
                return Win * 1.0m / (Win + Draw + Lose);
            }
        }

        public int Score
        {
            get; set;
        }
        public int Rank { get; set; } = _rnd.Next(2048);
        /// <summary>
        /// 已弃赛
        /// </summary>
        public bool IsDrop
        {
            get; set;
        }
        /// <summary>
        /// 随机tag 备用
        /// </summary>
        public int Tag { get; set; } = _rnd.Next(2048);
        /// <summary>
        /// 已经被轮空过了
        /// </summary>
        public bool HasBye
        {
            get; set;
        }

        #region 瑞士轮
        /// <summary>
        /// 瑞士轮数
        /// </summary>
        [Column(TypeName = "decimal(8, 6)")]
        public decimal OppRatio { get; set; } = 0;// 瑞士轮数 // 系统推荐
        [Column(TypeName = "decimal(8, 6)")]
        public decimal OppOppRatio { get; set; } = 0;// 瑞士轮数 // 系统推荐
        #endregion

        #region 淘汰赛

        #endregion

        #region 循环赛
        public int MiniScore
        {
            get; set;
        }
        #endregion


        // 提交队伍
        public PCLPokeTeam? BattleTeam
        {
            get; set;
        }
        public int BattleTeamId
        {
            get; set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonIsshoni.Net.Shared.Models
{
    /// <summary>
    /// 比赛报名选手
    /// </summary>
    public class PCLMatchPlayer
    {
        public int Id { get; set; }
        /// <summary>
        /// 选手Id
        /// </summary>
        //public ApplicationUser User { get; set; }
        //[NotMapped]
        //public UserData UserData { get; set; }

        [Column(TypeName = "varchar(270)")]
        public string UserId { get; set; } = "";

        /// <summary>
        /// 比赛Id
        /// </summary>
        //public PCLMatch PCLMatch { get; set; }
        public int PCLMatchId { get; set; }

        /// <summary>
        /// 参赛宣言
        /// </summary>
        [Column(TypeName = "nvarchar(50)")]
        public string Declaration { get; set; } = "";

        /// <summary>
        /// 马甲
        /// </summary>
        [Column(TypeName = "nvarchar(20)")]
        public string ShadowId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Column(TypeName = "nvarchar(30)")]
        public string? Remarks { get; set; } = "";

        // 团赛所属队伍

        public PCLMatchTeamGroup? PCLMatchTeamGroup { get; set; }
        public int? PCLMatchTeamGroupId { get; set; }

        [Column(TypeName = "varchar(15)")]
        public string QQ { get; set; } // QQ
        public bool IsChecked { get; set; }

        // 预提交队伍
        public PCLPokeTeam PreTeam { get; set; } = new();
        //public int PreTeamId { get; set; }
        /// <summary>
        /// 预提队伍Id
        /// </summary>
        public int PreTeamId { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonIsshoni.Net.Shared.Models
{
    public enum BattleState
    {
        Waiting,
        Player1Win,
        Player2Win,
        Draw,
        AllLose
    }
    public partial class PCLBattle
    {
        public int Id { get; set; }
        //public PCLMatchRound PCLMatchRound { get; set; }
        public int PCLMatchRoundId { get; set; }
        //public PCLMatch PCLMatch { get; set; }
        public int PCLMatchId { get; set; }
        //public List<PCLRoundPlayer> PCLRoundPlayers { get; set; } = new List<PCLRoundPlayer>();

        //public ApplicationUser Player1 { get; set; }
        //public ApplicationUser Player2 { get; set; }
        [Column(TypeName = "varchar(270)")]
        public string? Player1Id { get; set; }
        [Column(TypeName = "varchar(270)")]
        public string? Player2Id { get; set; }

        public BattleState PCLBattleState { get; set; } = BattleState.Waiting;

        public PCLPokeTeam Player1Team { get; set; }
        public int Player1TeamId { get; set; }
        public PCLPokeTeam Player2Team { get; set; }
        public int Player2TeamId { get; set; }
        // 比分
        [ConcurrencyCheck]
        public int Player1Score { get; set; } = 0;
        [ConcurrencyCheck]
        public int Player2Score { get; set; } = 0;
        // 小分
        [ConcurrencyCheck]
        public int Player1MiniScore { get; set; } = 0;
        [ConcurrencyCheck]
        public int Player2MiniScore { get; set; } = 0;

        [Column(TypeName = "nvarchar(50)")]
        public string Description { get; set; } = "";
        /// <summary>
        /// 是否战绩已经被提交
        /// </summary>
        [ConcurrencyCheck]
        public bool Submitted { get; set; } = false;
        /// <summary>
        /// 对局Tag 用于保存有用信息 瑞士轮桌号 
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// BO规则
        /// </summary>
        public int BO { get; set; }

        /// <summary>
        /// 瑞士轮轮号
        /// </summary>
        public int SwissRoundIdx { get; set; } = 0;
        /// <summary>
        /// 分组Id
        /// </summary>
        public int GroupId { get; set; }


    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokePSCore
{
    public class PsBattleData
    {
        public string Player1Id { get; set; }
        public string Player2Id { get; set; }

        public int Player1Score { get; set; } = 0;
        public int Player2Score { get; set; } = 0;

        public BattleResult Result { get; set; }

        public string BattleLog { get; set; } = string.Empty;

        public string Tier { get; set; } = string.Empty;
        [Key]
        public string RoomId { get; set; }






    }

    public enum BattleResult
    {
        Player1Win,
        Player2Win,
        Draw,
    }
}

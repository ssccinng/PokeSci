using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace PokemonDataAccess.Models
{
    /// <summary>
    /// 能力相关
    /// </summary>
    public class Statistic
    {
        public int Id { get; set; }
        #region 名字
        [Comment("中文名")]
        [Column(TypeName = "nvarchar(3)")]
        public string Name_Chs { get; set; }

        [Comment("英文名")]
        [Column(TypeName = "varchar(20)")]
        public string Name_Eng { get; set; }

        [Comment("日文名")]
        [Column(TypeName = "nvarchar(10)")]
        public string Name_Jpn { get; set; }
        #endregion
        /// <summary>
        /// 能力对应的口味
        /// </summary>
        /// <value></value>
        public Flavor Flavor { get; set; } 

        // /// <summary>
        // /// 能力下降时讨厌的口味
        // /// </summary>
        // /// <value></value>
        // public Flavor down { get; set; }

    }
}
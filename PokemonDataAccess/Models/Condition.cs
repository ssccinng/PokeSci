using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace PokemonDataAccess.Models
{
    /// <summary>
    /// 华丽大赛中评选所需要参考的能力
    /// </summary>
    public class Condition
    {
        public int Id { get; set; }
        #region 名字
        [Comment("中文名")]
        [Column(TypeName = "nvarchar(2)")]
        public string Name_Chs { get; set; }

        [Comment("英文名")]
        [Column(TypeName = "varchar(10)")]
        public string Name_Eng { get; set; }

        [Comment("日文名")]
        [Column(TypeName = "nvarchar(10)")]
        public string Name_Jpn { get; set; }
        #endregion
    }
}
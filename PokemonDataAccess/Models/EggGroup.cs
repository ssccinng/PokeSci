using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PokemonDataAccess.Models
{
    public class EggGroup
    {
        public int EggGroupId
        {
            get; set;
        }
        #region 名字
        [Comment("中文名")]
        [Column(TypeName = "nvarchar(20)")]
        public string Name_Chs
        {
            get; set;
        }

        [Comment("英文名")]
        [Column(TypeName = "varchar(40)")]
        public string Name_Eng
        {
            get; set;
        }

        [Comment("日文名")]
        [Column(TypeName = "nvarchar(20)")]
        public string Name_Jpn
        {
            get; set;
        }
        #endregion
    }
}
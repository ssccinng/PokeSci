using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PokemonDataAccess.Models
{
    public class Performance
    {
        public int Id
        {
            get; set;
        }
        #region 名字
        [Comment("中文名")]
        [Column(TypeName = "nvarchar(2)")]
        public string Name_Chs
        {
            get; set;
        }

        [Comment("英文名")]
        [Column(TypeName = "varchar(10)")]
        public string Name_Eng
        {
            get; set;
        }

        [Comment("日文名")]
        [Column(TypeName = "nvarchar(6)")]
        public string Name_Jpn
        {
            get; set;
        }
        #endregion
    }
}
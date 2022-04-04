using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonDataAccess.Models
{
    public class PokeType
    {
        public int Id { get; set; }

        #region 名字
        [Comment("中文名")]
        [Column(TypeName = "nvarchar(5)")]
        public string Name_Chs { get; set; }

        [Comment("英文名")]
        [Column(TypeName = "varchar(20)")]
        public string Name_Eng { get; set; }

        [Comment("日文名")]
        [Column(TypeName = "nvarchar(10)")]
        public string Name_Jpn { get; set; }
        #endregion

    }
}
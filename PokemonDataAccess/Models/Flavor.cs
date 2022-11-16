using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace PokemonDataAccess.Models
{
    /// <summary>
    /// 口味
    /// </summary>
    public class Flavor
    {
        public int Id
        {
            get; set;
        }
        #region 名字
        [Comment("中文名")]
        [Column(TypeName = "nvarchar(1)")]
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
        [Column(TypeName = "nvarchar(5)")]
        public string Name_Jpn
        {
            get; set;
        }
        #endregion

        public Condition Condition_Up
        {
            get; set;
        }
        public Performance Performance_Up
        {
            get; set;
        }
        // 体能up
    }
}
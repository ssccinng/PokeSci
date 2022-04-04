using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonDataAccess.Models
{
    public class Nature
    {
        public int NatureId { get; set; }


        #region 名字
        [Comment("中文名")]
        [Column(TypeName = "nvarchar(5)")]
        public string Name_Chs { get; set; }

        [Comment("英文名")]
        [Column(TypeName = "varchar(15)")]
        public string Name_Eng { get; set; }

        [Comment("日文名")]
        [Column(TypeName = "nvarchar(10)")]
        public string Name_Jpn { get; set; }
        #endregion

        public Statistic Stat_Up { get; set; }
        public Statistic Stat_Down { get; set; }

        public Performance Perf_Up { get; set; }
        public Performance Perf_Down { get; set; }

        public Flavor Flavor_Up { get; set; }
        public Flavor Flavor_Down { get; set; }

        public int? Performance_value { get; set; }


    }
}
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

using PokemonDataAccess.Interfaces;
namespace PokemonDataAccess.Models
{
    public class Ability
    {
        public int AbilityId { get; set; }

#region 名字
        [Comment("中文名")]
        [Column(TypeName = "nvarchar(20)")]
        public string Name_Chs { get; set; }

        [Comment("英文名")]
        [Column(TypeName = "varchar(40)")]
        public string Name_Eng { get; set; }

        [Comment("日文名")]
        [Column(TypeName = "nvarchar(20)")]
        public string Name_Jpn { get; set; }
#endregion

#region 描述
        [Comment("中文描述")]
        [Column(TypeName = "nvarchar(100)")]
        public string description_Chs  {get; set;}

        [Comment("英文描述")]
        [Column(TypeName = "varchar(200)")]
        public string description_Eng  {get; set;}

        [Comment("日文描述")]
        [Column(TypeName = "nvarchar(100)")]
        public string description_Jpn  {get; set;}
#endregion

    }
}
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PokemonDataAccess.Interfaces
{
    public interface IHasName
    {
        [Comment("中文名")]
        [Column(TypeName = "nvarchar(20)")]
        string Name_Chs
        {
            get; set;
        }

        [Comment("英文名")]
        [Column(TypeName = "varchar(40)")]
        string Name_Eng
        {
            get; set;
        }

        [Comment("日文名")]
        [Column(TypeName = "nvarchar(20)")]
        string Name_Jpn
        {
            get; set;
        }
    }
}
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PokemonDataAccess.Interfaces
{
    public interface IHasDescription
    {
        [Comment("中文描述")]
        [Column(TypeName = "nvarchar(100)")]
        string description_Chs
        {
            get; set;
        }

        [Comment("英文描述")]
        [Column(TypeName = "varchar(200)")]
        string description_Eng
        {
            get; set;
        }

        [Comment("日文描述")]
        [Column(TypeName = "nvarchar(100)")]
        string description_Jpn
        {
            get; set;
        }

    }
}
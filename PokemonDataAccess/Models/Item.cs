using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace PokemonDataAccess.Models
{
    /// <summary>
    /// 道具
    /// </summary>
    public class Item
    {
        public int ItemId
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

        #region 描述
        [Comment("中文描述")]
        [Column(TypeName = "nvarchar(100)")]
        public string description_Chs
        {
            get; set;
        }

        [Comment("英文描述")]
        [Column(TypeName = "varchar(200)")]
        public string description_Eng
        {
            get; set;
        }

        [Comment("日文描述")]
        [Column(TypeName = "nvarchar(100)")]
        public string description_Jpn
        {
            get; set;
        }
        public ItemType Item_Type
        {
            get; set;
        }
        #endregion
    }

    public enum ItemType
    {
        Abandon,
        Medicine,
        Carryorevolve,
        Ingredients,
        BattleItem,
        PokeBall,
        TMTR,
        Berry,
        ImportantItem,
        Others

    }
}
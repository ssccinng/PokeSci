using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PokemonIsshoni.Net.Server.Areas.Identity.Data;

// Add profile data for application users by adding properties to the PokemonIsshoniNetServerUser class
public class PokemonIsshoniNetServerUser : IdentityUser
{
    /// <summary>
    /// 昵称
    /// </summary>
    [PersonalData]
    /// 
    [Column(TypeName = "nvarchar(40)")]

    public string NickName { get; set; }

    //[PersonalData]
    //[Column(TypeName = "varchar(40)")]
    //public string NickName { get; set; } // 昵称
    [PersonalData]
    public DateTime DOB { get; set; } // 生日

    [PersonalData]
    [Column(TypeName = "varchar(20)")]
    public string City { get; set; } // 城市
    [PersonalData]
    [Column(TypeName = "varchar(40)")]
    public string HomeName { get; set; } // 剑盾游戏名字
                                         //[PersonalData]
                                         //public DateTime DOB { get; set; } // 生日
                                         // QQ
    [PersonalData]
    [Column(TypeName = "varchar(15)")]
    public string QQ { get; set; } // QQ

    [PersonalData]
    public DateTime Registertime { get; set; } = DateTime.Now;// 注册时间

    [Column(TypeName = "longtext")]
    //public string Avatar { get; set; } = "ServerImage/Avatar/ztxb.jpeg";
    public string Avatar { get; set; } = "ServerImages/Avatar/ania.jpg";

    public int TrainerIdInt { get; set; }
    /// <summary>
    /// GuSai
    /// </summary>
    public int GS { get; set; }
    [NotMapped]
    public string TrainerId { get => $"{TrainerIdInt / 10000:0000}-{TrainerIdInt % 10000:0000}"; }
}


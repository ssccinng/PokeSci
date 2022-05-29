////using Microsoft.AspNet.Identity.EntityFramework;
//using Microsoft.AspNetCore.Identity;
////using Microsoft.AspNetCore.Identity;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Threading.Tasks;

//namespace NewPokemonChineseLink.Shared.Models
//{
//    public class ApplicationUser : IdentityUser
//    {
//        [PersonalData]
//        [Column(TypeName = "varchar(40)")]
//        public string NickName { get; set; } // 昵称
//        [PersonalData]
//        public DateTime DOB { get; set; } // 生日

//        [PersonalData]
//        [Column(TypeName = "varchar(20)")]
//        public string City { get; set; } // 城市
//        [PersonalData]
//        [Column(TypeName = "varchar(40)")]
//        public string HomeName { get; set; } // 剑盾游戏名字
//        //[PersonalData]
//        //public DateTime DOB { get; set; } // 生日
//        // QQ
//        [PersonalData]
//        [Column(TypeName = "varchar(15)")]
//        public string QQ { get; set; } // QQ

//        [PersonalData]
//        public DateTime Registertime { get; set; } = DateTime.Now;// 注册时间

//        [Column(TypeName = "longtext")]
//        public string Avatar { get; set; } = "ServerImage/Avatar/ztxb.jpeg";

//        public int TrainerIdInt { get; set; }
//        [NotMapped]
//        public string TrainerId { get => $"{TrainerIdInt / 10000:0000}-{TrainerIdInt % 10000:0000}"; }
//        [NotMapped]
//        public UserData GetUserData => new () {
//            NickName = NickName,
//            DOB = DOB,
//            City = City,
//            HomeName = HomeName,
//            QQ = QQ,
//            Registertime = Registertime,
//            Avatar = Avatar,
//            TrainerIdInt = TrainerIdInt,
//            Email = Email,
//            UserId = Id,
//        };
//        //public ApplicationUser User { get; set; }
//        //public string UserId;
//    }
//}

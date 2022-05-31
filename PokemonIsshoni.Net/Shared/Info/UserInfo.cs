using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PokemonIsshoni.Net.Shared.Info
{
    public class UserInfo
    {
        public string UserId { get; set; } // 昵称
        public string Email { get; set; } // 昵称
        public string NickName { get; set; } // 昵称

        public DateTime DOB { get; set; } // 生日
        public string City { get; set; } // 城市

        public string HomeName { get; set; } // 剑盾游戏名字
                                             //[PersonalData]
                                             //public DateTime DOB { get; set; } // 生日
                                             // QQ
        public string QQ { get; set; } // QQ


        public DateTime Registertime { get; set; } = DateTime.Now;// 注册时间


        public string Avatar { get; set; } = "ServerImage/Avatar/ztxb.jpeg";

        public int TrainerIdInt { get; set; }
        [JsonIgnore]
        public string TrainerId { get => $"{TrainerIdInt / 10000:0000}-{TrainerIdInt % 10000:0000}"; }

    }
}

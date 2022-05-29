using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonIsshoni.Net.Shared.Info
{
    /// <summary>
    /// 比赛信息的暴露
    /// </summary>
    public class PCLMatchInfo
    {
        public UserInfo MatchHostUser { get; set; }
        public string Name { get; set; }
    }
}

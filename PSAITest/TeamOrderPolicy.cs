using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSAITest
{
    // 需要一个策略倾向
    public class TeamOrderPolicy
    {
        public List<string> OppPoke { get; set; } = new();
        /// <summary>
        /// 有或者没有
        /// </summary>
        public bool Has { get; set; } = true;
        
        public List<PolicyRes> ChoosePoke { get; set; } = new();
        // 对于此策略的子策略树 
        public List<TeamOrderPolicy> TeamOrderPolices { get; set; } = new();
    }

    public class PolicyRes
    {
       public string Res { get; set; }
        public int Ratio { get; set; } = 1;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSAITest
{
    internal class AIConfig
    {
        public string Username { get; set; } = "linliu";
        public string Password { get; set; } = "";
        public string Team { get; set; } = "";
        public List<TeamOrderPolicy> TeamOrderPolicies { get; set; } = new();
        public int OnlineCnt { get; set; } = 5;
        public int BattleCnt { get; set; } = 10;

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonIsshoni.Net.Shared.Models
{
    public partial class PCLBattle
    {

        /// <summary>
        /// 通过用户id获取对手Id
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string GetOppUserId(string UserId)
        {
            if (Player1Id == UserId)
            {
                return Player2Id;
            }
            else if (Player2Id == UserId)
            {
                return Player1Id;
            }
            else
            {
                return null;
            }
        }
    }
}

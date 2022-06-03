using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace PokePSCore
{
    public partial class PSClient
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="room">这个只是名字？</param>
        /// <param name="messages"></param>
        /// <returns></returns>
        private async Task SendAsync(string room, params string[] messages)
        {
            string data = $"{room}|{string.Join('|', messages)}";
            await _webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Text, true, CancellationToken.None);
            await WriteLogAsync(data, MsgType.Send);
        }
        /// <summary>
        /// 搜索对战
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public async Task SearchBattleAsync(string rule)
        {
            string data = $"/search {rule}";
            await SendAsync("", data);
        }
        /// <summary>
        /// 发起挑战
        /// </summary>
        /// <param name="player"></param>
        /// <param name="rule"></param>
        /// <returns></returns>
        public async Task ChallengeAsync(string player, string rule)
        {
            string data = $"/challenge {player}, {rule}";
            await SendAsync("", data);
        }
        /// <summary>
        /// 发送房间消息
        /// </summary>
        /// <param name="battleTag"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessageAsync(string battleTag, string message)
        {
            await SendAsync(battleTag, message);
        }
        /// <summary>
        /// 选招
        /// </summary>
        /// <param name="battleTag"></param>
        /// <param name="move"></param>
        /// <param name="turn"></param>
        /// <returns></returns>
        public async Task SendMoveAsync(string battleTag, int move, int turn)
        {
            await SendAsync(battleTag, $"/choose move {move}", turn.ToString());
        }
        /// <summary>
        /// 换人
        /// </summary>
        /// <param name="battleTag"></param>
        /// <param name="move"></param>
        /// <param name="turn"></param>
        /// <returns></returns>
        public async Task SendSwitchAsync(string battleTag, int pokemon, int turn)
        {
            await SendAsync(battleTag, $"/choose switch {pokemon}", turn.ToString());
        }
        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="battleTag"></param>
        /// <returns></returns>
        public async Task SendLeaveAsync(string battleTag)
        {
            await SendAsync("", $"/leave {battleTag}");
        }
        /// <summary>
        /// 加入房间
        /// </summary>
        /// <param name="battleTag"></param>
        /// <returns></returns>
        public async Task SendjoinAsync(string battleTag)
        {
            await SendAsync("", $"/join {battleTag}");
        }
        /// <summary>
        /// 获取房间
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="scoreLimit"></param>
        /// <returns></returns>
        public async Task GetRoomListAsync(string rule = null, int scoreLimit = -1) 
        {
            string data = $"/cmd roomlist {rule},{(scoreLimit == -1 ? "none" : scoreLimit)}";
            await SendAsync("", data);
            //await WriteLogAsync(data, MsgType.Send);
        }
    }
}

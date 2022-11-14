using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

using System.Text.Json;
using System.Text.Json.Serialization;

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
        public async Task SendAsync(string room, params string[] messages)
        {
            string data = $"{room}|{string.Join('|', messages)}";
            await WriteLogAsync(data, MsgType.Send);
            await _webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Text,
                true, CancellationToken.None);
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

        public async Task CancelChallengeAsync(string player, string rule)
        {
            string data = $"/cancelchallenge {player}";
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
            Console.WriteLine("准备发招");
            // await SendAsync(battleTag, $"/choose move {move}", turn.ToString());
            await SendAsync(battleTag, $"/choose move {move}", turn.ToString());
        }
        public async Task SendMoveAsync(string battleTag, int turn, params ChooseData[] move)
        {
            // dynamax 加入dymax
            Console.WriteLine("准备发招");
            // await SendAsync(battleTag, $"/choose move {move}", turn.ToString());
            await SendAsync(battleTag, "/choose " + string.Join(',', move.Select(s => s.ToString())), turn.ToString());
                // await SendAsync(battleTag, 
                // $"/choose move {string.Join(',', move.Select(s =>{List<string> list = new List<string> {s.MoveId.ToString()};if (s.Target != -1) list.Add(s.Target.ToString());if (s.Dmax) list.Add("dynamax");return string.Join(' ', list);}))}", 
                // turn.ToString());
        }
        /// <summary>
        /// 换人
        /// </summary>
        /// <param name="battleTag"></param>
        /// <param name="pokemon"></param>
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
        public async Task SendTeamOrderAsync(string battleTag, string teamOrder, int turn)
        {
            await SendAsync(battleTag, $"/team {teamOrder}", turn.ToString());

        }
        /// <summary>
        /// 加入房间
        /// </summary>
        /// <param name="battleTag"></param>
        /// <returns></returns>
        public async Task SendJoinAsync(string battleTag)
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

        /// <summary>
        /// 更改你的队伍，这个队伍需要
        /// </summary>
        /// <param name="team"></param>
        public async Task ChangeYourTeamAsync(string team)
        {
            string data = $"/utm {team}";
            await SendAsync("", data);
        }

        /// <summary>
        /// 接受挑战
        /// </summary>
        /// <param name="name"></param>
        public async Task AcceptChallengeAsync(string name)
        {
            string data = $"/accept {name}";
            await SendAsync("", data);
        }

        /// <summary>
        /// 拒绝挑战
        /// </summary>
        /// <param name="name"></param>
        public async Task RejectChallengeAsync(string name)
        {
            string data = $"/reject {name}";
            await SendAsync("", data);
        }

        /// <summary>
        /// 聊天
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        public async Task ChatWithIdAsync(string name, string msg)
        {
            string data = $"/pm {name}, {msg}";
            await SendAsync("", data);
        }

        public async Task GetUserDetails(string name)
        {
            string data = $"/cmd userdetails {name}";
            await SendAsync("", data);
        }

        public async Task<List<RankData>> GetRankAsync(string name)
        {
            var res = await _client.GetAsync($"https://play.pokemonshowdown.com/~~showdown/action.php?act=ladderget&user={name}");
            
            var data = (await res.Content.ReadAsStringAsync())[1..];
            return JsonSerializer.Deserialize<List<RankData>>(data);
        }
    }

    public class RankData
    {
        [JsonPropertyName("elo")]
        public string ELO{get;set;}
        [JsonPropertyName("formatid")]
        public string FormatId
        {
            get;set;
        }

    }
}
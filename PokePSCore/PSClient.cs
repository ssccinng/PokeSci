using System.Collections.Generic;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace PokePSCore
{
    enum MsgType
    {
        Receive,
        Send,
    }

    public enum Usage
    {
        Standby = 0,
        ChallOwner = 1,
        Search = 2,
    }

    public partial class PSClient
    {
        /// <summary>
        /// 对战字典
        /// </summary>
        public Dictionary<string, PsBattle> Battles = new();

        private Action<string> WriteLog;
        public Action UpdateUser;
        /// <summary>
        /// 参数为playerid和规则
        /// </summary>
        public Action<string, string> ChallengeAction;
        /// <summary>
        /// 参数为playerid和信息
        /// </summary>
        public Action<string, string> ChatAction;
        public Action<PsBattle> RequestsAction;
        //public Action<string, string> RequestsAction;
        public Action<string> UserDetailsAction;


        public Action<PsBattle> OnTeampreview;
        public Action<PsBattle, bool[]> OnForceSwitch;
        public Action<PsBattle> OnChooseMove;
        /// <summary>
        /// 参数为对战和是否赢了
        /// </summary>
        public Action<PsBattle, bool> BattleEndAction;
        public Action<PsBattle> BattleStartAction;
        public Action<PsBattle, string> BattleErrorAction;

        public Action<PsBattle, string> BattleInfo;

        private string _challId;

        private string _chall;
        // 设计事件

        public readonly string UserName;
        public readonly string Password;

        private HttpClient _client = new();
        private ClientWebSocket _webSocket = new();
        private bool _disposed = false;

        private string _psServer =
            //$"wss://sim.smogon.com/showdown/websocket";
            $"ws://sim.smogon.com:8000/showdown/websocket";

        private string _loginUrl1 = "https://play.pokemonshowdown.com/action.php?";

        //private string _loginUrl = "https://play.pokemonshowdown.com/~~showdown/action.php";
        private string _loginUrl = "https://play.pokemonshowdown.com/~~showdown/action.php";
        public PSClient(string userName, string pwd, string wsUrl = $"ws://sim.smogon.com:8000/showdown/websocket")
        {
            _psServer = wsUrl;
            UserName = userName;
            Password = pwd;

            _client.DefaultRequestHeaders.Add("cookies", "_ga=GA1.2.547000015.1647085978; __qca=P0-1033982634-1650164856242; __utmz=61629613.1659237138.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none); __gads=ID=628bdec33c4b1d78-22c9b75bb2d60028:T=1647085979:RT=1664088099:S=ALNI_MZUyNsXxn7brzUHqads5MNcElahtg; sid=%2C%2C3ff09a32f987b34265df04e71674654814; __utma=61629613.547000015.1647085978.1673344533.1678607451.3; _gid=GA1.2.2022799920.1680054479; _gat_gtag_UA_26211653_1=1; __gpi=UID=000004e0bb5e4cb8:T=1650164878:RT=1680054479:S=ALNI_MacRt3qyvqUR3XUDXFY0XL92XHqfg; _gat=1");
        }

        public async Task ConnectAsync()
        {
            if (!Uri.TryCreate(_psServer.Trim(), UriKind.Absolute, out Uri? webSocketUri))
            {
                return;
            }

            await _webSocket
                .ConnectAsync(webSocketUri, CancellationToken.None);
            new Thread(RecvMessage).Start();
        }

        private async void RecvMessage()
        {
            while (!_disposed)
            {
                var rcvBytes = new byte[25000];
                var rcvBuffer = new ArraySegment<byte>(rcvBytes);
                WebSocketReceiveResult rcvResult = await _webSocket.ReceiveAsync(rcvBuffer, CancellationToken.None);
                if (rcvResult?.MessageType != WebSocketMessageType.Text)
                {
                    Console.WriteLine("未知信息");
                    continue;
                }

                byte[] msgBytes = rcvBuffer.ToArray();
                var res = Encoding.UTF8.GetString(msgBytes, 0, rcvResult.Count);
                await WriteLogAsync(res, MsgType.Receive);
                await ExcuteMessageAsync(res);

                //.Split('|');
            }
        }


        /// <summary>
        /// 设置log输出位置
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public PSClient LogTo(Action<string> action)
        {
            WriteLog = action;
            return this;
        }

        /// <summary>
        /// 登陆账号
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="challId"></param>
        /// <param name="chall"></param>
        /// <returns></returns>
        public async Task<bool> LoginAsync(string userName, string password, string challId, string chall)
        {

            //var res = await _client.PostAsJsonAsync(_loginUrl1, new
            //{
            //    act = "login",
            //    name = userName,
            //    pass = password,
            //    challstr = $"{challId}%7C{chall}"
            //});
            var res = await _client.PostAsync($"{_loginUrl}?", new StringContent($"act=login&name={userName}&pass={password}&challstr={$"{challId}%7C{chall}"}"));
            //MultipartFormDataContent data1 = new()
            //{
            //    { new StringContent("login"), "act" },
            //    { new StringContent(userName), "name" },
            //    { new StringContent(password), "pass" },
            //    { new StringContent($"{challId}%7C{chall}"), "challstr" }
            //};
            //var res = await _client.PostAsync(_loginUrl, data1);
            Console.WriteLine(res.IsSuccessStatusCode);
            var dd = (await res.Content.ReadAsStringAsync())[1..];
            JsonElement data = JsonDocument.Parse((await res.Content.ReadAsStringAsync())[1..]).RootElement;
            if (!data.GetProperty("curuser").GetProperty("loggedin").GetBoolean()) return false;
            await SendAsync("", $"/trn {userName},0,{data.GetProperty("assertion").GetString()}");
            //await SendMessage("", "/avatar 159");
            await SetAvatarAsync("lillie");
            return true;
        }
        /// <summary>
        /// 分析对局数据
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="usage"></param>
        private async Task BattleTagAsync(string msg, Usage usage = Usage.Standby)
        {
            string[] battleData = msg.Split('\n');
            string tag = battleData[0].Split('|')[0][1..];
            Console.WriteLine($"tag: {tag}");
            var battle = Battles.GetValueOrDefault(tag) ?? new PsBattle(this, tag);
            BattleInfo?.Invoke(battle, msg);

            for (int i = 1; i < battleData.Length; i++)
            {
                string[] currData = battleData[i].Split('|');
                if (currData.Length < 2) continue;
                string cmd = currData[1];
                string[] other = currData[2..];
                switch (cmd)
                {
                    case "init":
                        Battles.TryAdd(tag, battle);
                        BattleStartAction?.Invoke(battle);
                        break;
                    case "player":
                        if (other[1] == UserName)
                        {
                            battle.PlayerPos = other[0] == "p1" ? PlayerPos.Player1 : PlayerPos.Player2;
                            Console.WriteLine("玩家位置: " + battle.PlayerPos);
                            if (battle.PlayerPos == PlayerPos.Player1)
                            {
                                battle.Player1 = UserName;
                            }
                            else
                            {
                                // battle.Turn++;
                                battle.Player2 = UserName;
                            }
                        }
                        else
                        {
                            if (other[0] == "p1")
                            {
                                battle.Player1 = other[1];
                            }
                            else
                            {
                                battle.Player2 = other[1];
                            }
                        }
                        break;
                    case "request":
                        if (other[0] != "")
                        {
                            // battle.Turn += 2;

                            //if (JsonDocument.Parse(other[0]).RootElement.TryGetProperty("forceSwitch", out var c))
                            //{
                            //    await SendSwitchAsync(tag, ++battle.idx, battle.Turn);
                            //}
                            // 随机队伍信息
                            // 应该还需要拆分
                            if (other[0].Length == 1)
                            {
                                await battle.RefreshByRequestAsync(other[1].Split('\n')[1]);
                                // RequestsAction?.Invoke(tag, other[1].Split('\n')[1]);
                                // other[1].split('\n')[1] 为队伍信息
                            }
                            else
                            {
                                await battle.RefreshByRequestAsync(other[0]);
                                // RequestsAction?.Invoke(tag, other[0]);

                                // other[0] 为队伍信息
                            }
                        }
                        RequestsAction?.Invoke(battle);

                        break;
                    case "teampreview":
                        // 后面还有个
                        OnTeampreview?.Invoke(battle);
                        // 选择队伍
                        // 可能需要事件通知
                        // MakeOrder
                        break;
                    case "turn":
                        // 回合开始
                        // 做出操作
                        // MakeAction
                        Console.WriteLine("到了turn");
                        OnChooseMove?.Invoke(battle);
                        // await SendMoveAsync(tag, 1, battle.Turn);
                        break;
                    case "callback":
                        if (other[0] == "trapped")
                        {
                            // makemove
                            // 招式失败 好像
                        }
                        break;
                    case "poke":
                        if (other[0] == (battle.PlayerPos == PlayerPos.Player1 ? "p2" : "p1"))
                        {
                            // 对手的队伍信息
                            var data = other[1].Split(", ");
                            // 0 名字 1 等级
                            await battle.InitOppTeamAsync(data[0]);
                            //battle.OppTeam;
                            Console.WriteLine(other[1]);
                            //Console.WriteLine("让我康康");
                        }
                        break;
                    case "win":
                        Console.WriteLine("对战结束");
                        BattleEndAction?.Invoke(battle, other[0].Contains(UserName));
                        Battles.Remove(battle.Tag);
                        // 对战结束
                        break;
                    case "error":
                        BattleErrorAction?.Invoke(battle, other[0]);
                        // 出现异常
                        break;
                    default:
                        battle.LogParse(cmd, other);
                        break;
                }
            }
        }

        public async Task<bool> LoginAsync()
        {
            return await LoginAsync(UserName, Password, _challId, _chall);
        }

        /// <summary>
        /// 设置头像
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task SetAvatarAsync(string id)
        {
            await SendAsync("", $"/avatar {id}");
        }

        /// <summary>
        /// 分析信息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task ExcuteMessageAsync(string message, Usage usage = Usage.Standby)
        {
            Console.WriteLine(message);
            Debug.WriteLine(message);
            var data = message.Split('|');
            Console.WriteLine(data[0]);
            if (data[0].Contains("battle"))
            {
                // 对战数据
                Console.WriteLine("这里是对战...");
                await BattleTagAsync(message, usage);
            }
            else
            {
                switch (data[1])
                {
                    case "challstr":
                        _challId = data[2];
                        _chall = data[3];
                        break;
                    case "updateuser":
                        UpdateUser?.Invoke();
                        // 用户更新 可认为登陆完毕（？
                        break;
                    case "updateseach":
                        // 更新房间，json
                        // {"searching":[],"games":{"battle-gen8randombattle-1605790743":"[Gen 8] Random Battle"}}
                        break;
                    case "deinit":
                        // 对战结束
                        break;
                    case "queryresponse":
                        switch (data[2])
                        {
                            case "roomlist":
                                // 想办法传回去
                                break;
                            case "userdetails":
                                UserDetailsAction?.Invoke(data[3]);
                                break;
                            default:
                                break;
                        }

                        break;
                    case "updatechallenges":
                        if (data[2].Split("\"")[3] != "challengeTo")
                        {
                        }

                        break;
                    case "pm":
                        string pmP1 = data[2].Trim(), pmP2 = data[3].Trim();
                        if (data[4].StartsWith('/'))
                        {
                            string[] cmd = data[4].Split(' ');
                            // foreach (var cmdd in cmd)
                            // {
                            //     Console.WriteLine(cmdd);
                            // }
                            switch (cmd[0])
                            {
                                case "/challenge":
                                    if (cmd.Length == 1)
                                    {
                                        await WriteLogAsync($"{pmP2}取消挑战", MsgType.Receive);
                                        break;
                                    }

                                    string rule = cmd[1];
                                    string rule2 = data[5];
                                    // Console.WriteLine($"rule2: {rule}");
                                    if (pmP1 == UserName)
                                    {
                                        await WriteLogAsync($"发起对{pmP2}挑战, 规则：{rule}", MsgType.Receive);
                                    }
                                    else
                                    {
                                        await WriteLogAsync($"收到{pmP1}挑战, 规则：{rule}", MsgType.Receive);
                                        // 触发挑战事件
                                        ChallengeAction?.Invoke(pmP1, rule);
                                    }

                                    break;
                                case "/log":
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            // 聊天信息
                            await WriteLogAsync($"[聊天信息] {pmP1} -> {pmP2}: {data[4]}", MsgType.Receive);
                            if (pmP2 == UserName)
                            {
                                ChatAction?.Invoke(pmP1, data[4]);
                                // if (data[4].StartsWith("临流"))
                                // {
                                //     await ChatWithIdAsync(pmP1, "临流揽镜曳双魂, 落红逐青裙");
                                // }
                                // if (data[4].StartsWith("依稀"))
                                // {
                                //     await ChatWithIdAsync(pmP1, "依稀往梦幻如真, 泪湿千里云");
                                // }
                                // if (data[4].StartsWith("风骤暖"))
                                // {
                                //     await ChatWithIdAsync(pmP1, "风骤暖, 草渐新, 年年秋复春");
                                // }
                                // if (data[4].StartsWith("温香"))
                                // {
                                //     await ChatWithIdAsync(pmP1, "温香软玉燕依人, 再启生死门");
                                // }
                            }
                        }

                        if (data[2].Trim() == UserName)
                        {
                            // 我发起的挑战
                        }

                        break;

                    default:
                        break;
                }
            }
            // if (data[0].Contains("battle"))
            // {
            //     // 为对战数据
            // }
        }

        public void DisconnectAsync()
        {
            _disposed = true;
            _webSocket.Dispose();
        }

        private async Task WriteLogAsync(string msg, MsgType type)
        {
            await Task.Run(() =>
            {
                switch (type)
                {
                    case MsgType.Receive:
                        WriteLog?.Invoke($"[{DateTime.Now:s}] << {msg}");
                        break;
                    case MsgType.Send:
                        WriteLog?.Invoke($"[{DateTime.Now:s}] >> {msg}");
                        break;
                    default:
                        break;
                }
            });
        }
    }
}
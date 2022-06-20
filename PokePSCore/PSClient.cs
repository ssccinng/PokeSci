using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace PokePSCore
{
    enum MsgType
    {
        Receive,
        Send,
    }
    public partial class PSClient
    {

        private Action<string> WriteLog;
        private Action UpdateUser;
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
        private string _loginUrl = "https://play.pokemonshowdown.com/action.php?";
        //private string _loginUrl = "https://play.pokemonshowdown.com/~~showdown/action.php";
        public PSClient(string userName, string pwd, string wsUrl = $"ws://sim.smogon.com:8000/showdown/websocket")
        {
            _psServer = wsUrl;
            UserName = userName;
            Password = pwd;
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
                await ExcuteMessageAsync(res);
                await WriteLogAsync(res, MsgType.Receive);

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
            var res = await _client.PostAsJsonAsync(_loginUrl, new
            {
                act = "login",
                name = userName,
                pass = password,
                challstr = $"{challId}%7C{chall}"
            });
            var dd = (await res.Content.ReadAsStringAsync())[1..];
            JsonElement data = JsonDocument.Parse((await res.Content.ReadAsStringAsync())[1..]).RootElement;
            if (!data.GetProperty("curuser").GetProperty("loggedin").GetBoolean()) return false;
            await SendAsync("", $"/trn {userName},0,{data.GetProperty("assertion").GetString()}");
            //await SendMessage("", "/avatar 159");
            await SetAvatarAsync("lillie");
            return true;
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
        private async Task ExcuteMessageAsync(string message)
        {
            var data = message.Split('|');
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
                case "deinit":
                    // 对战结束
                    break;
                case "queryresponse":
                    switch (data[2])
                    {
                        case "roomlist":
                            // 想办法传回去
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            if (data[0].Contains("battle"))
            {
                // 为对战数据
            }
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
                        WriteLog?.Invoke($"<< {msg}");
                        break;
                    case MsgType.Send:
                        WriteLog?.Invoke($">> {msg}");
                        break;
                    default:
                        break;
                }
            });
            
        }


    }
}

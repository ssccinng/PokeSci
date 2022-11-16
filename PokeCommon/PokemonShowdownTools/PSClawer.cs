using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace PokeCommon.PokemonShowdownTools
{
    /// <summary>
    /// PS爬虫
    /// </summary>
    public class PSSocket
    {
        private ClientWebSocket _webSocket = new();

        public LinkedList<string> Messages = new();

        // 定时发出check
        public Timer timer;

        // 房间被创建事件
        public Action<string> BattleRoomCreate;
        //public Action<string> BattleRoomCreate;


        public Dictionary<string, DateTime> RoomSet = new();
        public int LimitScore = 1200;
        public string RuleName = "gen8vgc2021series10";
        public async Task Start(string IP = "")
        {
            //ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;
            var PSServer =
                    //$"wss://sim.smogon.com/showdown/websocket";
                    $"ws://sim.smogon.com:8000/showdown/websocket";
            if (!Uri.TryCreate(PSServer.Trim(), UriKind.Absolute, out Uri webSocketUri))
            {
                return;
            }

            //_webSocket = new ClientWebSocket();
            //await _webSocket
            //    .ConnectAsync(webSocketUri, cts.Token);
            //_webSocket.Options.
            //_webSocket.Options.ClientCertificates = (new X509CertificateCollection()).;
            //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            await _webSocket
                .ConnectAsync(webSocketUri, CancellationToken.None);
            var a = RecvMessage();
            CheckMsg();
            await a;
        }

        public async Task CheckMsg()
        {
            while (true)
            {
                await _webSocket.SendAsync(Encoding.UTF8.GetBytes($"|/cmd roomlist {RuleName},{LimitScore},"), WebSocketMessageType.Text, true, CancellationToken.None);
                await Task.Delay(20000);
            }

        }

        public async Task RecvMessage()
        {

            while (true)
            {
                var rcvBytes = new byte[25000];
                var rcvBuffer = new ArraySegment<byte>(rcvBytes);
                WebSocketReceiveResult rcvResult = await _webSocket.ReceiveAsync(rcvBuffer, CancellationToken.None);
                //Console.WriteLine("接受成功!");
                if (rcvResult?.MessageType != WebSocketMessageType.Text)
                {
                    Console.WriteLine("未知信息");
                    continue;
                }
                byte[] msgBytes = rcvBuffer.ToArray();
                var res = Encoding.UTF8.GetString(msgBytes, 0, rcvResult.Count).Split('|');
                //if (res[1] == "queryresponse")
                //Console.WriteLine(Encoding.UTF8.GetString(rcvBuffer));
                //Console.WriteLine($"res[1] = '{res[1]}'");
                //Messages.AddLast(Encoding.UTF8.GetString(rcvBuffer));

                if (res[0].Length > 0 && res[0][0] == 'a')
                {
                    int aa = 1;
                }
                switch (res[1].Trim())
                {
                    case "queryresponse":

                        //var roomlist = JsonDocument.Parse(res[3]);
                        var roomlist = JsonDocument.Parse(res[3].Trim()).RootElement.GetProperty("rooms");
                        var kk = roomlist.EnumerateObject();
                        foreach (var item in kk)
                        {
                            RoomSet[item.Name] = DateTime.Now;
                            //Console.WriteLine(item.Name);
                            //Console.WriteLine(item.Value);
                        }

                        foreach (var item in RoomSet.ToList())
                        {
                            if (DateTime.Now - item.Value > TimeSpan.FromSeconds(90))
                            {
                                Console.WriteLine($"{item.Key} 房间对战结束");
                                await _webSocket.SendAsync(Encoding.UTF8.GetBytes($"|/join {item.Key}"), WebSocketMessageType.Text, true, CancellationToken.None);
                                await _webSocket.SendAsync(Encoding.UTF8.GetBytes($"|/noreply /leave {item.Key}"), WebSocketMessageType.Text, true, CancellationToken.None);

                                RoomSet.Remove(item.Key);
                            }
                        }
                        break;
                    case "init":
                        var data1 = Encoding.UTF8.GetString(rcvBuffer);
                        var data = data1.Split('\n');
                        PsBattle pb = new()
                        {
                            Title = res[0].Substring(1),
                            BattleId = int.Parse(res[0].Split('-')[2]),
                            Filename = $"{int.Parse(res[0].Split('-')[2])}.html",
                            Battledate = DateTime.Today.AddDays(-3).AddDays(-(int)DateTime.Today.AddDays(-3).DayOfWeek).AddDays(3),
                            // Battledate = DateTime.Today.AddDays(-4).AddDays(-(int)DateTime.Today.DayOfWeek),
                        };
                        PSReplayThonk.Thonk(data, ref pb);

                        //PSDisplayContext pd = new();
                        //pd.Psbattles.Add(pb);
                        //var td = pd.Teamdata.FirstOrDefault(s =>
                        //    s.Poke1 == pb.Player1poke1 &&
                        //    s.Poke2 == pb.Player1poke2 &&
                        //    s.Poke3 == pb.Player1poke3 &&
                        //    s.Poke4 == pb.Player1poke4 &&
                        //    s.Poke5 == pb.Player1poke5 &&
                        //    s.Poke6 == pb.Player1poke6 &&
                        //    s.Teamdate == pb.Battledate);
                        //if (td == null)
                        //{
                        //    Teamdatum teamdatum = new()
                        //    {
                        //        Poke1 = pb.Player1poke1,
                        //        Poke2 = pb.Player1poke2,
                        //        Poke3 = pb.Player1poke3,
                        //        Poke4 = pb.Player1poke4,
                        //        Poke5 = pb.Player1poke5,
                        //        Poke6 = pb.Player1poke6,

                        //        Highestrating = pb.Rating1,
                        //        Totalcnt = 1,
                        //        Win = pb.Whowin == 0 ? 1 : 0,
                        //        Teamdate = pb.Battledate
                        //    };
                        //    //pd.Teamdata.Add(teamdatum);
                        //}
                        //else
                        //{
                        //    td.Totalcnt++;
                        //    if (pb.Whowin == 0) td.Win++;
                        //    td.Highestrating = Math.Max(td.Highestrating, pb.Rating1);
                        //}

                        //var td1 = pd.Teamdata.FirstOrDefault(s =>
                        //    s.Poke1 == pb.Player2poke1 &&
                        //    s.Poke2 == pb.Player2poke2 &&
                        //    s.Poke3 == pb.Player2poke3 &&
                        //    s.Poke4 == pb.Player2poke4 &&
                        //    s.Poke5 == pb.Player2poke5 &&
                        //    s.Poke6 == pb.Player2poke6 &&
                        //    s.Teamdate == pb.Battledate);
                        //if (td1 == null)
                        //{
                        //    Teamdatum teamdatum = new()
                        //    {
                        //        Poke1 = pb.Player2poke1,
                        //        Poke2 = pb.Player2poke2,
                        //        Poke3 = pb.Player2poke3,
                        //        Poke4 = pb.Player2poke4,
                        //        Poke5 = pb.Player2poke5,
                        //        Poke6 = pb.Player2poke6,

                        //        Highestrating = pb.Rating2,
                        //        Totalcnt = 1,
                        //        Win = pb.Whowin == 1 ? 1 : 0,
                        //        Teamdate = pb.Battledate
                        //    };
                        //    //pd.Teamdata.Add(teamdatum);
                        //}
                        //else
                        //{
                        //    td1.Totalcnt++;
                        //    if (pb.Whowin == 1) td1.Win++;
                        //    td1.Highestrating = Math.Max(td1.Highestrating, pb.Rating2);
                        //}
                        //await pd.SaveChangesAsync();
                        Console.WriteLine($"{pb.BattleId} 记录成功");
                        string str1 = @"</title>
    <style>
    html,body {font-family:Verdana, sans-serif;font-size:10pt;margin:0;padding:0;}body{padding:12px 0;} .battle-log {font-family:Verdana, sans-serif;font-size:10pt;} .battle-log-inline {border:1px solid #AAAAAA;background:#EEF2F5;color:black;max-width:640px;margin:0 auto 80px;padding-bottom:5px;} .battle-log .inner {padding:4px 8px 0px 8px;} .battle-log .inner-preempt {padding:0 8px 4px 8px;} .battle-log .inner-after {margin-top:0.5em;} .battle-log h2 {margin:0.5em -8px;padding:4px 8px;border:1px solid #AAAAAA;background:#E0E7EA;border-left:0;border-right:0;font-family:Verdana, sans-serif;font-size:13pt;} .battle-log .chat {vertical-align:middle;padding:3px 0 3px 0;font-size:8pt;} .battle-log .chat strong {color:#40576A;} .battle-log .chat em {padding:1px 4px 1px 3px;color:#000000;font-style:normal;} .chat.mine {background:rgba(0,0,0,0.05);margin-left:-8px;margin-right:-8px;padding-left:8px;padding-right:8px;} .spoiler {color:#BBBBBB;background:#BBBBBB;padding:0px 3px;} .spoiler:hover, .spoiler:active, .spoiler-shown {color:#000000;background:#E2E2E2;padding:0px 3px;} .spoiler a {color:#BBBBBB;} .spoiler:hover a, .spoiler:active a, .spoiler-shown a {color:#2288CC;} .chat code, .chat .spoiler:hover code, .chat .spoiler:active code, .chat .spoiler-shown code {border:1px solid #C0C0C0;background:#EEEEEE;color:black;padding:0 2px;} .chat .spoiler code {border:1px solid #CCCCCC;background:#CCCCCC;color:#CCCCCC;} .battle-log .rated {padding:3px 4px;} .battle-log .rated strong {color:white;background:#89A;padding:1px 4px;border-radius:4px;} .spacer {margin-top:0.5em;} .message-announce {background:#6688AA;color:white;padding:1px 4px 2px;} .message-announce a, .broadcast-green a, .broadcast-blue a, .broadcast-red a {color:#DDEEFF;} .broadcast-green {background-color:#559955;color:white;padding:2px 4px;} .broadcast-blue {background-color:#6688AA;color:white;padding:2px 4px;} .infobox {border:1px solid #6688AA;padding:2px 4px;} .infobox-limited {max-height:200px;overflow:auto;overflow-x:hidden;} .broadcast-red {background-color:#AA5544;color:white;padding:2px 4px;} .message-learn-canlearn {font-weight:bold;color:#228822;text-decoration:underline;} .message-learn-cannotlearn {font-weight:bold;color:#CC2222;text-decoration:underline;} .message-effect-weak {font-weight:bold;color:#CC2222;} .message-effect-resist {font-weight:bold;color:#6688AA;} .message-effect-immune {font-weight:bold;color:#666666;} .message-learn-list {margin-top:0;margin-bottom:0;} .message-throttle-notice, .message-error {color:#992222;} .message-overflow, .chat small.message-overflow {font-size:0pt;} .message-overflow::before {font-size:9pt;content:'...';} .subtle {color:#3A4A66;}
    </style>
    <div class=""wrapper replay-wrapper"" style=""max-width:1180px;margin:0 auto"">
    <input type=""hidden"" name=""replayid"" value=""gen7vgc2019ultraseries-910491074"" />
    <div class=""battle""></div><div class=""battle-log""></div><div class=""replay-controls""></div><div class=""replay-controls-2""></div>
    <script type=""text/plain"" class=""battle-log-data"">";

                        string str2 = @"</script>
    </div>
    <script>
    let daily = Math.floor(Date.now()/1000/60/60/24);document.write('<script src=""https://play.pokemonshowdown.com/js/replay-embed.js?version'+daily+'""></'+'script>');
    </script>";
                        System.IO.File.WriteAllText($"/www/wwwroot/replay.mypokemon.top/PSreplay/{pb.Filename}", str1 + data1 + str2);
                        break;
                    default:
                        break;
                }

                //await ReadMessage(msgBytes);
                //Console.WriteLine("处理成功!");


            }
        }
    }
}
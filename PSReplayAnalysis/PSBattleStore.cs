using PSReplayAnalysis;
using PSReplayAnalysis.PokeLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PSReplayAnalysis
{
    public partial class PSReplayAnalysis
    {
        public required string RoomId { get; set; }

        public BattleData BattleData { get; set; } = new BattleData();

        public void Refresh(string data)
        {
            var datalines = data.Split('\n');

            foreach (var line in datalines)
            {
                var lastTurn = BattleData.BattleTurns.Last();
                string[] d = line.Split('|');
                if (d.Length < 2) continue;
                switch (d[1])
                {
                    // move要结算一次
                    case "player":
                        if (d.Length > 5)
                        {
                            int.TryParse(d[5], out int res);
                            if (d[2] == "p1")
                            {
                                BattleData.Player1Id = d[3];
                                BattleData.Player1Score = res; // 撕烤为0
                            }
                            else
                            {
                                BattleData.Player2Id = d[3];
                                BattleData.Player2Score = res; // 撕烤为0
                            }
                        }
                        break;

                    case "poke":
                        // 后者才是真名
                        var pspoke = PokeToId(d[3].Split(',')[0]);

                        if (d[2] == "p1")
                        {
                            // 赋予种族
                            lastTurn.Player1Team.Pokemons.Add(new Pokemon
                            {
                                Id = pspoke.id,
                                NickName = pspoke.poke.name,
                            });

                        }
                        else
                        {
                            lastTurn.Player2Team.Pokemons.Add(new Pokemon
                            {
                                Id = pspoke.id,
                                NickName = pspoke.poke.name,
                            });
                        }
                        break;

                    case "turn":
                        // next turn
                        BattleData.BattleTurns.Add(lastTurn.NextTurn());
                        break;
                    case "switch":
                        // 切换
                        var switchName = d[3].Split(',')[0];
                        var switchNickName = d[2][5..];
                        var swpoke = PokeToId(switchName);
                        var swdata = GetSidePos(d[2][..3]);
                        // 检测到不合理的话 直接一手把索罗亚克切换成那个 Todo: 记得处理一下
                        // 必须是开局的switch 才能加入
                        if (swdata.side == 1)
                        {
                            if (lastTurn.Player1Team.Pokemons.Any(s => s.NowPos == swdata.pos))
                            {
                                lastTurn.Player1Team.Pokemons.First(s => s.NowPos == swdata.pos).NowPos = -1;
                            }
                            // 还不行 要考虑到索罗亚直接换上来..
                            Pokemon pokemon = lastTurn.Player1Team.Pokemons.First(s => s.Id == swpoke.id && s.NowPos < 0);
                            pokemon.NowPos = swdata.pos;
                            pokemon.NickName = switchNickName;

                        }
                        else
                        {
                            if (lastTurn.Player2Team.Pokemons.Any(s => s.NowPos == swdata.pos))
                            {
                                lastTurn.Player2Team.Pokemons.First(s => s.NowPos == swdata.pos).NowPos = -1;
                            }

                            Pokemon pokemon = lastTurn.Player2Team.Pokemons.First(s => s.Id == swpoke.id && s.NowPos < 0);
                            pokemon.NowPos = swdata.pos;
                            pokemon.NickName = switchNickName;
                        }

                        break;
                    case "-terastallize":
                        var teraType = d[3];
                        var tsSide = GetSidePos(d[2]);
                        if (tsSide.side == 1)
                        {
                            lastTurn.Player1Team.Pokemons.First(s => s.NowPos == tsSide.pos).TeraType = teraType;
                        }
                        else
                        {
                            lastTurn.Player2Team.Pokemons.First(s => s.NowPos == tsSide.pos).TeraType = teraType;
                        }
                        break;
                    case "detailschange":
                        // 状态改变
                        var dcdata = GetSidePos(d[2][..3]);
                        var dcDetail = d[3].Split(',');
                        var dcName = dcDetail[0];
                        var dcPokeId = PokeToId(dcName);
                        if (dcdata.side == 1)
                        {
                            lastTurn.Player1Team.Pokemons.First(s => s.NowPos == dcdata.pos).Id = dcPokeId.id;
                        }
                        else
                        {
                            lastTurn.Player2Team.Pokemons.First(s => s.NowPos == dcdata.pos).Id = dcPokeId.id;
                        }
                        break;
                    case "-ability":
                        // 发动特性
                        break;

                    case "-singleturn":
                        // 单回合状态Rage Powder
                        var stStageSide = GetSidePos(d[2]);
                        var stStage = d[3].Split(":").Last().Replace(" ", "").Trim();
                        var stProp = typeof(PokemonStatus).GetProperty(stStage);
                        if (stProp == null)
                        {
                            // Console.WriteLine($"stStage: {stStage}为null");
                            continue;
                        }
                        if (stStageSide.side == 1)
                        {
                            stProp.SetValue(lastTurn.Side1Pokes[stStageSide.pos], 1);
                        }
                        else if (stStageSide.side == 2)
                        {
                            stProp.SetValue(lastTurn.Side2Pokes[stStageSide.pos], 1);

                        }
                        break;
                    case "-damage":
                        var hpr = d[3].Split('/');
                        // 这个后面有状态 可能要记录在当前状态
                        var hpn = int.Parse(hpr[0].Replace(" fnt", ""));
                        var damageStageSide = GetSidePos(d[2]);

                        if (damageStageSide.side == 1)
                        {
                            Pokemon pokemon = lastTurn.Player1Team.Pokemons.First(s => s.NowPos == damageStageSide.pos);
                            var delta = pokemon.HPRemain - hpn;
                            pokemon.HPRemain = hpn;
                            lastTurn.Reward1 += delta;
                            lastTurn.Reward2 -= delta;
                        }
                        else if (damageStageSide.side == 2)
                        {
                            var pokemon = lastTurn.Player2Team.Pokemons.First(s => s.NowPos == damageStageSide.pos);
                            var delta = lastTurn.Player2Team.Pokemons.First(s => s.NowPos == damageStageSide.pos).HPRemain - hpn;
                            pokemon.HPRemain = hpn;
                            lastTurn.Reward2 += delta;
                            lastTurn.Reward1 -= delta;
                        }

                        break;
                    case "drag":
                        var dragSide = GetSidePos(d[2]);
                        var dragPokeId = PokeToId(d[3].Split(',')[0]);

                        if (dragSide.side == 1)
                        {
                            if (lastTurn.Player1Team.Pokemons.Any(s => s.NowPos == dragSide.pos))
                            {
                                lastTurn.Player1Team.Pokemons.First(s => s.NowPos == dragSide.pos).NowPos = -1;
                            }
                            lastTurn.Player1Team.Pokemons.First(s => s.Id == dragPokeId.id).NowPos = dragSide.pos;
                        }
                        else
                        {
                            if (lastTurn.Player2Team.Pokemons.Any(s => s.NowPos == dragSide.pos))
                            {
                                lastTurn.Player2Team.Pokemons.First(s => s.NowPos == dragSide.pos).NowPos = -1;
                            }
                            lastTurn.Player2Team.Pokemons.First(s => s.Id == dragPokeId.id).NowPos = dragSide.pos;
                        }

                        break;
                    case "faint":
                        var faintSide = GetSidePos(d[2]);
                        if (faintSide.side == 1)
                        {
                            lastTurn.Player1Team.Pokemons.First(s => s.NowPos == faintSide.pos).NowPos = -2;
                        }
                        else if (faintSide.side == 2)
                        {
                            lastTurn.Player2Team.Pokemons.First(s => s.NowPos == faintSide.pos).NowPos = -2;
                        }
                        // 被击倒
                        // 坐标归-1
                        break;
                    case "move":
                        // 写出选择
                        // 结算一次状态
                        // 使用技能
                        var moveSide = GetSidePos(d[2]);
                        var moveTargetSide = GetSidePos(d[4]);
                        var moveName = d[3].Split(',')[0];
                        var moveId = Pokemondata.EnglishNametoMoveID(moveName);

                        //var moveId = MoveToId(moveName);

                        // 2 3
                        // 0 1
                        if (moveSide.side == 1)
                        {
                            // 有可能为空 对手
                            if (moveTargetSide.pos == -1)
                            {
                                int aa = 123;
                            }
                            //var tpos = moveTargetSide.pos == moveSide.pos;
                            lastTurn.Battle1Actions[moveSide.pos] = new ExporttoTrainData.BattleAction(moveId, moveTargetSide.pos);
                        }
                        else if (moveSide.side == 2)
                        {
                            moveSide.pos = 3 - moveSide.pos;
                            moveTargetSide.pos = 3 - moveTargetSide.pos;
                            lastTurn.Battle2Actions[moveSide.pos % 2] = new ExporttoTrainData.BattleAction(moveId, moveTargetSide.pos);
                            //lastTurn.Player2Team.Pokemons.First(s => s.NowPos == moveSide.pos).LastMove = moveId.num;
                            if (moveTargetSide.pos == -1)
                            {
                                int aa = 123;
                            }
                        }
                        break;
                    case "-heal":
                        if (d[2].Contains("Ui ui ui ui"))
                        {
                            int cc = 0;
                        }
                        var hph = d[3].Split('/');
                        // 这个后面有状态 可能要记录在当前状态
                        var hphn = int.Parse(hph[0].Replace(" fnt", ""));

                        var healStageSide = GetSidePos(d[2]);
                        if (d[2].StartsWith("p1:"))
                        {
                            var poke = GetPoke(BattleData, 1, d[2][4..]);
                            //Pokemon pokemon = lastTurn.Player1Team.Pokemons.First(s => s.Id == poke.num);
                            poke.HPRemain = hphn;
                            poke.NowPos = -1;
                        }
                        else if (d[2].StartsWith("p2:"))
                        {
                            var poke = GetPoke(BattleData, 2, d[2][4..]);

                            //Pokemon pokemon = lastTurn.Player2Team.Pokemons.First(s => s.Id == poke.num);
                            poke.HPRemain = hphn;
                            poke.NowPos = -1;
                        }
                        else
                        {
                            if (healStageSide.side == 1)
                            {
                                var pokemon = GetPoke(BattleData, 1, d[2][5..]);

                                //Pokemon pokemon = lastTurn.Player1Team.Pokemons.First(s => s.NowPos == healStageSide.pos);
                                var delta = hphn - pokemon.HPRemain;
                                pokemon.HPRemain = hphn;
                                lastTurn.Reward1 += delta; // 可能要区分来源
                                pokemon.NowPos = healStageSide.pos;

                            }
                            else if (healStageSide.side == 2)
                            {
                                var pokemon = GetPoke(BattleData, 2, d[2][5..]);

                                //Pokemon pokemon = lastTurn.Player2Team.Pokemons.First(s => s.NowPos == healStageSide.pos);
                                var delta = hphn - pokemon.HPRemain;
                                pokemon.HPRemain = hphn;
                                lastTurn.Reward2 += delta;
                                pokemon.NowPos = healStageSide.pos;
                            }
                        }

                        break;
                    case "-fail":
                        // 失败
                        break;
                    case "-fieldstart":
                        var fsMove = d[2].Split(":").Last().Trim().Replace(" ", "");
                        var fsProp = typeof(BattleField).GetProperty(fsMove);
                        if (fsProp == null)
                        {
                            // 有场地暂时分析不了
                            // Console.WriteLine("fsProp: " + fsMove);
                            break;
                        }
                        var fsDecr = (DecreaseAttribute)fsProp.GetCustomAttribute(typeof(DecreaseAttribute));
                        fsProp.SetValue(lastTurn.AllField, fsDecr?.InitValue ?? 1);
                        break;
                    case "-fieldend":
                        var feMove = d[2].Split(":").Last().Trim().Replace(" ", "");
                        var feProp = typeof(BattleField).GetProperty(feMove);
                        if (feProp == null)
                        {
                            // Console.WriteLine("feProp: " + feMove);
                            break;
                        }
                        feProp.SetValue(lastTurn.AllField, 0);

                        break;
                    case "-sidestart":
                        // 单边状态开始
                        var sidestartMove = d[3].Split(":").Last().Trim().Replace(" ", "");
                        var ssProp = typeof(OneSideBattleField).GetProperty(sidestartMove);

                        if (ssProp == null)
                        {
                            // Console.WriteLine("ssProp: " + sidestartMove);
                            break;
                        }
                        var decreaseAttribute = (DecreaseAttribute)ssProp.GetCustomAttribute(typeof(DecreaseAttribute));

                        if (GetPlayer(d[2][..2]) == 1)
                        {
                            ssProp.SetValue(lastTurn.Side1Field, decreaseAttribute?.InitValue ?? 1);

                        }
                        else
                        {
                            ssProp.SetValue(lastTurn.Side2Field, decreaseAttribute?.InitValue ?? 1);
                        }
                        break;
                    case "-sideend":
                        var sideendMove = d[3].Split(":").Last().Trim().Replace(" ", "");
                        var seProp = typeof(OneSideBattleField).GetProperty(sideendMove);
                        if (seProp == null)
                        {
                            // Console.WriteLine(sideendMove);
                            break;
                        }
                        if (GetPlayer(d[2][..2]) == 1)
                        {
                            seProp.SetValue(lastTurn.Side1Field, 0);

                        }
                        else
                        {
                            seProp.SetValue(lastTurn.Side2Field, 0);

                        }
                        break;
                    case "-enditem":
                        // 把之前没有item的都赋值
                        // 物品用完
                        break;
                    case "-supereffective":
                        // 效果绝佳
                        break;
                    case "-resisted":
                        // 抵抗
                        break;
                    case "activate":
                        // 大概是什么起效果
                        break;
                    case "-unboost":
                        // 能力减弱

                        var unboostSide = GetSidePos(d[2]);
                        var unboostType = d[3][..3];
                        var unboostValue = int.Parse(d[4]);

                        var unboostInfo = typeof(PokemonStatus).GetProperty(
                            $"{new CultureInfo("en").TextInfo.ToTitleCase(unboostType.ToLower())}Buff");

                        if (unboostSide.side == 1)
                        {
                            // 可以函数化吧
                            // 根据DecreaseAttribute给相应成员赋值, 不能超过最大值或者最小值

                            var da = (DecreaseAttribute)unboostInfo.GetCustomAttribute(typeof(DecreaseAttribute));
                            if (da == null)
                            {
                                // Console.WriteLine($"da: {unboostType}为null");
                                continue;
                            }
                            var value = (int)unboostInfo.GetValue(lastTurn.Side1Pokes[unboostSide.pos]);

                            var newValue = value - unboostValue;
                            if (newValue > da.MaxValue)
                            {
                                newValue = da.MaxValue;
                            }
                            else if (newValue < da.MinValue)
                            {
                                newValue = da.MinValue;
                            }
                            unboostInfo.SetValue(lastTurn.Side1Pokes[unboostSide.pos], newValue);


                        }
                        else if (unboostSide.side == 2)
                        {
                            var da = (DecreaseAttribute)unboostInfo.GetCustomAttribute(typeof(DecreaseAttribute));
                            if (da == null)
                            {
                                // Console.WriteLine($"da: {unboostType}为null");
                                continue;
                            }
                            var value = (int)unboostInfo.GetValue(lastTurn.Side2Pokes[unboostSide.pos]);

                            var newValue = value + unboostValue;
                            if (newValue > da.MaxValue)
                            {
                                newValue = da.MaxValue;
                            }
                            else if (newValue < da.MinValue)
                            {
                                newValue = da.MinValue;
                            }
                            unboostInfo.SetValue(lastTurn.Side2Pokes[unboostSide.pos], newValue);
                        }
                        break;

                    case "-boost":
                        var boostSide = GetSidePos(d[2]);
                        var boostType = d[3][..3];
                        var boostValue = int.Parse(d[4]);

                        var boostInfo = typeof(PokemonStatus).GetProperty(
                            $"{new CultureInfo("en").TextInfo.ToTitleCase(boostType.ToLower())}Buff");

                        if (boostSide.side == 1)
                        {
                            // 可以函数化吧
                            // 根据DecreaseAttribute给相应成员赋值, 不能超过最大值或者最小值

                            var da = (DecreaseAttribute)boostInfo.GetCustomAttribute(typeof(DecreaseAttribute));
                            if (da == null)
                            {
                                // Console.WriteLine($"da: {boostType}为null");
                                continue;
                            }
                            var value = (int)boostInfo.GetValue(lastTurn.Side1Pokes[boostSide.pos]);

                            var newValue = value + boostValue;
                            if (newValue > da.MaxValue)
                            {
                                newValue = da.MaxValue;
                            }
                            else if (newValue < da.MinValue)
                            {
                                newValue = da.MinValue;
                            }
                            boostInfo.SetValue(lastTurn.Side1Pokes[boostSide.pos], newValue);


                        }
                        else if (boostSide.side == 2)
                        {
                            var da = (DecreaseAttribute)boostInfo.GetCustomAttribute(typeof(DecreaseAttribute));
                            if (da == null)
                            {
                                // Console.WriteLine($"da: {boostType}为null");
                                continue;
                            }
                            var value = (int)boostInfo.GetValue(lastTurn.Side2Pokes[boostSide.pos]);

                            var newValue = value + boostValue;
                            if (newValue > da.MaxValue)
                            {
                                newValue = da.MaxValue;
                            }
                            else if (newValue < da.MinValue)
                            {
                                newValue = da.MinValue;
                            }
                            boostInfo.SetValue(lastTurn.Side2Pokes[boostSide.pos], newValue);
                        }
                        // 能力提升
                        break;
                    case "start":
                        BattleData.BattleTurns.Add(lastTurn.NextTurn());

                        // 对局开始
                        break;
                    case "-start":
                        // 已知寄生种子
                        break;
                    case "-end":
                        break;
                    case "crit":
                        // 状态
                        // ct
                        break;
                    case "swap":
                        // 位置交换
                        if (d[2].StartsWith("p1"))
                        {
                            var p1 = lastTurn.Player1Team.Pokemons.FirstOrDefault(s => s.NowPos == 0);
                            var p2 = lastTurn.Player1Team.Pokemons.FirstOrDefault(s => s.NowPos == 1);
                            if (p1 != null) p1.NowPos = 1;
                            if (p2 != null) p2.NowPos = 0;

                        }
                        else if (d[2].StartsWith("p2"))
                        {
                            var p1 = lastTurn.Player2Team.Pokemons.FirstOrDefault(s => s.NowPos == 0);
                            var p2 = lastTurn.Player2Team.Pokemons.FirstOrDefault(s => s.NowPos == 1);
                            if (p1 != null) p1.NowPos = 1;
                            if (p2 != null) p2.NowPos = 0;
                        }
                        // ct
                        break;
                    case "-status":
                        var statusSide = GetSidePos(d[2]);
                        var status = typeof(PokemonStatus).GetProperty(
                                                       $"{new CultureInfo("en").TextInfo.ToTitleCase(d[3].ToLower())}");
                        if (status == null)
                        {
                            // Console.WriteLine($"cureStatus: {d[3]} 为null");
                            break;
                        }
                        if (statusSide.side == 1)
                        {
                            status.SetValue(lastTurn.Side1Pokes[statusSide.pos], d[3] == "slp" ? 3 : 1);
                        }
                        else if (statusSide.side == 2)
                        {
                            status.SetValue(lastTurn.Side2Pokes[statusSide.pos], 1);

                        }
                        break;
                    case "-curestatus":
                        var cureSide = GetSidePos(d[2]);
                        var cureStatus = typeof(PokemonStatus).GetProperty(
                                                       $"{new CultureInfo("en").TextInfo.ToTitleCase(d[3].ToLower())}");
                        if (cureStatus == null)
                        {
                            // Console.WriteLine($"cureStatus: {d[3]} 为null");
                            break;
                        }
                        if (cureSide.side == 1)
                        {
                            cureStatus.SetValue(lastTurn.Side1Pokes[cureSide.pos], 0);
                        }
                        else if (cureSide.side == 2)
                        {
                            cureStatus.SetValue(lastTurn.Side2Pokes[cureSide.pos], 0);

                        }
                        // 状态治愈
                        break;
                    case "win":
                        //谁赢了
                        var winP = GetPlayerByName(BattleData, d[2]);
                        BattleData.WhoWin = winP == 1 ? BattleResult.Player1Win : BattleResult.Player2Win;
                        if (winP == 1)
                        {
                            lastTurn.Reward1 += 1000;
                        }
                        else
                        {
                            lastTurn.Reward2 += 1000;

                        }
                        // 实施奖励
                        break;
                    case "cant":
                        // 有些状态 比如睡觉在cant时要减少一次
                        break;
                    default:
                        //// Console.WriteLine(d[1]);
                        // 未处理 输出检查
                        break;

                }
            }
        }
    }
}

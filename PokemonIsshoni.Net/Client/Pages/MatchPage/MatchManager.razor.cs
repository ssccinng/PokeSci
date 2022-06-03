using MudBlazor;
using PokemonIsshoni.Net.Client.Pages.MudDialogCard;
using PokemonIsshoni.Net.Shared.Info;
using PokemonIsshoni.Net.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonIsshoni.Net.Client.Pages.MatchPage
{
    public partial class MatchManager
    {
        private PCLMatch _pclMatch;

        private async Task<IEnumerable<UserInfo>> SearchUser(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(300);

            // if text is null or empty, don't return values (drop-down will not open)
            if (string.IsNullOrEmpty(value))
                return _userDatas;
            //return new UserData[0];
            return _userDatas.Where(x => x.NickName.Contains(value, StringComparison.InvariantCultureIgnoreCase) || x.Email.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<PCLMatchPlayer>> SearchMatchUser(string value)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(300);

            // if text is null or empty, don't return values (drop-down will not open)
            if (string.IsNullOrEmpty(value))
                return _pclMatch.PCLMatchPlayerList;
            //return new UserData[0];
            return _pclMatch.PCLMatchPlayerList.Where(x => x.ShadowId.Contains(value, StringComparison.InvariantCultureIgnoreCase) || _userDatasDic[x.UserId].Email.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
        private async void AddUserToMatch(UserInfo player)
        {
            if (player == null)
            {
                PrintInfoBar("你啥也没选呢");
                return;
            }
            var dialog = Dialog.Show<ConfirmDialogCard>("操作确认", new DialogParameters { { "content", $"确认添加 {player.NickName} 至比赛?" } });
            var res1 = await dialog.Result;

            if (res1.Cancelled) return;
            PCLMatchPlayer player1 = new();
            player1.UserId = player.UserId;
            player1.ShadowId = player.NickName;
            player1.QQ = player.QQ;
            player1.PCLMatchId = Id;
            var res = await MatchService.AddPCLMatch(player1);
            if (res != null)
            {
                PrintInfoBar("添加成功", "Success");
                //res.UserData = player;
                _pclMatch.PCLMatchPlayerList.Add(res);
                _matchUserDatasDic.Add(res.UserId, res);
                StateHasChanged();
            }
            else
            {
                PrintInfoBar("添加失败");
                // CanSignin修改
            }
            // In real life use an asynchronous function for fetching data from an api.
            //await Task.Delay(300);

            //// if text is null or empty, don't return values (drop-down will not open)
            //if (string.IsNullOrEmpty(value))
            //    return new string[0];
            //return _userDatas.Select(s => s.NickName).Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
        private async void AddRefereeToMatch(UserInfo player, RefereeType refereeType)
        {
            if (player == null)
            {
                PrintInfoBar("你啥也没选呢");
                return;
            }
            var dialog = Dialog.Show<ConfirmDialogCard>("操作确认", new DialogParameters { { "content", $"确认添加 {player.NickName} 为裁判?" } });
            var res1 = await dialog.Result;

            if (res1.Cancelled) return;
            PCLReferee player1 = new();
            player1.UserId = player.UserId;
            player1.PCLMatchId = Id;
            player1.RefereeType = refereeType;
            //player1.UserId = value3.UserId;
            var res = await MatchService.RegisterRefereePCLMatchAsync(player1);
            if (res != null)
            {
                PrintInfoBar("添加成功", "Success");
                player1.Id = res.Id;
                //player1.UserId = player;
                _pclMatch.PCLMatchRefereeList.Add(player1);
                StateHasChanged();
            }
            else
            {
                PrintInfoBar("添加失败");
                // CanSignin修改
            }
            // In real life use an asynchronous function for fetching data from an api.
            //await Task.Delay(300);

            //// if text is null or empty, don't return values (drop-down will not open)
            //if (string.IsNullOrEmpty(value))
            //    return new string[0];
            //return _userDatas.Select(s => s.NickName).Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async void DeleteUserFromMatch(PCLMatchPlayer player)
        {
            var dialog = Dialog.Show<ConfirmDialogCard>("删除确认", new DialogParameters { { "content", $"确认删除选手 {player.ShadowId} ?" } });
            var res1 = await dialog.Result;
            if (res1.Cancelled) return;
            var res = await MatchService.DeRegisterPCLMatch(player);
            if (res)
            {
                PrintInfoBar("删除成功", "Success");
                //player1.UserId = player;
                _pclMatch.PCLMatchPlayerList.Remove(player);
                StateHasChanged();
            }
            else
            {
                PrintInfoBar("删除失败");
                // CanSignin修改
            }
        }

        private async void DeleteRefereeFromMatch(PCLReferee referee)
        {
            var dialog = Dialog.Show<ConfirmDialogCard>("删除确认", new DialogParameters { { "content", $"确认删除裁判 {_userDatasDic[referee.UserId].NickName} ?" } });
            var res1 = await dialog.Result;
            if (res1.Cancelled) return;
            var res = await MatchService.DeRegisterRefereePCLMatchAsync(referee);
            if (res)
            {
                PrintInfoBar("删除成功", "Success");
                //player1.UserId = player;
                _pclMatch.PCLMatchRefereeList.Remove(referee);
                StateHasChanged();
            }
            else
            {
                PrintInfoBar("删除失败");
                // CanSignin修改
            }
        }

        private async void AddUserToRound(PCLMatchRound pCLMatchRound, PCLMatchPlayer player)
        {
            if (player == null)
            {
                PrintInfoBar("你啥也没选呢");
                return;
            }
            var dialog = Dialog.Show<ConfirmDialogCard>("操作确认", new DialogParameters { { "content", $"确认添加 {player.ShadowId} 至比赛?" } });
            var res1 = await dialog.Result;

            if (res1.Cancelled) return;
            PCLRoundPlayer player1 = new();
            player1.UserId = player.UserId;
            player1.PCLMatchRoundId = pCLMatchRound.Id;
            var res = await MatchService.AddPCLRound(player1);

            if (res != null)
            {
                PrintInfoBar("添加成功", "Success");
                pCLMatchRound.PCLRoundPlayers.Add(res);
                // 加入对应轮
                //res.UserData = player;
                //_pclMatch.PCLMatchPlayerList.Add(res);
                StateHasChanged();
            }
            else
            {
                PrintInfoBar("添加失败");
                // CanSignin修改
            }
        }

        private async void DeleteUserFromRound(PCLMatchRound pCLMatchRound, PCLRoundPlayer player)
        {
            var dialog = Dialog.Show<ConfirmDialogCard>("删除确认", new DialogParameters { { "content", $"确认删除选手 {_matchUserDatasDic[player.UserId].ShadowId} ?" } });
            var res1 = await dialog.Result;
            if (res1.Cancelled) return;
            var res = await MatchService.DeRegisterPCLRound(player);
            if (res)
            {
                PrintInfoBar("删除成功", "Success");
                //player1.UserId = player;
                pCLMatchRound.PCLRoundPlayers.Remove(player);
                StateHasChanged();
            }
            else
            {
                PrintInfoBar("删除失败");
                // CanSignin修改
            }
        }
        #region 比赛流程控制
        private async Task<bool> MatchStart()
        {
            // 这里显示比赛信息 确认无误后再继续

            var dialog = Dialog.Show<MatchConfirmCard>("信息确认", new DialogParameters { { "PCLMatch", _pclMatch } });
            var res1 = await dialog.Result;
            if (res1.Cancelled) return false;
            if (_pclMatch.MatchState != MatchState.Registering)
            {
                PrintInfoBar("比赛已经启动！");
                return false;

            }

            if (_pclMatch.PCLMatchPlayerList.Count < 4)
            {
                PrintInfoBar("人太少了！再喊点人吧~");
                return false;

            }
            if (await MatchService.PCLMatcStartAsync(Id))
            {
                _pclMatch = await MatchService.GetMatchByIdAsync(Id);
                PrintInfoBar("比赛开始！", "Success");
                return true;
            }
            PrintInfoBar("创建失败...");
            // 这里可以刷新页面
            return false;
        }

        private async Task<bool> RoundStart(PCLMatchRound round)
        {
            var dialog = Dialog.Show<ConfirmDialogCard>("开始确认", new DialogParameters { { "content", $"确认开始本轮吗？" } });
            var res1 = await dialog.Result;
            if (res1.Cancelled) return false;
            if (_pclMatch.MatchState != MatchState.Running)
            {
                PrintInfoBar("比赛未启动！");
                return false;
            }

            if (round.PCLRoundState != RoundState.Waiting)
            {
                PrintInfoBar("数据有问题说！");
                return false;

            }
            await MatchService.PCLRoundStartAsync(Id, round.Id);
            _pclMatch = await MatchService.GetMatchByIdAsync(Id);
            PrintInfoBar("开始！", "Success");
            return true;
        }
        #endregion
        void PrintInfoBar(string message, string type = "Error")
        {
            Snackbar.Clear();
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomCenter;
            switch (type)
            {
                case "Success":
                    Snackbar.Add(message, Severity.Success);
                    break;
                case "Error":
                    Snackbar.Add(message, Severity.Error);
                    break;
                default:
                    Snackbar.Add(message, Severity.Error);
                    break;
            }

        }
    }
}

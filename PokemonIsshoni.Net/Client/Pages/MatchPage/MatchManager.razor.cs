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
            //player1.UserId = player.UserId;
            player1.ShadowId = player.NickName;
            player1.QQ = player.QQ;
            player1.PCLMatchId = Id;
            //var res = await MatchService.RegisterPCLMatch(player1);
            //if (res != null)
            //{
            //    PrintInfoBar("添加成功", "Success");
            //    res.UserData = player;
            //    _pclMatch.PCLMatchPlayerList.Add(res);
            //    StateHasChanged();
            //}
            //else
            //{
            //    PrintInfoBar("添加失败");
            //    // CanSignin修改
            //}
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
            Referee player1 = new();
            //player1.UserId = player.UserId;
            player1.PCLMatchId = Id;
            player1.refereeType = refereeType;
            //var res = await MatchService.RegisterRefereePCLMatch(player1);
            //if (res != null)
            //{
            //    PrintInfoBar("添加成功", "Success");
            //    player1.Id = res.Id;
            //    player1.UserData = player;
            //    _pclMatch.PCLMatchRefereeList.Add(player1);
            //    StateHasChanged();
            //}
            //else
            //{
            //    PrintInfoBar("添加失败");
            //    // CanSignin修改
            //}
            // In real life use an asynchronous function for fetching data from an api.
            //await Task.Delay(300);

            //// if text is null or empty, don't return values (drop-down will not open)
            //if (string.IsNullOrEmpty(value))
            //    return new string[0];
            //return _userDatas.Select(s => s.NickName).Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }


        private bool MatchStart()
        {

            return true;
        }

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudBlazor;
using PokemonIsshoni.Net.Shared.Models;

namespace PokemonIsshoni.Net.Client.Pages.MatchPage
{
    public partial class MatchDetail
    {
        private PCLMatch _pclMatch;
        private bool CanSignin => _pclMatch.MatchState == MatchState.Registering && !_pclMatch.PCLMatchPlayerList.Any(s => s.UserId == _userId);

        async Task<bool> SigninMatch()
        {
            try
            {
                if (!await OpenDialog())
                {
                    return false;
                }
                //PrintInfoBar("比赛准备创建", "Success");
                //if (!await MatchService.CheckPCLMatchPwd(Id, password.Value))
                //{
                //    PrintInfoBar("密语错误");
                //    return false;
                //}
                //var res = await MatchService.RegisterPCLMatch(player);
                //if (res != null)
                //{
                //    PrintInfoBar("报名成功", "Success");
                //    _pclMatch.PCLMatchPlayerList.Add(res);
                //}
                //else
                //{
                //    PrintInfoBar("报名失败");
                //    // CanSignin修改
                //}
                //Navigation.NavigateTo($"MatchDetail/{Id}", true);
                // 改颜色 + 跳转
            }
            catch
            {
                PrintInfoBar("报名失败");
            }
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

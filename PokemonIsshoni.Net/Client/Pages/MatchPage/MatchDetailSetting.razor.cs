using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using PokemonIsshoni.Net.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PokemonIsshoni.Net.Client.Pages.MatchPage
{
    public partial class MatchDetailSetting
    {
        private PCLMatch _pclMatch;
        private bool shouldRender = true;
        private string MatchNameCheck(string matchName)
        {
            if (matchName?.Length is > 30 or < 5) return "比赛名字需在5-30字之间~";
            return null;
        }
        private async void UploadLogo(InputFileChangeEventArgs e)
        {
            using var content = new MultipartFormDataContent();
            long maxFileSize = 1024 * 1024 * 10;
            shouldRender = false;
            if (e.File.Size <= maxFileSize)
            {
                var fileContent = new StreamContent(e.File.OpenReadStream());
                //PrintInfoBar(e.File.Name);
                content.Add(
                            content: fileContent,
                            name: "\"file\"",
                            fileName: e.File.Name);

                var res = await Http.PostAsync("upload/image", content);
                //_pclMatch.Logo = (await res.Content.ReadFromJsonAsync<UploadResult>()).url;

            }
            else
            {
                PrintInfoBar("太大了~~~");
            }
            shouldRender = true;
            StateHasChanged();

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

        async void SaveChange()
        {
            //var res = await MatchService.UpdatePCLMatch(_pclMatch);
            //if (res)
            //{
            //    PrintInfoBar("保存成功", "Success");
            //    success = false;
            //}
            //else
            //{
            //    PrintInfoBar("保存失败");
            //}
        }
    }

}

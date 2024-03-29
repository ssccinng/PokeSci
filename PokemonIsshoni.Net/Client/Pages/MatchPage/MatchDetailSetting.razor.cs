﻿using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using PokemonIsshoni.Net.Shared;
using PokemonIsshoni.Net.Shared.Models;

namespace PokemonIsshoni.Net.Client.Pages.MatchPage
{
    public partial class MatchDetailSetting
    {
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
                PCLMatch.Logo = (await res.Content.ReadFromJsonAsync<UploadResult>()).Url;

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
            var res = await MatchService.UpdatePCLMatchAsync(PCLMatch);
            if (res)
            {
                PrintInfoBar("保存成功", "Success");
                success = false;
            }
            else
            {
                PrintInfoBar("保存失败");
            }
        }
    }

}

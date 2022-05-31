using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using MudBlazor;
using PokemonIsshoni.Net.Shared;
using PokemonIsshoni.Net.Shared.Models;
//using PokemonIsshoni.Net.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PokemonIsshoni.Net.Client.Pages.MatchPage
{
    public partial class CreateMatch
    {

        private PCLMatch _pclMatch = new()
        {
            PCLMatchRoundList = new() { new PCLMatchRound { PCLRoundType = RoundType.Swiss } }
        };


        private bool shouldRender = true;
        protected override bool ShouldRender() => shouldRender;

        //private List<File> files = new();
        //private List<UploadResult> uploadResults = new();
        private void AddRound()
        {
            if (_pclMatch.PCLMatchRoundList.Count >= 5)
            {
                // 这也太多了
                PrintInfoBar("太...太多了！");
                return;
            }
            _pclMatch.PCLMatchRoundList.Add(new PCLMatchRound { PCLRoundType = RoundType.Swiss });
        }
        private void DeleteLastRound()
        {
            if (_pclMatch.PCLMatchRoundList.Count < 2)
            {
                PrintInfoBar("听不见！阶段这么少还想办比赛？重来！");
                return;
            }
            // 提示至少要有一个阶段
            _pclMatch.PCLMatchRoundList.Remove(_pclMatch.PCLMatchRoundList.Last());
        }

        private string MatchNameCheck(string matchName)
        {
            if (matchName?.Length is > 30 or < 5) return "比赛名字需在5-30字之间~";
            return null;
        }

        private bool MatchCheck()
        {

            return true;
        }

        private async void Create()
        {
            // 可能还要check一下
            // 加入跳转
            //_pclMatch.UserId = _userId;
            try
            {
                if (!await OpenDialog())
                {
                    return;
                }
                //PrintInfoBar("比赛准备创建", "Success");
                //_pclMatch.UserId = "123";
                var res = await MatchService.CreateMatch(_pclMatch);
                if (res != null)
                {
                    PrintInfoBar("比赛创建成功", "Success");
                    Navigation.NavigateTo($"MatchDetail/{res.Id}");
                }

                else
                {
                    PrintInfoBar("比赛创建失败");

                }

                // 改颜色 + 跳转
            }
            catch
            {
                PrintInfoBar("比赛创建失败");
            }


        }



        //private async Task OnInputFileChange(InputFileChangeEventArgs e)
        //{
        //    shouldRender = false;
        //    long maxFileSize = 1024 * 1024 * 10;
        //    var upload = false;

        //    using var content = new MultipartFormDataContent();

        //    var file = e.File;
        //    if (uploadResults.SingleOrDefault(
        //        f => f.FileName == file.Name) is null)
        //    {
        //        var fileContent = new StreamContent(file.OpenReadStream());

        //        //files.Add(
        //        //    new()
        //        //    {
        //        //        Name = file.Name
        //        //    });

        //        if (file.Size < maxFileSize)
        //        {
        //            content.Add(
        //                content: fileContent,
        //                name: "\"file\"",
        //                fileName: file.Name);

        //            upload = true;
        //        }
        //        else
        //        {
        //            Logger.LogInformation("{FileName} not uploaded (Err: 6)",
        //                file.Name);

        //            uploadResults.Add(
        //                new()
        //                {
        //                    FileName = file.Name,
        //                    ErrorCode = 6,
        //                    Uploaded = false
        //                });
        //        }
        //    }


        //    if (upload)
        //    {
        //        var response = await Http.PostAsync("/upload/image", content);

        //        var newUploadResults = await response.Content
        //            .ReadFromJsonAsync<IList<UploadResult>>();
        //        //_pclMatch.Logo = newUploadResults.
        //        uploadResults = uploadResults.Concat(newUploadResults).ToList();
        //    }

        //    shouldRender = true;
        //}

        //private static bool FileUpload(IList<UploadResult> uploadResults,
        //    string fileName, ILogger<CreateMatch> logger, out UploadResult result)
        //{
        //    result = uploadResults.SingleOrDefault(f => f.FileName == fileName);

        //    if (result is null)
        //    {
        //        logger.LogInformation("{FileName} not uploaded (Err: 5)", fileName);
        //        result = new();
        //        result.ErrorCode = 5;
        //    }

        //    return result.Uploaded;
        //}
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
                _pclMatch.Logo = (await res.Content.ReadFromJsonAsync<UploadResult>()).Url;

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

    }
}

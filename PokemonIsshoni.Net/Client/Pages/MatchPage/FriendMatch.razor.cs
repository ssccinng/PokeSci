//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.SignalR.Client;
//using System.Threading.Tasks;

//namespace PokemonIsshoni.Net.Client.Pages.MatchPage
//{
//    public partial class FriendMatch
//    {
//        [Parameter]
//        public int Id { get; set; }
//        private HubConnection hubConnection;
//        private bool power;
//        protected override async Task OnInitializedAsync()
//        {


//            hubConnection = new HubConnectionBuilder()
//                .WithUrl(NavigationManager.ToAbsoluteUri("/FriendMatchHub"))
//                .Build();


//            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
//            {
//                var encodedMsg = $"{user}: {message}";
//                StateHasChanged();
//            });

//            hubConnection.On<string, string>("ReceiveUpdate", async (user, message) =>
//            {
//            });
//            await hubConnection.StartAsync();


//            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
//            var user = authState.User;
//            if (user.IsInRole("Admin") || user.Identity.Name == "pokemonisshoni@163.com" || user.Identity.Name == "1209871646@qq.com")
//            {

//                power = true;
//            }

//            if (user.Identity.IsAuthenticated)
//            {


//            }

//            if (!power)
//            {
//                //NavigationManager.NavigateTo("/" + thematch.Id, true);
//            }
//        }
//        //async Task Send(string messageSend) =>
//        //    await hubConnection.SendAsync("SendMessage", userContext.NickName, messageSend);
//        async Task Update() =>
//            await hubConnection.SendAsync("SendUpdate", "1", "1");


//    }
//}

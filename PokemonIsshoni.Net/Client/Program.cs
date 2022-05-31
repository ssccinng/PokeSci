using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using PokemonIsshoni.Net.Client;
using PokemonIsshoni.Net.Client.Factory;
using PokemonIsshoni.Net.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("PokemonIsshoni.Net.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("PokemonIsshoni.Net.ServerAPI"));
// 匿名访问
builder.Services.AddHttpClient("PokemonIsshoni.Net.ServerAPI.Anonymous", client =>
{
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});
#region 自定义服务
builder.Services.AddScoped<UserInfoServices>();
builder.Services.AddScoped<PCLServices>();
#endregion

builder.Services.AddApiAuthorization()
    .AddAccountClaimsPrincipalFactory<CustomUserFactory>();
builder.Services.AddMudServices();
await builder.Build().RunAsync();



using System.IdentityModel.Tokens.Jwt;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using PokeCommon.Utils;
using PokemonIsshoni.Net.Server.Areas.Identity.Data;
using PokemonIsshoni.Net.Server.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("PokemonIsshoniNetServerContextConnection") ?? throw new InvalidOperationException("Connection string 'PokemonIsshoniNetServerContextConnection' not found.");

builder.Services.AddDbContext<PokemonIsshoniNetServerContext>(options =>
    //options.UseMySql(connectionString, ));;
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))); ;


builder.Services.AddDefaultIdentity<PokemonIsshoniNetServerUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PokemonIsshoniNetServerContext>();

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

//builder.Services.AddIdentityServer()
//    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

// 身份验证 角色
builder.Services.AddIdentityServer()
    .AddApiAuthorization<PokemonIsshoniNetServerUser, PokemonIsshoniNetServerContext>(options =>
    {
        options.IdentityResources["openid"].UserClaims.Add("name");
        options.ApiResources.Single().UserClaims.Add("name");
        options.IdentityResources["openid"].UserClaims.Add("role");
        options.ApiResources.Single().UserClaims.Add("role");
    });

builder.Services.AddTransient<IProfileService, ProfileService>();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("role");
// 邮件
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

builder.Services.AddAuthentication()
    .AddIdentityServerJwt();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(30);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});


builder.Services.AddServerSideBlazor().AddHubOptions(o =>
{
    o.MaximumReceiveMessageSize = 10 * 1024 * 1024;
});
//builder.WebHost.UseUrls("https://*:443");


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<PokemonIsshoniNetServerContext>();
    PokemonTools.PokemonContext = context;
    //Console.WriteLine(PokemonTools.GetNatureAsync(1).Result.Name_Chs);
    //context.Database.Migrate();

    //EnsureRole(services, (await context.Users.FirstAsync(s => s.Email == "wanghaosheng@linxrobot.com")).Id, "Admin").Wait();
    //EnsureRole(services, (await context.Users.FirstAsync(s => s.Email == "chenggang@linxrobot.com")).Id, "Admin").Wait();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors(policy =>
                policy.AllowAnyOrigin()
                .AllowAnyMethod());
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();


static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                    string uid, string role)
{
    IdentityResult IR = null;
    var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
    //var UserroleManager = serviceProvider.GetService<UserRoleManager<IdentityUserRole>>();
    if (roleManager == null)
    {
        throw new Exception("roleManager null");
    }

    if (!await roleManager.RoleExistsAsync(role))
    {
        IR = await roleManager.CreateAsync(new IdentityRole(role));
    }

    var userManager = serviceProvider.GetService<UserManager<PokemonIsshoniNetServerUser>>();

    var user = await userManager.FindByIdAsync(uid);

    if (user == null)
    {
        throw new Exception("The testUserPw password was probably not strong enough!");
    }
    IR = await userManager.AddToRoleAsync(user, role);

    return IR;
}


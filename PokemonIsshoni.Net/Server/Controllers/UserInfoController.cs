using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PokemonIsshoni.Net.Server.Areas.Identity.Data;
using PokemonIsshoni.Net.Shared.Info;

namespace PokemonIsshoni.Net.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        private readonly UserManager<PokemonIsshoniNetServerUser> _userManager;
        private readonly PokemonIsshoniNetServerContext _context;
        private readonly IUserStore<PokemonIsshoniNetServerUser> _userStore;
        private readonly IUserEmailStore<PokemonIsshoniNetServerUser> _emailStore;
        public UserInfoController(UserManager<PokemonIsshoniNetServerUser> userManager, 
            PokemonIsshoniNetServerContext context,
            IUserStore<PokemonIsshoniNetServerUser> userStore
            )
        {
            _userManager = userManager;
            _context = context;
            _userStore = userStore;
            _emailStore = GetEmailStore();

        }
        [Authorize]
        [HttpGet("/api/userinfo/GetAllUser")]

        public async Task<IEnumerable<UserInfo>> GetAllUser()
        {
            return _userManager.Users.Select(s => new UserInfo
            {
                UserId = s.Id,
                Avatar = s.Avatar,
                City = s.City,
                DOB = s.DOB,
                Email = s.Email,
                HomeName = s.HomeName,
                NickName = s.NickName,
                QQ = s.QQ,
                Registertime = s.Registertime,
                TrainerIdInt = s.TrainerIdInt,
            });
        }

        [HttpGet("/api/userinfo/GetUserByEmail/{email}")]
        public async Task<UserInfo> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return new UserInfo
            {
                UserId = user.Id,
                Avatar = user.Avatar,
                City = user.City,
                DOB = user.DOB,
                Email = email,
                HomeName = user.HomeName,
                NickName = user.NickName,
                QQ = user.QQ,
                Registertime = user.Registertime,
                TrainerIdInt = user.TrainerIdInt,
            };
        }

        [HttpGet("/api/userinfo/GetUserByName/{name}")]
        public async Task<UserInfo> GetUserByName(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            return new UserInfo
            {
                UserId = user.Id,
                Avatar = user.Avatar,
                City = user.City,
                DOB = user.DOB,
                Email = user.Email,
                HomeName = user.HomeName,
                NickName = user.NickName,
                QQ = user.QQ,
                Registertime = user.Registertime,
                TrainerIdInt = user.TrainerIdInt,
            };
        }

        [HttpGet("/api/userinfo/GetUserById/{Id}")]
        public async Task<UserInfo> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return new UserInfo
            {
                UserId = user.Id,
                Avatar = user.Avatar,
                City = user.City,
                DOB = user.DOB,
                Email = user.Email,
                HomeName = user.HomeName,
                NickName = user.NickName,
                QQ = user.QQ,
                Registertime = user.Registertime,
                TrainerIdInt = user.TrainerIdInt,
            };
        }

        [HttpGet("/api/userinfo/AddGuset/{cnt}")]
        [Authorize(Roles = "Admin")]
        public async Task AddGuset(int cnt)
        {
            //var ccq = Request.Body.;
            string abc = "abcdefghijklmnopqrstuvwxyz";
            Random a = new ();
            var gg = HttpContext;
            var cc = ControllerContext;
            Random rnd = new Random();
            //MetadataProvider
            //int Cnt = Request.Body.reada; ;
            for (int j = 0; j < cnt; ++j)
            {
                string b = "";

                for (int i = 0; i < 10; ++i)
                {
                    b += abc[a.Next(25)];
                }
                var user = Activator.CreateInstance<PokemonIsshoniNetServerUser>();
                user.QQ = "07910730";
                //user.UserName = b + "@qq.com";
                 //user.Email = b + "@qq.com";
                  user.NickName = $"游客{j}";
                 user.DOB = DateTime.Now;
                  user.HomeName = $"游客{j}";
                  user.City = $"双叶";
                user.TrainerIdInt = rnd.Next(99999999);
                await _userStore.SetUserNameAsync(user, b + "@qq.com", CancellationToken.None);
                await _emailStore.SetEmailAsync(user, b + "@qq.com", CancellationToken.None);
                var res = await _userManager.CreateAsync(user, "QWEqwe.987789123");
                Console.WriteLine(res.Succeeded);
                _context.PCLGuests.Add(new Shared.Models.Guest
                {
                    UserId = user.Id
                });
            }
            
            _context.SaveChanges();
        }
        private IUserEmailStore<PokemonIsshoniNetServerUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<PokemonIsshoniNetServerUser>)_userStore;
        }

    }
}

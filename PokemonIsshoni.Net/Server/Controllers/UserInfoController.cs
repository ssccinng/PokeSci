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
        public UserInfoController(UserManager<PokemonIsshoniNetServerUser> userManager, PokemonIsshoniNetServerContext context)
        {
            _userManager = userManager;
            _context = context;

        }
        [HttpGet("/api/userinfo/GetUserByEmail/{email}")]
        public async Task<UserInfo> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return new UserInfo
            {
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
    }
}

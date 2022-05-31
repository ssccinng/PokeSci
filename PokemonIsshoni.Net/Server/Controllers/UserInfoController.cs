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
        public UserInfoController(UserManager<PokemonIsshoniNetServerUser> userManager, PokemonIsshoniNetServerContext context)
        {
            _userManager = userManager;
            _context = context;

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
    }
}

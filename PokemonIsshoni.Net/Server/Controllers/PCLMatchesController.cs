using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokeCommon.Models;
using PokeCommon.PokemonHome;
using PokemonIsshoni.Net.Server.Areas.Identity.Data;
using PokemonIsshoni.Net.Shared.Info;
using PokemonIsshoni.Net.Shared.Models;

namespace PokemonIsshoni.Net.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PCLMatchesController : ControllerBase
    {
        private readonly PokemonIsshoniNetServerContext _context;

        private readonly PokemonHomeTools _pokeHomeTools = new(true);

        public PCLMatchesController(PokemonIsshoniNetServerContext context)
        {
            _context = context;
            _pokeHomeTools.UpdateRankMatchAsync().Wait();
        }

        // GET: api/PCLMatches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PCLMatch>>> GetPCLMatchs()
        {
          if (_context.PCLMatchs == null)
          {
              return NotFound();
          }
            var res = await _context.PCLMatchs.ToListAsync();
            res.ForEach(s => s.Password = "");
            return res;
        }

        // GET: api/PCLMatches/5
        //[AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<PCLMatch>> GetPCLMatch(int id)
        
        {
            var b = HttpContext.User.Claims.FirstOrDefault(s => s.Type.EndsWith("nameidentifier"));

            if (_context.PCLMatchs == null)
          {
              return NotFound();
          }
            //var pCLMatch = await _context.PCLMatchs.FindAsync(id);
            var pCLMatch = await _context.PCLMatchs
                                        .Include(s => s.PCLMatchRoundList)
                                        .Include(s => s.PCLMatchRefereeList)
                                        //.Include(s=>s.User)
                                        .Include(s => s.PCLMatchPlayerList).FirstOrDefaultAsync(s => s.Id == id);

            if (pCLMatch == null)
            {
                return NotFound();
            }

            // 如果是举办者或者有权限的人 才会返回密码
            if (HttpContext.User.IsInRole("Admin") || b?.Value == pCLMatch.UserId)
            {

            }
            else
            {
                pCLMatch.Password = "";
            }

            return pCLMatch;
        }

        //[HttpGet("Info/{id}")]
        //public async Task<ActionResult<PCLMatchInfo>> GetPCLMatchInfo(int id)
        //{
        //    if (_context.PCLMatchs == null)
        //    {
        //        return NotFound();
        //    }
        //    var pCLMatch = await _context.PCLMatchs.FindAsync(id);

        //    if (pCLMatch == null)
        //    {
        //        return NotFound();
        //    }

        //    return pCLMatch;
        //}

        // PUT: api/PCLMatches/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPCLMatch(int id, PCLMatch pCLMatch)
        {
            if (id != pCLMatch.Id)
            {
                return BadRequest();
            }

            _context.Entry(pCLMatch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PCLMatchExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PCLMatches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PCLMatch>> PostPCLMatch(PCLMatch pCLMatch)
        {
            //var a = HttpContext.User.Claims.ToList();
            //var b = HttpContext.User.Identity
            Console.WriteLine("到这里了");
            Debug.WriteLine("到这里了");

            var b = HttpContext.User.Claims.FirstOrDefault(s => s.Type.EndsWith("nameidentifier"));
            pCLMatch.UserId = b.Value;
            Console.WriteLine(b.Value);
            Debug.WriteLine(b.Value);
            //string uid = b.Value;
          if (_context.PCLMatchs == null)
          {
              return Problem("Entity set 'PokemonIsshoniNetServerContext.PCLMatchs'  is null.");
          }
            _context.PCLMatchs.Add(pCLMatch);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPCLMatch", new { id = pCLMatch.Id }, pCLMatch);
        }

        // DELETE: api/PCLMatches/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePCLMatch(int id)
        {
            if (_context.PCLMatchs == null)
            {
                return NotFound();
            }
            var pCLMatch = await _context.PCLMatchs.FindAsync(id);
            if (pCLMatch == null)
            {
                return NotFound();
            }

            _context.PCLMatchs.Remove(pCLMatch);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PCLMatchExists(int id)
        {
            return (_context.PCLMatchs?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // 要这样好呢 还是单独列出api好
        [HttpPost("RegisterUser/{pwd}")]
        [Authorize]
        public async Task<ActionResult<PCLMatchPlayer>> RegisterPCLMatch(PCLMatchPlayer user, string pwd)
        //public async Task<ActionResult<PCLMatchPlayer>> RegisterPCLMatch([FromBody] PCLMatchPlayer user)
        {
            // 搜一次
            var b = HttpContext.User.Claims.FirstOrDefault(s => s.Type.EndsWith("nameidentifier"));
            if (b == null) return NoContent();

            // 何时判断
            user.UserId = b.Value;
            //if (user.UserId == )
            var pCLMatch = await _context.PCLMatchs.Include(s => s.PCLMatchPlayerList).FirstAsync(s => s.Id == user.PCLMatchId);
            if (pCLMatch.PCLMatchPlayerList.Any(s => s.UserId == user.UserId || s.ShadowId == user.ShadowId))
                //if (pCLMatch.PCLMatchPlayerList.Any(s => s.UserId == user.UserId))
            {
                // 已报名 或者同名 这个可能要分开
                return Problem("已经报名，或者有同名人");
            }
            // 判断密码
            if (pCLMatch.IsPrivate)
            {
                if (pwd != pCLMatch.Password && !await HasPower(pCLMatch))
                {
                    return Problem("密码错误");
                }
                // 判断密码
            }
            //user.PreTeam = new PCLPokeTeam();
            _context.PCLMatchPlayers.Add(user);

            //_context.PCLMatchs.Add(pCLMatch);
            await _context.SaveChangesAsync();
            //user.PreTeam = null;
            //return CreatedAtAction("GetPCLMatch", new { id = pCLMatch.Id }, pCLMatch);
            return user;
        }

        [HttpPost("AddUser")]
        [Authorize]
        public async Task<ActionResult<PCLMatchPlayer>> AddUserPCLMatch(PCLMatchPlayer user)
        //public async Task<ActionResult<PCLMatchPlayer>> RegisterPCLMatch([FromBody] PCLMatchPlayer user)
        {
            // 搜一次
            var b = HttpContext.User.Claims.FirstOrDefault(s => s.Type.EndsWith("nameidentifier"));
            if (b == null) return NoContent();
            if (!await HasPower(user.PCLMatchId)) return NoContent();
            // 何时判断
            //user.UserId = b.Value;
            //if (user.UserId == )
            var pCLMatch = await _context.PCLMatchs.Include(s => s.PCLMatchPlayerList).FirstAsync(s => s.Id == user.PCLMatchId);
            //if (pCLMatch.PCLMatchPlayerList.Any(s => s.UserId == user.UserId || s.ShadowId == user.ShadowId))
            if (pCLMatch.PCLMatchPlayerList.Any(s => s.UserId == user.UserId))
            {
                return Problem("已经报名");
            }

            _context.PCLMatchPlayers.Add(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> HasPower(int matchId)
        {
            //var uid = HttpContext.User.Claims.FirstOrDefault(s => s.Type.EndsWith("nameidentifier"));
            var plcMatch = await _context.PCLMatchs.FindAsync(matchId);
            return await HasPower(plcMatch);
            //return HttpContext.User.IsInRole("Admin") || plcMatch.UserId == uid.Value;
        }
        public async Task<bool> HasPower(PCLMatch match)
        {
            var uid = HttpContext.User.Claims.FirstOrDefault(s => s.Type.EndsWith("nameidentifier"));
            //var plcMatch = await _context.PCLMatchs.FindAsync(matchId);
            return HttpContext.User.IsInRole("Admin") || match.UserId == uid.Value;
        }
        #region PokemonHome
        [HttpPost("/api/GetTrainerRankData")]
        public async Task<ActionResult<List<PokemonHomeTrainerRankData>>> GetTrainerRankData(PokemonHomeSession pokemonHomeSession)
        {
            return await _pokeHomeTools.GetTrainerDataAsync(pokemonHomeSession);
        }

        [HttpGet("/api/GetTrainerRankDataLast")]
        public ActionResult<List<PokemonHomeTrainerRankData>> GetTrainerRankDataLast()
        {
            return _pokeHomeTools.PokemonHomeTrainerRankDatas;
        }
        [HttpGet("/api/GetPokemonHomeSessions")]
        public ActionResult<List<PokemonHomeSession>> GetPokemonHomeSessions()
        {
            return _pokeHomeTools.PokemonHomeSessions;
        }

        #endregion
    }
}

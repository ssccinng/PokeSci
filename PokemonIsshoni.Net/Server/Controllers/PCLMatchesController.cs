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
        #region 比赛类CRUD
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
                                        .AsSplitQuery()

                                        .Include(s => s.PCLMatchRoundList)
                                            .ThenInclude(s => s.PCLRoundPlayers) // 这个ok 对战没有必要继续放
                                        .Include(s => s.PCLMatchRoundList)
                                            .ThenInclude(s => s.PCLBattles)
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
        [Authorize]
        public async Task<IActionResult> PutPCLMatch(int id, PCLMatch pCLMatch)
        {
            // 保存时机
            // 要有权限
            if (id != pCLMatch.Id)
            {
                return BadRequest();
            }
            var match = await _context.PCLMatchs.AsNoTracking().FirstAsync(s => s.Id == id);
            if (match.MatchState != MatchState.Registering)
            {
                return Problem("比赛已经开始");
            }
            if (!await HasPower(match))
            {
                return Problem("不准动！");
            }
            _context.Entry(pCLMatch).State = EntityState.Modified;
            for (int i = 0; i < pCLMatch.PCLMatchRoundList.Count; i++)
            {
                _context.Entry(pCLMatch.PCLMatchRoundList[i]).State = EntityState.Modified;

            }
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
        #endregion
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
            if (pCLMatch.PCLMatchPlayerList.Count >= pCLMatch.LimitPlayer)
            {
                return Problem("人数已满！");
            }
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
        public async Task<bool> HasPower(PCLMatch? match)
        {
            if (match == null) return false;

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

        #region 比赛流程控制
        [HttpPost("MatchStart/{id}")]
        [Authorize]
        public async Task<bool> MatchStart(int id)
        {
            //if (!int.TryParse(id, out var Id))
            //{
            //    return false;
            //}
            var plcMatch = await _context.PCLMatchs

                .Include(s => s.PCLMatchPlayerList)
                .Include(s => s.PCLMatchRoundList).FirstOrDefaultAsync(s => s.Id == id);
            if (!await HasPower(plcMatch))
            {
                return false;
            }
            
            // 比赛合理性检查
            if (plcMatch.MatchState != MatchState.Registering)
            {
                return false;
            }

            Random rand = new Random();
            foreach (var player in plcMatch.PCLMatchPlayerList)
            {
                plcMatch.PCLMatchRoundList[0].PCLRoundPlayers.Add(
                    new PCLRoundPlayer
                    {
                        BattleTeam = new(),
                        Rank = rand.Next(2048),
                        UserId = player.UserId,
                        //Tag = rand.Next(2048),
                    }
                    );
            }

            plcMatch.MatchState = MatchState.Running;
            plcMatch.RoundIdx = 0;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 进入下一轮
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("MatchNextRound/{id}")]
        [Authorize]

        public async Task<bool> MatchNextRound(int id)
        {
            return false;
        }

        /// <summary>
        /// 开启本轮
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roundId"></param>
        /// <returns></returns>
        [HttpPost("RoundStart/{id}/{roundId}")]
        [Authorize]
        public async Task<ActionResult<bool>> RoundStart(int id, int roundId)
        {
            var plcMatch = await _context.PCLMatchs
                .Include(s => s.PCLMatchRoundList)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (plcMatch == null) return NotFound();
            if (!await HasPower(plcMatch)) { return false; }
            if (plcMatch.MatchState != MatchState.Running) { return Problem("比赛未在进行"); }
            var round = plcMatch.PCLMatchRoundList[plcMatch.RoundIdx];
            if (round.Id != roundId)
            {
                return Problem("你这Id有问题啊");
            }
            if (round.PCLRoundState != RoundState.Waiting)
            {
                return Problem("该轮已经开始");
            }
            // 小组赛特判
            round.PCLRoundState = RoundState.WaitConfirm;
            await _context.SaveChangesAsync();

            return false;
        }
        /// <summary>
        /// 确认本轮参赛成员
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roundId"></param>
        /// <returns></returns>
        [HttpPost("RoundConfirm/{id}/{roundId}")]
        [Authorize]
        public async Task<ActionResult<bool>> RoundConfirm(int id, int roundId)
        {
            var plcMatch = await _context.PCLMatchs
                .Include(s => s.PCLMatchRoundList)
                .FirstOrDefaultAsync(s => s.Id == id);
            var round = await _context.PCLMatchRounds
                                                    .AsSplitQuery()
                                                    .Include(s => s.PCLRoundPlayers)
                                                    .Include(s => s.PCLBattles)
                                                    .FirstOrDefaultAsync(s => s.Id == roundId);
            if (plcMatch == null) return false;
            if (!await HasPower(plcMatch)) { return false; }
            if (plcMatch.MatchState != MatchState.Running) { return Problem("比赛未在进行"); }

            // 生成对局
            if (plcMatch.PCLMatchRoundList[plcMatch.RoundIdx].Id != roundId)
            {
                return Problem("数据有误");
            }
            round.PCLRoundState = RoundState.Running;

            switch (round.PCLRoundType)
            {
                case RoundType.Swiss:
                    if (
                    (await NextSwiss(roundId, 0)).Value
                        )
                    {
                        await _context.SaveChangesAsync();
                        return true;
                    }
                    break;
                case RoundType.Robin:
                    break;
                case RoundType.Elimination:
                    break;
                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// 进入下一轮瑞士轮
        /// </summary>
        /// <param name="roundId"></param>
        /// <param name="swissId"></param>
        /// <returns></returns>
        [HttpPost("NextSwiss/{roundId}/{swissId}")]
        [Authorize]
        public async Task<ActionResult<bool>> NextSwiss(int roundId, int swissId)
        {
            var round = await _context.PCLMatchRounds
                                                    .AsSplitQuery()
                                                    .Include(s => s.PCLRoundPlayers)
                                                    .Include(s => s.PCLBattles)
                                                    .FirstOrDefaultAsync(s => s.Id == roundId);
            var plcMatch = await _context.PCLMatchs
                .Include(s => s.PCLMatchRoundList)
                .FirstOrDefaultAsync(s => s.Id == round.PCLMatchId);
            if (plcMatch == null) return false;
            if (!await HasPower(plcMatch)) { return false; }
            if (plcMatch.MatchState != MatchState.Running) { return Problem("比赛未在进行"); }
            if (swissId != round.Swissidx) return Problem("你有点问题.jpg");

            var bl = round.GenNextSwissRoundBattes();
            if (bl != null)
            {
                round.Swissidx++;
                round.PCLBattles.AddRange(bl);

                await _context.SaveChangesAsync();
                return true;
            }
            // 生成下一轮对局


            return false;
        }



        /// <summary>
        /// 该轮对阵清算
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roundId"></param>
        /// <returns></returns>
        [HttpPost("RoundCalc/{id}/{roundId}")]
        [Authorize]
        public async Task<ActionResult<bool>> RoundCalc(int id, int roundId)
        {
            var plcMatch = await _context.PCLMatchs
                .Include(s => s.PCLMatchRoundList)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (!await HasPower(plcMatch)) { return false; }
            if (plcMatch.MatchState != MatchState.Running) { return Problem("比赛未在进行"); }

            return false;
        }

        /// <summary>
        /// 比赛结束
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("MatchEnd/{id}")]
        [Authorize]
        public async Task<bool> MatchEnd(int id)
        {
            return false;
        }
        #endregion
    }
}

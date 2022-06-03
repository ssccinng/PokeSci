using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonIsshoni.Net.Server.Areas.Identity.Data;
using PokemonIsshoni.Net.Shared.Models;

namespace PokemonIsshoni.Net.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PCLRoundPlayersController : ControllerBase
    {
        private readonly PokemonIsshoniNetServerContext _context;

        public PCLRoundPlayersController(PokemonIsshoniNetServerContext context)
        {
            _context = context;
        }

        // GET: api/PCLRoundPlayers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PCLRoundPlayer>>> GetPCLRoundPlayers()
        {
          if (_context.PCLRoundPlayers == null)
          {
              return NotFound();
          }
            return await _context.PCLRoundPlayers.ToListAsync();
        }

        // GET: api/PCLRoundPlayers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PCLRoundPlayer>> GetPCLRoundPlayer(int id)
        {
          if (_context.PCLRoundPlayers == null)
          {
              return NotFound();
          }
            var pCLRoundPlayer = await _context.PCLRoundPlayers.FindAsync(id);

            if (pCLRoundPlayer == null)
            {
                return NotFound();
            }

            return pCLRoundPlayer;
        }

        // PUT: api/PCLRoundPlayers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]

        public async Task<IActionResult> PutPCLRoundPlayer(int id, PCLRoundPlayer pCLRoundPlayer)
        {
            if (id != pCLRoundPlayer.Id)
            {
                return BadRequest();
            }

            _context.Entry(pCLRoundPlayer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PCLRoundPlayerExists(id))
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

        // POST: api/PCLRoundPlayers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]

        public async Task<ActionResult<PCLRoundPlayer>> PostPCLRoundPlayer(PCLRoundPlayer pCLRoundPlayer)
        {
            // 请判断权限
          if (_context.PCLRoundPlayers == null)
          {
              return Problem("Entity set 'PokemonIsshoniNetServerContext.PCLRoundPlayers'  is null.");
          }
            var pclRound = await _context.PCLMatchRounds.Include(s => s.PCLRoundPlayers).FirstOrDefaultAsync(s => s.Id == pCLRoundPlayer.PCLMatchRoundId);

            if (!await HasPower(pclRound))
            {
                return Problem("没有权限");
            }
          if (pclRound.PCLRoundPlayers.Any(s => s.UserId == pCLRoundPlayer.UserId))
            {
                return Problem("已经在此轮中");

            }
            pCLRoundPlayer.BattleTeam = new();
            //pCLRoundPlayer.Tag = 
            _context.PCLRoundPlayers.Add(pCLRoundPlayer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPCLRoundPlayer", new { id = pCLRoundPlayer.Id }, pCLRoundPlayer);
        }

        // DELETE: api/PCLRoundPlayers/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePCLRoundPlayer(int id)
        {
            if (_context.PCLRoundPlayers == null)
            {
                return NotFound();
            }
            var pCLRoundPlayer = await _context.PCLRoundPlayers.FindAsync(id);
            if (pCLRoundPlayer == null)
            {
                return NotFound();
            }


            _context.PCLRoundPlayers.Remove(pCLRoundPlayer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PCLRoundPlayerExists(int id)
        {
            return (_context.PCLRoundPlayers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<bool> HasPower(int roundId)
        {
            //var uid = HttpContext.User.Claims.FirstOrDefault(s => s.Type.EndsWith("nameidentifier"));
            var plcround = await _context.PCLMatchRounds.FindAsync(roundId);
            var plcMatch = await _context.PCLMatchs.FindAsync(plcround.PCLMatchId);
            return await HasPower(plcMatch);
            //return HttpContext.User.IsInRole("Admin") || plcMatch.UserId == uid.Value;
        }

        public async Task<bool> HasPower(PCLMatchRound? round)
        {
            if (round == null) return false;
            var plcMatch = await _context.PCLMatchs.FindAsync(round.PCLMatchId);

            //var plcMatch = await _context.PCLMatchs.FindAsync(matchId);
            return await HasPower(plcMatch);

        }
        public async Task<bool> HasPower(PCLMatch? match)
        {
            if (match == null) return false;
            var uid = HttpContext.User.Claims.FirstOrDefault(s => s.Type.EndsWith("nameidentifier"));
            //var plcMatch = await _context.PCLMatchs.FindAsync(matchId);
            return HttpContext.User.IsInRole("Admin") || match.UserId == uid.Value;
        }
    }
}

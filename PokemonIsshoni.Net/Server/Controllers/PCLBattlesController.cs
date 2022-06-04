using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonIsshoni.Net.Server.Areas.Identity.Data;
using PokemonIsshoni.Net.Shared.Models;
using Microsoft.AspNetCore.Authorization;

namespace PokemonIsshoni.Net.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PCLBattlesController : ControllerBase
    {
        private readonly PokemonIsshoniNetServerContext _context;

        public PCLBattlesController(PokemonIsshoniNetServerContext context)
        {
            _context = context;
        }
        #region PCLBattle CRUD
        // GET: api/PCLBattles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PCLBattle>>> GetPCLBattles()
        {
          if (_context.PCLBattles == null)
          {
              return NotFound();
          }
            return await _context.PCLBattles.ToListAsync();
        }

        // GET: api/PCLBattles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PCLBattle>> GetPCLBattle(int id)
        {
          if (_context.PCLBattles == null)
          {
              return NotFound();
          }
            var pCLBattle = await _context.PCLBattles.FindAsync(id);

            if (pCLBattle == null)
            {
                return NotFound();
            }

            return pCLBattle;
        }

        // PUT: api/PCLBattles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPCLBattle(int id, PCLBattle pCLBattle)
        {
            if (id != pCLBattle.Id)
            {
                return BadRequest();
            }

            _context.Entry(pCLBattle).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PCLBattleExists(id))
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

        // POST: api/PCLBattles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PCLBattle>> PostPCLBattle(PCLBattle pCLBattle)
        {
          if (_context.PCLBattles == null)
          {
              return Problem("Entity set 'PokemonIsshoniNetServerContext.PCLBattles'  is null.");
          }
            _context.PCLBattles.Add(pCLBattle);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPCLBattle", new { id = pCLBattle.Id }, pCLBattle);
        }

        // DELETE: api/PCLBattles/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePCLBattle(int id)
        {
            if (_context.PCLBattles == null)
            {
                return NotFound();
            }
            var pCLBattle = await _context.PCLBattles.FindAsync(id);
            if (pCLBattle == null)
            {
                return NotFound();
            }

            _context.PCLBattles.Remove(pCLBattle);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
        private bool PCLBattleExists(int id)
        {
            return (_context.PCLBattles?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        public async Task<ActionResult<List<PCLBattle>>> GetPCLRoundBattle(int roundId)
        {
            if (_context.PCLBattles == null)
            {
                return NotFound();
            }
            return await _context.PCLBattles.Where(s => s.PCLMatchRoundId == roundId).ToListAsync();
        }

        public async Task<bool> HasPower(PCLMatch match)
        {
            if (match == null) return false;

            var uid = HttpContext.User.Claims.FirstOrDefault(s => s.Type.EndsWith("nameidentifier"));
            return HttpContext.User.IsInRole("Admin") || match.UserId == uid.Value;
        }
        public async Task<bool> HasPower(int matchId)
        {
            var plcMatch = await _context.PCLMatchs.FindAsync(matchId);
            return await HasPower(plcMatch);
        }
    }
}

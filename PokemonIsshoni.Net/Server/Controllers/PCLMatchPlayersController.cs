using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonIsshoni.Net.Server.Areas.Identity.Data;
using PokemonIsshoni.Net.Shared.Models;

namespace PokemonIsshoni.Net.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PCLMatchPlayersController : ControllerBase
    {
        private readonly PokemonIsshoniNetServerContext _context;

        public PCLMatchPlayersController(PokemonIsshoniNetServerContext context)
        {
            _context = context;
        }

        // GET: api/PCLMatchPlayers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PCLMatchPlayer>>> GetPCLMatchPlayers()
        {
            if (_context.PCLMatchPlayers == null)
            {
                return NotFound();
            }
            return await _context.PCLMatchPlayers.ToListAsync();
        }

        // GET: api/PCLMatchPlayers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PCLMatchPlayer>> GetPCLMatchPlayer(int id)
        {
            if (_context.PCLMatchPlayers == null)
            {
                return NotFound();
            }
            var pCLMatchPlayer = await _context.PCLMatchPlayers.FindAsync(id);

            if (pCLMatchPlayer == null)
            {
                return NotFound();
            }

            return pCLMatchPlayer;
        }

        // PUT: api/PCLMatchPlayers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]

        public async Task<IActionResult> PutPCLMatchPlayer(int id, PCLMatchPlayer pCLMatchPlayer)
        {
            if (id != pCLMatchPlayer.Id)
            {
                return BadRequest();
            }

            _context.Entry(pCLMatchPlayer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PCLMatchPlayerExists(id))
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

        // POST: api/PCLMatchPlayers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]

        public async Task<ActionResult<PCLMatchPlayer>> PostPCLMatchPlayer(PCLMatchPlayer pCLMatchPlayer)
        {
            if (_context.PCLMatchPlayers == null)
            {
                return Problem("Entity set 'PokemonIsshoniNetServerContext.PCLMatchPlayers'  is null.");
            }
            _context.PCLMatchPlayers.Add(pCLMatchPlayer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPCLMatchPlayer", new { id = pCLMatchPlayer.Id }, pCLMatchPlayer);
        }

        // DELETE: api/PCLMatchPlayers/5
        [HttpDelete("{id}")]
        [Authorize]

        public async Task<IActionResult> DeletePCLMatchPlayer(int id)
        {
            var uid = HttpContext.User.Claims.FirstOrDefault(s => s.Type.EndsWith("nameidentifier"));

            if (_context.PCLMatchPlayers == null)
            {
                return NotFound();
            }
            var pCLMatchPlayer = await _context.PCLMatchPlayers.FindAsync(id);
            if (pCLMatchPlayer == null)
            {
                return NotFound();
            }
            // 这个功能最好函数化
            var plcMatch = await _context.PCLMatchs.FindAsync(pCLMatchPlayer.PCLMatchId);
            if (plcMatch != null)
            {
                if (plcMatch.UserId != uid.Value)
                {
                    return NoContent();
                }

            }
            else
            {
                return NoContent();
            }

            _context.PCLMatchPlayers.Remove(pCLMatchPlayer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PCLMatchPlayerExists(int id)
        {
            return (_context.PCLMatchPlayers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

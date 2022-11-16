using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonIsshoni.Net.Server.Areas.Identity.Data;
using PokemonIsshoni.Net.Shared.Models;

namespace PokemonIsshoni.Net.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PCLMatchRoundsController : ControllerBase
    {
        private readonly PokemonIsshoniNetServerContext _context;

        public PCLMatchRoundsController(PokemonIsshoniNetServerContext context)
        {
            _context = context;
        }

        // GET: api/PCLMatchRounds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PCLMatchRound>>> GetPCLMatchRounds()
        {
            if (_context.PCLMatchRounds == null)
            {
                return NotFound();
            }
            return await _context.PCLMatchRounds.ToListAsync();
        }

        // GET: api/PCLMatchRounds/5
        // 作为更新api
        [HttpGet("{id}")]
        public async Task<ActionResult<PCLMatchRound>> GetPCLMatchRound(int id)
        {
            if (_context.PCLMatchRounds == null)
            {
                return NotFound();
            }
            //var pCLMatchRound = await _context.PCLMatchRounds.FindAsync(id);
            var pCLMatchRound = await _context.PCLMatchRounds.Include(s => s.PCLBattles).Include(s => s.PCLRoundPlayers).FirstOrDefaultAsync(s => s.Id == id);

            if (pCLMatchRound == null)
            {
                return NotFound();
            }

            return pCLMatchRound;
        }

        // PUT: api/PCLMatchRounds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPCLMatchRound(int id, PCLMatchRound pCLMatchRound)
        {
            if (id != pCLMatchRound.Id)
            {
                return BadRequest();
            }

            _context.Entry(pCLMatchRound).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PCLMatchRoundExists(id))
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

        // POST: api/PCLMatchRounds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PCLMatchRound>> PostPCLMatchRound(PCLMatchRound pCLMatchRound)
        {
            if (_context.PCLMatchRounds == null)
            {
                return Problem("Entity set 'PokemonIsshoniNetServerContext.PCLMatchRounds'  is null.");
            }
            _context.PCLMatchRounds.Add(pCLMatchRound);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPCLMatchRound", new { id = pCLMatchRound.Id }, pCLMatchRound);
        }

        // DELETE: api/PCLMatchRounds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePCLMatchRound(int id)
        {
            if (_context.PCLMatchRounds == null)
            {
                return NotFound();
            }
            var pCLMatchRound = await _context.PCLMatchRounds.FindAsync(id);
            if (pCLMatchRound == null)
            {
                return NotFound();
            }

            _context.PCLMatchRounds.Remove(pCLMatchRound);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PCLMatchRoundExists(int id)
        {
            return (_context.PCLMatchRounds?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

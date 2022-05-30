using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonIsshoni.Net.Server.Areas.Identity.Data;
using PokemonIsshoni.Net.Shared.Models;

namespace PokemonIsshoni.Net.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PCLMatchesController : ControllerBase
    {
        private readonly PokemonIsshoniNetServerContext _context;

        public PCLMatchesController(PokemonIsshoniNetServerContext context)
        {
            _context = context;
        }

        // GET: api/PCLMatches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PCLMatch>>> GetPCLMatchs()
        {
          if (_context.PCLMatchs == null)
          {
              return NotFound();
          }
            return await _context.PCLMatchs.ToListAsync();
        }

        // GET: api/PCLMatches/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PCLMatch>> GetPCLMatch(int id)
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

            return pCLMatch;
        }

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
        public async Task<ActionResult<PCLMatch>> PostPCLMatch(PCLMatch pCLMatch)
        {
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
    }
}

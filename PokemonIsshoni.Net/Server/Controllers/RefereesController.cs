﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonIsshoni.Net.Server.Areas.Identity.Data;
using PokemonIsshoni.Net.Shared.Models;

namespace PokemonIsshoni.Net.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefereesController : ControllerBase
    {
        private readonly PokemonIsshoniNetServerContext _context;

        public RefereesController(PokemonIsshoniNetServerContext context)
        {
            _context = context;
        }

        // GET: api/Referees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PCLReferee>>> GetPCLReferees()
        {
            if (_context.PCLReferees == null)
            {
                return NotFound();
            }
            return await _context.PCLReferees.ToListAsync();
        }

        // GET: api/Referees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PCLReferee>> GetReferee(int id)
        {
            if (_context.PCLReferees == null)
            {
                return NotFound();
            }
            var referee = await _context.PCLReferees.FindAsync(id);

            if (referee == null)
            {
                return NotFound();
            }

            return referee;
        }

        // PUT: api/Referees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReferee(int id, PCLReferee referee)
        {
            if (id != referee.Id)
            {
                return BadRequest();
            }

            _context.Entry(referee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RefereeExists(id))
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

        // POST: api/Referees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<PCLReferee>> PostReferee(PCLReferee referee)
        {
            if (_context.PCLReferees == null)
            {
                return Problem("Entity set 'PokemonIsshoniNetServerContext.PCLReferees'  is null.");
            }
            var uid = HttpContext.User.Claims.FirstOrDefault(s => s.Type.EndsWith("nameidentifier"));
            // 或者admin也行 要加入函数！
            var plcMatch = await _context.PCLMatchs.FindAsync(referee.PCLMatchId);
            if (!await HasPower(referee.PCLMatchId))
            {
                return Problem("没有权限");
            }
            if (_context.PCLReferees.Any(s => s.PCLMatchId == referee.PCLMatchId && s.UserId == referee.UserId))
            {
                return Problem("已经在裁判列表中");
            }
            _context.PCLReferees.Add(referee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReferee", new { id = referee.Id }, referee);
        }

        // DELETE: api/Referees/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReferee(int id)
        {
            if (_context.PCLReferees == null)
            {
                return NotFound();
            }
            var referee = await _context.PCLReferees.FindAsync(id);
            if (referee == null)
            {
                return NotFound();
            }
            if (!await HasPower(referee.PCLMatchId))
            {
                return Problem("没有权限");
            }
            //判断是不是创建者

            _context.PCLReferees.Remove(referee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RefereeExists(int id)
        {
            return (_context.PCLReferees?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<bool> HasPower(int matchId)
        {
            var uid = HttpContext.User.Claims.FirstOrDefault(s => s.Type.EndsWith("nameidentifier"));
            var plcMatch = await _context.PCLMatchs.FindAsync(matchId);
            return HttpContext.User.IsInRole("Admin") || plcMatch.UserId == uid.Value;
        }
    }
}

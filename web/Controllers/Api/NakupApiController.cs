using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

namespace web.Controllers_Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class NakupApiController : ControllerBase
    {
        private readonly KnjiznicaContext _context;

        public NakupApiController(KnjiznicaContext context)
        {
            _context = context;
        }

        // GET: api/NakupApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nakup>>> GetNakupi()
        {
            return await _context.Nakupi.ToListAsync();
        }

        // GET: api/NakupApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Nakup>> GetNakup(int id)
        {
            var nakup = await _context.Nakupi.FindAsync(id);

            if (nakup == null)
            {
                return NotFound();
            }

            return nakup;
        }

        // PUT: api/NakupApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNakup(int id, Nakup nakup)
        {
            if (id != nakup.NakupID)
            {
                return BadRequest();
            }

            _context.Entry(nakup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NakupExists(id))
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

        // POST: api/NakupApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Nakup>> PostNakup(Nakup nakup)
        {
            _context.Nakupi.Add(nakup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNakup", new { id = nakup.NakupID }, nakup);
        }

        // DELETE: api/NakupApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNakup(int id)
        {
            var nakup = await _context.Nakupi.FindAsync(id);
            if (nakup == null)
            {
                return NotFound();
            }

            _context.Nakupi.Remove(nakup);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NakupExists(int id)
        {
            return _context.Nakupi.Any(e => e.NakupID == id);
        }
    }
}

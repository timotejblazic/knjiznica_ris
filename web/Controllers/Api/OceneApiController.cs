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
    public class OceneApiController : ControllerBase
    {
        private readonly KnjiznicaContext _context;

        public OceneApiController(KnjiznicaContext context)
        {
            _context = context;
        }

        // GET: api/OceneApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ocena>>> GetOcene()
        {
            return await _context.Ocene.ToListAsync();
        }

        // GET: api/OceneApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ocena>> GetOcena(int id)
        {
            var ocena = await _context.Ocene.FindAsync(id);

            if (ocena == null)
            {
                return NotFound();
            }

            return ocena;
        }

        // PUT: api/OceneApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOcena(int id, Ocena ocena)
        {
            if (id != ocena.OcenaID)
            {
                return BadRequest();
            }

            _context.Entry(ocena).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OcenaExists(id))
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

        // POST: api/OceneApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ocena>> PostOcena(Ocena ocena)
        {
            _context.Ocene.Add(ocena);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOcena", new { id = ocena.OcenaID }, ocena);
        }

        // DELETE: api/OceneApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOcena(int id)
        {
            var ocena = await _context.Ocene.FindAsync(id);
            if (ocena == null)
            {
                return NotFound();
            }

            _context.Ocene.Remove(ocena);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OcenaExists(int id)
        {
            return _context.Ocene.Any(e => e.OcenaID == id);
        }
    }
}

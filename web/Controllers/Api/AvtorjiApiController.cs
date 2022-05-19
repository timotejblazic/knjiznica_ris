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
    public class AvtorjiApiController : ControllerBase
    {
        private readonly KnjiznicaContext _context;

        public AvtorjiApiController(KnjiznicaContext context)
        {
            _context = context;
        }

        // GET: api/AvtorjiApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Avtor>>> GetAvtorji()
        {
            return await _context.Avtorji.ToListAsync();
        }

        // GET: api/AvtorjiApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Avtor>> GetAvtor(int id)
        {
            var avtor = await _context.Avtorji.FindAsync(id);

            if (avtor == null)
            {
                return NotFound();
            }

            return avtor;
        }

        // PUT: api/AvtorjiApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAvtor(int id, Avtor avtor)
        {
            if (id != avtor.AvtorID)
            {
                return BadRequest();
            }

            _context.Entry(avtor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AvtorExists(id))
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

        // POST: api/AvtorjiApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Avtor>> PostAvtor(Avtor avtor)
        {
            _context.Avtorji.Add(avtor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAvtor", new { id = avtor.AvtorID }, avtor);
        }

        // DELETE: api/AvtorjiApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAvtor(int id)
        {
            var avtor = await _context.Avtorji.FindAsync(id);
            if (avtor == null)
            {
                return NotFound();
            }

            _context.Avtorji.Remove(avtor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AvtorExists(int id)
        {
            return _context.Avtorji.Any(e => e.AvtorID == id);
        }
    }
}

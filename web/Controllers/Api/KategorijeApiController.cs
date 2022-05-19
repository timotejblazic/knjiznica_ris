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
    public class KategorijeApiController : ControllerBase
    {
        private readonly KnjiznicaContext _context;

        public KategorijeApiController(KnjiznicaContext context)
        {
            _context = context;
        }

        // GET: api/KategorijeApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Kategorija>>> GetKategorije()
        {
            return await _context.Kategorije.ToListAsync();
        }

        // GET: api/KategorijeApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Kategorija>> GetKategorija(int id)
        {
            var kategorija = await _context.Kategorije.FindAsync(id);

            if (kategorija == null)
            {
                return NotFound();
            }

            return kategorija;
        }

        // PUT: api/KategorijeApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKategorija(int id, Kategorija kategorija)
        {
            if (id != kategorija.KategorijaID)
            {
                return BadRequest();
            }

            _context.Entry(kategorija).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KategorijaExists(id))
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

        // POST: api/KategorijeApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Kategorija>> PostKategorija(Kategorija kategorija)
        {
            _context.Kategorije.Add(kategorija);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKategorija", new { id = kategorija.KategorijaID }, kategorija);
        }

        // DELETE: api/KategorijeApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKategorija(int id)
        {
            var kategorija = await _context.Kategorije.FindAsync(id);
            if (kategorija == null)
            {
                return NotFound();
            }

            _context.Kategorije.Remove(kategorija);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KategorijaExists(int id)
        {
            return _context.Kategorije.Any(e => e.KategorijaID == id);
        }
    }
}

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
    public class IzposojeApiController : ControllerBase
    {
        private readonly KnjiznicaContext _context;

        public IzposojeApiController(KnjiznicaContext context)
        {
            _context = context;
        }

        // GET: api/IzposojeApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Izposoja>>> GetIzposoje()
        {
            return await _context.Izposoje.ToListAsync();
        }

        // GET: api/IzposojeApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Izposoja>> GetIzposoja(int id)
        {
            var izposoja = await _context.Izposoje.FindAsync(id);

            if (izposoja == null)
            {
                return NotFound();
            }

            return izposoja;
        }

        // PUT: api/IzposojeApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIzposoja(int id, Izposoja izposoja)
        {
            if (id != izposoja.IzposojaID)
            {
                return BadRequest();
            }

            _context.Entry(izposoja).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IzposojaExists(id))
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

        // POST: api/IzposojeApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Izposoja>> PostIzposoja(Izposoja izposoja)
        {
            _context.Izposoje.Add(izposoja);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIzposoja", new { id = izposoja.IzposojaID }, izposoja);
        }

        // DELETE: api/IzposojeApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIzposoja(int id)
        {
            var izposoja = await _context.Izposoje.FindAsync(id);
            if (izposoja == null)
            {
                return NotFound();
            }

            _context.Izposoje.Remove(izposoja);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool IzposojaExists(int id)
        {
            return _context.Izposoje.Any(e => e.IzposojaID == id);
        }
    }
}

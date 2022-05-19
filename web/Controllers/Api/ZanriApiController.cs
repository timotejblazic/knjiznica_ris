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
    public class ZanriApiController : ControllerBase
    {
        private readonly KnjiznicaContext _context;

        public ZanriApiController(KnjiznicaContext context)
        {
            _context = context;
        }

        // GET: api/ZanriApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Zanr>>> GetZanri()
        {
            return await _context.Zanri.ToListAsync();
        }

        // GET: api/ZanriApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Zanr>> GetZanr(int id)
        {
            var zanr = await _context.Zanri.FindAsync(id);

            if (zanr == null)
            {
                return NotFound();
            }

            return zanr;
        }

        // PUT: api/ZanriApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutZanr(int id, Zanr zanr)
        {
            if (id != zanr.ZanrID)
            {
                return BadRequest();
            }

            _context.Entry(zanr).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZanrExists(id))
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

        // POST: api/ZanriApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Zanr>> PostZanr(Zanr zanr)
        {
            _context.Zanri.Add(zanr);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetZanr", new { id = zanr.ZanrID }, zanr);
        }

        // DELETE: api/ZanriApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZanr(int id)
        {
            var zanr = await _context.Zanri.FindAsync(id);
            if (zanr == null)
            {
                return NotFound();
            }

            _context.Zanri.Remove(zanr);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ZanrExists(int id)
        {
            return _context.Zanri.Any(e => e.ZanrID == id);
        }
    }
}

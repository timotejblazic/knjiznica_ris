using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;
using web.Filters;


namespace web.Controllers_Api
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKeyAuth]
    public class ZalozbeApiController : ControllerBase
    {
        private readonly KnjiznicaContext _context;

        public ZalozbeApiController(KnjiznicaContext context)
        {
            _context = context;
        }

        // GET: api/ZalozbeApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Zalozba>>> GetZalozbe()
        {
            return await _context.Zalozbe.ToListAsync();
        }

        // GET: api/ZalozbeApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Zalozba>> GetZalozba(int id)
        {
            var zalozba = await _context.Zalozbe.FindAsync(id);

            if (zalozba == null)
            {
                return NotFound();
            }

            return zalozba;
        }

        // PUT: api/ZalozbeApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutZalozba(int id, Zalozba zalozba)
        {
            if (id != zalozba.ZalozbaID)
            {
                return BadRequest();
            }

            _context.Entry(zalozba).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZalozbaExists(id))
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

        // POST: api/ZalozbeApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Zalozba>> PostZalozba(Zalozba zalozba)
        {
            _context.Zalozbe.Add(zalozba);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetZalozba", new { id = zalozba.ZalozbaID }, zalozba);
        }

        // DELETE: api/ZalozbeApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZalozba(int id)
        {
            var zalozba = await _context.Zalozbe.FindAsync(id);
            if (zalozba == null)
            {
                return NotFound();
            }

            _context.Zalozbe.Remove(zalozba);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ZalozbaExists(int id)
        {
            return _context.Zalozbe.Any(e => e.ZalozbaID == id);
        }
    }
}

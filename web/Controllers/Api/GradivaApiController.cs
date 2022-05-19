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
    public class GradivaApiController : ControllerBase
    {
        private readonly KnjiznicaContext _context;

        public GradivaApiController(KnjiznicaContext context)
        {
            _context = context;
        }

        // GET: api/GradivaApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gradivo>>> GetGradiva()
        {
            return await _context.Gradiva.ToListAsync();
        }

        // GET: api/GradivaApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Gradivo>> GetGradivo(int id)
        {
            var gradivo = await _context.Gradiva.FindAsync(id);

            if (gradivo == null)
            {
                return NotFound();
            }

            return gradivo;
        }

        // PUT: api/GradivaApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGradivo(int id, Gradivo gradivo)
        {
            if (id != gradivo.GradivoID)
            {
                return BadRequest();
            }

            _context.Entry(gradivo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GradivoExists(id))
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

        // POST: api/GradivaApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Gradivo>> PostGradivo(Gradivo gradivo)
        {
            _context.Gradiva.Add(gradivo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGradivo", new { id = gradivo.GradivoID }, gradivo);
        }

        // DELETE: api/GradivaApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGradivo(int id)
        {
            var gradivo = await _context.Gradiva.FindAsync(id);
            if (gradivo == null)
            {
                return NotFound();
            }

            _context.Gradiva.Remove(gradivo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GradivoExists(int id)
        {
            return _context.Gradiva.Any(e => e.GradivoID == id);
        }
    }
}

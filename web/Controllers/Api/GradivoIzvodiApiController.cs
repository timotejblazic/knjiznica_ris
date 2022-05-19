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
    public class GradivoIzvodiApiController : ControllerBase
    {
        private readonly KnjiznicaContext _context;

        public GradivoIzvodiApiController(KnjiznicaContext context)
        {
            _context = context;
        }

        // GET: api/GradivoIzvodiApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GradivoIzvod>>> GetGradivoIzvodi()
        {
            return await _context.GradivoIzvodi.ToListAsync();
        }

        // GET: api/GradivoIzvodiApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GradivoIzvod>> GetGradivoIzvod(int id)
        {
            var gradivoIzvod = await _context.GradivoIzvodi.FindAsync(id);

            if (gradivoIzvod == null)
            {
                return NotFound();
            }

            return gradivoIzvod;
        }

        // PUT: api/GradivoIzvodiApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGradivoIzvod(int id, GradivoIzvod gradivoIzvod)
        {
            if (id != gradivoIzvod.GradivoIzvodID)
            {
                return BadRequest();
            }

            _context.Entry(gradivoIzvod).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GradivoIzvodExists(id))
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

        // POST: api/GradivoIzvodiApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GradivoIzvod>> PostGradivoIzvod(GradivoIzvod gradivoIzvod)
        {
            _context.GradivoIzvodi.Add(gradivoIzvod);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGradivoIzvod", new { id = gradivoIzvod.GradivoIzvodID }, gradivoIzvod);
        }

        // DELETE: api/GradivoIzvodiApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGradivoIzvod(int id)
        {
            var gradivoIzvod = await _context.GradivoIzvodi.FindAsync(id);
            if (gradivoIzvod == null)
            {
                return NotFound();
            }

            _context.GradivoIzvodi.Remove(gradivoIzvod);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GradivoIzvodExists(int id)
        {
            return _context.GradivoIzvodi.Any(e => e.GradivoIzvodID == id);
        }
    }
}

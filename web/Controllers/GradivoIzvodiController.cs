using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;
using Microsoft.AspNetCore.Authorization;

namespace web.Controllers
{
    public class GradivoIzvodiController : Controller
    {
        private readonly KnjiznicaContext _context;

        public GradivoIzvodiController(KnjiznicaContext context)
        {
            _context = context;
        }

        // GET: GradivoIzvodi
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.GradivoIzvodi.ToListAsync());
        }

        // GET: GradivoIzvodi/Details/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradivoIzvod = await _context.GradivoIzvodi
                .Include(g => g.Gradivo)
                .Include(i => i.Izposoja)
                .Include(n => n.Nakup)
                .FirstOrDefaultAsync(m => m.GradivoIzvodID == id);
            if (gradivoIzvod == null)
            {
                return NotFound();
            }

            return View(gradivoIzvod);
        }

        // GET: GradivoIzvodi/Create
        [Authorize(Roles = "Administrator,Moderator")]
        public IActionResult Create()
        {
            ViewData["GradivoID"] = new SelectList(_context.Gradiva, "GradivoID", "Naslov");
            return View();
        }

        // POST: GradivoIzvodi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GradivoIzvodID,GradivoID")] GradivoIzvod gradivoIzvod)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gradivoIzvod);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GradivoID"] = new SelectList(_context.Gradiva, "GradivoID", "Naslov", gradivoIzvod.GradivoID);
            return View(gradivoIzvod);
        }

        // GET: GradivoIzvodi/Edit/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradivoIzvod = await _context.GradivoIzvodi.FindAsync(id);
            if (gradivoIzvod == null)
            {
                return NotFound();
            }
            ViewData["GradivoID"] = new SelectList(_context.Gradiva, "GradivoID", "Naslov");
            ViewData["IzposojaID"] = gradivoIzvod.IzposojaID;
            return View(gradivoIzvod);
        }

        // POST: GradivoIzvodi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GradivoIzvodID,GradivoID,IzposojaID")] GradivoIzvod gradivoIzvod)
        {
            if (id != gradivoIzvod.GradivoIzvodID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gradivoIzvod);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradivoIzvodExists(gradivoIzvod.GradivoIzvodID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["GradivoID"] = new SelectList(_context.Gradiva, "GradivoID", "Naslov", gradivoIzvod.GradivoID);
            return View(gradivoIzvod);
        }

        // GET: GradivoIzvodi/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradivoIzvod = await _context.GradivoIzvodi
                .FirstOrDefaultAsync(m => m.GradivoIzvodID == id);
            if (gradivoIzvod == null)
            {
                return NotFound();
            }

            return View(gradivoIzvod);
        }

        // POST: GradivoIzvodi/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gradivoIzvod = await _context.GradivoIzvodi.FindAsync(id);
            _context.GradivoIzvodi.Remove(gradivoIzvod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> IzposojaIDZapisiVGradivoIzvod (int? idGradivoIzvod, int? idIzposoja)
        {
            var gradivoIzvod = await _context.GradivoIzvodi.Where(
                gi => gi.GradivoIzvodID == idGradivoIzvod).FirstOrDefaultAsync();

            gradivoIzvod.IzposojaID = idIzposoja;
            _context.Update(gradivoIzvod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Gradiva");
        }

        public async Task<IActionResult> NakupIDZapisiVGradivoIzvod (int? idGradivoIzvod, int? idNakup)
        {
            var gradivoIzvod = await _context.GradivoIzvodi.Where(
                gi => gi.GradivoIzvodID == idGradivoIzvod).FirstOrDefaultAsync();

            gradivoIzvod.NakupID = idNakup;
            _context.Update(gradivoIzvod);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Gradiva");
        }

        private bool GradivoIzvodExists(int id)
        {
            return _context.GradivoIzvodi.Any(e => e.GradivoIzvodID == id);
        }
    }
}

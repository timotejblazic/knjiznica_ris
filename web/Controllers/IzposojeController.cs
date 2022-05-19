using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace web.Controllers
{
    [Authorize]
    public class IzposojeController : Controller
    {
        private readonly KnjiznicaContext _context;
        private readonly UserManager<Uporabnik> _usermanager;

        public IzposojeController(KnjiznicaContext context, UserManager<Uporabnik> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

        // GET: Izposoje
        public async Task<IActionResult> Index()
        {
            var user = await _usermanager.GetUserAsync(HttpContext.User);
            var knjiznicaContext = _context.Izposoje.Include(i => i.Uporabnik).Where(id => id.Uporabnik.Id.Equals(user.Id));
            
            return View(await knjiznicaContext.ToListAsync());
        }

        // GET: Izposoje/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izposoja = await _context.Izposoje
                .Include(i => i.Uporabnik)
                .FirstOrDefaultAsync(m => m.IzposojaID == id);
            if (izposoja == null)
            {
                return NotFound();
            }

        
            ViewData["IzposojaID"] = id.Value;
            Izposoja izposoja1 = _context.Izposoje.Where(
                i => i.IzposojaID == id.Value).Single();

            GradivoIzvod gi1 = _context.GradivoIzvodi.Where(
                g => g.GradivoIzvodID == izposoja1.IdIzposojenegaGradiva).Single();
            
            Gradivo gr1 = _context.Gradiva.Where(
                gr => gr.GradivoID == gi1.GradivoID).Single();

            ViewData["GiID"] = gi1.GradivoIzvodID;
            ViewData["GNaslov"] = gr1.Naslov;
            ViewData["GStStrani"] = gr1.SteviloStrani;

            return View(izposoja);
        }

        // GET: Izposoje/Create
        public IActionResult Create(int? idGradivoIzvod)
        {
            TempData["idGI"] = idGradivoIzvod;
            return View();
        }

        // POST: Izposoje/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IzposojaID,DatumIzposoje,DatumVrnitve,UporabnikID,IdIzposojenegaGradiva")] Izposoja izposoja)
        {
            var currentUser = await _usermanager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                izposoja.DatumIzposoje = DateTime.Now;
                izposoja.DatumVrnitve = DateTime.Now.AddDays(14);
                izposoja.UporabnikID = currentUser.Id;
                izposoja.IdIzposojenegaGradiva = (int)TempData["idGI"];
                _context.Add(izposoja);
                await _context.SaveChangesAsync();
                return RedirectToAction("IzposojaIDZapisiVGradivoIzvod", "GradivoIzvodi", new { idGradivoIzvod = (int)TempData["idGI"], idIzposoja = izposoja.IzposojaID });
            }
            return View(izposoja);
        }

        // GET: Izposoje/Edit/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izposoja = await _context.Izposoje.FindAsync(id);
            if (izposoja == null)
            {
                return NotFound();
            }
            ViewData["UporabnikID"] = new SelectList(_context.Uporabniki, "Id", "Id", izposoja.UporabnikID);
            return View(izposoja);
        }

        // POST: Izposoje/Edit/5
        [Authorize(Roles = "Administrator,Moderator")]
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IzposojaID,DatumIzposoje,DatumVrnitve,UporabnikID")] Izposoja izposoja)
        {
            if (id != izposoja.IzposojaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(izposoja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IzposojaExists(izposoja.IzposojaID))
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
            ViewData["UporabnikID"] = new SelectList(_context.Uporabniki, "Id", "Id", izposoja.UporabnikID);
            return View(izposoja);
        }

        // GET: Izposoje/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izposoja = await _context.Izposoje
                .Include(i => i.Uporabnik)
                .FirstOrDefaultAsync(m => m.IzposojaID == id);
            if (izposoja == null)
            {
                return NotFound();
            }

            return View(izposoja);
        }

        // POST: Izposoje/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var izposoja = await _context.Izposoje.FindAsync(id);
            _context.Izposoje.Remove(izposoja);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IzposojaExists(int id)
        {
            return _context.Izposoje.Any(e => e.IzposojaID == id);
        }
    }
}

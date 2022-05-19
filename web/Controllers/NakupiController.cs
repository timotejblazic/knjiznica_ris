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
    public class NakupiController : Controller
    {
        private readonly KnjiznicaContext _context;
        private readonly UserManager<Uporabnik> _usermanager;

        public NakupiController(KnjiznicaContext context, UserManager<Uporabnik> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

        // GET: Nakupi
        public async Task<IActionResult> Index()
        {
            var user = await _usermanager.GetUserAsync(HttpContext.User);
            var knjiznicaContext = _context.Nakupi.Include(i => i.Uporabnik).Where(id => id.Uporabnik.Id.Equals(user.Id));
            return View(await knjiznicaContext.ToListAsync());
        }

        // GET: Nakupi/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nakup = await _context.Nakupi
                .Include(n => n.Uporabnik)
                .FirstOrDefaultAsync(m => m.NakupID == id);
            if (nakup == null)
            {
                return NotFound();
            }

            ViewData["NakupID"] = id.Value;
            Nakup nakup1 = _context.Nakupi.Where(
                i => i.NakupID == id.Value).Single();

            GradivoIzvod gi1 = _context.GradivoIzvodi.Where(
                g => g.GradivoIzvodID == nakup1.IdKupljenegaGradiva).Single();
            
            Gradivo gr1 = _context.Gradiva.Where(
                gr => gr.GradivoID == gi1.GradivoID).Single();

            ViewData["GiID"] = gi1.GradivoIzvodID;
            ViewData["GNaslov"] = gr1.Naslov;
            ViewData["GStStrani"] = gr1.SteviloStrani;
            ViewData["GCena"] = gr1.CenaGradivo;

            return View(nakup);
        }

        // GET: Nakupi/Create
        public IActionResult Create(int? idGradivoIzvod)
        {
            TempData["idGI"] = idGradivoIzvod;
            return View();
        }

        // POST: Nakupi/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NakupID,DatumNakupa,UporabnikID,IdKupljenegaGradiva")] Nakup nakup)
        {
            var currentUser = await _usermanager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                nakup.DatumNakupa = DateTime.Now;
                nakup.UporabnikID = currentUser.Id;
                nakup.IdKupljenegaGradiva = (int)TempData["idGI"];
                _context.Add(nakup);
                await _context.SaveChangesAsync();
                return RedirectToAction("NakupIDZapisiVGradivoIzvod", "GradivoIzvodi", new { idGradivoIzvod = (int)TempData["idGI"], idNakup = nakup.NakupID });
            }
            return View(nakup);
        }

        // GET: Nakupi/Edit/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nakup = await _context.Nakupi.FindAsync(id);
            if (nakup == null)
            {
                return NotFound();
            }
            ViewData["UporabnikID"] = new SelectList(_context.Uporabniki, "Id", "Id", nakup.UporabnikID);
            return View(nakup);
        }

        // POST: Nakupi/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NakupID,DatumNakupa,UporabnikID")] Nakup nakup)
        {
            if (id != nakup.NakupID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nakup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NakupExists(nakup.NakupID))
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
            ViewData["UporabnikID"] = new SelectList(_context.Uporabniki, "Id", "Id", nakup.UporabnikID);
            return View(nakup);
        }

        // GET: Nakupi/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nakup = await _context.Nakupi
                .Include(n => n.Uporabnik)
                .FirstOrDefaultAsync(m => m.NakupID == id);
            if (nakup == null)
            {
                return NotFound();
            }

            return View(nakup);
        }

        // POST: Nakupi/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var nakup = await _context.Nakupi.FindAsync(id);
            _context.Nakupi.Remove(nakup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NakupExists(int id)
        {
            return _context.Nakupi.Any(e => e.NakupID == id);
        }
    }
}

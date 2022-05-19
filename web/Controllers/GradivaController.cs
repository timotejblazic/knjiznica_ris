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
    public class GradivaController : Controller
    {
        private readonly KnjiznicaContext _context;

        public GradivaController(KnjiznicaContext context)
        {
            _context = context;
        }

        // GET: Gradiva
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber, int? id)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NaslovSortParm"] = String.IsNullOrEmpty(sortOrder) ? "naslov_desc" : "";
            ViewData["LetoIzdajeSortParm"] = (String.IsNullOrEmpty(sortOrder) || sortOrder=="letoIzdaje") ? "letoIzdaje_desc" : "letoIzdaje";
            ViewData["SteviloStraniSortParm"] = (String.IsNullOrEmpty(sortOrder) || sortOrder=="steviloStrani") ? "steviloStrani_desc" : "steviloStrani";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            
            ViewData["CurrentFilter"] = searchString;
            var gradiva = from g in _context.Gradiva
                                        .Include(gi => gi.GradivoIzvodi)
                                            .ThenInclude(iz => iz.Izposoja)
                                        .Include(z => z.Zanr)
                                        .Include(k => k.Kategorija)
                                        .Include(za => za.Zalozba)
                                        .Include(a => a.Avtor)
                                        .AsNoTracking()
                        select g;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                gradiva = gradiva.Where(g => g.Naslov.Contains(searchString)
                                    || g.Opis.Contains(searchString)
                                    || g.Kategorija.Naziv.Contains(searchString)
                                    || g.Zanr.Naziv.Contains(searchString)
                                    || g.Zalozba.Naziv.Contains(searchString)
                                    || g.Avtor.Ime.Contains(searchString)
                                    || g.Avtor.Priimek.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "naslov_desc":
                    gradiva = gradiva.OrderByDescending(g => g.Naslov);
                    break;
                case "letoIzdaje":
                    gradiva = gradiva.OrderBy(g => g.LetoIzdaje);
                    break;
                case "letoIzdaje_desc":
                    gradiva = gradiva.OrderByDescending(g => g.LetoIzdaje);
                    break;
                case "steviloStrani":
                    gradiva = gradiva.OrderBy(g => g.SteviloStrani);
                    break;
                case "steviloStrani_desc":
                    gradiva = gradiva.OrderByDescending(g => g.SteviloStrani);
                    break;
                default:
                    gradiva = gradiva.OrderBy(g => g.Naslov);
                    break;
            }

            if (id != null)
            {
                ViewData["GradivoID"] = id.Value;
                Gradivo gradivo1 = gradiva.Where(
                    g => g.GradivoID == id.Value).Single();
                ViewData["GradivoIzvodiFromGradivoID"] = gradivo1.GradivoIzvodi;
            }

            int pageSize = 5;
            return View(await PaginatedList<Gradivo>.CreateAsync(gradiva.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Gradiva/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradivo = await _context.Gradiva
                .Include(o => o.Ocene)
                .Include(a => a.Avtor)
                .Include(z => z.Zanr)
                .Include(k => k.Kategorija)
                .Include(za => za.Zalozba)
                .FirstOrDefaultAsync(m => m.GradivoID == id);
            if (gradivo == null)
            {
                return NotFound();
            }
            if (id != null)
            {
                ViewData["GradivoID"] = id.Value;
                Gradivo gradivo1 = _context.Gradiva.Where(
                    g => g.GradivoID == id.Value).Single();
                ViewData["OceneFromGradivoID"] = gradivo1.Ocene;
            }

            return View(gradivo);
        }

        [Authorize(Roles = "Administrator,Moderator")]
        // GET: Gradiva/Create
        public IActionResult Create()
        {
            ViewData["KategorijaID"] = new SelectList(_context.Kategorije, "KategorijaID", "Naziv");
            ViewData["ZanrID"] = new SelectList(_context.Zanri, "ZanrID", "Naziv");
            ViewData["ZalozbaID"] = new SelectList(_context.Zalozbe, "ZalozbaID", "Naziv");
            ViewData["AvtorID"] = new SelectList(_context.Avtorji, "AvtorID", "PolnoIme");
            return View();
        }

        // POST: Gradiva/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GradivoID,Naslov,LetoIzdaje,SteviloStrani,Opis,CenaGradivo,KategorijaID,ZanrID,ZalozbaID,AvtorID")] Gradivo gradivo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gradivo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["KategorijaID"] = new SelectList(_context.Kategorije, "KategorijaID", "Naziv", gradivo.KategorijaID);
            ViewData["ZanrID"] = new SelectList(_context.Zanri, "ZanrID", "Naziv", gradivo.ZanrID);
            ViewData["ZalozbaID"] = new SelectList(_context.Zalozbe, "ZalozbaID", "Naziv", gradivo.ZalozbaID);
            ViewData["AvtorID"] = new SelectList(_context.Avtorji, "AvtorID", "PolnoIme", gradivo.AvtorID);
            return View(gradivo);
        }

        // GET: Gradiva/Edit/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradivo = await _context.Gradiva.FindAsync(id);
            if (gradivo == null)
            {
                return NotFound();
            }

            ViewData["KategorijaID"] = new SelectList(_context.Kategorije, "KategorijaID", "Naziv");
            ViewData["ZanrID"] = new SelectList(_context.Zanri, "ZanrID", "Naziv");
            ViewData["ZalozbaID"] = new SelectList(_context.Zalozbe, "ZalozbaID", "Naziv");
            ViewData["AvtorID"] = new SelectList(_context.Avtorji, "AvtorID", "PolnoIme");
            return View(gradivo);
        }

        // POST: Gradiva/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GradivoID,Naslov,LetoIzdaje,SteviloStrani,Opis,CenaGradivo,KategorijaID,ZanrID,ZalozbaID,AvtorID")] Gradivo gradivo)
        {
            if (id != gradivo.GradivoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gradivo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradivoExists(gradivo.GradivoID))
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

            ViewData["KategorijaID"] = new SelectList(_context.Kategorije, "KategorijaID", "Naziv", gradivo.KategorijaID);
            ViewData["ZanrID"] = new SelectList(_context.Zanri, "ZanrID", "Naziv", gradivo.ZanrID);
            ViewData["ZalozbaID"] = new SelectList(_context.Zalozbe, "ZalozbaID", "Naziv", gradivo.ZalozbaID);
            ViewData["AvtorID"] = new SelectList(_context.Avtorji, "AvtorID", "PolnoIme", gradivo.AvtorID);
            return View(gradivo);
        }

        // GET: Gradiva/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gradivo = await _context.Gradiva
                .FirstOrDefaultAsync(m => m.GradivoID == id);
            if (gradivo == null)
            {
                return NotFound();
            }

            return View(gradivo);
        }

        // POST: Gradiva/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gradivo = await _context.Gradiva.FindAsync(id);
            _context.Gradiva.Remove(gradivo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GradivoExists(int id)
        {
            return _context.Gradiva.Any(e => e.GradivoID == id);
        }
    }
}

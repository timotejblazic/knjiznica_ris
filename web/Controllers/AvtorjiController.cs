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
    public class AvtorjiController : Controller
    {
        private readonly KnjiznicaContext _context;

        public AvtorjiController(KnjiznicaContext context)
        {
            _context = context;
        }

        // GET: Avtorji
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ImeSortParm"] = String.IsNullOrEmpty(sortOrder) ? "ime_desc" : "";
            ViewData["PriimekSortParm"] = (String.IsNullOrEmpty(sortOrder) || sortOrder=="priimek") ? "priimek_desc" : "priimek";
            
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            
            ViewData["CurrentFilter"] = searchString;
            var avtorji = from a in _context.Avtorji
                        select a;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                avtorji = avtorji.Where(a => a.Priimek.Contains(searchString)
                                    || a.Ime.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "ime_desc":
                    avtorji = avtorji.OrderByDescending(a => a.Ime);
                    break;
                case "priimek":
                    avtorji = avtorji.OrderBy(a => a.Priimek);
                    break;
                case "priimek_desc":
                    avtorji = avtorji.OrderByDescending(a => a.Priimek);
                    break;
                default:
                    avtorji = avtorji.OrderBy(a => a.Ime);
                    break;
            }

            int pageSize = 5;
            return View(await PaginatedList<Avtor>.CreateAsync(avtorji.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Avtorji/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avtor = await _context.Avtorji
                .Include(g => g.Gradiva)
                .FirstOrDefaultAsync(m => m.AvtorID == id);
            if (avtor == null)
            {
                return NotFound();
            }

            if (id != null)
            {
                ViewData["AvtorID"] = id.Value;
                Avtor avtor1 = _context.Avtorji.Where(
                    a => a.AvtorID == id.Value).Single();
                ViewData["GradivaFromAvtorID"] = avtor1.Gradiva;
            }

            return View(avtor);
        }
        
        [Authorize(Roles = "Administrator,Moderator")]
        // GET: Avtorji/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Avtorji/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AvtorID,Ime,Priimek,Opis")] Avtor avtor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(avtor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(avtor);
        }

        [Authorize(Roles = "Administrator,Moderator")]
        // GET: Avtorji/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avtor = await _context.Avtorji.FindAsync(id);
            if (avtor == null)
            {
                return NotFound();
            }
            return View(avtor);
        }

        // POST: Avtorji/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AvtorID,Ime,Priimek,Opis")] Avtor avtor)
        {
            if (id != avtor.AvtorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(avtor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AvtorExists(avtor.AvtorID))
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
            return View(avtor);
        }

        // GET: Avtorji/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var avtor = await _context.Avtorji
                .FirstOrDefaultAsync(m => m.AvtorID == id);
            if (avtor == null)
            {
                return NotFound();
            }

            return View(avtor);
        }

        // POST: Avtorji/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var avtor = await _context.Avtorji.FindAsync(id);
            _context.Avtorji.Remove(avtor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AvtorExists(int id)
        {
            return _context.Avtorji.Any(e => e.AvtorID == id);
        }
    }
}

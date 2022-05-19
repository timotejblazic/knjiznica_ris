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
    public class ZalozbeController : Controller
    {
        private readonly KnjiznicaContext _context;

        public ZalozbeController(KnjiznicaContext context)
        {
            _context = context;
        }

        // GET: Zalozbe
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NazivSortParm"] = String.IsNullOrEmpty(sortOrder) ? "naziv_desc" : "";
            
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            
            ViewData["CurrentFilter"] = searchString;
            var zalozbe = from z in _context.Zalozbe
                        select z;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                zalozbe = zalozbe.Where(z => z.Naziv.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "naziv_desc":
                    zalozbe = zalozbe.OrderByDescending(z => z.Naziv);
                    break;
                default:
                    zalozbe = zalozbe.OrderBy(z => z.Naziv);
                    break;
            }

            int pageSize = 5;
            return View(await PaginatedList<Zalozba>.CreateAsync(zalozbe.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Zalozbe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zalozba = await _context.Zalozbe
                .FirstOrDefaultAsync(m => m.ZalozbaID == id);
            if (zalozba == null)
            {
                return NotFound();
            }

            return View(zalozba);
        }

        // GET: Zalozbe/Create
        [Authorize(Roles = "Administrator,Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Zalozbe/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ZalozbaID,Naziv,TelefonskaStevilka,Naslov")] Zalozba zalozba)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zalozba);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zalozba);
        }

        // GET: Zalozbe/Edit/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zalozba = await _context.Zalozbe.FindAsync(id);
            if (zalozba == null)
            {
                return NotFound();
            }
            return View(zalozba);
        }

        // POST: Zalozbe/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ZalozbaID,Naziv,TelefonskaStevilka,Naslov")] Zalozba zalozba)
        {
            if (id != zalozba.ZalozbaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zalozba);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZalozbaExists(zalozba.ZalozbaID))
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
            return View(zalozba);
        }

        // GET: Zalozbe/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zalozba = await _context.Zalozbe
                .FirstOrDefaultAsync(m => m.ZalozbaID == id);
            if (zalozba == null)
            {
                return NotFound();
            }

            return View(zalozba);
        }

        // POST: Zalozbe/Delete/5
        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zalozba = await _context.Zalozbe.FindAsync(id);
            _context.Zalozbe.Remove(zalozba);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZalozbaExists(int id)
        {
            return _context.Zalozbe.Any(e => e.ZalozbaID == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bliss_Programma.Data;
using Bliss_Programma.Models;
using Microsoft.AspNetCore.Authorization;

namespace Bliss_Programma.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LocatiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocatiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Locaties
        public async Task<IActionResult> Index()
        {
            return View(await _context.Locatie.ToListAsync());
        }

        // GET: Locaties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locatie = await _context.Locatie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locatie == null)
            {
                return NotFound();
            }
            foreach (Ruimte r in _context.Ruimte)
            {
                if (locatie.Id == r.LocatieId)
                {
                    if (!locatie.Werkplekken.Contains(r))
                    {
                        locatie.Werkplekken.Add(r);
                    }
                }
            }

            return View(locatie);
        }

        public async Task<IActionResult> EditRuimte(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruimte = await _context.Ruimte
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ruimte == null)
            {
                return NotFound();
            }

            return View(ruimte);
        }

        // GET: Locaties/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Locaties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Straatnaam,Nummer,Toevoeging,Postcode,Plaatsnaam")] Locatie locatie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(locatie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(locatie);
        }

        // GET: Locaties/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locatie = await _context.Locatie.FindAsync(id);
            if (locatie == null)
            {
                return NotFound();
            }
            return View(locatie);
        }

        // POST: Locaties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Straatnaam,Nummer,Toevoeging,Postcode,Plaatsnaam")] Locatie locatie)
        {
            if (id != locatie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(locatie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocatieExists(locatie.Id))
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
            return View(locatie);
        }

        // GET: Locaties/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locatie = await _context.Locatie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locatie == null)
            {
                return NotFound();
            }

            return View(locatie);
        }

        // POST: Locaties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var locatie = await _context.Locatie.FindAsync(id);
            _context.Locatie.Remove(locatie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocatieExists(int id)
        {
            return _context.Locatie.Any(e => e.Id == id);
        }
    }
}

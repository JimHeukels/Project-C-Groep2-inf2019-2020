﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bliss_Programma.Data;
using Bliss_Programma.Models;
using Bliss_Programma.Services;

namespace Bliss_Programma.Controllers
{
    public class RuimtesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RuimtesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Ruimtes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Ruimte.Include(r => r.Locatie);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Ruimtes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruimte = await _context.Ruimte
                .Include(r => r.Locatie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ruimte == null)
            {
                return NotFound();
            }

            return View(ruimte);
        }

        // GET: Ruimtes/Create
        public IActionResult Create(int ? id)
        {
            ViewData["LocatieId"] = id;
            return View();
        }

        // POST: Ruimtes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Lengte,Breedte,Oppervlakte,MaxWerkplekken,Naam,LocatieId")] Ruimte ruimte)
        {
            if (ModelState.IsValid && (int)Math.Round(ruimte.Oppervlakte / 1.95) >= ruimte.MaxWerkplekken)
            {
                ruimte.Id = 0;
                _context.Add(ruimte);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Locaties", new { id = ruimte.LocatieId });
            }
            ViewData["LocatieId"] = ruimte.LocatieId;
            ViewData["Error"] = "De maximum aantal werkplekken voor deze ruimte is " + (int)Math.Round(ruimte.Oppervlakte / 1.95);
            return View(ruimte);
        }

        // GET: Ruimtes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruimte = await _context.Ruimte.FindAsync(id);
            if (ruimte == null)
            {
                return NotFound();
            }
            ViewData["LocatieId"] = new SelectList(_context.Locatie, "Id", "Id", ruimte.LocatieId);
            return View(ruimte);
        }

        // POST: Ruimtes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Lengte,Breedte,Oppervlakte,MaxWerkplekken,Naam,LocatieId")] Ruimte ruimte)
        {
            if (id != ruimte.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid && Functies.maxbezetting(ruimte.Oppervlakte) >= ruimte.MaxWerkplekken)
            {
                try
                {
                    _context.Update(ruimte);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RuimteExists(ruimte.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Locaties", new { id = ruimte.LocatieId });
            }
            ViewData["LocatieId"] = new SelectList(_context.Locatie, "Id", "Id", ruimte.LocatieId);
            ViewData["Error"] = "De maximum aantal werkplekken voor deze ruimte is " + Functies.maxbezetting(ruimte.Oppervlakte);
            return View(ruimte);
        }

        // GET: Ruimtes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruimte = await _context.Ruimte
                .Include(r => r.Locatie)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ruimte == null)
            {
                return NotFound();
            }

            return View(ruimte);
        }

        // POST: Ruimtes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ruimte = await _context.Ruimte.FindAsync(id);
            _context.Ruimte.Remove(ruimte);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Locaties", new { id = ruimte.LocatieId });
        }

        private bool RuimteExists(int id)
        {
            return _context.Ruimte.Any(e => e.Id == id);
        }
    }
}

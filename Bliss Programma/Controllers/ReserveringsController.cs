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
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Bliss_Programma.Controllers
{
    //Authorize functie controleert of huidige sessie/gebruiker voldaan is. Als er geen ingelogde gebruiker is, wordt de gebruiker doorverwezen naar 
    //inlogpagina/account creatie
    [Authorize]
    public class ReserveringsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReserveringsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reserverings
        public async Task<IActionResult> Index()
        {
            // var x wrdt hieronder gedeclareerd en ingevuld met een user type. Dit is een standaard type(?) en heeft dus toegang tot gegevens van de gebruiker/sessie
            var x = User.FindFirst(ClaimTypes.NameIdentifier);

            //todo: de x.value statement vervangen met een Admin check
            if(x.Value == "46554b97-f02b-4ff6-bee4-9f91ee3be03a")
            {
                Console.WriteLine("test");
                return View(await _context.Reservering.Include(r => r.Ruimte).ThenInclude(x => x.Locatie).OrderBy(r => r.Datum).ToListAsync());
                
            }
            // _context is de door ons gedefinieërde 'database' variabele.
            // vervolgens kunnen we aan de hand van Linq een query schrijven naar die _context veriabele
            // In de Value variabele wordt standaard dus de id van de gebruiker opgeslagen.
            // Door x.Value dus mee te nemen in de query kunnen we alle reserveringen ophalen van 1 specifieke gebruiker
            var applicationDbContext = _context.Reservering.Where(y => y.WerknemerId == x.Value && y.Datum >= DateTime.Now).Include(r => r.Ruimte).ThenInclude(x => x.Locatie).OrderBy(r => r.Datum);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Reserverings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservering = await _context.Reservering
                .Include(r => r.Ruimte)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservering == null)
            {
                return NotFound();
            }

            return View(reservering);
        }

        // GET: Reserverings/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["RuimteId"] = new SelectList(_context.Ruimte, "Id", "Naam");
            return View();
        }

        // POST: Reserverings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Datum,WerknemerId,WerknemerEmail,RuimteId")] Reservering reservering)
        {
            if (ModelState.IsValid)
            {
                //currentRuimte haalt uit de database de informatie op van de geselecteerde ruimte. Door Single te gebruiken ipv where krijg je daadwerkelijk
                //één object terug (ipv een list met objecten wanneer je Where zou gebruiken) wat later aangesproken kan worden.
                //reserveringenCount haalt vervolgens het huidige aantal reserveringen op uit het object
                var currentRuimte = _context.Ruimte.Single(y => y.Id == reservering.RuimteId);
                int reserveringenCount = _context.Reservering.Count(y => y.RuimteId == reservering.RuimteId && y.Datum.Date == reservering.Datum.Date);
                
                //Doordat we hierboven list hebben gebruikt kunnen we nu maxPersonenRuimte vullen door het currentRuimte object direct aan te spreken.
                var maxPersonenRuimte = currentRuimte.MaxWerkplekken;

                if (_context.Reservering.FirstOrDefault(x => x.WerknemerId == reservering.WerknemerId && x.Datum.Date == reservering.Datum.Date) != null)
                {
                    //Gebruiker wordt nu teruggestuurd naar de reservering index pagina
                    //todo: geef de gebruiker een melding mee dat hij niet kan reserveren omdat er geen plek is.
                    TempData["Status"] = "U heeft deze ruimte al gereserveerd deze dag, uw reservering is niet toegevoegd.";
                    return RedirectToAction(nameof(Index));
                }

                if (reserveringenCount < maxPersonenRuimte)
                {
                    _context.Add(reservering);
                    await _context.SaveChangesAsync();
                    TempData["Status"] = "Uw reservering is succesvol toegevoegd.";
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    //Gebruiker wordt nu teruggestuurd naar de reservering index pagina
                    //todo: geef de gebruiker een melding mee dat hij niet kan reserveren omdat er geen plek is.
                    TempData["Status"] = "Deze ruimte is vol, uw reservering is niet toegevoegd.";
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["RuimteId"] = new SelectList(_context.Ruimte, "Id", "Id", reservering.RuimteId);
            return View(reservering);
        }

        // GET: Reserverings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservering = await _context.Reservering.FindAsync(id);
            if (reservering == null)
            {
                return NotFound();
            }
            ViewData["RuimteId"] = new SelectList(_context.Ruimte, "Id", "Id", reservering.RuimteId);
            return View(reservering);
        }

        // POST: Reserverings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Datum,WerknemerId,WerknemerEmail,RuimteId")] Reservering reservering)
        {
            //if (id != reservering.Id)
            //{
            //    return NotFound();
            //}

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(reservering);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!ReserveringExists(reservering.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["RuimteId"] = new SelectList(_context.Ruimte, "Id", "Id", reservering.RuimteId);
            //return View(reservering);

            if (ModelState.IsValid)
            {
                //currentRuimte haalt uit de database de informatie op van de geselecteerde ruimte. Door Single te gebruiken ipv where krijg je daadwerkelijk
                //één object terug (ipv een list met objecten wanneer je Where zou gebruiken) wat later aangesproken kan worden.
                //reserveringenCount haalt vervolgens het huidige aantal reserveringen op uit het object
                var currentRuimte = _context.Ruimte.Single(y => y.Id == reservering.RuimteId);
                int reserveringenCount = _context.Reservering.Count(y => y.RuimteId == reservering.RuimteId && y.Datum.Date == reservering.Datum.Date);

                //Doordat we hierboven list hebben gebruikt kunnen we nu maxPersonenRuimte vullen door het currentRuimte object direct aan te spreken.
                var maxPersonenRuimte = currentRuimte.MaxWerkplekken;

                if (_context.Reservering.FirstOrDefault(x => x.WerknemerId == reservering.WerknemerId && x.Datum.Date == reservering.Datum.Date) != null)
                {
                    //Gebruiker wordt nu teruggestuurd naar de reservering index pagina
                    //todo: geef de gebruiker een melding mee dat hij niet kan reserveren omdat er geen plek is.
                    TempData["Status"] = "U heeft deze ruimte al gereserveerd deze dag, uw reservering is niet aangepast.";
                    return RedirectToAction(nameof(Index));
                }

                if (reserveringenCount < maxPersonenRuimte)
                {
                    _context.Update(reservering);
                    await _context.SaveChangesAsync();
                    TempData["Status"] = "Uw reservering is succesvol aangepast.";
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    //Gebruiker wordt nu teruggestuurd naar de reservering index pagina
                    //todo: geef de gebruiker een melding mee dat hij niet kan reserveren omdat er geen plek is.
                    TempData["Status"] = "Deze ruimte is vol, uw reservering is niet aangepast.";
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["RuimteId"] = new SelectList(_context.Ruimte, "Id", "Id", reservering.RuimteId);
            return View(reservering);
        }

        // GET: Reserverings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservering = await _context.Reservering
                .Include(r => r.Ruimte)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservering == null)
            {
                return NotFound();
            }

            return View(reservering);
        }

        // POST: Reserverings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservering = await _context.Reservering.FindAsync(id);
            _context.Reservering.Remove(reservering);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReserveringExists(int id)
        {
            return _context.Reservering.Any(e => e.Id == id);
        }
    }
}

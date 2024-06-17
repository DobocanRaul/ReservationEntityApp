using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP_WebApp.Data;
using ASP_WebApp.Models;

namespace ASP_WebApp.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DocumentsController(ApplicationDbContext context)
        {
            _context = context;

        }

        // GET: Documents
        public async Task<IActionResult> Index()
            {
            var token = HttpContext.Session.GetString("Token");
            if(token=="not valid")
                return RedirectToAction("Index", "Home");
            return View(_context.Documents.ToList());
        }

        public async Task<IActionResult> Search(string format)
        {
            var token = HttpContext.Session.GetString("Token");
            if (token == "not valid")
                return RedirectToAction("Index", "Home");
            if(format==null)
                return RedirectToAction("Index");
            var documents = await _context.Documents
                .Where(d => d.Format.Contains(format))
                .ToListAsync();
            return View("Index", documents);
        }
        public async Task<IActionResult> DeleteReservations()
        {
            var token= HttpContext.Session.GetString("Token");
            var reservations = await _context.Reservations.Where(r => r.token == token).ToListAsync();

            foreach (var reservation in reservations)
            {
                if (reservation.Type == "Flight")
                {
                    var flight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == reservation.IdReservedResource);
                    flight.AvailableSeats++;
                }
                else { 
                    var hotel= await _context.Hotels.FirstOrDefaultAsync(h => h.Id == reservation.IdReservedResource);
                    hotel.AvailableRooms++;
                }
                _context.Reservations.Remove(reservation);

                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            var token = HttpContext.Session.GetString("Token");
            if (token == "not valid")
                return RedirectToAction("Index", "Home");
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Author,Format,Pages")] Document document)
        {

            var token = HttpContext.Session.GetString("Token");
            if (token == "not valid")
                return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {
                _context.Add(document);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            var token = HttpContext.Session.GetString("Token");
            if (token == "not valid")
                return RedirectToAction("Index", "Home");
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Author,Format,Pages")] Document document)
        {

            var token = HttpContext.Session.GetString("Token");
            if (token == "not valid")
                return RedirectToAction("Index", "Home");
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.Id))
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
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            var token = HttpContext.Session.GetString("Token");
            if (token == "not valid")
                return RedirectToAction("Index", "Home");
            if (id == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var token = HttpContext.Session.GetString("Token");
            if (token == "not valid")
                return RedirectToAction("Index", "Home");
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                _context.Documents.Remove(document);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}

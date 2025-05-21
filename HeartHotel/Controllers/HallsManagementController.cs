using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HeartHotel.Models;
using Firoozi.Helper;

namespace HeartHotel.Controllers
{
    public class HallsManagementController : Controller
    {
        private readonly EventisContext _context;

        public HallsManagementController(EventisContext context)
        {
            _context = context;
        }

        // GET: HallsManagement
        public async Task<IActionResult> Index(int? pageNumber, string k, int pageSize = 25)
        {
            var UserId = Helper.getUserId(HttpContext);
            if (UserId == 0)
            {
                return Redirect("/Login");
            }
            if (UserId > 10)
            {
                return NotFound();
            }
            ViewBag.UID = UserId;

            var eventisContext = _context.VenueHalls.Include(v => v.Venue);
            // return View(await eventisContext.ToListAsync());
            return View(await PaginatedList<VenueHall>.CreateAsync(eventisContext.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: HallsManagement/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var UserId = Helper.getUserId(HttpContext);
            if (UserId == 0)
            {
                return Redirect("/Login");
            }
            if (UserId > 10)
            {
                return NotFound();
            }
            ViewBag.UID = UserId;

            var venueHall = await _context.VenueHalls
                .Include(v => v.Venue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venueHall == null)
            {
                return NotFound();
            }

            return View(venueHall);
        }

        // GET: HallsManagement/Create
        public IActionResult Create()
        {
            var UserId = Helper.getUserId(HttpContext);
            if (UserId == 0)
            {
                return Redirect("/Login");
            }
            if (UserId > 10)
            {
                return NotFound();
            }
            ViewBag.UID = UserId;

            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Id");
            return View();
        }

        // POST: HallsManagement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VenueId,Title,Address")] VenueHall venueHall)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venueHall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Id", venueHall.VenueId);
            return View(venueHall);
        }

        // GET: HallsManagement/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var UserId = Helper.getUserId(HttpContext);
            if (UserId == 0)
            {
                return Redirect("/Login");
            }
            if (UserId > 10)
            {
                return NotFound();
            }
            ViewBag.UID = UserId;

            var venueHall = await _context.VenueHalls.FindAsync(id);
            if (venueHall == null)
            {
                return NotFound();
            }
            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Id", venueHall.VenueId);
            return View(venueHall);
        }

        // POST: HallsManagement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VenueId,Title,Address")] VenueHall venueHall)
        {
            if (id != venueHall.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venueHall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueHallExists(venueHall.Id))
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
            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Id", venueHall.VenueId);
            return View(venueHall);
        }

        // GET: HallsManagement/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var UserId = Helper.getUserId(HttpContext);
            if (UserId == 0)
            {
                return Redirect("/Login");
            }
            if (UserId > 10)
            {
                return NotFound();
            }
            ViewBag.UID = UserId;

            var venueHall = await _context.VenueHalls
                .Include(v => v.Venue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venueHall == null)
            {
                return NotFound();
            }

            return View(venueHall);
        }

        // POST: HallsManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venueHall = await _context.VenueHalls.FindAsync(id);
            if (venueHall != null)
            {
                _context.VenueHalls.Remove(venueHall);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VenueHallExists(int id)
        {
            return _context.VenueHalls.Any(e => e.Id == id);
        }
    }
}

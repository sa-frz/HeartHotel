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
    public class VenueHallManagerController : Controller
    {
        private readonly EventisContext _context;

        public VenueHallManagerController(EventisContext context)
        {
            _context = context;
        }

        // GET: VenueHallManager
        public async Task<IActionResult> Index()
        {
            var UserId = Helper.getUserId(HttpContext);
            if (UserId == 0)
            {
                return Redirect("/Login");
            }
            ViewBag.UID = UserId;

            var eventisContext = _context.VenueHallManagers
                                    .Include(v => v.User).Include(v => v.VenueHall)
                                    .OrderBy(o => o.User.Id).ThenBy(o => o.VenueHall.Id);
            return View(await eventisContext.ToListAsync());
        }

        // GET: VenueHallManager/Details/5
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
            ViewBag.UID = UserId;

            var venueHallManager = await _context.VenueHallManagers
                .Include(v => v.User)
                .Include(v => v.VenueHall)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venueHallManager == null)
            {
                return NotFound();
            }

            return View(venueHallManager);
        }

        // GET: VenueHallManager/Create
        public IActionResult Create()
        {
            var UserId = Helper.getUserId(HttpContext);
            if (UserId == 0)
            {
                return Redirect("/Login");
            }
            ViewBag.UID = UserId;

            ViewData["UserId"] = new SelectList(_context.Users.Select(s => new
            {
                s.Id,
                Name = s.Firstname + " " + s.Lastname
            }), "Id", "Name");

            ViewData["VenueHallId"] = new SelectList(_context.VenueHalls, "Id", "Title");
            return View();
        }

        // POST: VenueHallManager/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,VenueHallId,UserId")] VenueHallManager venueHallManager)
        {
            _context.Add(venueHallManager);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: VenueHallManager/Edit/5
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
            ViewBag.UID = UserId;

            var venueHallManager = await _context.VenueHallManagers.FindAsync(id);
            if (venueHallManager == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", venueHallManager.UserId);
            ViewData["VenueHallId"] = new SelectList(_context.VenueHalls, "Id", "Id", venueHallManager.VenueHallId);
            return View(venueHallManager);
        }

        // POST: VenueHallManager/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,VenueHallId,UserId")] VenueHallManager venueHallManager)
        {
            if (id != venueHallManager.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venueHallManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueHallManagerExists(venueHallManager.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", venueHallManager.UserId);
            ViewData["VenueHallId"] = new SelectList(_context.VenueHalls, "Id", "Id", venueHallManager.VenueHallId);
            return View(venueHallManager);
        }

        // GET: VenueHallManager/Delete/5
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
            ViewBag.UID = UserId;

            var venueHallManager = await _context.VenueHallManagers
                .Include(v => v.User)
                .Include(v => v.VenueHall)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venueHallManager == null)
            {
                return NotFound();
            }

            return View(venueHallManager);
        }

        // POST: VenueHallManager/Delete/5
        [HttpPost, ActionName("Delete")]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venueHallManager = await _context.VenueHallManagers.FindAsync(id);
            if (venueHallManager != null)
            {
                _context.VenueHallManagers.Remove(venueHallManager);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VenueHallManagerExists(int id)
        {
            return _context.VenueHallManagers.Any(e => e.Id == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HeartHotel.Models;
using Monitor = HeartHotel.Models.Monitor;
using Firoozi.Helper;
using HeartHotel.Hubs;

namespace HeartHotel.Controllers
{
    public class MonitorsController : Controller
    {
        private readonly EventisContext _context;
        private readonly SignalRHub _signalRHub;

        public MonitorsController(EventisContext context, SignalRHub signalRHub)
        {
            _context = context;
            _signalRHub = signalRHub;
        }

        // GET: Monitors
        public async Task<IActionResult> Index()
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

            return View(await _context.Monitors.ToListAsync());
        }

        // GET: Monitors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monitor = await _context.Monitors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (monitor == null)
            {
                return NotFound();
            }

            return View(monitor);
        }

        // GET: Monitors/Create
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
            
            return View();
        }

        // POST: Monitors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,Icon,MonitorId")] Monitor monitor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(monitor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(monitor);
        }

        // GET: Monitors/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
            
            if (id == null)
            {
                return NotFound();
            }

            var monitor = await _context.Monitors.FindAsync(id);
            if (monitor == null)
            {
                return NotFound();
            }
            return View(monitor);
        }

        // POST: Monitors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,Icon,MonitorId")] Monitor monitor)
        {
            if (id != monitor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monitor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonitorExists(monitor.Id))
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
            return View(monitor);
        }

        // GET: Monitors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var monitor = await _context.Monitors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (monitor == null)
            {
                return NotFound();
            }

            return View(monitor);
        }

        // POST: Monitors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var monitor = await _context.Monitors.FindAsync(id);
            if (monitor != null)
            {
                _context.Monitors.Remove(monitor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ShowText(int id)
        {
            await _signalRHub.NotifyGroup($"monitor{id}", "Change");
            // return RedirectToAction(nameof(Index));
            return Ok();
        }

        public async Task<IActionResult> ShowScreenSaver(int id)
        {
            await _signalRHub.NotifyGroup($"monitor{id}", "Screen Saver");
            // return RedirectToAction(nameof(Index));
            return Ok();
        }

        private bool MonitorExists(int id)
        {
            return _context.Monitors.Any(e => e.Id == id);
        }
    }
}

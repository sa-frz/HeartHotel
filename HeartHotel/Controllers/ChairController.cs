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
    public class ChairController : Controller
    {
        private readonly EventisContext _context;

        public ChairController(EventisContext context)
        {
            _context = context;
        }

        // GET: ChairController
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

            return View(await _context.Chairs.ToListAsync());
        }

        // GET: ChairController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chair = await _context.Chairs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chair == null)
            {
                return NotFound();
            }

            return View(chair);
        }

        // GET: ChairController/Create
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

        // POST: ChairController/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Post,Cv")] Chair chair, IEnumerable<IFormFile> Image)
        {
            if (ModelState.IsValid)
            {

                if (Image.Count() > 0)
                {
                    var validFile = Image.FirstOrDefault();
                    string fs = DateTime.Now.ToPersianDateTime().Replace("/", "").Replace(" ", "-").Replace(":", "") + "-" + validFile.Length;
                    fs = fs.Replace(" ", "-").Replace("?", "");
                    var strFilename = (fs + Path.GetExtension(validFile.FileName)).ToLower();
                    var dir = Path.Combine(
                          Directory.GetCurrentDirectory(),
                          "wwwroot", "Files", "Chairs");
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    var path = Path.Combine(dir, strFilename);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        validFile?.CopyTo(stream);
                    }

                    chair.Image = strFilename;
                }

                _context.Add(chair);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(chair);
        }

        // GET: ChairController/Edit/5
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

            var chair = await _context.Chairs.FindAsync(id);
            if (chair == null)
            {
                return NotFound();
            }
            return View(chair);
        }

        // POST: ChairController/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Post,Cv")] Chair chair, IEnumerable<IFormFile> Image)
        {
            if (id != chair.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (Image.Count() > 0)
                    {
                        var validFile = Image.FirstOrDefault();
                        string fs = DateTime.Now.ToPersianDateTime().Replace("/", "").Replace(" ", "-").Replace(":", "") + "-" + validFile.Length;
                        fs = fs.Replace(" ", "-").Replace("?", "");
                        var strFilename = (fs + Path.GetExtension(validFile.FileName)).ToLower();
                        var dir = Path.Combine(
                              Directory.GetCurrentDirectory(),
                              "wwwroot", "Files", "Chairs");
                        if (!Directory.Exists(dir))
                        {
                            Directory.CreateDirectory(dir);
                        }

                        var path = Path.Combine(dir, strFilename);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            validFile?.CopyTo(stream);
                        }

                        chair.Image = strFilename;
                    }
                    else
                    {
                        var Chairs = await _context.Chairs.Where(e => e.Id == id).FirstOrDefaultAsync();
                        chair.Image = Chairs.Image;
                    }

                    _context.Update(chair);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChairExists(chair.Id))
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
            return View(chair);
        }

        // GET: ChairController/Delete/5
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

            var chair = await _context.Chairs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chair == null)
            {
                return NotFound();
            }

            return View(chair);
        }

        // POST: ChairController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chair = await _context.Chairs.FindAsync(id);
            if (chair != null)
            {
                _context.Chairs.Remove(chair);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChairExists(int id)
        {
            return _context.Chairs.Any(e => e.Id == id);
        }
    }
}

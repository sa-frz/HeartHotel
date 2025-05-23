using Microsoft.AspNetCore.Mvc;
using HeartHotel.Models;
using Firoozi.Helper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HeartHotel.Controllers;
public class ConductorsController : Controller
{
    private readonly EventisContext _context;
    public ConductorsController(EventisContext context)
    {
        _context = context;
    }

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

        ViewBag.Chairs = new SelectList(await _context.Chairs.ToListAsync(), "Id", "Name");

        return View();
    }

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

        var program = await _context.Programs
            .Include(p => p.ProgramConductors)
            .Include(p => p.ChairsConductors)
            .ThenInclude(t => t.Chair)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (program == null)
        {
            return NotFound();
        }

        ViewBag.Chairs = new SelectList(await _context.Chairs.ToListAsync(), "Id", "Name");

        return View(program);
    }
}
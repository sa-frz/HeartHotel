using Microsoft.AspNetCore.Mvc;
using HeartHotel.Models;
using Firoozi.Helper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HeartHotel.Controllers;
public class ConductorsController : Controller
{
    private readonly EventisContext _context;
    public ConductorsController(EventisContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // var UserId = Helper.getUserId(HttpContext);
        // if (UserId == 0)
        // {
        //     return Redirect("/Login");
        // }
        // ViewBag.UID = UserId;

        return View();
    }

    public async Task<IActionResult> Edit(int? id)
    {
        // var UserId = Helper.getUserId(HttpContext);
        // if (UserId == 0)
        // {
        //     return Redirect("/Login");
        // }
        // ViewBag.UID = UserId;

        if (id == null)
        {
            return NotFound();
        }

        var program = await _context.Programs
            .Include(p => p.ProgramConductors)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (program == null)
        {
            return NotFound();
        }

        return View(program);
    }
}
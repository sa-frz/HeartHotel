using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HeartHotel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Firoozi.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Extensions;
using System.Threading.Tasks;

namespace HeartHotel.Controllers;

public class ScreenController : Controller
{
    private readonly EventisContext _context;
    private readonly ILogger<HomeController> _logger;

    public ScreenController(ILogger<HomeController> logger, EventisContext context)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Show1(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var program = await _context.Programs
            .Include(m => m.ProgramConductors)
            .FirstOrDefaultAsync(x => x.Id == id);

        return View(program);
    }

    public async Task<IActionResult> Show2(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var program = await _context.Programs
            .Include(m => m.ProgramConductors)
            .FirstOrDefaultAsync(x => x.Id == id);

        return View(program);
    }


}

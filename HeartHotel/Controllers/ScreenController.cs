using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HeartHotel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Firoozi.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Extensions;

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

    public IActionResult Show1(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lecture = _context.Lectures
        .Include(m => m.EventsPerson)
        .ThenInclude(m => m.Events)
        .Include(m => m.EventsPerson)
        .ThenInclude(m => m.Person)
        .FirstOrDefault(x => x.Id == id);
        if (lecture == null)
        {
            return NotFound();
        }

        return View(lecture);
    }

    public IActionResult Show2(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var lecture = _context.Lectures
        .Include(m => m.EventsPerson)
        .ThenInclude(m => m.Events)
        .Include(m => m.EventsPerson)
        .ThenInclude(m => m.Person)
        .FirstOrDefault(x => x.Id == id);
        if (lecture == null)
        {
            return NotFound();
        }

        return View(lecture);
    }


}

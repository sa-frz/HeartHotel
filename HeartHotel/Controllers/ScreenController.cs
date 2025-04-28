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

    public async Task<dynamic> Content(int ProgramId, bool showAll = false)
    {
        var program = await _context.Programs
            .Include(m => m.ProgramConductors)
            .Include(m => m.ChairsConductors)
            .ThenInclude(p => p.Chair)
            .FirstOrDefaultAsync(x => x.Id == ProgramId);

        var ProgramConductorsId = 0;
        if (!showAll)
        {
            foreach (var conductor in program.ProgramConductors)
            {
                if (conductor.SaatAz == null || conductor.SaatTa == null)
                {
                    continue;
                }
                var startTime = DateTime.Parse(conductor.SaatAz.ToString());
                var endTime = DateTime.Parse(conductor.SaatTa.ToString());
                var currentTime = DateTime.Now;

                if (currentTime >= startTime && currentTime <= endTime)
                {
                    ProgramConductorsId = conductor.Id;
                    break;
                }
            }
        }

        var ProgramConductors = program.ProgramConductors.Where(x => x.Id >= ProgramConductorsId).OrderBy(x =>
        x.Id).Select(s => new
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            SaatAz = s.SaatAz,
            SaatTa = s.SaatTa
        }).ToList();

        var chairs = program.ChairsConductors.OrderBy(o => o.RoleId).Select(s => new
        {
            Name = s.Chair.Name,
            Image = s.Chair.Image,
            s.RoleId
        });

        var result = new
        {
            ProgramName = program.Name,
            ProgramConductorsId = ProgramConductorsId,
            ProgramConductors = ProgramConductors,
            programChairs = chairs
        };
        return Json(result);
    }

    public async Task<IActionResult> Show(string? date = "1403/08/16")
    {
        var halls = await _context.Programs
            .Include(m => m.VenueHall)
            .Where(x => x.Date == date)
            .Select(s => new
            {
                Id = s.Id,
                VenueHallId = s.VenueHallId,
                Name = s.VenueHall.Title,
                Address = s.VenueHall.Address
            })
            .ToListAsync();

        var gregorianDate = date.ToGregorianDate();
        // var dayOfWeekName = gregorianDate.ToString("dddd", new System.Globalization.CultureInfo("en-US"));
        var dayOfWeekNumber = (int)gregorianDate.DayOfWeek;
        var dayOfWeek = new Dictionary<int, string>
        {
            { 0, "یکشنبه" },
            { 1, "دوشنبه" },
            { 2, "سه‌شنبه" },
            { 3, "چهارشنبه" },
            { 4, "پنج‌شنبه" },
            { 5, "جمعه" },
            { 6, "شنبه" },
        };
        ViewBag.DayOfWeek = dayOfWeek[dayOfWeekNumber];
        // ViewBag.DayOfWeekName = dayOfWeekName;
        ViewBag.date = date;

        var programs = new List<ProgramHallsViewModel>();
        foreach (var hall in halls)
        {
            var result = await Content(hall.Id, true);
            var jsonResult = result as JsonResult;
            var data = jsonResult?.Value as dynamic;
            var program = new ProgramHallsViewModel();
            program.VenueHallName = hall.Name;
            program.VenueHallAddress = hall.Address;
            program.ProgramName = data.ProgramName;
            program.ProgramConductorsId = data.ProgramConductorsId;
            program.ProgramConductors = ((IEnumerable<dynamic>)data.ProgramConductors).Select(c => new ProgramConductorViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                SaatAz = c.SaatAz,
                SaatTa = c.SaatTa
            }).ToList();

            programs.Add(program);
        }
        return View(programs);
    }

    public async Task<IActionResult> Show1(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var result = await Content(id.Value, true);

        ViewBag.ProgramName = result.Value.ProgramName;
        ViewBag.ProgramConductorsId = result.Value.ProgramConductorsId;
        ViewBag.ProgramConductors = result.Value.ProgramConductors;
        ViewBag.ProgramChairs = result.Value.programChairs;

        return View();
    }

    public async Task<IActionResult> Show2(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var result = await Content(id.Value);

        ViewBag.ProgramName = result.Value.ProgramName;
        ViewBag.ProgramConductorsId = result.Value.ProgramConductorsId;
        ViewBag.ProgramConductors = result.Value.ProgramConductors;

        return View();
    }


}

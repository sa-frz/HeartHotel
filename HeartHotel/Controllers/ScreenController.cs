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
            foreach (var conductor in program!.ProgramConductors)
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

        var ProgramConductors = program!.ProgramConductors.Where(x => x.Id >= ProgramConductorsId).OrderBy(x =>
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

    public async Task<IActionResult> Show(string? date = "1404/02/10")
    {
        var halls = await _context.Programs
            .Include(m => m.VenueHall)
            .Where(x => x.Date == date)
            .Select(s => new
            {
                Id = s.Id,
                VenueHallId = s.VenueHallId,
                Name = s.VenueHall.Title,
                Address = s.VenueHall.Address,
                s.ShowDate
            })
            .ToListAsync();

        var gregorianDate = date.ToGregorianDate();
        // var dayOfWeekName = gregorianDate.ToString("dddd", new System.Globalization.CultureInfo("en-US"));
        var dayOfWeekNumber = (int)gregorianDate.DayOfWeek;
        var dayOfWeekFa = new Dictionary<int, string>
        {
            { 0, "یکشنبه" },
            { 1, "دوشنبه" },
            { 2, "سه‌شنبه" },
            { 3, "چهارشنبه" },
            { 4, "پنج‌شنبه" },
            { 5, "جمعه" },
            { 6, "شنبه" },
        };

        var dayOfWeek = new Dictionary<int, string>
        {
            { 0, "Sunday" },
            { 1, "Monday" },
            { 2, "Tuesday" },
            { 3, "Wednesday" },
            { 4, "Thursday" },
            { 5, "Friday" },
            { 6, "Saturday" },
        };
        // ViewBag.DayOfWeek = dayOfWeek[dayOfWeekNumber];
        ViewBag.DayOfWeek = halls.FirstOrDefault()?.ShowDate.Replace(",", "-").Split('-')[0].Trim();
        ViewBag.date = halls.FirstOrDefault()?.ShowDate.Replace(",", "-").Split('-')[1].Trim();
        // ViewBag.date = date;

        // var programs = new List<ProgramHallsViewModel>();
        // foreach (var hall in halls)
        // {
        //     var result = await Content(hall.Id, true);
        //     var jsonResult = result as JsonResult;
        //     var data = jsonResult?.Value as dynamic;
        //     var program = new ProgramHallsViewModel();
        //     program.VenueHallName = hall.Name;
        //     program.VenueHallAddress = hall.Address;
        //     program.ProgramName = data.ProgramName;
        //     program.ProgramConductorsId = data.ProgramConductorsId;
        //     program.ProgramConductors = ((IEnumerable<dynamic>)data.ProgramConductors).Select(c => new ProgramConductorViewModel
        //     {
        //         Id = c.Id,
        //         Name = c.Name,
        //         Description = c.Description,
        //         SaatAz = c.SaatAz,
        //         SaatTa = c.SaatTa
        //     }).ToList();

        //     programs.Add(program);
        // }
        // return View(programs);

        var venueHall = await _context.Programs
            .Include(m => m.VenueHall)
            .Where(x => x.Date == date)
            .Select(s => s.VenueHall).Distinct()
            .ToListAsync();

        ViewBag.currentTime = date;

        return View(venueHall);
    }

    public async Task<IActionResult> Show1(int? id, int? hallId, string date)
    {
        if (id == null && hallId == null)
        {
            return NotFound();
        }

        ViewBag.Auto = false;

        if (id == null)
        {
            var currentDate = date ?? DateTime.Now.ToPersian();
            var currentTime = DateTime.Now.ToString("HH:mm");
            var Query = @$"SELECT TOP 1 Programs.* 
                                    FROM Programs
                                    LEFT OUTER JOIN ProgramConductors ON ProgramConductors.ProgramId = Programs.ID
                                    WHERE [Date] = '{currentDate}' 
                                    AND VenueHallId = {hallId} 
                                    AND CAST(SaatAz AS TIME) <= CAST('{currentTime}' AS TIME)
                                    AND CAST(SaatTa AS TIME) >= CAST('{currentTime}' AS TIME)";
            var program = await _context.Programs.FromSqlRaw(Query).FirstOrDefaultAsync();

            ViewBag.VenueHallId = hallId;
            ViewBag.Date = date;
            ViewBag.Auto = true;

            // var qq = await AllDayPrograms(hallId.Value, date);
            // var jj = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(qq);

            if (program == null)
            {
                ViewBag.ProgramName = "";
                ViewBag.ProgramConductorsId = 0;
                ViewBag.ProgramConductors = new List<dynamic>();
                ViewBag.ProgramChairs = new List<dynamic>();
                ViewBag.Id = 0;

                ViewBag.HallName = await _context.VenueHalls
                    .Where(x => x.Id == hallId!.Value)
                    .Select(s => s.Title)
                    .FirstOrDefaultAsync();
                return View();
            }
            id = program.Id;
        }

        var result = await Content(id.Value, true);

        ViewBag.ProgramName = result.Value.ProgramName;
        ViewBag.ProgramConductorsId = result.Value.ProgramConductorsId;
        ViewBag.ProgramConductors = result.Value.ProgramConductors;
        ViewBag.ProgramChairs = result.Value.programChairs;
        ViewBag.Id = id.Value;

        ViewBag.HallName = await _context.Programs
            .Include(v => v.VenueHall)
            .Where(x => x.Id == id.Value)
            .Select(s => s.VenueHall.Title)
            .FirstOrDefaultAsync();

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
        ViewBag.Id = id.Value;

        return View();
    }

    public async Task<IActionResult> Show3(int? id, int? hallId, string date)
    {
        if (id == null && hallId == null)
        {
            return NotFound();
        }

        if (id == null)
        {
            ViewBag.VenueHallId = hallId;
            ViewBag.Date = date;
            ViewBag.Auto = true;

            ViewBag.HallName = await _context.VenueHalls
                .Where(x => x.Id == hallId!.Value)
                .Select(s => s.Title)
                .FirstOrDefaultAsync();
            return View();
        }
        else
        {
            ViewBag.Id = id.Value;
            ViewBag.Auto = false;

            ViewBag.HallName = await _context.Programs
                .Include(v => v.VenueHall)
                .Where(x => x.Id == id.Value)
                .Select(s => s.VenueHall.Title)
                .FirstOrDefaultAsync();
        }

        return View();
    }

    [Route("/Monitors")]
    public IActionResult Monitors()
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

    [Route("/Monitor1")]
    public IActionResult Monitor1(string Content)
    {
        try
        {
            ViewBag.Content = Content.Split('_')[0];
            ViewBag.Icon = Content.Split('_')[1];
        }
        catch
        {
        }

        return View();
    }

    [Route("/Monitor2")]
    public IActionResult Monitor2(string Content)
    {
        try
        {
            ViewBag.Content = Content.Split('_')[0];
            ViewBag.Icon = Content.Split('_')[1];
        }
        catch
        {
        }

        return View();
    }

    [Route("/Monitor3")]
    public IActionResult Monitor3(string Content)
    {
        try
        {
            ViewBag.Content = Content.Split('_')[0];
            ViewBag.Icon = Content.Split('_')[1];
        }
        catch
        {
        }

        return View();
    }

    [Route("/Monitor4")]
    public IActionResult Monitor4(string Content)
    {
        try
        {
            ViewBag.Content = Content.Split('_')[0];
            ViewBag.Icon = Content.Split('_')[1];
        }
        catch
        {
        }

        return View();
    }

    [Route("/Monitor5")]
    public IActionResult Monitor5(string Content)
    {
        try
        {
            ViewBag.Content = Content.Split('_')[0];
            ViewBag.Icon = Content.Split('_')[1];
        }
        catch
        {
        }

        return View();
    }

    public IActionResult Slideshow(int Id)
    {
        ViewBag.Id = Id;

        var imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Slideshow");
        if (Directory.Exists(imageFolderPath))
        {
            var imageFiles = Directory.GetFiles(imageFolderPath)
                .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                               file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                               file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                               file.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                .Select(file => Path.GetFileName(file))
                .ToList();

            ViewBag.Images = imageFiles;
        }
        else
        {
            ViewBag.Images = new List<string>();
        }
        return View();
    }

}

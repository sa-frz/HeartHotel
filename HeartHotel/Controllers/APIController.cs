
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HeartHotel.Models;
using Microsoft.EntityFrameworkCore;
using Firoozi.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using HeartHotel.Hubs;

namespace HeartHotel.Controllers;

public class APIController : Controller
{
    private readonly EventisContext _context;
    private readonly SignalRHub _signalRHub;
    // private readonly IHubContext<NotificationHub> _hubContext;

    public APIController(EventisContext context, SignalRHub signalRHub)
    {
        _context = context;
        _signalRHub = signalRHub;
    }

    [Route("/api/sms/send")]
    [HttpPost]
    public async Task<IActionResult> smsSend(string Mobile)
    {
        try
        {
            var login = await _context.Users.Where(w => w.Mobile == Mobile && w.Id < 11).FirstOrDefaultAsync();
            if (login == null)
            {
                return NotFound();
            }

            var delOTP = await _context.Otps.Where(w => w.Mobile == Mobile).ToListAsync();
            _context.RemoveRange(delOTP);
            _context.SaveChanges();

            int code = Helper.GenerateCode();

            Otp otp = new Otp();
            otp.Code = code;
            otp.Mobile = Mobile;
            _context.Add(otp);
            await _context.SaveChangesAsync();

            Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi("686C384161674F6F396D347279346D346B48307A413341337841676D6631485A715A7867765149507035513D");
            var result = await api.VerifyLookup(Mobile, code.ToString(), "EventisOTP");

            var user = new User()
            {
                Mobile = Mobile
            };
            _context.Add(user);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("/api/sms/code")]
    [HttpPost]
    public async Task<IActionResult> CheckCode(string Mobile, int Code)
    {
        var otp = await _context.Otps.Where(w => w.Mobile == Mobile && w.Code == Code).FirstOrDefaultAsync();
        if (otp != null)
        {
            var login = await _context.Users.Where(w => w.Mobile == Mobile).FirstOrDefaultAsync();

            if (login == null)
            {
                return NotFound();
            }
            else
            {
                HttpContext.Session.SetString("userid", login.Id.ToString());
            }
            return Ok();
        }

        return NotFound();
    }

    [Route("/api/event/{id?}")]
    public PartialViewResult ShowDetails(int? id, bool admin = false, bool preview = false)
    {
        bool nf = false;
        Event majles;

        if (admin)
        {
            majles = _context.Events
                .Include(t => t.Times)
                .Include(m => m.Organizer)
                .Include(p => p.EventsPeople)
                .ThenInclude(i => i.Person)
                .Include(m => m.RegisterUserNavigation).FirstOrDefault(m => m.Id == id);
        }
        else
        {
            if (!preview)
            {
                majles = _context.Events
                .Include(t => t.Times)
                .Include(m => m.Organizer)
                .Include(p => p.EventsPeople)
                .ThenInclude(i => i.Person)
                .Include(m => m.RegisterUserNavigation).FirstOrDefault(m => m.Id == id && m.IsOk > 0);
            }
            else
            {
                majles = _context.Events
                .Include(t => t.Times)
                .Include(m => m.Organizer)
                .Include(p => p.EventsPeople)
                .ThenInclude(i => i.Person)
                .Include(m => m.RegisterUserNavigation).FirstOrDefault(m => m.Id == id && m.IsOk == null);
            }
        }

        ViewBag.Catalog = _context.Catalogs.Where(w => w.TypeId == 1).Select(s => s.Value).ToArray();
        ViewBag.CatalogID = _context.Catalogs.Where(w => w.TypeId == 1).Select(s => s.Id).ToArray();
        ViewBag.UID = "0";
        int UserId = 0;
        try
        {
            ViewBag.UID = HttpContext.Session.GetString("userid");
            UserId = Convert.ToInt32(HttpContext.Session.GetString("userid"));
        }
        catch { }

        if (majles == null)
        {
            nf = true;
        }

        //try
        //{
        //    if (!admin && !preview)
        //    {
        //        DateTime d = DateTime.Now.AddDays(-8);
        //        bool error = true;
        //        foreach (var item in majles.Times.OrderBy(o => o.Rooz))
        //        {
        //            DateTime rooz = item.Rooz.ToGregorianDate();
        //            if (rooz > d)
        //            {
        //                error = false;
        //                break;
        //            }
        //        }

        //        nf = error;
        //    }
        //}
        //catch { }

        ViewBag.Lat = "0.0000000000000000";
        ViewBag.Lon = "0.0000000000000000";

        try
        {
            ViewBag.Lat = majles.Lat.ToString().Replace("/", ".");
            ViewBag.Lon = majles.Lon.ToString().Replace("/", ".");
        }
        catch { }

        ViewBag.Err = nf.ToString();

        return PartialView(majles);
    }

    [Route("/api/event/presenters/{id?}")]
    public async Task<PartialViewResult> Presenters(int? id)
    {
        var presenters = await _context.EventsPeople
            .Include(m => m.Person)
            .Where(w => w.EventsId == id).ToListAsync();

        ViewBag.venueHalls = await _context.VenueHalls
                    .Include(v => v.Venue)
                    .Select(s => new SelectListItem()
                    {
                        Value = s.Id.ToString(),
                        Text = s.Venue.Title + " - " + s.Title
                    })
                    .ToListAsync();

        return PartialView(presenters);
    }

    [Route("/api/event/presenter/details/{id?}")]
    public async Task<PartialViewResult> PresenterDetails(int? id)
    {
        var presenters = await _context.EventsPeople
            .Include(m => m.Person)
            .Where(w => w.EventsId == id).ToListAsync();

        return PartialView(presenters);
    }

    [Route("/api/event/presenter/lecture/create")]
    [HttpPost]
    public async Task<IActionResult> CreateLecture(int eventsPersonId, int timesId, string saatAz, string saatTa, string text, int venueHallId)
    {
        try
        {
            var lecture = new Lecture()
            {
                EventsPersonId = eventsPersonId,
                TimesId = timesId,
                SaatAz = saatAz,
                SaatTa = saatTa,
                Text = text,
                VenueHallId = venueHallId
            };
            _context.Add(lecture);
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("/api/event/presenter/lecture/list")]
    public async Task<PartialViewResult> Lectures(int eventsPersonId)
    {
        try
        {
            ViewBag.Lecture = await _context.Lectures
                .Include(m => m.EventsPerson)
                .Include(m => m.Times)
                .Include(m => m.VenueHall)
                .ThenInclude(m => m.Venue)
                .Where(w => w.EventsPersonId == eventsPersonId)
                .Select(s => new
                {
                    Person = s.EventsPerson.Person.Name,
                    VenueHall = s.VenueHall.Title,
                    Venue = s.VenueHall.Venue.Title,
                    Rooz = s.Times.Rooz,
                    Roozehafte = s.Times.Roozehafte,
                    Text = s.Text,
                    SaatAz = s.SaatAz,
                    SaatTa = s.SaatTa,
                }).ToListAsync();

            return PartialView();
        }
        catch
        {
            return PartialView();
        }
    }

    [Route("/api/event/days/{id?}")]
    public async Task<IActionResult> eventDays(int? id)
    {
        var eventDays = await _context.Times
            .Where(w => w.EventsId == id).ToListAsync();

        return Json(eventDays);
    }

    [Route("/api/program/hall/{VenueHall}")]
    public async Task<PartialViewResult> Program(int VenueHall, string Date)
    {
        var program = await _context.Lectures
            .Include(m => m.EventsPerson)
            .ThenInclude(m => m.Person)
            .Include(m => m.EventsPerson)
            .ThenInclude(m => m.Events)
            .Include(m => m.Times)
            .Include(m => m.VenueHall)
            .Where(w => w.VenueHallId == VenueHall && w.Times.Rooz == Date).ToListAsync();

        return PartialView(program);
    }

    [Route("/api/program/halls")]
    public async Task<PartialViewResult> Halls(string Date)
    {
        var lecture = await _context.Lectures
        .Include(e => e.Times)
        .Select(s => new
        {
            // VenueHallId = s.VenueHallId,
            TimesId = s.TimesId,
            Rooz = s.Times.Rooz,
            Roozehafte = s.Times.Roozehafte
        })
        .Where(w => w.Rooz == Date)
        .Distinct()
       .ToListAsync();
        ViewBag.lecture = lecture;

        var halls = await _context.VenueHalls.ToListAsync();
        ViewBag.venueHalls = halls;

        return PartialView();
    }

    [Route("/api/program/create")]
    [HttpPost]
    public async Task<IActionResult> CreateProgram([FromBody] ProgramViewModel model)
    {
        try
        {
            var programData = new List<ProgramConductor>();
            foreach (var item in model.ProgramData)
            {
                var conductor = new ProgramConductor()
                {
                    Name = item.Name,
                    Description = item.Description,
                    SaatAz = item.Time.Split("-")[0].Trim(),
                    SaatTa = item.Time.Split("-")[1].Trim(),
                };
                programData.Add(conductor);
            }

            var program = new Programs()
            {
                Date = model.Date,
                ShowDate = model.ShowDate,
                Name = model.ProgramName,
                VenueHallId = model.VenueHallId.Value,
                ThemeId = model.ThemeId,
                ProgramConductors = programData
            };
            _context.Add(program);
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [Route("/api/program/edit")]
    [HttpPost]
    public async Task<IActionResult> EditProgram([FromBody] ProgramViewModel model)
    {
        try
        {
            var program = await _context.Programs
                .FirstOrDefaultAsync(m => m.Id == model.ProgramId);
            program.ShowDate = model.ShowDate;
            program.Name = model.ProgramName;
            program.ThemeId = model.ThemeId;

            _context.Update(program);
            await _context.SaveChangesAsync();

            var programData = await _context.ProgramConductors
                            .Where(w => w.ProgramId == model.ProgramId).ToListAsync();
            _context.ProgramConductors.RemoveRange(programData);
            await _context.SaveChangesAsync();

            var programConductors = new List<ProgramConductor>();
            foreach (var item in model.ProgramData)
            {
                var conductor = new ProgramConductor()
                {
                    Name = item.Name,
                    Description = item.Description,
                    SaatAz = item.Time.Split("-")[0].Trim(),
                    SaatTa = item.Time.Split("-")[1].Trim(),
                    ProgramId = model.ProgramId.Value
                };
                programConductors.Add(conductor);
            }
            _context.AddRange(programConductors);
            await _context.SaveChangesAsync();

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Route("/api/program/get")]
    [HttpPost]
    public async Task<PartialViewResult> GetProgram([FromBody] GetProgram model)
    {
        try
        {
            var program = await _context.Programs
                .Include(m => m.ProgramConductors)
                .Where(w => w.Date == model.Date && w.VenueHallId == model.VenueHallId).ToListAsync();

            return PartialView(program);
        }
        catch (Exception ex)
        {
            return PartialView(ex.Message);
        }
    }

    [Route("/api/send/notification")]
    // [HttpPost] 
    public async Task<IActionResult> SendNotification()
    {
        await _signalRHub.NotifyAllClients("ReceiveNotification");
        return Ok();
    }

    [Route("/api/send/notification/{id}")]
    // [HttpPost] 
    public async Task<IActionResult> SendNotification(string id)
    {
        await _signalRHub.NotifyClient(id, "برای شما یک رویداد جدید ثبت شده است.");
        return Ok();
    }

}

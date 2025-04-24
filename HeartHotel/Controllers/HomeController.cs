using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HeartHotel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Firoozi.Helper;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList.Extensions;

namespace HeartHotel.Controllers;

public class HomeController : Controller
{
    private readonly EventisContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, EventisContext context)
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Route("/Events")]
    public IActionResult Events(int? page = 1, string k = "", int pageSize = 25)
    {
        var UserId = Helper.getUserId(HttpContext);
        if (UserId == 0)
        {
            // return RedirectToAction(nameof(Index));
        }
        ViewBag.UID = UserId;

        var Events = ShowEvent(-1, 0, 0, "", k, "", false, "");
        ViewBag.srch = k;
        ViewBag.page = page;

        return View(Events.ToPagedList(page ?? 1, pageSize));
    }

    [Route("/Halls")]
    public IActionResult Halls()
    {
        // ViewBag.venueHalls = await _context.VenueHalls
        //             .Include(v => v.Venue)
        //             .Select(s => new
        //             {
        //                 Id = s.Id.ToString(),
        //                 VenueTitle = s.Venue.Title,
        //                 VenueHall = s.Title
        //             })
        //             .ToListAsync();

        

        return View();
    }

    public IOrderedQueryable<Event> ShowEvent(int id, int admin, int live, string pid, string srch, string asearch, bool offline, string ab)
    {
        ViewBag.Catalog = _context.Catalogs.Where(w => w.TypeId == 1).Select(s => s.Value).ToArray();
        ViewBag.CatalogID = _context.Catalogs.Where(w => w.TypeId == 1).Select(s => s.Id).ToArray();

        int UserId = 0;
        try
        {
            UserId = Convert.ToInt32(HttpContext.Session.GetString("userid"));
        }
        catch { }

        string isOK = "(Events.isOK = 1)";
        if (UserId > 0 && UserId <= 10)
        {
            isOK = "(Events.isOK >= 0)";
        }

        if (admin == 1)
        {
            isOK = "(Events.isOK = 0)";
        }

        string isLive = "";
        if (live == 1)
        {
            isLive = " AND (Events.Link1 <> '')";
        }

        string mids = "";
        if (!string.IsNullOrWhiteSpace(pid))
        {
            int personid = Convert.ToInt32(pid);
            var EventsId = _context.EventsPeople.Where(w => w.PersonId == personid).Select(s => s.EventsId).ToArray();
            if (EventsId.Count() > 0)
            {
                mids = " AND (Events.ID IN (" + string.Join(",", EventsId) + ")) ";
            }
        }

        string Query = "";
        Query += "SELECT DISTINCT ";
        Query += "Events.ID, Events.MainText, Events.Address, Events.Link1, Events.Link2, Events.Link3, Events.Link4, Events.Link5, Events.AghayanBanovan, Events.ZirNevis, Events.Kondactor, Events.ImageAdr, Events.RegisterDate, Events.UpdateDate, Events.isOK, Events.OKedUser, Events.OKedDate, Events.TelegramMsgID, Events.RegisterUser, Events.Lat, Events.Lon, Events.UpdateUser, Events.OrganizerID, Events.Ads, Events.Description, Events.SendtoTelegram, Events.Global, Events.Title, Events.CatalogID, ";
        Query += "case when (Rooz + SaatAz <= dbo.CurrentDateTime()) AND (dbo.CurrentDateTime() <= RoozTa + SaatTa) then 'T' else 'F' end as RegisterIP, Video, TypeID, PublishDateTime, SubjectsId ";
        Query += "FROM Events LEFT OUTER JOIN EventsPerson ON Events.ID = EventsPerson.EventsId LEFT OUTER JOIN Person ON Person.ID = EventsPerson.PersonID ";
        Query += "LEFT OUTER JOIN Times ON Events.ID = Times.EventsId ";

        if (id == -1)
        {
            Query += "WHERE {0}{2}{1}";
        }
        else if (id == 1)
        {
            //در حال برگزاری
            Query += "WHERE {0}{2} AND ((SELECT  COUNT(*) AS Expr1 FROM Times WHERE (EventsId = Events.ID) AND (Rooz + SaatAz <= dbo.CurrentDateTime()) AND (dbo.CurrentDateTime() <= RoozTa + SaatTa)) > 0){1}";
        }
        else
        {
            Query += "WHERE {0}{2} AND ((SELECT COUNT(*) AS Expr1 FROM Times WHERE (EventsId = Events.ID) AND ((RoozTa > dbo.CalculatePersianDate(GETDATE())) OR ((RoozTa = dbo.CalculatePersianDate(GETDATE())) AND (SaatTa >= LEFT(CONVERT(TIME, GETDATE()), 5))))) > 0){1}";
        }

        Query = string.Format(Query, isOK, mids, isLive);
        if (!string.IsNullOrEmpty(srch))
        {
            srch = srch.ToStandardChars().ToEnglishNumber();

            //bool offline = false;
            Query += " AND (";
            if (string.IsNullOrEmpty(asearch))
            {
                Query += "Person.Name LIKE N'%" + srch + "%'";
                Query += " OR Events.MainText LIKE N'%" + srch + "%'";
                Query += " OR Events.Address LIKE N'%" + srch + "%'";
                Query += " OR Events.Kondactor LIKE N'%" + srch + "%'";
                Query += " OR Events.ZirNevis LIKE N'%" + srch + "%'";
            }
            else
            {
                var flt = asearch.Substring(0, asearch.Length - 1).Split(",");
                List<string> items = new List<string>();
                foreach (string item in flt)
                {
                    if (item == "City")
                    {
                        items.Add("Events.Address LIKE N'" + srch + "%'");
                    }
                    else
                    {
                        if (item == "offline")
                        {
                            //offline = true;
                        }
                        else
                        {
                            items.Add(item + " LIKE N'%" + srch + "%'");
                        }
                    }
                }
                Query += string.Join(" OR ", items.ToArray());
            }
            Query += ")";

            //if (offline)
            //{
            //    Query += " AND (Events.Address <> '' OR Events.Address <> NULL)";
            //}
        }
        else
        {
            //if (asearch != null) {
            //    if (asearch.IndexOf("offline") > -1)
            //    {
            //        Query += " AND (Events.Address <> '' OR Events.Address <> NULL)";
            //    }
            //}
        }

        if (offline)
        {
            Query += " AND (Events.Address <> '' OR Events.Address <> NULL)";
        }

        switch (ab)
        {
            case "option1":
                Query += " AND (Events.AghayanBanovan = 0)";
                break;

            case "option2":
                Query += " AND (Events.AghayanBanovan = 1)";
                break;

            case "option3":
                Query += " AND (Events.AghayanBanovan = 2)";
                break;
        }

        var M = _context.Events.FromSqlRaw(Query)
                                .Include(t => t.EventsDetails)
                                .Include(t => t.Times)
                                .Include(t => t.Organizer)
                                .Include(t => t.Subjects)
                                .Include(p => p.EventsPeople)
                                .ThenInclude(i => i.Person);
        //.OrderByDescending(o => o.Id);

        if (id != -1)
        {
            Query = SortedbyDatetime(M);
        }
        IOrderedQueryable Events;
        try
        {
            Events = _context.Events.FromSqlRaw(Query)
                                      .Include(t => t.EventsDetails)
                                      .Include(t => t.Times)
                                      .Include(t => t.Organizer)
                                      .Include(t => t.Subjects)
                                      .Include(p => p.EventsPeople)
                                      .ThenInclude(i => i.Person)
                                      .OrderByDescending(o => o.RegisterDate);//.ThenByDescending(o => o.Id);
        }
        catch (Exception)
        {
            Events = M.OrderByDescending(o => o.RegisterDate);
        }
        return ((IOrderedQueryable<Event>)Events);
    }

    public string SortedbyDatetime(IIncludableQueryable<Event, Person?> Events)
    {
        string Query;
        DateTime current = DateTime.Now;
        List<string> Q = new List<string>();
        foreach (var item in Events)
        {
            string countdown = DateTime.Now.ToGregorianDateTimeString();
            foreach (var t in item.Times.OrderBy(o => o.Rooz).ThenBy(o => o.SaatAz))
            {
                DateTime crec = (t.Rooz + " " + t.SaatAz + ":00").ToGregorianDateTime();
                DateTime crecta = (t.RoozTa + " " + t.SaatTa + ":00").ToGregorianDateTime();
                if (crecta > current && crec <= current)
                {
                    countdown = DateTime.Now.ToGregorianDateTimeString(true);
                    break;
                }
                else if (crec > current)
                {
                    countdown = crec.ToGregorianDateTimeString(true);
                    break;
                }
            }
            Query = "";
            Query += "SELECT '" + item.RegisterIp;
            Query += "' AS RegisterIp, CAST('" + countdown + "' AS datetime) AS RegisterDate, Events.ID, Events.MainText, Events.Address, Events.Link1, Events.Link2, Events.Link3, Events.Link4, Events.Link5, Events.AghayanBanovan, Events.ZirNevis, Events.Kondactor, Events.ImageAdr, Events.OKedDate, Events.UpdateDate, Events.isOK, Events.OKedUser, Events.TelegramMsgID, Events.RegisterUser, Events.Lat, Events.Lon, Events.UpdateUser, Events.OrganizerID, Events.Ads, Events.Video, Events.TypeID, Events.PublishDateTime, Events.Description, Events.SendtoTelegram, Events.Global, Events.Title, Events.CatalogID, Events.SubjectsId ";
            Query += "\nFROM Events ";
            Query += "\nWHERE (ID = " + item.Id + ") ";
            Q.Add(Query);
        }

        Query = string.Join("\nUNION\n", Q);
        return Query;
    }

    // GET: Event/Details/5
    [Route("/E/{id?}")]
    public async Task<IActionResult> DetailsOLD(int? id)
    {
        if (id == null || _context.Events == null)
        {
            return NotFound();
        }

        var eventi = await _context.Events
            //.Include(e => e.Organizer)
            //.Include(e => e.RegisterUserNavigation)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (eventi == null)
        {
            return NotFound();
        }

        //ViewBag.Title = eventi.Title;
        return RedirectToAction(nameof(Details), new { id = id, title = eventi.Title.Replace(" ", "-") });
    }

    //[Route("/Event/{title}/{id?}")]
    [Route("/E/{id?}/{title}")]
    public async Task<IActionResult> Details(int? id, string title)
    {
        if (id == null || _context.Events == null)
        {
            return NotFound();
        }

        var eventi = await _context.Events
            //.Include(e => e.Organizer)
            //.Include(e => e.RegisterUserNavigation)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (eventi == null)
        {
            return NotFound();
        }

        ViewBag.img = (eventi.ImageAdr != null && eventi.ImageAdr.Trim() != "-") ? eventi.ImageAdr : null;
        ViewBag.Title = eventi.Title;
        ViewBag.ID = id;

        int UserId = 0;
        try
        {
            UserId = Convert.ToInt32(HttpContext.Session.GetString("userid"));
        }
        catch { }

        ViewBag.UID = UserId;

        return View();
    }

    [Route("/Presenters/{id?}")]
    public async Task<IActionResult> Presenters(int? id)
    {
        if (id == null || _context.Events == null)
        {
            return NotFound();
        }

        var eventi = await _context.Events
            .FirstOrDefaultAsync(m => m.Id == id);
        if (eventi == null)
        {
            return NotFound();
        }

        ViewBag.img = (eventi.ImageAdr != null && eventi.ImageAdr.Trim() != "-") ? eventi.ImageAdr : null;
        ViewBag.Title = eventi.Title;
        ViewBag.ID = id;

        int UserId = 0;
        try
        {
            UserId = Convert.ToInt32(HttpContext.Session.GetString("userid"));
        }
        catch { }

        ViewBag.UID = UserId;

        return View();
    }












    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

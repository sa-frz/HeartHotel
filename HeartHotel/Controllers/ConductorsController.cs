using Microsoft.AspNetCore.Mvc;
using HeartHotel.Models;
using Firoozi.Helper;

namespace HeartHotel.Controllers;
public class ConductorsController : Controller
{
    private readonly EventisContext _context;
    public ConductorsController( EventisContext context)
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
}
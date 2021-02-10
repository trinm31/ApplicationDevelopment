using App_Dev.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App_Dev.Areas.Authenticated.Controllers
{
    [Area("Authenticated")]
    [Authorize]
    public class HomeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            if (User.IsInRole(SD.Role_Admin))
            {
                return RedirectToAction("Index", "Users", new { area = "Authenticated" });
            }
            if (User.IsInRole(SD.Role_Staff))
            {
                return RedirectToAction("Index", "Overview", new { area = "Authenticated" });
            }
            if (User.IsInRole(SD.Role_Trainee))
            {
                return RedirectToAction("Index", "Trainee", new { area = "Authenticated" });
            }
            if (User.IsInRole(SD.Role_Trainer))
            {
                return RedirectToAction("Index", "Trainer", new { area = "Authenticated" });
            }
            return View();
        }
    }
}
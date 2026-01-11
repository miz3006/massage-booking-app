using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MassageStudio.Models;
using System;
using System.Threading.Tasks;

namespace MasazeBooking.Controllers
{
    [Authorize]
    public class MyProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public MyProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            return View(user);
        }

        // ADMIN MODE ON
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult EnableAdminMode()
        {
            Response.Cookies.Append(
                "admin_mode",
                "1",
                new CookieOptions
                {
                    Path = "/",                 // ✅ pomembno
                    HttpOnly = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.Lax,
                    IsEssential = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

            return RedirectToAction("Rezervacije", "Admin");

        }

        // ADMIN MODE OFF
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DisableAdminMode()
        {
            Response.Cookies.Delete(
                "admin_mode",
                new CookieOptions
                {
                    Path = "/"                  // ✅ mora bit isto kot Append
                });

            return RedirectToAction("", "Home");

        }

    }
}

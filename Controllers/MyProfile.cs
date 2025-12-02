using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using MassageStudio.Models;
using System.Threading.Tasks;

namespace MasazeBooking.Controllers
{
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
            {
                return RedirectToAction("Login", "Account");
            }

            return View(user);
        }
    }
}

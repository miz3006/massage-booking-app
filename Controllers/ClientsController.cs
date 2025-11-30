using MassageStudio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MassageStudio.Controllers
{
    
    public class ClientsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ClientsController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: /Clients/Index
        public async Task<IActionResult> Index()
        {
            // vsi uporabniki iz AspNetUsers
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }
    }
}

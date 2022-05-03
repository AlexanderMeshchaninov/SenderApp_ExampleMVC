using System;
using System.Threading.Tasks;
using Core.AppIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SenderApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<CustomIdentityUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            UserManager<CustomIdentityUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }
        
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"HomeController start - {DateTime.Now}");
            var result = await _userManager.Users.ToListAsync();
            
            _logger.LogInformation($"HomeController stop - {DateTime.Now}");
            return View(result);
        }
    }
}
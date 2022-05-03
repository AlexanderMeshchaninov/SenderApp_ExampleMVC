using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.AppIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SenderApp.CommonLogic.BussinessLogic;

namespace SenderApp.Controllers
{
    [Authorize(Roles = "sa, admin")]
    public class RolesController : Controller
    {
        private readonly ILogger<RolesController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly IBussinessLayer _bussinessLayer;

        public RolesController(
            ILogger<RolesController> logger,
            RoleManager<IdentityRole> roleManager,
            UserManager<CustomIdentityUser> userManager,
            IBussinessLayer bussinessLayer)
        {
            _logger = logger;
            _roleManager = roleManager;
            _userManager = userManager;
            _bussinessLayer = bussinessLayer;
        }
        
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"RolesController start - {DateTime.Now}");
            var result = await _roleManager.Roles.ToListAsync();
            
            _logger.LogInformation($"RolesController stop - {DateTime.Now}");
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            _logger.LogInformation($"Create roles start - {DateTime.Now}");
            var actionName = ControllerContext.ActionDescriptor.ActionName;
            
            if (!string.IsNullOrEmpty(name))
            {
                var responseFromFacade = await _bussinessLayer.RolesFacadeAsync(actionName, name);
                
                if (responseFromFacade.Succeeded)
                {
                    _logger.LogInformation($"Create roles stop with success - {DateTime.Now}");
                    return RedirectToAction("Index");
                }

                foreach (var error in responseFromFacade.Errors)
                {
                    _logger.LogError($"Create roles stop with {error} - {DateTime.Now}");
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            _logger.LogInformation($"Create roles stop - {DateTime.Now}");
            return View(name);
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation($"Delete roles start - {DateTime.Now}");
            var actionName = ControllerContext.ActionDescriptor.ActionName;
            
            if (!string.IsNullOrEmpty(id))
            {
                var responseFromFacade = await _bussinessLayer.RolesFacadeAsync(actionName, id);

                if (responseFromFacade.Succeeded)
                {
                    _logger.LogInformation($"Delete roles stop with success - {DateTime.Now}");
                    return RedirectToAction("Index");
                }
                
                foreach (var error in responseFromFacade.Errors)
                {
                    _logger.LogError($"Delete roles stop with {error} - {DateTime.Now}");
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            _logger.LogInformation($"Delete roles stop - {DateTime.Now}");
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> UserList()
        {
            _logger.LogInformation($"UserList start - {DateTime.Now}");
            var result = await _userManager.Users.ToListAsync();
            
            _logger.LogInformation($"UserList stop with success - {DateTime.Now}");
            return View(result);
        }
        
        public async Task<IActionResult> Edit(string userId)
        {
            _logger.LogInformation($"Edit roles start - {DateTime.Now}");
            
            if (!string.IsNullOrEmpty(userId))
            {
                var responseFromFacade = await _bussinessLayer.RolesReadFacade(userId);

                if (responseFromFacade != null)
                {
                    _logger.LogInformation($"Delete roles stop with success - {DateTime.Now}");
                    return View(responseFromFacade);
                }
            }

            _logger.LogInformation($"Edit roles stop - {DateTime.Now}");
            return NotFound();
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            _logger.LogInformation($"Edit roles start - {DateTime.Now}");
            var actionName = ControllerContext.ActionDescriptor.ActionName;
            
            if (!string.IsNullOrEmpty(userId) && roles != null)
            {
                var responseFromFacade = await _bussinessLayer.RolesFacadeAsync(actionName, userId, roles);
                
                if (responseFromFacade.Succeeded)
                {
                    _logger.LogInformation($"Edit roles stop with success - {DateTime.Now}");
                    return RedirectToAction("UserList");
                }
                
                foreach (var error in responseFromFacade.Errors)
                {
                    _logger.LogError($"Edit roles stop with {error} - {DateTime.Now}");
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            _logger.LogInformation($"Edit roles stop - {DateTime.Now}");
            return NotFound();
        }
    }
}
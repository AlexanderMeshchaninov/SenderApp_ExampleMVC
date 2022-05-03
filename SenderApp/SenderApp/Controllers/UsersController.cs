using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Core.AppIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SenderApp.CommonLogic.BussinessLogic;
using SenderApp.Models.DTO.Requests;
using SenderApp.Models.ViewModels;
using SenderApp.Models.ViewModels.ChangeUserPassword;
using SenderApp.Models.ViewModels.EditUserInfo;

namespace SenderApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly ILogger<UsersController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IBussinessLayer _bussinessLayer;
        private readonly IMapper _mapper;

        public UsersController(
            UserManager<CustomIdentityUser> userManager,
            ILogger<UsersController> logger,
            IHttpContextAccessor contextAccessor,
            IBussinessLayer bussinessLayer,
            IMapper mapper)
        {
            _userManager = userManager;
            _logger = logger;
            _contextAccessor = contextAccessor;
            _bussinessLayer = bussinessLayer;
            _mapper = mapper;
        }
        
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"UsersController start - {DateTime.Now}");
            var listOfUsers = await _userManager.Users.ToListAsync();
            
            var currentUserRole = _contextAccessor.HttpContext?.User
                .FindFirst(ClaimTypes.Role)?.Value;
            
            var currentUserName = _contextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.Name)?.Value;

            if (!string.IsNullOrEmpty(currentUserName))
            {
                switch (currentUserRole)
                {
                    case "admin" :
                        _logger.LogInformation($"UsersController stop - {DateTime.Now}");
                        return View(listOfUsers);
                
                    case "user" :
                        var userResult = listOfUsers
                            .Where(x => x.Email.Contains(currentUserName));
                        
                        _logger.LogInformation($"UsersController stop - {DateTime.Now}");
                        return View(userResult);
                    
                    default:
                        return NoContent();
                }
            }
            
            _logger.LogInformation($"UsersController stop - {DateTime.Now}");
            return View(listOfUsers);
        }

        [Authorize(Roles = "sa,admin")]
        public IActionResult Create()
        {
            return View();
        }
        
        [Authorize(Roles = "sa,admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel viewModel)
        {
            _logger.LogInformation($"Create user start - {DateTime.Now}");
            var actionName = ControllerContext.ActionDescriptor.ActionName;
            
            if (ModelState.IsValid)
            {
                var requestToFacade = _mapper.Map<RequestFacadeDto>(viewModel);
                var responseFromFacade = await _bussinessLayer.RegisterFacadeAsync(actionName, requestToFacade);
                
                if (responseFromFacade != null)
                {
                    _logger.LogInformation($"Create user stop with success - {DateTime.Now}");
                    return RedirectToAction("Index");
                }

                _logger.LogError($"Create user stop with no content - {DateTime.Now}");
                return NoContent();
            }
            
            _logger.LogInformation($"Create user stop - {DateTime.Now}");
            return View(viewModel);
        }
        
        [Authorize(Roles = "sa,admin,user")]
        public async Task<IActionResult> Edit(string id)
        {
            _logger.LogInformation($"Edit user start - {DateTime.Now}");
            
            if (!string.IsNullOrEmpty(id))
            {
                var responseFromFacade = await _bussinessLayer.UsersReadFacade(id);

                if (responseFromFacade != null)
                {
                    _logger.LogInformation($"Edit user stop with success - {DateTime.Now}");
                    return View(responseFromFacade);
                }
            }

            _logger.LogInformation($"Edit user stop - {DateTime.Now}");
            return NotFound();
        }
        
        [Authorize(Roles = "sa,admin,user")]
        [HttpPost]
        public async Task<IActionResult> Edit(EditUserViewModel viewModel)
        {
            _logger.LogInformation($"Edit users start - {DateTime.Now}");
            var actionName = ControllerContext.ActionDescriptor.ActionName;
            
            if (ModelState.IsValid)
            {
                var requestToFacade = _mapper.Map<RequestFacadeDto>(viewModel);
                var responseFromFacade = await _bussinessLayer.UsersFacadeAsync(actionName, requestToFacade);
                
                if (responseFromFacade.Succeeded)
                {
                    _logger.LogInformation($"Edit roles stop with success - {DateTime.Now}");
                    return RedirectToAction("Index");
                }
                
                foreach (var error in responseFromFacade.Errors)
                {
                    _logger.LogError($"Edit roles stop with {error} - {DateTime.Now}");
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            _logger.LogInformation($"Edit roles stop - {DateTime.Now}");
            return View(viewModel);
        }
        
        [Authorize(Roles = "sa,admin,user")]
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            _logger.LogInformation($"Delete users start - {DateTime.Now}");
            var actionName = ControllerContext.ActionDescriptor.ActionName;
            
            if (!string.IsNullOrEmpty(id))
            {
                var requestToFacade = new RequestFacadeDto() { Id = id };
                var responseFromFacade = await _bussinessLayer.UsersFacadeAsync(actionName, requestToFacade);

                if (responseFromFacade.Succeeded)
                {
                    _logger.LogInformation($"Delete users stop with success - {DateTime.Now}");
                    return RedirectToAction("Index");
                }
                
                foreach (var error in responseFromFacade.Errors)
                {
                    _logger.LogError($"Edit roles stop with {error} - {DateTime.Now}");
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            _logger.LogInformation($"Delete users stop with success - {DateTime.Now}");
            return RedirectToAction("Index");
        }
        
        [Authorize(Roles = "sa,admin,user")]
        public async Task<IActionResult> ChangePassword(string id)
        {
            _logger.LogInformation($"Change password users start - {DateTime.Now}");

            if (!string.IsNullOrEmpty(id))
            {
                var responseFromFacade = await _bussinessLayer.UsersReadFacade(id);
                
                if (responseFromFacade != null)
                {
                    var changePasswordViewModel = new ChangePasswordViewModel() 
                        { 
                            Id = responseFromFacade.Id, 
                            Email = responseFromFacade.Email
                        };
                    
                    _logger.LogInformation($"Change password users stop with success - {DateTime.Now}");
                    return View(changePasswordViewModel);
                }

                _logger.LogInformation($"Change password users stop with {responseFromFacade} - {DateTime.Now}");
                return NotFound();
            }
            
            _logger.LogInformation($"Change password users stop with no content - {DateTime.Now}");
            return NoContent();
        }
        
        [Authorize(Roles = "sa,admin,user")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            _logger.LogInformation($"Change password users start - {DateTime.Now}");
            
            if (ModelState.IsValid)
            {
                var requestToFacade = _mapper.Map<RequestFacadeDto>(viewModel);
                var responseFromFacade = await _bussinessLayer.UserChangePasswordFacade(requestToFacade);

                if (responseFromFacade.Succeeded)
                {
                    _logger.LogInformation($"Change password users stop with success - {DateTime.Now}");
                    return RedirectToAction("Index");
                }
                
                foreach (var error in responseFromFacade.Errors)
                {
                    _logger.LogInformation($"Change password users stop with {error} - {DateTime.Now}");
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            _logger.LogInformation($"Change password users stop - {DateTime.Now}");
            return View(viewModel);
        }
    }
}
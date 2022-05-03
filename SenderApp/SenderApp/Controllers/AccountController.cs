using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Core.Abstractions.Gateway;
using Core.AppIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SenderApp.CommonLogic.BussinessLogic;
using SenderApp.Models.DTO.Requests;
using SenderApp.Models.ViewModels;
using SenderApp.Models.ViewModels.LoginUser;
using SenderApp.Models.ViewModels.RegisterUser;
using SenderApp.Services.MessageService;
using SenderApp.Services.ReportService;
using SenderApp.Services.TemplateService;

namespace SenderApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private readonly ILogger<AccountController> _logger;
        private readonly IBussinessLayer _bussinessLayer;
        private readonly IMapper _mapper;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly IGateway _gateway;
        private readonly IReportService _reportService;
        private readonly ITemplateEngineService _templateEngineService;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountController(
            SignInManager<CustomIdentityUser> signInManager,
            IBussinessLayer bussinessLayer,
            ILogger<AccountController> logger,
            IMapper mapper,
            UserManager<CustomIdentityUser> userManager,
            IGateway gateway,
            IReportService reportService,
            ITemplateEngineService templateEngineService,
            IHttpContextAccessor contextAccessor)
        {
            _signInManager = signInManager;
            _bussinessLayer = bussinessLayer;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _gateway = gateway;
            _reportService = reportService;
            _templateEngineService = templateEngineService;
            _contextAccessor = contextAccessor;
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            _logger.LogInformation($"Register start - {DateTime.Now}");
            var actionName = ControllerContext.ActionDescriptor.ActionName;

            if (ModelState.IsValid)
            {
                var requestToFacade = _mapper.Map<RequestFacadeDto>(viewModel);
                var responseFromFacade = await _bussinessLayer.RegisterFacadeAsync(actionName, requestToFacade);
                
                if (responseFromFacade != null)
                {
                    // генерация токена для пользователя
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(responseFromFacade);
                    
                    var callbackUrl = Url.Action(
                        "ConfirmEmail", 
                        "Account", 
                        new { userId = responseFromFacade.Id, code = code }, protocol: 
                        HttpContext.Request.Scheme
                        );

                    //Устанавливаем импровизированный гейт Email
                    await _gateway.SetGateAsync(new EmailService());
                    
                    var userAuthAttrib = new RequestTemplateDto()
                    {
                        Email = responseFromFacade.Email,
                        CallBackUrl = callbackUrl,
                    };
                    
                    //Прогоняем через шаблонизатор сообщение о подтверждении почты
                    var message = await _templateEngineService.TemplateEngine(
                        "UserAuthTemplate", 
                        "userauthtemplate",
                        null,
                        userAuthAttrib);

                    var currentUserName = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                    //Отправляем сообщение
                    await _gateway.SendMessageAsync(
                        currentUserName,
                        viewModel.Email, 
                        "Confirm your account", 
                        message);
                    
                    //Генерируем отчет об отправке пользователю
                    await _reportService.GenerateReportAsync(
                        "UserAuthReport",
                        "UserAuthReportTemplate",
                        "userauthreport",
                        userAuthAttrib);
                    
                    _logger.LogInformation($"Register stop with success - {DateTime.Now}");
                    return Content("Please check our email message account to confirm the registration");
                }
                
                _logger.LogInformation($"Register stop with no content - {DateTime.Now}");
                return NoContent();
            }
            
            _logger.LogInformation($"Register stop with success - {DateTime.Now}");
            return View(viewModel);
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            _logger.LogInformation($"ConfirmEmail start - {DateTime.Now}");
            
            if (userId == null || code == null)
            {
                return View("Error");
            }
            
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user == null)
            {
                return View("Error");
            }
            
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                var userRole = new List<string>() {"user"};

                await _bussinessLayer.RolesFacadeAsync("Edit", user.Id, userRole);
                _logger.LogInformation($"Confirm email stop with success - {DateTime.Now}");
                return RedirectToAction("Index", "Home");
            }

            _logger.LogInformation($"ConfirmEmail stop with error - {DateTime.Now}");
            return View("Error");
        }
        
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var result = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            
            return View(result);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            _logger.LogInformation($"Login start - {DateTime.Now}");

            if (ModelState.IsValid)
            {
                var requestToFacade = _mapper.Map<RequestFacadeDto>(viewModel);
                var responseFromFacade = await _bussinessLayer.LoginFacadeAsync(requestToFacade);
                
                if (responseFromFacade.Succeeded)
                {
                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(viewModel.ReturnUrl) && Url.IsLocalUrl(viewModel.ReturnUrl))
                    {
                        _logger.LogInformation($"Login stop with success - {DateTime.Now}");
                        return Redirect(viewModel.ReturnUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                
                _logger.LogInformation($"Login stop with incorrect login or password - {DateTime.Now}");
                ModelState.AddModelError("", "Incorrect login/password or your email has not been confirmed");
            }
            
            _logger.LogInformation($"Login stop - {DateTime.Now}");
            return View(viewModel);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
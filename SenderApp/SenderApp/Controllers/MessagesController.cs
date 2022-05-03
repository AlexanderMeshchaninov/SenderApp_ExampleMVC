using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Core.Abstractions.Gateway;
using Core.Abstractions.Services;
using Core.AppIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SenderApp.CommonLogic.BussinessLogic;
using SenderApp.Models.DTO.Requests;
using SenderApp.Models.ViewModels.SendMessagesUser;
using SenderApp.Services.MessageService;
using SenderApp.Services.TemplateService;

namespace SenderApp.Controllers
{
    [Authorize(Roles = "sa, admin, manager")]
    public class MessagesController : Controller
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly IGateway _gateway;
        private readonly ITemplateEngineService _templateEngineService;
        private readonly IBussinessLayer _bussinessLayer;
        private readonly IMapper _mapper;
        private readonly ISendScheduler _sendScheduler;
        private readonly IHttpContextAccessor _contextAccessor;

        public MessagesController(
            ILogger<MessagesController> logger, 
            UserManager<CustomIdentityUser> userManager,
            IGateway gateway,
            IHttpContextAccessor contextAccessor,
            ITemplateEngineService templateEngineService,
            IBussinessLayer bussinessLayer,
            IMapper mapper,
            ISendScheduler sendScheduler)
        {
            _logger = logger;
            _userManager = userManager;
            _gateway = gateway;
            _templateEngineService = templateEngineService;
            _bussinessLayer = bussinessLayer;
            _mapper = mapper;
            _sendScheduler = sendScheduler;
             _contextAccessor = contextAccessor;
        }
        
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"MessagesController start - {DateTime.Now}");
            var result = await _userManager.Users.ToListAsync();
            _logger.LogInformation($"MessagesController stop - {DateTime.Now}");
            
            return View(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> UserSendMessageNow(string id)
        {
            _logger.LogInformation($"User message menu start - {DateTime.Now}");
            
            if (!string.IsNullOrEmpty(id))
            {
                var responseFromFacade = await _bussinessLayer.UsersReadFacade(id);
                
                if (responseFromFacade != null)
                {
                    var sendMessageViewModel = new UserMessageViewModel()
                    {
                        Id = responseFromFacade.Id,
                        Email = responseFromFacade.Email,
                    };
                    
                    _logger.LogInformation($"User message menu  stop with success - {DateTime.Now}");
                    return View(sendMessageViewModel);
                }

                _logger.LogInformation($"User message menu stop with {responseFromFacade} - {DateTime.Now}");
                return NotFound();
            }
            
            _logger.LogInformation($"User message menu stop - {DateTime.Now}");
            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> SendMessageNow(UserMessageViewModel userMessageViewModel)
        {
            _logger.LogInformation($"Send message start - {DateTime.Now}");
            
            if (ModelState.IsValid)
            {
                var requestTemplate = new RequestTemplateDto()
                {
                    Email = userMessageViewModel.Email,
                    Message = userMessageViewModel.Message,
                    Template = userMessageViewModel.Template
                };
                
                string templateSubject = string.Empty;
                
                switch (requestTemplate.Template)
                {
                    case "UserMessageHappyNewYearTemplate":
                        templateSubject = "Happy New Year";
                        break;
                    
                    case "UserCustomMessageTemplate":
                        templateSubject = "Dear customer";
                        break;
                    
                    case "UserMessageHappyBirthdayTemplate":
                        templateSubject = "Happy Birthday";
                        break;
                }
                
                await _gateway.SetGateAsync(new EmailService());
                
                var message = await _templateEngineService.TemplateEngine(
                    requestTemplate.Template,
                    "usermessagenow",
                    null,
                    requestTemplate);

                var currentUserName = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                await _gateway.SendMessageAsync(currentUserName, requestTemplate.Email, templateSubject, message);
                
                _logger.LogInformation($"Send message stop with success - {DateTime.Now}");
                return RedirectToAction("Index");
            }
            
            _logger.LogInformation($"Send message stop with model state is not valid - {DateTime.Now}");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UserSendMessageByTime(string id)
        {
            _logger.LogInformation($"User message menu start - {DateTime.Now}");
            
            if (!string.IsNullOrEmpty(id))
            {
                var responseFromFacade = await _bussinessLayer.UsersReadFacade(id);
                
                if (responseFromFacade != null)
                {
                    var sendMessageViewModel = new UserMessageViewModel()
                    {
                        Id = responseFromFacade.Id,
                        Email = responseFromFacade.Email,
                    };
                    
                    _logger.LogInformation($"User message menu  stop with success - {DateTime.Now}");
                    return View(sendMessageViewModel);
                }

                _logger.LogInformation($"User message menu stop with {responseFromFacade} - {DateTime.Now}");
                return NotFound();
            }
            
            _logger.LogInformation($"User message menu stop - {DateTime.Now}");
            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> SendMessageByTime(UserMessageViewModel userMessageViewModel)
        {
            _logger.LogInformation($"Send message by time start - {DateTime.Now}");
            
            if (ModelState.IsValid && userMessageViewModel.IsSendingMessageBySpecificTime)
            {
                var requestTemplate = new RequestTemplateDto()
                {
                    Email = userMessageViewModel.Email,
                    Message = userMessageViewModel.Message,
                    Template = userMessageViewModel.Template
                };
                
                string templateSubject = string.Empty;
                
                switch (requestTemplate.Template)
                {
                    case "UserMessageHappyNewYearTemplate":
                        templateSubject = "Happy New Year";
                        break;
                    case "UserCustomMessageTemplate":
                        templateSubject = "Dear customer";
                        break;
                    case "UserMessageHappyBirthdayTemplate":
                        templateSubject = "Happy Birthday";
                        break;
                }
                
                var message = await _templateEngineService.TemplateEngine(
                    requestTemplate.Template,
                    "usermessagebytime",
                    null,
                    requestTemplate);

                var currentUserName = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
                await _sendScheduler.Run(
                    currentUserName,
                    requestTemplate.Email,
                    message,
                    templateSubject,
                    userMessageViewModel.SendingMessageTime);

                _logger.LogInformation($"Send message by time stop with success - {DateTime.Now}");
                return RedirectToAction("Index");
            }
            
            _logger.LogInformation($"Send message by time stop with model state is not valid - {DateTime.Now}");
            return RedirectToAction("Index");
        }
    }
}
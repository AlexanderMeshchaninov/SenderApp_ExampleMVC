using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Abstractions.Layers;
using Core.AppIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SenderApp.Data.RolesRepository;
using SenderApp.Data.UsersRepository;
using SenderApp.Models.DTO.Requests;
using SenderApp.Models.DTO.Responses;
using SenderApp.Models.ViewModels;
using SenderApp.Models.ViewModels.LoginUser;

namespace SenderApp.CommonLogic.BussinessLogic
{
    public interface IBussinessLayer :
        IRegisterFacade<CustomIdentityUser, RequestFacadeDto>,
        ILoginFacade<ResponseLoginDto, RequestFacadeDto>,
        IRolesFacade<ResponseIdentityDto>,
        IRolesEditFacade<ResponseChangeRoleDto>,
        IUsersFacade<ResponseIdentityDto, RequestFacadeDto>,
        IUsersEditFacade<ResponseEditUserDto>
    {
    }

    public sealed class BussinessLogic : IBussinessLayer
    {
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly SignInManager<CustomIdentityUser> _signInManager;
        private readonly ILogger<BussinessLogic> _logger;
        private readonly IMapper _mapper;
        private readonly IRolesRepository _rolesRepository;
        private readonly IUserRepository _userRepository;
        
        public BussinessLogic(
            UserManager<CustomIdentityUser> userManager, 
            SignInManager<CustomIdentityUser> signInManager,
            ILogger<BussinessLogic> logger,
            IMapper mapper,
            IRolesRepository rolesRepository,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _mapper = mapper;
            _rolesRepository = rolesRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Фасад для регистрации новых пользователей
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<CustomIdentityUser> RegisterFacadeAsync(string actionName, RequestFacadeDto requestDto)
        {
            _logger.LogInformation($"RegisterFacade start - {DateTime.Now}");
            
            if (requestDto != null)
            {
                var user = new CustomIdentityUser()
                {
                    Email = requestDto.Email, 
                    UserName = requestDto.Email
                };
            
                // добавляем пользователя
                var identityResult = await _userManager.CreateAsync(user, requestDto.Password);
                
                if (identityResult.Succeeded)
                {
                    if (actionName.Contains("Register"))
                    {
                        // установка куки
                        await _signInManager.SignInAsync(user, false);
                    }
                    
                    _logger.LogInformation($"RegisterFacade stop with success - {DateTime.Now}");
                    return user;
                }
                
                _logger.LogInformation($"RegisterFacade stop and return with NULL requestDto - {DateTime.Now}");
                return null;
            }
            
            _logger.LogInformation($"RegisterFacade stop and return NULL requestDto - {DateTime.Now}");
            return null;
        }
        
        /// <summary>
        /// Фасад для входа в приложение зарегистрированных пользователей
        /// </summary>
        /// <param name="requestLoginDto"></param>
        /// <returns></returns>
        public async Task<ResponseLoginDto> LoginFacadeAsync(RequestFacadeDto requestLoginDto)
        {
            _logger.LogInformation($"LoginFacade start - {DateTime.Now}");
            var responseLoginDto = new ResponseLoginDto();
            
            var identifyUser = await _userManager.FindByEmailAsync(requestLoginDto.Email);
            
            if (identifyUser != null)
            {
                var isEmailConfirmed = await CheckIsEmailConfirmedAsync(identifyUser);
                var isAdmin = await CheckIsAdminAsync(identifyUser);
            
                if (isEmailConfirmed || isAdmin)
                {
                    var loginViewModel = _mapper.Map<LoginViewModel>(requestLoginDto);
                
                    var signInResult = await _signInManager.PasswordSignInAsync(
                        loginViewModel.Email, 
                        loginViewModel.Password, 
                        loginViewModel.RememberMe,
                        false);
                
                    responseLoginDto = _mapper.Map<ResponseLoginDto>(signInResult);
                
                    if (signInResult.Succeeded)
                    {
                        _logger.LogInformation($"LoginFacade stop with success - {DateTime.Now}");
                        return responseLoginDto;
                    }
                
                    _logger.LogInformation($"LoginFacade stop and return with {responseLoginDto} - {DateTime.Now}");
                    return responseLoginDto;
                }
            }
            
            _logger.LogInformation($"LoginFacade stop and return with {requestLoginDto} - {DateTime.Now}");
            return responseLoginDto;
        }
        
        /// <summary>
        /// Фасад для управление "ролями" пользователей
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="item"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public async Task<ResponseIdentityDto> RolesFacadeAsync(string actionName, string item, List<string> roles)
        {
            _logger.LogInformation($"RolesFacade start - {DateTime.Now}");

            switch (actionName)
            {
                case "Create":
                    _logger.LogInformation($"Create action start - {DateTime.Now}");
                    var responseFromCreate = await _rolesRepository.CreateRoleAsync(item);
            
                    if (responseFromCreate.Succeeded)
                    {
                        _logger.LogInformation($"Create action return with success - {DateTime.Now}");
                        return responseFromCreate;
                    }

                    _logger.LogInformation($"Create action return with {responseFromCreate} - {DateTime.Now}");
                    return responseFromCreate;
                
                case "Edit":
                    _logger.LogInformation($"Update action start - {DateTime.Now}");
                    var responseFromUpdate = await _rolesRepository.UpdateRoleByIdAsync(item, roles);
            
                    if (responseFromUpdate.Succeeded)
                    {
                        _logger.LogInformation($"Update action return with success - {DateTime.Now}");
                        return responseFromUpdate;
                    }

                    _logger.LogInformation($"Update action return with {responseFromUpdate} - {DateTime.Now}");
                    return responseFromUpdate;
                    
                case "Delete":
                    _logger.LogInformation($"Delete action start - {DateTime.Now}");
                    var responseFromDelete = await _rolesRepository.DeleteRoleByIdAsync(item);

                    if (responseFromDelete.Succeeded)
                    {
                        _logger.LogInformation($"Delete action return with success - {DateTime.Now}");
                        return responseFromDelete;
                    }

                    _logger.LogInformation($"Delete action return with {responseFromDelete} - {DateTime.Now}");
                    return responseFromDelete;
            }

            _logger.LogInformation($"RolesFacade return NULL - {DateTime.Now}");
            return null;
        }
        
        /// <summary>
        /// Фасад для чтения "роли" найденного пользователя
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<ResponseChangeRoleDto> RolesReadFacade(string item)
        {
            _logger.LogInformation($"Edit role action start - {DateTime.Now}");
            var responseFromEdit = await _rolesRepository.ReadRoleByIdAsync(item);
            
            if (responseFromEdit != null)
            {
                _logger.LogInformation($"Edit role action stop with success - {DateTime.Now}");
                return responseFromEdit;
            }

            _logger.LogInformation($"Edit role action return with {responseFromEdit} - {DateTime.Now}");
            return responseFromEdit;
        }

        /// <summary>
        /// Фасад для чтения пользователя
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task<ResponseEditUserDto> UsersReadFacade(string item)
        {
            _logger.LogInformation($"Edit user action start - {DateTime.Now}");
            var responseFromEdit = await _userRepository.ReadUserByIdAsync(item);
            
            if (responseFromEdit != null)
            {
                _logger.LogInformation($"Edit user action stop with success - {DateTime.Now}");
                return responseFromEdit;
            }

            _logger.LogInformation($"Edit user action return with {responseFromEdit} - {DateTime.Now}");
            return responseFromEdit;
        }
        
        /// <summary>
        /// Фасад для редактирования и удаления пользователя
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseIdentityDto> UsersFacadeAsync(string actionName, RequestFacadeDto requestDto)
        {
            _logger.LogInformation($"User facade start - {DateTime.Now}");

            switch (actionName)
            {
                case "Edit":
                    if (requestDto != null)
                    {
                        var responseFromRepository = await _userRepository.UpdateUserByIdAsync(requestDto.Id, requestDto);

                        if (responseFromRepository.Succeeded)
                        {
                            _logger.LogInformation($"User facade return with success - {DateTime.Now}");
                            return responseFromRepository;
                        }

                        _logger.LogInformation($"User facade return with {responseFromRepository} - {DateTime.Now}");
                        return responseFromRepository;
                    }
            
                    _logger.LogInformation($"User facade stop and return NULL requestDto - {DateTime.Now}");
                    return null;
                
                case "Delete":
                    if (requestDto != null)
                    {
                        var responseFromRepository = await _userRepository.DeleteUserByIdAsync(requestDto.Id);

                        if (responseFromRepository.Succeeded)
                        {
                            _logger.LogInformation($"User facade return with success - {DateTime.Now}");
                            return responseFromRepository;
                        }

                        _logger.LogInformation($"User facade return with {responseFromRepository} - {DateTime.Now}");
                        return responseFromRepository;
                    }
            
                    _logger.LogInformation($"User facade stop and return NULL requestDto - {DateTime.Now}");
                    return null;
            }
         
            _logger.LogInformation($"User facade stop and return NULL requestDto - {DateTime.Now}");
            return null;
        }

        /// <summary>
        /// Фасад для смены пароля пользователя
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        public async Task<ResponseIdentityDto> UserChangePasswordFacade(RequestFacadeDto requestDto)
        {
            _logger.LogInformation($"User facade start - {DateTime.Now}");

            if (requestDto != null)
            {
                var identityUser = await _userManager.FindByIdAsync(requestDto.Id);
                
                var identityResult = await _userManager
                    .ChangePasswordAsync(identityUser, requestDto.OldPassword, requestDto.NewPassword);
                
                var responseFromFacade = _mapper.Map<ResponseIdentityDto>(identityResult);
                
                if (identityResult.Succeeded)
                {
                    _logger.LogInformation($"User facade return with success - {DateTime.Now}");
                    return responseFromFacade;
                }

                _logger.LogInformation($"User facade return with {responseFromFacade} - {DateTime.Now}");
                return responseFromFacade;
            }

            _logger.LogInformation($"User facade return with NULL - {DateTime.Now}");
            return null;
        }

        private async Task<bool> CheckIsEmailConfirmedAsync(CustomIdentityUser identityUser)
        {
            //Проверка подтвердил ли пользователь свой email
            var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(identityUser);

            if (!isEmailConfirmed)
            {
                return await Task.FromResult(false);
            }

            return await Task.FromResult(true);
        }
        private async Task<bool> CheckIsAdminAsync(CustomIdentityUser identityUser)
        {
            var userRoleList = await _userManager.GetRolesAsync(identityUser);
            var userRole = userRoleList.FirstOrDefault();

            if (userRole != null && userRole.Contains("admin"))
            {
                return await Task.FromResult(true);
            }
            
            return await Task.FromResult(false);
        }
    }
}
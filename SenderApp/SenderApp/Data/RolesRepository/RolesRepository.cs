using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Abstractions.Data.RolesRepository;
using Core.AppIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SenderApp.Models.DTO.Responses;
using SenderApp.Models.ViewModels;
using SenderApp.Models.ViewModels.ChangeUserRole;

namespace SenderApp.Data.RolesRepository
{
    public interface IRolesRepository : 
        IRolesRepository<ResponseIdentityDto>,
        IRolesEditRepository<ResponseChangeRoleDto>
    {
    }

    public sealed class RolesRepository : IRolesRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<RolesRepository> _logger;

        public RolesRepository(
        RoleManager<IdentityRole> roleManager,
        UserManager<CustomIdentityUser> userManager,
        IMapper mapper,
        ILogger<RolesRepository> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Метод создания роли в базе данных
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<ResponseIdentityDto> CreateRoleAsync(string name)
        {
            _logger.LogInformation($"CreateRoleAsync start - {DateTime.Now}");
            
            try
            {
                var identityResult = await _roleManager.CreateAsync(new IdentityRole(name));
                
                _logger.LogInformation($"New role created with success - {DateTime.Now}");
                var responseFromRepository = _mapper.Map<ResponseIdentityDto>(identityResult);

                _logger.LogInformation($"CreateRoleAsync stop with success - {DateTime.Now}");
                return responseFromRepository;
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateRoleAsync stop with {ex} - {DateTime.Now}");
                return null;
            }
        }
        
        /// <summary>
        /// Метод чтения роли в базе данных
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseChangeRoleDto> ReadRoleByIdAsync(string id)
        {
            _logger.LogInformation($"ReadRoleAsync start - {DateTime.Now}");

            try
            {
                var identityUser = await _userManager.FindByIdAsync(id);
                
                if(identityUser != null)
                {
                    // получем список ролей пользователя
                    var userRoles = await _userManager.GetRolesAsync(identityUser);
                    var allRoles = _roleManager.Roles.ToList();
                    
                    var model = new ChangeRoleViewModel
                    {
                        UserId = identityUser.Id,
                        UserEmail = identityUser.Email,
                        UserRoles = userRoles,
                        AllRoles = allRoles
                    };
                    
                    var responseFromRepository = _mapper.Map<ResponseChangeRoleDto>(model);

                    _logger.LogInformation($"ReadRoleAsync return with success - {DateTime.Now}");
                    return responseFromRepository;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"EditRoleAsync return with {ex} - {DateTime.Now}");
                return null;
            }

            _logger.LogInformation($"EditRoleAsync return with NULL - {DateTime.Now}");
            return null;
        }
        
        /// <summary>
        /// Метод обновления роли в базе данных
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        public async Task<ResponseIdentityDto> UpdateRoleByIdAsync(string id, List<string> roles)
        {
            _logger.LogInformation($"UpdateRoleAsync start - {DateTime.Now}");

            try
            {
                var identityUser = await _userManager.FindByIdAsync(id);
                
                if(identityUser != null)
                {
                    var userRoles = await _userManager.GetRolesAsync(identityUser);
                    // получаем список ролей, которые были добавлены
                    var addedRoles = roles.Except(userRoles);
                    // получаем роли, которые были удалены
                    var removedRoles = userRoles.Except(roles);
            
                    await _userManager.AddToRolesAsync(identityUser, addedRoles);
                    var identityResult = await _userManager.RemoveFromRolesAsync(identityUser, removedRoles);

                    var responseFromRepository = _mapper.Map<ResponseIdentityDto>(identityResult);

                    _logger.LogInformation($"UpdateRoleAsync return with success - {DateTime.Now}");
                    return responseFromRepository;
                }
                
                _logger.LogInformation($"UpdateRoleAsync return NULL - {DateTime.Now}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"UpdateRoleAsync return with {ex} - {DateTime.Now}");
                return null;
            }
        }
        
        /// <summary>
        /// Метод удаления роли в базе данных
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseIdentityDto> DeleteRoleByIdAsync(string id)
        {
            _logger.LogInformation($"DeleteRoleAsync start - {DateTime.Now}");
            
            try
            {
                var identityRole = await _roleManager.FindByIdAsync(id);
                
                if (identityRole != null)
                {
                    var identityResult = await _roleManager.DeleteAsync(identityRole);
                    
                    _logger.LogInformation($"Role deleted with success - {DateTime.Now}");
                    var responseFromRepository = _mapper.Map<ResponseIdentityDto>(identityResult);

                    _logger.LogInformation($"DeleteRoleAsync return with success - {DateTime.Now}");
                    return responseFromRepository;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"DeleteRoleAsync return with {ex} - {DateTime.Now}");
                return null;
            }

            _logger.LogInformation($"DeleteRoleAsync return with NULL - {DateTime.Now}");
            return null;
        }
    }
}
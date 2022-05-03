using System;
using System.Threading.Tasks;
using AutoMapper;
using Core.Abstractions.Data.UsersRepository;
using Core.AppIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SenderApp.Models.DTO.Requests;
using SenderApp.Models.DTO.Responses;
using SenderApp.Models.ViewModels;
using SenderApp.Models.ViewModels.EditUserInfo;

namespace SenderApp.Data.UsersRepository
{
    public interface IUserRepository : 
        IUsersRepository<ResponseIdentityDto, RequestFacadeDto>,
        IUsersEditRepository<ResponseEditUserDto>
    {
    }

    public class UsersRepository : IUserRepository
    {
        private readonly ILogger<UsersRepository> _logger;
        private readonly UserManager<CustomIdentityUser> _userManager;
        private readonly IMapper _mapper;

        public UsersRepository(
            ILogger<UsersRepository> logger,
            UserManager<CustomIdentityUser> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }
        
        public async Task<ResponseIdentityDto> UpdateUserByIdAsync(string id, RequestFacadeDto requestFacadeDto)
        {
            _logger.LogInformation($"Update user start - {DateTime.Now}");

            try
            {
                var identityUser = await _userManager.FindByIdAsync(id);

                if (identityUser != null)
                {
                    identityUser.Email = requestFacadeDto.Email;
                    identityUser.UserName = requestFacadeDto.Email;
                    
                    var identityResult = await _userManager.UpdateAsync(identityUser);
                    
                    var responseFromRepository = _mapper.Map<ResponseIdentityDto>(identityResult);
                    
                    if (identityResult.Succeeded)
                    {
                        _logger.LogInformation($"Update user return with success - {DateTime.Now}");
                        return responseFromRepository;
                    }

                    _logger.LogInformation($"Update user return with {responseFromRepository} - {DateTime.Now}");
                    return responseFromRepository;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Update user return with {ex} - {DateTime.Now}");
                return null;
            }

            _logger.LogInformation($"Update user return with NULL - {DateTime.Now}");
            return null;
        }
        
        public async Task<ResponseIdentityDto> DeleteUserByIdAsync(string id)
        {
            _logger.LogInformation($"Delete user start - {DateTime.Now}");

            try
            {
                var identityUser = await _userManager.FindByIdAsync(id);
                
                if (identityUser != null)
                {
                    var identityResult = await _userManager.DeleteAsync(identityUser);
                    var responseFromRepository = _mapper.Map<ResponseIdentityDto>(identityResult);
                    
                    if (identityResult.Succeeded)
                    {
                        _logger.LogInformation($"Delete user return with success - {DateTime.Now}");
                        return responseFromRepository;
                    }
                    
                    _logger.LogInformation($"Delete user return with {responseFromRepository} - {DateTime.Now}");
                    return responseFromRepository;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Delete user return with {ex} - {DateTime.Now}");
                return null;
            }

            _logger.LogInformation($"Delete user return with NULL - {DateTime.Now}");
            return null;
        }

        public async Task<ResponseEditUserDto> ReadUserByIdAsync(string id)
        {
            _logger.LogInformation($"ReadUserAsync start - {DateTime.Now}");

            try
            {
                var identityUser = await _userManager.FindByIdAsync(id);
                
                if (identityUser != null)
                {
                    var userViewModel = new EditUserViewModel
                    {
                        Id = identityUser.Id, 
                        Email = identityUser.Email
                    };

                    var responseFromRepository = _mapper.Map<ResponseEditUserDto>(userViewModel);

                    _logger.LogInformation($"ReadUserAsync return with success - {DateTime.Now}");
                    return responseFromRepository;
                }

                _logger.LogInformation($"ReadUserAsync return NULL - {DateTime.Now}");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"ReadUserAsync return with {ex} - {DateTime.Now}");
                return null;
            }
        }
    }
}
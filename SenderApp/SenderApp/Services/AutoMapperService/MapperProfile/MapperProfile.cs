using AutoMapper;
using Core.AppIdentity.Models;
using Microsoft.AspNetCore.Identity;
using SenderApp.Models.DTO.Requests;
using SenderApp.Models.DTO.Responses;
using SenderApp.Models.ViewModels.ChangeUserPassword;
using SenderApp.Models.ViewModels.ChangeUserRole;
using SenderApp.Models.ViewModels.EditUserInfo;
using SenderApp.Models.ViewModels.LoginUser;
using SenderApp.Models.ViewModels.RegisterUser;
using SenderApp.Models.ViewModels.SendMessagesUser;

namespace SenderApp.Services.AutoMapperService.MapperProfile
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            //Requests
            CreateMap<RegisterViewModel, RequestFacadeDto>();
            CreateMap<LoginViewModel, RequestFacadeDto>();
            CreateMap<CreateUserViewModel, RequestFacadeDto>();
            CreateMap<EditUserViewModel, RequestFacadeDto>();
            CreateMap<ChangePasswordViewModel, RequestFacadeDto>();
            CreateMap<UserMessageViewModel, RequestTemplateDto>();
            
            //Mapping inside Facade
            CreateMap<RequestFacadeDto, LoginViewModel>();
            CreateMap<RequestFacadeDto, CustomIdentityUser>();
            CreateMap<RequestFacadeDto, RegisterViewModel>();
            
            //Responses
            CreateMap<IdentityResult, ResponseIdentityDto>();
            CreateMap<SignInResult, ResponseLoginDto>();
            CreateMap<ChangeRoleViewModel, ResponseChangeRoleDto>();
            CreateMap<EditUserViewModel, ResponseEditUserDto>();
        }
    }
}
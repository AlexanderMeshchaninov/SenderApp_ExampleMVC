using System.Collections.Generic;
using Core.Abstractions.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace SenderApp.Models.ViewModels.ChangeUserRole
{
    public class ChangeRoleViewModel : AbstractChangeRoleBaseModel
    {
        public override string UserId { get; set; }
        public override string UserEmail { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
        
        public ChangeRoleViewModel()
        {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
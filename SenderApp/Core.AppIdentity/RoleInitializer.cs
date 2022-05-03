using System.Threading.Tasks;
using Core.AppIdentity.Models;
using Microsoft.AspNetCore.Identity;

namespace Core.AppIdentity
{
    public static class RoleInitializer
    {
        /// <summary>
        /// Инициализация базы данных с созданием ролей "админ" и "пользователь"
        /// Создание одной учетной записи "админа"
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        public static async Task InitializeAsync(
            UserManager<CustomIdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            //Инициализация первичных данный суперадмина (пароль должен быть буквы и цифры, большие и маленькие)
            string superAdminEmail = "sa001@mail.ru";
            string superAdminPassword = "12345";
            
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            
            if (await userManager.FindByNameAsync(superAdminEmail) == null)
            {
                var superAdmin = new CustomIdentityUser 
                    {
                        Email = superAdminEmail, 
                        UserName = superAdminEmail 
                    };
                
                var identityResult = await userManager.CreateAsync(superAdmin, superAdminPassword);
                
                if (identityResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(superAdmin, "admin");
                }
            }
        }
    }
}
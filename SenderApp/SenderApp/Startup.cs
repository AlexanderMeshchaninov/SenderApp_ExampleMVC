using Core.AppIdentity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SenderApp.Data.Identity;
using SenderApp.Registration;

namespace SenderApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //IDENTITY (MYSQL - DATABASE)--->
            services.RegisterMySqlIdentityProvider(Configuration);
            services.AddIdentity<CustomIdentityUser, IdentityRole>(options =>
                {
                    //По умолчанию поставил так
                    options.Password.RequiredLength = 5; //минимальная длинна пароля
                    options.Password.RequireNonAlphanumeric = false; //требуется ли применять символы
                    options.Password.RequireLowercase = false; //требуются ли символы в нижнем регистре
                    options.Password.RequireUppercase = false; //требуются ли символя в верхнем регистре
                    options.Password.RequireDigit = false; //требуются ли применять цифры в пароле
                })
                .AddEntityFrameworkStores<SenderIdentityDbContext>()
                .AddDefaultTokenProviders();
            services.AddDatabaseDeveloperPageExceptionFilter();
            
            //AUTOMAPPER--->
            services.RegisterMapperService();
            //BussinessLayer--->
            services.RegisterBussinessLayer();
            //RolesRepository--->
            services.RegisterRolesRepository();
            //UsersRepository--->
            services.RegisterUsersRepository();
            //ContentAccessor--->
            services.AddHttpContextAccessor();
            //"Gateway"--->
            services.RegisterGateway();
            //ReportService--->
            services.RegistryReportService();
            //TemplateService--->
            services.RegisterTemplateService();
            //Quartz scheduler--->
            services.RegisterSendEmailScheduler();
            
            services.AddHttpClient();
            
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
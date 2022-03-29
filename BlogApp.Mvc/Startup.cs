using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BlogApp.Entities.Concrete;
using BlogApp.Mvc.AutoMapper.Profiles;
using BlogApp.Mvc.Helpers.Abstract;
using BlogApp.Mvc.Helpers.Concrete;
using BlogApp.Services.AutoMapper.Profiles;
using BlogApp.Services.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProgrammersBlog.Services.AutoMapper.Profiles;

namespace BlogApp.Mvc
{
    public class Startup
    {
        private IConfiguration _configuration;
        public Startup(IConfiguration configuraiton)
        {
            _configuration = configuraiton;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AboutUsPageInfo>(_configuration.GetSection("AboutUsPageInfo"));
            services.Configure<WebsiteInfo>(_configuration.GetSection("WebsiteInfo"));
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            }).AddNToastNotifyToastr();
            services.AddSession();
            services.AddAutoMapper(typeof(CategoryProfile),typeof(ArticleProfile),typeof(UserProfile),typeof(ViewModelsProfile),typeof(CommentProfile));
            services.LoadMyServices(connectionString:_configuration.GetConnectionString("LocalDB"));
            services.AddScoped<IImageHelper,ImageHelper>();
            services.ConfigureApplicationCookie(options => {
                options.LoginPath = new PathString("/Admin/Auth/Login/");
                options.LogoutPath = new PathString("/Admin/Auth/Logout/");
                options.Cookie = new CookieBuilder{
                    Name = "BlogApp",
                    HttpOnly = true,
                    SameSite = SameSiteMode.Strict,
                    SecurePolicy = CookieSecurePolicy.SameAsRequest
                };
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
                options.AccessDeniedPath = new PathString("/Admin/Auth/AccessDenied/");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }

            app.UseSession();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseNToastNotify();

            app.UseEndpoints(endpoints =>
            {   
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

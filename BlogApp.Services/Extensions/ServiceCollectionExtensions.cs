using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EntityFramework;
using BlogApp.Data.Concrete.EntityFramework.Contexts;
using BlogApp.Entities.Concrete;
using BlogApp.Services.Abstract;
using BlogApp.Services.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BlogApp.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadMyServices(this IServiceCollection serviceCollection, string connectionString){
            serviceCollection.AddDbContext<BlogContext>(options => options.UseSqlServer(connectionString));
            serviceCollection.AddIdentity<User,Role>(options => {
                //Password Options
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                //Username and Email Options
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+$";
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<BlogContext>();
            serviceCollection.AddScoped<IUnitOfWork,UnitOfWork>();
            serviceCollection.AddScoped<ICategoryService,CategoryManager>();
            serviceCollection.AddScoped<IArticleService,ArticleManager>();
            serviceCollection.AddScoped<ICommentService,CommentManager>();

            return serviceCollection;
        }
    }
}
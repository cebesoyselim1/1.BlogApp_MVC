using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EntityFramework;
using BlogApp.Data.Concrete.EntityFramework.Contexts;
using BlogApp.Services.Abstract;
using BlogApp.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;

namespace BlogApp.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadMyServices(this IServiceCollection serviceCollection){
            serviceCollection.AddDbContext<BlogContext>();
            serviceCollection.AddScoped<IUnitOfWork,UnitOfWork>();
            serviceCollection.AddScoped<ICategoryService,CategoryManager>();
            serviceCollection.AddScoped<IArticleService,ArticleManager>();

            return serviceCollection;
        }
    }
}
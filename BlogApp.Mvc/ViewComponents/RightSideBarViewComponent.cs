using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Mvc.Models;
using BlogApp.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Mvc.ViewComponents
{
    public class RightSideBarViewComponent:ViewComponent
    {
        public IArticleService _articleService { get; set; }
        public ICategoryService _categoryService { get; set; }
        public RightSideBarViewComponent(IArticleService articleService, ICategoryService categoryService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(){
            var categoriesResult = await _categoryService.GetAllNonDeletedAndActiveAsync();
            var articlesResult = await _articleService.GetAllByViewCountAsync(isAscending: true, 5);
            return View(new RightSideBarViewModel(){
                Categories = categoriesResult.Data.Categories,
                Articles = articlesResult.Data.Articles
            });
        }
    }
}
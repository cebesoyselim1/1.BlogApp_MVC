using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Mvc.Controllers
{
    public class HomeController:Controller
    {
        private readonly IArticleService _articleSerice;
        public HomeController(IArticleService articleService)
        {
            _articleSerice = articleService;
        }

        public async Task<IActionResult> Index(){
            var articleListDto = await _articleSerice.GetAllNonDeletedAndActiveAsync();
            return View(articleListDto.Data);
        }
    }
}
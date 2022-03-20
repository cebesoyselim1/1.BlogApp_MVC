using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Services.Abstract;
using BlogApp.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController:Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task<IActionResult> Index(){
            var articles = await _articleService.GetAllNonDeletedAsync();
            if(articles.ResultStatus == ResultStatus.Success) return View(articles.Data);
            return NotFound();
        }

        public IActionResult Add(){
            return View();
        }
    }
}
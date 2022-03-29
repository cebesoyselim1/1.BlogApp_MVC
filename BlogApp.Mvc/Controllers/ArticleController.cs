using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.ComplexTypes;
using BlogApp.Mvc.Models;
using BlogApp.Services.Abstract;
using BlogApp.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Mvc.Controllers
{
    public class ArticleController:Controller
    {
        private readonly IArticleService _articleSerive;
        public ArticleController(IArticleService articleService)
        {
            _articleSerive = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword, int currentPage = 1, int pageSize = 5, bool isAcending = false){
            var searhResult = await _articleSerive.SearchAsync(keyword, currentPage, pageSize, isAcending);
            if(searhResult.ResultStatus == ResultStatus.Success){
                return View(new ArticleSearchViewModel()
                {
                    ArticleListDto = searhResult.Data,
                    Keyword = keyword
                });
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int articleId){
            var articleResult = await _articleSerive.GetAsync(articleId);
            if(articleResult.ResultStatus == ResultStatus.Success){
                var userArticles = await _articleSerive.GetAllByUserIdOnFilter(articleResult.Data.Article.UserId, FilterBy.Category, OrderBy.Date, false, 10, articleResult.Data.Article.CategoryId, DateTime.Now, DateTime.Now, 0, 99999, 0, 99999);
                await _articleSerive.IncreaseViewCountAsync(articleId);
                return View(new ArticleDetailViewModel(){
                    ArticleDto = articleResult.Data,
                    ArticleDetailRightSideBarViewModel = new ArticleDetailRightSideBarViewModel(){
                        ArticleListDto = userArticles.Data,
                        Header = "User's most viewed articles in same category",
                        User = articleResult.Data.Article.User
                    }
                });
            }
            return NotFound();
        }
    }
}
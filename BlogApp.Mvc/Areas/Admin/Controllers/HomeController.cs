using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Concrete;
using BlogApp.Mvc.Areas.Admin.Models;
using BlogApp.Services.Abstract;
using BlogApp.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Mvc.Areas.Admin.Controllers
{
    public class HomeController:Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly ICommentService _commentService;
        private UserManager<User> _userManager;

        public HomeController(IArticleService articleService, ICategoryService categoryService, ICommentService commentService, UserManager<User> userManager)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _commentService = commentService;
            _userManager = userManager;
        }
        
        [Area("Admin")]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Index(){
            var articleCountResult = await _articleService.CountByNonDeleted();
            var categoryCountResult = await _categoryService.CountByNonDeleted();
            var commentCountResult = await _commentService.CountByNonDeleted();
            var userCount = await _userManager.Users.CountAsync();
            var articlesResult = await _articleService.GetAll();

            if(articleCountResult.ResultStatus == ResultStatus.Success && categoryCountResult.ResultStatus == ResultStatus.Success && commentCountResult.ResultStatus == ResultStatus.Success && userCount > -1 && articlesResult.ResultStatus == ResultStatus.Success){
                return View(new DashboardViewModel(){
                    ArticlesCount = articleCountResult.Data,
                    CategoriesCount = categoryCountResult.Data,
                    CommentsCount = commentCountResult.Data,
                    UsersCount = userCount,
                    Articles = articlesResult.Data
                });
            }

            return NotFound();
        }
    }
}
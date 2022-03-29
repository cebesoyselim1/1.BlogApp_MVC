using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Concrete;
using BlogApp.Entities.Dtos;
using BlogApp.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BlogApp.Mvc.Controllers
{
    public class HomeController:Controller
    {
        private readonly IArticleService _articleSerice;
        private readonly AboutUsPageInfo _aboutUsPageInfo;
        public HomeController(IArticleService articleService, IOptions<AboutUsPageInfo> aboutUsPageInfo)
        {
            _articleSerice = articleService;
            _aboutUsPageInfo = aboutUsPageInfo.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false){
            var articleListDto = categoryId == null ?
            await _articleSerice.GetAllByPagingAsync(null,currentPage,pageSize,isAscending) :
            await _articleSerice.GetAllByPagingAsync(categoryId.Value,currentPage,pageSize,isAscending);
            return View(articleListDto.Data);
        }

        [HttpGet]
        public IActionResult About(){
            return View(_aboutUsPageInfo);
        }

        [HttpGet]
        public IActionResult Contact(){
            return View();
        }

        [HttpPost]
        public IActionResult Contact(EmailSendDto emailSendDto){
            return View();
        }
    }
}
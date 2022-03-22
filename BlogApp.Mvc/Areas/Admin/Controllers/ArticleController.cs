using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using BlogApp.Entities.ComplexTypes;
using BlogApp.Entities.Concrete;
using BlogApp.Entities.Dtos.ArticleDtos;
using BlogApp.Mvc.Areas.Admin.Models;
using BlogApp.Mvc.Helpers.Abstract;
using BlogApp.Services.Abstract;
using BlogApp.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController:BaseController
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;

        public ArticleController(IArticleService articleService, ICategoryService categoryService, UserManager<User> userManager, IMapper mapper,IImageHelper imageHelper):base(userManager,mapper,imageHelper)
        {
            _articleService = articleService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(){
            var articles = await _articleService.GetAllNonDeletedAsync();
            if(articles.ResultStatus == ResultStatus.Success) return View(articles.Data);
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Add(){
            var categories = await _categoryService.GetAllNonDeletedAndActiveAsync();
            if(categories.ResultStatus == ResultStatus.Success){
                return View(new ArticleAddViewModel()
                {
                    Categories = categories.Data.Categories
                });
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Add(ArticleAddViewModel articleAddViewModel){
            if(ModelState.IsValid){
                var articleAddDto = Mapper.Map<ArticleAddDto>(articleAddViewModel);
                var imageResult = await ImageHelper.Upload(articleAddDto.Title, articleAddViewModel.ThumbnailFile, PictureType.Post);
                articleAddDto.Thumbnail = imageResult.Data.FullName;
                var result = await _articleService.AddAsync(articleAddDto, LoggedInUser.UserName, LoggedInUser.Id);
                if(result.ResultStatus == ResultStatus.Success){
                    TempData.Add("SuccessMessage", result.Message);
                    return RedirectToAction("Index", "Article");
                }else{
                    ModelState.AddModelError("", result.Message);
                }
            }
            var categoriesResult = await _categoryService.GetAllNonDeletedAndActiveAsync();
            var articleAddErrorViewModel = new ArticleAddViewModel()
            {
                Categories = categoriesResult.Data.Categories
            };
            return View(articleAddErrorViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int articleId){
            var articleResult = await _articleService.GetUpdateDtoAsync(articleId);
            var categoriesResult = await _categoryService.GetAllNonDeletedAndActiveAsync();

            if(articleResult.ResultStatus == ResultStatus.Success && categoriesResult.ResultStatus == ResultStatus.Success){
                var articleUpdateViewModel = Mapper.Map<ArticleUpdateViewModel>(articleResult.Data);
                articleUpdateViewModel.Categories = categoriesResult.Data.Categories;
                return View(articleUpdateViewModel);
            }else{
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(ArticleUpdateViewModel articleUpdateViewModel){
            if(ModelState.IsValid){
                bool isThumbnailUploaded = false;
                var oldThumbnailFile = articleUpdateViewModel.Thumbnail;
                if(articleUpdateViewModel.ThumbnailFile != null){
                    var uploadResult = await ImageHelper.Upload(articleUpdateViewModel.Title, articleUpdateViewModel.ThumbnailFile, PictureType.Post);
                    if(uploadResult.ResultStatus == ResultStatus.Success){
                        articleUpdateViewModel.Thumbnail = uploadResult.Data.FullName;
                    }else{
                        articleUpdateViewModel.Thumbnail = "Thumbnail/defaultThumbnail.jpg";
                    }
                    if(oldThumbnailFile != "postImages/defaultThumbnail.jpg"){
                        isThumbnailUploaded = true;
                    }
                }
                var articleUpdateDto = Mapper.Map<ArticleUpdateDto>(articleUpdateViewModel);
                var result = await _articleService.UpdateAsync(articleUpdateDto,LoggedInUser.UserName);
                if(result.ResultStatus == ResultStatus.Success){
                    if(isThumbnailUploaded){
                        ImageHelper.Delete(oldThumbnailFile);
                    }
                    TempData.Add("SuccessMessage", result.Message);
                    return RedirectToAction("Index", "Article");
                }else{
                    ModelState.AddModelError("", result.Message);
                }
            }
            var categories = await _categoryService.GetAllNonDeletedAndActiveAsync();
            articleUpdateViewModel.Categories = categories.Data.Categories;
            return View(articleUpdateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int articleId){
            var result = await _articleService.DeleteAsync(articleId);
            var articleResult = JsonSerializer.Serialize(result);
            return Json(articleResult);
        }

        [HttpGet]
        public async Task<JsonResult> GetAll(){
            var articles = await _articleService.GetAllAsync();
            var articlesJson = JsonSerializer.Serialize(articles, new JsonSerializerOptions()
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(articlesJson);
        }

    }
}
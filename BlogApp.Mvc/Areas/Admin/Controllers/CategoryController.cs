using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using BlogApp.Entities.Concrete;
using BlogApp.Entities.Dtos.CategoryDtos;
using BlogApp.Mvc.Areas.Admin.Models;
using BlogApp.Mvc.Helpers.Abstract;
using BlogApp.Services.Abstract;
using BlogApp.Shared.Utilities.Extensions;
using BlogApp.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]
    public class CategoryController:BaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService, UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper):base(userManager,mapper,imageHelper)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(){
            var result = await _categoryService.GetAllNonDeletedAsync();

            if(result.ResultStatus == ResultStatus.Success){
                return View(result.Data);
            }

            return View();
        }
        [HttpGet]
        public IActionResult Add(){
            return PartialView("_CategoryAddPartial");
        }
        [HttpPost]
        public async Task<IActionResult> Add(CategoryAddDto categoryAddDto){
            if(ModelState.IsValid){
                var result = await _categoryService.AddAsync(categoryAddDto,LoggedInUser.UserName);
                if(result.ResultStatus == ResultStatus.Success){
                    var categoryAddAjaxViewModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel(){
                        CategoryDto = result.Data,
                        CategoryAddPartial = await this.RenderViewToStringAsync("_CategoryAddPartial",categoryAddDto)
                    });
                    return Json(categoryAddAjaxViewModel);
                }
            }
            var categoryAddAjaxErrorViewModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel(){
                CategoryAddPartial = await this.RenderViewToStringAsync("_CategoryAddPartial",categoryAddDto)
            });
            return Json(categoryAddAjaxErrorViewModel);
        }
        [HttpGet]
        public async Task<JsonResult> GetAll(){
            var result = await _categoryService.GetAllNonDeletedAsync();
            var categoryListDto = JsonSerializer.Serialize(result.Data,new JsonSerializerOptions(){
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(categoryListDto);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int categoryId){
            var result = await _categoryService.DeleteAsync(categoryId,LoggedInUser.UserName);
            var categoryDto = JsonSerializer.Serialize(result.Data,new JsonSerializerOptions(){
                ReferenceHandler = ReferenceHandler.Preserve
            });

            return Json(categoryDto);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int categoryId){
            var result = await _categoryService.GetUpdateDtoAsync(categoryId);
            if(result.ResultStatus == ResultStatus.Success){
                return PartialView("_CategoryUpdatePartial",result.Data);
            }else{
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateDto categoryUpdateDto){
            if(ModelState.IsValid){
                var result = await _categoryService.UpdateAsync(categoryUpdateDto,LoggedInUser.UserName);
                if(result.ResultStatus == ResultStatus.Success){
                    var categoryUpdateAjaxViewModel = JsonSerializer.Serialize(new CategoryUpdateAjaxViewModel(){
                        CategoryDto = result.Data,
                        CategoryUpdatePartial = await this.RenderViewToStringAsync("_CategoryUpdatePartial",categoryUpdateDto)
                    });
                    return Json(categoryUpdateAjaxViewModel);
                }
            }
            var categoryUpdateAjaxErrorViewModel = JsonSerializer.Serialize(new CategoryUpdateAjaxViewModel(){
                CategoryUpdatePartial = await this.RenderViewToStringAsync("_CategoryUpdatePartial",categoryUpdateDto)
            });
            return Json(categoryUpdateAjaxErrorViewModel);
        }
    }
}
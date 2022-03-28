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
    public class CategoryController:BaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService, UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper):base(userManager,mapper,imageHelper)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Category.Read")]
        public async Task<IActionResult> Index(){
            var result = await _categoryService.GetAllNonDeletedAsync();

            if(result.ResultStatus == ResultStatus.Success){
                return View(result.Data);
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Category.Create")]
        public IActionResult Add(){
            return PartialView("_CategoryAddPartial");
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Category.Create")]
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
        [Authorize(Roles = "SuperAdmin,Category.Read")]
        public async Task<JsonResult> GetAll(){
            var result = await _categoryService.GetAllNonDeletedAsync();
            var categoryListDto = JsonSerializer.Serialize(result.Data,new JsonSerializerOptions(){
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(categoryListDto);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Category.Delete")]
        public async Task<IActionResult> Delete(int categoryId){
            var result = await _categoryService.DeleteAsync(categoryId,LoggedInUser.UserName);
            var categoryDto = JsonSerializer.Serialize(result.Data,new JsonSerializerOptions(){
                ReferenceHandler = ReferenceHandler.Preserve
            });

            return Json(categoryDto);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Category.Update")]
        public async Task<IActionResult> Update(int categoryId){
            var result = await _categoryService.GetUpdateDtoAsync(categoryId);
            if(result.ResultStatus == ResultStatus.Success){
                return PartialView("_CategoryUpdatePartial",result.Data);
            }else{
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Category.Update")]
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

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Category.Read")]
        public async Task<IActionResult> DeletedCategories(){
            var result = await _categoryService.GetAllByDeletedAsync();

            if(result.ResultStatus == ResultStatus.Success){
                return View(result.Data);
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Category.Read")]
        public async Task<JsonResult> GetAllDeletedCategories(){
            var result = await _categoryService.GetAllByDeletedAsync();
            var categoryListDto = JsonSerializer.Serialize(result.Data,new JsonSerializerOptions(){
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(categoryListDto);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Category.Update")]
        public async Task<IActionResult> UndoDelete(int categoryId){
            var result = await _categoryService.UndoDeleteAsync(categoryId,LoggedInUser.UserName);
            var undoDeletedCategory = JsonSerializer.Serialize(result.Data,new JsonSerializerOptions(){
                ReferenceHandler = ReferenceHandler.Preserve
            });

            return Json(undoDeletedCategory);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Category.Delete")]
        public async Task<IActionResult> HardDelete(int categoryId){
            var result = await _categoryService.HardDeleteAsync(categoryId);
            var deletedCategory = JsonSerializer.Serialize(result,new JsonSerializerOptions(){
                ReferenceHandler = ReferenceHandler.Preserve
            });

            return Json(deletedCategory);
        }

    }
}
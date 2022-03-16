using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BlogApp.Entities.Dtos.CategoryDtos;
using BlogApp.Mvc.Areas.Admin.Models;
using BlogApp.Services.Abstract;
using BlogApp.Shared.Utilities.Extensions;
using BlogApp.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Editor")]
    public class CategoryController:Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(){
            var result = await _categoryService.GetAllNonDeleted();

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
                var result = await _categoryService.Add(categoryAddDto,"Selim Cebesoy");
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
            var result = await _categoryService.GetAllNonDeleted();
            var categoryListDto = JsonSerializer.Serialize(result.Data,new JsonSerializerOptions(){
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(categoryListDto);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int categoryId){
            var result = await _categoryService.Delete(categoryId,"Selim Cebesoy");
            var categoryDto = JsonSerializer.Serialize(result.Data,new JsonSerializerOptions(){
                ReferenceHandler = ReferenceHandler.Preserve
            });

            return Json(categoryDto);
        }
        [HttpGet]
        public async Task<IActionResult> Update(int categoryId){
            var result = await _categoryService.GetUpdateDto(categoryId);
            if(result.ResultStatus == ResultStatus.Success){
                return PartialView("_CategoryUpdatePartial",result.Data);
            }else{
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateDto categoryUpdateDto){
            if(ModelState.IsValid){
                var result = await _categoryService.Update(categoryUpdateDto,"Selim Cebesoy");
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
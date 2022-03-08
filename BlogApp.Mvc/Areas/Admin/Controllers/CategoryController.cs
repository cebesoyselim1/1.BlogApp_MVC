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
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController:Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(){
            var result = await _categoryService.GetAll();

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
            var result = await _categoryService.GetAll();
            var categoryListDto = JsonSerializer.Serialize(result.Data,new JsonSerializerOptions(){
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(categoryListDto);
        }
    }
}
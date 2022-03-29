using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using BlogApp.Entities.Dtos.CommentDtos;
using BlogApp.Mvc.Models;
using BlogApp.Services.Abstract;
using BlogApp.Shared.Utilities.Extensions;
using BlogApp.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Mvc.Controllers
{
    public class CommentController:Controller
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<JsonResult> Add(CommentAddDto commentAddDto){
            if(ModelState.IsValid){
                var commentResult = await _commentService.AddAsync(commentAddDto);
                if(commentResult.ResultStatus == ResultStatus.Success){
                    var commentAddAjaxViewModel = JsonSerializer.Serialize(new CommentAddAjaxViewModel()
                    {
                        CommentDto = commentResult.Data,
                        CommentAddPartial = await this.RenderViewToStringAsync("_CommentAddPartial", commentAddDto)
                    }, new JsonSerializerOptions(){
                        ReferenceHandler = ReferenceHandler.Preserve
                    });

                    return Json(commentAddAjaxViewModel);
                }
                ModelState.AddModelError("", commentResult.Message);
            }

            var commentAddAjaxErrorViewModel = JsonSerializer.Serialize(new CommentAddAjaxViewModel()
            {
                CommentAddDto = commentAddDto,
                CommentAddPartial = await this.RenderViewToStringAsync("_CommentAddPartial", commentAddDto)
            });
            return Json(commentAddAjaxErrorViewModel);
        }
    }
}
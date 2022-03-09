using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Dtos.CategoryDtos;

namespace BlogApp.Mvc.Areas.Admin.Models
{
    public class CategoryUpdateAjaxViewModel
    {
        public CategoryDto CategoryDto { get; set; }
        public string CategoryUpdatePartial { get; set; }
        public CategoryUpdateDto CategoryUpdateDto { get; set; }
    }
}
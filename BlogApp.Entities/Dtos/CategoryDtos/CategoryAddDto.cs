using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Entities.Dtos.CategoryDtos
{
    public class CategoryAddDto
    {
        [DisplayName("Category Name")]
        [Required(ErrorMessage ="{0} required.")]
        [MaxLength(70,ErrorMessage ="{0} must have at most {1} characters.")]
        [MinLength(3,ErrorMessage ="{0} must have at least {1} characters.")]
        public string Name { get; set; }
        [DisplayName("Category Description")]
        [MaxLength(200,ErrorMessage ="{0} must have at most {1} characters.")]
        [MinLength(3,ErrorMessage ="{0} must have at least {1} characters.")]
        public string Description { get; set; }
        [DisplayName("Category Notes")]
        [MaxLength(200,ErrorMessage ="{0} must have at most {1} characters.")]
        [MinLength(3,ErrorMessage ="{0} must have at least {1} characters.")]
        public string Note { get; set; }
        [DisplayName("Is Active?")]
        [Required(ErrorMessage = "{0} required.")]
        public bool IsActive { get; set; }

    }
}
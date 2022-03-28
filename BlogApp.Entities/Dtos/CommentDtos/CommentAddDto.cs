using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Entities.Dtos.CommentDtos
{
    public class CommentAddDto
    {
        [DisplayName("Comment")]
        [Required(ErrorMessage = "{0} required.")]
        [MaxLength(1000, ErrorMessage = "{0} must have at most {1} characters.")]
        [MinLength(2, ErrorMessage = "{0} must have at least {1} characters.")]
        public string Text { get; set; }
        [DisplayName("Name")]
        [Required(ErrorMessage = "{0} required.")]
        [MaxLength(50, ErrorMessage = "{0} must have at most {1} characters.")]
        [MinLength(2, ErrorMessage = "{0} must have at least {1} characters.")]
        public string CreatedByName { get; set; }
        [Required(ErrorMessage = "{0} required.")]
        public int ArticleId { get; set; }
    }
}

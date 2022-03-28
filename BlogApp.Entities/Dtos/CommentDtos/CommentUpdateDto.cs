using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Entities.Dtos.CommentDtos
{
    public class CommentUpdateDto
    {
        [Required(ErrorMessage = "{0} required.")]
        public int Id { get; set; }
        [DisplayName("Comment")]
        [Required(ErrorMessage = "{0} required.")]
        [MaxLength(1000, ErrorMessage = "{0} must have at most {1} characters.")]
        [MinLength(2, ErrorMessage = "{0} must have at least {1} characters.")]
        public string Text { get; set; }
        [DisplayName("Is Active?")]
        [Required(ErrorMessage = "{0} required.")]
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "{0} required.")]
        public int ArticleId { get; set; }
    }
}

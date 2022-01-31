using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Concrete;

namespace BlogApp.Entities.Dtos.ArticleDtos
{
    public class ArticleUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [DisplayName("Article Title")]
        [Required(ErrorMessage = "{0} required.")]
        [MinLength(3,ErrorMessage = "{0} has at least {1} characters.")]
        [MaxLength(100,ErrorMessage = "{0} has at most {1} characters.")]
        public string Title { get; set; }
        [DisplayName("Article Content")]
        [Required(ErrorMessage = "{0} required.")]
        public string Content { get; set; }
        [DisplayName("Article Thumbnail")]
        [Required(ErrorMessage = "{0} required.")]
        public string Thumbnail { get; set; }
        [DisplayName("Article Date")]
        [Required(ErrorMessage = "{0} required.")]
        public DateTime Date { get; set; }
        [DisplayName("Article View Count")]
        [Required(ErrorMessage = "{0} required.")]
        public int ViewCount { get; set; }
        [DisplayName("Article Comment Count")]
        [Required(ErrorMessage = "{0} required.")]
        public int CommentCont { get; set; }
        [DisplayName("Article Seo Author")]
        [Required(ErrorMessage = "{0} required.")]
        [MinLength(3,ErrorMessage = "{0} has at least {1} characters.")]
        [MaxLength(100,ErrorMessage = "{0} has at most {1} characters.")]
        public string SeoAuthor { get; set; }
        [DisplayName("Article Seo Description")]
        [Required(ErrorMessage = "{0} required.")]
        [MinLength(3,ErrorMessage = "{0} has at least {1} characters.")]
        [MaxLength(500,ErrorMessage = "{0} has at most {1} characters.")]
        public string SeoDescription { get; set; }
        [DisplayName("Article Seo Tags")]
        [Required(ErrorMessage = "{0} required.")]
        [MinLength(3,ErrorMessage = "{0} has at least {1} characters.")]
        [MaxLength(500,ErrorMessage = "{0} has at most {1} characters.")]
        public string SeoTags { get; set; }
        public Category Category { get; set; }
        [DisplayName("Article Category")]
        [Required(ErrorMessage = "{0} required.")]
        public int CategoryId { get; set; }
        [DisplayName("Article Note")]
        [MaxLength(500,ErrorMessage = "{0} has at most {1} characters.")]
        public string Note { get; set; }
        [DisplayName("Is Active?")]
        [Required(ErrorMessage = "{0} required.")]
        public bool IsActive { get; set; }
        [DisplayName("Is Deleted?")]
        [Required(ErrorMessage = "{0} required.")]
        public bool IsDeleted { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Entities.Dtos.UserDtos
{
    public class UserUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [DisplayName("UserName")]
        [Required(ErrorMessage = "{0} required")]
        [MinLength(8,ErrorMessage = "{0} has at least {1} characters.")]
        [MaxLength(50,ErrorMessage = "{0} has at most {1} caharacters.")]
        public string UserName { get; set; }
        [DisplayName("E-Mail")]
        [Required(ErrorMessage = "{0} required")]
        [MinLength(10,ErrorMessage = "{0} has at least {1} characters.")]
        [MaxLength(100,ErrorMessage = "{0} has at most {1} caharacters.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Telephone Number")]
        [Required(ErrorMessage = "{0} required")]
        [MinLength(13,ErrorMessage = "{0} has at least {1} characters.")]
        [MaxLength(13,ErrorMessage = "{0} has at most {1} caharacters.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [DisplayName("Image File")]
        [DataType(DataType.Upload)]
        public IFormFile PictureFile { get; set; }
        [DisplayName("Image")]
        public string Picture { get; set; }
        [DisplayName("Name")]
        [MaxLength(30, ErrorMessage = "{0} has at most {1} characters.")]
        [MinLength(2, ErrorMessage = "{0} has at least {1} characters.")]
        public string FirstName { get; set; }
        [DisplayName("Surname")]
        [MaxLength(30, ErrorMessage = "{0} has at most {1} characters.")]
        [MinLength(2, ErrorMessage = "{0} has at least {1} characters.")]
        public string LastName { get; set; }
        [DisplayName("About")]
        [MaxLength(1000, ErrorMessage = "{0} has at most {1} characters.")]
        [MinLength(5, ErrorMessage = "{0} has at least {1} characters.")]
        public string About { get; set; }
        [DisplayName("Twitter")]
        [MaxLength(250, ErrorMessage = "{0} has at most {1} characters.")]
        [MinLength(20, ErrorMessage = "{0} has at least {1} characters.")]
        public string TwitterLink { get; set; }
        [DisplayName("Facebook")]
        [MaxLength(250, ErrorMessage = "{0} has at most {1} characters.")]
        [MinLength(20, ErrorMessage = "{0} has at least {1} characters.")]
        public string FacebookLink { get; set; }
        [DisplayName("Instagram")]
        [MaxLength(250, ErrorMessage = "{0} has at most {1} characters.")]
        [MinLength(20, ErrorMessage = "{0} has at least {1} characters.")]
        public string InstagramLink { get; set; }
        [DisplayName("LinkedIn")]
        [MaxLength(250, ErrorMessage = "{0} has at most {1} characters.")]
        [MinLength(20, ErrorMessage = "{0} has at least {1} characters.")]
        public string LinkedInLink { get; set; }
        [DisplayName("Youtube")]
        [MaxLength(250, ErrorMessage = "{0} has at most {1} characters.")]
        [MinLength(20, ErrorMessage = "{0} has at least {1} characters.")]
        public string YoutubeLink { get; set; }
        [DisplayName("GitHub")]
        [MaxLength(250, ErrorMessage = "{0} has at most {1} characters.")]
        [MinLength(20, ErrorMessage = "{0} has at least {1} characters.")]
        public string GitHubLink { get; set; }
        [DisplayName("Website")]
        [MaxLength(250, ErrorMessage = "{0} has at most {1} characters.")]
        [MinLength(20, ErrorMessage = "{0} has at least {1} characters.")]
        public string WebsiteLink { get; set; }
    }
}
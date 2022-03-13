using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Entities.Dtos.UserDtos
{
    public class UserAddDto
    {
        [DisplayName("UserName")]
        [Required(ErrorMessage = "{0} required")]
        [MinLength(8,ErrorMessage = "{0} has at least {1} characters.")]
        [MaxLength(50,ErrorMessage = "{0} has at most {1} caharacters.")]
        public string Username { get; set; }
        [DisplayName("E-Mail")]
        [Required(ErrorMessage = "{0} required")]
        [MinLength(10,ErrorMessage = "{0} has at least {1} characters.")]
        [MaxLength(100,ErrorMessage = "{0} has at most {1} caharacters.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Password")]
        [Required(ErrorMessage = "{0} required")]
        [MinLength(6,ErrorMessage = "{0} has at least {1} characters.")]
        [MaxLength(50,ErrorMessage = "{0} has at most {1} caharacters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Telephone Number")]
        [Required(ErrorMessage = "{0} required")]
        [MinLength(13,ErrorMessage = "{0} has at least {1} characters.")]
        [MaxLength(13,ErrorMessage = "{0} has at most {1} caharacters.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        [DisplayName("Image")]
        [Required(ErrorMessage = "{0} required")]
        [DataType(DataType.Upload)]
        public IFormFile PictureFile { get; set; }
        public string Picture { get; set; }
    }
}
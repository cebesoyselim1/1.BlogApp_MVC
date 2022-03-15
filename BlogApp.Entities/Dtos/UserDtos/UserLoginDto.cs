using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Entities.Dtos.UserDtos
{
    public class UserLoginDto
    {
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
        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }
    }
}
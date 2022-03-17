using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Entities.Dtos.UserDtos
{
    public class UserPasswordChangeDto
    {
        [DisplayName("Current Password")]
        [Required(ErrorMessage = "{0} required")]
        [MinLength(6,ErrorMessage = "{0} has at least {1} characters.")]
        [MaxLength(50,ErrorMessage = "{0} has at most {1} caharacters.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DisplayName("New Password")]
        [Required(ErrorMessage = "{0} required")]
        [MinLength(6,ErrorMessage = "{0} has at least {1} characters.")]
        [MaxLength(50,ErrorMessage = "{0} has at most {1} caharacters.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DisplayName("Re-New Password")]
        [Required(ErrorMessage = "{0} required")]
        [MinLength(6,ErrorMessage = "{0} has at least {1} characters.")]
        [MaxLength(50,ErrorMessage = "{0} has at most {1} caharacters.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword",ErrorMessage = "Passwords didn't match.")]
        public string ReNewPassword { get; set; }
    }
}
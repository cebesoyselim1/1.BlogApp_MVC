using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Entities.Dtos
{
    public class EmailSendDto
    {
        [DisplayName("Name")]
        [Required(ErrorMessage = "{0} required.")]
        [MinLength(5,ErrorMessage ="{0} has at least {1} characters.")]
        [MaxLength(60,ErrorMessage ="{0} has at least {1} characters.")]
        public string Name { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "{0} required.")]
        [MinLength(10,ErrorMessage ="{0} has at least {1} characters.")]
        [MaxLength(100,ErrorMessage ="{0} has at least {1} characters.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Subject")]
        [Required(ErrorMessage = "{0} required.")]
        [MinLength(5,ErrorMessage ="{0} has at least {1} characters.")]
        [MaxLength(125,ErrorMessage ="{0} has at least {1} characters.")]
        public string Subject { get; set; }
        [DisplayName("Message")]
        [Required(ErrorMessage = "{0} required.")]
        [MinLength(20,ErrorMessage ="{0} has at least {1} characters.")]
        [MaxLength(1500,ErrorMessage ="{0} has at least {1} characters.")]
        public string Message { get; set; }
    }
}
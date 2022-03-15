using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Dtos.UserDtos;

namespace BlogApp.Mvc.Areas.Admin.Models
{
    public class UserUpdateAjaxViewModel
    {
        public UserDto UserDto { get; set; }
        public string UserUpdatePartial { get; set; }
        public UserUpdateDto UserUpdateDto { get; set; }
    }
}
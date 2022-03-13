using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Dtos.UserDtos;

namespace BlogApp.Mvc.Areas.Admin.Models
{
    public class UserAddAjaxViewModel
    {
        public UserDto UserDto { get; set; }
        public string UserAddPartial { get; set; }
        public UserAddDto UserAddDto { get; set; }
    }
}
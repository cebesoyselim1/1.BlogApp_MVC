using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Mvc.Areas.Admin.Controllers
{
    public class HomeController:Controller
    {
        [Area("Admin")]
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Index(){
            return View();
        }
    }
}
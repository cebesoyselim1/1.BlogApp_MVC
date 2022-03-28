using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Concrete;
using BlogApp.Entities.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController:Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        [HttpGet]
        public IActionResult Login(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto){
            if(ModelState.IsValid){
                var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
                if(user != null){
                    var result = await _signInManager.PasswordSignInAsync(user,userLoginDto.Password,userLoginDto.RememberMe,false);
                    if(result.Succeeded){
                        return RedirectToAction("Index","Home");
                    }else{
                        ModelState.AddModelError("","Wrong Email or Password");
                    }
                }else{
                    ModelState.AddModelError("","Wrong Email or Password");
                }
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout(){
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home",new {Area = ""});
        }

        [Authorize]
        public IActionResult AccessDenied(){
            return View();
        }
    }
}
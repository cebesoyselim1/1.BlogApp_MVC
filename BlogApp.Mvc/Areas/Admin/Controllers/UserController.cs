using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using BlogApp.Entities.Concrete;
using BlogApp.Entities.Dtos.UserDtos;
using BlogApp.Mvc.Areas.Admin.Models;
using BlogApp.Shared.Utilities.Extensions;
using BlogApp.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController:Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment env, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index(){
            var users = await _userManager.Users.ToListAsync();
            return View(new UserListDto(){
                Users = users,
                ResultStatus = ResultStatus.Success
            });
        }
        
        [HttpGet]
        public IActionResult Login(){
            return View("UserLogin");
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
            return View("UserLogin");
        }
        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout(){
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home",new {Area = ""});
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangeDetails(){
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
            return View(userUpdateDto);
        } 

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeDetails(UserUpdateDto userUpdateDto){
            if(ModelState.IsValid){
                var isPictureUpdateted = false;
                var oldUser = await _userManager.GetUserAsync(HttpContext.User);
                var oldUserPicture = oldUser.Picture;
                if(userUpdateDto.PictureFile != null){
                    userUpdateDto.Picture = await ImageUpload(userUpdateDto.Username,userUpdateDto.PictureFile);
                    if(oldUserPicture != "defaultUser.png"){
                        isPictureUpdateted = true;
                    }
                }

                var updatedUser = _mapper.Map<UserUpdateDto,User>(userUpdateDto,oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);
                if(result.Succeeded){
                    if(isPictureUpdateted){
                        ImageDelete(oldUserPicture);
                    }
                    TempData.Add("SuccessMessage",$"{updatedUser.UserName} has successfully been updated.");
                    return View(userUpdateDto);
                }else{
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                    return View(userUpdateDto);
                }
            }
            return View(userUpdateDto);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(){
            return PartialView("_UserAddPartial");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(UserAddDto userAddDto){
            if(ModelState.IsValid){
                userAddDto.Picture = await ImageUpload(userAddDto.Username,userAddDto.PictureFile);
                var user = _mapper.Map<User>(userAddDto);
                var result = await _userManager.CreateAsync(user,userAddDto.Password);
                if(result.Succeeded){
                    var userAddAjaxViewModel = JsonSerializer.Serialize(new UserAddAjaxViewModel(){
                        UserDto = new UserDto(){
                            User = user,
                            ResultStatus = ResultStatus.Success,
                            Message = $"{user.UserName} has successfully been created."
                        },
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial",userAddDto)
                    });
                    return Json(userAddAjaxViewModel);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                var userAddAjaxErrorViewModelInner = JsonSerializer.Serialize(new UserAddAjaxViewModel(){
                    UserAddDto = userAddDto,
                    UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial",userAddDto)
                });
                return Json(userAddAjaxErrorViewModelInner);
            }
            var userAddAjaxErrorViewModel = JsonSerializer.Serialize(new UserAddAjaxViewModel(){
                UserAddDto = userAddDto,
                UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial",userAddDto)
            });
            return Json(userAddAjaxErrorViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers(){
            var users = await _userManager.Users.ToListAsync();
            var userListDto = JsonSerializer.Serialize(new UserListDto(){
                Users = users,
                ResultStatus = ResultStatus.Success
            },new JsonSerializerOptions(){
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(userListDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int userId){
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.DeleteAsync(user);
            if(result.Succeeded){
                var userDto = JsonSerializer.Serialize(new UserDto(){
                    User = user,
                    ResultStatus = ResultStatus.Success,
                    Message = $"{user.UserName} has successfully been deleted."
                });
                return Json(userDto);
            }else{
                var errorMessages = "";
                foreach(var error in result.Errors){
                    errorMessages += $"*{error}\n";
                }
                var userDto = JsonSerializer.Serialize(new UserDto(){
                    User = user,
                    ResultStatus = ResultStatus.Error,
                    Message = $"{errorMessages}"
                });
                return Json(errorMessages);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int userId){
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
            return PartialView("_UserUpdatePartial",userUpdateDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto){
            if(ModelState.IsValid){
                var isPictureUploaded = false;
                var oldUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userUpdateDto.Id);
                var oldPicture = oldUser.Picture;
                if(userUpdateDto.PictureFile != null){
                    userUpdateDto.Picture = await ImageUpload(userUpdateDto.Username,userUpdateDto.PictureFile);
                    isPictureUploaded = true;
                }
                var user = _mapper.Map<UserUpdateDto,User>(userUpdateDto,oldUser);
                var result = await _userManager.UpdateAsync(user);
                if(result.Succeeded){
                    if(isPictureUploaded){
                        ImageDelete(oldPicture);
                    }
                    var userUpdateAjaxViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel(){
                        UserDto = new UserDto(){
                            User = user,
                            ResultStatus = ResultStatus.Success,
                            Message = $"{user.UserName} has successfully been updated."
                        },
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial",userUpdateDto)
                    });
                    return Json(userUpdateAjaxViewModel);
                }else{
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                    var userUpdateAjaxErrorViewModelInner = JsonSerializer.Serialize(new UserUpdateAjaxViewModel(){
                        UserUpdateDto = userUpdateDto,
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial",userUpdateDto)
                    });
                    return Json(userUpdateAjaxErrorViewModelInner);
                }
            }
            var userUpdateAjaxErrorViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel(){
                UserUpdateDto = userUpdateDto,
                UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial",userUpdateDto)
            });
            return Json(userUpdateAjaxErrorViewModel);
        }
        
        [Authorize(Roles = "Admin,Editor")]
        public async Task<string> ImageUpload(string userName, IFormFile pictureFile){

            var wwwRoot = _env.WebRootPath;
            //var fileName = Path.GetFileNameWithoutExtension(userAddDto.PictureFile.FileName);
            var fileExtension = Path.GetExtension(pictureFile.FileName);
            DateTime dateTime = new DateTime();
            var newFileName = $"{userName}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}";
            var path = Path.Combine($@"{wwwRoot}\img",newFileName);
            await using(var stream = new FileStream(path,FileMode.Create)){
                await pictureFile.CopyToAsync(stream);
            }
            return newFileName;
        }
        
        [Authorize(Roles = "Admin,Editor")]
        public bool ImageDelete(string imageName){
            var wwwRoot = _env.WebRootPath;
            var path = Path.Combine($@"{wwwRoot}\img",imageName);
            if(System.IO.File.Exists(path)){
                System.IO.File.Delete(path);
                return true;
            }
            return false;
        }

        public IActionResult AccessDenied(){
            return View();
        }
    }
}
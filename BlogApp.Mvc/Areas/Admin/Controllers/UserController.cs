using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using BlogApp.Entities.ComplexTypes;
using BlogApp.Entities.Concrete;
using BlogApp.Entities.Dtos.UserDtos;
using BlogApp.Mvc.Areas.Admin.Models;
using BlogApp.Mvc.Helpers.Abstract;
using BlogApp.Mvc.Helpers.Concrete;
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
        private readonly IImageHelper _imageHelper;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment env, IMapper mapper, IImageHelper imageHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
            _mapper = mapper;
            _imageHelper = imageHelper;
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
                    var imageUpdateDto = await _imageHelper.Upload(userUpdateDto.Username,userUpdateDto.PictureFile,PictureType.User);
                    userUpdateDto.Picture = imageUpdateDto.ResultStatus == ResultStatus.Success ? imageUpdateDto.Data.FullName : oldUserPicture;
                    if(oldUserPicture != "UsersImage/defaultUser.png"){
                        isPictureUpdateted = true;
                    }
                }

                var updatedUser = _mapper.Map<UserUpdateDto,User>(userUpdateDto,oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);
                if(result.Succeeded){
                    if(isPictureUpdateted){
                        _imageHelper.Delete(oldUserPicture);
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
        [Authorize]
        public IActionResult PasswordChange(){
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PasswordChange(UserPasswordChangeDto userPasswordChangeDto){
            if(ModelState.IsValid){
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var IsValidate = await _userManager.CheckPasswordAsync(user,userPasswordChangeDto.CurrentPassword);
                if(IsValidate){
                    var result = await _userManager.ChangePasswordAsync(user,userPasswordChangeDto.CurrentPassword,userPasswordChangeDto.NewPassword);
                    if(result.Succeeded){
                        await _userManager.UpdateSecurityStampAsync(user);
                        await _signInManager.SignOutAsync();
                        await _signInManager.PasswordSignInAsync(user,userPasswordChangeDto.NewPassword,true,false);
                        TempData.Add("SuccessMessage","Password has successfully been updated.");
                        return View();
                    }else{
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("",error.Description);
                        }
                        return View(userPasswordChangeDto);
                    }
                }else{
                    ModelState.AddModelError("","Current password you entered is wrong.");
                    return View(userPasswordChangeDto);
                }
            }
            return View(userPasswordChangeDto);
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
                var imageUpdateDto = await _imageHelper.Upload(userAddDto.Username,userAddDto.PictureFile,PictureType.User);
                userAddDto.Picture = imageUpdateDto.ResultStatus == ResultStatus.Success ? imageUpdateDto.Data.FullName : "UserImages/defaultUser.png";
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
                    var imageUpdateDto = await _imageHelper.Upload(userUpdateDto.Username,userUpdateDto.PictureFile,PictureType.User);
                    userUpdateDto.Picture = imageUpdateDto.ResultStatus == ResultStatus.Success ? imageUpdateDto.Data.FullName : "UserImages/defaultUser.png";
                    isPictureUploaded = true;
                }
                var user = _mapper.Map<UserUpdateDto,User>(userUpdateDto,oldUser);
                var result = await _userManager.UpdateAsync(user);
                if(result.Succeeded){
                    if(isPictureUploaded){
                        _imageHelper.Delete(oldPicture);
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

        public IActionResult AccessDenied(){
            return View();
        }
    }
}
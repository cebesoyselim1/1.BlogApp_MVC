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
using NToastNotify;

namespace BlogApp.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController:BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _env;
        private readonly IToastNotification _toastNotification;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment env, IMapper mapper, IImageHelper imageHelper, IToastNotification toastNotification):base(userManager,mapper,imageHelper)
        {
            _signInManager = signInManager;
            _env = env;
            _toastNotification = toastNotification;
        }

        [Authorize(Roles = "SuperAdmin,User.Read")]
        public async Task<IActionResult> Index(){
            var users = await UserManager.Users.ToListAsync();
            return View(new UserListDto(){
                Users = users,
                ResultStatus = ResultStatus.Success
            });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ChangeDetails(){
            var user = await UserManager.GetUserAsync(HttpContext.User);
            var userUpdateDto = Mapper.Map<UserUpdateDto>(user);
            return View(userUpdateDto);
        } 

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeDetails(UserUpdateDto userUpdateDto){
            if(ModelState.IsValid){
                var isPictureUpdateted = false;
                var oldUser = await UserManager.GetUserAsync(HttpContext.User);
                var oldUserPicture = oldUser.Picture;
                if(userUpdateDto.PictureFile != null){
                    var imageUpdateDto = await ImageHelper.Upload(userUpdateDto.UserName,userUpdateDto.PictureFile,PictureType.User);
                    userUpdateDto.Picture = imageUpdateDto.ResultStatus == ResultStatus.Success ? imageUpdateDto.Data.FullName : oldUserPicture;
                    if(oldUserPicture != "UsersImage/defaultUser.png"){
                        isPictureUpdateted = true;
                    }
                }

                var updatedUser = Mapper.Map<UserUpdateDto,User>(userUpdateDto,oldUser);
                var result = await UserManager.UpdateAsync(updatedUser);
                if(result.Succeeded){
                    if(isPictureUpdateted){
                        ImageHelper.Delete(oldUserPicture);
                    }
                    _toastNotification.AddSuccessToastMessage($"{updatedUser.UserName} has successfully been updated.");
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
                var user = await UserManager.GetUserAsync(HttpContext.User);
                var IsValidate = await UserManager.CheckPasswordAsync(user,userPasswordChangeDto.CurrentPassword);
                if(IsValidate){
                    var result = await UserManager.ChangePasswordAsync(user,userPasswordChangeDto.CurrentPassword,userPasswordChangeDto.NewPassword);
                    if(result.Succeeded){
                        await UserManager.UpdateSecurityStampAsync(user);
                        await _signInManager.SignOutAsync();
                        await _signInManager.PasswordSignInAsync(user,userPasswordChangeDto.NewPassword,true,false);
                        _toastNotification.AddSuccessToastMessage("Password has successfully been updated.");
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
        [Authorize(Roles = "SuperAdmin,User.Create")]
        public IActionResult Add(){
            return PartialView("_UserAddPartial");
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,User.Create")]
        public async Task<IActionResult> Add(UserAddDto userAddDto){
            if(ModelState.IsValid){
                var imageUpdateDto = await ImageHelper.Upload(userAddDto.UserName,userAddDto.PictureFile,PictureType.User);
                userAddDto.Picture = imageUpdateDto.ResultStatus == ResultStatus.Success ? imageUpdateDto.Data.FullName : "UserImages/defaultUser.png";
                var user = Mapper.Map<User>(userAddDto);
                var result = await UserManager.CreateAsync(user,userAddDto.Password);
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
        [Authorize(Roles = "SuperAdmin,User.Read")]
        public async Task<IActionResult> GetAllUsers(){
            var users = await UserManager.Users.ToListAsync();
            var userListDto = JsonSerializer.Serialize(new UserListDto(){
                Users = users,
                ResultStatus = ResultStatus.Success
            },new JsonSerializerOptions(){
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(userListDto);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,User.Delete")]
        public async Task<IActionResult> Delete(int userId){
            var user = await UserManager.FindByIdAsync(userId.ToString());
            var result = await UserManager.DeleteAsync(user);
            if(result.Succeeded){
                if(user.Picture != "UserImages/defaultUser.png"){
                    ImageHelper.Delete(user.Picture);
                }
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

        [Authorize(Roles = "SuperAdmin,User.Read")]
        [HttpGet]
        public async Task<PartialViewResult> GetDetail(int userId)
        {
            var user = await UserManager.Users.SingleOrDefaultAsync(u=>u.Id==userId);
            return PartialView("_GetDetailPartial", new UserDto{User = user});
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,User.Update")]
        public async Task<IActionResult> Update(int userId){
            var user = await UserManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var userUpdateDto = Mapper.Map<UserUpdateDto>(user);
            return PartialView("_UserUpdatePartial",userUpdateDto);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,User.Update")]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto){
            if(ModelState.IsValid){
                var isPictureUploaded = false;
                var oldUser = await UserManager.Users.FirstOrDefaultAsync(u => u.Id == userUpdateDto.Id);
                var oldPicture = oldUser.Picture;
                if(userUpdateDto.PictureFile != null){
                    var imageUpdateDto = await ImageHelper.Upload(userUpdateDto.UserName,userUpdateDto.PictureFile,PictureType.User);
                    userUpdateDto.Picture = imageUpdateDto.ResultStatus == ResultStatus.Success ? imageUpdateDto.Data.FullName : "UserImages/defaultUser.png";
                    isPictureUploaded = true;
                }
                var user = Mapper.Map<UserUpdateDto,User>(userUpdateDto,oldUser);
                var result = await UserManager.UpdateAsync(user);
                if(result.Succeeded){
                    if(isPictureUploaded){
                        ImageHelper.Delete(oldPicture);
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

    }
}
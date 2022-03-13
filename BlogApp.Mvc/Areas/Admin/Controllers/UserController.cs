using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using BlogApp.Entities.Concrete;
using BlogApp.Entities.Dtos.UserDtos;
using BlogApp.Mvc.Areas.Admin.Models;
using BlogApp.Shared.Utilities.Extensions;
using BlogApp.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController:Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, IWebHostEnvironment env, IMapper mapper)
        {
            _userManager = userManager;
            _env = env;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(){
            var users = await _userManager.Users.ToListAsync();
            return View(new UserListDto(){
                Users = users,
                ResultStatus = ResultStatus.Success
            });
        }

        [HttpGet]
        public IActionResult Add(){
            return PartialView("_UserAddPartial");
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserAddDto userAddDto){
            Console.WriteLine(userAddDto.PhoneNumber);
            if(ModelState.IsValid){
                userAddDto.Picture = await ImageUpload(userAddDto);
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

        public async Task<string> ImageUpload(UserAddDto userAddDto){
            var wwwRoot = _env.WebRootPath;
            //var fileName = Path.GetFileNameWithoutExtension(userAddDto.PictureFile.FileName);
            var fileExtension = Path.GetExtension(userAddDto.PictureFile.FileName);
            DateTime dateTime = new DateTime();
            var newFileName = $"{userAddDto.Username}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}";
            var path = Path.Combine($@"{wwwRoot}\img",newFileName);
            await using(var stream = new FileStream(path,FileMode.Create)){
                Console.WriteLine("sa");
                await userAddDto.PictureFile.CopyToAsync(stream);
            }
            return newFileName;
        }

    }
}
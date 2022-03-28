using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using BlogApp.Entities.Concrete;
using BlogApp.Entities.Dtos.RoleDtos;
using BlogApp.Entities.Dtos.UserDtos;
using BlogApp.Mvc.Areas.Admin.Models;
using BlogApp.Mvc.Helpers.Abstract;
using BlogApp.Mvc.Helpers.Concrete;
using BlogApp.Shared.Utilities.Extensions;
using BlogApp.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController:BaseController
    {
        private readonly RoleManager<Role> _roleManager;
        public RoleController(RoleManager<Role> roleManager, UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper):base(userManager,mapper,imageHelper)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Roles.Read")]    
        public async Task<IActionResult> Index(){
            var roles = await _roleManager.Roles.ToListAsync();
            return View(new RoleListDto(){
                ResultStatus = ResultStatus.Success,
                Roles = roles
            });
        }

        [HttpGet]
        [Authorize(Roles = ("SuperAdmin,Roles.Read"))]
        public async Task<IActionResult> GetAllRoles(){
            var roles = await _roleManager.Roles.ToListAsync();
            var rolseJson = JsonSerializer.Serialize(new RoleListDto()
            {
                ResultStatus = ResultStatus.Success,
                Message = $"Roles has successfully been brought",
                Roles = roles
            });
            return Json(rolseJson);
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,User.Update")]
        public async Task<IActionResult> Assign(int userId){
            var user = await UserManager.FindByIdAsync((userId).ToString());
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await UserManager.GetRolesAsync(user);
            var userRoleAssignDto = new UserRoleAssignDto()
            {
                UserId = user.Id,
                UserName = user.UserName,
            };
            foreach (var role in roles)
            {
                var roleAssignDto = new RoleAssignDto()
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    HasRole = userRoles.Contains(role.Name)
                };
                userRoleAssignDto.RoleAssignDtos.Add(roleAssignDto);
            }

            return PartialView("_RoleAssignPartial", userRoleAssignDto);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,User.Update")]
        public async Task<IActionResult> Assign(UserRoleAssignDto userRoleAssignDto) {
            if(ModelState.IsValid){
                var user = await UserManager.Users.SingleOrDefaultAsync(u => u.Id == userRoleAssignDto.UserId);
                foreach (var roleAssignDto in userRoleAssignDto.RoleAssignDtos)
                {
                    
                    if(roleAssignDto.HasRole){
                        if(!await UserManager.IsInRoleAsync(user, roleAssignDto.RoleName)){
                            await UserManager.AddToRoleAsync(user, roleAssignDto.RoleName);
                        }
                    }else{
                        if(await UserManager.IsInRoleAsync(user, roleAssignDto.RoleName)){
                            await UserManager.RemoveFromRoleAsync(user, roleAssignDto.RoleName);
                        }
                    }
                }
                var userRoleAssignAjaxViewModel = JsonSerializer.Serialize(new UserRoleAssignAjaxViewModel()
                {
                    UserDto = new UserDto()
                    {
                        ResultStatus = ResultStatus.Success,
                        Message = $"{user.UserName}'s roles has been updated.",
                        User = user
                    },
                    RoleAssignPartial = await this.RenderViewToStringAsync("_RoleAssignPartial",userRoleAssignDto)
                });
                return Json(userRoleAssignAjaxViewModel);
            }else{
                var userRoleAssignAjaxErrorViewModel = JsonSerializer.Serialize(new UserRoleAssignAjaxViewModel()
                {
                    UserRoleAssignDto = userRoleAssignDto,
                    RoleAssignPartial = await this.RenderViewToStringAsync("_RoleAssignPartial",userRoleAssignDto)
                });
                return Json(userRoleAssignAjaxErrorViewModel);
            }
        }

    }
}
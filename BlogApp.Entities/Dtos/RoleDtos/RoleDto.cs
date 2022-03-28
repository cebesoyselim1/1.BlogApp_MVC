using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Concrete;
using BlogApp.Shared.Entities.Abstract;

namespace BlogApp.Entities.Dtos.RoleDtos
{
    public class RoleDto:DtoGetBase
    {
        public Role Role { get; set; }
    }
}
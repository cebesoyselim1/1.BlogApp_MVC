using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Shared.Entities.Abstract;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Entities.Concrete
{
    public class Role:IdentityRole<int>
    {
    }
}
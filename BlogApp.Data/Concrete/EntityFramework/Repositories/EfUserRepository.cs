using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entities.Concrete;
using BlogApp.Shared.Data.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EntityFramework.Repositories
{
    public class EfUserRepository : EfEntityRepositoryBase<User>, IUserRepository
    {
        public EfUserRepository(DbContext context) : base(context)
        {
        }
    }
}
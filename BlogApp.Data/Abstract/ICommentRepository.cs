using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Concrete;
using BlogApp.Shared.Data.Abstact;

namespace BlogApp.Data.Abstract
{
    public interface ICommentRepository:IEntityRepository<Comment>
    {
        
    }
}
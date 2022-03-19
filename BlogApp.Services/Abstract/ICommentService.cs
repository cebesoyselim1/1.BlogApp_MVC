using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Shared.Utilities.Results.Abstract;

namespace BlogApp.Services.Abstract
{
    public interface ICommentService
    {
        public Task<IDataResult<int>> Count();
        public Task<IDataResult<int>> CountByNonDeleted();
    }
}
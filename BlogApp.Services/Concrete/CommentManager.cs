using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Services.Abstract;
using BlogApp.Services.Utilities;
using BlogApp.Shared.Utilities.Results.Abstract;
using BlogApp.Shared.Utilities.Results.ComplexTypes;
using BlogApp.Shared.Utilities.Results.Concrete;

namespace BlogApp.Services.Concrete
{
    public class CommentManager : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IDataResult<int>> Count(){
            var commentsCount = await _unitOfWork.Comments.CountAsync();

            if(commentsCount > -1){
                return new DataResult<int>(ResultStatus.Success,Messages.Comment.Count(commentsCount),commentsCount);
            }

            return new DataResult<int>(ResultStatus.Error,Messages.Comment.NotFound(isPlural:true),-1);
        }
        
        public async Task<IDataResult<int>> CountByNonDeleted(){
            var commentsCount = await _unitOfWork.Comments.CountAsync(c => !c.IsDeleted);

            if(commentsCount > -1){
                return new DataResult<int>(ResultStatus.Success,Messages.Comment.Count(commentsCount),commentsCount);
            }

            return new DataResult<int>(ResultStatus.Error,Messages.Comment.NotFound(isPlural:true),-1);
        }
    }
}
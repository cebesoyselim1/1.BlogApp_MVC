using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogApp.Data.Abstract;
using BlogApp.Entities.Concrete;
using BlogApp.Entities.Dtos.CommentDtos;
using BlogApp.Services.Abstract;
using BlogApp.Services.Utilities;
using BlogApp.Shared.Utilities.Results.Abstract;
using BlogApp.Shared.Utilities.Results.ComplexTypes;
using BlogApp.Shared.Utilities.Results.Concrete;

namespace BlogApp.Services.Concrete
{
    public class CommentManager : ManagerBase, ICommentService
    {
        public CommentManager(IUnitOfWork unitOfWork, IMapper mapper):base(unitOfWork,mapper){}

        public async Task<IDataResult<CommentDto>> GetAsync(int commentId)
        {
            var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId);
            if (comment != null)
            {
                return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.Get(isPlural:false, comment.Id), new CommentDto
                {
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Comment.Get(isPlural:false, commentId:commentId),
                    Comment = comment
                });
            }
            return new DataResult<CommentDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: false), new CommentDto
            {
                ResultStatus = ResultStatus.Error,
                Message = Messages.Comment.NotFound(isPlural:false),
                Comment = null
            });
        }

        public async Task<IDataResult<CommentUpdateDto>> GetCommentUpdateDtoAsync(int commentId)
        {
            var result = await UnitOfWork.Comments.AnyAsync(c => c.Id == commentId);
            if (result)
            {
                var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId);
                var commentUpdateDto = Mapper.Map<CommentUpdateDto>(comment);
                return new DataResult<CommentUpdateDto>(ResultStatus.Success, Messages.Comment.Update(comment.Text), commentUpdateDto);
            }
            else
            {
                return new DataResult<CommentUpdateDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: false), null);
            }
        }

        public async Task<IDataResult<CommentListDto>> GetAllAsync()
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(null,c => c.Article);
            if (comments.Count > -1)
            {
                return new DataResult<CommentListDto>(ResultStatus.Success, Messages.Comment.Get(isPlural:false), new CommentListDto
                {
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Comment.Get(isPlural:true),
                    Comments = comments
                });
            }
            return new DataResult<CommentListDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: true), new CommentListDto
            {
                ResultStatus = ResultStatus.Error,
                Message = Messages.Comment.NotFound(isPlural: true),
                Comments = null
            });
        }

        public async Task<IDataResult<CommentListDto>> GetAllByDeletedAsync()
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(c=>c.IsDeleted,c => c.Article);
            if (comments.Count > -1)
            {
                return new DataResult<CommentListDto>(ResultStatus.Success, Messages.Comment.Get(isPlural:true), new CommentListDto
                {
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Comment.Get(isPlural:true),
                    Comments = comments
                });
            }
            return new DataResult<CommentListDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: true), new CommentListDto
            {
                ResultStatus = ResultStatus.Error,
                Message = Messages.Comment.NotFound(isPlural:true),
                Comments = null
            });
        }

        public async Task<IDataResult<CommentListDto>> GetAllByNonDeletedAsync()
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(c => !c.IsDeleted,c => c.Article);
            if (comments.Count > -1)
            {
                return new DataResult<CommentListDto>(ResultStatus.Success, Messages.Comment.Get(isPlural:true), new CommentListDto
                {
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Comment.Get(isPlural:true),
                    Comments = comments
                });
            }
            return new DataResult<CommentListDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: true), new CommentListDto
            {
                ResultStatus = ResultStatus.Error,
                Message = Messages.Comment.NotFound(isPlural:true),
                Comments = null
            });
        }

        public async Task<IDataResult<CommentListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(c => !c.IsDeleted&&c.IsActive,c => c.Article);
            if (comments.Count > -1)
            {
                return new DataResult<CommentListDto>(ResultStatus.Success, Messages.Comment.Get(isPlural:true), new CommentListDto
                {
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Comment.Get(isPlural:true),
                    Comments = comments
                });
            }
            return new DataResult<CommentListDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: true), new CommentListDto
            {
                ResultStatus = ResultStatus.Error,
                Message = Messages.Comment.NotFound(isPlural:true),
                Comments = null
            });
        }

        public async Task<IDataResult<CommentDto>> AddAsync(CommentAddDto commentAddDto)
        {
            var comment = Mapper.Map<Comment>(commentAddDto);
            var addedComment = await UnitOfWork.Comments.AddAsync(comment);
            await UnitOfWork.SaveAsync();
            return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.Add(commentAddDto.Text), new CommentDto
            {
                ResultStatus = ResultStatus.Success,
                Message = Messages.Comment.Add(commentAddDto.Text),
                Comment = addedComment
            });
        }

        public async Task<IDataResult<CommentDto>> UpdateAsync(CommentUpdateDto commentUpdateDto, string modifiedByName)
        {
            var oldComment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentUpdateDto.Id);
            var comment = Mapper.Map<CommentUpdateDto, Comment>(commentUpdateDto, oldComment);
            comment.ModifiedByName = modifiedByName;
            var updatedComment = await UnitOfWork.Comments.UpdateAsync(comment);
            updatedComment.Article = await UnitOfWork.Articles.GetAsync(a => a.Id == updatedComment.ArticleId);
            await UnitOfWork.SaveAsync();
            return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.Update(comment.Text), new CommentDto
            {
                ResultStatus = ResultStatus.Success,
                Message = Messages.Comment.Update(comment.Text),
                Comment = updatedComment
            });
        }

        public async Task<IDataResult<CommentDto>> DeleteAsync(int commentId, string modifiedByName)
        {
            var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId);
            if (comment != null)
            {
                comment.IsActive = false;
                comment.IsDeleted = true;
                comment.ModifiedByName = modifiedByName;
                comment.ModifiedDate = DateTime.Now;
                var deletedComment = await UnitOfWork.Comments.UpdateAsync(comment);
                await UnitOfWork.SaveAsync();
                return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.Delete(deletedComment.Text), new CommentDto
                {
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Comment.Delete(comment.Text),
                    Comment = deletedComment,
                });
            }
            return new DataResult<CommentDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: false), new CommentDto
            {
                ResultStatus = ResultStatus.Success,
                Message = Messages.Comment.NotFound(isPlural:false),
                Comment = null
            });
        }

        public async Task<IResult> HardDeleteAsync(int commentId)
        {
            var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId);
            if (comment != null)
            {
                await UnitOfWork.Comments.DeleteAsync(comment);
                await UnitOfWork.SaveAsync();
                return new Result(ResultStatus.Success, Messages.Comment.HardDelete(comment.CreatedByName));
            }
            return new Result(ResultStatus.Error, Messages.Comment.NotFound(isPlural: false));
        }

        public async Task<IDataResult<int>> CountAsync(){
            var commentsCount = await UnitOfWork.Comments.CountAsync();

            if(commentsCount > -1){
                return new DataResult<int>(ResultStatus.Success, Messages.Comment.Count(commentsCount), commentsCount);
            }

            return new DataResult<int>(ResultStatus.Error, Messages.Comment.NotFound(isPlural:true), -1);
        }
        
        public async Task<IDataResult<int>> CountByNonDeletedAsync(){
            var commentsCount = await UnitOfWork.Comments.CountAsync(c => !c.IsDeleted);

            if(commentsCount > -1){
                return new DataResult<int>(ResultStatus.Success, Messages.Comment.Count(commentsCount), commentsCount);
            }

            return new DataResult<int>(ResultStatus.Error, Messages.Comment.NotFound(isPlural:true), -1);
        }

        public async Task<IDataResult<CommentDto>> ApproveAsync(int commentId, string modifiedByName)
        {
            var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId, c => c.Article);
            if(comment != null){
                comment.ModifiedByName = modifiedByName;
                comment.ModifiedDate = DateTime.Now;
                comment.IsActive = true;
                var updatedComment = await UnitOfWork.Comments.UpdateAsync(comment);
                await UnitOfWork.SaveAsync();
                return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.Approve(comment.Text), new CommentDto()
                {
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Comment.Approve(comment.Text),
                    Comment = updatedComment
                });
            }
            return new DataResult<CommentDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: false), new CommentDto()
            {
                ResultStatus = ResultStatus.Error,
                Message = Messages.Comment.NotFound(isPlural:false),
                Comment = null
            });
        }

        public async Task<IDataResult<CommentDto>> UndoDeleteAsync(int commentId, string modifiedByName)
        {
            var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId);
            if(comment != null){
                comment.IsActive = true;
                comment.IsDeleted = false;
                comment.ModifiedDate = DateTime.Now;
                comment.ModifiedByName = modifiedByName;
                var updatedComment = await UnitOfWork.Comments.UpdateAsync(comment);
                await UnitOfWork.SaveAsync();

                return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.UndoDelete(comment.Text), new CommentDto()
                {
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Comment.UndoDelete(comment.Text),
                    Comment = comment
                });
            }
            return new DataResult<CommentDto>(ResultStatus.Error, Messages.Comment.NotFound(isPlural: false), new CommentDto()
            {
                ResultStatus = ResultStatus.Error,
                Message = Messages.Comment.NotFound(isPlural: false),
                Comment = null
            });
        }
    }
}
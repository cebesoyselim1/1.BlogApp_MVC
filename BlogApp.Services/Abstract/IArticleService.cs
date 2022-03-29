using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.ComplexTypes;
using BlogApp.Entities.Dtos.ArticleDtos;
using BlogApp.Shared.Utilities.Results.Abstract;

namespace BlogApp.Services.Abstract
{
    public interface IArticleService
    {
        Task<IDataResult<ArticleDto>> GetAsync(int articleId);
        Task<IDataResult<ArticleUpdateDto>> GetUpdateDtoAsync(int articleId);
        Task<IDataResult<ArticleListDto>> GetAllAsync();
        Task<IDataResult<ArticleListDto>> GetAllByDeletedAsync();
        Task<IDataResult<ArticleListDto>> GetAllNonDeletedAsync();
        Task<IDataResult<ArticleListDto>> GetAllNonDeletedAndActiveAsync();
        Task<IDataResult<ArticleListDto>> GetAllByViewCountAsync(bool isAscending, int? takeSize);
        Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId);
        Task<IDataResult<ArticleListDto>> GetAllByPagingAsync(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false);
        Task<IDataResult<ArticleListDto>> GetAllByUserIdOnFilter(int userId, FilterBy filterBy, OrderBy orderBy, bool isAscending, int takeSize, int categoryId, DateTime startAt, DateTime endAt, int minViewCount, int maxViewCount, int minCommentCount, int maxCommentCount);
        Task<IDataResult<ArticleListDto>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false);
        Task<IResult> IncreaseViewCountAsync(int articleId);
        Task<IDataResult<ArticleDto>> AddAsync(ArticleAddDto articleAddDto, string createdName, int userId);
        Task<IDataResult<ArticleDto>> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedName);
        Task<IResult> DeleteAsync(int articleId, string modifiedByName);
        Task<IResult> HardDeleteAsync(int articleId);
        Task<IDataResult<ArticleDto>> UndoDeleteAsync(int articleId,string modifiedByName);
        public Task<IDataResult<int>> CountAsync();
        public Task<IDataResult<int>> CountByNonDeletedAsync();
    }
}
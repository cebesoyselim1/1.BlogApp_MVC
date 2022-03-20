using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Dtos.ArticleDtos;
using BlogApp.Shared.Utilities.Results.Abstract;

namespace BlogApp.Services.Abstract
{
    public interface IArticleService
    {
        Task<IDataResult<ArticleDto>> GetAsync(int articleId);
        Task<IDataResult<ArticleListDto>> GetAllAsync();
        Task<IDataResult<ArticleListDto>> GetAllNonDeletedAsync();
        Task<IDataResult<ArticleListDto>> GetAllNonDeletedAndActiveAsync();
        Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId);
        Task<IDataResult<ArticleDto>> AddAsync(ArticleAddDto articleAddDto, string createdName);
        Task<IDataResult<ArticleDto>> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedName);
        Task<IResult> DeleteAsync(int articleId);
        Task<IResult> HardDeleteAsync(int articleId);
        public Task<IDataResult<int>> CountAsync();
        public Task<IDataResult<int>> CountByNonDeletedAsync();
    }
}
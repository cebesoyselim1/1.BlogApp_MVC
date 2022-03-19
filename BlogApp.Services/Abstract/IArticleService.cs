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
        Task<IDataResult<ArticleDto>> Get(int articleId);
        Task<IDataResult<ArticleListDto>> GetAll();
        Task<IDataResult<ArticleListDto>> GetAllNonDeleted();
        Task<IDataResult<ArticleListDto>> GetAllNonDeletedAndActive();
        Task<IDataResult<ArticleListDto>> GetAllByCategory(int categoryId);
        Task<IDataResult<ArticleDto>> Add(ArticleAddDto articleAddDto, string createdName);
        Task<IDataResult<ArticleDto>> Update(ArticleUpdateDto articleUpdateDto, string modifiedName);
        Task<IResult> Delete(int articleId);
        Task<IResult> HardDelete(int articleId);
        public Task<IDataResult<int>> Count();
        public Task<IDataResult<int>> CountByNonDeleted();
    }
}
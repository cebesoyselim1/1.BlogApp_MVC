using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using BlogApp.Data.Abstract;
using BlogApp.Entities.ComplexTypes;
using BlogApp.Entities.Concrete;
using BlogApp.Entities.Dtos.ArticleDtos;
using BlogApp.Services.Abstract;
using BlogApp.Services.Utilities;
using BlogApp.Shared.Utilities.Results.Abstract;
using BlogApp.Shared.Utilities.Results.ComplexTypes;
using BlogApp.Shared.Utilities.Results.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Services.Concrete
{
    public class ArticleManager : ManagerBase, IArticleService
    {
        private readonly UserManager<User> _userManager;
        public ArticleManager(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager):base(unitOfWork,mapper){
            _userManager = userManager;
        }

        public async Task<IDataResult<ArticleDto>> AddAsync(ArticleAddDto articleAddDto, string createdByName, int userId)
        {
            var article = Mapper.Map<Article>(articleAddDto);
            article.CreatedByName = createdByName;
            article.ModifiedByName = createdByName;
            article.UserId = userId;
            var addingArticle = await UnitOfWork.Articles.AddAsync(article);
            await UnitOfWork.SaveAsync();

            return new DataResult<ArticleDto>(ResultStatus.Success, Messages.Article.Add(articleAddDto.Title) ,new ArticleDto(){
                ResultStatus = ResultStatus.Success,
                Article = addingArticle,
                Message = Messages.Article.Add(article.Title) 
            });
        }

        public async Task<IResult> DeleteAsync(int articleId, string modifiedByName)
        {
            var article = await UnitOfWork.Articles.GetAsync(a => a.Id == articleId, a => a.User, a => a.User);

            if(article != null){
                article.Comments = await UnitOfWork.Comments.GetAllAsync(c => c.ArticleId == article.Id && c.IsActive && !c.IsDeleted);
                article.IsActive = false;
                article.IsDeleted = true;
                article.ModifiedDate = DateTime.Now;
                article.ModifiedByName = modifiedByName;

                await UnitOfWork.Articles.UpdateAsync(article);
                await UnitOfWork.SaveAsync();

                return new Result(ResultStatus.Success, Messages.Article.Delete(article.Title));
            }

            return new Result(ResultStatus.Error, Messages.Article.NotFound(isPlural:false));
        }

        public async Task<IDataResult<ArticleDto>> GetAsync(int articleId)
        {
            var article = await UnitOfWork.Articles.GetAsync(a => a.Id == articleId, a => a.User, a => a.Category);

            if(article != null){
                article.Comments = await UnitOfWork.Comments.GetAllAsync(c => c.ArticleId == articleId && c.IsActive);
                return new DataResult<ArticleDto>(ResultStatus.Success, Messages.Article.Get(isPlural:false,article.Title), new ArticleDto(){
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Article.Get(isPlural:false,article.Title),
                    Article = article
                });
            }

            return new DataResult<ArticleDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural:false), new ArticleDto(){
                ResultStatus = ResultStatus.Error,
                Message = Messages.Article.NotFound(isPlural:false),
                Article = null
            });
        }

        public async Task<IDataResult<ArticleListDto>> GetAllAsync()
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(null,a => a.Category, a => a.User, a => a.Comments);

            if(articles.Count >= 0){
                return new DataResult<ArticleListDto>(ResultStatus.Success, Messages.Article.Get(isPlural:true), new ArticleListDto(){
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Article.Get(isPlural:true),
                    Articles = articles
                });
            }

            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural:true), new ArticleListDto(){
                ResultStatus = ResultStatus.Error, 
                Message = Messages.Article.NotFound(isPlural:true),
                Articles = null
            });
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId)
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(c => c.CategoryId == categoryId, c => c.Category, c => c.User, c => c.Comments);

            if(articles.Count >= 0){
                return new DataResult<ArticleListDto>(ResultStatus.Success, Messages.Article.Get(isPlural:true), new ArticleListDto(){
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Article.Get(isPlural:true),
                    Articles = articles
                });
            }

            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural:true), new ArticleListDto(){
                ResultStatus = ResultStatus.Error,
                Message = Messages.Article.Get(isPlural:true),
                Articles = null
            });
        }

        public async Task<IDataResult<ArticleListDto>> GetAllNonDeletedAsync()
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(c => !c.IsDeleted, c => c.Category, c => c.User, c => c.Comments);

            if(articles.Count >= 0){
                return new DataResult<ArticleListDto>(ResultStatus.Success, Messages.Article.Get(isPlural:true), new ArticleListDto(){
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Article.Get(isPlural:true),
                    Articles = articles
                });
            }

            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural:true), new ArticleListDto(){
                ResultStatus = ResultStatus.Error,
                Message = Messages.Article.NotFound(isPlural:true),
                Articles = null
            });
        }

        public async Task<IDataResult<ArticleListDto>> GetAllNonDeletedAndActiveAsync()
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(c => !c.IsDeleted && c.IsActive, c => c.Category, c => c.User, c => c.Comments);

            if(articles.Count >= 0){
                return new DataResult<ArticleListDto>(ResultStatus.Success, Messages.Article.Get(isPlural:true), new ArticleListDto(){
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Article.Get(isPlural:true),
                    Articles = articles
                });
            }

            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural:true), new ArticleListDto(){
                ResultStatus = ResultStatus.Error,
                Message = Messages.Article.NotFound(isPlural:true),
                Articles = null
            });
        }

        public async Task<IResult> HardDeleteAsync(int articleId)
        {
            var article = await UnitOfWork.Articles.GetAsync(a => a.Id == articleId, a => a.Category, a => a.User, a => a.Comments);

            if(article != null){
                await UnitOfWork.Articles.DeleteAsync(article);
                await UnitOfWork.SaveAsync();

                return new Result(ResultStatus.Success, Messages.Article.HardDelete(article.Title));
            }

            return new Result(ResultStatus.Error, Messages.Article.NotFound(isPlural:false));
        }

        public async Task<IDataResult<ArticleDto>> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName)
        {
            var oldArticle = await UnitOfWork.Articles.GetAsync(a => a.Id == articleUpdateDto.Id);
            var article = Mapper.Map<ArticleUpdateDto,Article>(articleUpdateDto,oldArticle);

            if(article != null){
                article.ModifiedByName = modifiedByName;

                var updatingArticle = await UnitOfWork.Articles.UpdateAsync(article);
                await UnitOfWork.SaveAsync();

                return new DataResult<ArticleDto>(ResultStatus.Success, Messages.Article.Update(article.Title), new ArticleDto(){
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Article.Update(article.Title),
                    Article = updatingArticle
                });
            }

            return new DataResult<ArticleDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural:false), new ArticleDto(){
                ResultStatus=ResultStatus.Error,
                Message=Messages.Article.NotFound(isPlural:false),
                Article=null
            });
        }

        public async Task<IDataResult<int>> CountAsync(){
            var commentCount = await UnitOfWork.Comments.CountAsync();

            if(commentCount > -1){
                return new DataResult<int>(ResultStatus.Success,Messages.Comment.Count(commentCount), commentCount);
            }

            return new DataResult<int>(ResultStatus.Error, Messages.Comment.NotFound(isPlural:true), -1);
        }
        
        public async Task<IDataResult<int>> CountByNonDeletedAsync(){
            var commentCount = await UnitOfWork.Comments.CountAsync(c => !c.IsDeleted);

            if(commentCount > -1){
                return new DataResult<int>(ResultStatus.Success, Messages.Comment.Count(commentCount), commentCount);
            }

            return new DataResult<int>(ResultStatus.Error, Messages.Comment.NotFound(isPlural:true), -1);
        }

        public async Task<IDataResult<ArticleUpdateDto>> GetUpdateDtoAsync(int articleId)
        {
            var article = await UnitOfWork.Articles.GetAsync(a => a.Id == articleId);

            if(article != null){
                var articleUpdateDto = Mapper.Map<ArticleUpdateDto>(article);
                return new DataResult<ArticleUpdateDto>(ResultStatus.Success, Messages.Article.Get(isPlural:false,article.Title), articleUpdateDto);
            }

            return new DataResult<ArticleUpdateDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural:false), null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByDeletedAsync()
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(a => a.IsDeleted, a => a.Category);
            if(articles != null){
                return new DataResult<ArticleListDto>(ResultStatus.Success, Messages.Article.Get(isPlural: true), new ArticleListDto()
                {
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Article.Get(isPlural: true),
                    Articles = articles
                });
            }

            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural: true), new ArticleListDto()
            {
                ResultStatus = ResultStatus.Error,
                Message = Messages.Article.NotFound(isPlural: true),
                Articles = null
            });
        }

        public async Task<IDataResult<ArticleDto>> UndoDeleteAsync(int articleId, string modifiedByName)
        {
            var article = await UnitOfWork.Articles.GetAsync(a => a.Id == articleId);

            if(article != null){
                article.IsActive = true;
                article.IsDeleted = false;
                article.ModifiedDate = DateTime.Now;
                article.ModifiedByName = modifiedByName;

                var updatedArticle = UnitOfWork.Articles.UpdateAsync(article);
                await UnitOfWork.SaveAsync();

                return new DataResult<ArticleDto>(ResultStatus.Success, Messages.Article.UndoDelete(article.Title), new ArticleDto()
                {
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Article.UndoDelete(article.Title),
                    Article = article
                });
            }

            return new DataResult<ArticleDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural: false), new ArticleDto()
            {
                ResultStatus = ResultStatus.Error,
                Message = Messages.Article.NotFound(isPlural: false),
                Article = null
            });
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByViewCountAsync(bool isAscending, int? takeSize)
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted, a => a.Category, a => a.User);

            var sortedArticles = isAscending ? articles.OrderBy(a => a.ViewCount) : articles.OrderByDescending(a => a.ViewCount);

            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto()
            {
                ResultStatus = ResultStatus.Success,
                Articles = takeSize == null ? sortedArticles.ToList() : sortedArticles.Take(takeSize.Value).ToList()
            });
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByPagingAsync(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;
            var articles = categoryId == null ?
            await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted, a => a.Category, a => a.User) :
            await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted && a.CategoryId == categoryId, a => a.Category, a => a.User);
            
            var sortedArticles = isAscending ?
            articles.OrderBy(a => a.ViewCount).Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList() : 
            articles.OrderByDescending(a => a.ViewCount).Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();

            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto()
            {
                ResultStatus = ResultStatus.Success,
                CategoryId = categoryId == null ? null : categoryId,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                Articles = sortedArticles,
                isAscending = isAscending
            });

        }

        public async Task<IDataResult<ArticleListDto>> SearchAsync(string keyword, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            if(string.IsNullOrWhiteSpace(keyword)){
                pageSize = pageSize > 20 ? 20 : pageSize;
                var articles = await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted, a => a.Category, a => a.User);

                var sortedArticles = isAscending ?
                articles.OrderBy(a => a.ViewCount).Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList() : 
                articles.OrderByDescending(a => a.ViewCount).Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();

                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto()
                {
                    ResultStatus = ResultStatus.Success,
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalCount = articles.Count,
                    Articles = sortedArticles,
                    isAscending = isAscending
                });
            }

            var searchedArticles = await UnitOfWork.Articles.SearchAsync(new List<Expression<Func<Article, bool>>>()
            {
                a => a.Title.Contains(keyword),
                a => a.Category.Name.Contains(keyword),
                a => a.SeoDescription.Contains(keyword),
                a => a.SeoTags.Contains(keyword)
            }, a => a.Category, a => a.User);

            var searchedAndSortedArticles = isAscending ?
            searchedArticles.OrderBy(a => a.ViewCount).Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList() : 
            searchedArticles.OrderByDescending(a => a.ViewCount).Skip(pageSize * (currentPage - 1)).Take(pageSize).ToList();

            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto()
            {
                ResultStatus = ResultStatus.Success,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = searchedArticles.Count,
                Articles = searchedAndSortedArticles,
                isAscending = isAscending
            });
        }

        public async Task<IResult> IncreaseViewCountAsync(int articleId)
        {
            var article = await UnitOfWork.Articles.GetAsync(a => a.Id == articleId);
            if(article == null){
                return new Result(ResultStatus.Error, Messages.Article.NotFound(isPlural: false));
            }
            article.ViewCount += 1;
            await UnitOfWork.Articles.UpdateAsync(article);
            await UnitOfWork.SaveAsync();
            return new Result(ResultStatus.Success, Messages.Article.IncreaseViewCount(article.Title));
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByUserIdOnFilter(int userId, FilterBy filterBy, OrderBy orderBy, bool isAscending, int takeSize, int categoryId, DateTime startAt, DateTime endAt, int minViewCount, int maxViewCount, int minCommentCount, int maxCommentCount)
        {
            var anyUser = await _userManager.Users.AnyAsync(u => u.Id == userId);

            if(!anyUser){
                return new DataResult<ArticleListDto>(ResultStatus.Error, $"User whose id is {userId} not found.", null);
            }

            var userArticles = await UnitOfWork.Articles.GetAllAsync(a => a.UserId == userId && a.IsActive && !a.IsDeleted);
            List<Article> sordtedArticles = new List<Article>();
            switch(filterBy){
                case FilterBy.Category:
                    switch(orderBy){
                        case OrderBy.Date:
                            sordtedArticles = isAscending ? userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderBy(a => a.Date).ToList() : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.Date).ToList();
                            break;
                        case OrderBy.ViewCount:
                            sordtedArticles = isAscending ? userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderBy(a => a.ViewCount).ToList() : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.ViewCount).ToList();
                            break;
                        case OrderBy.CommentCount:
                            sordtedArticles = isAscending ? userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderBy(a => a.CommentCount).ToList() : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize).OrderByDescending(a => a.CommentCount).ToList();
                            break;
                    }
                    break;
                case FilterBy.Date:
                    switch(orderBy){
                        case OrderBy.Date:
                            sordtedArticles = isAscending ? userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize).OrderBy(a => a.Date).ToList() : userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize).OrderByDescending(a => a.Date).ToList();
                            break;
                        case OrderBy.ViewCount:
                            sordtedArticles = isAscending ? userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize).OrderBy(a => a.ViewCount).ToList() : userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize).OrderByDescending(a => a.ViewCount).ToList();
                            break;
                        case OrderBy.CommentCount:
                            sordtedArticles = isAscending ? userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize).OrderBy(a => a.CommentCount).ToList() : userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize).OrderByDescending(a => a.CommentCount).ToList();
                            break;
                    }
                    break;
                case FilterBy.ViewCount:
                    switch(orderBy){
                        case OrderBy.Date:
                            sordtedArticles = isAscending ? userArticles.Where(a => a.ViewCount >= minViewCount && a.ViewCount <= maxViewCount).Take(takeSize).OrderBy(a => a.Date).ToList() : userArticles.Where(a => a.ViewCount >= minViewCount && a.ViewCount <= maxViewCount).Take(takeSize).OrderByDescending(a => a.Date).ToList();
                            break;
                        case OrderBy.ViewCount:
                            sordtedArticles = isAscending ? userArticles.Where(a => a.ViewCount >= minViewCount && a.ViewCount <= maxViewCount).Take(takeSize).OrderBy(a => a.ViewCount).ToList() : userArticles.Where(a => a.ViewCount >= minViewCount && a.ViewCount <= maxViewCount).Take(takeSize).OrderByDescending(a => a.ViewCount).ToList();
                            break;
                        case OrderBy.CommentCount:
                            sordtedArticles = isAscending ? userArticles.Where(a => a.ViewCount >= minViewCount && a.ViewCount <= maxViewCount).Take(takeSize).OrderBy(a => a.CommentCount).ToList() : userArticles.Where(a => a.ViewCount >= minViewCount && a.ViewCount <= maxViewCount).Take(takeSize).OrderByDescending(a => a.CommentCount).ToList();
                            break;
                    }
                    break;
                case FilterBy.CommentCount:
                    switch(orderBy){
                        case OrderBy.Date:
                            sordtedArticles = isAscending ? userArticles.Where(a => a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount).Take(takeSize).OrderBy(a => a.Date).ToList() : userArticles.Where(a => a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount).Take(takeSize).OrderByDescending(a => a.Date).ToList();
                            break;
                        case OrderBy.ViewCount:
                            sordtedArticles = isAscending ? userArticles.Where(a => a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount).Take(takeSize).OrderBy(a => a.ViewCount).ToList() : userArticles.Where(a => a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount).Take(takeSize).OrderByDescending(a => a.ViewCount).ToList();
                            break;
                        case OrderBy.CommentCount:
                            sordtedArticles = isAscending ? userArticles.Where(a => a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount).Take(takeSize).OrderBy(a => a.CommentCount).ToList() : userArticles.Where(a => a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount).Take(takeSize).OrderByDescending(a => a.CommentCount).ToList();
                            break;
                    }
                    break;
            }
            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto(){
                Articles = sordtedArticles
            });
        }
    }
}
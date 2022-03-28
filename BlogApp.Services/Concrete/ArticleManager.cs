using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogApp.Data.Abstract;
using BlogApp.Entities.Concrete;
using BlogApp.Entities.Dtos.ArticleDtos;
using BlogApp.Services.Abstract;
using BlogApp.Services.Utilities;
using BlogApp.Shared.Utilities.Results.Abstract;
using BlogApp.Shared.Utilities.Results.ComplexTypes;
using BlogApp.Shared.Utilities.Results.Concrete;

namespace BlogApp.Services.Concrete
{
    public class ArticleManager : ManagerBase, IArticleService
    {
        public ArticleManager(IUnitOfWork unitOfWork, IMapper mapper):base(unitOfWork,mapper){ }

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
            var article = await UnitOfWork.Articles.GetAsync(a => a.Id == articleId);

            if(article != null){
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
            var article = await UnitOfWork.Articles.GetAsync(c => c.Id == articleId);

            if(article != null){
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
            var articles = await UnitOfWork.Articles.GetAllAsync(c => !c.IsDeleted && !c.IsActive, c => c.Category, c => c.User, c => c.Comments);

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
    }
}
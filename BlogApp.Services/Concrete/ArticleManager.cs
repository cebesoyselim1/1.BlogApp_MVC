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
    public class ArticleManager : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ArticleManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IDataResult<ArticleDto>> AddAsync(ArticleAddDto articleAddDto, string createdByName, int userId)
        {
            var article = _mapper.Map<Article>(articleAddDto);
            article.CreatedByName = createdByName;
            article.ModifiedByName = createdByName;
            article.UserId = userId;
            var addingArticle = await _unitOfWork.Articles.AddAsync(article);
            await _unitOfWork.SaveAsync();

            return new DataResult<ArticleDto>(ResultStatus.Success,Messages.Article.Add(articleAddDto.Title),new ArticleDto(){ResultStatus = ResultStatus.Success, Article = addingArticle, Message = $"{article.Title} has successfully been created." });
        }

        public async Task<IResult> DeleteAsync(int articleId)
        {
            var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId, a => a.Category, a => a.User, a => a.Comments);

            if(article != null){
                article.IsDeleted = true;

                await _unitOfWork.Articles.UpdateAsync(article);
                await _unitOfWork.SaveAsync();

                return new Result(ResultStatus.Success,Messages.Article.Delete(article.Title));
            }

            return new Result(ResultStatus.Error,Messages.Article.NotFound(isPlural:false));
        }

        public async Task<IDataResult<ArticleDto>> GetAsync(int articleId)
        {
            var article = await _unitOfWork.Articles.GetAsync(c => c.Id == articleId);

            if(article != null){
                return new DataResult<ArticleDto>(ResultStatus.Success,Messages.Article.Get(isPlural:false,article.Title),new ArticleDto(){Article = article,ResultStatus = ResultStatus.Success});
            }

            return new DataResult<ArticleDto>(ResultStatus.Error,Messages.Article.NotFound(isPlural:false),new ArticleDto(){Article = null,ResultStatus = ResultStatus.Error, Message = "Article not found."});
        }

        public async Task<IDataResult<ArticleListDto>> GetAllAsync()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(null,a => a.Category, a => a.User, a => a.Comments);

            if(articles.Count >= 0){
                return new DataResult<ArticleListDto>(ResultStatus.Success,Messages.Article.Get(isPlural:true),new ArticleListDto(){ResultStatus = ResultStatus.Success, Articles = articles});
            }

            return new DataResult<ArticleListDto>(ResultStatus.Error,Messages.Article.NotFound(isPlural:true), new ArticleListDto(){ResultStatus = ResultStatus.Error, Articles = null, Message = "Article not found."});
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId)
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(c => c.CategoryId == categoryId, c => c.Category, c => c.User, c => c.Comments);

            if(articles.Count >= 0){
                return new DataResult<ArticleListDto>(ResultStatus.Success,Messages.Article.Get(isPlural:true),new ArticleListDto(){ResultStatus = ResultStatus.Success, Articles = articles});
            }

            return new DataResult<ArticleListDto>(ResultStatus.Error,Messages.Article.NotFound(isPlural:true),new ArticleListDto(){ResultStatus = ResultStatus.Error, Articles = null, Message = "Article not found."});
        }

        public async Task<IDataResult<ArticleListDto>> GetAllNonDeletedAsync()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(c => !c.IsDeleted, c => c.Category, c => c.User, c => c.Comments);

            if(articles.Count >= 0){
                return new DataResult<ArticleListDto>(ResultStatus.Success,Messages.Article.Get(isPlural:true),new ArticleListDto(){ResultStatus = ResultStatus.Success, Articles = articles});
            }

            return new DataResult<ArticleListDto>(ResultStatus.Error,Messages.Article.NotFound(isPlural:true),new ArticleListDto(){ResultStatus = ResultStatus.Error, Articles = null, Message = "Article not found."});
        }

        public async Task<IDataResult<ArticleListDto>> GetAllNonDeletedAndActiveAsync()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(c => !c.IsDeleted && !c.IsActive, c => c.Category, c => c.User, c => c.Comments);

            if(articles.Count >= 0){
                return new DataResult<ArticleListDto>(ResultStatus.Success,Messages.Article.Get(isPlural:true),new ArticleListDto(){ResultStatus = ResultStatus.Success, Articles = articles});
            }

            return new DataResult<ArticleListDto>(ResultStatus.Error,Messages.Article.NotFound(isPlural:true),new ArticleListDto(){ResultStatus = ResultStatus.Error, Articles = null, Message = "Article not found."});
        }

        public async Task<IResult> HardDeleteAsync(int articleId)
        {
            var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId, a => a.Category, a => a.User, a => a.Comments);

            if(article != null){
                await _unitOfWork.Articles.DeleteAsync(article);
                await _unitOfWork.SaveAsync();

                return new Result(ResultStatus.Success, Messages.Article.HardDelete(article.Title));
            }

            return new Result(ResultStatus.Error, Messages.Article.NotFound(isPlural:false));
        }

        public async Task<IDataResult<ArticleDto>> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName)
        {
            var oldArticle = await _unitOfWork.Articles.GetAsync(a => a.Id == articleUpdateDto.Id);
            var article = _mapper.Map<ArticleUpdateDto,Article>(articleUpdateDto,oldArticle);

            if(article != null){
                article.ModifiedByName = modifiedByName;

                var updatingArticle = await _unitOfWork.Articles.UpdateAsync(article);
                await _unitOfWork.SaveAsync();

                return new DataResult<ArticleDto>(ResultStatus.Success,Messages.Article.Update(article.Title),new ArticleDto(){ResultStatus = ResultStatus.Success, Article = updatingArticle, Message = Messages.Article.Update(article.Title)});
            }

            return new DataResult<ArticleDto>(ResultStatus.Error,Messages.Article.NotFound(isPlural:false),new ArticleDto(){ResultStatus=ResultStatus.Error,Article=null,Message=Messages.Article.NotFound(isPlural:false)});
        }

        public async Task<IDataResult<int>> CountAsync(){
            var commentCount = await _unitOfWork.Comments.CountAsync();

            if(commentCount > -1){
                return new DataResult<int>(ResultStatus.Success,Messages.Comment.Count(commentCount),commentCount);
            }

            return new DataResult<int>(ResultStatus.Error,Messages.Comment.NotFound(isPlural:true),-1);
        }
        
        public async Task<IDataResult<int>> CountByNonDeletedAsync(){
            var commentCount = await _unitOfWork.Comments.CountAsync(c => !c.IsDeleted);

            if(commentCount > -1){
                return new DataResult<int>(ResultStatus.Success,Messages.Comment.Count(commentCount),commentCount);
            }

            return new DataResult<int>(ResultStatus.Error,Messages.Comment.NotFound(isPlural:true),-1);
        }

        public async Task<IDataResult<ArticleUpdateDto>> GetUpdateDtoAsync(int articleId)
        {
            var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId);

            if(article != null){
                var articleUpdateDto = _mapper.Map<ArticleUpdateDto>(article);
                return new DataResult<ArticleUpdateDto>(ResultStatus.Success, Messages.Article.Get(isPlural:false,article.Title), articleUpdateDto);
            }

            return new DataResult<ArticleUpdateDto>(ResultStatus.Error, Messages.Article.NotFound(isPlural:false), null);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogApp.Data.Abstract;
using BlogApp.Entities.Concrete;
using BlogApp.Entities.Dtos.ArticleDtos;
using BlogApp.Services.Abstract;
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
        public async Task<IResult> Add(ArticleAddDto articleAddDto, string createdName)
        {
            var article = _mapper.Map<Article>(articleAddDto);
            article.CreatedName = createdName;
            article.ModifiedName = createdName;

            await _unitOfWork.Articles.AddAsync(article);
            await _unitOfWork.SaveAsync();

            return new Result(ResultStatus.Success,$"{article.Title} has successfully been created.");
        }

        public async Task<IResult> Delete(int articleId)
        {
            var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId, a => a.Category, a => a.User, a => a.Comments);

            if(article != null){
                article.IsDeleted = true;

                await _unitOfWork.Articles.UpdateAsync(article);
                await _unitOfWork.SaveAsync();

                return new Result(ResultStatus.Success,$"{article.Title} has successfully been deleted.");
            }

            return new Result(ResultStatus.Error,"Article not found");
        }

        public async Task<IDataResult<ArticleDto>> Get(int articleId)
        {
            var article = await _unitOfWork.Articles.GetAsync(c => c.Id == articleId);

            if(article != null){
                return new DataResult<ArticleDto>(ResultStatus.Success,$"{article.Title} has successfully been brought.",new ArticleDto(){Article = article,ResultStatus = ResultStatus.Success});
            }

            return new DataResult<ArticleDto>(ResultStatus.Error,"Article not found",new ArticleDto(){Article = null,ResultStatus = ResultStatus.Error});
        }

        public async Task<IDataResult<ArticleListDto>> GetAll()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(null,a => a.Category, a => a.User, a => a.Comments);

            if(articles.Count >= 0){
                return new DataResult<ArticleListDto>(ResultStatus.Success,"Articles has successfully been brought.",new ArticleListDto(){ResultStatus = ResultStatus.Success, Articles = articles});
            }

            return new DataResult<ArticleListDto>(ResultStatus.Error,"Article not found.", new ArticleListDto(){ResultStatus = ResultStatus.Error, Articles = null});
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByCategory(int categoryId)
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(c => c.CategoryId == categoryId, c => c.Category, c => c.User, c => c.Comments);

            if(articles.Count >= 0){
                return new DataResult<ArticleListDto>(ResultStatus.Success,"Articles has successfully been brought.",new ArticleListDto(){ResultStatus = ResultStatus.Success, Articles = articles});
            }

            return new DataResult<ArticleListDto>(ResultStatus.Error,"Article not found.",new ArticleListDto(){ResultStatus = ResultStatus.Error, Articles = null});
        }

        public async Task<IDataResult<ArticleListDto>> GetAllNonDeleted()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(c => !c.IsDeleted, c => c.Category, c => c.User, c => c.Comments);

            if(articles.Count >= 0){
                return new DataResult<ArticleListDto>(ResultStatus.Success,"Articles has successfully been brought.",new ArticleListDto(){ResultStatus = ResultStatus.Success, Articles = articles});
            }

            return new DataResult<ArticleListDto>(ResultStatus.Error,"Article not found.",new ArticleListDto(){ResultStatus = ResultStatus.Error, Articles = null});
        }

        public async Task<IDataResult<ArticleListDto>> GetAllNonDeletedAndActive()
        {
            var articles = await _unitOfWork.Articles.GetAllAsync(c => !c.IsDeleted && !c.IsActive, c => c.Category, c => c.User, c => c.Comments);

            if(articles.Count >= 0){
                return new DataResult<ArticleListDto>(ResultStatus.Success,"Articles has successfully been brought.",new ArticleListDto(){ResultStatus = ResultStatus.Success, Articles = articles});
            }

            return new DataResult<ArticleListDto>(ResultStatus.Error,"Article not found.",new ArticleListDto(){ResultStatus = ResultStatus.Error, Articles = null});
        }

        public async Task<IResult> HardDelete(int articleId)
        {
            var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId, a => a.Category, a => a.User, a => a.Comments);

            if(article != null){
                await _unitOfWork.Articles.DeleteAsync(article);
                await _unitOfWork.SaveAsync();

                return new Result(ResultStatus.Success, $"{article.Title} has successfully been deleted from database.");
            }

            return new Result(ResultStatus.Error, "Article not found.");
        }

        public async Task<IResult> Update(ArticleUpdateDto articleUpdateDto, string modifiedName)
        {
            var article = _mapper.Map<Article>(articleUpdateDto);

            if(article != null){
                article.ModifiedName = modifiedName;

                await _unitOfWork.Articles.UpdateAsync(article);
                await _unitOfWork.SaveAsync();

                return new Result(ResultStatus.Success,$"{article.Title} has successfully been updated.");
            }

            return new Result(ResultStatus.Error,"Article not found.");
        }
    }
}
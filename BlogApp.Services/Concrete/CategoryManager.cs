using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogApp.Data.Abstract;
using BlogApp.Entities.Concrete;
using BlogApp.Entities.Dtos.CategoryDtos;
using BlogApp.Services.Abstract;
using BlogApp.Services.Utilities;
using BlogApp.Shared.Utilities.Results.Abstract;
using BlogApp.Shared.Utilities.Results.ComplexTypes;
using BlogApp.Shared.Utilities.Results.Concrete;

namespace BlogApp.Services.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork =  unitOfWork;
            _mapper = mapper;
        }

        public async Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto categoryAddDto, string createdByName)
        {
            var category = _mapper.Map<Category>(categoryAddDto);
            
            category.CreatedByName = createdByName;
            category.ModifiedByName = createdByName;

            var addingCategory =  await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveAsync();

            return new DataResult<CategoryDto>(ResultStatus.Success,Messages.Category.Add(category.Name),new CategoryDto(){
                ResultStatus = ResultStatus.Success,
                Category= addingCategory,
                Message = Messages.Category.Add(category.Name)
            });
        }

        public async Task<IDataResult<CategoryDto>> DeleteAsync(int categoryId, string modifiedByName)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);

            if(category != null){
                category.ModifiedByName = modifiedByName;
                category.ModifiedDate = DateTime.Now;
                category.IsDeleted = true;

                await _unitOfWork.Categories.UpdateAsync(category);
                await _unitOfWork.SaveAsync();

                return new DataResult<CategoryDto>(ResultStatus.Success,Messages.Category.Delete(category.Name),new CategoryDto(){
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Category.Delete(category.Name),
                    Category = category
                });
            }
            
            return new DataResult<CategoryDto>(ResultStatus.Error,Messages.Category.NotFound(isPlural:false),new CategoryDto(){
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.NotFound(isPlural:false),
                Category = null
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(null, c => c.Articles);

            if(categories.Count() >= 0){
                return new DataResult<CategoryListDto>(ResultStatus.Success,Messages.Category.Get(isPlural:true),new CategoryListDto(){
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Category.Get(isPlural:true),
                    Categories = categories
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error,Messages.Category.NotFound(isPlural:true),new CategoryListDto(){
                ResultStatus = ResultStatus.Error, 
                Message = Messages.Category.NotFound(isPlural:true),
                Categories = null
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllNonDeletedAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted, c => c.Articles);

            if(categories.Count() >= 0){
                return new DataResult<CategoryListDto>(ResultStatus.Success,Messages.Category.Get(isPlural:true),new CategoryListDto(){
                    ResultStatus = ResultStatus.Success, 
                    Message = Messages.Category.Get(isPlural:true),
                    Categories = categories
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error,Messages.Category.NotFound(isPlural:true),new CategoryListDto(){
                ResultStatus = ResultStatus.Error, 
                Message = Messages.Category.NotFound(isPlural:true),
                Categories = null
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllNonDeletedAndActiveAsync(){
            var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted && c.IsActive, c => c.Articles);

            if(categories.Count >= 0){
                return new DataResult<CategoryListDto>(ResultStatus.Success,Messages.Category.Get(isPlural:true),new CategoryListDto(){
                    ResultStatus = ResultStatus.Success, 
                    Message = Messages.Category.Get(isPlural:true),
                    Categories = categories
                });
            }

            return new DataResult<CategoryListDto>(ResultStatus.Error,Messages.Category.NotFound(isPlural:true),new CategoryListDto(){
                ResultStatus = ResultStatus.Error, 
                Message = Messages.Category.NotFound(isPlural:true),
                Categories = null
            });
        }

        public async Task<IDataResult<CategoryDto>> GetAsync(int categoryId)
        {  
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);

            if(category != null){
                return new DataResult<CategoryDto>(ResultStatus.Success,Messages.Category.Get(isPlural:false,category.Name),new CategoryDto(){
                    ResultStatus = ResultStatus.Success, 
                    Message = Messages.Category.Get(isPlural:false,category.Name),
                    Category = category
                });
            }

            return new DataResult<CategoryDto>(ResultStatus.Error,Messages.Category.NotFound(isPlural:false), new CategoryDto(){
                ResultStatus = ResultStatus.Error, 
                Message = Messages.Category.NotFound(isPlural:false),
                Category = null
            });
        }

        /// <summary>
        /// With given category id it returns CategoryUpdateDto.
        /// </summary>
        /// <param name="categoryId">Id which is integer and greater than 0.</param>
        /// <returns>It returns asynchronous IDataResult.</returns>
        public async Task<IDataResult<CategoryUpdateDto>> GetUpdateDtoAsync(int categoryId){
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            
            if(category != null){
                var categoryUpdateDto = _mapper.Map<CategoryUpdateDto>(category);
                return new DataResult<CategoryUpdateDto>(ResultStatus.Success,Messages.Category.Update(category.Name),categoryUpdateDto);
            }
            return new DataResult<CategoryUpdateDto>(ResultStatus.Error,Messages.Category.NotFound(isPlural:false),null);
        }

        public async Task<IResult> HardDeleteAsync(int categoryId)
        {
             var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);

             if(category != null){
                 await _unitOfWork.Categories.DeleteAsync(category);
                 await _unitOfWork.SaveAsync();

                 return new Result(ResultStatus.Success,Messages.Category.HardDelete(category.Name));
             }

             return new Result(ResultStatus.Error,Messages.Category.NotFound(isPlural:false));
        }

        public async Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            var oldCategory = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryUpdateDto.Id);
            var category = _mapper.Map<CategoryUpdateDto,Category>(categoryUpdateDto,oldCategory);

            if(category != null){
                category.ModifiedByName = modifiedByName;
                
                var updatingCategory = await _unitOfWork.Categories.UpdateAsync(category);
                await _unitOfWork.SaveAsync();

                return new DataResult<CategoryDto>(ResultStatus.Success,Messages.Category.Update(category.Name),new CategoryDto(){
                    ResultStatus = ResultStatus.Success, 
                    Message = Messages.Category.Update(category.Name),
                    Category = updatingCategory
                });
            }
            
            return new DataResult<CategoryDto>(ResultStatus.Success,Messages.Category.NotFound(isPlural:false),new CategoryDto(){
                ResultStatus = ResultStatus.Success, 
                Message = Messages.Category.NotFound(isPlural:false),
                Category = null
            });
        }

        public async Task<IDataResult<int>> CountAsync(){
            var categoriesCount = await _unitOfWork.Categories.CountAsync();

            if(categoriesCount > -1){
                return new DataResult<int>(ResultStatus.Success,Messages.Category.Count(categoriesCount),categoriesCount);
            }

            return new DataResult<int>(ResultStatus.Error,Messages.Category.NotFound(isPlural:true),-1);
        }
        
        public async Task<IDataResult<int>> CountByNonDeletedAsync(){
            var categoriesCount = await _unitOfWork.Categories.CountAsync(c => !c.IsDeleted);

            if(categoriesCount > -1){
                return new DataResult<int>(ResultStatus.Success,Messages.Category.Count(categoriesCount),categoriesCount);
            }

            return new DataResult<int>(ResultStatus.Error,Messages.Category.NotFound(isPlural:true),-1);
        }
    }
}
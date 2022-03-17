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

        public async Task<IDataResult<CategoryDto>> Add(CategoryAddDto categoryAddDto, string createdName)
        {
            var category = _mapper.Map<Category>(categoryAddDto);
            
            category.CreatedName = createdName;
            category.ModifiedName = createdName;

            var addingCategory =  await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveAsync();

            return new DataResult<CategoryDto>(ResultStatus.Success,Messages.Category.Add(category.Name),new CategoryDto(){
                ResultStatus = ResultStatus.Success,
                Category= addingCategory,
                Message = Messages.Category.Add(category.Name)
            });
        }

        public async Task<IDataResult<CategoryDto>> Delete(int categoryId, string modifiedName)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);

            if(category != null){
                category.ModifiedName = modifiedName;
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

        public async Task<IDataResult<CategoryListDto>> GetAll()
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

        public async Task<IDataResult<CategoryListDto>> GetAllNonDeleted()
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

        public async Task<IDataResult<CategoryListDto>> GetAllNonDeletedAndActive(){
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

        public async Task<IDataResult<CategoryDto>> Get(int categoryId)
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

        public async Task<IDataResult<CategoryUpdateDto>> GetUpdateDto(int categoryId){
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            
            if(category != null){
                var categoryUpdateDto = _mapper.Map<CategoryUpdateDto>(category);
                return new DataResult<CategoryUpdateDto>(ResultStatus.Success,Messages.Category.Update(category.Name),categoryUpdateDto);
            }
            return new DataResult<CategoryUpdateDto>(ResultStatus.Error,Messages.Category.NotFound(isPlural:false),null);
        }

        public async Task<IResult> HardDelete(int categoryId)
        {
             var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);

             if(category != null){
                 await _unitOfWork.Categories.DeleteAsync(category);
                 await _unitOfWork.SaveAsync();

                 return new Result(ResultStatus.Success,Messages.Category.HardDelete(category.Name));
             }

             return new Result(ResultStatus.Error,Messages.Category.NotFound(isPlural:false));
        }

        public async Task<IDataResult<CategoryDto>> Update(CategoryUpdateDto categoryUpdateDto, string modifiedName)
        {
            var oldCategory = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryUpdateDto.Id);
            var category = _mapper.Map<CategoryUpdateDto,Category>(categoryUpdateDto,oldCategory);

            if(category != null){
                category.ModifiedName = modifiedName;
                
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
    }
}
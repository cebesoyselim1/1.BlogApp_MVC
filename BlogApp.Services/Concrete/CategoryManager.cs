using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogApp.Data.Abstract;
using BlogApp.Entities.Concrete;
using BlogApp.Entities.Dtos.CategoryDtos;
using BlogApp.Services.Abstract;
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

            return new DataResult<CategoryDto>(ResultStatus.Success,$"{category.Name} has successfully been added.",new CategoryDto(){
                ResultStatus = ResultStatus.Success,
                Category= addingCategory,
                Message = $"{category.Name} has successfully been added."
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

                return new DataResult<CategoryDto>(ResultStatus.Success,$"{category.Name} has successfully been deleted.",new CategoryDto(){
                    ResultStatus = ResultStatus.Success,
                    Message = $"{category.Name} has successfully been deleted.",
                    Category = category
                });
            }
            
            return new DataResult<CategoryDto>(ResultStatus.Error,"Category not found.",new CategoryDto(){
                ResultStatus = ResultStatus.Error,
                Message = "Category not found.",
                Category = null
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAll()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(null, c => c.Articles);

            if(categories.Count() >= 0){
                return new DataResult<CategoryListDto>(ResultStatus.Success,"Categories has successfully been brought.",new CategoryListDto(){
                    ResultStatus = ResultStatus.Success,
                    Message = "Categories has successfully been brought.",
                    Categories = categories
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error,"No categories found.",new CategoryListDto(){
                ResultStatus = ResultStatus.Error, 
                Message = "No categories found.",
                Categories = null
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllNonDeleted()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted, c => c.Articles);

            if(categories.Count() >= 0){
                return new DataResult<CategoryListDto>(ResultStatus.Success,"Categories has successfully been brought.",new CategoryListDto(){
                    ResultStatus = ResultStatus.Success, 
                    Message = "Categories has successfully been brought.",
                    Categories = categories
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error,"No categories found.",new CategoryListDto(){
                ResultStatus = ResultStatus.Error, 
                Message = "No categories found.",
                Categories = null
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllNonDeletedAndActive(){
            var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted && c.IsActive, c => c.Articles);

            if(categories.Count >= 0){
                return new DataResult<CategoryListDto>(ResultStatus.Success,"Categories has successfullt been brought.",new CategoryListDto(){
                    ResultStatus = ResultStatus.Success, 
                    Message = "Categories has successfullt been brought.",
                    Categories = categories
                });
            }

            return new DataResult<CategoryListDto>(ResultStatus.Error,"No categories found.",new CategoryListDto(){
                ResultStatus = ResultStatus.Error, 
                Message = "No categories found.",
                Categories = null
            });
        }

        public async Task<IDataResult<CategoryDto>> Get(int categoryId)
        {  
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);

            if(category != null){
                return new DataResult<CategoryDto>(ResultStatus.Success,$"{category.Name} has successfully been brought.",new CategoryDto(){
                    ResultStatus = ResultStatus.Success, 
                    Message = $"{category.Name} has successfully been brought.",
                    Category = category
                });
            }

            return new DataResult<CategoryDto>(ResultStatus.Error,"Category not found.", new CategoryDto(){
                ResultStatus = ResultStatus.Error, 
                Message = "Category not found.",
                Category = null
            });
        }

        public async Task<IResult> HardDelete(int categoryId)
        {
             var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);

             if(category != null){
                 await _unitOfWork.Categories.DeleteAsync(category);
                 await _unitOfWork.SaveAsync();

                 return new Result(ResultStatus.Success,$"{category.Name} has successfully been deleted from database.");
             }

             return new Result(ResultStatus.Error,"Category not found.");
        }

        public async Task<IDataResult<CategoryDto>> Update(CategoryUpdateDto categoryUpdateDto, string modifiedName)
        {
            var category = _mapper.Map<Category>(categoryUpdateDto);

            if(category != null){
                category.ModifiedName = modifiedName;
                
                var updatingCategory = await _unitOfWork.Categories.UpdateAsync(category);
                await _unitOfWork.SaveAsync();

                return new DataResult<CategoryDto>(ResultStatus.Success,$"{category.Name} has successfully been updated.",new CategoryDto(){
                    ResultStatus = ResultStatus.Success, 
                    Message = $"{category.Name} has successfully been updated.",
                    Category = updatingCategory
                });
            }
            
            return new DataResult<CategoryDto>(ResultStatus.Success,"Category not found.",new CategoryDto(){
                ResultStatus = ResultStatus.Success, 
                Message = "Category not found.",
                Category = null
            });
        }
    }
}
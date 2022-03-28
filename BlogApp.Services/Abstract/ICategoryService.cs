using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Concrete;
using BlogApp.Entities.Dtos.CategoryDtos;
using BlogApp.Shared.Utilities.Results.Abstract;

namespace BlogApp.Services.Abstract
{
    public interface ICategoryService
    {
        Task<IDataResult<CategoryDto>> GetAsync(int categoryId);
        /// <summary>
        /// With given category id it returns CategoryUpdateDto.
        /// </summary>
        /// <param name="categoryId">Id which is integer and greater than 0.</param>
        /// <returns>It returns asynchronous IDataResult.</returns>
        Task<IDataResult<CategoryUpdateDto>> GetUpdateDtoAsync(int categoryId);
        Task<IDataResult<CategoryListDto>> GetAllAsync();
        Task<IDataResult<CategoryListDto>> GetAllByDeletedAsync();
        Task<IDataResult<CategoryListDto>> GetAllNonDeletedAsync();
        Task<IDataResult<CategoryListDto>> GetAllNonDeletedAndActiveAsync();
        Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto c, string createdName);
        Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto c, string modifiedName);
        Task<IDataResult<CategoryDto>> DeleteAsync(int categoryId, string modifiedName);
        Task<IResult> HardDeleteAsync(int categoryId);
        Task<IDataResult<CategoryDto>> UndoDeleteAsync(int categoryId, string modifiedByName);
        public Task<IDataResult<int>> CountAsync();
        public Task<IDataResult<int>> CountByNonDeletedAsync();
    }
}
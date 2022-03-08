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
        Task<IDataResult<CategoryDto>> Get(int categoryId);
        Task<IDataResult<CategoryListDto>> GetAll();
        Task<IDataResult<CategoryListDto>> GetAllNonDeleted();
        Task<IDataResult<CategoryListDto>> GetAllNonDeletedAndActive();
        Task<IDataResult<CategoryDto>> Add(CategoryAddDto c, string createdName);
        Task<IDataResult<CategoryDto>> Update(CategoryUpdateDto c, string modifiedName);
        Task<IDataResult<CategoryDto>> Delete(int categoryId, string modifiedName);
        Task<IResult> HardDelete(int categoryId);
    }
}
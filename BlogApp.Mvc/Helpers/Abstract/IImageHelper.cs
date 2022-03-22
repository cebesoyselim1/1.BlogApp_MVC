using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.ComplexTypes;
using BlogApp.Entities.Dtos.ImageDtos;
using BlogApp.Shared.Utilities.Results.Abstract;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Mvc.Helpers.Abstract
{
    public interface IImageHelper
    {
        Task<IDataResult<ImageUploadDto>> Upload(string name, IFormFile pictureFile, PictureType pictureType, string folderName = null);
        IDataResult<ImageDeleteDto> Delete(string pictureName);
    }
}
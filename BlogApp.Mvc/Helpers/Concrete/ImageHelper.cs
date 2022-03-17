using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Dtos.ImageDtos;
using BlogApp.Mvc.Helpers.Abstract;
using BlogApp.Shared.Utilities.Extensions;
using BlogApp.Shared.Utilities.Results.Abstract;
using BlogApp.Shared.Utilities.Results.ComplexTypes;
using BlogApp.Shared.Utilities.Results.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Mvc.Helpers.Concrete
{
    public class ImageHelper : IImageHelper
    {

        private readonly IWebHostEnvironment _env;
        private readonly string _wwwRoot;
        private readonly string imagePath;

        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
            _wwwRoot = _env.WebRootPath;
            imagePath = "img";
        }

        public IDataResult<ImageDeleteDto> Delete(string pictureName)
        {
            var path = Path.Combine($"{_wwwRoot}/{imagePath}/UserPictures",pictureName);
            if(System.IO.File.Exists(path)){
                var fileInfo = new FileInfo(path);
                var imageDeleteDto = new ImageDeleteDto(){
                    FullName = pictureName,
                    Extension = fileInfo.Extension,
                    Path = path,
                    Size = fileInfo.Length
                };
                System.IO.File.Delete(path);
                return new DataResult<ImageDeleteDto>(ResultStatus.Success,$"{pictureName} has successfully been deleted.",imageDeleteDto);
            }
            return new DataResult<ImageDeleteDto>(ResultStatus.Error,"Path not found.",null);
        }

        public async Task<IDataResult<ImageUploadDto>> Upload(string userName, IFormFile pictureFile, string folderName = "UserImages")
        {
            if(!Directory.Exists($"{_wwwRoot}/{imagePath}/{folderName}")){
                Directory.CreateDirectory($"{_wwwRoot}/{imagePath}/{folderName}");
            }

            var oldFileName = Path.GetFileNameWithoutExtension(pictureFile.FileName);
            var fileExtension = Path.GetExtension(pictureFile.FileName);

            DateTime dateTime = new DateTime();
            var newFileName = $"{userName}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}";

            var path = Path.Combine($"{_wwwRoot}/{imagePath}/{folderName}",newFileName);

            await using(var stream = new FileStream(path,FileMode.Create)){
                await pictureFile.CopyToAsync(stream);
            }

            return new DataResult<ImageUploadDto>(ResultStatus.Success,$"{userName}'s image has successfully been uploaded.",new ImageUploadDto(){
                FullName = $"{folderName}/{newFileName}",
                OldName = oldFileName,
                Extension = fileExtension,
                Path = path,
                FolderName = folderName,
                Size = pictureFile.Length
            });
        }
    }
}
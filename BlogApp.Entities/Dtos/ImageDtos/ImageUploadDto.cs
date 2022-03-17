using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Entities.Dtos.ImageDtos
{
    public class ImageUploadDto
    {
        public string FullName { get; set; }
        public string OldName { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public string FolderName { get; set; }
        public long Size { get; set; }
    }
}
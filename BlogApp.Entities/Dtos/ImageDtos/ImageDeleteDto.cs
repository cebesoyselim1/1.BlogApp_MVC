using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Entities.Dtos.ImageDtos
{
    public class ImageDeleteDto
    {
        public string FullName { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
    }
}
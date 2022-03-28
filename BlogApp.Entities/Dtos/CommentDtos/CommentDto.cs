using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Entities.Concrete;
using BlogApp.Shared.Entities.Abstract;

namespace BlogApp.Entities.Dtos.CommentDtos
{
    public class CommentDto:DtoGetBase
    {
        public Comment Comment { get; set; }
    }
}

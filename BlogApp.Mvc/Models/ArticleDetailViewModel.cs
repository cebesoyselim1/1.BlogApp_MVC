using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Dtos.ArticleDtos;

namespace BlogApp.Mvc.Models
{
    public class ArticleDetailViewModel
    {
        public ArticleDto ArticleDto { get; set; }
        public ArticleDetailRightSideBarViewModel ArticleDetailRightSideBarViewModel { get; set; }
    }
}
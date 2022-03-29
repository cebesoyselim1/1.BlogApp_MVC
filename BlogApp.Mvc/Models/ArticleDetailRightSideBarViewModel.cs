using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Entities.Concrete;
using BlogApp.Entities.Dtos.ArticleDtos;

namespace BlogApp.Mvc.Models
{
    public class ArticleDetailRightSideBarViewModel
    {
        public string Header { get; set; }
        public ArticleListDto ArticleListDto { get; set; }
        public User User { get; set; }
        
    }
}
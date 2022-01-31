using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogApp.Entities.Concrete;
using BlogApp.Entities.Dtos.ArticleDtos;

namespace BlogApp.Services.AutoMapper.Profiles
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap<ArticleAddDto,Article>().ForMember(dest => dest.CreatedName,opt => opt.MapFrom(c => DateTime.Now));

            CreateMap<ArticleUpdateDto,Article>().ForMember(dest => dest.ModifiedName,opt => opt.MapFrom(c => DateTime.Now));
        }
    }
}
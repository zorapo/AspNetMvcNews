using App.Entities.Concrete;
using App.Entities.Dtos.NewsCommentDtos;
using AutoMapper;

namespace App.Service.AutoMapper.Profiles
{
    public class NewsCommentProfile:Profile
    {
        public NewsCommentProfile()
        {
            CreateMap<NewsCommentAddDto, NewsComment>().ReverseMap();
            CreateMap<NewsCommentDto, NewsComment>().ReverseMap();
            CreateMap<NewsCommentUpdateDto, NewsComment>().ReverseMap(); 
        }
    }
}

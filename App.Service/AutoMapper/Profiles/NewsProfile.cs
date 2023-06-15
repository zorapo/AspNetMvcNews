using App.Entities.Concrete;
using App.Entities.Dtos.NewsDtos;
using AutoMapper;

namespace App.Service.AutoMapper.Profiles
{
    public class NewsProfile:Profile
    {
        public NewsProfile()
        {           
        CreateMap<NewsAddDto, News>().ReverseMap();
        CreateMap<NewsDto, News>().ReverseMap();
        CreateMap<NewsUpdateDto, News>().ReverseMap();


        }
    }
}

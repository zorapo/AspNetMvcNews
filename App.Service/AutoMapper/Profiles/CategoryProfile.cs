using App.Entities.Concrete;
using App.Entities.Dtos.CategoryDtos;
using AutoMapper;

namespace App.Service.AutoMapper.Profiles
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryAddDto, Category>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<CategoryUpdateDto, Category>().ReverseMap();
        
        }
    }
}

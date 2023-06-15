using App.Entities.Concrete;
using App.Entities.Dtos.UserDtos;
using AutoMapper;

namespace App.Web.Mvc.AutoMapper.Profiles
{
    public class UserProfile:Profile
	{
        public UserProfile()
        {
            CreateMap<UserAddDto, User>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<UserRegisterDto, User>().ReverseMap();

        }
    }
}

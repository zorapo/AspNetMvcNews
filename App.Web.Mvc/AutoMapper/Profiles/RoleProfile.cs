using App.Entities.Concrete;
using App.Entities.Dtos.RoleDtos;
using AutoMapper;

namespace App.Web.Mvc.AutoMapper.Profiles
{
    public class RoleProfile:Profile
	{
        public RoleProfile()
        {           
        CreateMap<RoleAddDto, Role>().ReverseMap();
		CreateMap<RoleDto, Role>().ReverseMap();
		CreateMap<RoleUpdateDto, Role>().ReverseMap();
        }
	}
}

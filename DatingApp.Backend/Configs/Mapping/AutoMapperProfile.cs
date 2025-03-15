using AutoMapper;
using DatingApp.Backend.Core.Entities;
using DatingApp.Backend.Dtos.User;
using DatingApp.Backend.Models.User;

namespace DatingApp.Backend.Configs.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region AppUsers
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<RegisterUserInput, CreateUserInput>();
            #endregion
        }
    }
}

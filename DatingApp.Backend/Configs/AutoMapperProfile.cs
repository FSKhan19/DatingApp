using AutoMapper;
using DatingApp.Backend.Core.Entities;
using DatingApp.Backend.Dtos.User;
using DatingApp.Backend.Models.User;

namespace DatingApp.Backend.Configs
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region AppUsers
            CreateMap<CreateUserInput, AppUser>();
            CreateMap<AppUser, GetUserDto>().ReverseMap();
            #endregion
        }
    }
}

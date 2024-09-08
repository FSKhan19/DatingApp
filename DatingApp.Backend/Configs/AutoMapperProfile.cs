using AutoMapper;
using DatingApp.Backend.Core.Entities;
using DatingApp.Backend.Models.User;

namespace DatingApp.Backend.Configs
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region AppUsers
            CreateMap<CreateUser, AppUser>();
            CreateMap<AppUser, GetUser>().ReverseMap();
            #endregion
        }
    }
}

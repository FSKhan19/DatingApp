using DatingApp.Backend.Core.Entities;
using DatingApp.Backend.Dtos.User;
using DatingApp.Backend.Models.User;
using Mapster;

namespace DatingApp.Backend.Configs.Mapping
{
    public class UserMapping: IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<AppUser, UserDto>().TwoWays();
            config.NewConfig<RegisterUserInput, CreateUserInput>();
        }
    }
}

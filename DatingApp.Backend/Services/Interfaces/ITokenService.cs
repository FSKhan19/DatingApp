using DatingApp.Backend.Core.Entities;
using DatingApp.Backend.Dtos.User;

namespace DatingApp.Backend.Services.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(UserCredentialsDto appUser);
    }
}

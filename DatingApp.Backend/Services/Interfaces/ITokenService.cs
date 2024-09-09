using DatingApp.Backend.Core.Entities;

namespace DatingApp.Backend.Services.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(AppUser appUser);
    }
}

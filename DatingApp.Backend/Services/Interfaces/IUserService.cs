using DatingApp.Backend.Dtos.Common;
using DatingApp.Backend.Dtos.User;
using DatingApp.Backend.Models.User;

namespace DatingApp.Backend.Services.Interfaces
{
    public interface IUserService: IScopedService
    {
        Task<(List<UserDto> Users, int TotalCount)> GetAsync(int pageNumber = 1, int pageSize = 10);
        Task<Result<UserDto>> GetAsync(int id);
        Task<Result<UserCredentialsDto>> CreateAsync(CreateUserInput input);
        Task<Result<UserDto>> UpdateAsync(UpdateUserInput input);
        Task<Result<bool>> DeleteAsync(int id);
        Task<Result<UserCredentialsDto>> ValidateUsernameAsync(string userName);
    }
}

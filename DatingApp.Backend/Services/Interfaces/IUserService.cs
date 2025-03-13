using DatingApp.Backend.Dtos.Common;
using DatingApp.Backend.Dtos.User;
using DatingApp.Backend.Models.User;

namespace DatingApp.Backend.Services.Interfaces
{
    public interface IUserService: IScopedService
    {
        Task<(List<GetUserDto> Users, int TotalCount)> GetAsync(int pageNumber = 1, int pageSize = 10);
        Task<Result<GetUserDto>> GetAsync(int id);
        Task<Result<GetUserDto>> CreateAsync(CreateUserInput input);
        Task<Result<GetUserDto>> UpdateAsync(UpdateUserInput input);
        Task<Result<bool>> DeleteAsync(int id);
    }
}

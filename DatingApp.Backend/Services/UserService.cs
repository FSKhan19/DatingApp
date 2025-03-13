using AutoMapper;
using DatingApp.Backend.Core;
using DatingApp.Backend.Core.Entities;
using DatingApp.Backend.Models.User;
using DatingApp.Backend.Services.Interfaces;
using DatingApp.Backend.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using DatingApp.Backend.Dtos.Common;
using DatingApp.Backend.Consts;
using DatingApp.Backend.Dtos.User;
using System.Drawing.Printing;

namespace DatingApp.Backend.Services
{
    public class UserService: IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<AppUser> _repositoryUser;

        public UserService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repositoryUser = _unitOfWork.GetRepository<AppUser>();
        }

        #region Helper Methods
        private async Task<Result<AppUser>> GetAppUserAsync(int id)
        {
            var user = await _repositoryUser.FirstOrDefaultAsync(m => m.Id == id && !m.IsDeleted);

            return user == null ?
                Result<AppUser>.Failure(Error.Record.RECORD_NOT_FOUND) :
                Result<AppUser>.Success(user);
        }

        private async Task<bool> UserNameExistsAsync(string userName)
        {
            return await _repositoryUser.AnyAsync(m => m.UserName == userName && !m.IsDeleted);
        }
        #endregion

        public async Task<(List<GetUserDto> Users, int TotalCount)> GetAsync(int pageNumber = 1, int pageSize = 10)
        {
            var query = _repositoryUser.GetAllReadonly().Where(x => !x.IsDeleted);

            var totalCount = await query.CountAsync();
            var appUsers = await query
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (_mapper.Map<List<GetUserDto>>(appUsers), totalCount);
        }

        public async Task<Result<GetUserDto>> GetAsync(int id)
        {
            var user = await GetAppUserAsync(id);

            return user.Value == null ?
                Result<GetUserDto>.Failure(user.Error) :
                Result<GetUserDto>.Success(_mapper.Map<GetUserDto>(user.Value));
        }

        public async Task<Result<GetUserDto>> CreateAsync(CreateUserInput input)
        {
            var isExists = await UserNameExistsAsync(input.UserName);
            if (isExists)
                return Result<GetUserDto>.Failure(Error.Record.USERNAME_ALREADY_TAKEN);

            var user = _mapper.Map<AppUser>(input);

            await _unitOfWork.BeginTransactionAsync();
            var appUser = await _repositoryUser.InsertAsync(user);
            await _unitOfWork.CommitAsync();

            return Result<GetUserDto>.Success(_mapper.Map<GetUserDto>(appUser));
        }

        public async Task<Result<GetUserDto>> UpdateAsync(UpdateUserInput input)
        {
            var isExists = await UserNameExistsAsync(input.UserName);
            if (isExists)
                return Result<GetUserDto>.Failure(Error.Record.USERNAME_ALREADY_TAKEN);

            var res = await GetAppUserAsync(input.Id);

            if (!res.IsSuccess)
                return Result<GetUserDto>.Failure(res.Error);

            var appUser = res.Value;

            await _unitOfWork.BeginTransactionAsync();
            appUser.UserName = input.UserName;
            appUser.LastModificationTime = DateTime.Now;
            var updatedUser = await _repositoryUser.UpdateAsync(appUser);
            await _unitOfWork.CommitAsync();

            return Result<GetUserDto>.Success(_mapper.Map<GetUserDto>(appUser));
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var res = await GetAppUserAsync(id);
            if (!res.IsSuccess)
                return Result<bool>.Failure(res.Error);

            var appUser = res.Value;
            appUser.IsDeleted = true;

            await _unitOfWork.BeginTransactionAsync();
            await _repositoryUser.UpdateAsync(appUser);
            await _unitOfWork.CommitAsync();

            return Result<bool>.Success(true);
        }
    }
}

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
using System.Security.Cryptography;
using System.Text;

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
            var user = await _repositoryUser.FirstOrDefaultAsync(m => m.Id == id);

            return user == null ?
                Result<AppUser>.Failure(Error.Record.RECORD_NOT_FOUND) :
                Result<AppUser>.Success(user);
        }

        private async Task<bool> IsUserExists(string userName)
        {
            return await _repositoryUser.GetAllReadonly().AnyAsync(m => m.UserName.Trim().ToLower() == userName.Trim().ToLower());
        }
        #endregion

        public async Task<(List<UserDto> Users, int TotalCount)> GetAsync(int pageNumber = 1, int pageSize = 10)
        {
            var query = _repositoryUser.GetAllReadonly();

            var totalCount = await query.CountAsync();
            var appUsers = await query
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (_mapper.Map<List<UserDto>>(appUsers), totalCount);
        }

        public async Task<Result<UserDto>> GetAsync(int id)
        {
            var user = await _repositoryUser.GetAllReadonly().FirstOrDefaultAsync(m => m.Id == id);

            return user == null ?
                Result<UserDto>.Failure(Error.Record.RECORD_NOT_FOUND) :
                Result<UserDto>.Success(_mapper.Map<UserDto>(user));
        }
        public async Task<Result<UserCredentialsDto>> CreateAsync(CreateUserInput input)
        {
            var isExists = await IsUserExists(input.UserName);
            if (isExists)
                return Result<UserCredentialsDto>.Failure(Error.Record.USERNAME_ALREADY_TAKEN);

            using HMACSHA512 hmac = new HMACSHA512();
            var appUserObj = new AppUser()
            {
                UserName = input.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input.Password)),
                PasswordSalt = hmac.Key
            };

            await _unitOfWork.BeginTransactionAsync();
            var appUser = await _repositoryUser.InsertAsync(appUserObj);
            await _unitOfWork.CommitAsync();

            return Result<UserCredentialsDto>.Success(_mapper.Map<UserCredentialsDto>(appUserObj));
        }

        public async Task<Result<UserDto>> UpdateAsync(UpdateUserInput input)
        {
            var isExists = await IsUserExists(input.UserName);
            if (isExists)
                return Result<UserDto>.Failure(Error.Record.USERNAME_ALREADY_TAKEN);

            var res = await GetAppUserAsync(input.Id);

            if (!res.IsSuccess)
                return Result<UserDto>.Failure(res.Error);

            var appUser = res.Value;

            await _unitOfWork.BeginTransactionAsync();
            appUser.UserName = input.UserName;
            appUser.LastModificationTime = DateTime.Now;
            var updatedUser = await _repositoryUser.UpdateAsync(appUser);
            await _unitOfWork.CommitAsync();

            return Result<UserDto>.Success(_mapper.Map<UserDto>(appUser));
        }

        public async Task<Result<bool>> DeleteAsync(int id)
        {
            var appUser = await _repositoryUser
                .GetAllIncluding(x=>x.Photos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (appUser == null)
                return Result<bool>.Failure(Error.Record.RECORD_NOT_FOUND);

            appUser.IsDeleted = true;

            // Soft-delete all associated photos first
            foreach (var photo in appUser.Photos)
            {
                photo.IsDeleted = true;
            }

            await _unitOfWork.BeginTransactionAsync();
            await _repositoryUser.UpdateAsync(appUser);
            await _unitOfWork.CommitAsync();

            return Result<bool>.Success(true);
        }

        public async Task<Result<UserCredentialsDto>> ValidateUsernameAsync(string userName)
        {
            var user = await _repositoryUser
                .GetAllReadonly()
                .FirstOrDefaultAsync(x => x.UserName == userName);

            return user == null ?
                Result<UserCredentialsDto>.Failure(Error.Record.INVALID_USERNAME) :
                Result<UserCredentialsDto>.Success(new UserCredentialsDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    PasswordHash = user.PasswordHash,
                    PasswordSalt = user.PasswordSalt
                });
        }
    }
}

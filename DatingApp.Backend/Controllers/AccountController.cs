using AutoMapper;
using DatingApp.Backend.Consts;
using DatingApp.Backend.Core;
using DatingApp.Backend.Core.Entities;
using DatingApp.Backend.Core.Repositories;
using DatingApp.Backend.Data;
using DatingApp.Backend.Models.User;
using DatingApp.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

namespace DatingApp.Backend.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        public AccountController(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<RegisterUserResponse>> Register(RegisterUserRequest user)
        {
            if (await IsUserExists(user.UserName))
                return BadRequest(Error.Record.USERNAME_ALREADY_TAKEN);

            using HMACSHA512 hmac = new HMACSHA512();
            var appUser = new AppUser()
            {
                UserName = user.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password)),
                PasswordSalt = hmac.Key
            };
            await _unitOfWork.CreateTransactionAsync();
            var repoUser = _unitOfWork.GetRepository<AppUser>();
            await repoUser.InsertAsync(appUser);
            await _unitOfWork.CommitAsync();
            return new RegisterUserResponse
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(appUser)
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginUserResponse>> Login(LoginUserRequest user)
        {
            var repoUser = _unitOfWork.GetRepository<AppUser>();
            var appUser = await repoUser.SingleAsync(x=>x.UserName == user.UserName);

            if (appUser == null)
                return Unauthorized(Error.Record.INVALID_USERNAME);

            using HMACSHA512 hmac = new HMACSHA512(appUser.PasswordSalt);
            var computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));

            for (int i = 0; i < computedhash.Length; i++)
            {
                if (appUser.PasswordHash[i] != computedhash[i])
                    return Unauthorized(Error.Record.INVALID_PASSWORD);
            }

            return new LoginUserResponse
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(appUser)
            };
        }

        private async Task<bool> IsUserExists(string userName)
        {
            var repoUser = _unitOfWork.GetRepository<AppUser>();
            return await repoUser.AnyAsync(x=>x.UserName.ToLower() == userName.ToLower());
        }
    }
}

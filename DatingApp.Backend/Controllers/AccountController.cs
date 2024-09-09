using AutoMapper;
using DatingApp.Backend.Consts;
using DatingApp.Backend.Core.Entities;
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
        private readonly DatingAppContext _context;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        public AccountController(DatingAppContext context, IMapper mapper, ITokenService tokenService)
        {
            _context = context;
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
            await _context.Users.AddAsync(appUser);
            await _context.SaveChangesAsync();
            return new RegisterUserResponse
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(appUser)
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginUserResponse>> Login(LoginUserRequest user)
        {
            var appUser = await _context.Users.SingleOrDefaultAsync(x=>x.UserName == user.UserName);

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
            return await _context.Users.AnyAsync(x=>x.UserName.ToLower() == userName.ToLower());
        }
    }
}
